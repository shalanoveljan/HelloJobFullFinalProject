using AutoMapper;
using HelloJob.Core.Helper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
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
        private readonly ITagDAL _tag;

        public CourseService(IWebHostEnvironment env, ICourseDAL CourseRepository, IMapper mapper, ITagDAL tag)
        {
            _env = env;
            _CourseRepository = CourseRepository;
            _mapper = mapper;
            _tag = tag;
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

        public async Task<PagginatedResponse<CourseGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _CourseRepository.GetQuery(x => !x.IsDeleted)
             .AsNoTrackingWithIdentityResolution()
             .Include(x=>x.Category)
             .Include(x => x.TagsCourse)
             .ThenInclude(x => x.Tag);

            var totalCount = await query.CountAsync();


            var paginatedCourses = await query.ToPagedListAsync<Course>(pageNumber, pageSize);

            //IQueryable<Course> courses=_CourseRepository.
           var CourseGetDtos = paginatedCourses.Datas.Select( x =>
                  new CourseGetDto
                  {
                      Title = x.Title,
                      Id = x.Id,
                      Description = x.Description,
                      Agency = x.Agency,
                      Image = x.Image,
                      Price = x.Price,
                      IsPremium = x.IsPremium,
                      IsRetirement = x.IsRetirement,
                      IsSertificate = x.IsSertificate,
                      Level = x.Level,
                      Mode = x.Mode,
                      Period = x.Period,
                      Tags =x.TagsCourse.Select(x=>new TagGetDto { Name=x.Tag.Name , Id=x.Tag.Id}) ,
                      CategoryId =x.CategoryId ,
                      Category=new CategoryGetDto { Name=x.Category.Name ,Id=x.Category.Id },
                  }
                  
                  ).ToList();

            var pagginatedResponse = new PagginatedResponse<CourseGetDto>(CourseGetDtos, paginatedCourses.PageNumber,
                paginatedCourses.PageSize,
                totalCount);


            return pagginatedResponse;
        }

        public async Task<IDataResult<CourseGetDto>> GetAsync(int id)
        {
            var query = _CourseRepository.GetQuery(x => !x.IsDeleted)
                         .AsNoTrackingWithIdentityResolution()
                         .Include(x=>x.Category)
                         .Include(x => x.TagsCourse)
                         .ThenInclude(x => x.Tag);
            var Courses = await query.Select(x =>
              new CourseGetDto
              {
                  Title = x.Title,
                  Id = x.Id,
                  Description = x.Description,
                  Agency = x.Agency,
                  Image = x.Image,
                  Price = x.Price,
                  maxAge=x.maxAge,
                  minAge=x.minAge,
                  IsPremium = x.IsPremium,
                  IsRetirement = x.IsRetirement,
                  IsSertificate = x.IsSertificate,
                  Level = x.Level,
                  Mode = x.Mode,
                  CategoryId = x.CategoryId,
                  Period = x.Period,
                  Tags = x.TagsCourse.Select(y => new TagGetDto { Name = y.Tag.Name, Id = y.TagId }),
                  Category = new CategoryGetDto { Name = x.Category.Name, Image = x.Category.Image,Id=x.Category.Id },
              }).ToListAsync();


            CourseGetDto? Course = Courses.FirstOrDefault(x => x.Id == id);

            if (Course == null)
            {
                return new ErrorDataResult<CourseGetDto>("Course Not Found");
            }

            return new SuccessDataResult<CourseGetDto>(Course,"Get Course");
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
            Course? Course = await _CourseRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "TagsCourse.Tag");
            if (Course == null)
            {
                return new ErrorResult("The Course not found");

            }
            Course.CategoryId = dto.CategoryId;
            Course.Agency=dto.Agency;
            Course.maxAge = dto.maxAge;
            Course.minAge= dto.minAge;
            Course.Description = dto.Description;
            Course.Mode= dto.Mode;
            Course.Level= dto.Level;
            Course.IsPremium= dto.IsPremium;
            Course.IsRetirement= dto.IsRetirement;
            Course.IsSertificate= dto.IsSertificate;
            Course.Price= dto.Price;
            Course.Period= dto.Period;
            Course.Title= dto.Title;
            


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
