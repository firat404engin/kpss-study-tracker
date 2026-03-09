using System.ComponentModel.DataAnnotations;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Pages.Exams
{
    public class CreateModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty]
        public List<LessonResultVm> LessonResults { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Tüm dersleri al
            var lessons = await _context.Lessons
                .OrderBy(l => l.Name)
                .ToListAsync();

            // Eğer hiç ders yoksa, temel dersleri oluştur
            if (!lessons.Any())
            {
                var basicLessons = new[]
                {
                    new Lesson { Name = "Türkçe" },
                    new Lesson { Name = "Matematik" },
                    new Lesson { Name = "Tarih" },
                    new Lesson { Name = "Coğrafya" },
                    new Lesson { Name = "Vatandaşlık" }
                };

                _context.Lessons.AddRange(basicLessons);
                await _context.SaveChangesAsync();
                
                lessons = await _context.Lessons.OrderBy(l => l.Name).ToListAsync();
            }

            // Tercih edilen dersleri filtrele
            var preferred = new[] { "Türkçe", "Matematik", "Tarih", "Coğrafya", "Vatandaşlık", "Güncel Bilgiler" };
            var filtered = lessons.Where(l => preferred.Contains(l.Name)).ToList();
            
            // Eğer tercih edilen dersler yoksa, tüm dersleri göster
            if (!filtered.Any()) 
            {
                filtered = lessons.Take(4).ToList(); // En fazla 4 ders göster
            }

            LessonResults = filtered
                .Select(l => new LessonResultVm { LessonId = l.Id, LessonName = l.Name })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Sadece Name validation'ı kontrol et
            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ModelState.AddModelError("Input.Name", "Deneme adı zorunludur.");
            }

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                // Deneme sınavını oluştur
                var userId = GetCurrentUserIdRequired();
                var exam = new MockExam 
                { 
                    UserId = userId,
                    Name = Input.Name.Trim(), 
                    Date = Input.Date, 
                    Notes = Input.Notes?.Trim() 
                };
                
                _context.MockExams.Add(exam);
                await _context.SaveChangesAsync();

                // Ders sonuçlarını ekle
                foreach (var r in LessonResults)
                {
                    if (r.Correct > 0 || r.Wrong > 0)
                    {
                        _context.MockExamResults.Add(new MockExamResult
                        {
                            MockExamId = exam.Id,
                            LessonId = r.LessonId,
                            Correct = r.Correct,
                            Wrong = r.Wrong
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                // Hata durumunda sayfayı yeniden yükle
                await OnGetAsync();
                ModelState.AddModelError("", $"Kaydetme sırasında hata oluştu: {ex.Message}");
                return Page();
            }
        }

        public class InputModel
        {
            [Required(ErrorMessage = "Deneme adı zorunludur.")]
            public string Name { get; set; } = string.Empty;
            public DateTime Date { get; set; } = DateTime.Today;
            public string? Notes { get; set; }
        }

        public class LessonResultVm
        {
            public int LessonId { get; set; }
            public string LessonName { get; set; } = string.Empty;
            public int Correct { get; set; }
            public int Wrong { get; set; }
        }
    }
}


