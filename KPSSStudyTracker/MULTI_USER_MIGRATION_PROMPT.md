# Çok Kullanıcılı Sisteme Geçiş İçin Prompt

Aşağıdaki prompt'u kullanarak projeni çok kullanıcılı sisteme dönüştürebilirsin:

---

**PROMPT:**

Benim KPSS Study Tracker projemi çok kullanıcılı bir sisteme dönüştürmeni istiyorum. Şu anda sistem tek kullanıcılı çalışıyor ve tüm veriler global olarak saklanıyor. Her kullanıcının kendi bireysel verilerine sahip olmasını istiyorum.

**Yapılması Gerekenler:**

1. **Database Modellerini Güncelle:**
   - `Topic`, `MockExam`, `MockExamResult`, `StudyPlan`, `WeeklyPlan`, `DailyPlan`, `PlanTopic`, `DailyTodo` modellerine `UserId` foreign key ekle
   - Her kullanıcı sadece kendi Topic'lerini, Denemelerini, Planlarını ve Todo'larını görebilmeli
   - `Lesson` ve `MotivationQuote` modelleri global kalabilir (tüm kullanıcılar için ortak)

2. **AppDbContext'i Güncelle:**
   - Tüm ilişkileri UserAccount ile bağla
   - Cascade delete ayarlarını düzenle (kullanıcı silindiğinde kendi verileri de silinsin)

3. **Tüm Sayfaları Güncelle:**
   - Her PageModel'de `GetCurrentUserId()` helper metodu ekle (Claims'den kullanıcı ID'sini alsın)
   - Tüm veritabanı sorgularını kullanıcıya göre filtrele
   - Yeni veri eklerken otomatik olarak kullanıcı ID'sini ekle
   - Etkilenen sayfalar:
     - `/Pages/Lessons/*` - Kullanıcıya özel konular
     - `/Pages/Exams/*` - Kullanıcıya özel denemeler
     - `/Pages/StudyPlan/*` - Kullanıcıya özel planlar
     - `/Pages/Todo/*` - Kullanıcıya özel todo listeleri
     - `/Pages/Statistics/*` - Kullanıcıya özel istatistikler
     - `/Pages/Index.cshtml.cs` - Ana sayfa kullanıcıya özel verileri göstersin

4. **Migration Oluştur:**
   - Yeni migration oluştur: `AddUserIdToUserSpecificTables`
   - Mevcut veriler için varsayılan kullanıcı ID'si atama (örneğin admin kullanıcısına)
   - Veritabanı şemasını güncelle

5. **Kullanıcı Yönetimi:**
   - Kullanıcı kayıt sayfası ekle (Register sayfası)
   - Her kullanıcı kendi hesabını oluşturabilsin
   - Şifre hash'leme mevcut sistemi korun (SHA256)

6. **Güvenlik:**
   - Tüm sayfalarda `[Authorize]` attribute'unu kontrol et
   - Kullanıcılar sadece kendi verilerine erişebilsin (başkasının ID'sini manipüle edememeli)

**Önemli Notlar:**
- Mevcut veriler kaybolmasın, migration ile admin kullanıcısına atansın
- `Lesson` (Dersler) tablosu global kalacak, herkes aynı dersleri görecek
- `MotivationQuote` tablosu global kalacak
- Her kullanıcı kendi Topic'lerini oluşturabilmeli, kendi denemelerini girebilmeli, kendi planlarını yapabilmeli

**Teknoloji Stack:**
- ASP.NET Core Razor Pages
- Entity Framework Core
- SQL Server
- Cookie Authentication

Lütfen bu dönüşümü yap ve tüm kodları güncelle.

---

**Kullanım:**
Bu prompt'u doğrudan AI'a gönderebilirsin. Tüm gerekli değişiklikleri yapacak.




