using Jay.Core.Application.Helpers;
using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Interfaces.Services
{
    public interface ICommentService : IGenericService<Comment, CommentViewModel>
    {
        
    }
}
