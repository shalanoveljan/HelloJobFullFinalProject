using AutoMapper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Extensions;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class ExperienceService : IExperienceService
    {
        readonly IExperienceDAL _ExperienceRepository;
        readonly IMapper _mapper;

        public ExperienceService(IExperienceDAL ExperienceRepository, IMapper mapper)
        {
            _ExperienceRepository = ExperienceRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(ExperiencePostDto dto)
        {
            Experience Experience = _mapper.Map<Experience>(dto);
            if (Experience == null)
            {
                return new ErrorResult("Experience is null");
            }

            await _ExperienceRepository.AddAsync(Experience);

            return new SuccessResult("Create Experience successfully");

        }

        public async Task<PagginatedResponse<ExperienceGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _ExperienceRepository.GetQuery(x => !x.IsDeleted);
            var totalCount = await query.CountAsync();

            var paginatedExperiences = await query.ToPagedListAsync(pageNumber, pageSize);
            var ExperienceGetDtos = paginatedExperiences.Datas.Select(x =>
               new ExperienceGetDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   CreateAt = DateTime.Now,
               }).ToList();
            var pagginatedResponse = new PagginatedResponse<ExperienceGetDto>(ExperienceGetDtos, paginatedExperiences.PageNumber,
              paginatedExperiences.PageSize,
              totalCount);
            return pagginatedResponse;
        }

        public async Task<IDataResult<ExperienceGetDto>> GetAsync(int id)
        {
            var Experience = _ExperienceRepository.GetAsync(x => !x.IsDeleted && x.Id==id).Result;
            if (Experience == null)
            {
                return new ErrorDataResult<ExperienceGetDto>("Experience Not Found");
            }
            ExperienceGetDto dto = new ExperienceGetDto
            {
                Id = Experience.Id,
                Name = Experience.Name,
                CreateAt = DateTime.Now,
            };
           

            return new SuccessDataResult<ExperienceGetDto>(dto,"Get Experience");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Experience? Experience = await _ExperienceRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Experience == null)
            {
                return new ErrorResult("Experience Not Found");
            }
            Experience.IsDeleted = true;
            await _ExperienceRepository.UpdateAsync(Experience);
            return new SuccessResult("Experience removed");
        }

        public async Task<IResult> UpdateAsync(int id, ExperiencePostDto dto)
        {
            Experience? Experience = await _ExperienceRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Experience == null)
            {
                return new ErrorResult("Experience is null");
            }
            Experience.Name = dto.Name;
            

            await _ExperienceRepository.UpdateAsync(Experience);

            return new SuccessResult("Update Experience successfully");
        }
        
     
    }
}
