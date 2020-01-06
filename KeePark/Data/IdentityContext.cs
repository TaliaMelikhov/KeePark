using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeePark.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KeePark.Models
{
    public class IdentityContext : IdentityDbContext<GeneralUser>
    {
        public IdentityContext()
        {
        }

        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        public DbSet<KeePark.Data.GeneralUser> GeneralUser { get; set; }

    }

}