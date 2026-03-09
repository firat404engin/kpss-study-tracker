# 🚀 MSSQL → PostgreSQL Migration Guide

## 📋 Özet

Bu proje **MSSQL LocalDB**'den **PostgreSQL**'e tam geçiş yapmıştır.

## ✅ Yapılan Değişiklikler

### 1. **NuGet Paketleri**
```xml
<!-- Kaldırılan -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.10" />

<!-- Eklenen -->
<PackageReference Include="Microsoft.Data.SqlClient" Version="5.1.5" />
<!-- (Npgsql.EntityFrameworkCore.PostgreSQL zaten mevcuttu) -->
```

### 2. **Program.cs**
```csharp
// Önce:
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

// Sonra:
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
```

### 3. **appsettings.json**
```json
// Önce:
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=KPSSStudyTrackerDb;Trusted_Connection=true;TrustServerCertificate=true;"

// Sonra:
"DefaultConnection": "Host=localhost;Port=5432;Database=KPSSStudyTrackerDb;Username=postgres;Password=postgres"
```

### 4. **Migrations**
- ❌ Eski MSSQL migration'ları silindi
- ✅ Yeni PostgreSQL migration'ı oluşturuldu: `InitialPostgreSQL`

## 🛠️ PostgreSQL Kurulum ve Konfigürasyon

### Windows'da PostgreSQL Kurulumu

1. **PostgreSQL İndir ve Kur:**
   ```
   https://www.postgresql.org/download/windows/
   ```
   - Version: 16.x veya üstü
   - Port: 5432 (varsayılan)
   - Password: `postgres` (veya kendi şifrenizi)

2. **pgAdmin Kullanarak Veritabanı Oluştur:**
   ```sql
   CREATE DATABASE "KPSSStudyTrackerDb";
   ```

3. **Veya Command Line ile:**
   ```bash
   psql -U postgres
   CREATE DATABASE "KPSSStudyTrackerDb";
   \q
   ```

### Docker ile PostgreSQL (Alternatif)

```bash
docker run --name kpss-postgres \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=KPSSStudyTrackerDb \
  -p 5432:5432 \
  -d postgres:16
```

## 📊 Veri Taşıma Seçenekleri

### Seçenek 1: Otomatik C# Tool (Önerilen)

```bash
# 1. MSSQL ve PostgreSQL'in ikisi de çalışır durumda olmalı
# 2. DatabaseMigrator.cs'teki connection string'leri güncelle
# 3. Program.cs'den çağır veya standalone çalıştır

cd KPSSStudyTracker
dotnet run migrate
```

**Not:** `DatabaseMigrator.cs` dosyasında connection string'leri düzenleyin:
```csharp
var mssqlConn = "Server=(localdb)\\mssqllocaldb;Database=KPSSStudyTrackerDb;...";
var postgresConn = "Host=localhost;Port=5432;Database=KPSSStudyTrackerDb;...";
```

### Seçenek 2: Manuel SQL Script

1. **MSSQL'den Export:**
   ```bash
   # migrate_mssql_to_postgresql.sql dosyasındaki SELECT'leri kullan
   ```

2. **CSV'ye Kaydet:**
   - SQL Server Management Studio
   - Azure Data Studio
   - sqlcmd

3. **PostgreSQL'e Import:**
   ```bash
   psql -U postgres -d KPSSStudyTrackerDb
   \copy "Kullanicilar" FROM '/path/to/kullanicilar.csv' CSV HEADER
   ```

4. **Sequence'ları Resetle:**
   ```sql
   SELECT setval('"Kullanicilar_Id_seq"', (SELECT MAX("Id") FROM "Kullanicilar"));
   -- Diğer tablolar için tekrarla
   ```

### Seçenek 3: Temiz Başlangıç (Veri Taşıma Yok)

```bash
# Veritabanını oluştur ve migration'ları uygula
cd KPSSStudyTracker
dotnet ef database update

# Uygula (seed data otomatik oluşturulur)
dotnet run
```

## 🔧 Migration Komutları

### Yeni Migration Oluştur
```bash
dotnet ef migrations add MigrationName
```

### Migration Uygula
```bash
dotnet ef database update
```

### Migration Geri Al
```bash
dotnet ef database update PreviousMigrationName
```

### Migration Sil
```bash
dotnet ef migrations remove
```

### Veritabanını Sıfırla
```bash
dotnet ef database drop
dotnet ef database update
```

## 📝 Connection String Formatları

### Development (appsettings.json)
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=KPSSStudyTrackerDb;Username=postgres;Password=postgres"
```

### Production (Environment Variables)
```bash
export ConnectionStrings__DefaultConnection="Host=prod-server;Port=5432;Database=KPSSStudyTrackerDb;Username=app_user;Password=strong_password;SSL Mode=Require"
```

### Docker Compose
```yaml
services:
  postgres:
    image: postgres:16
    environment:
      POSTGRES_DB: KPSSStudyTrackerDb
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  app:
    build: .
    environment:
      ConnectionStrings__DefaultConnection: "Host=postgres;Port=5432;Database=KPSSStudyTrackerDb;Username=postgres;Password=postgres"
    depends_on:
      - postgres

volumes:
  postgres_data:
```

## 🔍 Veritabanı Schema Karşılaştırma

### MSSQL vs PostgreSQL

| Özellik | MSSQL | PostgreSQL |
|---------|-------|------------|
| Tablo İsimleri | `Kullanicilar` | `"Kullanicilar"` (quotes) |
| Identity/Serial | `IDENTITY(1,1)` | `SERIAL` / `BIGSERIAL` |
| DateTime | `datetime2` | `timestamp` |
| Boolean | `bit` | `boolean` |
| String | `nvarchar(max)` | `text` |
| Sequence | Auto | `tablename_id_seq` |

### Önemli Farklar

1. **Case Sensitivity:**
   - MSSQL: Case-insensitive
   - PostgreSQL: Case-sensitive (quoted identifiers)

2. **Auto-Increment:**
   - MSSQL: `IDENTITY` column
   - PostgreSQL: `SERIAL` + sequence

3. **Date Functions:**
   ```csharp
   // MSSQL
   .Where(x => x.Date >= DateTime.Now.AddDays(-7))
   
   // PostgreSQL (aynı kod çalışır, EF Core translate eder)
   .Where(x => x.Date >= DateTime.Now.AddDays(-7))
   ```

## ⚠️ Bilinen Sorunlar ve Çözümleri

### 1. Sequence Out of Sync
**Sorun:** Insert sırasında duplicate key error

**Çözüm:**
```sql
SELECT setval('"Kullanicilar_Id_seq"', (SELECT MAX("Id") FROM "Kullanicilar"));
```

### 2. Connection Refused
**Sorun:** PostgreSQL'e bağlanılamıyor

**Çözüm:**
- PostgreSQL servisinin çalıştığını kontrol edin
- Port 5432'nin açık olduğunu kontrol edin
- Firewall ayarlarını kontrol edin

### 3. Password Authentication Failed
**Sorun:** Şifre kabul edilmiyor

**Çözüm:**
```bash
# pg_hba.conf dosyasını düzenle
# C:\Program Files\PostgreSQL\16\data\pg_hba.conf

# Şu satırı bulun ve değiştirin:
# host    all    all    127.0.0.1/32    scram-sha-256
# Şuna:
# host    all    all    127.0.0.1/32    md5

# PostgreSQL'i restart edin
```

## 📚 Ek Kaynaklar

- [PostgreSQL Documentation](https://www.postgresql.org/docs/)
- [Npgsql Entity Framework Core Provider](https://www.npgsql.org/efcore/)
- [Entity Framework Core Migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## 🎯 Sonraki Adımlar

1. ✅ PostgreSQL kurulumu
2. ✅ Connection string güncelleme
3. ✅ Migration oluşturma
4. ⏳ Veri taşıma (opsiyonel)
5. ⏳ Test ve doğrulama
6. ⏳ Production deployment

## 📞 Destek

Sorun yaşarsanız:
1. Connection string'i kontrol edin
2. PostgreSQL servisini kontrol edin
3. Migration'ları tekrar oluşturun
4. Log dosyalarını kontrol edin

---

**Not:** Bu migration guide, KPSS Study Tracker projesinin MSSQL'den PostgreSQL'e geçişi için hazırlanmıştır.
