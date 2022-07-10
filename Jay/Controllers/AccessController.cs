using Jay.Core.Application.DTOs.Email;
using Jay.Core.Application.Enums;
using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.User;
using Jay.Presentation.WebApp.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Jay.Presentation.WebApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ValidateSession _session;
        private readonly IHttpContextAccessor _httpContext;

        public AccessController(IUserService userService, ValidateSession session, IEmailService emailService, IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _session = session;
            _emailService = emailService;
            _httpContext = httpContext;
        }

        public IActionResult Index(bool userNameErr = false)
        {
            if (_session.HasUser())
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            ViewData["userNameErr"] = userNameErr;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {

            if (_session.HasUser())
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            if (!ModelState.IsValid)
                return View(vm);

            UserViewModel user = await _userService.CheckUser(vm.UserName);

            if (user == null)
            {
                ModelState.AddModelError("userNotExistent", "Este nombre de usuario no existe");
                return View(vm);
            }

            user = await _userService.Login(vm);

            if (user != null)
            {
                HttpContext.Session.Set<UserViewModel>("user", user);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
                ModelState.AddModelError("userValidation", "Contraseña incorrecta");

            return View(vm);
        }

        public IActionResult Register()
        {
            if (_session.HasUser())
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel vm, IFormFile img)
        {
            if (_session.HasUser())
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            if (!ModelState.IsValid)
                return View(vm);

            UserViewModel user = await _userService.CheckUser(vm.UserName);

            if (user != null)
            {
                ViewData["userExistent"] = "Este nombre de usuario ya esta en uso";
                return View(vm);
            }

            vm.ProfilePicture = img.FileName;
            HttpContext.Session.Set<UserViewModel>("user", vm);
            UserViewModel userVM = await _userService.Add(vm);

            if (userVM != null && userVM.Id != 0)
            {
                userVM.ProfilePicture = UploadImg(img, userVM.Id);
                await _userService.DML(userVM, DMLAction.Update, userVM.Id);
            }
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index");
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "Access", action = "Index" });
        }

        public async Task<IActionResult> VerifyAccount(int id, string key)
        {
            bool verified = await _userService.VerifyAccountWithKey(id, key);
            ViewData["verified"] = verified;
            return View();
        }

        public async Task<IActionResult> VerifyEmail(bool send=false)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });
            
            if(send)
                await _userService.SendVerificationEmail(_httpContext.HttpContext.Session.Get<UserViewModel>("user").Id);

            return View();
        }


        public async Task<IActionResult> ResetPasswordGet(string UserName)
        {
            if (_session.HasUser())
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            UserViewModel user = await _userService.CheckUser(UserName);

            if (user == null)
            {
                return RedirectToAction("Index", new { userNameErr = true });
            }

            Guid guid = Guid.NewGuid();
            string[] guids = guid.ToString().Split("-");
            string pass = "";
            for (int i = 0; i < guids.Length; i++)
            {
                pass += guids[i];
            }
            user.Password = Stuff.EncryptSHA256(pass);
            await _userService.DML(user, DMLAction.Update, user.Id);

            await _userService.SendResetEmail(user.Email, user.Name, pass);

            ViewData["userName"] = UserName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string UserName)
        {
            if (_session.HasUser())
                return RedirectToRoute(new { controller = "Home", action = "Index" });

            UserViewModel user = await _userService.CheckUser(UserName);

            if (user == null)
            {
                return RedirectToAction("Index", new { userNameErr = true });
            }

            Guid guid = Guid.NewGuid();
            string[] guids = guid.ToString().Split("-");
            string pass = "";
            for (int i = 0; i < guids.Length; i++)
            {
                pass += guids[i];
            }
            user.Password = Stuff.EncryptSHA256(pass);
            await _userService.DML(user, DMLAction.Update, user.Id);

            await _userService.SendResetEmail(user.Email, user.Name, pass);

            ViewData["userName"] = UserName;
            return View();
        }


        private string UploadImg(IFormFile file, int id, bool editMode = false, string imgUrl = "")
        {
            if (editMode && file == null)
            {
                return imgUrl;
            }
            string basePath = $"/media/profile/{id}";

            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string filename = guid + fileInfo.Extension;

            string filePath = Path.Combine(path, filename);

            using (var strem = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(strem);
            }

            if (editMode)
            {
                string[] oldImgF = imgUrl.Split("/");
                string oldImg = oldImgF[^1];
                string oldPath = Path.Combine(path, oldImg);
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            return $"{basePath}/{filename}";
        }
    }
}
