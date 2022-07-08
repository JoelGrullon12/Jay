using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.ViewModels.User
{
    public class ShowUserViewModel
    {
        public UserViewModel User { get; set; }
        public List<PostViewModel> Posts { get; set; }
    }
}
