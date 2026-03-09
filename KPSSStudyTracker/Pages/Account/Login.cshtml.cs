using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KPSSStudyTracker.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KPSSStudyTracker.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly AppDbContext _context;
        public LoginModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public LoginInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var hash = ComputeSha256(Input.Password);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == Input.Username && u.PasswordHash == hash);
            if (user == null)
            {
                ErrorMessage = "Kullanıcı adı veya şifre hatalı.";
                return Page();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Username), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToPage("/Index");
        }

        private static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(input)));
        }

        public class LoginInput
        {
            [Required]
            public string Username { get; set; } = string.Empty;
            [Required]
            public string Password { get; set; } = string.Empty;
        }
    }
}


