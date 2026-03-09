using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Admin
{
    public class SeedTarihModel : BasePageModel
    {
        private readonly AppDbContext _context;

        public SeedTarihModel(AppDbContext context)
        {
            _context = context;
        }

        public int TarihDersId { get; set; }
        public int MevcutKonuSayisi { get; set; }
        public List<Topic> MevcutKonular { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsAdmin)
            {
                return RedirectToPage("/Index");
            }
            
            var userId = GetCurrentUserIdRequired();
            var tarihDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Tarih");
            if (tarihDersi != null)
            {
                TarihDersId = tarihDersi.Id;
                MevcutKonular = await _context.Topics.Where(t => t.LessonId == tarihDersi.Id).ToListAsync();
                MevcutKonuSayisi = MevcutKonular.Count;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostSeedTarihAsync()
        {
            try
            {
                // Tarih dersini bul veya oluştur
                var tarihDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Tarih");
                if (tarihDersi == null)
                {
                    tarihDersi = new Lesson { Name = "Tarih" };
                    _context.Lessons.Add(tarihDersi);
                    await _context.SaveChangesAsync();
                }

                var userId = GetCurrentUserIdRequired();
                
                // Mevcut konuları kontrol et (topics are now global)
                var mevcutKonular = await _context.Topics.Where(t => t.LessonId == tarihDersi.Id).ToListAsync();
                var mevcutBasliklar = mevcutKonular.Select(k => k.Title).ToHashSet();

                // Tüm tarih konularını ekle (47 konu)
                var yeniKonular = new List<Topic>
                {
                    // İslamiyet Öncesi Türk Tarihi (3 konu)
                    new Topic { LessonId = tarihDersi.Id, Title = "İslamiyet Öncesi Türk Tarihi Siyasi Tarih - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "İslamiyet Öncesi Türk Tarihi Siyasi Tarih - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "İslamiyet Öncesi Türk Tarihi - Kültür ve Uygarlık - III" },
                    
                    // Türk - İslam Tarihi (2 konu)
                    new Topic { LessonId = tarihDersi.Id, Title = "Türk - İslam Tarihi - Siyasi Tarih - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Türk - İslam Tarihi - Kültür ve Uygarlık - II" },
                    
                    // Türkiye Tarihi (2 konu)
                    new Topic { LessonId = tarihDersi.Id, Title = "Türkiye Tarihi Siyasi Tarih - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Türkiye Tarihi Kültür ve Uygarlık - II" },
                    
                    // Osmanlı Kültür ve Uygarlığı (4 konu)
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Kültür ve Uygarlığı - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Kültür ve Uygarlığı - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Kültür ve Uygarlığı - III" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Kültür ve Uygarlığı - IV" },
                    
                    // Osmanlı Siyasi Tarihi (9 konu)
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Kuruluş Dönemi" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Yükselme Dönemi - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Yükselme Dönemi - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Duraklama Dönemi - XVII. Yüzyılda Osmanlı" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Siyasi Tarih - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Gerileme Dönemi - XVIII. Yüzyıl Islahatlar - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Dağılma Dönemi - XIX. Yüzyıl Siyasi Tarih - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Osmanlı Dağılma Dönemi - XIX. Yüzyıl Islahatlar - III" },
                    
                    // T.C. İnkılap Tarihi (16 konu)
                    new Topic { LessonId = tarihDersi.Id, Title = "T.C. İnkılap Tarihi (Atatürk'ün Hayatı)" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Trablusgarp ve Balkan Savaşları" },
                    new Topic { LessonId = tarihDersi.Id, Title = "I. Dünya Savaşı ve Sonuçları - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "I. Dünya Savaşı ve Sonuçları - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "I. Dünya Savaşı ve Sonuçları - III" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Kurtuluş Savaşı Hazırlık Dönemi - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Kurtuluş Savaşı Hazırlık Dönemi - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "I. TBMM Dönemi" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Kurtuluş Savaşı Cepheler Dönemi - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Kurtuluş Savaşı Cepheler Dönemi - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Lozan Barış Antlaşması" },
                    new Topic { LessonId = tarihDersi.Id, Title = "İç Politik Gelişmeler" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Atatürk İlkeleri" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Atatürk İnkılapları - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Atatürk İnkılapları - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Atatürk Dönemi Türk Dış Politikası" },
                    
                    // Çağdaş Türk ve Dünya Tarihi (10 konu)
                    new Topic { LessonId = tarihDersi.Id, Title = "Çağdaş Türk ve Dünya Tarihi (İki Küresel Savaş Arasında Dünya)" },
                    new Topic { LessonId = tarihDersi.Id, Title = "II. Dünya Savaşı - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "II. Dünya Savaşı - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Soğuk Savaş Dönemi - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Soğuk Savaş Dönemi - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Yumuşama (Detant) Dönemi ve Sonrası - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Yumuşama (Detant) Dönemi ve Sonrası - II" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Küreselleşen Dünya - I" },
                    new Topic { LessonId = tarihDersi.Id, Title = "Küreselleşen Dünya - II" }
                };

                // Sadece eksik olan konuları ekle
                var eklenecekKonular = yeniKonular.Where(k => !mevcutBasliklar.Contains(k.Title)).ToList();
                
                if (eklenecekKonular.Any())
                {
                    _context.Topics.AddRange(eklenecekKonular);
                    await _context.SaveChangesAsync();
                    SuccessMessage = $"{eklenecekKonular.Count} yeni konu başarıyla eklendi!";
                }
                else
                {
                    SuccessMessage = "Tüm konular zaten mevcut!";
                }

                // Sayfayı yenile
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Hata oluştu: {ex.Message}";
                return Page();
            }
        }
    }
}
