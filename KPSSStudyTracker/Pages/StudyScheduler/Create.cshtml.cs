using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KPSSStudyTracker.Pages.StudyScheduler
{
    public class CreateModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public SchedulerInput Input { get; set; } = new();

        public List<LessonWithTopics> AvailableLessons { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public DateTime ExamDate { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return RedirectToPage("/Account/Login");

            // Sınav tarihini config'den al
            var config = HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            ExamDate = DateTime.Parse(config["KPSS:ExamDate"] ?? DateTime.UtcNow.Date.AddMonths(6).ToString("yyyy-MM-dd"));

            // Dersleri ve konularını getir
            var lessons = await _context.Lessons
                .Include(l => l.Topics)
                .Where(l => l.Topics.Any())
                .ToListAsync();

            // Kullanıcının tamamladığı konuları al
            var completedTopicIds = await _context.UserTopicProgresses
                .Where(utp => utp.UserId == userId && utp.Completed)
                .Select(utp => utp.TopicId)
                .ToListAsync();

            AvailableLessons = lessons.Select(l => new LessonWithTopics
            {
                LessonId = l.Id,
                LessonName = l.Name,
                Topics = l.Topics.Select(t => new TopicInfo
                {
                    TopicId = t.Id,
                    TopicTitle = t.Title,
                    IsCompleted = completedTopicIds.Contains(t.Id)
                }).ToList()
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var userId = GetCurrentUserIdRequired();

            // Validasyonlar
            if (Input.ExamDate <= DateTime.UtcNow.Date)
            {
                ErrorMessage = "Sınav tarihi bugünden sonra olmalıdır.";
                await OnGetAsync();
                return Page();
            }

            if (Input.SelectedDays == null || !Input.SelectedDays.Any())
            {
                ErrorMessage = "En az bir çalışma günü seçmelisiniz.";
                await OnGetAsync();
                return Page();
            }

            if (Input.SelectedLessonIds == null || !Input.SelectedLessonIds.Any())
            {
                ErrorMessage = "En az bir ders seçmelisiniz.";
                await OnGetAsync();
                return Page();
            }

            // Çalışma programı oluştur
            var schedule = await GenerateStudyScheduleAsync(userId);

            if (schedule == null || !schedule.Any())
            {
                ErrorMessage = "Çalışma programı oluşturulamadı. Lütfen parametreleri kontrol edin.";
                await OnGetAsync();
                return Page();
            }

            // Veritabanına kaydet
            await SaveScheduleToDatabase(userId, schedule);

            TempData["SuccessMessage"] = $"Çalışma programı başarıyla oluşturuldu! {schedule.Count} günlük plan oluşturuldu.";
            return RedirectToPage("/StudyScheduler/View");
        }

        private async Task<List<DailySchedule>> GenerateStudyScheduleAsync(int userId)
        {
            var schedule = new List<DailySchedule>();

            // 1. Bugünün tarihini al
            var startDate = DateTime.UtcNow.Date;
            var endDate = Input.ExamDate;

            // 2. Seçilen dersleri ve konuları al
            var selectedLessonsWithTopics = await _context.Lessons
                .Include(l => l.Topics)
                .Where(l => Input.SelectedLessonIds.Contains(l.Id))
                .ToListAsync();

            if (!selectedLessonsWithTopics.Any())
            {
                return schedule;
            }

            // Tamamlanmış konuları filtrele
            var completedTopicIds = new List<int>();
            if (Input.ExcludeCompletedTopics)
            {
                completedTopicIds = await _context.UserTopicProgresses
                    .Where(utp => utp.UserId == userId && utp.Completed)
                    .Select(utp => utp.TopicId)
                    .ToListAsync();
            }

            // Her ders için kalan konuları hazırla
            var lessonTopicQueues = selectedLessonsWithTopics.ToDictionary(
                l => l.Id,
                l => new Queue<Topic>(l.Topics.Where(t => !completedTopicIds.Contains(t.Id)).ToList())
            );

            // 3. Çalışma günlerini hesapla
            var workDays = new List<DateTime>();
            for (var date = startDate; date < endDate; date = date.AddDays(1))
            {
                var dayOfWeek = (int)date.DayOfWeek;
                if (dayOfWeek == 0) dayOfWeek = 7;
                
                if (Input.SelectedDays.Contains(dayOfWeek))
                {
                    workDays.Add(date);
                }
            }

            if (!workDays.Any())
            {
                return schedule;
            }

            // 4. Her güne birden fazla ders dağıt
            var lessonsPerDay = Math.Min(Input.SelectedLessonIds.Count, 3); // Maksimum 3 ders/gün
            var hoursPerLesson = Input.DailyHours / lessonsPerDay;
            var topicsPerLesson = Math.Max(1, (int)(hoursPerLesson * Input.TopicsPerHour));
            
            var lessonIndex = 0;
            var lessonIds = Input.SelectedLessonIds.ToList();

            foreach (var workDay in workDays)
            {
                var dailyLessons = new List<LessonSchedule>();
                var remainingHours = Input.DailyHours;

                // Her gün için dersler seç (rotasyon ile)
                for (int i = 0; i < lessonsPerDay && remainingHours > 0; i++)
                {
                    var currentLessonId = lessonIds[lessonIndex % lessonIds.Count];
                    lessonIndex++;

                    // Bu dersin konuları bitti mi kontrol et
                    if (!lessonTopicQueues.ContainsKey(currentLessonId) || !lessonTopicQueues[currentLessonId].Any())
                    {
                        continue;
                    }

                    var lesson = selectedLessonsWithTopics.First(l => l.Id == currentLessonId);
                    var topicQueue = lessonTopicQueues[currentLessonId];
                    
                    // Bu ders için konular seç
                    var lessonTopics = new List<Topic>();
                    var topicsToTake = Math.Min(topicsPerLesson, topicQueue.Count);
                    
                    for (int j = 0; j < topicsToTake; j++)
                    {
                        if (topicQueue.Any())
                        {
                            lessonTopics.Add(topicQueue.Dequeue());
                        }
                    }

                    if (lessonTopics.Any())
                    {
                        var lessonHours = Math.Min(hoursPerLesson, remainingHours);
                        var questionCount = (int)(lessonHours * 15); // Saatte ~15 soru

                        dailyLessons.Add(new LessonSchedule
                        {
                            LessonId = lesson.Id,
                            LessonName = lesson.Name,
                            Topics = lessonTopics,
                            QuestionCount = questionCount,
                            EstimatedHours = lessonHours
                        });

                        remainingHours -= lessonHours;
                    }
                }

                if (dailyLessons.Any())
                {
                    schedule.Add(new DailySchedule
                    {
                        Date = workDay,
                        Lessons = dailyLessons,
                        EstimatedHours = Input.DailyHours - remainingHours
                    });
                }

                // Tüm konular bitti mi kontrol et
                if (lessonTopicQueues.All(kvp => !kvp.Value.Any()))
                {
                    break;
                }
            }

            return schedule;
        }

        private async Task SaveScheduleToDatabase(int userId, List<DailySchedule> schedule)
        {
            // Mevcut aktif planları pasif yap
            var existingPlans = await _context.WeeklyPlans
                .Where(wp => wp.UserId == userId && wp.IsActive)
                .ToListAsync();

            foreach (var plan in existingPlans)
            {
                plan.IsActive = false;
            }

            // Çalışma günü sayısını hesapla
            var totalDays = schedule.Count;
            var totalTopics = schedule.Sum(s => s.Lessons.Sum(l => l.Topics.Count));
            var totalLessons = schedule.Sum(s => s.Lessons.Count);
            
            // Yeni planı oluştur
            var weeklyPlan = new WeeklyPlan
            {
                UserId = userId,
                WeekNumber = 1,
                PlanTitle = $"Otomatik Program - {DateTime.UtcNow.Date:dd/MM/yyyy}",
                Description = $"Sınav: {Input.ExamDate:dd/MM/yyyy}, {totalDays} gün, {totalTopics} konu, {totalLessons} ders planı, Günlük {Input.DailyHours}h",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.WeeklyPlans.Add(weeklyPlan);
            await _context.SaveChangesAsync();

            // Her gün için birden fazla ders kaydedilecek
            foreach (var day in schedule)
            {
                if (!day.Lessons.Any()) continue;

                foreach (var lessonSchedule in day.Lessons)
                {
                    var dailyPlan = new DailyPlan
                    {
                        WeeklyPlanId = weeklyPlan.Id,
                        DayNumber = (int)day.Date.DayOfWeek == 0 ? 7 : (int)day.Date.DayOfWeek,
                        LessonId = lessonSchedule.LessonId,
                        TopicCount = lessonSchedule.Topics.Count,
                        DailyGoal = $"{day.Date:dd/MM/yyyy} - {lessonSchedule.LessonName}: {lessonSchedule.Topics.Count} konu + {lessonSchedule.QuestionCount} soru (~{Math.Round(lessonSchedule.EstimatedHours, 1)}h)",
                        IsCompleted = false,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.DailyPlans.Add(dailyPlan);
                    await _context.SaveChangesAsync();

                    // Konuları ekle
                    foreach (var topic in lessonSchedule.Topics)
                    {
                        var planTopic = new PlanTopic
                        {
                            WeeklyPlanId = weeklyPlan.Id,
                            DailyPlanId = dailyPlan.Id,
                            TopicId = topic.Id,
                            IsCompleted = false,
                            CreatedAt = DateTime.UtcNow
                        };

                        _context.PlanTopics.Add(planTopic);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public class SchedulerInput
        {
            [Required(ErrorMessage = "Sınav tarihi zorunludur")]
            public DateTime ExamDate { get; set; }

            [Required(ErrorMessage = "Günlük çalışma saati zorunludur")]
            [Range(1, 16, ErrorMessage = "Günlük çalışma saati 1-16 arası olmalıdır")]
            public double DailyHours { get; set; } = 8;

            [Range(1, 10, ErrorMessage = "Saatlik konu sayısı 1-10 arası olmalıdır")]
            public int TopicsPerHour { get; set; } = 2;

            public List<int> SelectedDays { get; set; } = new();
            public List<int> SelectedLessonIds { get; set; } = new();
            public bool ExcludeCompletedTopics { get; set; } = true;
        }

        public class LessonWithTopics
        {
            public int LessonId { get; set; }
            public string LessonName { get; set; } = string.Empty;
            public List<TopicInfo> Topics { get; set; } = new();
        }

        public class TopicInfo
        {
            public int TopicId { get; set; }
            public string TopicTitle { get; set; } = string.Empty;
            public bool IsCompleted { get; set; }
        }

        public class LessonSchedule
        {
            public int LessonId { get; set; }
            public string LessonName { get; set; } = string.Empty;
            public List<Topic> Topics { get; set; } = new();
            public int QuestionCount { get; set; }
            public double EstimatedHours { get; set; }
        }

        public class DailySchedule
        {
            public DateTime Date { get; set; }
            public List<LessonSchedule> Lessons { get; set; } = new();
            public double EstimatedHours { get; set; }
        }
    }
}
