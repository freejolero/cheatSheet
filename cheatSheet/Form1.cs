using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cheatSheet
{
    public partial class Form1 : Form
    {
        private string directoryPath = "";
        List<Dictionary<string, string>> list;
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public void BringToFront(string title)
        {            
            IntPtr handle = FindWindow(null, title);

            // Verify that Calculator is a running process.
            if (handle == IntPtr.Zero)
            {
                return;
            }                        
            SetForegroundWindow(handle);
        }

        public void populateGrid(String title)
        {
            using (StreamReader r = new StreamReader(directoryPath + "\\prueba.json"))
            {
                var json = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
                foreach (Dictionary<string, string> item in list)
                {
                    string[] row = new string[] { title, item["descripcion"], item["codigo"] };
                    dataGridView1.Rows.Add(row);
                }
            }
        }

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public Form1()
        {
            InitializeComponent();
            int id = 0;     // The id of the hotkey. 
            var HotKeyManager = new HotkeyManager(this);
            RegisterHotKey(HotKeyManager.Handle, id, (int)KeyModifier.Control, Keys.Space.GetHashCode());
            directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.cheatSheet";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private void ExampleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);       // Unregister hotkey with id 0 before closing the form. You might want to call this more than once with different id values if you are planning to register more than one hotkey.
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}