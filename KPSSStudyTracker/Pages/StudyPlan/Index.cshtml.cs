using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using System.Text.Json;

namespace KPSSStudyTracker.Pages.StudyPlan
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public Dictionary<int, WeekData> StudyPlanByWeek { get; set; } = new();
        public List<string> AvailableLessons { get; set; } = new();
        public List<string> AvailableSubjects { get; set; } = new();
        public string SelectedLesson { get; set; } = string.Empty;
        public string SelectedSubject { get; set; } = string.Empty;
        public string SelectedCompletion { get; set; } = string.Empty;
        public double OverallProgress { get; set; }
        public int CompletedCount { get; set; }
        public int TotalCount { get; set; }
        public List<Lesson> Lessons { get; set; } = new();

        public class WeekData
        {
            public Dictionary<int, DayData> Days { get; set; } = new();
            public double Progress { get; set; }
        }

        public class DayData
        {
            public List<StudyPlanItem> Items { get; set; } = new();
            public bool IsCompleted { get; set; }
        }

        public class StudyPlanItem
        {
            public int Id { get; set; }
            public string LessonName { get; set; } = string.Empty;
            public string TopicTitle { get; set; } = string.Empty;
            public bool IsCompleted { get; set; }
            public int SolvedQuestions { get; set; }
            public int CorrectAnswers { get; set; }
            public int WrongAnswers { get; set; }
            public string? Source { get; set; }
            public string? Notes { get; set; }
        }

        public async Task OnGetAsync(string? lessonFilter, string? subjectFilter, string? completionFilter)
        {
            // StudyPlan kaldırıldı: Todo ekranına yönlendir
            Response.Redirect("/Todo");
            return;

            // Load dynamic plans with related data
            var userId = GetCurrentUserIdRequired();
            var weeklyPlans = await _context.WeeklyPlans
                .Where(w => w.UserId == userId)
                .Include(w => w.DailyPlans)
                    .ThenInclude(d => d.PlanTopics)
                        .ThenInclude(pt => pt.Topic)
                .Include(w => w.DailyPlans)
                    .ThenInclude(d => d.Lesson)
                .OrderBy(w => w.WeekNumber)
                .ToListAsync();

            // Calculate totals and build view model from dynamic data
            int total = 0;
            int completed = 0;

            foreach (var w in weeklyPlans)
            {
                var weekData = new WeekData();

                // Ensure 3 days slots even if not present
                for (int day = 1; day <= 3; day++)
                {
                    var dayPlan = w.DailyPlans.FirstOrDefault(d => d.DayNumber == day);
                    var dayData = new DayData();

                    if (dayPlan != null)
                    {
                        var lessonName = dayPlan.Lesson?.Name ?? "";
                        foreach (var pt in dayPlan.PlanTopics.OrderBy(pt => pt.Id))
                        {
                            total++;
                            if (pt.IsCompleted) completed++;
                            dayData.Items.Add(new StudyPlanItem
                            {
                                Id = pt.Id, // PlanTopicId for toggling
                                LessonName = lessonName,
                                TopicTitle = pt.Topic?.Title ?? "",
                                IsCompleted = pt.IsCompleted
                            });
                        }
                        dayData.IsCompleted = dayPlan.PlanTopics.All(x => x.IsCompleted) && dayPlan.PlanTopics.Any();
                    }

                    weekData.Days[day] = dayData;
                }

                // Week progress from PlanTopics
                var wTotal = w.DailyPlans.SelectMany(d => d.PlanTopics).Count();
                var wCompleted = w.DailyPlans.SelectMany(d => d.PlanTopics).Count(pt => pt.IsCompleted);
                weekData.Progress = wTotal > 0 ? Math.Round((double)wCompleted / wTotal * 100, 1) : 0;

                StudyPlanByWeek[w.WeekNumber] = weekData;
            }

            TotalCount = total;
            CompletedCount = completed;
            OverallProgress = TotalCount > 0 ? Math.Round((double)CompletedCount / TotalCount * 100, 1) : 0;

            // Filters: keep simple for now
            AvailableLessons = await _context.Lessons.Select(l => l.Name).Distinct().ToListAsync();
            AvailableSubjects = new List<string> { "Tümü", "Tarih", "Türkçe", "Coğrafya", "Matematik" };
        }

        public async Task<IActionResult> OnPostToggleCompletionAsync(int id, bool isCompleted)
        {
            try
            {
                var planTopic = await _context.PlanTopics.FindAsync(id);
                if (planTopic != null)
                {
                    planTopic.IsCompleted = isCompleted;
                    planTopic.CompletedAt = isCompleted ? DateTime.UtcNow : null;
                    await _context.SaveChangesAsync();
                    return new JsonResult(new { success = true });
                }
                return new JsonResult(new { success = false, error = "Plan konusu bulunamadı" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }

        public async Task<IActionResult> OnPostClearAllAsync()
        {
            try
            {
                var userId = GetCurrentUserIdRequired();
                
                // Remove dynamic plans with bulk operations (only user's plans)
                var userWeeklyPlans = await _context.WeeklyPlans
                    .Where(w => w.UserId == userId)
                    .Select(w => w.Id)
                    .ToListAsync();
                
                await _context.PlanTopics
                    .Where(pt => userWeeklyPlans.Contains(pt.WeeklyPlanId))
                    .ExecuteDeleteAsync();
                
                await _context.DailyPlans
                    .Where(d => userWeeklyPlans.Contains(d.WeeklyPlanId))
                    .ExecuteDeleteAsync();
                
                await _context.WeeklyPlans
                    .Where(w => w.UserId == userId)
                    .ExecuteDeleteAsync();

                // Remove old static study plans (only user's)
                await _context.StudyPlans
                    .Where(sp => sp.UserId == userId)
                    .ExecuteDeleteAsync();

                // Optional: reset user topic progress
                await _context.UserTopicProgresses
                    .Where(utp => utp.UserId == userId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(utp => utp.Completed, utp => false)
                        .SetProperty(utp => utp.SolvedQuestions, utp => 0)
                        .SetProperty(utp => utp.CorrectAnswers, utp => 0)
                        .SetProperty(utp => utp.WrongAnswers, utp => 0)
                    );

                TempData["SuccessMessage"] = "Mevcut tüm çalışma programları silindi.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Silme işlemi sırasında hata oluştu: {ex.Message}";
            }

            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnPostRegenerateAsync(int startWeek = 1, int topicsPerDay = 5)
        {
            try
            {
                // Clear everything first
                await OnPostClearAllAsync();

                // Reload lessons with topics (topics are now global)
                var userId = GetCurrentUserIdRequired();
                var lessons = await _context.Lessons
                    .Include(l => l.Topics)
                    .Where(l => l.Topics.Any())
                    .OrderBy(l => l.Name)
                    .ToListAsync();

                if (!lessons.Any())
                {
                    TempData["ErrorMessage"] = "Ders/Konu bulunamadı. Önce veritabanını doldurun.";
                    return RedirectToPage("Index");
                }

                // Create queues with all topics (topics are global)
                var lessonIdToQueue = lessons.ToDictionary(
                    l => l.Id,
                    l => new Queue<Topic>(l.Topics.OrderBy(t => t.Id).ToList())
                );

                int currentWeek = startWeek;
                var lessonIds = lessons.Select(l => l.Id).ToList();
                int lessonIndex = 0;

                bool AnyTopicsRemaining() => lessonIdToQueue.Values.Any(q => q.Count > 0);

                while (AnyTopicsRemaining())
                {
                    var weeklyPlan = new WeeklyPlan
                    {
                        UserId = userId,
                        WeekNumber = currentWeek,
                        PlanTitle = $"Hafta {currentWeek} Çalışma Planı",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.WeeklyPlans.Add(weeklyPlan);
                    await _context.SaveChangesAsync();

                    for (int day = 1; day <= 3 && AnyTopicsRemaining(); day++)
                    {
                        int rotations = 0;
                        while (lessonIdToQueue[lessonIds[lessonIndex]].Count == 0 && rotations < lessonIds.Count)
                        {
                            lessonIndex = (lessonIndex + 1) % lessonIds.Count;
                            rotations++;
                        }

                        if (lessonIdToQueue[lessonIds[lessonIndex]].Count == 0)
                        {
                            break;
                        }

                        var chosenLessonId = lessonIds[lessonIndex];
                        var queue = lessonIdToQueue[chosenLessonId];

                        var picked = new List<Topic>();
                        for (int i = 0; i < topicsPerDay && queue.Count > 0; i++)
                        {
                            picked.Add(queue.Dequeue());
                        }

                        var dailyPlan = new DailyPlan
                        {
                            WeeklyPlanId = weeklyPlan.Id,
                            DayNumber = day,
                            LessonId = chosenLessonId,
                            TopicCount = picked.Count,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.DailyPlans.Add(dailyPlan);
                        await _context.SaveChangesAsync();

                        foreach (var topic in picked)
                        {
                            _context.PlanTopics.Add(new PlanTopic
                            {
                                WeeklyPlanId = weeklyPlan.Id,
                                DailyPlanId = dailyPlan.Id,
                                TopicId = topic.Id,
                                IsCompleted = false,
                                CreatedAt = DateTime.UtcNow
                            });
                        }

                        lessonIndex = (lessonIndex + 1) % lessonIds.Count;
                    }

                    await _context.SaveChangesAsync();
                    currentWeek++;
                }

                TempData["SuccessMessage"] = "Planlar baştan oluşturuldu.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Plan oluşturma sırasında hata: {ex.Message}";
            }

            return RedirectToPage("Index");
        }
    }
}
