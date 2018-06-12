using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SalaryCalculator
{
    public class ExistedEmployeePage : EmployeePage
    {
        public readonly Employee Employee;

        List<Control> controlsList
        {
            get { return new List<Control>() { name, hireDate, group, baseSalary, hasChief, chief }; }
        }

        List<Label> labelsList
        {
            get
            {
                var labels = new List<Label>();
                foreach (var control in ((TableLayoutPanel)control).Controls)
                    if (control is Label)
                        labels.Add((Label)control);
                return labels;
            }
        }

        public event EventHandler SubordinatesListShowing;

        public ExistedEmployeePage(Employee employee, IEnumerable<Employee> data) : base(employee.Name, data)
        {
            Employee = employee;
            SetDefaultValues();
            SetVisibility();
            DisableControls();
        }

        protected override void InitializeMenu()
        {
            menu = new MenuStrip();
            CreateSaveMenuItem(false);

            var editMenuItem = new ToolStripMenuItem("Редактировать");
            editMenuItem.Name = "edit";
            editMenuItem.Click += new EventHandler((sender, e) =>
            {
                EnableControls();
                menu.Items["save"].Visible = true;
                menu.Items["edit"].Visible = false;
            });
            menu.Items.Add(editMenuItem);

            var showSubordinatesMenuItem = new ToolStripMenuItem("Показать список подчиненных");
            showSubordinatesMenuItem.Name = "showSubordinates";
            showSubordinatesMenuItem.Click += new EventHandler(
                (sender, e) => SubordinatesListShowing(this, new EmployeeEventArgs(Employee)));
            menu.Items.Add(showSubordinatesMenuItem);
        }

        void SetDefaultValues()
        {
            name.Text = Employee.Name;
            hireDate.Value = Employee.HireDate;
            group.SelectedIndex = (int)Employee.Group;
            baseSalary.Value = (decimal)Employee.BaseSalary;
            hasChief.Checked = Employee.ChiefId != null;

            if (hasChief.Checked)
                chief.SelectedIndex = chief.Items.IndexOf(Employee.Chief);
        }

        void SetVisibility()
        {
            menu.Items["showSubordinates"].Visible = Employee.CanBeChief && Employee.Subordinates.Count > 0;

            if (hasChief.Checked)
            {
                foreach (var control in controlsList)
                    control.Visible = true;
                foreach (var label in labelsList)
                    label.Visible = true;
            }
        }

        void DisableControls()
        {
            foreach (var control in controlsList)
                control.Enabled = false;
        }

        void EnableControls()
        {
            foreach (var control in controlsList)
                control.Enabled = true;
        }
    }
}
