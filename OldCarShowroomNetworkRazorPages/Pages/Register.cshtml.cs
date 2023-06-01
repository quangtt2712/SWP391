using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using REPOs;
using BOs.Models;
using System.Threading.Tasks;
using System.Linq;

namespace OldCarShowroomNetworkRazorPage.Pages
{
    public class RegisterModel : PageModel
    {
        public readonly UserRepository _userRepo;


        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
        public string Msg3 { get; set; }
        public string Msg4 { get; set; }
        public string Msg5 { get; set; }
        public string Msg6 { get; set; }

        public RegisterModel(UserRepository userRepo) => _userRepo = userRepo;
        
        [BindProperty]
        public User user { get; set; }

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
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
                return Page();
            }
            if (checkUserName != null)
            {
                Msg1 = "Tài khoản đã tồn tại";
                return Page();
            }
            if (user.Password == null)
            {
                Msg2 = "Bạn cần phải nhập mật khẩu";
                return Page();
            }
            if (user.Address == null)
            {
                Msg3 = "Bạn cần phải nhập địa chỉ";
                return Page();
            }
            if (user.Email == null)
            {
                Msg4 = "Bạn cần phải nhập Email";
                return Page();
            }
            if (checkEmail != null)
            {
                Msg4 = "Email đã tồn tại";
                return Page();
            }
            if (user.Phone == null)
            {
                Msg5 = "Bạn cần phải nhập SĐT";
                return Page();
            }
            if (checkPhone != null)
            {
                Msg5 = "SĐT đã tồn tại";
                return Page();
            }
            if (user.FullName == null)
            {
                Msg6 = "Bạn cần phải nhập Họ và tên";
                return Page();
            }
            user.RoleId= 1;
            _userRepo.Add(user);
            return RedirectToPage("./Home");
        }
    }
}
