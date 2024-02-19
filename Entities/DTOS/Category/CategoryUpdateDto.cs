using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.DTOS
{
    public record CategoryUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsParent { get; set; }
        public string Image { get; set; }
        public int? ParentId { get; set; }
    }
}
