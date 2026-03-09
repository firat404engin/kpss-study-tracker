# KPSS Study Tracker

KPSS hazırlık için kişisel çalışma takip uygulaması. Dersler, konular, ilerleme takibi ve deneme sınavları.

## 🚀 Özellikler

- ✅ Çoklu kullanıcı desteği
- ✅ Ders ve konu yönetimi (Admin)
- ✅ İlerleme takibi
- ✅ Deneme sınavı sonuçları
- ✅ İstatistikler
- ✅ Çalışma planları
- ✅ Günlük yapılacaklar listesi

## 🛠️ Teknolojiler

- .NET 8.0
- ASP.NET Core Razor Pages
- Entity Framework Core
- PostgreSQL (ücretsiz hosting için)
- Bootstrap 5 + Tailwind CSS

## 📋 Gereksinimler

- .NET 8.0 SDK
- PostgreSQL (yerel geliştirme için) veya Render.com PostgreSQL (production)

## 🚀 Yerel Kurulum

1. **Repository'yi klonlayın:**
   ```bash
   git clone https://github.com/KULLANICI_ADINIZ/KPSSStudyTracker.git
   cd KPSSStudyTracker
   ```

2. **Connection String'i ayarlayın:**
   `appsettings.json` dosyasında PostgreSQL connection string'i güncelleyin:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=KPSSStudyTrackerDb;Username=postgres;Password=postgres"
     }
   }
   ```

3. **Veritabanını oluşturun:**
   ```bash
   dotnet ef database update
   ```

4. **Uygulamayı çalıştırın:**
   ```bash
   dotnet run
   ```

5. **Tarayıcıda açın:**
   - URL: `https://localhost:5001` veya `http://localhost:5000`
   - Admin: `admin` / Şifre: `admin123`

## 🌐 Ücretsiz Hosting (Render.com)

Detaylı deployment adımları için: [DEPLOYMENT_STEPS.md](DEPLOYMENT_STEPS.md)

### Hızlı Başlangıç:

1. ✅ GitHub'a push edin
2. ✅ Render.com'da hesap oluşturun
3. ✅ PostgreSQL database oluşturun (ücretsiz)
4. ✅ Web Service oluşturun
5. ✅ Environment variables ekleyin
6. ✅ Deploy!

**Uygulama başlarken otomatik olarak:**
- Migration'lar çalışır
- Veritabanı tabloları oluşturulur
- SeedData çalışır (admin kullanıcısı ve tüm konular eklenir)

## 📝 Migration

Migration'lar uygulama başlangıcında otomatik çalışır. Manuel çalıştırmak isterseniz:

```bash
dotnet ef database update
```

## 🔐 Varsayılan Kullanıcı

- **Username:** `admin`
- **Password:** `admin123`

**Önemli:** İlk çalıştırmada otomatik oluşturulur. Production'da şifreyi değiştirin!

## 📚 Dersler ve Konular

Uygulama şu dersleri destekler:
- **Tarih** (47 konu)
- **Türkçe** (43 konu)
- **Matematik** (15 konu)
- **Coğrafya** (60 konu)

Tüm konular ve Source/Notes bilgileri SeedData ile otomatik eklenir.

## 🛠️ Geliştirme

### Yeni Migration Oluşturma:
```bash
dotnet ef migrations add MigrationName
```

### Migration'ı Geri Alma:
```bash
dotnet ef migrations remove
```

### Veritabanını Temizleme ve Yeniden Oluşturma:
```bash
dotnet ef database drop
dotnet ef database update
```

## 📄 Lisans

Bu proje eğitim amaçlıdır.

## 🤝 Katkıda Bulunma

İssue ve Pull Request'ler hoş karşılanır!

---

**Not:** Production'da admin şifresini değiştirmeyi unutmayın! 🔒

