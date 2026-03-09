using System;
using System.Security.Cryptography;
using System.Text;
using Npgsql;

var connStr = "Host=dpg-d6nk2b4r85hc73fpr75g-a.frankfurt-postgres.render.com;Port=5432;Database=kpssdb;Username=kpss_user;Password=8bZO5qUY7dgfcoHuEzmBSrcSuenPaiZT;SSL Mode=Require";

Console.WriteLine("🔐 Yeni Admin Kullanıcısı Oluşturuluyor...\n");

var username = "admin";
var password = "admin123";

// SHA256 hash
using var sha256 = SHA256.Create();
var passwordBytes = Encoding.UTF8.GetBytes(password);
var hashBytes = sha256.ComputeHash(passwordBytes);
var passwordHash = Convert.ToHexString(hashBytes);

try
{
    using var conn = new NpgsqlConnection(connStr);
    await conn.OpenAsync();
    
    // Önce mevcut admin kullanıcısını sil
    var deleteCmd = new NpgsqlCommand("DELETE FROM \"Kullanicilar\" WHERE \"Username\" = @username", conn);
    deleteCmd.Parameters.AddWithValue("username", username);
    await deleteCmd.ExecuteNonQueryAsync();
    
    // Yeni admin kullanıcı ekle
    var insertCmd = new NpgsqlCommand(
        "INSERT INTO \"Kullanicilar\" (\"Username\", \"PasswordHash\", \"IsAdmin\", \"CreatedAtUtc\") VALUES (@username, @hash, @admin, @created)", 
        conn);
    
    insertCmd.Parameters.AddWithValue("username", username);
    insertCmd.Parameters.AddWithValue("hash", passwordHash);
    insertCmd.Parameters.AddWithValue("admin", true);
    insertCmd.Parameters.AddWithValue("created", DateTime.UtcNow);
    
    await insertCmd.ExecuteNonQueryAsync();
    
    Console.WriteLine("✅ Admin kullanıcısı oluşturuldu!");
    Console.WriteLine($"\n📝 Giriş Bilgileri:");
    Console.WriteLine($"   Kullanıcı Adı: {username}");
    Console.WriteLine($"   Şifre: {password}");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ HATA: {ex.Message}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
}
