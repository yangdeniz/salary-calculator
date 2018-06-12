using System;

namespace SalaryCalculator
{
    public class EmployeeIdEventArgs : EventArgs
    {
        public int Id { get; set; }
        public EmployeeIdEventArgs(int id)
        {
            Id = id;
        }
    }
}
