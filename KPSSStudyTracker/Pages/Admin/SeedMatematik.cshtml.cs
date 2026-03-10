using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Admin
{
    public class SeedMatematikModel : BasePageModel
    {
        private readonly AppDbContext _context;

        public SeedMatematikModel(AppDbContext context)
        {
            _context = context;
        }

        public int MatematikDersId { get; set; }
        public int MevcutKonuSayisi { get; set; }
        public List<Topic> MevcutKonular { get; set; } = new();
        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!await IsAdminAsync(_context))
            {
                return RedirectToPage("/Index");
            }
            
            var userId = GetCurrentUserIdRequired();
            var matematikDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Matematik");
            if (matematikDersi != null)
            {
                MatematikDersId = matematikDersi.Id;
                MevcutKonular = await _context.Topics.Where(t => t.LessonId == matematikDersi.Id).ToListAsync();
                MevcutKonuSayisi = MevcutKonular.Count;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostSeedMatematikAsync()
        {
            try
            {
                // Matematik dersini bul veya oluştur
                var matematikDersi = await _context.Lessons.FirstOrDefaultAsync(l => l.Name == "Matematik");
                if (matematikDersi == null)
                {
                    matematikDersi = new Lesson { Name = "Matematik" };
                    _context.Lessons.Add(matematikDersi);
                    await _context.SaveChangesAsync();
                }

                var userId = GetCurrentUserIdRequired();
                
                // Mevcut konuları kontrol et (topics are now global)
                var mevcutKonular = await _context.Topics.Where(t => t.LessonId == matematikDersi.Id).ToListAsync();
                var mevcutBasliklar = mevcutKonular.Select(k => k.Title).ToHashSet();

                // Tüm Matematik konularını ekle (15 konu)
                var yeniKonular = new List<Topic>
                {
                    // Problemler (10 konu)
                    new Topic { LessonId = matematikDersi.Id, Title = "Sayı-Kesir Problemleri - 1" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Sayı-Kesir Problemleri - 2" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Sayı-Kesir Problemleri - 3" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Sayı-Kesir Problemleri - 4" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Sayı-Kesir Problemleri - 5" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Yaş Problemleri - 1" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Yaş Problemleri - 2" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Karışım Problemleri" },
                    new Topic { LessonId = matematikDersi.Id, Title = "İşçi-Havuz Problemleri" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Yüzde-Kar-Zarar Problemleri - 1" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Yüzde-Kar-Zarar Problemleri - 2" },
                    
                    // Sayısal Mantık (5 konu)
                    new Topic { LessonId = matematikDersi.Id, Title = "Temel Mantık Kavramları" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Kümeler ve Küme İşlemleri" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Örüntü ve Diziler" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Akıl Yürütme Problemleri" },
                    new Topic { LessonId = matematikDersi.Id, Title = "Sıralama ve Karşılaştırma Problemleri" }
                };

                // Sadece eksik olan konuları ekle
                var eklenecekKonular = yeniKonular.Where(k => !mevcutBasliklar.Contains(k.Title)).ToList();
                
                if (eklenecekKonular.Any())
                {
                    _context.Topics.AddRange(eklenecekKonular);
                    await _context.SaveChangesAsync();
                    SuccessMessage = $"{eklenecekKonular.Count} yeni Matematik konusu başarıyla eklendi!";
                }
                else
                {
                    SuccessMessage = "Tüm Matematik konuları zaten mevcut!";
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



