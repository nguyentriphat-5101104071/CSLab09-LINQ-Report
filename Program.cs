using System;
using System.Windows.Forms;

namespace CSLab09_LINQ_Report
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmBanHang());
        }
    }
}
