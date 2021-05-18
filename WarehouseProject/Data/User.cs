﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseProject.Data
{
    public class User : IIdentity
    {

        // Testing to Login
        // Seed the database with a user
        // ViewModel login get the 
        // Get the name after he logs in 
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }



        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }


        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string role;
        public string Role 
        {
            get { return role; } 
            set { role = value; } 
        }
        public string AuthenticationType { get { return "Custom authentication"; } }

        public bool IsAuthenticated { get { return !string.IsNullOrEmpty(name); } }

        public string InvalidLogin { get; set; }
        
    }
}
