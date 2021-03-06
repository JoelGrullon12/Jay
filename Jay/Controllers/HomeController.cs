using Jay.Core.Application.Enums;
using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.Post;
using Jay.Core.Application.ViewModels.User;
using Jay.Presentation.WebApp.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jay.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly ICommentService _comService;
        private readonly IUserService _userService;
        private readonly ValidateSession _session;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserViewModel _user;

        public HomeController(IPostService postService, ValidateSession session, IHttpContextAccessor httpContext, ICommentService comService, IUserService userService)
        {
            _postService = postService;
            _session = session;
            _httpContext = httpContext;
            _comService = comService;
            _user = _httpContext.HttpContext.Session.Get<UserViewModel>("user");
            _userService = userService;
        }

        public async Task<IActionResult> Index(bool errFileFormat = false)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            if (!await _userService.IsUserActive(_user.Id))
                return RedirectToRoute(new { controller = "Access", action = "VerifyEmail" });

            ViewBag.ErrFormat = errFileFormat;

            return View(await _postService.GetAllViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> AddWithMedia(string postTitle, string postText, IFormFile postMedia)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            if (!await _userService.IsUserActive(_user.Id))
                return RedirectToRoute(new { controller = "Access", action = "VerifyEmail" });

            FileInfo fileInfo = new(postMedia.FileName);
            int mediaType;

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
                    return RedirectToAction("Index", new { errFileFormat=true });
            }

            PostViewModel vm = new()
            {
                PostTitle = postTitle,
                PostText = postText,
                MediaUrl = postMedia.FileName,
                MediaType = mediaType,
                UserId = _user.Id
            };

            PostViewModel postVM = await _postService.Add(vm);

            if (postVM != null && postVM.Id != 0)
            {
                postVM.MediaUrl = UploadMedia(postMedia, postVM.Id, (MediaType)mediaType);
                await _postService.DML(postVM, DMLAction.Update, postVM.Id);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> Add(string postTitle, string postText)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            if (!await _userService.IsUserActive(_user.Id))
                return RedirectToRoute(new { controller = "Access", action = "VerifyEmail" });

            PostViewModel vm = new()
            {
                PostTitle = postTitle,
                PostText = postText,
                MediaUrl = null,
                MediaType = (int)MediaType.NoMedia,
                UserId = _user.Id
            };

            await _postService.DML(vm, DMLAction.Insert);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(int postId, string commentText)
        {
            if (!_session.HasUser())
                return RedirectToRoute(new { controller = "Access", action = "Index" });

            if (!await _userService.IsUserActive(_user.Id))
                return RedirectToRoute(new { controller = "Access", action = "VerifyEmail" });

            CommentViewModel vm = new()
            {
                Text = commentText,
                UserId = _user.Id,
                PostId = postId
            };

            await _comService.DML(vm, DMLAction.Insert);

            return RedirectToAction("Post", "Posts", new { id = postId });
        }

        //Metodo SOLO para subir la multimedia, no edita porque no la necesita///////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private string UploadMedia(IFormFile file, int id, MediaType mediaType)
        {
            string basePath = "";

            switch (mediaType)
            {
                case MediaType.Image:
                    basePath = $"/media/post/img/{id}";
                    break;

                case MediaType.Video:
                    basePath = $"/media/post/vid/{id}";
                    break;
            }

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

            return $"{basePath}/{filename}";
        }
    }
}


//Lo comento por aqui para no perderlo XD
//private string UploadMedia(IFormFile file, int id, MediaType mediaType, bool editMode = false, string imgUrl = "")
//{
//    if (editMode && file == null)
//    {
//        return imgUrl;
//    }

//    string basePath = "";

//    switch (mediaType)
//    {
//        case MediaType.Image:
//            basePath = $"/media/post/img/{id}";
//            break;

//        case MediaType.Video:
//            basePath = $"/media/post/vid/{id}";
//            break;
//    }

//    string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

//    if (!Directory.Exists(path))
//        Directory.CreateDirectory(path);

//    Guid guid = Guid.NewGuid();
//    FileInfo fileInfo = new(file.FileName);
//    string filename = guid + fileInfo.Extension;

//    string filePath = Path.Combine(path, filename);

//    using (var strem = new FileStream(filePath, FileMode.Create))
//    {
//        file.CopyTo(strem);
//    }

//    if (editMode)
//    {
//        string[] oldImgF = imgUrl.Split("/");
//        string oldImg = oldImgF[^1];
//        string oldPath = Path.Combine(path, oldImg);
//        if (System.IO.File.Exists(oldPath))
//            System.IO.File.Delete(oldPath);
//    }

//    return $"{basePath}/{filename}";
//}