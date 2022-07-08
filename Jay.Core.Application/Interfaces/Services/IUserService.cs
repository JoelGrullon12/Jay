using Jay.Core.Application.Helpers;
using Jay.Core.Application.ViewModels.Friend;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Interfaces.Services
{
    public interface IUserService : IGenericService<User, UserViewModel>
    {
        Task<UserViewModel> CheckUser(string userName);
        Task<UserViewModel> Login(LoginViewModel vm);
        Task<List<UserViewModel>> SearchAllViewModel(string search);
        Task<UserViewModel> GetByIdViewModel(int id);
        Task AddFriend(int friendId);
        Task<UserViewModel> GetByIdWithIncludesViewModel(int id);
        Task<FriendsViewModel> GetFriendsViewModel();
        Task<List<int>> GetFriendsArray();
    }
}
