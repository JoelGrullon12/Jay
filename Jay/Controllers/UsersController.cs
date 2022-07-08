using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.User;
using Jay.Presentation.WebApp.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jay.Presentation.WebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IPostService _postService;
        private readonly ValidateSession _session;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserViewModel _user;

        public UsersController(IUserService userService, ValidateSession session, IHttpContextAccessor httpContext, IPostService postService)
        {
            _userService = userService;
            _session = session;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.Session.Get<UserViewModel>("user");
            _postService = postService;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            return View(await _userService.GetByIdViewModel(id));
        }

        public async Task<IActionResult> Search(string search)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            if (search != null)
            {
                ViewData["searchParam"] = search;
                return View(await _userService.SearchAllViewModel(search));
            }

            return View();

        }

        public async Task<IActionResult> UserDetails(int id)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            return View(await _userService.GetByIdWithIncludesViewModel(id));
        }

        public async Task<IActionResult> AddFriend(string urlFrom, int frId)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            await _userService.AddFriend(frId);

            return Redirect(urlFrom);
        }
    }
}
