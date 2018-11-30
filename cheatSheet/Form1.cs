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
            Console.WriteLine(title);
            if (File.Exists(directoryPath + "\\" + title + ".json"))
            {
                using (StreamReader r = new StreamReader(directoryPath + "\\" + title + ".json"))
            {
                var json = r.ReadToEnd();
                list = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(json);
                int col = 0;
                int row = 0;
                this.tableLayoutPanel1.Controls.Clear();
                foreach (Dictionary<string, string> item in list)
                {
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
                    this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
                    Button newButton = new Button();
                    newButton.FlatAppearance.BorderSize = 0;
                    newButton.FlatStyle = FlatStyle.Flat;
                    newButton.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                    newButton.Size = new Size(131, 25);
                    new ToolTip().SetToolTip(newButton, item["codigo"]);
                    newButton.Text = item["codigo"];
                    newButton.TextAlign = ContentAlignment.MiddleRight;
                    newButton.UseVisualStyleBackColor = true;
                    newButton.Click += new EventHandler(this.button1_Click);
                    newButton.KeyUp += new KeyEventHandler(this.button1_KeyUp);
                    this.tableLayoutPanel1.Controls.Add(newButton, col++, row);
                    Label newLabel = new Label();
                    newLabel.AutoSize = false;
                    newLabel.Size = new Size(268, 27);
                    string descripcion = item["descripcion"];
                    new ToolTip().SetToolTip(newLabel, descripcion);
                    if(descripcion.Length >= 33)
                    {
                        newLabel.Text = descripcion.Substring(0, 33);
                    }
                    else
                    {
                        newLabel.Text = descripcion;
                    }
                    
                    newLabel.TextAlign = ContentAlignment.MiddleLeft;
                    this.tableLayoutPanel1.Controls.Add(newLabel, col++, row);
                    if (col == 5)
                    {
                        col = 0;
                        row++;
                    }
                }
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

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetText((sender as Button).Text);
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}