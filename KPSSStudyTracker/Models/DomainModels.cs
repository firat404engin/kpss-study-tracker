using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KPSSStudyTracker.Models
{
    public class UserAccount
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public bool IsAdmin { get; set; } = false;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }

    public class Lesson
    {
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public List<Topic> Topics { get; set; } = new();
    }

    public class Topic
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        [MaxLength(150)]
        public string? Source { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        // Navigation property for user progress
        public List<UserTopicProgress> UserProgress { get; set; } = new();
    }

    // User-specific progress for topics
    public class UserTopicProgress
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public UserAccount? User { get; set; }
        
        public int TopicId { get; set; }
        public Topic? Topic { get; set; }

        public bool Completed { get; set; }
        public int SolvedQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        [MaxLength(150)]
        public string? Source { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAtUtc { get; set; }

        [NotMapped]
        public double AccuracyPct => SolvedQuestions == 0 ? 0 : Math.Round((double)CorrectAnswers / Math.Max(1, CorrectAnswers + WrongAnswers) * 100, 2);
    }

    public class MockExam
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        [MaxLength(1000)]
        public string? Notes { get; set; }

        public int UserId { get; set; }
        public UserAccount? User { get; set; }

        public List<MockExamResult> Results { get; set; } = new();

        [NotMapped]
        public double TotalNet => Results.Sum(r => r.Net);
    }

    public class MockExamResult
    {
        public int Id { get; set; }
        public int MockExamId { get; set; }
        public MockExam? MockExam { get; set; }

        // Link to a Lesson for per-lesson scores
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }

        public int Correct { get; set; }
        public int Wrong { get; set; }

        [NotMapped]
        public double Net => Math.Round(Correct - Wrong / 4.0, 2);
    }

    public class MotivationQuote
    {
        public int Id { get; set; }
        [Required, MaxLength(300)]
        public string Text { get; set; } = string.Empty;
        [MaxLength(120)]
        public string? Author { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        public bool IsCustom { get; set; }
    }

    public class StudyPlan
    {
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string BlockName { get; set; } = string.Empty;
        [Range(1, 52)]
        public int WeekNumber { get; set; }
        [Range(1, 3)]
        public int DayNumber { get; set; }
        [Required, MaxLength(200)]
        public string Subject { get; set; } = string.Empty;
        [Required, MaxLength(500)]
        public string Topics { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAtUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public UserAccount? User { get; set; }
    }

    // Dynamic Weekly Plan System
    public class WeeklyPlan
    {
        public int Id { get; set; }
        [Required, Range(1, 52)]
        public int WeekNumber { get; set; }
        [Required, MaxLength(150)]
        public string PlanTitle { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public int UserId { get; set; }
        public UserAccount? User { get; set; }

        public List<DailyPlan> DailyPlans { get; set; } = new();
        public List<PlanTopic> PlanTopics { get; set; } = new();

        [NotMapped]
        public double Progress => PlanTopics.Count > 0 ? Math.Round((double)PlanTopics.Count(t => t.IsCompleted) / PlanTopics.Count * 100, 1) : 0;
    }

    public class DailyPlan
    {
        public int Id { get; set; }
        public int WeeklyPlanId { get; set; }
        public WeeklyPlan? WeeklyPlan { get; set; }
        [Range(1, 7)]
        public int DayNumber { get; set; }
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        [Range(1, 20)]
        public int TopicCount { get; set; }
        [MaxLength(300)]
        public string? DailyGoal { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public List<PlanTopic> PlanTopics { get; set; } = new();
    }

    public class PlanTopic
    {
        public int Id { get; set; }
        public int WeeklyPlanId { get; set; }
        public WeeklyPlan? WeeklyPlan { get; set; }
        public int DailyPlanId { get; set; }
        public DailyPlan? DailyPlan { get; set; }
        public int TopicId { get; set; }
        public Topic? Topic { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // Daily Todo System
    public class DailyTodo
    {
        public int Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string? Notes { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        public int UserId { get; set; }
        public UserAccount? User { get; set; }
    }
}


