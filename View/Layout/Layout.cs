using System.Windows.Forms;

namespace SalaryCalculator
{
    public class Layout : TableLayoutPanel
    {
        public Layout(MenuStrip menu, Control control)
        {
            Dock = DockStyle.Fill;
            RowCount = 2;
            ColumnCount = 1;
            RowStyles.Add(new RowStyle(SizeType.AutoSize));

            Controls.Add(menu, 0, 0);
            Controls.Add(control, 0, 1);
        }
    }
}
