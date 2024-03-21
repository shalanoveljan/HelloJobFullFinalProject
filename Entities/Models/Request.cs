using Entities.Common;
using HelloJob.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Entities.Models
{
    public class Request:BaseEntity
    {
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
        public Resume Resume { get; set; }
        public int ResumeId { get; set; }
    }
}
