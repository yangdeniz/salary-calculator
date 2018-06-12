using System;

namespace SalaryCalculator
{
    public class EmployeeEventArgs : EventArgs
    {
        public Employee Employee { get; set; }
        public EmployeeEventArgs(Employee employee)
        {
            Employee = employee;
        }
    }
}
