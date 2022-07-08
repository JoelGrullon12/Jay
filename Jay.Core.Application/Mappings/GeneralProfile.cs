using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Jay.Core.Application.ViewModels.Comment;
using Jay.Core.Application.ViewModels.Post;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;

namespace Jay.Core.Application.Mappings
{
    public class GeneralProfile:Profile
    {
        public GeneralProfile()
        {
            CreateMap<Post, PostViewModel>()
            .ReverseMap()
                .ForMember(d=>d.CreatedBy, o=>o.Ignore())
                .ForMember(d=>d.Modified, o=>o.Ignore())
                .ForMember(d=>d.ModifiedBy, o=>o.Ignore());

            CreateMap<User, UserViewModel>()
                .ForMember(d=>d.RepPassword, o=>o.Ignore())
            .ReverseMap()
                .ForMember(d=>d.Created, o=>o.Ignore())
                .ForMember(d=>d.CreatedBy, o=>o.Ignore())
                .ForMember(d=>d.Modified, o=>o.Ignore())
                .ForMember(d=>d.ModifiedBy, o=>o.Ignore())
                .ForMember(d=>d.Users, o=>o.Ignore())
                .ForMember(d=>d.Friends, o=>o.Ignore())
                .ForMember(d=>d.UserFriends, o=>o.Ignore())
                .ForMember(d=>d.FriendsUser, o=>o.Ignore())
                ;

            CreateMap<Comment, CommentViewModel>()
            .ReverseMap()
                .ForMember(d=>d.CreatedBy, o=>o.Ignore())
                .ForMember(d=>d.Modified, o=>o.Ignore())
                .ForMember(d=>d.ModifiedBy, o=>o.Ignore());
        }
    }
}
