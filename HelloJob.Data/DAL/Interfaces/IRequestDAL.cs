﻿using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.DAL.Interfaces
{
    public interface IRequestDAL: IRepositoryBase<Request>
    {
        Task<Request> GetByVacancyIdAndRequestId(int vacancyId,string userid);
    }
}
