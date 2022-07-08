using AutoMapper;
using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Repositories;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.Friend;
using Jay.Core.Application.ViewModels.Post;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Services
{
    public class PostService : GenericService<Post, PostViewModel>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserViewModel _user;
        private readonly IUserService _userService;

        public PostService(IPostRepository postRepository, IMapper mapper, IHttpContextAccessor httpContext, IUserService userService) : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.Session.Get<UserViewModel>("user");
            _userService = userService;
        }

        public override async Task<List<PostViewModel>> GetAllViewModel()
        {
            var postList = await _postRepository.GetAllWithIncludesAsync(new List<string>() { "User", "Comments" });
            FriendsViewModel frVM = await _userService.GetFriendsViewModel();
            List<Post> posts = new List<Post>();

            foreach (UserViewModel u in frVM.Users)
            {
                posts.AddRange(postList.Where(p => p.UserId == u.Id));
            }

            foreach (UserViewModel u in frVM.Friends)
            {
                posts.AddRange(postList.Where(p => p.UserId == u.Id));
            }

            posts.AddRange(postList.Where(p => p.UserId == _user.Id));

            posts = posts.OrderByDescending(p => p.Created).ToList();

            return _mapper.Map<List<PostViewModel>>(posts);
        }

        public async Task<List<PostViewModel>> GetSpecificViewModel(bool areMines)
        {
            var postList = await _postRepository.GetAllWithIncludesAsync(new List<string>() { "User", "Comments" });
            FriendsViewModel frVM = await _userService.GetFriendsViewModel();
            List<Post> posts = new List<Post>();

            foreach(UserViewModel u in frVM.Users)
            {
                posts.AddRange(postList.Where(p => p.UserId == u.Id));
            }

            foreach (UserViewModel u in frVM.Friends)
            {
                posts.AddRange(postList.Where(p => p.UserId == u.Id));
            }

            posts.AddRange(postList.Where(p => p.UserId == _user.Id));

            posts = areMines ? posts.Where(p => p.UserId == _user.Id).ToList()
                : posts.Where(p => p.UserId != _user.Id).ToList();

            posts = posts.OrderByDescending(p => p.Created).ToList();

            return _mapper.Map<List<PostViewModel>>(posts);
        }

        public async Task<PostViewModel> GetByIdWithIncludesViewModel(int id)
        {
            var post = await _postRepository.GetPostForShowAsync(id);

            PostViewModel postVM = _mapper.Map<PostViewModel>(post);

            for (int i=0; i < postVM.Comments.Count; i++)
            {
                UserViewModel userVM = await _userService.GetByIdViewModel(postVM.Comments.ElementAt(i).UserId);
                postVM.Comments.ElementAt(i).User = userVM;
            }

            return postVM;
        }

    }
}
