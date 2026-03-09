using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;

namespace KPSSStudyTracker.MigrationTool
{
    /// <summary>
    /// MSSQL'den PostgreSQL'e otomatik veri taşıma aracı
    /// 
    /// KULLANIM:
    /// 1. Bu dosyayı projeye ekleyin
    /// 2. Connection string'leri güncelleyin
    /// 3. Program.cs'den çağırın veya ayrı console app olarak çalıştırın
    /// 
    /// dotnet run migrate
    /// </summary>
    public class DatabaseMigrator
    {
        private readonly string _mssqlConnectionString;
        private readonly string _postgresConnectionString;

        public DatabaseMigrator(string mssqlConn, string postgresConn)
        {
            _mssqlConnectionString = mssqlConn;
            _postgresConnectionString = postgresConn;
        }

        public async Task MigrateAllData()
        {
            Console.WriteLine("🚀 MSSQL -> PostgreSQL Veri Taşıma Başlıyor...\n");

            try
            {
                // Sıralama önemli (foreign key constraints)
                await MigrateTable("Kullanicilar", "Id", "Username", "PasswordHash", "IsAdmin", "CreatedAtUtc");
                await MigrateTable("Dersler", "Id", "Name");
                await MigrateTable("Konular", "Id", "Title", "LessonId", "Source", "Notes", "CreatedAtUtc");
                await MigrateTable("KullaniciKonuIlerlemeleri", "Id", "UserId", "TopicId", "Completed", "SolvedQuestions", 
                    "CorrectAnswers", "WrongAnswers", "Source", "Notes", "CreatedAtUtc", "CompletedAtUtc");
                await MigrateTable("Denemeler", "Id", "UserId", "Name", "Date", "Notes");
                await MigrateTable("DenemeSonuclari", "Id", "MockExamId", "Subject", "Correct", "Wrong", "Empty", "Net");
                await MigrateTable("MotivasyonSozleri", "Id", "Text", "Author");
                await MigrateTable("CalismaPlanlari", "Id", "UserId", "Title", "StartDate", "EndDate", "Status", "Notes", "CreatedAt");
                await MigrateTable("HaftalikPlanlar", "Id", "UserId", "WeekNumber", "PlanTitle", "Description", "IsActive", "CreatedAt");
                await MigrateTable("GunlukPlanlar", "Id", "WeeklyPlanId", "DayNumber", "LessonId", "TopicCount", 
                    "DailyGoal", "IsCompleted", "CompletedAt", "CreatedAt");
                await MigrateTable("PlanKonulari", "Id", "WeeklyPlanId", "DailyPlanId", "TopicId", "IsCompleted", "CompletedAt", "CreatedAt");
                await MigrateTable("Yapilacaklar", "Id", "UserId", "Title", "Description", "Date", "IsCompleted", "Priority", "Category");

                // Sequence'ları resetle
                await ResetSequences();

                Console.WriteLine("\n✅ Tüm veriler başarıyla taşındı!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ HATA: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private async Task MigrateTable(string tableName, params string[] columns)
        {
            Console.Write($"📦 {tableName} taşınıyor... ");

            using var mssqlConn = new SqlConnection(_mssqlConnectionString);
            using var pgConn = new NpgsqlConnection(_postgresConnectionString);

            await mssqlConn.OpenAsync();
            await pgConn.OpenAsync();

            // MSSQL'den veri çek
            var selectCmd = new SqlCommand($"SELECT {string.Join(", ", columns)} FROM {tableName} ORDER BY Id", mssqlConn);
            using var reader = await selectCmd.ExecuteReaderAsync();

            int rowCount = 0;

            while (await reader.ReadAsync())
            {
                // PostgreSQL'e insert et
                var columnList = string.Join(", ", columns.Select(c => $"\"{c}\""));
                var paramList = string.Join(", ", columns.Select((c, i) => $"@p{i}"));
                var insertSql = $"INSERT INTO \"{tableName}\" ({columnList}) VALUES ({paramList})";

                using var insertCmd = new NpgsqlCommand(insertSql, pgConn);

                for (int i = 0; i < columns.Length; i++)
                {
                    var value = reader.IsDBNull(i) ? DBNull.Value : reader.GetValue(i);
                    insertCmd.Parameters.AddWithValue($"@p{i}", value);
                }

                await insertCmd.ExecuteNonQueryAsync();
                rowCount++;
            }

            Console.WriteLine($"✓ {rowCount} satır taşındı");
        }

        private async Task ResetSequences()
        {
            Console.WriteLine("\n🔄 Sequence'lar resetleniyor...");

            using var pgConn = new NpgsqlConnection(_postgresConnectionString);
            await pgConn.OpenAsync();

            string[] tables = { 
                "Kullanicilar", "Dersler", "Konular", "KullaniciKonuIlerlemeleri",
                "Denemeler", "DenemeSonuclari", "MotivasyonSozleri", "CalismaPlanlari",
                "HaftalikPlanlar", "GunlukPlanlar", "PlanKonulari", "Yapilacaklar"
            };

            foreach (var table in tables)
            {
                var sql = $"SELECT setval('\"{table}_Id_seq\"', (SELECT COALESCE(MAX(\"Id\"), 0) FROM \"{table}\"))";
                using var cmd = new NpgsqlCommand(sql, pgConn);
                await cmd.ExecuteNonQueryAsync();
                Console.WriteLine($"  ✓ {table}_Id_seq resetlendi");
            }
        }
    }

    // =====================================================
    // STANDALONE CONSOLE APP OLARAK ÇALIŞTIRMAK İÇİN:
    // =====================================================
    /*
    class Program
    {
        static async Task Main(string[] args)
        {
            var mssqlConn = "Server=(localdb)\\mssqllocaldb;Database=KPSSStudyTrackerDb;Trusted_Connection=true;TrustServerCertificate=true;";
            var postgresConn = "Host=dpg-d6nk2b4r85hc73fpr75g-a.frankfurt-postgres.render.com;Port=5432;Database=kpssdb;Username=kpss_user;Password=8bZO5qUY7dgfcoHuEzmBSrcSuenPaiZT;SSL Mode=Require";

            var migrator = new DatabaseMigrator(mssqlConn, postgresConn);
            await migrator.MigrateAllData();

            Console.WriteLine("\nMigrasyon tamamlandı. Devam etmek için bir tuşa basın...");
            Console.ReadKey();
        }
    }
    */
}
