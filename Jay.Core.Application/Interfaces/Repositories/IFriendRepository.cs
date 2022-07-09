using Jay.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Interfaces.Repositories
{
    public interface IFriendRepository : IGenericRepository<Friend>
    {
        Task<Friend> GetFriendshipAsync(int userid, int frid);
    }
}
