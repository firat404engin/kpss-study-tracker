using KPSSStudyTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // English DbSets
        public DbSet<UserAccount> Users => Set<UserAccount>();
        public DbSet<Lesson> Lessons => Set<Lesson>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<UserTopicProgress> UserTopicProgresses => Set<UserTopicProgress>();
        public DbSet<MockExam> MockExams => Set<MockExam>();
        public DbSet<MockExamResult> MockExamResults => Set<MockExamResult>();
        public DbSet<MotivationQuote> MotivationQuotes => Set<MotivationQuote>();
        public DbSet<StudyPlan> StudyPlans => Set<StudyPlan>();
        public DbSet<WeeklyPlan> WeeklyPlans => Set<WeeklyPlan>();
        public DbSet<DailyPlan> DailyPlans => Set<DailyPlan>();
        public DbSet<PlanTopic> PlanTopics => Set<PlanTopic>();
        public DbSet<DailyTodo> DailyTodos => Set<DailyTodo>();

        // Turkish DbSets for easier access
        public DbSet<UserAccount> Kullanicilar => Set<UserAccount>();
        public DbSet<Lesson> Dersler => Set<Lesson>();
        public DbSet<Topic> Konular => Set<Topic>();
        public DbSet<UserTopicProgress> KullaniciKonuIlerlemeleri => Set<UserTopicProgress>();
        public DbSet<MockExam> Denemeler => Set<MockExam>();
        public DbSet<MockExamResult> DenemeSonuclari => Set<MockExamResult>();
        public DbSet<MotivationQuote> MotivasyonSozleri => Set<MotivationQuote>();
        public DbSet<StudyPlan> CalismaPlanlari => Set<StudyPlan>();
        public DbSet<WeeklyPlan> HaftalikPlanlar => Set<WeeklyPlan>();
        public DbSet<DailyPlan> GunlukPlanlar => Set<DailyPlan>();
        public DbSet<PlanTopic> PlanKonulari => Set<PlanTopic>();
        public DbSet<DailyTodo> Yapilacaklar => Set<DailyTodo>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Turkish table names
            modelBuilder.Entity<UserAccount>().ToTable("Kullanicilar");
            modelBuilder.Entity<Lesson>().ToTable("Dersler");
            modelBuilder.Entity<Topic>().ToTable("Konular");
            modelBuilder.Entity<UserTopicProgress>().ToTable("KullaniciKonuIlerlemeleri");
            modelBuilder.Entity<MockExam>().ToTable("Denemeler");
            modelBuilder.Entity<MockExamResult>().ToTable("DenemeSonuclari");
            modelBuilder.Entity<MotivationQuote>().ToTable("MotivasyonSozleri");
            modelBuilder.Entity<StudyPlan>().ToTable("CalismaPlanlari");
            modelBuilder.Entity<WeeklyPlan>().ToTable("HaftalikPlanlar");
            modelBuilder.Entity<DailyPlan>().ToTable("GunlukPlanlar");
            modelBuilder.Entity<PlanTopic>().ToTable("PlanKonulari");
            modelBuilder.Entity<DailyTodo>().ToTable("Yapilacaklar");

            // Weekly Plan relationships
            modelBuilder.Entity<WeeklyPlan>()
                .HasMany(w => w.DailyPlans)
                .WithOne(d => d.WeeklyPlan)
                .HasForeignKey(d => d.WeeklyPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeeklyPlan>()
                .HasMany(w => w.PlanTopics)
                .WithOne(pt => pt.WeeklyPlan)
                .HasForeignKey(pt => pt.WeeklyPlanId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DailyPlan>()
                .HasOne(d => d.Lesson)
                .WithMany()
                .HasForeignKey(d => d.LessonId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DailyPlan>()
                .HasMany(d => d.PlanTopics)
                .WithOne(pt => pt.DailyPlan)
                .HasForeignKey(pt => pt.DailyPlanId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlanTopic>()
                .HasOne(pt => pt.Topic)
                .WithMany()
                .HasForeignKey(pt => pt.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Lesson>()
                .HasMany(l => l.Topics)
                .WithOne(t => t.Lesson)
                .HasForeignKey(t => t.LessonId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserTopicProgress relationships
            modelBuilder.Entity<UserTopicProgress>()
                .HasOne(utp => utp.User)
                .WithMany()
                .HasForeignKey(utp => utp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserTopicProgress>()
                .HasOne(utp => utp.Topic)
                .WithMany(t => t.UserProgress)
                .HasForeignKey(utp => utp.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint: one progress record per user per topic
            modelBuilder.Entity<UserTopicProgress>()
                .HasIndex(utp => new { utp.UserId, utp.TopicId })
                .IsUnique();

            modelBuilder.Entity<UserTopicProgress>()
                .HasIndex(utp => utp.UserId);

            modelBuilder.Entity<UserTopicProgress>()
                .HasIndex(utp => utp.TopicId);

            modelBuilder.Entity<MockExamResult>()
                .HasOne(r => r.MockExam)
                .WithMany(e => e.Results)
                .HasForeignKey(r => r.MockExamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserAccount>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<DailyTodo>()
                .HasIndex(t => t.Date);

            // User relationships and cascade deletes (Topic artık user'a bağlı değil, global)
            modelBuilder.Entity<MockExam>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudyPlan>()
                .HasOne(sp => sp.User)
                .WithMany()
                .HasForeignKey(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeeklyPlan>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DailyTodo>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for UserId for better query performance
            modelBuilder.Entity<MockExam>()
                .HasIndex(e => e.UserId);

            modelBuilder.Entity<WeeklyPlan>()
                .HasIndex(w => w.UserId);

            modelBuilder.Entity<DailyTodo>()
                .HasIndex(t => new { t.UserId, t.Date });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Convert all DateTime values to UTC before saving to PostgreSQL
            foreach (var entry in ChangeTracker.Entries())
            {
                foreach (var property in entry.Properties)
                {
                    if (property.CurrentValue is DateTime dateTime)
                    {
                        if (dateTime.Kind == DateTimeKind.Local)
                        {
                            property.CurrentValue = dateTime.ToUniversalTime();
                        }
                        else if (dateTime.Kind == DateTimeKind.Unspecified)
                        {
                            property.CurrentValue = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                        }
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}


