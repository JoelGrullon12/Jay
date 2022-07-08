using AutoMapper;
using Jay.Core.Application.Enums;
using Jay.Core.Application.Interfaces.Repositories;
using Jay.Core.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jay.Core.Application.Services
{
    public class GenericService<T, VM> : IGenericService<T, VM>
        where T : class
        where VM : class
    {
        private readonly IGenericRepository<T> _repo;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<T> genericRepo, IMapper mapper)
        {
            _repo = genericRepo;
            _mapper = mapper;
        }

        public virtual async Task<List<VM>> GetAllViewModel()
        {
            var tList = await _repo.GetAllAsync();

            return _mapper.Map<List<VM>>(tList);
        }

        public virtual async Task<VM> GetByIdViewModel(int id)
        {
            T t = await _repo.GetByIdAsync(id);
            VM vm = _mapper.Map<VM>(t);

            return vm;
        }

        public virtual async Task DML(VM vm, DMLAction action, int id = 0)
        {
            T t = _mapper.Map<T>(vm);

            switch (action)
            {
                case DMLAction.Insert:
                    await _repo.AddAsync(t);
                    break;

                case DMLAction.Update:
                    await _repo.UpdateAsync(t, id);
                    break;

                case DMLAction.Delete:
                    await _repo.DeleteAsync(t);
                    break;
            }
        }

        public virtual async Task<VM> Add(VM vm)
        {
            T t = _mapper.Map<T>(vm);

            t = await _repo.AddAsync(t);

            VM gvm = _mapper.Map<VM>(t);

            return gvm;
        }
    }
}
