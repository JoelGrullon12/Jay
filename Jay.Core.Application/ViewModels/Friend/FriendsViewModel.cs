using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.Post;
using Jay.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.ViewModels.Friend
{
    public class FriendsViewModel
    {
        public ICollection<UserViewModel> Users { get; set; }
        public ICollection<UserViewModel> Friends { get; set; }
    }
}
