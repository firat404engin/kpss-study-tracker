using KPSSStudyTracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Pages
{
    [AllowAnonymous]
    public class HealthModel : PageModel
    {
        private readonly AppDbContext _context;
        public HealthModel(AppDbContext context) { _context = context; }

        public string Status { get; set; } = "";
        public string DatabaseStatus { get; set; } = "";
        public string ConnectionString { get; set; } = "";
        public int UserCount { get; set; }
        public int LessonCount { get; set; }
        public int TopicCount { get; set; }
        public string ErrorMessage { get; set; } = "";

        public async Task OnGetAsync()
        {
            try
            {
                Status = "✅ Uygulama çalışıyor";
                
                // Connection string'i göster (şifre gizli)
                var connStr = _context.Database.GetConnectionString() ?? "Yok";
                ConnectionString = connStr.Contains("Password=") 
                    ? connStr.Substring(0, connStr.IndexOf("Password=")) + "Password=***" 
                    : connStr;

                // Database'e bağlan
                await _context.Database.CanConnectAsync();
                DatabaseStatus = "✅ Database bağlantısı başarılı";

                // Verileri say
                UserCount = await _context.Users.CountAsync();
                LessonCount = await _context.Lessons.CountAsync();
                TopicCount = await _context.Topics.CountAsync();
            }
            catch (Exception ex)
            {
                Status = "❌ Uygulama hatası";
                DatabaseStatus = "❌ Database bağlantısı başarısız";
                ErrorMessage = ex.Message;
            }
        }
    }
}
