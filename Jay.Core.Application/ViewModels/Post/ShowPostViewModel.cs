using Jay.Core.Application.ViewModels.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.ViewModels.Post
{
    public class ShowPostViewModel
    {
        public PostViewModel Post { get; set; }
        public List<CommentViewModel> Comments { get; set; }
    }
}
