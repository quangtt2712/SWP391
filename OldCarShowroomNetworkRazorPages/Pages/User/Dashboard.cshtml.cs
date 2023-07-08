using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OldCarShowroomNetworkRazorPages.Pages.User
{
    [Authorize(Roles = "User")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
