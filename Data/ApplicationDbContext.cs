using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using smalandscamping.Models;

namespace smalandscamping.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<smalandscamping.Models.Cottage> Cottage { get; set; }
        public DbSet<smalandscamping.Models.Booking> Booking { get; set; }
    }
}
