using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PayrollSystem.Data
{
    public class Context : DbContext
    {
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<UserActivity> UserActivites { get; set; }
    }
}