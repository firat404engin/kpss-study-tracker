# KPSS Study Tracker - Ücretsiz Hosting Rehberi

Projenizi ücretsiz olarak yayınlamak için birkaç seçeneğiniz var. Şu anda SQL Server kullanıyorsunuz, ücretsiz hosting'ler için PostgreSQL'e geçmeniz önerilir.

## 🚀 Seçenekler

### 1. **Render.com** (Önerilen - En Kolay)

**Avantajlar:**
- ✅ PostgreSQL ile ücretsiz tier (750 saat/ay)
- ✅ Kolay deployment (GitHub'dan otomatik)
- ✅ SSL sertifikası ücretsiz
- ✅ Kolay kurulum

**Kurulum:**
1. GitHub'da repository oluşturun
2. Render.com'a kaydolun (render.com)
3. "New Web Service" seçin
4. GitHub repository'yi bağlayın
5. PostgreSQL database ekleyin (ücretsiz)
6. Environment variables:
   - `ConnectionStrings__DefaultConnection`: PostgreSQL connection string (Render otomatik sağlar)

**Maliyet:** Ücretsiz (uygulama 15 dakika hareketsizlikten sonra uykuya geçer, sonraki istekte uyanır)

---

### 2. **Railway.app**

**Avantajlar:**
- ✅ PostgreSQL ile ücretsiz tier ($5 ücretsiz kredi/ay)
- ✅ Kolay deployment
- ✅ İyi performans

**Kurulum:**
1. Railway.app'e kaydolun
2. "New Project" → "Deploy from GitHub"
3. PostgreSQL ekleyin (ücretsiz)
4. Environment variables ayarlayın

**Maliyet:** $5 ücretsiz kredi/ay (yaklaşık küçük projeler için yeterli)

---

### 3. **Fly.io**

**Avantajlar:**
- ✅ PostgreSQL desteği
- ✅ Küçük projeler için ücretsiz
- ✅ İyi performans

**Kurulum:**
1. fly.io'ya kaydolun
2. CLI ile deploy edin

**Maliyet:** Ücretsiz tier mevcut

---

### 4. **Azure App Service** (SQL Server için)

**Avantajlar:**
- ✅ SQL Server desteği (veritabanını değiştirmenize gerek yok)
- ✅ Ücretsiz tier (F1)
- ⚠️ Sınırlı kaynaklar

**Kurulum:**
1. Azure hesabı oluşturun (ücretsiz trial)
2. App Service oluşturun
3. SQL Database ekleyin (küçük projeler için ücretsiz değil ama trial kredi ile başlayabilirsiniz)

**Maliyet:** F1 tier ücretsiz ama sınırlı kaynaklar

---

## 📋 Deployment İçin Yapılması Gerekenler

### Seçenek 1: PostgreSQL'e Geçiş (Render/Railway için)

1. **NuGet Paketlerini Güncelle:**
```xml
<!-- SQL Server yerine PostgreSQL -->
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.4" />
```

2. **Program.cs'i Güncelle:**
```csharp
// SQL Server yerine PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

3. **Migration'ları Yeniden Oluştur:**
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
```

4. **Connection String Formatı (PostgreSQL):**
```
Server=localhost;Database=KPSSStudyTrackerDb;Port=5432;User Id=user;Password=password;
```

### Seçenek 2: SQL Server ile Devam (Azure için)

Azure App Service kullanıyorsanız değişiklik yapmaya gerek yok, sadece connection string'i güncelleyin.

---

## 🔧 Deployment Adımları (Render.com Örneği)

### 1. GitHub'a Push Edin
```bash
git init
git add .
git commit -m "Initial commit"
git remote add origin https://github.com/kullaniciadi/KPSSStudyTracker.git
git push -u origin main
```

### 2. Render.com'da Servis Oluşturun

1. **Web Service Oluştur:**
   - Name: `kpss-study-tracker`
   - Environment: `.NET`
   - Build Command: `dotnet publish -c Release`
   - Start Command: `dotnet KPSSStudyTracker.dll`

2. **PostgreSQL Database Oluştur:**
   - Name: `kpss-db`
   - Render otomatik connection string sağlar

3. **Environment Variables Ekleyin:**
   - `ConnectionStrings__DefaultConnection`: PostgreSQL connection string (Render'dan kopyalayın)
   - `ASPNETCORE_ENVIRONMENT`: `Production`

### 3. Veritabanı Migration'ları Çalıştırın

Render deployment sırasında otomatik çalıştırabilir veya manuel:
- Start Command: `dotnet ef database update && dotnet KPSSStudyTracker.dll`

---

## 📝 Environment Variables Örneği

```env
ConnectionStrings__DefaultConnection=Server=localhost;Database=KPSSStudyTrackerDb;Port=5432;User Id=user;Password=password;
ASPNETCORE_ENVIRONMENT=Production
```

---

## 🎯 Hızlı Başlangıç (Render.com)

1. ✅ GitHub repository oluştur
2. ✅ Render.com hesabı aç
3. ✅ New → Web Service → GitHub repo seç
4. ✅ PostgreSQL database ekle
5. ✅ Environment variables ayarla
6. ✅ Deploy!

**Tahmini süre:** 10-15 dakika

---

## 💡 Öneriler

1. **PostgreSQL'e geçin** - Ücretsiz hosting'lerde daha kolay
2. **GitHub kullanın** - Deployment'ları kolaylaştırır
3. **Render.com başlayın** - En kolay seçenek
4. **Environment variables** kullanın - Connection string'leri güvenli tutun

---

## 🆘 Sorun Giderme

### Migration Hatası
```bash
# Render deployment command'e ekleyin:
dotnet ef database update --no-build
```

### Connection String Hatası
- Render'da PostgreSQL connection string'i doğru kopyaladığınızdan emin olun
- Environment variable adı: `ConnectionStrings__DefaultConnection` (çift alt çizgi önemli!)

### Uygulama Uyuyor (Render)
- Render ücretsiz tier'da 15 dakika hareketsizlikten sonra uykuya geçer
- İlk istekte 30-60 saniye uyanır
- Sürekli aktif tutmak için ücretli plan gerekir

---

## 📚 Yararlı Linkler

- [Render.com Documentation](https://render.com/docs)
- [Railway.app Documentation](https://docs.railway.app)
- [PostgreSQL .NET Provider](https://www.npgsql.org/efcore/)
- [Azure App Service](https://azure.microsoft.com/en-us/products/app-service/)

---

**Not:** En kolay ve ücretsiz seçenek **Render.com + PostgreSQL** kombinasyonudur. ⭐

