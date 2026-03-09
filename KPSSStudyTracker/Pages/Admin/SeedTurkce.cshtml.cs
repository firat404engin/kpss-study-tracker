using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Admin
{
    public class SeedTurkceModel : BasePageModel
    {
        private readonly AppDbContext _context;

        public SeedTurkceModel(AppDbContext context)
        {
            _context = context;
        }

        public int TurkceDersId { get; set; }
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
            var turkceDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Türkçe");
            if (turkceDersi != null)
            {
                TurkceDersId = turkceDersi.Id;
                MevcutKonular = await _context.Topics.Where(t => t.LessonId == turkceDersi.Id).ToListAsync();
                MevcutKonuSayisi = MevcutKonular.Count;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostSeedTurkceAsync()
        {
            try
            {
                // Türkçe dersini bul veya oluştur
                var turkceDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Türkçe");
                if (turkceDersi == null)
                {
                    turkceDersi = new Lesson { Name = "Türkçe" };
                    _context.Lessons.Add(turkceDersi);
                    await _context.SaveChangesAsync();
                }

                var userId = GetCurrentUserIdRequired();
                
                // Mevcut konuları kontrol et (topics are now global)
                var mevcutKonular = await _context.Topics.Where(t => t.LessonId == turkceDersi.Id).ToListAsync();
                var mevcutBasliklar = mevcutKonular.Select(k => k.Title).ToHashSet();

                // Tüm Türkçe konularını ekle (43 konu)
                var yeniKonular = new List<Topic>
                {
                    // Sözcükte Anlam (8 konu)
                    new Topic { LessonId = turkceDersi.Id, Title = "Sözcüğün Anlam Özellikleri I" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Sözcüğün Anlam Özellikleri II" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Sözcükler Arası Anlam İlişkileri" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Sözcükler Arası Anlam İlişkileri Soru Çözümü" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Mecaz Yolları" },
                    new Topic { LessonId = turkceDersi.Id, Title = "İkileme - Deyim - Atasözleri" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Altı Çizili Söz" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Cümle Tamamlama" },
                    
                    // Cümlede Anlam (10 konu)
                    new Topic { LessonId = turkceDersi.Id, Title = "Cümle Yorumu" },
                    new Topic { LessonId = turkceDersi.Id, Title = "İki Cümle Analizi" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Cümle Birleştirme" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Anlamlarına Göre Cümleler" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Anlam İlişkilerine Göre Cümleler" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Kanıtlanabilirlik Açısından Cümleler" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Cümlede Kavramlar" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Cümle Oluşturma" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Cümlede Anlam - Etkinlik" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Anlatım Özellikleri" },
                    
                    // Paragrafta Anlam (12 konu)
                    new Topic { LessonId = turkceDersi.Id, Title = "Anlatım Biçimleri" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Düşünceyi Geliştirme Yolları I" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Düşünceyi Geliştirme Yolları II" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragrafta Boşluk Tamamlama" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragrafa Cümle Yerleştirme" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragraf Akışı Bozan Cümle" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragrafı İkiye Bölme" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragraf Yer Değiştirme" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragraf Oluşturma" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragraf Soru - Cevap" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragraf Konu - Ana Düşünce" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Paragraf Yardımcı Düşünceler" },
                    
                    // Dil Bilgisi (13 konu)
                    new Topic { LessonId = turkceDersi.Id, Title = "Dil Bilgisi Yol Haritası" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Kök Bilgisi - Yapı Bilgisi" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Yapım Ekleri - Yapı Bilgisi" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Çekim Ekleri - Yapı Bilgisi" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Yapısına Göre Sözcükler - Yapı Bilgisi" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Yapı Bilgisi - Çıkmış Sınav Soruları" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Ses Bilgisi" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Ses Bilgisi - Etkinlik ve Çıkmış Sorular" },
                    new Topic { LessonId = turkceDersi.Id, Title = "İsim (Ad) - Sözcük Türleri" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Sıfat (Ön Ad) - Sözcük Türleri" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Zamir (Adıl) - Sözcük Türleri" },
                    new Topic { LessonId = turkceDersi.Id, Title = "Tamlamalar - Sözcük Türleri" }
                };

                // Sadece eksik olan konuları ekle
                var eklenecekKonular = yeniKonular.Where(k => !mevcutBasliklar.Contains(k.Title)).ToList();
                
                if (eklenecekKonular.Any())
                {
                    _context.Topics.AddRange(eklenecekKonular);
                    await _context.SaveChangesAsync();
                    SuccessMessage = $"{eklenecekKonular.Count} yeni Türkçe konusu başarıyla eklendi!";
                }
                else
                {
                    SuccessMessage = "Tüm Türkçe konuları zaten mevcut!";
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



