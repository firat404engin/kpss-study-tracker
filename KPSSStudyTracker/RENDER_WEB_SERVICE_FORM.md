# 🚀 Render.com Web Service Form - Adım Adım

## ✅ Form Doldurma Rehberi

### 1. Name (Zorunlu)
```
KPSSStudyTracker
```
✅ İyi görünüyor - istediğiniz isim olabilir

### 2. Project (Opsiyonel)
- Boş bırakabilirsiniz veya bir project seçin

### 3. Environment (Opsiyonel)
- Boş bırakabilirsiniz

### 4. Language ⚠️ (ÖNEMLİ - DEĞİŞTİRİN!)

**Şu anda:** `Node` ❌

**Olması gereken:** `Other` seçin

**Not:** Render'da `.NET` seçeneği yok! Şunlar var:
- Docker
- Elixir
- Go
- Node
- Python 3
- Ruby
- Rust

**.NET için ne yapmalı:**
1. Language dropdown'unu açın
2. **`Other`** seçeneğini seçin (eğer varsa)
3. VEYA hiçbir şey seçmeyin/boş bırakın - Render `.csproj` dosyasını görünce otomatik `.NET` algılar!

**Önemli:** Render `.csproj` dosyanızı görünce otomatik olarak `.NET` runtime'ını kullanır, Language seçimi kritik değil. Ama `Node` seçili kalırsa build komutları yanlış olur, o yüzden `Other` seçin veya boş bırakın.

### 5. Branch
```
main
```
✅ Doğru - zaten ayarlanmış

### 6. Region
- Oregon (US West) - Mevcut servislerinizle aynı ✅
- Veya Europe seçin (Türkiye'ye daha yakın)

### 7. Root Directory (Opsiyonel)
- Boş bırakın

### 8. Build Command ⚠️ (ÖNEMLİ - DEĞİŞTİRİN!)

**Şu anda:** `yarn` ❌

**Olması gereken:**
```
dotnet publish -c Release -o ./publish
```

**Nasıl değiştirilir:**
1. Build Command alanına tıklayın
2. İçeriği silin
3. Şunu yazın: `dotnet publish -c Release -o ./publish`

### 9. Start Command ⚠️ (ÖNEMLİ - DEĞİŞTİRİN!)

**Şu anda:** `yarn start` ❌

**Olması gereken:**
```
dotnet ./publish/KPSSStudyTracker.dll
```

**Nasıl değiştirilir:**
1. Start Command alanına tıklayın
2. İçeriği silin
3. Şunu yazın: `dotnet ./publish/KPSSStudyTracker.dll`

### 10. Instance Type ⚠️ (ÖNEMLİ - KONTROL EDİN!)

**Seçmeniz gereken:**
- **Free** ✅ ($0/month)
- 512 MB (RAM)
- 0.1 CPU

**Kontrol:** Free seçili mi kontrol edin

### 11. Environment Variables ✅ (İYİ GÖRÜNÜYOR!)

Environment Variables bölümünde:

1. **ConnectionStrings__DefaultConnection** ✅
   - Değer gizli görünüyor (•••••••••••) ✅
   - Bu doğru - değer eklenmiş

2. **ASPNETCORE_ENVIRONMENT** ✅
   - Değer gizli görünüyor (•••••••••••) ✅
   - Bu doğru - değer eklenmiş

**Önemli:** Environment variable değerlerinin doğru olduğundan emin olun:
- `ConnectionStrings__DefaultConnection` = Dönüştürülmüş PostgreSQL connection string
- `ASPNETCORE_ENVIRONMENT` = `Production`

### 12. "There's an error above" Hatası

Bu hata muhtemelen şunlardan kaynaklanıyor:
1. ❌ Language: `Node` olarak ayarlanmış (`.NET` olmalı)
2. ❌ Build Command: `yarn` (dotnet komutu olmalı)
3. ❌ Start Command: `yarn start` (dotnet komutu olmalı)

**Çözüm:**
1. Language'ı `.NET` veya `Other` yapın
2. Build Command'ı `dotnet publish -c Release -o ./publish` yapın
3. Start Command'ı `dotnet ./publish/KPSSStudyTracker.dll` yapın

Hatalar düzeltildikten sonra hata kaybolacak!

---

## ✅ Doğru Form Ayarları Özeti

| Alan | Değer |
|------|-------|
| Name | `KPSSStudyTracker` |
| Project | (boş) |
| Environment | (boş) |
| Language | `.NET` veya `Other` |
| Branch | `main` |
| Region | Oregon veya Europe |
| Root Directory | (boş) |
| Build Command | `dotnet publish -c Release -o ./publish` |
| Start Command | `dotnet ./publish/KPSSStudyTracker.dll` |
| Instance Type | **Free** |
| Environment Variables | `ConnectionStrings__DefaultConnection` (PostgreSQL connection string) |
| | `ASPNETCORE_ENVIRONMENT` = `Production` |

---

## 🎯 Son Adım

Tüm hatalar düzeltildikten sonra:
1. Formun altında **"Deploy web service"** butonuna tıklayın
2. Build başlayacak (3-5 dakika)
3. İlk deployment'ta otomatik olarak:
   - ✅ Migration'lar çalışacak
   - ✅ Veritabanı tabloları oluşturulacak
   - ✅ SeedData çalışacak (admin kullanıcısı ve tüm konular)

---

## 🔍 Kontrol Listesi

- [ ] Language `.NET` veya `Other` olarak ayarlandı
- [ ] Build Command: `dotnet publish -c Release -o ./publish`
- [ ] Start Command: `dotnet ./publish/KPSSStudyTracker.dll`
- [ ] Instance Type: **Free** seçili
- [ ] Environment Variable: `ConnectionStrings__DefaultConnection` (PostgreSQL connection string)
- [ ] Environment Variable: `ASPNETCORE_ENVIRONMENT` = `Production`
- [ ] "There's an error above" hatası kayboldu ✅
- [ ] "Deploy web service" butonu aktif ✅

Hepsi tamamlandığında deploy edin! 🚀

