using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.User;
using Jay.Presentation.WebApp.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Jay.Presentation.WebApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _comService;
        private readonly ValidateSession _session;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserViewModel _user;

        public PostsController(IPostService postService, ValidateSession session, IHttpContextAccessor httpContext, ICommentService comService)
        {
            _postService = postService;
            _session = session;
            _httpContext = httpContext;
            _comService = comService;
            _user = _httpContext.HttpContext.Session.Get<UserViewModel>("user");
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyPosts()
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            return View(await _postService.GetSpecificViewModel(true));
        }

        public async Task<IActionResult> FriendPosts()
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            return View(await _postService.GetSpecificViewModel(false));
        }

        public async Task<IActionResult> Post(int id)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            return View(await _postService.GetByIdWithIncludesViewModel(id));
        }
    }
}
