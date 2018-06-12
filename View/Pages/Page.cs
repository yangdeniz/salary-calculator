using System;
using System.Windows.Forms;

namespace SalaryCalculator
{
    public abstract class Page : TabPage
    {
        protected Layout layout;
        protected MenuStrip menu;
        protected Control control;

        public event EventHandler PageClosed = delegate { };

        public Page(string pageName)
        {
            Text = pageName;

            InitializeMenu();
            CreateCloseMenuItem();

            InitializeControl();

            layout = new Layout(menu, control);
            Controls.Add(layout);
        }

        protected abstract void InitializeMenu();
        protected abstract void InitializeControl();

        void CreateCloseMenuItem()
        {
            var closeMenuItem = new ToolStripMenuItem("Закрыть");
            closeMenuItem.Name = "close";
            closeMenuItem.Click += new EventHandler((sender, e) => Close());
            menu.Items.Add(closeMenuItem);
        }

        protected void Close()
        {
            PageClosed(this, new EventArgs());
        }
    }
}
