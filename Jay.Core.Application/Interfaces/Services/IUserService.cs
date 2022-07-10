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
        Task DeleteFriend(int userId, int frId, int frType);
        Task<UserViewModel> Add(UserViewModel vm);
        Task<bool> VerifyAccountWithKey(int id, string key);
        Task<bool> IsUserActive(int id);
        Task SendVerificationEmail(int id);
        Task SendResetEmail(string email, string name, string pass);
    }
}
