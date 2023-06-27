using AspNetCoreHero.ToastNotification.Abstractions;
using BOs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REPOs;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace OldCarShowroomNetworkRazorPages.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class User : ControllerBase
    {
        private readonly BOs.Models.OldCarShowroomNetworkContext _context;
        public readonly UserRepository _userRepo;
        private readonly INotyfService _toastNotification;
        public User(UserRepository userRepo, INotyfService toastNotification)
        {
            _context = new OldCarShowroomNetworkContext();
            _userRepo = userRepo;
            _toastNotification = toastNotification;
        }
        /*[HttpGet]
        [Route("login-google")]
        public IActionResult getUserByEmail(string email)
        {
            var checkLoginByEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(email)); ;

            if (checkLoginByEmail != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, "Email already in use.");
            }
            
        }*/
    }
}
