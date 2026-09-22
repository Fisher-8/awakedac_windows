using System;
using System.Windows.Forms;

namespace awakedac_windows
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenu;
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Visible = false;
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Close", null, (s, a) => this.Close());
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = Properties.Resources.awakedac;
            notifyIcon.ContextMenuStrip = contextMenu;
            notifyIcon.Visible = true;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            base.OnFormClosing(e);
        }
    }
}
