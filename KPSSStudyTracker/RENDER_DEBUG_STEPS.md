# 🔍 Render Debug Adımları - Development Mode Hatası

## ⚠️ Durum
Uygulama açılıyor ama Development Mode hatası gösteriyor. Bu, gerçek bir exception olduğunu gösterir.

---

## 🔍 Adım 1: Logs Kontrolü (ÖNEMLİ!)

1. **Render Dashboard** → Web Service'inize gidin
2. **"Logs"** sekmesine tıklayın
3. **En son logları** okuyun

**Aramanız gereken hatalar:**
- ❌ "Exception"
- ❌ "Error"
- ❌ "Failed"
- ❌ "Connection"
- ❌ "Migration"
- ❌ "An error occurred"

**Logs'u buraya kopyalayın** (son 50-100 satır)

---

## ✅ Adım 2: Environment Variables Kontrolü

Render Dashboard → Web Service → **"Environment"** sekmesinde şunlar var mı kontrol edin:

### Zorunlu Environment Variables:

1. **`ASPNETCORE_ENVIRONMENT`**
   - **Value:** `Production`
   - ❌ Eğer yoksa veya `Development` ise bu hata olur!

2. **`ConnectionStrings__DefaultConnection`**
   - **Value:** PostgreSQL connection string
   - **Format:** `Host=...;Database=...;Username=...;Password=...;Port=5432;SSL Mode=Require`

**Önemli:** `ConnectionStrings__DefaultConnection` → Çift alt çizgi (`__`) kullanın!

---

## 🔧 Adım 3: Environment Variable Ekleme/Düzeltme

Eğer `ASPNETCORE_ENVIRONMENT` yoksa veya yanlışsa:

1. **"Environment"** sekmesine gidin
2. **"Add Environment Variable"** tıklayın
3. **Key:** `ASPNETCORE_ENVIRONMENT`
4. **Value:** `Production`
5. **"Save"** tıklayın
6. **Uygulama otomatik yeniden deploy olacak**

---

## 🐛 Yaygın Hatalar ve Çözümleri

### Hata 1: "Connection string" hatası
```
❌ An error occurred while migrating or seeding the database.
❌ Connection refused
```

**Çözüm:**
- `ConnectionStrings__DefaultConnection` kontrol edin
- PostgreSQL database'iniz çalışıyor mu kontrol edin (Render Dashboard)
- Connection string format'ı doğru mu kontrol edin

### Hata 2: "Migration" hatası
```
❌ syntax error at or near "IF"
❌ relation "xxxxx" does not exist
```

**Çözüm:**
- Bu hatayı düzelttik (SQL Server syntax kaldırıldı)
- Yeni deploy yapıldı mı kontrol edin

### Hata 3: "An error occurred while migrating or seeding"
```
❌ An error occurred while migrating or seeding the database.
```

**Çözüm:**
- Logs'taki detaylı hata mesajını bulun
- Muhtemelen connection string veya migration sorunu

---

## 📋 Yapılacaklar Checklist

- [ ] Logs sekmesine gidildi
- [ ] Son 50-100 satır log okundu
- [ ] Hata mesajları bulundu
- [ ] `ASPNETCORE_ENVIRONMENT` = `Production` kontrol edildi
- [ ] `ConnectionStrings__DefaultConnection` kontrol edildi
- [ ] Logs'taki gerçek hata mesajı not edildi

---

## 🆘 Sonraki Adım

**Logs'taki gerçek hata mesajını paylaşın!**

Logs'ta şunları arayın ve paylaşın:
1. "Exception" kelimesi içeren satırlar
2. "Error" kelimesi içeren satırlar
3. "Failed" kelimesi içeren satırlar
4. Stack trace varsa (detaylı hata bilgisi)

Bu bilgilerle sorunu tam olarak tespit edip çözebiliriz! 🔍


