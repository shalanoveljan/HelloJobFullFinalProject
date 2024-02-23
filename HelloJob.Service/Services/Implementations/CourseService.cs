using AutoMapper;
using HelloJob.Core.Helper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Extensions;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class CourseService : ICourseService
    {
        readonly IWebHostEnvironment _env;
        readonly ICourseDAL _CourseRepository;
        readonly IMapper _mapper;


        public CourseService(IWebHostEnvironment env, ICourseDAL CourseRepository, IMapper mapper)
        {
            _env = env;
            _CourseRepository = CourseRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(CoursePostDto dto)
        {
            Course Course = _mapper.Map<Course>(dto);

            if (dto.ImageFile == null)
            {
                return new ErrorResult("The field image is required");
            }

            if (!dto.ImageFile.IsImage())
            {
                return new ErrorResult("Image is not valid");

            }

            if (dto.ImageFile.IsSizeOk(2))
            {
                return new ErrorResult("Image size is not valid");

            }


            Course.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/courses");
            Course.TagsCourse = new List<TagCourse>();
            foreach (var item in dto.TagsIds)
            {
                Course.TagsCourse.Add(new TagCourse
                {
                    Course = Course,
                    TagId = item,
                });
            }

            await _CourseRepository.AddAsync(Course);
            return new SuccessResult("Create Course successfully");

        }

        public async Task<PagginatedResponse<CourseGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 8)
        {
            var query = _CourseRepository.GetQuery(x => !x.IsDeleted)
             .AsNoTrackingWithIdentityResolution()
             .Include(x => x.TagsCourse)
             .ThenInclude(x=> x.Tag);

            var paginatedCourses = await query.ToPagedListAsync(pageNumber, pageSize);

            var CourseGetDtos = paginatedCourses.Datas.Select(x =>
                new CourseGetDto
                {
                    Title = x.Title,
                    Id = x.Id,
                    Description = x.Description,
                     Agency = x.Agency,
                    Image = x.Image,
                    Price= x.Price,
                    IsPremium = x.IsPremium,
                    IsRetirement= x.IsRetirement,
                    IsSertificate= x.IsSertificate,
                    Level=x.Level,
                    Mode= x.Mode,
                    Period=x.Period,
                    Tags = x.TagsCourse.Select(y => new TagGetDto { Name = y.Tag.Name, Id = y.TagId }),
                    Category = new CategoryGetDto { Name = x.Category.Name },



                }).ToList();
            var pagginatedResponse = new PagginatedResponse<CourseGetDto>(CourseGetDtos, pageNumber, pageSize, paginatedCourses.Datas.Count());


            return pagginatedResponse;
        }

        public async Task<IDataResult<CourseGetDto>> GetAsync(int id)
        {
            var query = _CourseRepository.GetQuery(x => !x.IsDeleted)
                         .AsNoTrackingWithIdentityResolution()
                         .Include(x => x.TagsCourse)
                         .ThenInclude(x=>x.Tag);
            var Courses = await query.Select(x =>
              new CourseGetDto
              {
                  Title = x.Title,
                  Id = x.Id,
                  Description = x.Description,
                  Agency = x.Agency,
                  Image = x.Image,
                  Price= x.Price,
                  IsPremium = x.IsPremium,
                  IsRetirement = x.IsRetirement,
                  IsSertificate = x.IsSertificate,
                  Level = x.Level,
                  Mode = x.Mode,
                  Period = x.Period,
                  Tags = x.TagsCourse.Select(y => new TagGetDto { Name = y.Tag.Name, Id = y.TagId }),
                  Category = new CategoryGetDto { Name = x.Category.Name, Image = x.Category.Image },
              }).ToListAsync();
            CourseGetDto? Course = Courses.FirstOrDefault(x => x.Id == id);

            if (Course == null)
            {
                return new ErrorDataResult<CourseGetDto>("Course Not Found");
            }

            return new SuccessDataResult<CourseGetDto>("Get Course");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Course? Course = await _CourseRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "TagsCourse.Tag");

            if (Course == null)
            {
                return new ErrorResult("Course Not Found");
            }
            Course.IsDeleted = true;
            await _CourseRepository.UpdateAsync(Course);
            return new SuccessResult("Course removed");
        }

        public async Task<IResult> UpdateAsync(int id, CoursePostDto dto)
        {
            Course Course = _mapper.Map<Course>(dto);
            Course.CategoryId = dto.CategoryId;

            if (Course == null)
            {
                return new ErrorResult("The Course not found");

            }

            if (dto.ImageFile != null)
            {
                if (!dto.ImageFile.IsImage())
                {
                    return new ErrorResult("Image is not valid");

                }

                if (dto.ImageFile.IsSizeOk(2))
                {
                    return new ErrorResult("Image size is not valid");

                }

                Course.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/courses");
            }
         

            Course.TagsCourse.Clear();

            foreach (var item in dto.TagsIds)
            {
                Course.TagsCourse.Add(new TagCourse
                {
                    Course = Course,
                    TagId = item,
                });
            }

            await _CourseRepository.UpdateAsync(Course);
            return new SuccessResult("Update Course successfully");

        }
      
    }
}
