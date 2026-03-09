# 🚀 Render.com Setup - Adım Adım

## 📋 1. PostgreSQL Database Oluşturma

1. Render Dashboard → **"New +"** butonuna tıklayın
2. **"Postgres"** seçin
3. Form doldurun:
   - **Name:** `kpss-study-db` (veya istediğiniz isim)
   - **Database:** `KPSSStudyTrackerDb`
   - **User:** Render otomatik oluşturur (değiştirmeyin)
   - **Region:** Avrupa seçin (opsiyonel)
   - **PostgreSQL Version:** 16 (varsayılan)
   - **Plan:** **Free** ✅
4. **"Create Database"** butonuna tıklayın
5. **2-3 dakika bekleyin** ⏰

### Connection String Kopyalama

Database oluştuktan sonra:

1. Database sayfasına gidin (oluşturduğunuz database'e tıklayın)
2. **"Connections"** sekmesine tıklayın
3. **"Internal Database URL"** kopyalayın
   - Format: `postgresql://user:password@host:5432/dbname`
   - Örnek: `postgresql://kpss_study_db_user:abc123@dpg-xxxxx-a.frankfurt-postgres.render.com:5432/kpssstudytrackerdb`
4. **Bu connection string'i kaydedin!** 📝

### Connection String Dönüşümü

Render'ın verdiği format:
```
postgresql://user:password@host:5432/dbname
```

.NET için gerekli format:
```
Host=host;Database=dbname;Username=user;Password=password;Port=5432;SSL Mode=Require
```

**Manuel Dönüşüm Örneği:**

Render'dan gelen:
```
postgresql://kpss_study_db_user:abc123xyz@dpg-xxxxx-a.frankfurt-postgres.render.com:5432/kpssstudytrackerdb
```

.NET için:
```
Host=dpg-xxxxx-a.frankfurt-postgres.render.com;Database=kpssstudytrackerdb;Username=kpss_study_db_user;Password=abc123xyz;Port=5432;SSL Mode=Require
```

**Adım Adım:**
1. `postgresql://` kaldır
2. `user:password@` → `Username=user;Password=password;`
3. `host:5432/` → `Host=host;Port=5432;`
4. `dbname` → `Database=dbname;`
5. Sonuna `;SSL Mode=Require` ekle

---

## 📋 2. Web Service Oluşturma

1. Render Dashboard → **"New +"** butonuna tıklayın
2. **"Web Service"** seçin
3. **"Connect a repository"** bölümünde:
   - GitHub repository'nizi seçin: `firat404engin/KPSSStudyTracker`
   - Eğer görünmüyorsa: "Configure account" → GitHub'ı bağlayın

### 2.1 Temel Ayarlar

- **Name:** `kpss-study-tracker` (veya istediğiniz isim)
- **Region:** Avrupa (veya size yakın)
- **Branch:** `main`
- **Root Directory:** (boş bırakın)
- **Runtime:** `.NET` (Render otomatik algılar)

### 2.2 Build & Start Commands

- **Build Command:**
  ```
  dotnet publish -c Release -o ./publish
  ```

- **Start Command:**
  ```
  dotnet ./publish/KPSSStudyTracker.dll
  ```

### 2.3 Plan

- **Plan:** **Free** ✅

### 2.4 Environment Variables (Çok Önemli!)

**"Environment"** sekmesine tıklayın ve şunları ekleyin:

#### Variable 1: Connection String
- **Key:** `ConnectionStrings__DefaultConnection`
- **Value:** Dönüştürülmüş PostgreSQL connection string (yukarıdaki)
- **Önemli:** Çift alt çizgi (`__`) kullanın!

#### Variable 2: Environment
- **Key:** `ASPNETCORE_ENVIRONMENT`
- **Value:** `Production`

### 2.5 Deploy!

**"Create Web Service"** butonuna tıklayın.

---

## ⏳ 3. İlk Deployment

1. Render otomatik olarak:
   - ✅ GitHub'dan kodunuzu çeker
   - ✅ .NET uygulamanızı build eder
   - ✅ Uygulamayı başlatır

2. **Build süresi: 3-5 dakika** ⏰

3. Build sırasında:
   - **"Events"** sekmesinde ilerleme görünür
   - **"Logs"** sekmesinde detaylar görünür

4. Build tamamlandıktan sonra:
   - Uygulama otomatik başlar
   - **Migration'lar otomatik çalışır** (Program.cs'de ayarladık)
   - **SeedData çalışır** (admin kullanıcısı ve tüm konular eklenir)

---

## 🔍 4. Test

1. Deployment tamamlandıktan sonra:
   - Render size bir URL verecek
   - Örnek: `https://kpss-study-tracker.onrender.com`

2. URL'yi tarayıcıda açın

3. Uygulama çalışıyorsa ✅:
   - Login sayfası görünmeli
   - **Username:** `admin`
   - **Password:** `admin123`

4. Eğer hata varsa:
   - **"Logs"** sekmesine bakın
   - Connection string doğru mu kontrol edin
   - Environment variables doğru mu kontrol edin

---

## ⚠️ Önemli Notlar

### Connection String Formatı

Render'ın PostgreSQL connection string formatı:
```
postgresql://user:password@host:5432/dbname
```

.NET/Npgsql için gerekli format:
```
Host=host;Database=dbname;Username=user;Password=password;Port=5432;SSL Mode=Require
```

**Dönüşüm Script'i (Manuel):**

Render'dan gelen:
```
postgresql://kpss_study_db_user:abc123xyz@dpg-xxxxx-a.frankfurt-postgres.render.com:5432/kpssstudytrackerdb
```

.NET için (Adım adım):
1. `postgresql://` kaldır
2. `kpss_study_db_user:abc123xyz@` → `Username=kpss_study_db_user;Password=abc123xyz;`
3. `dpg-xxxxx-a.frankfurt-postgres.render.com:5432/` → `Host=dpg-xxxxx-a.frankfurt-postgres.render.com;Port=5432;`
4. `kpssstudytrackerdb` → `Database=kpssstudytrackerdb;`
5. Sonuna ekle: `;SSL Mode=Require`

**Sonuç:**
```
Host=dpg-xxxxx-a.frankfurt-postgres.render.com;Database=kpssstudytrackerdb;Username=kpss_study_db_user;Password=abc123xyz;Port=5432;SSL Mode=Require
```

### Environment Variable Adı

**Çift alt çizgi (`__`) önemli!**
- ✅ Doğru: `ConnectionStrings__DefaultConnection`
- ❌ Yanlış: `ConnectionStrings_DefaultConnection`
- ❌ Yanlış: `ConnectionStrings.DefaultConnection`

---

## 🆘 Sorun Giderme

### Hata: "Unable to connect to database"
1. Connection string doğru mu kontrol edin
2. PostgreSQL database çalışıyor mu kontrol edin
3. `SSL Mode=Require` eklediniz mi?
4. Environment variable adı doğru mu? (`ConnectionStrings__DefaultConnection`)

### Hata: "Table does not exist"
- Migration'lar çalışmamış olabilir
- Logs'a bakın, migration hatası var mı?
- Uygulama yeniden başlatılırsa migration otomatik çalışır

### Uygulama Uyuyor
- Normal! Ücretsiz tier'da 15 dakika hareketsizlikten sonra uyur
- İlk istekte 30-60 saniye uyanır (cold start)

### Build Başarısız
- Logs'a bakın
- Connection string formatı doğru mu?
- Environment variables doğru mu?

---

## ✅ Checklist

- [ ] PostgreSQL database oluşturuldu
- [ ] Connection string kopyalandı
- [ ] Connection string .NET formatına dönüştürüldü
- [ ] Web Service oluşturuldu
- [ ] GitHub repository bağlandı
- [ ] Build Command ayarlandı
- [ ] Start Command ayarlandı
- [ ] Environment variables eklendi (`ConnectionStrings__DefaultConnection`)
- [ ] Environment variable eklendi (`ASPNETCORE_ENVIRONMENT=Production`)
- [ ] İlk deployment tamamlandı
- [ ] Uygulama çalışıyor ✅
- [ ] Login yapılabiliyor ✅

---

**Tahmini Süre:** 15-20 dakika ⏰

**Maliyet:** Tamamen ÜCRETSİZ! 🎉


