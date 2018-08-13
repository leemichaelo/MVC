using Microsoft.AspNet.Identity.EntityFramework;
using PayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PayrollSystem.Data
{
    public class Context : IdentityDbContext<UserLogin>
    {
        public DbSet<Person> People { get; set; }
        public DbSet<UserActivity> UserActivites { get; set; }

        public Context()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
            //
        }
    }
}