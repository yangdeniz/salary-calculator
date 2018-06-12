using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace SalaryCalculator
{
    public class EmployeesListPage : Page
    {
        public Employee Chief;

        public event EventHandler EmployeeAdding = delegate { };
        public event EventHandler SalaryCalculating = delegate { };
        public event EventHandler EmployeePageOpening = delegate { };

        public EmployeesListPage(Employee chief = null) 
            : base(chief == null ? "Все сотрудники" : "Подчиненные сотрудника " + chief)
        {
            Chief = chief;
        }

        protected override void InitializeMenu()
        {
            menu = new MenuStrip();

            var calculateMenuItem = new ToolStripMenuItem("Рассчитать зарплату");
            calculateMenuItem.Click += new EventHandler((sender, e) => ShowDatePickerForm());
            menu.Items.Add(calculateMenuItem);

            var addMenuItem = new ToolStripMenuItem("Добавить нового сотрудника");
            addMenuItem.Click += new EventHandler((sender, e) => EmployeeAdding(sender, e));
            menu.Items.Add(addMenuItem);
        }

        protected override void InitializeControl()
        {
            control = new DataGridView();
            var table = (DataGridView)control;

            table.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table.Dock = DockStyle.Fill;
            table.BackgroundColor = Color.White;
            table.ReadOnly = true;
            table.AllowUserToAddRows = false;
            table.RowHeadersVisible = false;
            table.AllowUserToResizeRows = false;
            table.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            CreateColumns(table);

            table.CellDoubleClick += new DataGridViewCellEventHandler((sender, e) => 
            {
                var row = e.RowIndex;
                var employeeId = (int)table.Rows[row].Cells["id"].Value;
                EmployeePageOpening(sender, new EmployeeIdEventArgs(employeeId));
            });
        }

        void CreateColumns(DataGridView table)
        {
            table.Columns.Add("id", "Номер");
            table.Columns.Add("name", "Имя");
            table.Columns.Add("hireDate", "Дата приема на работу");
            table.Columns.Add("group", "Группа");
            table.Columns.Add("chief", "Руководитель");
            table.Columns.Add("baseSalary", "Базовая ставка");
            table.Columns.Add("salary", "Зарплата на дату");

            var dateColumnStyle = new DataGridViewCellStyle() { Format = "dd.MM.yyyy" };
            var numericColumnStyle = new DataGridViewCellStyle()
            {
                Format = "N",
                FormatProvider = (new CultureInfo("ru-Ru")).NumberFormat
            };

            table.Columns["hireDate"].DefaultCellStyle = dateColumnStyle;
            table.Columns["baseSalary"].DefaultCellStyle = numericColumnStyle;
            table.Columns["salary"].DefaultCellStyle = numericColumnStyle;
        }

        public void FillTable(IEnumerable<Employee> data)
        {
            var table = (DataGridView)control;
            table.Rows.Clear();
            table.Columns["salary"].Visible = false;
            foreach (var e in data)
                table.Rows.Add(e.Id, e.Name, e.HireDate, e.Group, e.Chief, e.BaseSalary);
        }

        public void FillTableWithSalary(DateTime date, Dictionary<Employee, double> data)
        {
            var table = (DataGridView)control;
            table.Rows.Clear();
            table.Columns["salary"].Visible = true;
            table.Columns["salary"].HeaderText = "Зарплата на " + date.ToString("dd.MM.yyyy");

            foreach (var e in data.Keys)
                table.Rows.Add(e.Id, e.Name, e.HireDate, e.Group, e.Chief, e.BaseSalary, data[e]);

            table.Rows.Add("Итого");
            table.Rows[table.RowCount - 1].Cells["salary"].Value = data.Values.Sum();
        }

        void ShowDatePickerForm()
        {
            var datePickerForm = new DatePickerForm();
            datePickerForm.CalculateButtonClick += new EventHandler((sender, e) => SalaryCalculating(this, e));
            datePickerForm.Show();
        }
    }
}
