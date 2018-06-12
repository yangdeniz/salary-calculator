using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SalaryCalculator
{
    public abstract class EmployeePage : Page
    {
        protected TextBox name;
        protected DateTimePicker hireDate;
        protected ComboBox group;
        protected NumericUpDown baseSalary;
        protected CheckBox hasChief;
        protected ComboBox chief;

        public event EventHandler EmployeeDataSaving = delegate { };

        public EmployeePage(string pageName, IEnumerable<Employee> data) : base(pageName)
        {
            ChiefComboBoxDataBind(data);
        }

        protected void CreateSaveMenuItem(bool isVisible)
        {
            var saveMenuItem = new ToolStripMenuItem("Сохранить");
            saveMenuItem.Name = "save";
            saveMenuItem.Click += new EventHandler(
                (sender, e) => EmployeeDataSaving(this, new EmployeeDataEventArgs(
                    name.Text,
                    hireDate.Value,
                    group.SelectedItem,
                    baseSalary.Value,
                    hasChief.Checked,
                    chief.SelectedItem,
                    this is ExistedEmployeePage ? (int?)((ExistedEmployeePage)this).Employee.Id : null
                    ))
                );
            saveMenuItem.Visible = isVisible;
            menu.Items.Add(saveMenuItem);
        }

        protected override void InitializeControl()
        {
            control = new TableLayoutPanel();
            var panel = (TableLayoutPanel)control;
            panel.AutoSize = true;
            panel.RowCount = 6;
            panel.ColumnCount = 2;
            panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40.00F));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60.00F));

            InitializeControls();
            BindControls();

            var controlLabelsText = CreateLabelsText();
            var row = 0;
            foreach (var control in controlLabelsText)
            {
                var label = CreateLabel(control.Key, control.Value);
                AddControl(control.Key, label, row++);
            }
        }

        void InitializeControls()
        {
            name = new TextBox() { Name = "name", Dock = DockStyle.Fill };
            hireDate = new DateTimePicker() { Name = "hireDate", Dock = DockStyle.Fill };
            group = new ComboBox()
            {
                Name = "group",
                Dock = DockStyle.Fill
            };
            baseSalary = new NumericUpDown()
            {
                Name = "baseSalary",
                Minimum = 0,
                Maximum = decimal.MaxValue,
                Increment = 500,
                DecimalPlaces = 2,
                ThousandsSeparator = true,
                Dock = DockStyle.Fill
            };
            hasChief = new CheckBox() { Name = "hasChief" };
            chief = new ComboBox()
            {
                Name = "Chief",
                Dock = DockStyle.Fill,
                Visible = false
            };
        }

        void BindControls()
        {
            GroupComboBoxDataBind();

            hasChief.CheckedChanged += new EventHandler((sender, e) =>
            {
                var label = control.Controls["chiefLabel"];
                label.Visible = !label.Visible;
                chief.Visible = !chief.Visible;
            });
        }

        void GroupComboBoxDataBind()
        {
            foreach (var e in Enum.GetValues(typeof(EmployeeGroup)))
                group.Items.Add(e);
        }

        void ChiefComboBoxDataBind(IEnumerable<Employee> data)
        {
            foreach (var e in data)
                chief.Items.Add(e);
        }

        Label CreateLabel(Control control, string text)
        {
            return new Label()
            {
                Name = control.Name + "Label",
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Visible = control.Visible
            };
        }

        Dictionary<Control, string> CreateLabelsText()
        {
            var dict = new Dictionary<Control, string>();
            dict[name] = "Имя";
            dict[hireDate] = "Дата приема на работу";
            dict[group] = "Группа сотрудников";
            dict[baseSalary] = "Базовая ставка";
            dict[hasChief] = "Указать руководителя";
            dict[chief] = "Руководитель";
            return dict;
        }

        void AddControl(Control control, Label label, int row)
        {
            var panel = (TableLayoutPanel)this.control;
            panel.Controls.Add(label, 0, row);
            panel.Controls.Add(control, 1, row);
        }

        public void ShowDataSavedMessage()
        {
            MessageBox.Show("Данные о сотруднике успешно сохранены", "Данные сохранены");
            Close();
        }

        public void ShowErrorMessage(string text)
        {
            MessageBox.Show(text, "Ошибка");
        }
    }
}
