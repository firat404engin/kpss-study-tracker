using System.ComponentModel.DataAnnotations;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Lessons
{
    public class CreateModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public CreateModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Sadece admin ders ekleyebilir
            if (!await IsAdminAsync(_context))
            {
                return RedirectToPage("Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Sadece admin ders ekleyebilir
            if (!await IsAdminAsync(_context))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
                return Page();

            var lesson = new Lesson { Name = Input.Name };
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        public class InputModel
        {
            [Required, MaxLength(100)]
            public string Name { get; set; } = string.Empty;
        }
    }
}



