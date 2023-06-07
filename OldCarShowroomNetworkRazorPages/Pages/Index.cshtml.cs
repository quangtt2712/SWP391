using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using REPOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BOs.Models;

namespace OldCarShowroomNetworkRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        public readonly UserRepository _userRepo;

        public string Key { get; set; }
        public string Role { get; set; }

        public IndexModel(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Key") == null)
            {
                return Page();
            }else if(HttpContext.Session.GetString("Key") != null) {
                return Page();
            }
            return RedirectToPage("./Login");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            HttpContext.Session.Remove("Key");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Page();
        }

    }
}
