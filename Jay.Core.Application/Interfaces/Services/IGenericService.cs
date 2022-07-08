using Jay.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Interfaces.Services
{
    public interface IGenericService<T, VM>
        where T : class
        where VM : class
    {
        Task<List<VM>> GetAllViewModel();
        Task<VM> GetByIdViewModel(int id);
        Task DML(VM vm, DMLAction action, int id = 0);
        Task<VM> Add(VM vm);
    }
}
