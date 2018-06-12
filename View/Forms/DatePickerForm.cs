using System;
using System.Windows.Forms;
using System.Drawing;

namespace SalaryCalculator
{
    public partial class DatePickerForm : Form
    {
        public event EventHandler CalculateButtonClick = delegate { };

        public DatePickerForm()
        {
            Width = 200;
            Height = 150;
            StartPosition = FormStartPosition.CenterScreen;

            var panel = new TableLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.RowCount = 3;
            panel.ColumnCount = 1;
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            for (var i = 0; i < panel.RowCount; i++)
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 33.00F));

            var label = new Label();
            label.Text = "Выберите дату расчета:";
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            panel.Controls.Add(label, 0, 0);

            var datePicker = new DateTimePicker();
            datePicker.Dock = DockStyle.Fill;
            panel.Controls.Add(datePicker);

            var btn = new Button();
            btn.Text = "Рассчитать";
            btn.Click += new EventHandler((sender, e) =>
            {
                CalculateButtonClick(sender, new DateEventArgs(datePicker.Value));
                Close();
            });
            
            panel.Controls.Add(btn, 0, 2);

            Controls.Add(panel);

            InitializeComponent();
        }
    }
}
