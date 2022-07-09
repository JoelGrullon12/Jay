using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string PostText { get; set; }
        public string MediaUrl { get; set; }
        public int MediaType { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
        public UserViewModel User { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
    }
}
