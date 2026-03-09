using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Npgsql;

var mssqlConn = "Server=(localdb)\\mssqllocaldb;Database=KPSSStudyTrackerDb;Trusted_Connection=true;TrustServerCertificate=true;";
var postgresConn = "Host=dpg-d6nk2b4r85hc73fpr75g-a.frankfurt-postgres.render.com;Port=5432;Database=kpssdb;Username=kpss_user;Password=8bZO5qUY7dgfcoHuEzmBSrcSuenPaiZT;SSL Mode=Require";

Console.WriteLine("🚀 MSSQL -> PostgreSQL Veri Taşıma Başlıyor...\n");

try
{
    // Sıralama önemli (foreign key constraints)
    await MigrateTable(mssqlConn, postgresConn, "Kullanicilar", "Id", "Username", "PasswordHash", "IsAdmin", "CreatedAtUtc");
    await MigrateTable(mssqlConn, postgresConn, "Dersler", "Id", "Name");
    await MigrateTable(mssqlConn, postgresConn, "Konular", "Id", "Title", "LessonId", "Source", "Notes", "CreatedAtUtc");
    await MigrateTable(mssqlConn, postgresConn, "KullaniciKonuIlerlemeleri", "Id", "UserId", "TopicId", "Completed", "SolvedQuestions", 
        "CorrectAnswers", "WrongAnswers", "Source", "Notes", "CreatedAtUtc", "CompletedAtUtc");
    await MigrateTable(mssqlConn, postgresConn, "Denemeler", "Id", "UserId", "Name", "Date", "Notes");
    await MigrateTable(mssqlConn, postgresConn, "DenemeSonuclari", "Id", "MockExamId", "Subject", "Correct", "Wrong", "Empty", "Net");

    Console.WriteLine("\n✅ Tüm veriler başarıyla taşındı!");
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ HATA: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}

Console.WriteLine("\nDevam etmek için bir tuşa basın...");
Console.ReadKey();

async Task MigrateTable(string mssqlConnStr, string pgConnStr, string tableName, params string[] columns)
{
    Console.Write($"📦 {tableName} taşınıyor... ");

    using var mssqlConn = new SqlConnection(mssqlConnStr);
    using var pgConn = new NpgsqlConnection(pgConnStr);

    await mssqlConn.OpenAsync();
    await pgConn.OpenAsync();

    var selectCmd = new SqlCommand($"SELECT {string.Join(", ", columns)} FROM {tableName} ORDER BY Id", mssqlConn);
    using var reader = await selectCmd.ExecuteReaderAsync();

    int rowCount = 0;

    while (await reader.ReadAsync())
    {
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
