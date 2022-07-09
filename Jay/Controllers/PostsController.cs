using Jay.Core.Application.Enums;
using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.Post;
using Jay.Core.Application.ViewModels.User;
using Jay.Presentation.WebApp.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
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

        public async Task<IActionResult> Edit(int id, bool errFileFormat = false)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            ViewBag.ErrFormat = errFileFormat;

            return View(await _postService.GetByIdWithIncludesViewModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm, IFormFile media)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            PostViewModel oldpost = await _postService.GetByIdViewModel(vm.Id);

            int mediaType=oldpost.MediaType;

            if (media != null)
            {
                FileInfo fileInfo = new(media.FileName);

                switch (fileInfo.Extension)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                        mediaType = (int)MediaType.Image;
                        break;

                    case ".mp4":
                    case ".mkv":
                    case ".flv":
                        mediaType = (int)MediaType.Video;
                        break;

                    default:
                        return RedirectToAction("Edit", new { id = vm.Id, errFileFormat = true });
                }
            }

            vm.MediaUrl = UploadMedia(media, vm.Id, (MediaType)mediaType, oldpost.MediaType, oldpost.MediaUrl);

            vm.UserId = _user.Id;
            vm.Created = oldpost.Created;
            vm.MediaType = mediaType;
            await _postService.DML(vm, DMLAction.Update, vm.Id);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }


        public async Task<IActionResult> Delete(int id)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            PostViewModel vm = await _postService.GetByIdWithIncludesViewModel(id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PostViewModel vm)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });


            await _postService.DML(vm, DMLAction.Delete);

            if (vm.MediaType != 0)
            {
                string basePath = "";

                switch (vm.MediaType)
                {
                    case 1:
                        basePath = $"/media/post/img/{vm.Id}";
                        break;

                    case 2:
                        basePath = $"/media/post/vid/{vm.Id}";
                        break;
                }

                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

                if (Directory.Exists(path))
                {
                    DirectoryInfo dir = new(path);
                    foreach (FileInfo file in dir.GetFiles())
                    {
                        file.Delete();
                    }

                    foreach (DirectoryInfo file in dir.GetDirectories())
                    {
                        file.Delete(true);
                    }

                    Directory.Delete(path);
                }
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        //////Subir multimedia///////////////////////////////////////////////////////////////////////////////
        private string UploadMedia(IFormFile file, int id, MediaType mediaType, int oldType, string oldFile = "")
        {
            //Si no lo cambiaron
            if (file == null)
            {
                return oldFile;
            }

            string basePath = "";
            string path = "";

            //Borrar el archivo antiguo
            if (oldType != 0)
            {
                if (System.IO.File.Exists(oldFile))
                    System.IO.File.Delete(oldFile);

                basePath = "";

                switch (oldType)
                {
                    case 1:
                        basePath = $"/media/post/img/{id}";
                        break;

                    case 2:
                        basePath = $"/media/post/vid/{id}";
                        break;
                }

                path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

                if (Directory.Exists(path))
                {
                    DirectoryInfo dir = new(path);
                    foreach (FileInfo fileinfo in dir.GetFiles())
                    {
                        fileinfo.Delete();
                    }

                    foreach (DirectoryInfo fileinfo in dir.GetDirectories())
                    {
                        fileinfo.Delete(true);
                    }

                    Directory.Delete(path);
                }
            }


            //Guardar el archivo nuevo
            basePath = "";

            switch (mediaType)
            {
                case MediaType.Image:
                    basePath = $"/media/post/img/{id}";
                    break;

                case MediaType.Video:
                    basePath = $"/media/post/vid/{id}";
                    break;
            }

            path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

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

            return $"{basePath}/{filename}";
        }
    }
}
