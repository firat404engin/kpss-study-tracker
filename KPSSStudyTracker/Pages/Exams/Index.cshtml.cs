using KPSSStudyTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Pages.Exams
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) { _context = context; }

        public List<ExamVm> Exams { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = GetCurrentUserIdRequired();
            var exams = await _context.MockExams
                .Where(e => e.UserId == userId)
                .Include(e => e.Results)
                .ThenInclude(r => r.Lesson)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            Exams = exams.Select(e =>
            {
                // GK dersleri: Tarih, Coğrafya, Vatandaşlık, Güncel Bilgiler (varsa)
                var gkLessons = new[] { "Tarih", "Coğrafya", "Vatandaşlık", "Güncel Bilgiler" };
                // GY dersleri: Türkçe, Matematik, Geometri (varsa)
                var gyLessons = new[] { "Türkçe", "Matematik", "Geometri" };

                double gkNet = e.Results
                    .Where(r => r.Lesson != null && gkLessons.Contains(r.Lesson!.Name))
                    .Sum(r => (double)r.Correct - r.Wrong / 4.0);

                double gyNet = e.Results
                    .Where(r => r.Lesson != null && gyLessons.Contains(r.Lesson!.Name))
                    .Sum(r => (double)r.Correct - r.Wrong / 4.0);

                double totalNet = gkNet + gyNet;

                // KPSS-B P3: Gerçek katsayılarla hesaplama
                double totalWeightedScore = 0;
                foreach (var result in e.Results.Where(r => r.Lesson != null))
                {
                    double net = result.Correct - result.Wrong / 4.0;
                    double coefficient = result.Lesson!.Name.ToLower() switch
                    {
                        var name when name.Contains("türkçe") => 0.5,
                        var name when name.Contains("matematik") => 0.5,
                        var name when name.Contains("tarih") => 0.3,
                        var name when name.Contains("coğrafya") => 0.2,
                        var name when name.Contains("vatandaşlık") => 0.15,
                        var name when name.Contains("güncel") => 0.05,
                        _ => 0
                    };
                    totalWeightedScore += net * coefficient;
                }
                double p3Score = Math.Round(50d + totalWeightedScore, 2);

                return new ExamVm
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    Net = Math.Round(totalNet, 2),
                    GkNet = Math.Round(gkNet, 2),
                    GyNet = Math.Round(gyNet, 2),
                    P3Score = p3Score
                };
            }).ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = GetCurrentUserIdRequired();
            var exam = await _context.MockExams
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (exam != null)
            {
                _context.MockExams.Remove(exam);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Deneme sınavı başarıyla silindi.";
            }
            else
            {
                TempData["ErrorMessage"] = "Deneme sınavı bulunamadı.";
            }
            
            return RedirectToPage();
        }

        public class ExamVm
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public DateTime Date { get; set; }
            public double Net { get; set; }
            public double GkNet { get; set; }
            public double GyNet { get; set; }
            public double P3Score { get; set; }
        }
    }
}


