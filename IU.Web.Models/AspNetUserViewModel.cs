﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Web.Models
{
    public class AspNetUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Image { get; set; }
        public string CountryId { get; set; }
        public string AccountType { get; set; }
        public string FullName { get; set; }
        public string Class { get; set; }
        public string Semester { get; set; }
        public string Subject { get; set; }
    }

    public class Register
    {
 
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string CountryId { get; set; }
    }
}
