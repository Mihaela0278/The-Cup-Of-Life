using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TheCupOfLife.Data.Models;

namespace TheCupOfLife.Data
{
    public class TheCupOfLifeContext : IdentityDbContext<User>
    {
        public DbSet<Post> Posts { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public override DbSet<User> Users { get; set; }

        public TheCupOfLifeContext(DbContextOptions<TheCupOfLifeContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }

        
}