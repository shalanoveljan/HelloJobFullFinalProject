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
    public class BlogService : IBlogService
    {
        public Task<IResult> CreateAsync(BlogPostDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<PagginatedResponse<BlogGetDto>> GetAllAsync(int page = 1)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<BlogUpdateDto>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(int id, BlogPostDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
