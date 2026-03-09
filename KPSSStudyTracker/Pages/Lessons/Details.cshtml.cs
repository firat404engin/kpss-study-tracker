using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Lessons
{
    public class DetailsModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public DetailsModel(AppDbContext context) { _context = context; }

        public int LessonId { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public List<TopicWithProgress> Topics { get; set; } = new();
        public bool IsAdmin { get; set; } = false;

        public class TopicWithProgress
        {
            public Topic Topic { get; set; } = null!;
            public UserTopicProgress? Progress { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = GetCurrentUserIdRequired();
            IsAdmin = await IsAdminAsync(_context);
            
            var lesson = await _context.Lessons.Include(l => l.Topics).FirstOrDefaultAsync(l => l.Id == id);
            if (lesson == null) return RedirectToPage("Index");
            LessonId = id;
            LessonName = lesson.Name;
            
            // Get all topics for this lesson (global)
            var allTopics = lesson.Topics.OrderBy(t => t.Id).ToList();
            var topicIds = allTopics.Select(t => t.Id).ToList();
            
            // Get user progress for these topics
            var userProgress = await _context.UserTopicProgresses
                .Where(utp => utp.UserId == userId && topicIds.Contains(utp.TopicId))
                .ToListAsync();
            
            var progressDict = userProgress.ToDictionary(utp => utp.TopicId);
            
            // Combine topics with progress
            Topics = allTopics.Select(t => new TopicWithProgress
            {
                Topic = t,
                Progress = progressDict.GetValueOrDefault(t.Id)
            }).ToList();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAddTopicAsync(int id, string Title, int SolvedQuestions, int CorrectAnswers, int WrongAnswers, string? Source, string? Notes)
        {
            // Sadece admin konu ekleyebilir
            if (!await IsAdminAsync(_context))
            {
                return Forbid();
            }
            
            var userId = GetCurrentUserIdRequired();
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return RedirectToPage("Index");
            
            // Create global topic
            var topic = new Topic
            {
                LessonId = id,
                Title = Title
            };
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            
            // Update topic with Source and Notes (global)
            if (!string.IsNullOrEmpty(Source))
                topic.Source = Source;
            if (!string.IsNullOrEmpty(Notes))
                topic.Notes = Notes;
            await _context.SaveChangesAsync();
            
            // Create user progress for this topic
            var progress = new UserTopicProgress
            {
                UserId = userId,
                TopicId = topic.Id,
                SolvedQuestions = SolvedQuestions,
                CorrectAnswers = CorrectAnswers,
                WrongAnswers = WrongAnswers
            };
            _context.UserTopicProgresses.Add(progress);
            await _context.SaveChangesAsync();
            
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostToggleAsync(int id, int topicId)
        {
            var userId = GetCurrentUserIdRequired();
            var progress = await _context.UserTopicProgresses
                .FirstOrDefaultAsync(utp => utp.TopicId == topicId && utp.UserId == userId);
            
            if (progress == null)
            {
                // Create progress if doesn't exist
                progress = new UserTopicProgress
                {
                    UserId = userId,
                    TopicId = topicId
                };
                _context.UserTopicProgresses.Add(progress);
            }
            
            progress.Completed = !progress.Completed;
            progress.CompletedAtUtc = progress.Completed ? DateTime.UtcNow : null;
            await _context.SaveChangesAsync();

            // ⭐ Senkronizasyon: Çalışma programındaki PlanTopic'leri güncelle
            var planTopics = await _context.PlanTopics
                .Where(pt => pt.TopicId == topicId)
                .Include(pt => pt.DailyPlan)
                .ToListAsync();

            foreach (var planTopic in planTopics)
            {
                planTopic.IsCompleted = progress.Completed;
                planTopic.CompletedAt = progress.CompletedAtUtc;
                
                // Günlük planı kontrol et
                if (planTopic.DailyPlan != null)
                {
                    var allPlanTopics = await _context.PlanTopics
                        .Where(pt => pt.DailyPlanId == planTopic.DailyPlanId)
                        .ToListAsync();
                    
                    planTopic.DailyPlan.IsCompleted = allPlanTopics.All(pt => pt.IsCompleted);
                    planTopic.DailyPlan.CompletedAt = planTopic.DailyPlan.IsCompleted ? DateTime.UtcNow : null;
                }
            }

            await _context.SaveChangesAsync();
            
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostDeleteTopicAsync(int id, int topicId)
        {
            var userId = GetCurrentUserIdRequired();
            var progress = await _context.UserTopicProgresses
                .FirstOrDefaultAsync(utp => utp.TopicId == topicId && utp.UserId == userId);
            
            if (progress != null)
            {
                _context.UserTopicProgresses.Remove(progress);
                await _context.SaveChangesAsync();
            }
            
            // Note: We don't delete the Topic itself as it's global
            // Only delete user's progress
            
            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostUpdateTopicAsync(int id, int topicId, string Title, int SolvedQuestions, int CorrectAnswers, int WrongAnswers, string? Source, string? Notes)
        {
            // Sadece admin konu güncelleyebilir (global topic bilgilerini)
            if (!await IsAdminAsync(_context))
            {
                return Forbid();
            }
            
            var userId = GetCurrentUserIdRequired();
            
            // Update global topic title
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic != null)
            {
                topic.Title = Title;
            }
            
            // Update topic with Source and Notes (global)
            if (topic != null)
            {
                if (!string.IsNullOrEmpty(Source))
                    topic.Source = Source;
                if (!string.IsNullOrEmpty(Notes))
                    topic.Notes = Notes;
            }
            
            // Update or create user progress
            var progress = await _context.UserTopicProgresses
                .FirstOrDefaultAsync(utp => utp.TopicId == topicId && utp.UserId == userId);
            
            if (progress == null)
            {
                progress = new UserTopicProgress
                {
                    UserId = userId,
                    TopicId = topicId
                };
                _context.UserTopicProgresses.Add(progress);
            }
            
            progress.SolvedQuestions = SolvedQuestions;
            progress.CorrectAnswers = CorrectAnswers;
            progress.WrongAnswers = WrongAnswers;
            
            await _context.SaveChangesAsync();
            return RedirectToPage(new { id });
        }
    }
}


