using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Net;
using System.Media;

namespace Yasfib
{
    public partial class Form3 : Form
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyButtomheight;
        }

        [DllImport("dwmapi.dll")]

        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarinset);
        //[DllImport("winmm.dll")]
        
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains("dwm"))
                {
                    MARGINS margins = new MARGINS();
                    margins.cxLeftWidth = -1;
                    margins.cxRightWidth = -1;
                    margins.cyTopHeight = -1;
                    margins.cyButtomheight = -1;
                    //set all the four value -1 to apply glass effect to the whole window
                    //set your own value to make specific part of the window glassy.
                    IntPtr hwnd = this.Handle;
                    int result = DwmExtendFrameIntoClientArea(hwnd, ref margins);
                    this.BackColor = System.Drawing.Color.BlanchedAlmond;
                    this.BackgroundImage = null;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.ShowPrintDialog();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
