using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Pages.StudyScheduler
{
    public class ViewModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public ViewModel(AppDbContext context) { _context = context; }

        public WeeklyPlan? ActivePlan { get; set; }
        public List<WeekSchedule> WeeklySchedules { get; set; } = new();
        public ScheduleStats Stats { get; set; } = new();
        
        // Haftalık Navigasyon
        [BindProperty(SupportsGet = true)]
        public int CurrentWeek { get; set; } = 1;
        public WeekSchedule? CurrentWeekSchedule { get; set; }
        public bool HasPreviousWeek => CurrentWeek > 1;
        public bool HasNextWeek => CurrentWeek < Stats.TotalWeeks;

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return RedirectToPage("/Account/Login");

            // Aktif planı getir
            ActivePlan = await _context.WeeklyPlans
                .Include(wp => wp.DailyPlans)
                    .ThenInclude(dp => dp.Lesson)
                .Include(wp => wp.DailyPlans)
                    .ThenInclude(dp => dp.PlanTopics)
                        .ThenInclude(pt => pt.Topic)
                            .ThenInclude(t => t.UserProgress) // ⭐ UserProgress'i yükle
                .Where(wp => wp.UserId == userId && wp.IsActive)
                .OrderByDescending(wp => wp.CreatedAt)
                .FirstOrDefaultAsync();

            if (ActivePlan == null)
            {
                return Page();
            }

            // Günlük planları tarihe göre grupla (bir günde birden fazla ders olabilir)
            var dailyPlansGroupedByDate = ActivePlan.DailyPlans
                .OrderBy(dp => dp.Id)
                .ToList();

            // Tarihleri oluştur ve planları grupla
            var currentDate = DateTime.UtcNow.Date;
            var plansByDate = new Dictionary<DateTime, List<DailyPlan>>();
            
            foreach (var dailyPlan in dailyPlansGroupedByDate)
            {
                if (!plansByDate.ContainsKey(currentDate))
                {
                    plansByDate[currentDate] = new List<DailyPlan>();
                }
                
                plansByDate[currentDate].Add(dailyPlan);
                
                // Eğer bu günün son planıysa bir sonraki güne geç
                // (Basit mantık: her 2-3 plan bir gün kabul et)
                if (plansByDate[currentDate].Count >= 3)
                {
                    currentDate = currentDate.AddDays(1);
                }
            }

            // Günlük schedule'ları oluştur
            var allDays = new List<DaySchedule>();
            foreach (var kvp in plansByDate.OrderBy(x => x.Key))
            {
                var date = kvp.Key;
                var plans = kvp.Value;

                var daySchedule = new DaySchedule
                {
                    Date = date,
                    DayName = GetTurkishDayName(date.DayOfWeek),
                    Lessons = plans.Select(dp => new LessonScheduleInfo
                    {
                        DailyPlanId = dp.Id,
                        LessonName = dp.Lesson?.Name ?? "Bilinmeyen",
                        Topics = dp.PlanTopics.Select(pt => new TopicSchedule
                        {
                            TopicId = pt.TopicId,
                            TopicTitle = pt.Topic?.Title ?? "Bilinmeyen",
                            // ⭐ UserTopicProgress'ten durum oku (senkronizasyon)
                            IsCompleted = pt.Topic?.UserProgress
                                .FirstOrDefault(up => up.UserId == userId && up.TopicId == pt.TopicId)?.Completed 
                                ?? pt.IsCompleted,
                            CompletedAt = pt.Topic?.UserProgress
                                .FirstOrDefault(up => up.UserId == userId && up.TopicId == pt.TopicId)?.CompletedAtUtc 
                                ?? pt.CompletedAt
                        }).ToList(),
                        EstimatedHours = dp.TopicCount * 0.5,
                        IsCompleted = dp.IsCompleted,
                        QuestionCount = ExtractQuestionCount(dp.DailyGoal)
                    }).ToList(),
                    EstimatedHours = plans.Sum(dp => dp.TopicCount * 0.5),
                    IsCompleted = plans.All(dp => dp.IsCompleted)
                };

                allDays.Add(daySchedule);
            }

            // Haftalara böl
            var weekNumber = 1;
            for (int i = 0; i < allDays.Count; i += 7)
            {
                var weekDays = allDays.Skip(i).Take(7).ToList();
                if (weekDays.Any())
                {
                    WeeklySchedules.Add(new WeekSchedule
                    {
                        WeekNumber = weekNumber,
                        Days = weekDays,
                        StartDate = weekDays.First().Date,
                        EndDate = weekDays.Last().Date
                    });
                    weekNumber++;
                }
            }

            // İstatistikler
            Stats.TotalDays = allDays.Count;
            Stats.TotalTopics = allDays.Sum(s => s.Lessons.Sum(l => l.Topics.Count));
            Stats.CompletedTopics = allDays.Sum(s => s.Lessons.Sum(l => l.Topics.Count(t => t.IsCompleted)));
            Stats.TotalHours = allDays.Sum(s => s.EstimatedHours);
            Stats.CompletedDays = allDays.Count(s => s.IsCompleted);
            Stats.ProgressPercent = Stats.TotalTopics > 0 ? Math.Round((double)Stats.CompletedTopics / Stats.TotalTopics * 100, 1) : 0;
            Stats.TotalWeeks = WeeklySchedules.Count;

            // Geçerli haftayı ayarla
            if (CurrentWeek < 1) CurrentWeek = 1;
            if (CurrentWeek > Stats.TotalWeeks) CurrentWeek = Stats.TotalWeeks;
            
            CurrentWeekSchedule = WeeklySchedules.FirstOrDefault(w => w.WeekNumber == CurrentWeek);

            return Page();
        }

        private int ExtractQuestionCount(string dailyGoal)
        {
            // DailyGoal'dan soru sayısını çıkar: "50 soru" formatında
            if (string.IsNullOrEmpty(dailyGoal)) return 0;
            
            var match = System.Text.RegularExpressions.Regex.Match(dailyGoal, @"(\d+)\s*soru");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int count))
            {
                return count;
            }
            return 0;
        }

        public async Task<IActionResult> OnPostCompleteTopicAsync(int topicId, int dailyPlanId, int currentWeek = 1)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return RedirectToPage("/Account/Login");

            // PlanTopic'i güncelle (çalışma programındaki durum)
            var planTopic = await _context.PlanTopics
                .FirstOrDefaultAsync(pt => pt.TopicId == topicId && pt.DailyPlanId == dailyPlanId);

            if (planTopic != null)
            {
                planTopic.IsCompleted = !planTopic.IsCompleted;
                planTopic.CompletedAt = planTopic.IsCompleted ? DateTime.UtcNow : null;
                await _context.SaveChangesAsync();

                // Günlük planı kontrol et
                var dailyPlan = await _context.DailyPlans
                    .Include(dp => dp.PlanTopics)
                    .FirstOrDefaultAsync(dp => dp.Id == dailyPlanId);

                if (dailyPlan != null)
                {
                    dailyPlan.IsCompleted = dailyPlan.PlanTopics.All(pt => pt.IsCompleted);
                    dailyPlan.CompletedAt = dailyPlan.IsCompleted ? DateTime.UtcNow : null;
                    await _context.SaveChangesAsync();
                }
            }

            // ⭐ Senkronizasyon: UserTopicProgress'i güncelle (ders detay sayfası için)
            var userProgress = await _context.UserTopicProgresses
                .FirstOrDefaultAsync(utp => utp.TopicId == topicId && utp.UserId == userId.Value);

            if (userProgress == null)
            {
                // UserTopicProgress yoksa oluştur
                userProgress = new UserTopicProgress
                {
                    UserId = userId.Value,
                    TopicId = topicId,
                    Completed = planTopic?.IsCompleted ?? false,
                    CompletedAtUtc = planTopic?.IsCompleted == true ? DateTime.UtcNow : null
                };
                _context.UserTopicProgresses.Add(userProgress);
            }
            else
            {
                // Varsa güncelle
                userProgress.Completed = planTopic?.IsCompleted ?? false;
                userProgress.CompletedAtUtc = userProgress.Completed ? DateTime.UtcNow : null;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage(new { CurrentWeek = currentWeek });
        }

        private string GetTurkishDayName(DayOfWeek dayOfWeek)
        {
            return dayOfWeek switch
            {
                DayOfWeek.Monday => "Pazartesi",
                DayOfWeek.Tuesday => "Salı",
                DayOfWeek.Wednesday => "Çarşamba",
                DayOfWeek.Thursday => "Perşembe",
                DayOfWeek.Friday => "Cuma",
                DayOfWeek.Saturday => "Cumartesi",
                DayOfWeek.Sunday => "Pazar",
                _ => "Bilinmeyen"
            };
        }

        public class WeekSchedule
        {
            public int WeekNumber { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public List<DaySchedule> Days { get; set; } = new();
        }

        public class DaySchedule
        {
            public DateTime Date { get; set; }
            public string DayName { get; set; } = string.Empty;
            public List<LessonScheduleInfo> Lessons { get; set; } = new();
            public double EstimatedHours { get; set; }
            public bool IsCompleted { get; set; }
        }

        public class LessonScheduleInfo
        {
            public int DailyPlanId { get; set; }
            public string LessonName { get; set; } = string.Empty;
            public List<TopicSchedule> Topics { get; set; } = new();
            public int QuestionCount { get; set; }
            public double EstimatedHours { get; set; }
            public bool IsCompleted { get; set; }
        }

        public class TopicSchedule
        {
            public int TopicId { get; set; }
            public string TopicTitle { get; set; } = string.Empty;
            public bool IsCompleted { get; set; }
            public DateTime? CompletedAt { get; set; }
        }

        public class ScheduleStats
        {
            public int TotalDays { get; set; }
            public int TotalWeeks { get; set; }
            public int CompletedDays { get; set; }
            public int TotalTopics { get; set; }
            public int CompletedTopics { get; set; }
            public double TotalHours { get; set; }
            public double ProgressPercent { get; set; }
        }
    }
}
