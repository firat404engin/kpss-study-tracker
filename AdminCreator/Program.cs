using System;
using System.Security.Cryptography;
using System.Text;
using Npgsql;

var connStr = "Host=dpg-d6nk2b4r85hc73fpr75g-a.frankfurt-postgres.render.com;Port=5432;Database=kpssdb;Username=kpss_user;Password=8bZO5qUY7dgfcoHuEzmBSrcSuenPaiZT;SSL Mode=Require";

Console.WriteLine("🔑 TÜM KULLANICILARIN ŞİFRELERİNİ GÜNCELLE\n");

try
{
    using var conn = new NpgsqlConnection(connStr);
    await conn.OpenAsync();
    
    // SHA256 hash fonksiyonu
    string ComputeSha256(string input)
    {
        using var sha = SHA256.Create();
        return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
    }
    
    // Tüm kullanıcıları güncelle
    var users = new[]
    {
        ("deneme", "123456", true),
        ("deneme2", "123456", false),
        ("admin", "admin123", true)
    };
    
    foreach (var (username, password, isAdmin) in users)
    {
        var hash = ComputeSha256(password);
        
        // Kullanıcı var mı kontrol et
        var checkCmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"Kullanicilar\" WHERE \"Username\" = @username", conn);
        checkCmd.Parameters.AddWithValue("username", username);
        var exists = Convert.ToInt32(await checkCmd.ExecuteScalarAsync()) > 0;
        
        if (exists)
        {
            // Güncelle
            var updateCmd = new NpgsqlCommand(
                "UPDATE \"Kullanicilar\" SET \"PasswordHash\" = @hash, \"IsAdmin\" = @admin WHERE \"Username\" = @username", 
                conn);
            updateCmd.Parameters.AddWithValue("username", username);
            updateCmd.Parameters.AddWithValue("hash", hash);
            updateCmd.Parameters.AddWithValue("admin", isAdmin);
            await updateCmd.ExecuteNonQueryAsync();
            
            Console.WriteLine($"✅ '{username}' güncellendi (şifre: {password})");
        }
        else
        {
            // Ekle
            var insertCmd = new NpgsqlCommand(
                "INSERT INTO \"Kullanicilar\" (\"Username\", \"PasswordHash\", \"IsAdmin\", \"CreatedAtUtc\") VALUES (@username, @hash, @admin, @created)", 
                conn);
            insertCmd.Parameters.AddWithValue("username", username);
            insertCmd.Parameters.AddWithValue("hash", hash);
            insertCmd.Parameters.AddWithValue("admin", isAdmin);
            insertCmd.Parameters.AddWithValue("created", DateTime.UtcNow);
            await insertCmd.ExecuteNonQueryAsync();
            
            Console.WriteLine($"✅ '{username}' eklendi (şifre: {password})");
        }
    }
    
    Console.WriteLine("\n🎉 Tüm kullanıcılar hazır!");
    Console.WriteLine("\n📝 Giriş Bilgileri:");
    Console.WriteLine("   🌐 Site: https://kpss-study-tracker.onrender.com");
    Console.WriteLine("\n   👤 Kullanıcı 1:");
    Console.WriteLine("      Username: deneme");
    Console.WriteLine("      Password: 123456");
    Console.WriteLine("      Admin: Evet");
    Console.WriteLine("\n   👤 Kullanıcı 2:");
    Console.WriteLine("      Username: deneme2");
    Console.WriteLine("      Password: 123456");
    Console.WriteLine("      Admin: Hayır");
    Console.WriteLine("\n   👤 Kullanıcı 3:");
    Console.WriteLine("      Username: admin");
    Console.WriteLine("      Password: admin123");
    Console.WriteLine("      Admin: Evet");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ HATA: {ex.Message}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
}
