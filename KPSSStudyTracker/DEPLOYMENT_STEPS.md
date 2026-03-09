# 🚀 Ücretsiz Yayınlama Adımları - Render.com

PostgreSQL'i bilgisayarınıza kurmadan ücretsiz yayınlamak için **Render.com** kullanacağız. Render hem uygulamanızı hem de PostgreSQL veritabanını ücretsiz olarak sunuyor.

## 📋 Gereksinimler

1. ✅ GitHub hesabı (ücretsiz)
2. ✅ Render.com hesabı (ücretsiz)
3. ✅ 15-20 dakika zaman

---

## 🔧 Adım 1: Projeyi GitHub'a Yükleyin

### 1.1 GitHub'da Repository Oluşturun

1. https://github.com adresine gidin
2. "New repository" butonuna tıklayın
3. Repository adı: `KPSSStudyTracker` (veya istediğiniz isim)
4. Public veya Private seçin
5. "Create repository" butonuna tıklayın

### 1.2 Projeyi GitHub'a Push Edin

**Terminal/PowerShell'de şu komutları çalıştırın:**

```bash
# Proje klasörünüze gidin
cd "C:\Users\Firat\Desktop\KPSS Study Tracker\KPSSStudyTracker"

# Git repository'si başlatın (eğer yoksa)
git init

# Tüm dosyaları ekleyin
git add .

# İlk commit
git commit -m "Initial commit - PostgreSQL migration"

# GitHub repository'nizi ekleyin (URL'yi kendi repository'nizle değiştirin)
git remote add origin https://github.com/KULLANICI_ADINIZ/KPSSStudyTracker.git

# Branch adını main yapın
git branch -M main

# GitHub'a gönderin
git push -u origin main
```

**Not:** `KULLANICI_ADINIZ` yerine GitHub kullanıcı adınızı yazın.

---

## 🌐 Adım 2: Render.com'da Hesap Oluşturun

1. https://render.com adresine gidin
2. "Get Started for Free" butonuna tıklayın
3. GitHub hesabınızla giriş yapın (en kolay yöntem)
4. Render'a GitHub repository'lere erişim izni verin

---

## 🗄️ Adım 3: PostgreSQL Database Oluşturun

1. Render dashboard'da **"New +"** butonuna tıklayın
2. **"PostgreSQL"** seçin
3. Aşağıdaki bilgileri girin:
   - **Name:** `kpss-study-db` (veya istediğiniz isim)
   - **Database:** `KPSSStudyTrackerDb`
   - **User:** Render otomatik oluşturur (değiştirmeyin)
   - **Region:** Avrupa seçin (daha hızlı olabilir)
   - **PostgreSQL Version:** 16 (varsayılan)
   - **Plan:** **Free** ✅ (ücretsiz seçin)
4. **"Create Database"** butonuna tıklayın
5. Birkaç dakika bekleyin (database oluşturuluyor)

### 3.1 Connection String'i Kopyalayın

Database oluştuktan sonra:
1. Database sayfasına gidin
2. **"Connections"** sekmesine tıklayın
3. **"Internal Database URL"** veya **"External Connection String"** alanından connection string'i kopyalayın
4. Şu formatta olacak:
   ```
   postgresql://user:password@host:5432/database_name
   ```
5. **Bu connection string'i kaydedin, az sonra kullanacağız!** 📝

---

## 🚀 Adım 4: Web Service (Uygulama) Oluşturun

1. Render dashboard'da **"New +"** butonuna tıklayın
2. **"Web Service"** seçin
3. GitHub repository'nizi seçin (bağlantılı ise)
   - Eğer görünmüyorsa, repository'yi manuel bağlayın
4. Aşağıdaki ayarları yapın:

### 4.1 Temel Ayarlar
- **Name:** `kpss-study-tracker` (veya istediğiniz isim)
- **Region:** Avrupa seçin
- **Branch:** `main` (veya hangi branch'de kod varsa)
- **Root Directory:** (boş bırakın - otomatik bulur)
- **Runtime:** `.NET` (Render otomatik algılar)
- **Build Command:** 
   ```bash
   dotnet publish -c Release -o ./publish
   ```
- **Start Command:**
   ```bash
   dotnet ./publish/KPSSStudyTracker.dll
   ```
- **Plan:** **Free** ✅ (ücretsiz seçin)

### 4.2 Environment Variables (Çok Önemli!)

**"Environment Variables"** bölümüne tıklayın ve şunları ekleyin:

1. **Connection String:**
   - **Key:** `ConnectionStrings__DefaultConnection`
   - **Value:** PostgreSQL connection string (yukarıda kopyaladığınız)
   - **Örnek:**
     ```
     Host=dpg-xxxxx-a.frankfurt-postgres.render.com;Database=KPSSStudyTrackerDb;Username=user;Password=password;Port=5432;SSL Mode=Require
     ```
   - **Not:** Connection string'i PostgreSQL formatına çevirmeniz gerekebilir
   - **Format dönüşümü:**
     - Render'ın verdiği: `postgresql://user:password@host:5432/dbname`
     - .NET için: `Host=host;Database=dbname;Username=user;Password=password;Port=5432;SSL Mode=Require`

2. **Environment:**
   - **Key:** `ASPNETCORE_ENVIRONMENT`
   - **Value:** `Production`

### 4.3 Create Web Service

**"Create Web Service"** butonuna tıklayın

---

## ⏳ Adım 5: İlk Deployment

1. Render otomatik olarak:
   - ✅ Kodunuzu GitHub'dan çeker
   - ✅ .NET uygulamanızı build eder
   - ✅ Uygulamayı başlatır
   
2. **İlk build 3-5 dakika sürebilir** ⏰

3. Build sırasında logları izleyebilirsiniz:
   - "Events" sekmesinde build ilerlemesini görebilirsiniz
   - Hata varsa "Logs" sekmesinde görebilirsiniz

---

## 🗃️ Adım 6: Veritabanı Migration

Uygulama ilk çalıştığında veritabanı tablolarını oluşturması gerekiyor. İki seçenek var:

### Seçenek A: Otomatik (Önerilen)

Render'ın "Start Command" kısmını güncelleyin:

```
dotnet ef database update --project ./publish && dotnet ./publish/KPSSStudyTracker.dll
```

**Ama bu çalışmayabilir çünkü publish klasöründe migration dosyaları olmayabilir.**

### Seçenek B: Manuel (Daha Güvenilir)

1. Render dashboard'da uygulamanıza gidin
2. **"Shell"** sekmesine tıklayın (Render ücretsiz tier'da olmayabilir)
3. Veya **Render CLI** kullanın:
   ```bash
   # Render CLI kurulumu
   npm install -g render-cli
   
   # Login
   render login
   
   # Shell'e bağlan
   render shell
   ```

4. Migration komutu:
   ```bash
   cd /opt/render/project/src
   dotnet ef database update
   ```

### Seçenek C: En Kolay Yöntem ⭐

**appsettings.json** dosyasını güncelleyip migration'ı uygulama başlangıcında çalıştırabiliriz. Şimdilik uygulama çalıştığında hata verecektir, ama sonra düzeltebiliriz.

---

## 🔍 Adım 7: Test

1. Deployment tamamlandıktan sonra Render size bir URL verecek:
   - Örnek: `https://kpss-study-tracker.onrender.com`

2. Bu URL'yi tarayıcıda açın

3. Uygulama çalışıyorsa ✅
   - Login sayfası görünmeli
   - Admin: `admin` / Şifre: `admin123`

4. Eğer hata varsa:
   - "Logs" sekmesine bakın
   - Connection string doğru mu kontrol edin
   - Database migration yapıldı mı kontrol edin

---

## 📝 Connection String Dönüşüm Script'i

Render'ın PostgreSQL connection string formatı:
```
postgresql://user:password@host:5432/dbname
```

.NET için gerekli format:
```
Host=host;Database=dbname;Username=user;Password=password;Port=5432;SSL Mode=Require
```

**Manuel Dönüşüm:**
- `postgresql://` → Kaldır
- `user:password@` → `Username=user;Password=password;`
- `host:5432/` → `Host=host;Port=5432;`
- `dbname` → `Database=dbname;`
- Sonuna ekle: `;SSL Mode=Require`

---

## ⚠️ Önemli Notlar

1. **Ücretsiz Tier Sınırlamaları:**
   - Uygulama 15 dakika hareketsizlikten sonra uykuya geçer
   - İlk istekte 30-60 saniye uyanır (cold start)
   - PostgreSQL database 90 gün hareketsizlikten sonra silinebilir

2. **Sürekli Aktif Tutmak:**
   - Render ücretsiz tier'da "Always On" özelliği yok
   - Sürekli aktif tutmak için ücretli plan gerekir (~$7/ay)
   - Ama küçük projeler için ücretsiz yeterli!

3. **Database Migration:**
   - İlk deployment'ta migration yapılması gerekir
   - Render Shell kullanabilirsiniz veya migration'ı programatik yapabilirsiniz

---

## 🎯 Hızlı Başlangıç Checklist

- [ ] GitHub repository oluşturuldu
- [ ] Kod GitHub'a push edildi
- [ ] Render.com hesabı oluşturuldu
- [ ] PostgreSQL database oluşturuldu
- [ ] Connection string kopyalandı ve dönüştürüldü
- [ ] Web Service oluşturuldu
- [ ] Environment variables eklendi
- [ ] İlk deployment tamamlandı
- [ ] Migration yapıldı
- [ ] Uygulama çalışıyor ✅

---

## 🆘 Sorun Giderme

### Hata: "Unable to connect to database"
- Connection string doğru mu kontrol edin
- PostgreSQL database çalışıyor mu kontrol edin
- SSL Mode=Require eklediniz mi?

### Hata: "Table does not exist"
- Migration yapılmamış
- `dotnet ef database update` komutunu çalıştırın

### Uygulama Uyuyor
- Normal! Ücretsiz tier'da 15 dakika hareketsizlikten sonra uyur
- İlk istekte uyanır (30-60 saniye bekleyin)

---

**Tahmini Süre:** 15-20 dakika ⏰

**Maliyet:** Tamamen ÜCRETSİZ! 🎉

