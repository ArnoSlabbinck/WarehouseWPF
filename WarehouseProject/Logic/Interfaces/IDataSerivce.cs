﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Logic.Interfaces
{
    public interface IDataSerivce
    {

        Task<bool> Add(Object obj);

        Task<bool> Update(Object obj);

        Task<bool> Delete(Object obj);
    }
}
