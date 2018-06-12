using System.Data.Entity;

namespace SalaryCalculator
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext() : base("DefaultConnection")
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
