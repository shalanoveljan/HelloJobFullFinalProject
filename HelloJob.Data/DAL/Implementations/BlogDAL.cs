﻿using HelloJob.Core.DataAccess.EntityFramework;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Data.DAL.Implementations
{
    public class BlogDAL: EFRepositoryBase<Blog, HelloJobDbContext>, IBlogDAL
    {
    }
}
