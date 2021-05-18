﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseModels;
using System.Security.Cryptography;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace WarehouseProject.Data
{
    public class AuthenticationService : IAuthenticationService
    {

        User ErrorUser = new User();


        /// <summary>
        /// Check User for psw and username
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User Login(string username, string password)
        {

            string usernameInDb;
            using (var ctx = new WarehouseDataAccess.WarehouseDBContext())
            {
                ctx.Database.Log = (message) => Debug.WriteLine(message);
                Employee employee = ctx.Employees.Where(p => p.UserName == username).FirstOrDefault();
                usernameInDb = employee.UserName;
                //
                if (usernameInDb != null)
                {
                    //Check the  password
                    string Salt = employee.PassWordSalt;
                    string pswd = CalculateHashPassword(Salt, password);

                    if (pswd == employee.PassWord)
                    {
                        //Create a User from Employee

                        User user = new User();
                        user.Email = employee.Email;
                        user.Name = employee.FullName();
                        user.Password = employee.PassWord;
                        user.Role = employee.JobTitle;
                        user.Username = employee.UserName;
                        return user;
                    }

                    else
                    {
                        // Should raise error  => capture that error and display that message to User
                        
                        ErrorUser.InvalidLogin = "Wrong Password";
                        // Find the Username => then 
                        if (employee.FailedPasswordAttemptCount == 3)
                        {
                            employee.IsLockedOut = true;
                            return ErrorUser;
                        }
                        employee.FailedPasswordAttemptCount++;
                        return ErrorUser;
                    }
                }
                ErrorUser.InvalidLogin = "Username wasn't found";
                
                

            }

            return ErrorUser;

        }

        /// <summary>
        /// Need to  get the password
        /// Need the username
        /// No duplicates => First Check if user not in database
        /// Password hashing sha256 
        /// </summary>
        /// <param name="FirstName"></param>
        /// <param name="LastName"></param>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        public bool Register(string firstName, string lastName, string username, string password, string email, string role)
        {
            //Check first of the user doesn't exist already
            bool userExist = false;
            using (var context = new WarehouseDataAccess.WarehouseDBContext())
            {
                // if empty than false 
                userExist = context.Employees.FirstOrDefault(p => p.FirstName == firstName
                && p.LastName == lastName && p.Email == email) != null ? true : false;

                if (userExist == false)
                {
                    //Generate an 128-bit random number salt to make crypto collitions unlikely
                    //First hash password
                    var supervisor = context.Supervisor.First();
                    string Salt = GenerateSalt();
                    password = CalculateHashPassword(password, Salt);
                    Employee Employee = new Employee
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        PassWord = password,
                        PassWordSalt = Salt,
                        JobTitle = role,
                        FailedPasswordAttemptCount = 0,
                        IsActive = false,
                        UserName = username,
                        Supervisor = supervisor
                    };
                    context.SaveChanges();
                }

                else
                {
                    // Raise an Error when the User 
                    return false; 
                
                }

                
            }

            return false;
             
            //Check firstName and Lastname and Email in datbase zit
            // Als dat zo is dan User already exist

            

        }
        private string GenerateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[36];
            rng.GetBytes(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            return salt;

        }
        private string CalculateHashPassword(string Salt, string textPassword)
        {
            //Conver the salted password to a byte array
            byte[] saltedHashBytes = Encoding.UTF8.GetBytes(textPassword + Salt);
            // Use the hash algorithm to calculate the hash
            HashAlgorithm algorithm = new SHA256Managed();
            string password = Convert.ToBase64String(algorithm.ComputeHash(saltedHashBytes));

            return password;
             
        }
        /// <summary>
        /// We need to reset the user when logged out
        /// Set every item to null
        /// 
        /// </summary>
        public bool LogOut(User user)
        {
            user.Name = string.Empty;
            user.Password = string.Empty;
            user.Username = string.Empty;
            user.Role = string.Empty;

            return true;
            

        }

    }
}