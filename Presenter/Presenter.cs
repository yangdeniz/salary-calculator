using System;
using System.Collections.Generic;

namespace SalaryCalculator
{
    public class Presenter
    {
        IView view;
        Model model;

        public Presenter(IView view)
        {
            this.view = view;
            model = new Model();

            model.DataUpdated += new EventHandler((sender, e) => view.UpdateData());
        }

        public void LoadEmployee(int id)
        {
            var data = model.GetEmployee(id);
            view.OpenEmployeePage(data);
        }

        public void LoadEmployeesList()
        {
            var data = model.GetEmployees();
            view.OpenEmployeesListPage(data);
        }

        public void LoadEmployeesList(Employee employee)
        {
            var data = model.GetSubordinatesList(employee);
            view.OpenEmployeesListPage(data, employee);
        }

        public List<Employee> LoadChiefsList()
        {
            return model.GetChiefs();
        }

        public List<Employee> LoadSubordinatesList(Employee chief = null)
        {
            if (chief == null)
                return model.GetEmployees();

            return model.GetSubordinatesList(chief);
        }

        public Dictionary<Employee, double> LoadSalaries(DateTime date, Employee chief)
        {
            if (chief == null)
                return model.CalculateSalaries(date);
            else
                return model.CalculateSalaries(date, chief);
        }

        public void SaveData(int? id, string name, DateTime hireDate, object group, decimal baseSalary, bool hasChief, object chief)
        {
            Validate(name, hireDate, group, baseSalary, hasChief, chief);

            var employee = new Employee(
                name,
                hireDate,
                (EmployeeGroup)group,
                (double)baseSalary,
                hasChief ? (int?)((Employee)chief).Id : null
                );

            if (id == null)
                model.AddEmployee(employee);
            else
            {
                employee.Id = (int)id;
                model.UpdateEmployee(employee);
            }
        }

        void Validate(string name, DateTime hireDate, object group, decimal baseSalary, bool hasChief, object chief)
        {
            if (name == "") throw new ArgumentException("Не заполнено имя сотрудника");
            else if (group == null) throw new ArgumentException("Не указана группа сотрудника");
            else if (baseSalary == 0) throw new ArgumentException("Не указана базовая ставка");
            else if (hasChief && chief == null) throw new ArgumentException("Не указан руководитель");
        }
    }
}
