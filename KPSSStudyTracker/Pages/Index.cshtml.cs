using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Data;
using System.Text.Json;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages;

public class IndexModel : BasePageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public IndexModel(ILogger<IndexModel> logger, AppDbContext context, IConfiguration config)
    {
        _logger = logger;
        _context = context;
        _config = config;
    }

    public DateTime ExamDate { get; set; }
    public DateTime ApplicationStartDate { get; set; }
    public DateTime ApplicationEndDate { get; set; }
    public string DailyQuote { get; set; } = string.Empty;
    public int TotalTopicsCompleted { get; set; }
    public int TotalQuestions { get; set; }
    public string RecentExamsJson { get; set; } = "[]";
    public string RecentExamsNetJson { get; set; } = "[]";
    public List<Models.DailyTodo> TodayTodos { get; set; } = new();
    public List<LessonProgressItem> LessonProgress { get; set; } = new();

    public class LessonProgressItem
    {
        public string LessonName { get; set; } = string.Empty;
        public int TotalTopics { get; set; }
        public int CompletedTopics { get; set; }
        public int WeeklyCompleted { get; set; }
        public double OverallPercent => TotalTopics > 0 ? Math.Round((double)CompletedTopics / TotalTopics * 100, 1) : 0;
    }

    public async Task OnGetAsync()
    {
        try
        {
            ExamDate = DateTime.Parse(_config["KPSS:ExamDate"] ?? "2026-09-06");
            ApplicationStartDate = DateTime.Parse(_config["KPSS:ApplicationStartDate"] ?? "2026-07-01");
            ApplicationEndDate = DateTime.Parse(_config["KPSS:ApplicationEndDate"] ?? "2026-07-13");
        }
        catch
        {
            ExamDate = new DateTime(2026, 9, 6);
            ApplicationStartDate = new DateTime(2026, 7, 1);
            ApplicationEndDate = new DateTime(2026, 7, 13);
        }
        
        // Türkçe motivasyon mesajları - KPSS odaklı
        var turkishQuotes = new List<string>
        {
            "🎯 KPSS yolculuğunda her adım seni hedefine yaklaştırıyor!",
            "📚 Bugün çözdüğün her soru, yarının başarısının temelini atıyor.",
            "💪 Zorluklar seni güçlendirir, pes etme ve devam et!",
            "🌟 Hayallerin gerçek olması için çalışmaya devam et.",
            "⚡ Her gün bir adım daha yaklaşıyorsun KPSS başarısına!",
            "🔥 Sabır ve azimle her şey mümkündür, sen bunu başarabilirsin!",
            "🎓 Başarı, küçük çabaların tekrarıdır - her gün çalış!",
            "💎 Kendine inan, çünkü sen bunu başarabilirsin!",
            "🚀 Hedefin için çalışmak, en güzel yatırımdır.",
            "⭐ Her soru çözdüğünde bir adım daha ilerliyorsun.",
            "🎪 Başarısızlık, başarıya giden yolda bir duraktır.",
            "🌈 Gelecek, hayallerinin güzelliğine inananlara aittir.",
            "📖 Her gün yeni bir şey öğren, her gün geliş.",
            "🏆 Başarı, hazırlık ve fırsatın buluşmasıdır.",
            "💡 Yapabileceğine inananlar, yapamayanlardan daha iyidir.",
            "🎨 Çalışmak, başarının anahtarıdır - anahtarı çevir!",
            "🌟 Bugün yaptığın çalışma, yarının başarısıdır.",
            "🎯 KPSS için çalışmak, geleceğin için en iyi yatırım!",
            "💪 Zorluklar seni güçlendirir, her zorluk bir fırsattır.",
            "🚀 Hayallerin gerçek olması için çalışmaya devam et!",
            "📚 Her konu öğrendiğinde, KPSS'e bir adım daha yaklaşıyorsun.",
            "🎓 Başarı, hazırlık ve kararlılığın birleşimidir.",
            "⭐ Kendine inan, çünkü sen bunu başarabilirsin!",
            "🌈 Her gün bir fırsat, her soru bir adım ileri!",
            "🔥 KPSS yolculuğunda sabırlı ol, başarı seni bekliyor!"
        };
        
        DailyQuote = await _context.MotivationQuotes.OrderBy(q => Guid.NewGuid()).Select(q => q.Text).FirstOrDefaultAsync() 
                    ?? turkishQuotes[new Random().Next(turkishQuotes.Count)];

        var userId = GetCurrentUserId();
        if (!userId.HasValue)
        {
            // User not logged in, skip data loading
            return;
        }

        TotalTopicsCompleted = await _context.UserTopicProgresses.CountAsync(utp => utp.UserId == userId && utp.Completed);
        TotalQuestions = await _context.UserTopicProgresses.Where(utp => utp.UserId == userId).SumAsync(utp => utp.SolvedQuestions);

        var today = DateTime.UtcNow.Date;
        TodayTodos = await _context.DailyTodos
            .Where(t => t.UserId == userId && t.Date.Date == today)
            .OrderBy(t => t.IsCompleted)
            .ThenBy(t => t.Id)
            .ToListAsync();

        // Per-lesson progress (overall) + weekly completed using PlanTopics timestamps
        var lessons = await _context.Lessons.Include(l => l.Topics).ToListAsync();
        var lessonTopicIds = lessons.ToDictionary(l => l.Id, l => l.Topics.Select(t => t.Id).ToList());
        
        // Get user progress for all topics
        var allTopicIds = lessons.SelectMany(l => l.Topics.Select(t => t.Id)).ToList();
        var userProgress = await _context.UserTopicProgresses
            .Include(utp => utp.Topic)
            .Where(utp => utp.UserId == userId && allTopicIds.Contains(utp.TopicId))
            .ToListAsync();
        
        var progressByLessonId = userProgress
            .Where(utp => utp.Topic != null)
            .GroupBy(utp => utp.Topic!.LessonId)
            .ToDictionary(g => g.Key, g => g.ToList());
        
        var weekStart = DateTime.UtcNow.Date.AddDays(-7);
        var userWeeklyPlans = await _context.WeeklyPlans
            .Where(w => w.UserId == userId)
            .Select(w => w.Id)
            .ToListAsync();
        var weeklyCompletedByLesson = _context.PlanTopics
            .Include(pt => pt.Topic)
            .Where(pt => userWeeklyPlans.Contains(pt.WeeklyPlanId) && pt.IsCompleted && pt.CompletedAt != null && pt.CompletedAt >= weekStart)
            .AsEnumerable()
            .GroupBy(pt => pt.Topic!.LessonId)
            .ToDictionary(g => g.Key, g => g.Count());

        LessonProgress = lessons
            .Select(l => 
            {
                var lessonProgress = progressByLessonId.GetValueOrDefault(l.Id, new List<Models.UserTopicProgress>());
                return new LessonProgressItem
                {
                    LessonName = l.Name,
                    TotalTopics = l.Topics.Count,
                    CompletedTopics = lessonProgress.Count(p => p.Completed),
                    WeeklyCompleted = weeklyCompletedByLesson.ContainsKey(l.Id) ? weeklyCompletedByLesson[l.Id] : 0
                };
            })
            .Where(lp => lp.TotalTopics > 0)
            .OrderByDescending(lp => lp.OverallPercent)
            .ToList();

        var recent = _context.MockExams
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date)
            .Take(8)
            .Select(e => new { e.Name, Net = e.Results.Sum(r => (double)r.Correct - r.Wrong / 4.0) })
            .ToList();
        RecentExamsJson = JsonSerializer.Serialize(recent.Select(r => r.Name).Reverse().ToList());
        RecentExamsNetJson = JsonSerializer.Serialize(recent.Select(r => Math.Round(r.Net, 2)).Reverse().ToList());
    }

    public IActionResult OnPostNewQuote()
    {
        return RedirectToPage();
    }
}
