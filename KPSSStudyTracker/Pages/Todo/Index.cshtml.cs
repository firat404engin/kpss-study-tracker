using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using KPSSStudyTracker.Data;
using KPSSStudyTracker.Models;

namespace KPSSStudyTracker.Pages.Todo
{
    public class IndexModel : BasePageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context) { _context = context; }

        [BindProperty(SupportsGet = true)]
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;

        public List<DailyTodo> Items { get; set; } = new();

        [BindProperty]
        public string NewTitle { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            var userId = GetCurrentUserIdRequired();
            var targetDate = Date.Date.ToUniversalTime();
            Items = await _context.DailyTodos
                .Where(t => t.UserId == userId && t.Date.Date == targetDate)
                .OrderBy(t => t.IsCompleted)
                .ThenBy(t => t.Id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAddAsync(DateTime date, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return RedirectToPage("Index", new { date = date.ToString("yyyy-MM-dd") });
            }
            var userId = GetCurrentUserIdRequired();
            _context.DailyTodos.Add(new DailyTodo
            {
                UserId = userId,
                Date = date.Date,
                Title = title.Trim(),
                CreatedAt = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
            return RedirectToPage("Index", new { date = date.ToString("yyyy-MM-dd") });
        }

        public class ToggleRequest { public int id { get; set; } public bool isCompleted { get; set; } }
        public class NotesRequest { public int id { get; set; } public string? notes { get; set; } public bool? complete { get; set; } }

        public async Task<IActionResult> OnPostToggleAsync([FromBody] ToggleRequest req)
        {
            var userId = GetCurrentUserIdRequired();
            var item = await _context.DailyTodos
                .FirstOrDefaultAsync(t => t.Id == req.id && t.UserId == userId);
            if (item == null) return new JsonResult(new { success = false });
            item.IsCompleted = req.isCompleted;
            item.CompletedAt = req.isCompleted ? DateTime.UtcNow : null;
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true });
        }

        public async Task<IActionResult> OnPostUpdateNotesAsync([FromBody] NotesRequest req)
        {
            var userId = GetCurrentUserIdRequired();
            var item = await _context.DailyTodos
                .FirstOrDefaultAsync(t => t.Id == req.id && t.UserId == userId);
            if (item == null) return new JsonResult(new { success = false });
            item.Notes = string.IsNullOrWhiteSpace(req.notes) ? null : req.notes.Trim();
            if (req.complete.HasValue && req.complete.Value)
            {
                item.IsCompleted = true;
                item.CompletedAt = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return new JsonResult(new { success = true, isCompleted = item.IsCompleted });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var userId = GetCurrentUserIdRequired();
            var item = await _context.DailyTodos
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (item != null)
            {
                _context.DailyTodos.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index", new { date = DateTime.UtcNow.Date.ToString("yyyy-MM-dd") });
        }
    }
}


