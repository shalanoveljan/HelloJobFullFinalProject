using AutoMapper;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Mappers
{
    public class GlobalMapping:Profile
    {
        public GlobalMapping() 
        {
            CreateMap<Blog, BlogPostDto>().ReverseMap();
            CreateMap<Blog, BlogGetDto>().ReverseMap();
            CreateMap<Blog, BlogUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryGetDto>().ReverseMap();
            CreateMap<Category, CategoryPostDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

        }

    }
}
