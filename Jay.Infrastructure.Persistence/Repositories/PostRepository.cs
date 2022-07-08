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
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly JayContext _dbContext;

        public PostRepository(JayContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<Post>> GetAllWithIncludesAsync(List<string> props)
        {
            var query = _dbContext.Set<Post>().OrderByDescending(p=>p.Created).AsQueryable();

            foreach (string prop in props)
            {
                query = query.Include(prop);
            }

            return await query.ToListAsync();
        }

        public virtual async Task<Post> GetPostForShowAsync(int id)
        {
            var query = await _dbContext.Set<Post>().FindAsync(id);

            _dbContext.Entry(query).Reference("User").Load();
            _dbContext.Entry(query).Collection("Comments").Load();

            return query;
        }
    }
}

