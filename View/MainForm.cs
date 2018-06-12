using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SalaryCalculator
{
    public partial class MainForm : Form, IView
    {
        Presenter presenter;

        MenuStrip menu;
        TabControl tabControl;
        Layout layout;

        public MainForm()
        {
            presenter = new Presenter(this);

            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(800, 500);

            InitializeMenu();
            InitializeTabControl();

            layout = new Layout(menu, tabControl);
            Controls.Add(layout);

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        void InitializeMenu()
        {
            menu = new MenuStrip();

            var employeeMenu = new ToolStripMenuItem("Сотрудники");

            var showAllMenuItem = new ToolStripMenuItem("Показать всех");
            showAllMenuItem.Click += new EventHandler((sender, e) => presenter.LoadEmployeesList());

            var addMenuItem = new ToolStripMenuItem("Добавить");
            addMenuItem.Click += new EventHandler((sender, e) => OpenEmployeePage());

            employeeMenu.DropDownItems.Add(showAllMenuItem);
            employeeMenu.DropDownItems.Add(addMenuItem);

            menu.Items.Add(employeeMenu);
        }

        void InitializeTabControl()
        {
            tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            tabControl.Alignment = TabAlignment.Bottom;
            tabControl.Visible = false;
        }

        void OpenPage(Page page)
        {
            tabControl.Visible = true;
            tabControl.TabPages.Add(page);
            tabControl.SelectedTab = page;
            page.PageClosed += new EventHandler((sender, e) =>
            {
                tabControl.TabPages.Remove((TabPage)sender);
                if (tabControl.TabCount == 0)
                    tabControl.Visible = false;
            });
        }

        public void OpenEmployeePage(EmployeePage page)
        {
            OpenPage(page);
            page.EmployeeDataSaving += new EventHandler(
                (sender, e) => TrySaveData((EmployeePage)sender, (EmployeeDataEventArgs)e));
        }

        public void OpenEmployeePage()
        {
            var data = presenter.LoadChiefsList();
            var page = new NewEmployeePage(data);
            OpenEmployeePage(page);
        }

        public void OpenEmployeePage(Employee employee)
        {
            var data = presenter.LoadChiefsList();
            var page = new ExistedEmployeePage(employee, data);
            OpenEmployeePage(page);

            page.SubordinatesListShowing += new EventHandler((sender, e) => 
            {
                var emp = (e as EmployeeEventArgs).Employee;
                presenter.LoadEmployeesList(emp);
            });
        }

        public void OpenEmployeesListPage(List<Employee> employees, Employee chief = null)
        {
            if (employees.Count == 0)
            {
                ShowEmptyDatabaseWindow();
                return;
            }

            var page = new EmployeesListPage(chief);
            OpenPage(page);
            page.FillTable(employees);

            page.EmployeeAdding += new EventHandler((sender, e) => OpenEmployeePage());

            page.EmployeePageOpening += new EventHandler((sender, e) => 
            {
                var id = (e as EmployeeIdEventArgs).Id;
                presenter.LoadEmployee(id);
            });

            page.SalaryCalculating += new EventHandler((sender, e) => 
            {
                var date = (e as DateEventArgs).Date;
                var employee = (sender as EmployeesListPage).Chief;
                var salaries = presenter.LoadSalaries(date, employee);
                page.FillTableWithSalary(date, salaries);
            });
        }

        public void UpdateData()
        {
            var pages = tabControl.TabPages;
            foreach (var page in pages)
                if (page is EmployeesListPage)
                {
                    var data = presenter.LoadSubordinatesList((page as EmployeesListPage).Chief);
                    (page as EmployeesListPage).FillTable(data);
                }
        }

        void TrySaveData(EmployeePage sender, EmployeeDataEventArgs e)
        {
            try
            {
                presenter.SaveData(e.Id, e.Name, e.HireDate, e.Group, e.BaseSalary, e.HasChief, e.ChiefId);
                sender.ShowDataSavedMessage();
            }
            catch (Exception ex)
            {
                sender.ShowErrorMessage(ex.Message);
            }
        }

        void ShowEmptyDatabaseWindow()
        {
            var answer = MessageBox.Show(
                "База данных сотрудников пуста. Добавить нового сотрудника?",
                "Нет данных о сотрудниках",
                MessageBoxButtons.YesNo
                );

            if (answer == DialogResult.Yes)
                OpenEmployeePage();
        }
    }
}
