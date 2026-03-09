# 🚀 KPSS Study Tracker - Ücretsiz Deployment Rehberi

## 🎯 En İyi Seçenekler

### 1️⃣ **Render.com** (ÖNERİLEN - TAMAMEN ÜCRETSİZ)
- ✅ **$0/ay** (750 saat ücretsiz)
- ✅ PostgreSQL dahil
- ✅ SSL sertifikası otomatik
- ✅ Kolay kurulum
- ❌ 15 dakika inactivity sonrası uyur (ilk açılış 30-50 saniye)

### 2️⃣ **Railway.app**
- ✅ $5 kredi/ay ücretsiz
- ✅ PostgreSQL dahil
- ✅ Çok hızlı
- ❌ Kredi bitince ücretli

### 3️⃣ **Fly.io**
- ✅ 3 VM ücretsiz
- ✅ PostgreSQL dahil
- ✅ Global CDN
- ❌ Command line gerektirir

---

## 📦 RENDER.COM İLE DEPLOYMENT

### Adım 1: GitHub'a Projeyi Yükle

```bash
cd "c:\Users\Firat\Desktop\KPSS Study Tracker"

# .gitignore ekle
echo "bin/
obj/
*.user
.vs/
appsettings.Development.json
*.db" > .gitignore

# Git başlat
git init
git add .
git commit -m "Initial commit"

# GitHub'da repository oluştur (kpss-study-tracker)
# Sonra:
git remote add origin https://github.com/KULLANICI_ADI/kpss-study-tracker.git
git branch -M main
git push -u origin main
```

### Adım 2: Render.com Hesabı

1. **[render.com](https://render.com)** → Sign Up (GitHub ile)
2. GitHub repository erişimi ver

### Adım 3: Blueprint ile Otomatik Deploy

**render.yaml** dosyası projenizde hazır! Render otomatik algılayacak.

1. Render Dashboard → **New** → **Blueprint**
2. GitHub repo seç: `kpss-study-tracker`
3. **Apply** → Render otomatik olarak:
   - PostgreSQL veritabanı oluşturur
   - Web service deploy eder
   - Environment variables ayarlar

### Adım 4: URL'i Al

Deploy tamamlandığında:
- URL: `https://kpss-study-tracker.onrender.com`
- İlk açılış: 30-50 saniye (cold start)
- Sonraki açılışlar: hızlı

---

## 🔧 RAILWAY.APP İLE DEPLOYMENT

### Adım 1: Railway Hesabı

1. **[railway.app](https://railway.app)** → Sign Up (GitHub ile)
2. $5 ücretsiz kredi alacaksınız

### Adım 2: Deploy

```bash
# Railway CLI kur
npm install -g @railway/cli

# Login
railway login

# Proje oluştur
cd "c:\Users\Firat\Desktop\KPSS Study Tracker\KPSSStudyTracker"
railway init

# PostgreSQL ekle
railway add postgresql

# Deploy
railway up
```

### Adım 3: Environment Variables

Railway Dashboard → Variables:
```
ASPNETCORE_ENVIRONMENT = Production
ConnectionStrings__DefaultConnection = ${{Postgres.DATABASE_URL}}
```

### Adım 4: Domain

Railway otomatik bir domain verir:
- `kpss-study-tracker.up.railway.app`

---

## 🪂 FLY.IO İLE DEPLOYMENT

### Adım 1: Fly CLI Kur

```bash
# Windows
powershell -Command "iwr https://fly.io/install.ps1 -useb | iex"

# Login
flyctl auth login
```

### Adım 2: Deploy

```bash
cd "c:\Users\Firat\Desktop\KPSS Study Tracker\KPSSStudyTracker"

# Fly app oluştur
flyctl launch --name kpss-study-tracker --region fra

# PostgreSQL ekle
flyctl postgres create --name kpss-db --region fra

# Attach database
flyctl postgres attach --app kpss-study-tracker kpss-db

# Deploy
flyctl deploy
```

---

## 🆓 DİĞER ÜCRETSİZ SEÇENEKLER

### **Azure App Service** (Öğrenciyseniz)
- $100 kredi/yıl (GitHub Student Pack)
- PostgreSQL dahil
- [education.github.com/pack](https://education.github.com/pack)

### **Heroku Alternatives**
- **Koyeb**: 512MB RAM ücretsiz
- **Vercel**: Frontend için harika (backend sınırlı)
- **Netlify**: Statik siteler için

---

## ⚙️ DEPLOYMENT SORUN GİDERME

### ❌ "Cold Start" Çok Uzun

**Çözüm (Render):**
```bash
# Cron job ile canlı tut (15 dakikada bir ping)
curl https://kpss-study-tracker.onrender.com
```

Veya ücretsiz cron servisler:
- [cron-job.org](https://cron-job.org)
- [uptimerobot.com](https://uptimerobot.com)

### ❌ Database Connection Failed

**Çözüm:**
1. Environment variable'ı kontrol et
2. Database URL doğru mu?
3. SSL mode ekle: `SSL Mode=Require`

### ❌ Migration Hatası

**Çözüm:**
```bash
# Render Shell'de:
cd /app
dotnet ef database update
```

---

## 📊 PERFORMANS KARŞILAŞTIRMA

| Platform | Ücretsiz Limit | Cold Start | Hız | Kolay Kurulum |
|----------|---------------|------------|-----|---------------|
| Render | 750h/ay | 30-50s | ⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| Railway | $5 kredi | Yok | ⭐⭐⭐⭐⭐ | ⭐⭐⭐⭐⭐ |
| Fly.io | 3 VM | 10-20s | ⭐⭐⭐⭐ | ⭐⭐⭐ |

---

## 🎯 ÖNERİ

**Render.com kullanın!**
- Tamamen ücretsiz
- PostgreSQL dahil
- Kolay kurulum
- SSL otomatik
- GitHub entegrasyonu

**Hızlı başlangıç:**
1. GitHub'a push
2. Render'da Blueprint deploy
3. 5 dakikada hazır!

---

## 📞 DEPLOYMENT SONRASI

### İlk Giriş

1. `https://kpss-study-tracker.onrender.com` aç
2. İlk açılış 30-50 saniye bekle (cold start)
3. Login: `admin` / `admin123`
4. Admin şifresini değiştir!

### Özel Domain (Opsiyonel)

Kendi domain'iniz varsa:
1. Render Dashboard → Settings → Custom Domain
2. DNS'e CNAME ekle: `yourdomain.com` → `kpss-study-tracker.onrender.com`

### SSL Sertifikası

Render otomatik Let's Encrypt SSL sertifikası verir (ücretsiz).

---

## 📚 EK KAYNAKLAR

- [Render Docs](https://render.com/docs)
- [Railway Docs](https://docs.railway.app)
- [Fly.io Docs](https://fly.io/docs)
- [ASP.NET Core Deployment](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/)

---

## ✅ SONUÇ

**3 basit adımda ücretsiz deploy:**
1. ✅ GitHub'a push
2. ✅ Render.com'da blueprint deploy
3. ✅ Hazır!

**URL:** `https://kpss-study-tracker.onrender.com`

**Maliyet:** $0 💰

**Süre:** 5 dakika ⚡
