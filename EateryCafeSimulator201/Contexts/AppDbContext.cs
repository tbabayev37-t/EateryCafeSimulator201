using System.Reflection;
using EateryCafeSimulator201.Models;
using Microsoft.EntityFrameworkCore;

namespace EateryCafeSimulator201.Contexts
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Cheff> Cheffs { get; set; }
        public DbSet<Department> Departments { get; set; }
        
    }
}
