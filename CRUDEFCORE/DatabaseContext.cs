using CRUDEFCORE.Models.Domain;
using Microsoft.EntityFrameworkCore;
namespace CRUDEFCORE;

public class DatabaseContext:DbContext
{
    public DatabaseContext( DbContextOptions options):base(options)
    {
        
    }

    public DbSet<Employee> Employees { get; set; }
}