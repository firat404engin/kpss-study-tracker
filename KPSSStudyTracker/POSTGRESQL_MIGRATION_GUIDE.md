# PostgreSQL'e Geçiş Rehberi

## ✅ Genel Durum

**İyi haber:** Projeniz Entity Framework Core kullanıyor ve PostgreSQL'e geçiş **çoğunlukla sorunsuz** olacak!

### 🟢 Sorun Olmayacak Kısımlar

1. **Entity Framework Core Modelleri** ✅
   - Tüm model tanımları (`DomainModels.cs`) PostgreSQL'de çalışır
   - `AppDbContext` yapılandırması sorunsuz
   - Relationship'ler (One-to-Many, Foreign Keys) aynı şekilde çalışır

2. **LINQ Sorguları** ✅
   - Tüm LINQ sorguları PostgreSQL'de aynı şekilde çalışır
   - EF Core PostgreSQL provider'ı otomatik çeviri yapar

3. **CRUD İşlemleri** ✅
   - Insert, Update, Delete işlemleri sorunsuz
   - Async metodlar çalışır

### 🟡 Dikkat Edilmesi Gerekenler

1. **Mevcut Migration Dosyaları** ⚠️
   - Migration'larda SQL Server'a özel SQL kodları var:
     - `IF NOT EXISTS (SELECT * FROM sys.columns...)` 
     - `SELECT TOP 1` → PostgreSQL'de `LIMIT 1`
     - `NVARCHAR` → PostgreSQL'de `VARCHAR` veya `TEXT`
     - `IDENTITY(1,1)` → PostgreSQL'de `SERIAL` veya `GENERATED ALWAYS AS IDENTITY`
   
   **Çözüm:** Mevcut migration'ları silip PostgreSQL için yeniden oluşturulması gerekebilir.

2. **Manuel SQL Kodları** ⚠️
   - Migration'larda SQL Server syntax'ı kullanılmış
   - Bunların PostgreSQL uyumlu hale getirilmesi gerekir

3. **Veri Tipleri** ✅ (Otomatik Çevrilir)
   - `NVARCHAR` → `VARCHAR` (EF Core otomatik yapar)
   - `INT IDENTITY` → `SERIAL` (EF Core otomatik yapar)

## 🔧 Geçiş Adımları

### 1. PostgreSQL NuGet Paketini Ekle

```bash
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### 2. Program.cs'i Güncelle

```csharp
// ÖNCE (SQL Server):
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// SONRA (PostgreSQL):
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

### 3. Connection String Güncelle

```json
// appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=KPSSStudyTrackerDb;Username=postgres;Password=yourpassword"
  }
}
```

### 4. Migration'ları Temizle ve Yeniden Oluştur

**ÖNEMLİ:** Mevcut SQL Server migration'ları PostgreSQL'de çalışmaz. Yeniden oluşturulması gerekir.

#### Seçenek A: Migration'ları Sil ve Yeniden Oluştur (Önerilen)

```bash
# Tüm migration dosyalarını sil (Migrations klasöründeki .cs dosyaları)
# Veya:
dotnet ef migrations remove --force

# PostgreSQL için yeni migration oluştur
dotnet ef migrations add InitialCreatePostgreSQL

# Veritabanını oluştur
dotnet ef database update
```

#### Seçenek B: Mevcut Migration'ları Düzelt (Manuel)

Migration dosyalarındaki SQL Server syntax'larını PostgreSQL'e çevirmek gerekir. Bu daha zor ve hata riski var.

**Örnek Düzeltmeler:**

| SQL Server | PostgreSQL |
|------------|------------|
| `SELECT TOP 1` | `SELECT ... LIMIT 1` |
| `IF NOT EXISTS (SELECT * FROM sys.columns...)` | `DO $$ BEGIN IF NOT EXISTS (SELECT 1 FROM information_schema.columns...)` |
| `NVARCHAR(150)` | `VARCHAR(150)` |
| `IDENTITY(1,1)` | `SERIAL` veya `GENERATED ALWAYS AS IDENTITY` |

### 5. Manuel SQL Kodlarını Düzelt

Migration dosyalarında şu gibi kodlar var:

**SQL Server:**
```sql
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Konular]') AND name = 'Source')
BEGIN
    ALTER TABLE [Konular] ADD [Source] NVARCHAR(150) NULL;
END
```

**PostgreSQL:**
```sql
DO $$ 
BEGIN 
    IF NOT EXISTS (
        SELECT 1 FROM information_schema.columns 
        WHERE table_name = 'Konular' AND column_name = 'Source'
    ) THEN
        ALTER TABLE "Konular" ADD COLUMN "Source" VARCHAR(150) NULL;
    END IF;
END $$;
```

## ⚠️ Potansiyel Sorunlar ve Çözümleri

### Sorun 1: Migration'lardaki SQL Server Syntax

**Risk:** Orta-Yüksek
**Çözüm:** Migration'ları yeniden oluşturmak (Seçenek A önerilir)

### Sorun 2: Case Sensitivity

**Risk:** Düşük
**PostgreSQL:** Tablo/kolon isimleri case-sensitive (tırnak içinde)
**SQL Server:** Case-insensitive
**Çözüm:** EF Core otomatik yönetiyor, ama manuel SQL'de dikkat edin

### Sorun 3: String Fonksiyonları

**Risk:** Çok Düşük
**Not:** Projenizde özel string fonksiyonu kullanmıyorsunuz, sorun olmaz

### Sorun 4: Date/Time Fonksiyonları

**Risk:** Düşük
**Not:** `DateTime.UtcNow` C# tarafında, sorun olmaz

## ✅ Önerilen Yaklaşım

### Hızlı ve Güvenli Yöntem:

1. ✅ Migration klasörünü sil (ya da yeni bir branch'te çalış)
2. ✅ PostgreSQL paketini ekle
3. ✅ Program.cs'i güncelle
4. ✅ Connection string'i PostgreSQL formatına çevir
5. ✅ `dotnet ef migrations add InitialCreate` - Yeni migration oluştur
6. ✅ `dotnet ef database update` - Veritabanını oluştur
7. ✅ SeedData çalışacak ve veriler eklenecek

**Süre:** 10-15 dakika

## 🎯 Sonuç

**PostgreSQL'e geçiş yapılabilir!** ✅

- Ana kodda sorun yok
- Sadece migration'ların yeniden oluşturulması gerekli
- Veri kaybı riski YOK (migration'ları silip yeniden oluşturuyoruz, verileri değil)
- SeedData otomatik çalışacak

**Öneri:** Test ortamında önce deneyin, sonra production'a alın.

## 📝 Hızlı Başlangıç Komutları

```bash
# 1. PostgreSQL paketi ekle
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

# 2. Migration klasörünü temizle (manuel silin veya backup alın)

# 3. Yeni migration oluştur
dotnet ef migrations add InitialCreatePostgreSQL

# 4. Veritabanını oluştur
dotnet ef database update

# 5. Çalıştır ve test et
dotnet run
```

---

**Not:** Eğer isterseniz, ben PostgreSQL'e geçişi yapabilirim! Sadece onaylayın. 🚀

