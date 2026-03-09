using KPSSStudyTracker.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq;
using KPSSStudyTracker.Pages;
using KPSSStudyTracker.Models;

namespace KPSSStudyTracker.Pages.Statistics
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) { _context = context; }

        // Chart data properties
        public string LessonLabelsJson { get; set; } = "[]";
        public string CorrectJson { get; set; } = "[]";
        public string WrongJson { get; set; } = "[]";
        public string CompletionLabelsJson { get; set; } = "[]";
        public string CompletionDataJson { get; set; } = "[]";
        public string NetProgressLabelsJson { get; set; } = "[]";
        public string NetProgressDataJson { get; set; } = "[]";
        
        // Statistics properties
        public List<string> WeakTopics { get; set; } = new();
        public int TotalQuestionsSolved { get; set; }
        public double OverallAccuracy { get; set; }
        public int TotalTopicsCompleted { get; set; }
        public int TotalTopics { get; set; }
        public double CompletionRate { get; set; }
        public double AverageNetScore { get; set; }

        public async Task OnGetAsync()
        {
            var userId = GetCurrentUserIdRequired();
            
            // Get all lessons with topics (topics are now global)
            var lessons = await _context.Dersler
                .Include(l => l.Topics)
                .ToListAsync();
            
            var topicIds = lessons.SelectMany(l => l.Topics.Select(t => t.Id)).ToList();
            
            // Get user progress for all topics
            var userProgress = await _context.UserTopicProgresses
                .Include(utp => utp.Topic)
                .Where(utp => utp.UserId == userId && topicIds.Contains(utp.TopicId))
                .ToListAsync();
            
            var progressByTopicId = userProgress.ToDictionary(utp => utp.TopicId);
            var progressByLessonId = userProgress
                .GroupBy(utp => utp.Topic.LessonId)
                .ToDictionary(g => g.Key, g => g.ToList());
            
            // Calculate statistics by lesson
            var byLesson = lessons.Select(l =>
            {
                var lessonTopicIds = l.Topics.Select(t => t.Id).ToList();
                var lessonProgress = progressByLessonId.GetValueOrDefault(l.Id, new List<UserTopicProgress>())
                    .Where(utp => lessonTopicIds.Contains(utp.TopicId))
                    .ToList();
                
                return new
                {
                    l.Name,
                    Correct = lessonProgress.Sum(p => p.CorrectAnswers),
                    Wrong = lessonProgress.Sum(p => p.WrongAnswers),
                    Completed = lessonProgress.Count(p => p.Completed),
                    Total = l.Topics.Count,
                    SolvedQuestions = lessonProgress.Sum(p => p.SolvedQuestions)
                };
            })
            .Where(l => l.Total > 0)
            .ToList();

            // Get mock exam statistics (only current user's exams)
            var mockExams = await _context.Denemeler
                .Where(me => me.UserId == userId)
                .Include(me => me.Results)
                .OrderBy(me => me.Date)
                .Select(me => new
                {
                    me.Name,
                    me.Date,
                    NetScore = me.Results.Sum(r => r.Correct - (double)r.Wrong / 4)
                })
                .ToListAsync();

            // Prepare chart data
            LessonLabelsJson = JsonSerializer.Serialize(byLesson.Select(x => x.Name));
            CorrectJson = JsonSerializer.Serialize(byLesson.Select(x => x.Correct));
            WrongJson = JsonSerializer.Serialize(byLesson.Select(x => x.Wrong));
            CompletionLabelsJson = JsonSerializer.Serialize(byLesson.Select(x => x.Name));
            CompletionDataJson = JsonSerializer.Serialize(byLesson.Select(x => x.Total == 0 ? 0 : (int)Math.Round(100.0 * x.Completed / x.Total)));
            
            // Mock exam progress data
            NetProgressLabelsJson = JsonSerializer.Serialize(mockExams.Select(x => x.Date.ToString("dd/MM")));
            NetProgressDataJson = JsonSerializer.Serialize(mockExams.Select(x => Math.Round(x.NetScore, 1)));

            // Calculate overall statistics
            TotalQuestionsSolved = byLesson.Sum(x => x.SolvedQuestions);
            TotalTopicsCompleted = byLesson.Sum(x => x.Completed);
            TotalTopics = byLesson.Sum(x => x.Total);
            CompletionRate = TotalTopics == 0 ? 0 : Math.Round(100.0 * TotalTopicsCompleted / TotalTopics, 1);
            
            var totalCorrect = byLesson.Sum(x => x.Correct);
            var totalWrong = byLesson.Sum(x => x.Wrong);
            OverallAccuracy = (totalCorrect + totalWrong) == 0 ? 0 : Math.Round(100.0 * totalCorrect / (totalCorrect + totalWrong), 1);
            
            AverageNetScore = mockExams.Any() ? Math.Round(mockExams.Average(x => x.NetScore), 1) : 0;

            // Find weak topics (lowest accuracy) - using UserTopicProgress
            WeakTopics = userProgress
                .Where(utp => utp.SolvedQuestions > 0)
                .Select(utp => new { 
                    utp.Topic.Title, 
                    Accuracy = utp.CorrectAnswers + utp.WrongAnswers > 0 ? 
                        (double)utp.CorrectAnswers / (utp.CorrectAnswers + utp.WrongAnswers) : 0 
                })
                .OrderBy(x => x.Accuracy)
                .Take(3)
                .Select(x => x.Title)
                .ToList();
        }
    }
}


