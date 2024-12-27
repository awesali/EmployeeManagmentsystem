using EmployeeManagmentsystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace EmployeeManagmentsystem.Data
{
    public class EmployeeDb : DbContext
    {
        public EmployeeDb(DbContextOptions<EmployeeDb> options) :base(options) { }
        public DbSet<Employee> employees { get; set; }
        public DbSet<User> user { get; set; }
    }
}
