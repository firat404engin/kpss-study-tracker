using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KPSSStudyTracker.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace KPSSStudyTracker.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly AppDbContext _context;
        public RegisterModel(AppDbContext context) { _context = context; }

        [BindProperty]
        public RegisterInput Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == Input.Username))
            {
                ErrorMessage = "Bu kullanıcı adı zaten kullanılıyor.";
                return Page();
            }

            // Check password strength
            if (Input.Password.Length < 6)
            {
                ErrorMessage = "Şifre en az 6 karakter olmalıdır.";
                return Page();
            }

            // Create new user
            var user = new Models.UserAccount
            {
                Username = Input.Username,
                PasswordHash = ComputeSha256(Input.Password),
                CreatedAtUtc = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Auto-login after registration
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
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

        public class RegisterInput
        {
            [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
            [MaxLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir.")]
            [Display(Name = "Kullanıcı Adı")]
            public string Username { get; set; } = string.Empty;

            [Required(ErrorMessage = "Şifre zorunludur.")]
            [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
            [DataType(DataType.Password)]
            [Display(Name = "Şifre")]
            public string Password { get; set; } = string.Empty;

            [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor.")]
            [Display(Name = "Şifre Tekrar")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }
    }
}

