using Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Image { get; set; }=null!;
        public int? ParentId { get; set; }
        public Category? Parent { get; set; }= null!;
        public bool IsParent { get; set; }

        public IEnumerable<Category> Children { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
    }
}
