using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using KPSSStudyTracker.Pages;

namespace KPSSStudyTracker.Pages.Lessons
{
    public class DeleteModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public DeleteModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public int Id { get; set; }
        public string LessonName { get; set; } = string.Empty;
        public int TopicCount { get; set; }
        public int CompletedTopicCount { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Sadece admin ders silebilir
            if (!await IsAdminAsync(_context))
            {
                TempData["ErrorMessage"] = "Bu işlem için admin yetkisi gereklidir.";
                return RedirectToPage("Index");
            }

            var lesson = await _context.Lessons
                .Include(l => l.Topics)
                .FirstOrDefaultAsync(l => l.Id == id);
            
            if (lesson == null)
            {
                return NotFound();
            }

            Id = id;
            LessonName = lesson.Name;
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
            // Sadece admin ders silebilir
            if (!await IsAdminAsync(_context))
            {
                return Forbid();
            }

            var lesson = await _context.Lessons
                .Include(l => l.Topics)
                .FirstOrDefaultAsync(l => l.Id == Id);
                
            if (lesson != null) 
            {
                _context.Lessons.Remove(lesson);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}


