using System.Collections.Generic;

namespace SalaryCalculator
{
    public interface IView
    {
        void OpenEmployeePage();
        void OpenEmployeePage(Employee employee);
        void OpenEmployeesListPage(List<Employee> employees, Employee employee = null);
        void UpdateData();
    }
}
