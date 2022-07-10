using Jay.Core.Application.Helpers;
using Jay.Core.Application.Interfaces.Repositories;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;
using Jay.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly JayContext _dbContext;

        public UserRepository(JayContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CheckUserAsync(string userName)
        {
            User user = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(user => user.Username == userName);
            return user;
        }

        public async Task<User> LoginAsync(LoginViewModel vm)
        {
            string pass = Stuff.EncryptSHA256(vm.Password);
            User user = await _dbContext.Set<User>()
                .FirstOrDefaultAsync(user => user.Username == vm.UserName && user.Password == pass);
            return user;
        }

        public async Task<User> GetByIdWithIncludeAsync(int id, List<string> props)
        {
            var query = await _dbContext.Set<User>().FindAsync(id);

            foreach (string prop in props)
            {
                _dbContext.Entry(query).Collection(prop).Load();
            }

            return query;
        }

        public async Task<string> GetKeyAsync(int id)
        {
            var user = await _dbContext.Set<User>().FindAsync(id);
            return user.Password;
        }

        public async Task<bool> VerifyAccountAsync(int id, string key)
        {
            var user = await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Id == id && u.Password == key);

            if (user == null)
            {
                return false;
            }

            user.Active = true;

            await UpdateAsync(user, id);
            return true;
        }

        public async Task<bool> IsUserActiveAsync(int id)
        {
            var user = await _dbContext.Set<User>().FindAsync(id);

            return user.Active;
        }
    }
}

