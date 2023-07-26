using HrSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HrSystem.Models
{
    public class ITIContext : IdentityDbContext<ApplicationUser>
    {

        
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<LeaveAttend> LeaveAttends { get; set; }
        public virtual DbSet<OffDays> OffDays { get; set; }
        public virtual DbSet<General_Settings> Performances { get; set; }
        


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=HrSystem1;Integrated Security=True;TrustServerCertificate=True");
        }

        public ITIContext() : base()
        {
        }

        public ITIContext(DbContextOptions<ITIContext> options)
            : base(options)
        {
        }


        


    //    // Register the converter on the DbContext
    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        var dateTimeOffsetConverter = new ValueConverter<DateTimeOffset, DateTime>(
    //v => v.UtcDateTime,
    //v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
    //        modelBuilder.Entity<Employee>()
    //        .Property(e => e.BirthDate)
    //        .HasConversion(dateTimeOffsetConverter);
    //        modelBuilder.Entity<Employee>()
    //        .Property(e => e.HireDate)
    //        .HasConversion(dateTimeOffsetConverter);
    //    }


    }
}
