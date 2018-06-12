using System;

namespace SalaryCalculator
{
    public class EmployeeDataEventArgs : EventArgs
    {
        public int? Id;
        public string Name;
        public DateTime HireDate;
        public object Group;
        public decimal BaseSalary;
        public bool HasChief;
        public object ChiefId;

        public EmployeeDataEventArgs(string name, DateTime hireDate, object group, decimal baseSalary, bool hasChief, object chiefId, int? id = null)
        {
            Id = id;
            Name = name;
            HireDate = hireDate;
            Group = group;
            BaseSalary = baseSalary;
            HasChief = hasChief;
            ChiefId = chiefId;
        }
    }
}
