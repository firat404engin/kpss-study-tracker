# 📚 KPSS Study Tracker

**KPSS Sınavına hazırlananlar için kapsamlı takip ve planlama uygulaması**

## 🎯 Özellikler

### 📖 Ders ve Konu Yönetimi
- ✅ Dersler ve konular kategorize edilebilir
- ✅ Konu bazında ilerleme takibi
- ✅ Tamamlanan konular işaretlenebilir
- ✅ Her konu için soru çözüm sayısı takibi

### 📅 Otomatik Çalışma Programı
- ✅ Sınav tarihine göre otomatik program oluşturma
- ✅ Günlük çalışma saati belirleme
- ✅ Haftalık program görünümü
- ✅ Gün gün ders dağılımı
- ✅ Her gün birden fazla ders
- ✅ Konu + Soru çözümü planlaması
- ✅ Collapsible (açılır/kapanır) ders kartları
- ✅ Haftalık navigasyon sistemi

### 📊 İstatistikler
- ✅ Genel ilerleme takibi
- ✅ Ders bazında istatistikler
- ✅ Haftalık ilerleme raporu
- ✅ Tamamlanma yüzdeleri

### 🎯 Deneme Sınavları
- ✅ Deneme sonuçları kaydedilebilir
- ✅ Net hesaplama (Doğru - Yanlış/4)
- ✅ Ders bazında net analizi
- ✅ Geçmiş denemeler görüntüleme

### ✅ Günlük Yapılacaklar
- ✅ To-do list sistemi
- ✅ Tarih bazında görevler
- ✅ Öncelik seviyeleri
- ✅ Kategori bazında filtreleme

### 🔐 Kullanıcı Yönetimi
- ✅ Güvenli kullanıcı girişi (SHA256)
- ✅ Admin yetkilendirme sistemi
- ✅ Kullanıcı bazında veri izolasyonu

### 🎨 Modern Arayüz
- ✅ Responsive tasarım (mobil + desktop)
- ✅ Tailwind CSS ile modern UI
- ✅ Dark mode desteği
- ✅ Türkçe dil desteği
- ✅ Gradient ve animasyonlar

### 🔄 İki Yönlü Senkronizasyon
- ✅ Çalışma programı ↔ Ders detayı senkronizasyonu
- ✅ Konu tamamlandığında tüm sayfalarda güncellenir
- ✅ UserTopicProgress ile merkezi durum yönetimi

## 🛠️ Teknolojiler

### Backend
- **ASP.NET Core 8.0** - Razor Pages
- **Entity Framework Core 8.0** - ORM
- **PostgreSQL** - Veritabanı
- **Cookie Authentication** - Oturum yönetimi

### Frontend
- **Tailwind CSS** - Utility-first CSS
- **Bootstrap Icons** - İkon kütüphanesi
- **Chart.js** - Grafikler
- **Day.js** - Tarih işlemleri

### Database
- **PostgreSQL 16** (Production)
- **MSSQL LocalDB** (Development - eski)

## 🚀 Kurulum

### Gereksinimler
- .NET 8.0 SDK
- PostgreSQL 16+
- Node.js (frontend paketleri için - opsiyonel)

### 1. Repository'yi Clone'la
```bash
git clone https://github.com/KULLANICI_ADI/kpss-study-tracker.git
cd kpss-study-tracker
```

### 2. PostgreSQL Veritabanı Oluştur
```sql
CREATE DATABASE "KPSSStudyTrackerDb";
```

### 3. Connection String Ayarla
`KPSSStudyTracker/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=KPSSStudyTrackerDb;Username=postgres;Password=postgres"
  }
}
```

### 4. Migration Uygula
```bash
cd KPSSStudyTracker
dotnet ef database update
```

### 5. Çalıştır
```bash
dotnet run --urls http://localhost:5203
```

### 6. Tarayıcıda Aç
```
http://localhost:5203
```

### 7. İlk Giriş
- **Kullanıcı:** `admin`
- **Şifre:** `admin123`

⚠️ İlk girişte admin şifresini değiştirin!

## 📦 Ücretsiz Deployment

### Render.com (ÖNERİLEN)

**Hızlı Kurulum:**
1. GitHub'da repository oluştur
2. Windows PowerShell'de:
   ```powershell
   .\deploy.ps1
   ```
3. [render.com](https://render.com) → Blueprint Deploy
4. 5 dakikada hazır!

**Detaylı rehber:** [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md)

**URL:** `https://kpss-study-tracker.onrender.com`

**Maliyet:** $0 💰

## 📁 Proje Yapısı

```
KPSS Study Tracker/
├── KPSSStudyTracker/
│   ├── Pages/                    # Razor Pages
│   │   ├── Account/             # Login/Register
│   │   ├── Lessons/             # Ders yönetimi
│   │   ├── StudyScheduler/      # Çalışma programı
│   │   ├── Statistics/          # İstatistikler
│   │   └── StudyPlan/           # Haftalık plan
│   ├── Models/                  # Domain models
│   ├── Data/                    # DbContext, SeedData
│   ├── MigrationTool/          # MSSQL → PostgreSQL
│   └── wwwroot/                # Static files
├── Migrations/                  # EF Core migrations
├── Dockerfile                   # Docker build
├── render.yaml                  # Render.com config
├── deploy.ps1                   # Quick deploy (Windows)
├── deploy.sh                    # Quick deploy (Linux/Mac)
├── DEPLOYMENT_GUIDE.md         # Deployment rehberi
└── POSTGRESQL_MIGRATION.md     # Migration rehberi
```

## 🔄 MSSQL'den PostgreSQL'e Geçiş

Proje MSSQL LocalDB'den PostgreSQL'e geçirilmiştir.

**Migration rehberi:** [POSTGRESQL_MIGRATION.md](POSTGRESQL_MIGRATION.md)

## 🗓️ KPSS 2026 Tarihleri

- **Sınav Tarihi:** 6 Eylül 2026
- **Başvuru:** 1-13 Temmuz 2026

## 📸 Ekran Görüntüleri

### Ana Sayfa
- Sınava kalan süre sayacı
- Günlük istatistikler
- Hızlı erişim menüsü

### Çalışma Programı
- Haftalık navigasyon
- Gün gün ders dağılımı
- Konu + Soru çözümü
- Collapsible kartlar
- İlerleme takibi

### Ders Yönetimi
- Ders ve konu listesi
- Tamamlanma durumu
- Soru çözüm istatistikleri

## 🤝 Katkıda Bulunma

1. Fork'layın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit'leyin (`git commit -m 'feat: Add amazing feature'`)
4. Push'layın (`git push origin feature/amazing-feature`)
5. Pull Request açın

## 📝 Lisans

Bu proje MIT lisansı altında lisanslanmıştır.

## 👤 Geliştirici

**KPSS Study Tracker**
- GitHub: [@KULLANICI_ADI](https://github.com/KULLANICI_ADI)

## 🙏 Teşekkürler

- ASP.NET Core ekibine
- Tailwind CSS ekibine
- PostgreSQL topluluğuna

## 📞 Destek

Sorun yaşarsanız:
- GitHub Issues: Yeni issue açın
- Email: destek@example.com

---

**⭐ Projeyi beğendiyseniz yıldız vermeyi unutmayın!**

---

## 🎓 Başarılar!

KPSS sınavında başarılar dileriz! 📚✨
