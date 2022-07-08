using Jay.Core.Application.Helpers;
using Jay.Core.Application.ViewModels.Post;
using Jay.Core.Application.ViewModels.User;
using Jay.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<Post, PostViewModel>
    {
        Task<List<PostViewModel>> GetSpecificViewModel(bool areMines);
        Task<List<PostViewModel>> GetAllViewModel();
        Task<PostViewModel> GetByIdWithIncludesViewModel(int id);
    }
}
