using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KPSSStudyTracker.Data;

namespace KPSSStudyTracker.Pages
{
    public abstract class BasePageModel : PageModel
    {
        protected int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        protected int GetCurrentUserIdRequired()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue)
            {
                throw new UnauthorizedAccessException("Kullanıcı oturum açmamış.");
            }
            return userId.Value;
        }

        protected async Task<bool> IsAdminAsync(AppDbContext context)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return false;
            
            var user = await context.Users.FindAsync(userId.Value);
            return user != null && user.IsAdmin;
        }

        protected bool IsAdminSync(AppDbContext context)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return false;
            
            var user = context.Users.Find(userId.Value);
            return user != null && user.IsAdmin;
        }
    }
}




