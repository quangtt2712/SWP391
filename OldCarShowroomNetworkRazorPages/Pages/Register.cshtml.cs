using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using BOs.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace OldCarShowroomNetworkRazorPage.Pages
{
    public class RegisterModel : PageModel
    {
        public readonly UserRepository _userRepo;
        private readonly INotyfService _toastNotification;

        public RegisterModel(UserRepository userRepo, INotyfService toastNotification)
        {
            _userRepo = userRepo;
            _toastNotification = toastNotification;
        }

        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        public string Msg3 { get; set; }
        public string Msg4 { get; set; }
        public string Msg5 { get; set; }
        public string Msg6 { get; set; }
        
        [BindProperty]
        public User user { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var checkUserName = _userRepo.GetAll().FirstOrDefault(p => p.Username.Equals(user.Username));
            var checkEmail = _userRepo.GetAll().FirstOrDefault(p => p.Email.Equals(user.Email));
            var checkPhone= _userRepo.GetAll().FirstOrDefault(p => p.Phone.Equals(user.Phone));
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (user.Username == null)
            {
                Msg1 = "Bạn cần phải nhập tài khoản";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (checkUserName != null)
            {
                Msg1 = "Tài khoản đã tồn tại";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (user.Password == null)
            {
                Msg2 = "Bạn cần phải nhập mật khẩu";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (user.Address == null)
            {
                Msg3 = "Bạn cần phải nhập địa chỉ";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (user.Email == null)
            {
                Msg4 = "Bạn cần phải nhập Email";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (checkEmail != null)
            {
                Msg4 = "Email đã tồn tại";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (user.Phone == null)
            {
                Msg5 = "Bạn cần phải nhập SĐT";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (checkPhone != null)
            {
                Msg5 = "SĐT đã tồn tại";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            if (user.FullName == null)
            {
                Msg6 = "Bạn cần phải nhập Họ và tên";
                _toastNotification.Error("Đăng kí thất bại");
                return Page();
            }
            user.RoleId = 1;
            _userRepo.Add(user);
            HttpContext.Session.SetString("Key", user.Email);
            _toastNotification.Success("Đăng kí thành công");
            return RedirectToPage("./Login");
        }
    }
}
