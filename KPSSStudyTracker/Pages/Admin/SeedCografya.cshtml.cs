using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Admin
{
    public class SeedCografyaModel : BasePageModel
    {
        private readonly AppDbContext _context;

        public SeedCografyaModel(AppDbContext context)
        {
            _context = context;
        }

        public int CografyaDersId { get; set; }
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
            var cografyaDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Coğrafya");
            if (cografyaDersi != null)
            {
                CografyaDersId = cografyaDersi.Id;
                MevcutKonular = await _context.Topics.Where(t => t.LessonId == cografyaDersi.Id).ToListAsync();
                MevcutKonuSayisi = MevcutKonular.Count;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostSeedCografyaAsync()
        {
            try
            {
                // Coğrafya dersini bul veya oluştur
                var cografyaDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Coğrafya");
                if (cografyaDersi == null)
                {
                    cografyaDersi = new Lesson { Name = "Coğrafya" };
                    _context.Lessons.Add(cografyaDersi);
                    await _context.SaveChangesAsync();
                }

                var userId = GetCurrentUserIdRequired();
                
                // Mevcut konuları kontrol et (topics are now global)
                var mevcutKonular = await _context.Topics.Where(t => t.LessonId == cografyaDersi.Id).ToListAsync();
                var mevcutBasliklar = mevcutKonular.Select(k => k.Title).ToHashSet();

                // Tüm Coğrafya konularını ekle (60 konu)
                var yeniKonular = new List<Topic>
                {
                    // Coğrafi Konum (5 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "COĞRAFİ KONUM - 1" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "COĞRAFİ KONUM - 2" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "COĞRAFİ KONUM - 3" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "COĞRAFİ KONUM - 4" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "COĞRAFİ KONUM - 5" },
                    
                    // Türkiye'nin Yer Şekilleri (11 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 1 (İÇ ve DIŞ KUVVET)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 2 (JEOLOJİK ZAMANLAR)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 3 (DAĞLAR 1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 4 (DAĞLAR 2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 5 (PLATOLAR)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 6 (OVALAR)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 7 (AKARSU ŞEKİLLERİ)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 8 (RÜZGAR VE BUZULLAR)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 9 (KARSTİK ŞEKİLLER)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 10 (KIYI ŞEKİLLENMESİ)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN YER ŞEKİLLERİ - 11 (GENEL ÖZELLİKLER)" },
                    
                    // Türkiye İklimi (4 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE İKLİMİ - 1 (SICAKLIK)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE İKLİMİ - 2 (BASINÇ ve RÜZGAR)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE İKLİMİ - 3 (NEMLİLİK ve YAĞIŞ)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE İKLİMİ - 4 (MİKROKLİMALAR)" },
                    
                    // Su, Toprak ve Bitki Varlığı (10 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 1 (AKARSULAR -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 2 (AKARSULAR -2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 3 (GÖLLER)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 4 (DENİZLER)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 5 (YERALTI SULARI)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 6 (TOPRAKLAR -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 7 (TOPRAKLAR -2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 8 (BİTKİ -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 9 (BİTKİ -2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN SU, TOPRAK ve BİTKİ VARLIĞI - 10 (BİTKİ -3)" },
                    
                    // Çevre ve Doğal Afetler (3 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'DE ÇEVRE ve DOĞAL AFETLER - 1" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'DE ÇEVRE ve DOĞAL AFETLER - 2" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'DE ÇEVRE ve DOĞAL AFETLER - 3" },
                    
                    // Beşeri Coğrafya (8 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 1 (NÜFUS -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 2 (NÜFUS -2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 3 (NÜFUS -3)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 4 (NÜFUS -4)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 5 (YERLEŞME -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 6 (YERLEŞME -2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 7 (YERLEŞME -3)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN BEŞERİ COĞRAFYASI - 8 (GÖÇLER)" },
                    
                    // Ekonomik Coğrafya (16 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 1 (EKONOMİ POLİTİKALARI)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 2 (TARIM 1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 3 (TARIM 2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 4 (TARIM 3)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 5 (TARIM 4)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 6 (HAYVANCILIK)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 7 (MADENLER 1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 8 (MADENLER 2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 9 (ENERJİ KAYNAKLARI -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 10 (ENERJİ KAYNAKLARI 2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 11 (SANAYİ)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 12 (SANAYİ ve TİCARET)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 13 (ULAŞIM -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 14 (ULAŞIM -2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 15 (TURİZM -1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'NİN EKONOMİK COĞRAFYASI - 16 (TURİZM -2)" },
                    
                    // Bölge Kavramı ve Sistematik (3 konu)
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 1)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (JEOPOLİTİK BÖLGE 2)" },
                    new Topic { LessonId = cografyaDersi.Id, Title = "TÜRKİYE'DE BÖLGE KAVRAMI ve SİSTEMATİĞİ (PLAN BÖLGELER)" }
                };

                // Sadece eksik olan konuları ekle
                var eklenecekKonular = yeniKonular.Where(k => !mevcutBasliklar.Contains(k.Title)).ToList();
                
                if (eklenecekKonular.Any())
                {
                    _context.Topics.AddRange(eklenecekKonular);
                    await _context.SaveChangesAsync();
                    SuccessMessage = $"{eklenecekKonular.Count} yeni Coğrafya konusu başarıyla eklendi!";
                }
                else
                {
                    SuccessMessage = "Tüm Coğrafya konuları zaten mevcut!";
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



