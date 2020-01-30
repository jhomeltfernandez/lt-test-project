using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LTTestProject.DataAccess
{
    public class LTTestProjectIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public LTTestProjectIdentityDbContext(DbContextOptions<LTTestProjectIdentityDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
