using System.ComponentModel.DataAnnotations;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Pages.StudyPlan
{
    public class CreateModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public List<Lesson> AvailableLessons { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = GetCurrentUserIdRequired();
            var lessons = await _context.Lessons
                .Include(l => l.Topics)
                .ToListAsync();
            
            // Show all lessons with topics (topics are now global)
            AvailableLessons = lessons
                .Where(l => l.Topics.Any())
                .Select(l => new Lesson
                {
                    Id = l.Id,
                    Name = l.Name,
                    Topics = l.Topics.ToList()
                })
                .OrderBy(l => l.Name)
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Allow empty PlanTitle; set a default later
            ModelState.Remove("Input.PlanTitle");
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                // Load lessons with topics (topics are now global)
                var userId = GetCurrentUserIdRequired();
                var lessons = await _context.Lessons
                    .Include(l => l.Topics)
                    .Where(l => l.Topics.Any())
                    .OrderBy(l => l.Name)
                    .ToListAsync();

                if (!lessons.Any())
                {
                    ModelState.AddModelError("", "Veritabanında ders ve konu bulunamadı.");
                    await OnGetAsync();
                    return Page();
                }

                // Prepare per-lesson topic queues (ordered)
                var lessonIdToQueue = lessons.ToDictionary(
                    l => l.Id,
                    l => new Queue<Topic>(l.Topics
                        .OrderBy(t => t.Id)
                        .ToList())
                );

                int startWeek = Input.WeekNumber;
                int currentWeek = startWeek;
                int topicsPerDayTarget = 5; // default chunk size per day

                // Round-robin lesson iterator
                var lessonIds = lessons.Select(l => l.Id).ToList();
                int lessonIndex = 0;

                bool AnyTopicsRemaining() => lessonIdToQueue.Values.Any(q => q.Count > 0);

                // Continue creating weeks (3 days each) until topics are exhausted
                while (AnyTopicsRemaining())
                {
                    var weeklyPlan = new WeeklyPlan
                    {
                        UserId = userId,
                        WeekNumber = currentWeek,
                        PlanTitle = string.IsNullOrWhiteSpace(Input.PlanTitle)
                            ? $"Hafta {currentWeek} Çalışma Planı"
                            : Input.PlanTitle.Trim(),
                        Description = Input.Description?.Trim(),
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.WeeklyPlans.Add(weeklyPlan);
                    await _context.SaveChangesAsync();

                    for (int day = 1; day <= 3 && AnyTopicsRemaining(); day++)
                    {
                        // Find next lesson that still has topics
                        int rotations = 0;
                        while (lessonIdToQueue[lessonIds[lessonIndex]].Count == 0 && rotations < lessonIds.Count)
                        {
                            lessonIndex = (lessonIndex + 1) % lessonIds.Count;
                            rotations++;
                        }

                        if (lessonIdToQueue[lessonIds[lessonIndex]].Count == 0)
                        {
                            break; // no topics anywhere
                        }

                        var chosenLessonId = lessonIds[lessonIndex];
                        var chosenQueue = lessonIdToQueue[chosenLessonId];

                        var pickedTopics = new List<Topic>();
                        for (int i = 0; i < topicsPerDayTarget && chosenQueue.Count > 0; i++)
                        {
                            pickedTopics.Add(chosenQueue.Dequeue());
                        }

                        var dailyPlan = new DailyPlan
                        {
                            WeeklyPlanId = weeklyPlan.Id,
                            DayNumber = day,
                            LessonId = chosenLessonId,
                            TopicCount = pickedTopics.Count,
                            DailyGoal = null,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.DailyPlans.Add(dailyPlan);
                        await _context.SaveChangesAsync();

                        // Create PlanTopic entries for picked topics
                        foreach (var topic in pickedTopics)
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

                        // advance to next lesson for round-robin
                        lessonIndex = (lessonIndex + 1) % lessonIds.Count;
                    }

                    await _context.SaveChangesAsync();
                    currentWeek++;
                }

                TempData["SuccessMessage"] = $"Planlar başarıyla oluşturuldu. Başlangıç haftası: {startWeek}";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Plan oluşturulurken hata oluştu: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Hafta numarası zorunludur.")]
            [Range(1, 52, ErrorMessage = "Hafta numarası 1-52 arasında olmalıdır.")]
            public int WeekNumber { get; set; }

            [MaxLength(100, ErrorMessage = "Plan başlığı en fazla 100 karakter olabilir.")]
            public string PlanTitle { get; set; } = string.Empty;

            [MaxLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
            public string? Description { get; set; }

            // Manual day inputs removed; plan is auto-generated from database topics
            public List<DayInputModel> Days { get; set; } = new();
        }

        public class DayInputModel
        {
            public int LessonId { get; set; }
            public int TopicCount { get; set; }
            public string? DailyGoal { get; set; }
        }
    }
}

