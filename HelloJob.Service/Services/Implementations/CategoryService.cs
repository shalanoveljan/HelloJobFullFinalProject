using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Entities.DTOS;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        public Task<IResult> CreateAsync(CategoryPostDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<PagginatedResponse<CategoryGetDto>> GetAllAsync(int page = 1)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<CategoryUpdateDto>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(int id, CategoryPostDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
