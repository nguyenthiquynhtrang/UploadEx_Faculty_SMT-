using NguyenThiQuynhTrangBTH2.Models;
using Microsoft.EntityFrameworkCore;
namespace NguyenThiQuynhTrangBTH2.Data

{
    public class ApplicationDbContext : DbContext
    {
          public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
          {

          }
          public DbSet<NguyenThiQuynhTrangBTH2.Models.Student> Students {get; set;}
          public DbSet<NguyenThiQuynhTrangBTH2.Models.Employee> Employees {get; set;}
          public DbSet<NguyenThiQuynhTrangBTH2.Models.Person> Persons {get; set;}
          public DbSet<NguyenThiQuynhTrangBTH2.Models.Customer> Customers {get; set;}
          public DbSet<NguyenThiQuynhTrangBTH2.Models.Faculty> Faculty {get; set;} = default!;
          public DbSet<NguyenThiQuynhTrangBTH2.Models.Position> Position {get; set;} = default!;
    }
}