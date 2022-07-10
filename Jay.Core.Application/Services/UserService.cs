using AutoMapper;
using Jay.Core.Application.DTOs.Email;
using Jay.Core.Application.Enums;
using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Repositories;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.Friend;
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
    public class UserService : GenericService<User, UserViewModel>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _frRepository;
        private readonly IPostRepository _postRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserViewModel _user;

        public UserService(IUserRepository userRepository, IMapper mapper, IFriendRepository frRepository, 
            IHttpContextAccessor httpContext, IPostRepository postRepository, IEmailService emailService) : base(userRepository, mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _frRepository = frRepository;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.Session.Get<UserViewModel>("user");
            _postRepository = postRepository;
            _emailService = emailService;
        }

        public async Task<UserViewModel> CheckUser(string userName)
        {
            User user = await _userRepository.CheckUserAsync(userName);

            if (user == null)
                return null;

            UserViewModel userVM = _mapper.Map<UserViewModel>(user);

            return userVM;
        }

        public async Task<UserViewModel> Login(LoginViewModel vm)
        {
            User user = await _userRepository.LoginAsync(vm);

            if (user == null)
                return null;

            UserViewModel userVM = _mapper.Map<UserViewModel>(user);

            return userVM;
        }

        public override async Task<UserViewModel> Add(UserViewModel vm)
        {
            vm.Password = Stuff.EncryptSHA256(vm.Password);
            User user = _mapper.Map<User>(vm);

            user = await _userRepository.AddAsync(user);

            UserViewModel userVM = _mapper.Map<UserViewModel>(user);

            await SendVerificationEmail(userVM.Id);

            return userVM;
        }

        public async Task SendVerificationEmail(int id)
        {
            string key = await _userRepository.GetKeyAsync(id);
            var user = await _userRepository.GetByIdAsync(id);

            await _emailService.SendAsync(new EmailRequest
            {
                To = user.Email,
                Subject = "Bienvenido/a a Jay",
                Body = Stuff.SetVerifyEmail("/media/app/secondary-logo.png", $"/Access/VerifyAccount?id={id}&key={key}")
            });
        }

        public async Task SendResetEmail(string email, string name, string pass)
        {
            await _emailService.SendAsync(new EmailRequest
            {
                To = email,
                Subject = "Contraseña restaurada",
                Body = Stuff.SetResetEmail(name, pass)
            });
        }

        public async Task<List<UserViewModel>> SearchAllViewModel(string search)
        {
            var userList = await _userRepository.GetAllAsync();
            List<UserViewModel> users = _mapper.Map<List<UserViewModel>>(userList);
            List<UserViewModel> userVM = new List<UserViewModel>(users);

            foreach (UserViewModel user in users)
            {
                if (user.Id == _user.Id)
                {
                    userVM.Remove(user);
                }else if (!user.Name.ToLower().Contains(search.ToLower()) && !user.UserName.ToLower().Contains(search.ToLower()))
                {
                    userVM.Remove(user);
                }
            }

            return userVM;
        }

        public override async Task<UserViewModel> GetByIdViewModel(int id)
        {
            var user = await _userRepository.GetByIdWithIncludeAsync(id, new List<string> { "Posts" });

            return _mapper.Map<UserViewModel>(user);
        }

        public async Task<UserViewModel> GetByIdWithIncludesViewModel(int id)
        {
            var user = await _userRepository.GetByIdWithIncludeAsync(id, new List<string> { "Posts", "Comments" });

            for(int i = 0; i < user.Posts.Count; i++)
            {
                Post post = await _postRepository.GetByIdWithIncludeAsync(user.Posts.ElementAt(i).Id, new List<string>(), new List<string> { "Comments" });
                user.Posts.ElementAt(i).Comments = post.Comments;

                for (int e = 0; e < post.Comments.Count; e++)
                {
                    UserViewModel userVM = await GetByIdViewModel(post.Comments.ElementAt(e).UserId);
                    post.Comments.ElementAt(e).User = _mapper.Map<User>(userVM);
                }
            }

            user.Posts = user.Posts.OrderByDescending(p => p.Created).ToList();
            return _mapper.Map<UserViewModel>(user);
        }

        public async Task AddFriend(int friendId)
        {
            Friend fr = new()
            {
                UserId = _user.Id,
                FriendId = friendId
            };
            await _frRepository.AddAsync(fr);
        }

        public async Task<FriendsViewModel> GetFriendsViewModel()
        {
            var user = await _userRepository.GetByIdWithIncludeAsync(_user.Id, new List<string> { "Users", "Friends" });
            FriendsViewModel friends = new()
            {
                Users = _mapper.Map<ICollection<UserViewModel>>(user.Users),
                Friends = _mapper.Map<ICollection<UserViewModel>>(user.Friends)
            };

            return friends;
        }

        public async Task<List<int>> GetFriendsArray()
        {
            var user = await _userRepository.GetByIdWithIncludeAsync(_user.Id, new List<string> { "Users", "Friends" });

            List<int> ids = new List<int>();

            foreach(User u in user.Users)
            {
                if(!ids.Contains(u.Id))
                    ids.Add(u.Id);
            }

            foreach (User u in user.Friends)
            {
                if (!ids.Contains(u.Id))
                    ids.Add(u.Id);
            }

            return ids;
        }

        public async Task DeleteFriend(int userId, int frId, int frType)
        {
            Friend friend = new();
            if (frType == 1)
                friend = await _frRepository.GetFriendshipAsync(userId, frId);
            else if (frType == 2)
                friend = await _frRepository.GetFriendshipAsync(frId, userId);

            await _frRepository.DeleteAsync(friend);
        }

        public async Task<bool> VerifyAccountWithKey(int id, string key)
        {
            return await _userRepository.VerifyAccountAsync(id, key);
        }

        public async Task<bool> IsUserActive(int id)
        {
            return await _userRepository.IsUserActiveAsync(id);
        }
    }
}
