using System.ComponentModel.DataAnnotations;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Pages.Exams
{
    public class EditModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ExamName { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var userId = GetCurrentUserIdRequired();
            var exam = await _context.MockExams
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (exam == null)
            {
                return NotFound();
            }

            ExamName = exam.Name;
            Input = new InputModel
            {
                Name = exam.Name,
                Date = exam.Date,
                Notes = exam.Notes
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (string.IsNullOrWhiteSpace(Input.Name))
            {
                ModelState.AddModelError("Input.Name", "Deneme adı zorunludur.");
            }

            if (!ModelState.IsValid)
            {
                var exam = await _context.MockExams.FindAsync(id);
                if (exam != null)
                {
                    ExamName = exam.Name;
                }
                return Page();
            }

            try
            {
                var userId = GetCurrentUserIdRequired();
                var exam = await _context.MockExams
                    .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
                if (exam == null)
                {
                    return NotFound();
                }

                exam.Name = Input.Name.Trim();
                exam.Date = Input.Date;
                exam.Notes = Input.Notes?.Trim();

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Deneme sınavı başarıyla güncellendi.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Güncelleme sırasında hata oluştu: {ex.Message}");
                var exam = await _context.MockExams.FindAsync(id);
                if (exam != null)
                {
                    ExamName = exam.Name;
                }
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
    }
}



