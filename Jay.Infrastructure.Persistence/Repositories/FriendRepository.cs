using Jay.Core.Application.Interfaces.Repositories;
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
    public class FriendRepository : GenericRepository<Friend>, IFriendRepository
    {
        private readonly JayContext _dbContext;

        public FriendRepository(JayContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Friend> GetFriendshipAsync(int userid, int frid)
        {
            return await _dbContext.Set<Friend>().FirstOrDefaultAsync(
                fr => fr.UserId == userid && fr.FriendId == frid);
        }
    }
}

