using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SalaryCalculator
{
    public class Model
    {
        public event EventHandler DataUpdated = delegate { };

        public void AddEmployee(Employee employee)
        {
            using (EmployeeContext db = new EmployeeContext())
            {
                db.Employees.Add(employee);
                db.SaveChanges();
            }
            DataUpdated(this, new EmployeeEventArgs(employee));
        }

        public void UpdateEmployee(Employee employee)
        {
            using (EmployeeContext db = new EmployeeContext())
            {
                var e = db.Employees.Find(employee.Id);
                if (e != null)
                {
                    e.Name = employee.Name;
                    e.HireDate = employee.HireDate;
                    e.Group = employee.Group;
                    e.BaseSalary = employee.BaseSalary;
                    e.ChiefId = employee.ChiefId;
                    db.Entry(e).State = EntityState.Modified;
                    db.SaveChanges();
                }
                DataUpdated(this, new EmployeeEventArgs(e));
            }
        }

        public Employee GetEmployee(int id)
        {
            return GetEmployees().Where(e => e.Id == id).First();
        }

        public List<Employee> GetEmployees()
        {
            var employees = new List<Employee>();
            using (EmployeeContext db = new EmployeeContext())
            {
                employees = db.Employees.ToList();
            }

            foreach (var e in employees)
                InitializeEmployee(e, employees);

            return employees;
        }

        public List<Employee> GetChiefs()
        {
            return GetEmployees().Where(e => e.CanBeChief).ToList();
        }

        public List<Employee> GetSubordinatesList(Employee employee)
        {
            return GetEmployees().Where(e => e.ChiefId == employee.Id).ToList();
        }

        List<Employee> GetSubordinatesList(Employee employee, IEnumerable<Employee> employees)
        {
            return employees.Where(e => e.ChiefId == employee.Id).ToList();
        }

        Employee GetChief(Employee employee, IEnumerable<Employee> employees)
        {
            return employee.ChiefId != null ? employees.Where(e => e.Id == employee.ChiefId).First() : null;
        }

        void InitializeEmployee(Employee employee, IEnumerable<Employee> data)
        {
            employee.Chief = GetChief(employee, data);

            if (employee.CanBeChief)
                employee.Subordinates = GetSubordinatesList(employee, data);
        }

        public Dictionary<Employee, double> CalculateSalaries(DateTime date)
        {
            var dict = new Dictionary<Employee, double>();
            foreach (var employee in GetEmployees())
                dict[employee] = employee.CalculateSalary(date);
            return dict;
        }

        public Dictionary<Employee, double> CalculateSalaries(DateTime date, Employee chief)
        {
            var dict = new Dictionary<Employee, double>();
            foreach (var employee in GetSubordinatesList(chief))
                dict[employee] = employee.CalculateSalary(date);
            return dict;
        }
    }
}
