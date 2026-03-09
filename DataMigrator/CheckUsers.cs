using Npgsql;

var connStr = "Host=dpg-d6nk2b4r85hc73fpr75g-a.frankfurt-postgres.render.com;Port=5432;Database=kpssdb;Username=kpss_user;Password=8bZO5qUY7dgfcoHuEzmBSrcSuenPaiZT;SSL Mode=Require";

Console.WriteLine("📊 PostgreSQL Kullanıcı Kontrolü\n");

try
{
    using var conn = new NpgsqlConnection(connStr);
    await conn.OpenAsync();
    
    var cmd = new NpgsqlCommand("SELECT \"Id\", \"Username\", \"IsAdmin\" FROM \"Kullanicilar\"", conn);
    using var reader = await cmd.ExecuteReaderAsync();
    
    Console.WriteLine("ID | Kullanıcı Adı | Admin");
    Console.WriteLine("---|---------------|------");
    
    while (await reader.ReadAsync())
    {
        Console.WriteLine($"{reader.GetInt32(0),2} | {reader.GetString(1),-13} | {(reader.GetBoolean(2) ? "Evet" : "Hayır")}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"❌ HATA: {ex.Message}");
}
