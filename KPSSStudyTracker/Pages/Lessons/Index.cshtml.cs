using KPSSStudyTracker.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Pages;
using KPSSStudyTracker.Models;

namespace KPSSStudyTracker.Pages.Lessons
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) { _context = context; }

        public List<LessonVm> Lessons { get; set; } = new();
        public bool IsAdmin { get; set; } = false;

        public async Task OnGetAsync()
        {
            var userId = GetCurrentUserIdRequired();
            IsAdmin = await IsAdminAsync(_context);
            var lessons = await _context.Lessons
                .Include(l => l.Topics)
                .ToListAsync();
            
            var topicIds = lessons.SelectMany(l => l.Topics.Select(t => t.Id)).ToList();
            var userProgress = await _context.UserTopicProgresses
                .Include(utp => utp.Topic)
                .Where(utp => utp.UserId == userId && topicIds.Contains(utp.TopicId))
                .ToListAsync();
            
            var progressDict = userProgress.GroupBy(utp => utp.Topic.LessonId).ToDictionary(g => g.Key, g => g.ToList());
            
            Lessons = lessons.Select(l => 
            {
                var topicIdsForLesson = l.Topics.Select(t => t.Id).ToList();
                var progressForLesson = progressDict.GetValueOrDefault(l.Id, new List<UserTopicProgress>())
                    .Where(utp => topicIdsForLesson.Contains(utp.TopicId))
                    .ToList();
                
                return new LessonVm
                {
                    Id = l.Id,
                    Name = l.Name,
                    TopicCount = l.Topics.Count,
                    CompletedCount = progressForLesson.Count(p => p.Completed)
                };
            })
            .Where(l => l.TopicCount > 0)
            .OrderBy(l => l.Name)
            .ToList();
        }

        public class LessonVm
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int TopicCount { get; set; }
            public int CompletedCount { get; set; }
        }
    }
}



