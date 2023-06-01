using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using BOs.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OldCarShowroomNetworkRazorPage.Pages
{
    public class HomeModel : PageModel
    {
        public readonly UserRepository _userRepo;
        public IEnumerable<User> user { get; set; }
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Key") == null)
            {
                return RedirectToPage("./Login");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            HttpContext.Session.Remove("Key");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("./Index");
        }
    }
}
