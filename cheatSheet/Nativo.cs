using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cheatSheet
{
    public sealed class HotkeyManager : NativeWindow, IDisposable
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        private Form1 form1;
        public HotkeyManager(Form1 form1)
        {
            CreateHandle(new CreateParams());
            this.form1 = form1;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                //MessageBox.Show(GetActiveWindowTitle());                
                if (!this.form1.WindowState.Equals(FormWindowState.Normal)) {
                    this.form1.populateGrid(GetActiveProcessInfo());
                    this.form1.WindowState = FormWindowState.Normal;
                    //this.form1.BringToFront("Form1");
                }
            }
        }

        private String GetActiveProcessInfo()
        {
            IntPtr hwnd = GetForegroundWindow();

            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            Process p = Process.GetProcessById((int)pid);
            return p.ProcessName;
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        public void Dispose()
        {
            DestroyHandle();
        }


    }
}
