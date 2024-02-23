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
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class BlogService : IBlogService
    {
        readonly IWebHostEnvironment _env;
        readonly IBlogDAL _blogRepository;
         readonly IMapper _mapper;


        public BlogService(IWebHostEnvironment env, IBlogDAL blogRepository, IMapper mapper, HelloJobDbContext context)
        {
            _env = env;
            _blogRepository = blogRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(BlogPostDto dto)
        {
            Blog blog = _mapper.Map<Blog>(dto);
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

            blog.Storage = "wwwroot";

            blog.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/blogs");

            await _blogRepository.AddAsync(blog);
            return new SuccessResult("Create Blog successfully");
        
    }

        public async Task<PagginatedResponse<BlogGetDto>> GetAllAsync(int pageNumber = 1, int pageSize=8)
        {
            var query = _blogRepository.GetQuery(x => !x.IsDeleted)
             .AsNoTrackingWithIdentityResolution()
             .Include(x => x.Category);

            var paginatedBlogs = await query.ToPagedListAsync(pageNumber, pageSize);

            var BlogGetDtos = paginatedBlogs.Datas.Select(x =>
                new BlogGetDto
                {
                    Title = x.Title,
                    Id = x.Id,
                    Description = x.Description,
                    smallDescription = x.smallDescription,
                    Image = x.Image,
                    CreatedAt = x.CreatedAt,
                    ViewCount = x.ViewCount,
                    Category = new CategoryGetDto { Name = x.Category.Name,Image=x.Category.Image },
                }).ToList();
            var pagginatedResponse = new PagginatedResponse<BlogGetDto>(BlogGetDtos, pageNumber, pageSize, paginatedBlogs.Datas.Count());


            return pagginatedResponse;
        }

        public async Task<IDataResult<BlogGetDto>> GetAsync(int id)
        {
            var query = _blogRepository.GetQuery(x => !x.IsDeleted)
                         .AsNoTrackingWithIdentityResolution()
                         .Include(x => x.Category);
            var blogs = await query.Select(x =>
              new BlogGetDto
              {
                  Title = x.Title,
                  Id = x.Id,
                  Description = x.Description,
                  smallDescription = x.smallDescription,
                  Image = x.Image,
                  CreatedAt = x.CreatedAt,
                  ViewCount = x.ViewCount,
                  Category = new CategoryGetDto { Name = x.Category.Name },
              }).ToListAsync();
            BlogGetDto? blog= blogs.FirstOrDefault(x=>x.Id== id);
           
            if (blog == null)
            {
                return new ErrorDataResult<BlogGetDto>("Blog Not Found");
            }

             return new SuccessDataResult<BlogGetDto>("Get Blog"); 
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Blog? blog = await _blogRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Category");

            if (blog == null)
            {
                return new ErrorResult("Blog Not Found");
            }
            blog.IsDeleted = true;
            await _blogRepository.UpdateAsync(blog);
            return new SuccessResult("Blog removed");
        }

        public async Task<IResult> UpdateAsync(int id, BlogPostDto dto)
        {
            Blog blog = _mapper.Map<Blog>(dto);
            blog.CategoryId = dto.CategoryId;

            if (blog == null)
            {
                return new ErrorResult("The Blog not found");

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

                blog.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/blogs");
            }
            else
            {
                return new ErrorResult("The field image is required");
            }

            await _blogRepository.UpdateAsync(blog);
            return new SuccessResult("Update Blog successfully");

        }
    }
}
