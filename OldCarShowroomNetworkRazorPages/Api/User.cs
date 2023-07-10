using BOs.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using REPOs;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace OldCarShowroomNetworkRazorPages.Api
{
	[Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
		public readonly UserRepository _userRepo;
		public User(UserRepository userRepo)
        {
            _context = new OldCarShowroomNetworkContext();
			_userRepo = userRepo;

		}
		
		[HttpGet]
        [Route("login-google")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(e => e.Email.Equals(email));
			
			if (user != null && user.RoleId == 1)
            {
				ClaimsIdentity identity = null;
				identity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name,email),
					new Claim(ClaimTypes.Role, "User")
				}, CookieAuthenticationDefaults.AuthenticationScheme);

				var principal = new ClaimsPrincipal(identity);
				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				HttpContext.Session.SetString("Key", email);
				HttpContext.Session.SetString("Role", user.RoleId.ToString());

				return Ok(user);
            }
			else if (user != null && user.RoleId == 0)
			{
				ClaimsIdentity identity = null;
				identity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name,email),
					new Claim(ClaimTypes.Role, "Admin")
				}, CookieAuthenticationDefaults.AuthenticationScheme);

				var principal = new ClaimsPrincipal(identity);
				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				HttpContext.Session.SetString("Key", email);
				HttpContext.Session.SetString("Role", user.RoleId.ToString());

				return Ok(user);
			}
			else if (user != null && user.RoleId == 2)
			{
				ClaimsIdentity identity = null;
				identity = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.Name,email),
					new Claim(ClaimTypes.Role, "Staff")
				}, CookieAuthenticationDefaults.AuthenticationScheme);

				var principal = new ClaimsPrincipal(identity);
				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
				HttpContext.Session.SetString("Key", email);
				HttpContext.Session.SetString("Role", user.RoleId.ToString());

				return Ok(user);
			}

			return Ok(1);
        }
		[HttpPost]
		[Route("login-google1")]
		public IActionResult GetUserByCreateEmail(string email, string username)
		{
            
            var user = new BOs.Models.User {
				Username = username,
                Email = email,
                RoleId = 1,
                Address = "",
                Password = "",
                Phone = "",
                FullName= "",
			};
			_userRepo.Add(user);
			ClaimsIdentity identity = null;
			identity = new ClaimsIdentity(new[]
			{
					new Claim(ClaimTypes.Name,email),
					new Claim(ClaimTypes.Role, "User")
				}, CookieAuthenticationDefaults.AuthenticationScheme);

			var principal = new ClaimsPrincipal(identity);
			HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
			HttpContext.Session.SetString("Key", email);
			HttpContext.Session.SetString("Role", user.RoleId.ToString());
        
			return Ok(user);
		}
	}
}
