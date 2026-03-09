using Npgsql;

var connStr = "Host=dpg-d6nk2b4r85hc73fpr75g-a.frankfurt-postgres.render.com;Port=5432;Database=kpssdb;Username=kpss_user;Password=8bZO5qUY7dgfcoHuEzmBSrcSuenPaiZT;SSL Mode=Require";

Console.WriteLine("🔧 PostgreSQL Sequence Reset Başlıyor...\n");

try
{
    using var conn = new NpgsqlConnection(connStr);
    await conn.OpenAsync();
    
    var tables = new[] 
    { 
        "Kullanicilar", 
        "Dersler", 
        "Konular", 
        "KullaniciKonuIlerlemeleri",
        "Denemeler",
        "DenemeSonuclari",
        "HaftalikPlanlar",
        "GunlukPlanlar",
        "PlanKonulari",
        "Yapilacaklar",
        "CalismaPlanlari",
        "MotivasyonSozleri"
    };
    
    foreach (var table in tables)
    {
        try
        {
            // Get max ID
            var maxIdCmd = new NpgsqlCommand($"SELECT COALESCE(MAX(\"Id\"), 0) FROM \"{table}\"", conn);
            var maxId = Convert.ToInt32(await maxIdCmd.ExecuteScalarAsync());
            
            if (maxId > 0)
            {
                // Reset sequence
                var resetCmd = new NpgsqlCommand(
                    $"SELECT setval(pg_get_serial_sequence('\"{table}\"', 'Id'), {maxId + 1}, false)", 
                    conn);
                await resetCmd.ExecuteNonQueryAsync();
                
                Console.WriteLine($"✅ {table}: sequence resetlendi (son ID: {maxId}, yeni: {maxId + 1})");
            }
            else
            {
                Console.WriteLine($"ℹ️  {table}: boş tablo, reset gerekmiyor");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️  {table}: {ex.Message}");
        }
    }
    
    Console.WriteLine("\n🎉 Tüm sequence'ler resetlendi!");
}
catch (Exception ex)
{
    Console.WriteLine($"\n❌ HATA: {ex.Message}");
    Console.WriteLine($"Stack: {ex.StackTrace}");
}
