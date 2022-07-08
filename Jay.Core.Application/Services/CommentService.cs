using AutoMapper;
using Jay.Core.Application.Interfaces.Repositories;
using Jay.Core.Application.Interfaces.Services;
using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.Post;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Services
{
    public class CommentService : GenericService<Comment, CommentViewModel>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper):base(commentRepository, mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

    }
}
