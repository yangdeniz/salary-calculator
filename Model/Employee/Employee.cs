using System;
using System.Collections.Generic;

namespace SalaryCalculator
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime HireDate { get; set; }

        EmployeeGroup group;
        public EmployeeGroup Group
        {
            get { return group; }
            set
            {
                group = value;
                calculator = Mapping.Calculators[group];
            }
        }

        double baseSalary;
        public double BaseSalary
        {
            get { return baseSalary; }
            set
            {
                if (value <= 0) throw new ArgumentException("Базовая ставка зарплаты должна быть неотрицательной");
                baseSalary = value;
            }
        }

        public int? ChiefId { get; set; }

        public Employee Chief { get; set; }

        public bool CanBeChief { get { return Group == EmployeeGroup.Manager || Group == EmployeeGroup.Salesman; } }

        public List<Employee> Subordinates { get; set; }

        Calculator calculator;

        public Employee()
        {
        }

        public Employee(string name, DateTime hireDate, EmployeeGroup group, double baseSalary, int? chiefId = null)
        {
            Name = name;
            HireDate = hireDate;
            Group = group;
            BaseSalary = baseSalary;
            ChiefId = chiefId;
        }

        public int GetExperience(DateTime date)
        {
            return date < HireDate ? 0 : date.Subtract(HireDate).Days / 365;
        }

        public double CalculateSalary(DateTime date)
        {
            return calculator.Calculate(this, date);
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return (obj is Employee) && ((Employee)obj).Id == Id;
        }
    }
}
