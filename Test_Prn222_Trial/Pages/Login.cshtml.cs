using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Test_Prn222_Trial.Models;

namespace Test_Prn222_Trial.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Sp25PharmaceuticalDbContext _context;

        // Constructor for dependency injection of the database context
        public LoginModel(Sp25PharmaceuticalDbContext context)
        {
            _context = context;
        }

        // Bind the form inputs
        [BindProperty]
        public string EmailAddress { get; set; }

        [BindProperty]
        public string StoreAccountPassword { get; set; }

        public bool LoginFailed { get; set; } = false;

        public IActionResult OnPost()
        {
            var storeAccount = _context.StoreAccounts
                .FirstOrDefault(a => a.EmailAddress == EmailAddress && a.StoreAccountPassword == StoreAccountPassword);

            if (storeAccount != null)
            {
                HttpContext.Session.SetString("StoreAccountDescription", storeAccount.StoreAccountDescription);
                HttpContext.Session.SetString("Role", storeAccount.Role.ToString());
                if (storeAccount.Role == 2 || storeAccount.Role == 3)
                {
                    return RedirectToPage("/Dashboard");
                }
                else
                {
                    LoginFailed = true;
                    return Page();
                }
            }
            else
            {
                LoginFailed = true;
                return Page();
            }
        }
    }
}
