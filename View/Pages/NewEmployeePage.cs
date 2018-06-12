using System.Collections.Generic;
using System.Windows.Forms;

namespace SalaryCalculator
{
    public class NewEmployeePage : EmployeePage
    {
        public NewEmployeePage(IEnumerable<Employee> data) : base("Добавление нового сотрудника", data)
        {
        }

        protected override void InitializeMenu()
        {
            menu = new MenuStrip();
            CreateSaveMenuItem(true);
        }
    }
}
