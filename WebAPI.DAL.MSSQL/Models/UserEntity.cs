﻿using System;

namespace WebAPI.DAL.MSSQL.Models
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DoB { get; set; }
    }
}
