using Authentication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Authentication.Infraestructure
{
    public class AuthContext : DbContext
    {
        public AuthContext() : base("AuthContext")
        {
            Database.SetInitializer<AuthContext>(new MyDbInitializer());
        }

        public DbSet<User> Users { get; set; }
    }
}