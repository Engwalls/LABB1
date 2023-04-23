using LABB1.Models;
using Microsoft.EntityFrameworkCore;

namespace LABB1.Data
{
    public class Labb1Context : DbContext
    {
        public Labb1Context(DbContextOptions<Labb1Context> options)
                : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }  
        public DbSet<Leave> Leaves { get; set; }   
    }
}
