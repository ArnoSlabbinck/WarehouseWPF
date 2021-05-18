﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Data
{

    class Admin : IIdentity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
