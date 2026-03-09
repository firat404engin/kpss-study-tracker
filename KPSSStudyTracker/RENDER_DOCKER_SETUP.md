# 🐳 Render.com Docker Setup

## ✅ Dockerfile Eklendi!

Dockerfile oluşturuldu ve GitHub'a push edildi. Şimdi Render'da ayarları güncelleyin.

---

## 🔧 Render'da Yapılacaklar

### 1. Language Değiştir

**Şu anda:** `Node` ❌

**Olması gereken:**
- **Language:** `Docker` seçin

**Nasıl:**
1. Web Service'inizin "Settings" sekmesine gidin
2. Language dropdown'unu açın
3. **`Docker`** seçin

### 2. Build Command Değiştir

**Şu anda:** `dotnet publish -c Release -o ./publish` veya başka bir komut ❌

**Olması gereken:**
- **Build Command:** Boş bırakın (hiçbir şey yazmayın)

**Nasıl:**
1. Build Command alanına gidin
2. İçeriği tamamen silin
3. Boş bırakın

**Önemli:** Docker kullanırken build komutu Dockerfile'da tanımlıdır, bu alan boş olmalı!

### 3. Start Command Değiştir

**Şu anda:** `yarn start` ❌

**Olması gereken:**
- **Start Command:** Boş bırakın (hiçbir şey yazmayın)

**Nasıl:**
1. Start Command alanına gidin
2. İçeriği tamamen silin (şu anki `yarn start` komutunu silin)
3. Boş bırakın

**Önemli:** Docker kullanırken start komutu Dockerfile'da tanımlıdır (ENTRYPOINT), bu alan boş olmalı!

### 4. Environment Variables Kontrol

Aşağıdaki environment variables'ların ekli olduğundan emin olun:

1. **`ConnectionStrings__DefaultConnection`**
   - Değer: PostgreSQL connection string (dönüştürülmüş)
   
2. **`ASPNETCORE_ENVIRONMENT`**
   - Değer: `Production`

### 5. Instance Type Kontrol

- **Instance Type:** `Free` seçili olmalı ✅

---

## 📋 Doğru Ayarlar Özeti

| Alan | Değer |
|------|-------|
| Language | **Docker** ✅ |
| Branch | `main` ✅ |
| Region | Oregon veya Europe ✅ |
| Root Directory | (boş) ✅ |
| **Build Command** | **(BOŞ - hiçbir şey yazma)** ✅ |
| **Start Command** | **(BOŞ - hiçbir şey yazma)** ✅ |
| Instance Type | **Free** ✅ |
| Environment Variables | `ConnectionStrings__DefaultConnection` (PostgreSQL) ✅ |
| | `ASPNETCORE_ENVIRONMENT` = `Production` ✅ |

---

## 💾 Değişiklikleri Kaydet

1. Tüm ayarları yaptıktan sonra **"Save Changes"** butonuna tıklayın
2. Render otomatik olarak yeniden deploy edecek
3. Veya **"Manual Deploy"** → **"Deploy latest commit"** butonuna tıklayın

---

## 🚀 İlk Docker Build

- İlk Docker build **5-7 dakika** sürebilir ⏰
- Dockerfile'daki tüm katmanlar build edilecek
- Sonraki build'ler daha hızlı olacak (cached layers)

---

## 🔍 Build Sırasında

1. **"Logs"** sekmesine bakın
2. Docker build ilerlemesini görebilirsiniz
3. Örnek loglar:
   ```
   ==> Building Docker image...
   ==> Pulling base image mcr.microsoft.com/dotnet/sdk:8.0
   ==> Building application...
   ==> Publishing application...
   ==> Creating runtime image...
   ```

---

## ✅ Başarılı Build

Build başarılı olduktan sonra:
- Uygulama otomatik başlar
- Migration'lar otomatik çalışır (Program.cs'de ayarladık)
- SeedData çalışır (admin kullanıcısı ve tüm konular)
- URL verilir (örnek: `https://kpss-studytracker.onrender.com`)

---

## 🆘 Sorun Giderme

### Hata: "docker: command not found"
- Render'da Docker desteği aktif mi kontrol edin
- Language'ı `Docker` olarak seçtiğinizden emin olun

### Hata: "Cannot connect to Docker daemon"
- Render otomatik Docker kullanır, bu hata normalde görünmez
- Eğer görürseniz Render support'a başvurun

### Hata: "build failed"
- Logs'a bakın, hangi adımda hata veriyor?
- Dockerfile doğru mu kontrol edin
- Environment variables doğru mu kontrol edin

### Build Çok Uzun Sürüyor
- Normal! İlk Docker build 5-7 dakika sürebilir
- Sabırlı olun, sonraki build'ler daha hızlı olacak

---

## ✅ Checklist

- [ ] Language: `Docker` seçildi
- [ ] Build Command: **BOŞ** (silindi)
- [ ] Start Command: **BOŞ** (silindi - yarn start kaldırıldı)
- [ ] Instance Type: `Free` seçili
- [ ] Environment Variables: `ConnectionStrings__DefaultConnection` (PostgreSQL)
- [ ] Environment Variables: `ASPNETCORE_ENVIRONMENT` = `Production`
- [ ] "Save Changes" tıklandı
- [ ] Deploy başladı
- [ ] Logs'da Docker build görünüyor ✅

---

**Önemli:** Build ve Start Command'ları **TAMAMEN BOŞ** bırakın! Dockerfile otomatik olarak kullanılacak. 🐳


