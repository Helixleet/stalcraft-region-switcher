using System;
using System.Windows.Forms;

namespace STALZONERegionSwitcher
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // AppUserModelID for taskbar grouping (Windows)
            try
            {
                NativeMethods.SetCurrentProcessExplicitAppUserModelID("stalzone.region.switcher.1.0");
            }
            catch { }

            Application.Run(new MainForm());
        }
    }

    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("shell32.dll", SetLastError = true)]
        public static extern void SetCurrentProcessExplicitAppUserModelID(
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string AppID);
    }
}
