using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> CheckUserAsync(string userName);
        Task<User> LoginAsync(LoginViewModel vm);
        Task<User> GetByIdWithIncludeAsync(int id, List<string> props);
        Task<string> GetKeyAsync(int id);
        Task<bool> VerifyAccountAsync(int id, string key);
        Task<bool> IsUserActiveAsync(int id);
    }
}
