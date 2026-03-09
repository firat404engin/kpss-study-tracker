using System.ComponentModel.DataAnnotations;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Lessons
{
    public class EditModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public EditModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public InputModel Input { get; set; } = new();
        
        public string OriginalName { get; set; } = string.Empty;
        public int TopicCount { get; set; }
        public int CompletedTopicCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Sadece admin ders düzenleyebilir
            if (!await IsAdminAsync(_context))
            {
                TempData["ErrorMessage"] = "Bu işlem için admin yetkisi gereklidir.";
                return RedirectToPage("Index");
            }

            var lesson = await _context.Lessons
                .Include(l => l.Topics)
                .FirstOrDefaultAsync(l => l.Id == id);
                
            if (lesson == null) return RedirectToPage("Index");
            
            Input = new InputModel { Id = lesson.Id, Name = lesson.Name };
            OriginalName = lesson.Name;
            TopicCount = lesson.Topics.Count;
            // Topics are global, count completed by checking UserTopicProgress
            var userId = GetCurrentUserId();
            if (userId.HasValue)
            {
                var topicIds = lesson.Topics.Select(t => t.Id).ToList();
                CompletedTopicCount = await _context.UserTopicProgresses
                    .CountAsync(utp => utp.UserId == userId && topicIds.Contains(utp.TopicId) && utp.Completed);
            }
            else
            {
                CompletedTopicCount = 0;
            }
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Sadece admin ders düzenleyebilir
            if (!await IsAdminAsync(_context))
            {
                return Forbid();
            }

            if (!ModelState.IsValid) return Page();
            
            var lesson = await _context.Lessons.FindAsync(Input.Id);
            if (lesson == null) return RedirectToPage("Index");
            
            lesson.Name = Input.Name;
            await _context.SaveChangesAsync();
            
            return RedirectToPage("Index");
        }

        public class InputModel
        {
            public int Id { get; set; }
            [Required(ErrorMessage = "Ders adı zorunludur.")]
            [MaxLength(100, ErrorMessage = "Ders adı en fazla 100 karakter olabilir.")]
            public string Name { get; set; } = string.Empty;
        }
    }
}


