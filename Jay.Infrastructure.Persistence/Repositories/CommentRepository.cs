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
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly JayContext _dbContext;

        public CommentRepository(JayContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}

