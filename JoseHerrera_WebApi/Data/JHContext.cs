using JoseHerrera_WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace JoseHerrera_WebApi.Data
{
    public class JHContext : DbContext
    {
        public JHContext(DbContextOptions<JHContext> options)
            : base(options)
        {

        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Add a unique index to the OHIP Number
            modelBuilder.Entity<Employee>()
            .HasIndex(p => p.SIN)
            .IsUnique();


         
            modelBuilder.Entity<Department>()
                .HasMany(p => p.Employees)
                .WithOne(d => d.Department)
                .HasForeignKey(d=>d.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict);

        }




    }
}
