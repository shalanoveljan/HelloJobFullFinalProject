using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CategoryPostDto
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsParent { get; set; }
        public int? ParentId { get; set; }
    }
}
