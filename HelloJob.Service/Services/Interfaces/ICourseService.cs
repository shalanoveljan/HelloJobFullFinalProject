using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Entities.DTOS;
using HelloJob.Service.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Interfaces
{
    public interface ICourseService
    {
        public Task<PagginatedResponse<CourseGetDto>> GetAllAsync(int pageNumber=1, int pageSize=6);

        public Task<IResult> CreateAsync(CoursePostDto dto);

        public Task<IResult> RemoveAsync(int id);

        public Task<IResult> UpdateAsync(int id, CoursePostDto dto);
        public Task<IDataResult<CourseGetDto>> GetAsync(int id);
    }
}
