using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class VacancyService : IVacancyService
    {
        public Task<IResult> CreateAsync(ResumePostDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<PagginatedResponse<ResumeGetDto>> GetAllAsync(string userId, bool isAdmin, int pageNumber = 1, int pageSize = 6)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<List<ResumeGetDto>>> GetAllForResumePageInWebSiteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<ResumeGetDto>> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task IncreaseCount(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> SetOrderStatus(int resumeId, Order orderStatus)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAsync(int id, ResumePostDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
