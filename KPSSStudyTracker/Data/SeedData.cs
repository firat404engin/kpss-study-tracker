using KPSSStudyTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace KPSSStudyTracker.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await ctx.Database.MigrateAsync();

            if (!await ctx.Users.AnyAsync())
            {
                var user = new UserAccount
                {
                    Username = "admin",
                    PasswordHash = ComputeSha256("admin123"),
                    IsAdmin = true
                };
                ctx.Users.Add(user);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to save admin user: {ex.Message}", ex);
                }
            }

            // Seed temel dersler
            if (!await ctx.Lessons.AnyAsync())
            {
                var basicLessons = new[]
                {
                    new Lesson { Name = "Türkçe" },
                    new Lesson { Name = "Matematik" },
                    new Lesson { Name = "Tarih" },
                    new Lesson { Name = "Coğrafya" },
                    new Lesson { Name = "Vatandaşlık" }
                };
                ctx.Lessons.AddRange(basicLessons);
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to save lessons: {ex.Message}", ex);
                }
            }

            if (!await ctx.MotivationQuotes.AnyAsync())
            {
                ctx.MotivationQuotes.AddRange(
                    new MotivationQuote { Text = "Başarı, küçük çabaların tekrarıdır.", Author = "Robert Collier" },
                    new MotivationQuote { Text = "Yapabileceğine inananlar, yapamayanlardan daha iyidir.", Author = "Henry Ford" },
                    new MotivationQuote { Text = "Gelecek, hayallerinin güzelliğine inananlara aittir.", Author = "Eleanor Roosevelt" }
                );
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to save motivation quotes: {ex.Message}", ex);
                }
            }

            // Statik StudyPlan verileri artık seed edilmiyor (dinamik plana geçildi)

            // Seed Tarih Dersi ve Konuları
            var tarihDersi = await ctx.Lessons.FirstOrDefaultAsync(l => l.Name == "Tarih");
            if (tarihDersi == null)
            {
                tarihDersi = new Lesson { Name = "Tarih" };
                ctx.Lessons.Add(tarihDersi);
                await ctx.SaveChangesAsync();
            }

            // Tarih konularını kontrol et ve eksik olanları ekle
            var mevcutKonular = await ctx.Topics.Where(t => t.LessonId == tarihDersi.Id).ToListAsync();
            
            // Source ve Notes mapping dictionary
            var tarihSourceNotesMap = new Dictionary<string, (string Source, string Notes)>
            {
                { "Kamp Programı ve Kitap İçeriği", ("KPSS Tarih", "Genel bakış ve program tanıtımı") },
                { "Sıkça Sorulan Sorular", ("KPSS Tarih", "SSS bölümü") },
                { "Tarih Dersine Nasıl Çalışmalıyım", ("KPSS Tarih", "Çalışma stratejileri") },
                { "İslamiyet Öncesi Türk Tarihi Siyasi Tarih - I", ("KPSS Tarih", "İlk Türk devletleri") },
                { "İslamiyet Öncesi Türk Tarihi Siyasi Tarih - II", ("KPSS Tarih", "Türk devletleri devam") },
                { "İslamiyet Öncesi Türk Tarihi - Kültür ve Uygarlık - III", ("KPSS Tarih", "Kültürel gelişim") },
                { "Türk - İslam Tarihi - Siyasi Tarih - I", ("KPSS Tarih", "İslamiyet sonrası Türk tarihi") },
                { "Türk - İslam Tarihi - Kültür ve Uygarlık - II", ("KPSS Tarih", "İslami kültür") },
                { "Türkiye Tarihi Siyasi Tarih - I", ("KPSS Tarih", "Anadolu Türk tarihi") },
                { "Türkiye Tarihi Kültür ve Uygarlık - II", ("KPSS Tarih", "Anadolu kültürü") },
                { "Osmanlı Kültür ve Uygarlığı - I", ("KPSS Tarih", "Osmanlı kültürü başlangıç") },
                { "Osmanlı Kültür ve Uygarlığı - II", ("KPSS Tarih", "Osmanlı kültürü gelişim") },
                { "Osmanlı Kültür ve Uygarlığı - III", ("KPSS Tarih", "Osmanlı kültürü zirve") },
                { "Osmanlı Kültür ve Uygarlığı - IV", ("KPSS Tarih", "Osmanlı kültürü son dönem") },
                { "Osmanlı Kuruluş Dönemi", ("KPSS Tarih", "Osmanlı'nın kuruluşu") },
                { "Osmanlı Yükselme Dönemi - I", ("KPSS Tarih", "Osmanlı yükselişi başlangıç") },
                { "Osmanlı Yükselme Dönemi - II", ("KPSS Tarih", "Osmanlı yükselişi devam") },
                { "Osmanlı Duraklama Dönemi - XVII. Yüzyılda Osmanlı", ("KPSS Tarih", "17. yüzyıl Osmanlı") },
                { "Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Siyasi Tarih - I", ("KPSS Tarih", "18. yüzyıl siyasi tarih") },
                { "Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Islahatlar - II", ("KPSS Tarih", "18. yüzyıl ıslahatları") },
                { "Osmanlı Dağılma Dönemi - XIX. Yüzyıl Siyasi Tarih - I", ("KPSS Tarih", "19. yüzyıl siyasi tarih") },
                { "Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - II", ("KPSS Tarih", "19. yüzyıl ıslahatları") },
                { "Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - III", ("KPSS Tarih", "19. yüzyıl ıslahatları devam") },
                { "T.C. İnkılap Tarihi (Atatürk'ün Hayatı)", ("KPSS Tarih", "Atatürk'ün hayatı") },
                { "Trablusgarp ve Balkan Savaşları", ("KPSS Tarih", "Savaşlar dönemi") },
                { "I. Dünya Savaşı ve Sonuçları - I", ("KPSS Tarih", "1. Dünya Savaşı başlangıç") },
                { "I. Dünya Savaşı ve Sonuçları - II", ("KPSS Tarih", "1. Dünya Savaşı devam") },
                { "I. Dünya Savaşı ve Sonuçları - III", ("KPSS Tarih", "1. Dünya Savaşı sonuçları") },
                { "Kurtuluş Savaşı Hazırlık Dönemi - I", ("KPSS Tarih", "Kurtuluş savaşı hazırlık") },
                { "Kurtuluş Savaşı Hazırlık Dönemi - II", ("KPSS Tarih", "Kurtuluş savaşı hazırlık devam") },
                { "I. TBMM Dönemi", ("KPSS Tarih", "TBMM'nin kuruluşu") },
                { "Kurtuluş Savaşı Cepheler Dönemi - I", ("KPSS Tarih", "Cepheler dönemi başlangıç") },
                { "Kurtuluş Savaşı Cepheler Dönemi - II", ("KPSS Tarih", "Cepheler dönemi devam") },
                { "Lozan Barış Antlaşması", ("KPSS Tarih", "Lozan antlaşması") },
                { "İç Politik Gelişmeler", ("KPSS Tarih", "İç politika") },
                { "Atatürk İlkeleri", ("KPSS Tarih", "Atatürk'ün ilkeleri") },
                { "Atatürk İnkılapları - I", ("KPSS Tarih", "İnkılaplar başlangıç") },
                { "Atatürk İnkılapları - II", ("KPSS Tarih", "İnkılaplar devam") },
                { "Atatürk Dönemi Türk Dış Politikası", ("KPSS Tarih", "Dış politika") },
                { "Çağdaş Türk ve Dünya Tarihi (İki Küresel Savaş Arasında Dünya)", ("KPSS Tarih", "Savaşlar arası dönem") },
                { "II. Dünya Savaşı - I", ("KPSS Tarih", "2. Dünya Savaşı başlangıç") },
                { "II. Dünya Savaşı - II", ("KPSS Tarih", "2. Dünya Savaşı devam") },
                { "Soğuk Savaş Dönemi - I", ("KPSS Tarih", "Soğuk savaş başlangıç") },
                { "Soğuk Savaş Dönemi - II", ("KPSS Tarih", "Soğuk savaş devam") },
                { "Yumuşama (Detant) Dönemi ve Sonrası - I", ("KPSS Tarih", "Yumuşama dönemi") },
                { "Yumuşama (Detant) Dönemi ve Sonrası - II", ("KPSS Tarih", "Yumuşama dönemi devam") },
                { "Küreselleşen Dünya - I", ("KPSS Tarih", "Küreselleşme başlangıç") },
                { "Küreselleşen Dünya - II", ("KPSS Tarih", "Küreselleşme devam") }
            };

            // Mevcut konuların Source ve Notes'larını güncelle (NULL veya boş olanlar için)
            bool tarihUpdated = false;
            foreach (var konu in mevcutKonular)
            {
                if (tarihSourceNotesMap.TryGetValue(konu.Title, out var sourceNotes))
                {
                    // NULL veya boş string kontrolü
                    bool needsUpdate = (konu.Source == null || string.IsNullOrWhiteSpace(konu.Source)) || 
                                     (konu.Notes == null || string.IsNullOrWhiteSpace(konu.Notes));
                    
                    if (needsUpdate)
                    {
                        konu.Source = sourceNotes.Source;
                        konu.Notes = sourceNotes.Notes;
                        tarihUpdated = true;
                    }
                }
            }

            if (tarihUpdated)
            {
                await ctx.SaveChangesAsync();
            }

            // Eksik konuları ekle
            if (mevcutKonular.Count < 47)
            {
                var mevcutBasliklar = mevcutKonular.Select(k => k.Title).ToHashSet();
                var yeniKonular = new List<Topic>();
                
                foreach (var kvp in tarihSourceNotesMap)
                {
                    if (!mevcutBasliklar.Contains(kvp.Key))
                    {
                        yeniKonular.Add(new Topic 
                        { 
                            LessonId = tarihDersi.Id, 
                            Title = kvp.Key, 
                            Source = kvp.Value.Source, 
                            Notes = kvp.Value.Notes 
                        });
                    }
                }

                if (yeniKonular.Any())
                {
                    ctx.Topics.AddRange(yeniKonular);
                    await ctx.SaveChangesAsync();
                }
            }

            // Seed Türkçe Dersi ve Konuları
            var turkceDersi = await ctx.Lessons.FirstOrDefaultAsync(l => l.Name == "Türkçe");
            if (turkceDersi == null)
            {
                turkceDersi = new Lesson { Name = "Türkçe" };
                ctx.Lessons.Add(turkceDersi);
                await ctx.SaveChangesAsync();
            }

            // Türkçe konularını kontrol et ve eksik olanları ekle
            var mevcutTurkceKonular = await ctx.Topics.Where(t => t.LessonId == turkceDersi.Id).ToListAsync();
            
            var turkceSourceNotesMap = new Dictionary<string, (string Source, string Notes)>
            {
                // Sözcükte Anlam (8 konu)
                { "Sözcüğün Anlam Özellikleri I", ("KPSS Türkçe", "Sözcüğün temel anlam özellikleri") },
                { "Sözcüğün Anlam Özellikleri II", ("KPSS Türkçe", "Sözcüğün yan anlam özellikleri") },
                { "Sözcükler Arası Anlam İlişkileri", ("KPSS Türkçe", "Eş anlamlı, zıt anlamlı, eş sesli sözcükler") },
                { "Sözcükler Arası Anlam İlişkileri Soru Çözümü", ("KPSS Türkçe", "Anlam ilişkileri soru çözüm teknikleri") },
                { "Mecaz Yolları", ("KPSS Türkçe", "Mecaz anlam ve mecaz yolları") },
                { "İkileme - Deyim - Atasözleri", ("KPSS Türkçe", "İkileme, deyim ve atasözleri") },
                { "Altı Çizili Söz", ("KPSS Türkçe", "Altı çizili sözlerin anlamı") },
                { "Cümle Tamamlama", ("KPSS Türkçe", "Cümle tamamlama soruları") },
                // Cümlede Anlam (10 konu)
                { "Cümle Yorumu", ("KPSS Türkçe", "Cümle yorumlama teknikleri") },
                { "İki Cümle Analizi", ("KPSS Türkçe", "İki cümle arasındaki ilişki") },
                { "Cümle Birleştirme", ("KPSS Türkçe", "Cümle birleştirme soruları") },
                { "Anlamlarına Göre Cümleler", ("KPSS Türkçe", "Cümle türleri ve anlamları") },
                { "Anlam İlişkilerine Göre Cümleler", ("KPSS Türkçe", "Cümleler arası anlam ilişkileri") },
                { "Kanıtlanabilirlik Açısından Cümleler", ("KPSS Türkçe", "Kanıtlanabilir ve kanıtlanamaz cümleler") },
                { "Cümlede Kavramlar", ("KPSS Türkçe", "Cümlede kavram analizi") },
                { "Cümle Oluşturma", ("KPSS Türkçe", "Cümle oluşturma soruları") },
                { "Cümlede Anlam - Etkinlik", ("KPSS Türkçe", "Cümlede anlam etkinlikleri") },
                { "Anlatım Özellikleri", ("KPSS Türkçe", "Anlatım özellikleri ve teknikleri") },
                // Paragrafta Anlam (12 konu)
                { "Anlatım Biçimleri", ("KPSS Türkçe", "Açıklayıcı, öyküleyici, betimleyici anlatım") },
                { "Düşünceyi Geliştirme Yolları I", ("KPSS Türkçe", "Tanımlama, örnekleme, karşılaştırma") },
                { "Düşünceyi Geliştirme Yolları II", ("KPSS Türkçe", "Tanık gösterme, sayısal veriler") },
                { "Paragrafta Boşluk Tamamlama", ("KPSS Türkçe", "Paragrafta boşluk doldurma") },
                { "Paragrafa Cümle Yerleştirme", ("KPSS Türkçe", "Paragrafa cümle ekleme") },
                { "Paragraf Akışı Bozan Cümle", ("KPSS Türkçe", "Paragraf akışını bozan cümleler") },
                { "Paragrafı İkiye Bölme", ("KPSS Türkçe", "Paragrafı bölme teknikleri") },
                { "Paragraf Yer Değiştirme", ("KPSS Türkçe", "Paragraf sıralaması") },
                { "Paragraf Oluşturma", ("KPSS Türkçe", "Paragraf oluşturma soruları") },
                { "Paragraf Soru - Cevap", ("KPSS Türkçe", "Paragraf soru-cevap teknikleri") },
                { "Paragraf Konu - Ana Düşünce", ("KPSS Türkçe", "Paragraf konu ve ana düşünce") },
                { "Paragraf Yardımcı Düşünceler", ("KPSS Türkçe", "Paragraf yardımcı düşünceler") },
                // Dil Bilgisi (13 konu)
                { "Dil Bilgisi Yol Haritası", ("KPSS Türkçe", "Dil bilgisi konularına genel bakış") },
                { "Kök Bilgisi - Yapı Bilgisi", ("KPSS Türkçe", "Sözcük kökleri ve yapı bilgisi") },
                { "Yapım Ekleri - Yapı Bilgisi", ("KPSS Türkçe", "Yapım ekleri ve işlevleri") },
                { "Çekim Ekleri - Yapı Bilgisi", ("KPSS Türkçe", "Çekim ekleri ve işlevleri") },
                { "Yapısına Göre Sözcükler - Yapı Bilgisi", ("KPSS Türkçe", "Basit, türemiş, birleşik sözcükler") },
                { "Yapı Bilgisi - Çıkmış Sınav Soruları", ("KPSS Türkçe", "Yapı bilgisi çıkmış sorular") },
                { "Ses Bilgisi", ("KPSS Türkçe", "Ses bilgisi kuralları") },
                { "Ses Bilgisi - Etkinlik ve Çıkmış Sorular", ("KPSS Türkçe", "Ses bilgisi etkinlikleri") },
                { "İsim (Ad) - Sözcük Türleri", ("KPSS Türkçe", "İsim türleri ve özellikleri") },
                { "Sıfat (Ön Ad) - Sözcük Türleri", ("KPSS Türkçe", "Sıfat türleri ve özellikleri") },
                { "Zamir (Adıl) - Sözcük Türleri", ("KPSS Türkçe", "Zamir türleri ve özellikleri") },
                { "Tamlamalar - Sözcük Türleri", ("KPSS Türkçe", "İsim ve sıfat tamlamaları") }
            };

            // Mevcut Türkçe konularının Source ve Notes'larını güncelle (NULL veya boş olanlar için)
            bool turkceUpdated = false;
            foreach (var konu in mevcutTurkceKonular)
            {
                if (turkceSourceNotesMap.TryGetValue(konu.Title, out var sourceNotes))
                {
                    // NULL veya boş string kontrolü
                    bool needsUpdate = (konu.Source == null || string.IsNullOrWhiteSpace(konu.Source)) || 
                                     (konu.Notes == null || string.IsNullOrWhiteSpace(konu.Notes));
                    
                    if (needsUpdate)
                    {
                        konu.Source = sourceNotes.Source;
                        konu.Notes = sourceNotes.Notes;
                        turkceUpdated = true;
                    }
                }
            }

            if (turkceUpdated)
            {
                await ctx.SaveChangesAsync();
            }

            // Eksik Türkçe konuları ekle
            if (mevcutTurkceKonular.Count < 43)
            {
                var mevcutBasliklar = mevcutTurkceKonular.Select(k => k.Title).ToHashSet();
                var yeniKonular = new List<Topic>();
                
                foreach (var kvp in turkceSourceNotesMap)
                {
                    if (!mevcutBasliklar.Contains(kvp.Key))
                    {
                        yeniKonular.Add(new Topic 
                        { 
                            LessonId = turkceDersi.Id, 
                            Title = kvp.Key, 
                            Source = kvp.Value.Source, 
                            Notes = kvp.Value.Notes 
                        });
                    }
                }

                if (yeniKonular.Any())
                {
                    ctx.Topics.AddRange(yeniKonular);
                    await ctx.SaveChangesAsync();
                }
            }

            // Seed Matematik Dersi ve Konuları
            var matematikDersi = await ctx.Lessons.FirstOrDefaultAsync(l => l.Name == "Matematik");
            if (matematikDersi == null)
            {
                matematikDersi = new Lesson { Name = "Matematik" };
                ctx.Lessons.Add(matematikDersi);
                await ctx.SaveChangesAsync();
            }

            // Matematik konularını kontrol et ve eksik olanları ekle
            var mevcutMatematikKonular = await ctx.Topics.Where(t => t.LessonId == matematikDersi.Id).ToListAsync();
            
            var matematikSourceNotesMap = new Dictionary<string, (string Source, string Notes)>
            {
                // Problemler (10 konu)
                { "Sayı-Kesir Problemleri - 1", ("KPSS Matematik", "Sayı problemleri temel") },
                { "Sayı-Kesir Problemleri - 2", ("KPSS Matematik", "Sayı problemleri gelişmiş") },
                { "Sayı-Kesir Problemleri - 3", ("KPSS Matematik", "Kesir problemleri temel") },
                { "Sayı-Kesir Problemleri - 4", ("KPSS Matematik", "Kesir problemleri gelişmiş") },
                { "Sayı-Kesir Problemleri - 5", ("KPSS Matematik", "Karma problemler") },
                { "Yaş Problemleri - 1", ("KPSS Matematik", "Yaş problemleri temel") },
                { "Yaş Problemleri - 2", ("KPSS Matematik", "Yaş problemleri gelişmiş") },
                { "Karışım Problemleri", ("KPSS Matematik", "Karışım ve oran problemleri") },
                { "İşçi-Havuz Problemleri", ("KPSS Matematik", "İşçi ve havuz problemleri") },
                { "Yüzde-Kar-Zarar Problemleri - 1", ("KPSS Matematik", "Yüzde problemleri") },
                { "Yüzde-Kar-Zarar Problemleri - 2", ("KPSS Matematik", "Kar-zarar problemleri") },
                // Sayısal Mantık (5 konu)
                { "Temel Mantık Kavramları", ("KPSS Matematik", "Mantık temel kavramları") },
                { "Kümeler ve Küme İşlemleri", ("KPSS Matematik", "Küme teorisi ve işlemler") },
                { "Örüntü ve Diziler", ("KPSS Matematik", "Sayı örüntüleri ve diziler") },
                { "Akıl Yürütme Problemleri", ("KPSS Matematik", "Mantıksal akıl yürütme") },
                { "Sıralama ve Karşılaştırma Problemleri", ("KPSS Matematik", "Sıralama ve karşılaştırma") }
            };

            // Mevcut Matematik konularının Source ve Notes'larını güncelle (NULL veya boş olanlar için)
            bool matematikUpdated = false;
            foreach (var konu in mevcutMatematikKonular)
            {
                if (matematikSourceNotesMap.TryGetValue(konu.Title, out var sourceNotes))
                {
                    // NULL veya boş string kontrolü
                    bool needsUpdate = (konu.Source == null || string.IsNullOrWhiteSpace(konu.Source)) || 
                                     (konu.Notes == null || string.IsNullOrWhiteSpace(konu.Notes));
                    
                    if (needsUpdate)
                    {
                        konu.Source = sourceNotes.Source;
                        konu.Notes = sourceNotes.Notes;
                        matematikUpdated = true;
                    }
                }
            }

            if (matematikUpdated)
            {
                await ctx.SaveChangesAsync();
            }

            // Eksik Matematik konuları ekle
            if (mevcutMatematikKonular.Count < 15)
            {
                var mevcutBasliklar = mevcutMatematikKonular.Select(k => k.Title).ToHashSet();
                var yeniKonular = new List<Topic>();
                
                foreach (var kvp in matematikSourceNotesMap)
                {
                    if (!mevcutBasliklar.Contains(kvp.Key))
                    {
                        yeniKonular.Add(new Topic 
                        { 
                            LessonId = matematikDersi.Id, 
                            Title = kvp.Key, 
                            Source = kvp.Value.Source, 
                            Notes = kvp.Value.Notes 
                        });
                    }
                }

                if (yeniKonular.Any())
                {
                    ctx.Topics.AddRange(yeniKonular);
                    await ctx.SaveChangesAsync();
                }
            }

            // Seed Coğrafya Dersi ve Konuları
            var cografyaDersi = await ctx.Lessons.FirstOrDefaultAsync(l => l.Name == "Coğrafya");
            if (cografyaDersi == null)
            {
                cografyaDersi = new Lesson { Name = "Coğrafya" };
                ctx.Lessons.Add(cografyaDersi);
                await ctx.SaveChangesAsync();
            }

            // Coğrafya konularını kontrol et ve eksik olanları ekle
            var mevcutCografyaKonular = await ctx.Topics.Where(t => t.LessonId == cografyaDersi.Id).ToListAsync();
            
            var cografyaSourceNotesMap = new Dictionary<string, (string Source, string Notes)>
            {
                // Coğrafi Konum (5 konu)
                { "COĞRAFİ KONUM - 1", ("KPSS Coğrafya", "Coğrafi konum temel") },
                { "COĞRAFİ KONUM - 2", ("KPSS Coğrafya", "Coğrafi konum gelişmiş") },
                { "COĞRAFİ KONUM - 3", ("KPSS Coğrafya", "Coğrafi konum uygulamaları") },
                { "COĞRAFİ KONUM - 4", ("KPSS Coğrafya", "Coğrafi konum detaylar") },
                { "COĞRAFİ KONUM - 5", ("KPSS Coğrafya", "Coğrafi konum özet") },
                // Türkiye'nin Yer Şekilleri (11 konu)
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 1 (İÇ ve DIŞ KUVVET)", ("KPSS Coğrafya", "İç ve dış kuvvetler") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 2 (JEOLOJİK ZAMANLAR)", ("KPSS Coğrafya", "Jeolojik zamanlar") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 3 (DAĞLAR 1)", ("KPSS Coğrafya", "Dağlar bölüm 1") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 4 (DAĞLAR 2)", ("KPSS Coğrafya", "Dağlar bölüm 2") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 5 (PLATOLAR)", ("KPSS Coğrafya", "Platolar") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 6 (OVALAR)", ("KPSS Coğrafya", "Ovalar") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 7 (AKARSU ŞEKİLLERİ)", ("KPSS Coğrafya", "Akarsu şekilleri") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 8 (RÜZGAR VE BUZULLAR)", ("KPSS Coğrafya", "Rüzgar ve buzullar") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 9 (KARSTİK ŞEKİLLER)", ("KPSS Coğrafya", "Karstik şekiller") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 10 (KIYI ŞEKİLLENMESİ)", ("KPSS Coğrafya", "Kıyı şekillenmesi") },
                { "TÜRKİYE'NİN YER ŞEKİLLERİ - 11 (GENEL ÖZELLİKLER)", ("KPSS Coğrafya", "Yer şekilleri genel özellikler") },
                // Türkiye İklimi (4 konu)
                { "TÜRKİYE İKLİMİ - 1 (SICAKLIK)", ("KPSS Coğrafya", "İklim sıcaklık") },
                { "TÜRKİYE İKLİMİ - 2 (BASINÇ ve RÜZGAR)", ("KPSS Coğrafya", "Basınç ve rüzgar") },
                { "TÜRKİYE İKLİMİ - 3 (NEMLİLİK ve YAĞIŞ)", ("KPSS Coğrafya", "Nemlilik ve yağış") },
                { "TÜRKİYE İKLİMİ - 4 (MİKROKLİMALAR)", ("KPSS Coğrafya", "Mikro iklimler") },
                // Su, Toprak ve Bitki Varlığı (10 konu)
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 1 (AKARSULAR -1)", ("KPSS Coğrafya", "Akarsular bölüm 1") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 2 (AKARSULAR -2)", ("KPSS Coğrafya", "Akarsular bölüm 2") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 3 (GÖLLER)", ("KPSS Coğrafya", "Göller") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 4 (DENİZLER)", ("KPSS Coğrafya", "Denizler") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 5 (YERALTI SULARI)", ("KPSS Coğrafya", "Yeraltı suları") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 6 (TOPRAKLAR -1)", ("KPSS Coğrafya", "Topraklar bölüm 1") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 7 (TOPRAKLAR -2)", ("KPSS Coğrafya", "Topraklar bölüm 2") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 8 (BİTKİ -1)", ("KPSS Coğrafya", "Bitki örtüsü bölüm 1") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 9 (BİTKİ -2)", ("KPSS Coğrafya", "Bitki örtüsü bölüm 2") },
                { "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 10 (BİTKİ -3)", ("KPSS Coğrafya", "Bitki örtüsü bölüm 3") },
                // Çevre ve Doğal Afetler (3 konu)
                { "TÜRKİYE'DE ÇEVRE ve DOĞAL AFETLER - 1", ("KPSS Coğrafya", "Çevre ve afetler bölüm 1") },
                { "TÜRKİYE'DE ÇEVRE ve DOĞAL AFETLER - 2", ("KPSS Coğrafya", "Çevre ve afetler bölüm 2") },
                { "TÜRKİYE'DE ÇEVRE ve DOĞAL AFETLER - 3", ("KPSS Coğrafya", "Çevre ve afetler bölüm 3") },
                // Beşeri Coğrafya (8 konu)
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 1 (NÜFUS -1)", ("KPSS Coğrafya", "Nüfus bölüm 1") },
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 2 (NÜFUS -2)", ("KPSS Coğrafya", "Nüfus bölüm 2") },
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 3 (NÜFUS -3)", ("KPSS Coğrafya", "Nüfus bölüm 3") },
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 4 (NÜFUS -4)", ("KPSS Coğrafya", "Nüfus bölüm 4") },
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 5 (YERLEŞME -1)", ("KPSS Coğrafya", "Yerleşme bölüm 1") },
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 6 (YERLEŞME -2)", ("KPSS Coğrafya", "Yerleşme bölüm 2") },
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 7 (YERLEŞME -3)", ("KPSS Coğrafya", "Yerleşme bölüm 3") },
                { "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 8 (GÖÇLER)", ("KPSS Coğrafya", "Göçler") },
                // Ekonomik Coğrafya (16 konu)
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 1 (EKONOMİ POLİTİKALARI)", ("KPSS Coğrafya", "Ekonomi politikaları") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 2 (TARIM 1)", ("KPSS Coğrafya", "Tarım bölüm 1") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 3 (TARIM 2)", ("KPSS Coğrafya", "Tarım bölüm 2") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 4 (TARIM 3)", ("KPSS Coğrafya", "Tarım bölüm 3") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 5 (TARIM 4)", ("KPSS Coğrafya", "Tarım bölüm 4") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 6 (HAYVANCILIK)", ("KPSS Coğrafya", "Hayvancılık") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 7 (MADENLER 1)", ("KPSS Coğrafya", "Madenler bölüm 1") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 8 (MADENLER 2)", ("KPSS Coğrafya", "Madenler bölüm 2") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 9 (ENERJİ KAYNAKLARI -1)", ("KPSS Coğrafya", "Enerji kaynakları bölüm 1") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 10 (ENERJİ KAYNAKLARI 2)", ("KPSS Coğrafya", "Enerji kaynakları bölüm 2") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 11 (SANAYİ)", ("KPSS Coğrafya", "Sanayi") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 12 (SANAYİ ve TİCARET)", ("KPSS Coğrafya", "Sanayi ve ticaret") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 13 (ULAŞIM -1)", ("KPSS Coğrafya", "Ulaşım bölüm 1") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 14 (ULAŞIM -2)", ("KPSS Coğrafya", "Ulaşım bölüm 2") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 15 (TURİZM -1)", ("KPSS Coğrafya", "Turizm bölüm 1") },
                { "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 16 (TURİZM -2)", ("KPSS Coğrafya", "Turizm bölüm 2") },
                // Bölge Kavramı ve Sistematik (3 konu)
                { "TÜRKİYE'DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 1)", ("KPSS Coğrafya", "Jeopolitik bölge bölüm 1") },
                { "TÜRKİYE'DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 2)", ("KPSS Coğrafya", "Jeopolitik bölge bölüm 2") },
                { "TÜRKİYE'DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (PLAN BÖLGELER)", ("KPSS Coğrafya", "Plan bölgeler") }
            };

            // Mevcut Coğrafya konularının Source ve Notes'larını güncelle (NULL veya boş olanlar için)
            bool cografyaUpdated = false;
            foreach (var konu in mevcutCografyaKonular)
            {
                if (cografyaSourceNotesMap.TryGetValue(konu.Title, out var sourceNotes))
                {
                    // NULL veya boş string kontrolü
                    bool needsUpdate = (konu.Source == null || string.IsNullOrWhiteSpace(konu.Source)) || 
                                     (konu.Notes == null || string.IsNullOrWhiteSpace(konu.Notes));
                    
                    if (needsUpdate)
                    {
                        konu.Source = sourceNotes.Source;
                        konu.Notes = sourceNotes.Notes;
                        cografyaUpdated = true;
                    }
                }
            }

            if (cografyaUpdated)
            {
                await ctx.SaveChangesAsync();
            }

            // Eksik Coğrafya konuları ekle
            if (mevcutCografyaKonular.Count < 60)
            {
                var mevcutBasliklar = mevcutCografyaKonular.Select(k => k.Title).ToHashSet();
                var yeniKonular = new List<Topic>();
                
                foreach (var kvp in cografyaSourceNotesMap)
                {
                    if (!mevcutBasliklar.Contains(kvp.Key))
                    {
                        yeniKonular.Add(new Topic 
                        { 
                            LessonId = cografyaDersi.Id, 
                            Title = kvp.Key, 
                            Source = kvp.Value.Source, 
                            Notes = kvp.Value.Notes 
                        });
                    }
                }

                if (yeniKonular.Any())
                {
                    ctx.Topics.AddRange(yeniKonular);
                    await ctx.SaveChangesAsync();
                }
            }
        }

        public static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }
    }
}


