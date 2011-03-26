using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace Yasfib
{
    public partial class Form2 : Form
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
        public Form2()
        {
            InitializeComponent();
        }
        void wf(string filename, string content)
        {
            StreamWriter tr;
            tr = File.CreateText(filename);
            tr.Write(content);
            tr.Close();
        }
        public string rf(string filename)
        {
            return File.ReadAllText(filename);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {

                if (clsProcess.ProcessName.Contains("dwm"))
                {
                    MARGINS margins = new MARGINS();
                    margins.cxLeftWidth = 10;
                    margins.cxRightWidth = 10;
                    margins.cyTopHeight = 90;
                    margins.cyButtomheight = 200;
                    //set all the four value -1 to apply glass effect to the whole window
                    //set your own value to make specific part of the window glassy.
                    IntPtr hwnd = this.Handle;
                    int result = DwmExtendFrameIntoClientArea(hwnd, ref margins);
                    this.BackColor = System.Drawing.Color.BlanchedAlmond;
                    this.BackgroundImage = null;
                    //label1.ForeColor = Color.White;
                }
            }
            general.Text = rf("config/general.conf");
            if (general.Text == "Show my home page")
            {
                homepage.ReadOnly = false;
            }
            else
            {
                homepage.ReadOnly = true;
            }
            homepage.Text = rf("home");
            if (rf("config/ad.conf") == "1")
            {
                
                checkBox1.Checked = true;
                checkBox1.Enabled = false;
                HttpProxHost.ReadOnly = true;
                HttpProxPort.ReadOnly = true;
                HttpsProxHost.ReadOnly = true;
                HttpsProxPort.ReadOnly = true;
                HttpProxHost.Text = "127.0.0.1";
                HttpProxPort.Text = "8118";
                HttpProxHost.Text = rf("config/uproxyip.conf");
                HttpProxPort.Text = rf("config/uproxyport.conf");
                HttpsProxHost.Text = rf("config/sproxyip.conf");
                HttpsProxPort.Text = rf("config/sproxyport.conf");
                abOn.Checked = true;
            }
            else
            {
                abOn.Checked = false; abOff.Checked = true;
            }
            if (rf("config/proxybool.conf") == "1")
            {
                checkBox1.Checked = true;
                HttpProxHost.Text = rf("config/uproxyip.conf");
                HttpProxPort.Text = rf("config/uproxyport.conf");
                HttpsProxHost.Text = rf("config/sproxyip.conf");
                HttpsProxPort.Text = rf("config/sproxyport.conf");
            }
            if (checkBox1.Checked == true && rf("config/ad.conf") != "1")
            {
                HttpProxHost.ReadOnly = false;
                HttpProxPort.ReadOnly = false;
                HttpsProxHost.ReadOnly = false;
                HttpsProxPort.ReadOnly = false;
            }
            else
            {
                HttpProxHost.ReadOnly = true;
                HttpProxPort.ReadOnly = true;
                HttpsProxHost.ReadOnly = true;
                HttpsProxPort.ReadOnly = true;
            }
            if (rf("ab") == "a")
            {
                abAggressive.Checked = true;
            }
            else { abConservative.Checked = true; }
            if (Yasfib.Form1.isChinese == true)
            {
                tabPage1.Text = "常规";
                label1.Text = "启动时：";
                label2.Text = "首页：";
                tabPage3.Text = "隐私";
                label3.Text = "屏蔽广告：";
                abOff.Text = "关";
                abOn.Text = "开";
                button3.Text = "删除浏览记录";
                tabPage4.Text="代理";
                checkBox1.Text = "使用代理服务器";
                label7.Text = "端口";
                wsdf.Text = "快速选择：";
                button4.Text = "台湾";
                button5.Text = "美国";
                button6.Text = "加拿大";
                button7.Text = "德国";
                this.Text = "首选项";
                abAggressive.Text = "全部网页";
                abConservative.Text = "智能";
                label8.Text = "反封杀模式";
            }
        }

        private void general_SelectedValueChanged(object sender, EventArgs e)
        {
            if (general.Text == "Show my home page")
            {
                homepage.ReadOnly = false;
            }
            else if (general.Text == "Show about:blank")
            {
                homepage.Text = "about:blank";
                homepage.ReadOnly = true;
            }
            else if (general.Text == "Show about:yasfib")
            {
                homepage.Text = "javascript:document.write('Yasfib');";
                homepage.ReadOnly = true;
            }
            else
            {
                homepage.ReadOnly = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wf("config/general.conf", general.Text);
            wf("home", homepage.Text);
            wf("config/uproxyip.conf", HttpProxHost.Text);
            wf("config/uproxyport.conf", HttpProxPort.Text);
            wf("config/sproxyip.conf", HttpsProxHost.Text);
            wf("config/sproxyport.conf", HttpsProxPort.Text);
            if (checkBox1.Checked == true)
            {
                wf("config/proxybool.conf", "1");
            }
            else
            {
                wf("config/proxybool.conf", "0");
            }
            if (abOn.Checked == true)
            {
                wf("config/ad.conf", "1");
            }
            else
            {
                wf("config/ad.conf", "0");
            }
            if (rf("config/proxybool.conf") == "1")
            {
                Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = Convert.ToInt32(rf("config/proxybool.conf"));
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http"] = rf("config/uproxyip.conf");
                Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl"] = rf("config/sproxyip.conf");
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http_port"] = Convert.ToInt32(rf("config/uproxyport.conf"));
                Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl_port"] = Convert.ToInt32(rf("config/sproxyport.conf"));
            }
            else
            {
                Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 0;
            }
            this.Close();
            if (abAggressive.Checked == true)
            {
                wf("ab", "a");
            }
            else
            {
                wf("ab", "c");
            }
        }

        private void general_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (abOn.Checked == true)
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = false;
                HttpProxHost.ReadOnly = true;
                HttpProxPort.ReadOnly = true;
                HttpsProxHost.ReadOnly = true;
                HttpsProxPort.ReadOnly = true;
                HttpProxHost.Text = "127.0.0.1";
                HttpProxPort.Text = "8118";
                HttpsProxPort.Text = "0";
            }
            else
            {
                checkBox1.Checked = false;
                checkBox1.Enabled = true;
                HttpProxHost.ReadOnly = true;
                HttpProxHost.Text = "";
                HttpProxPort.ReadOnly = true;
                HttpProxPort.Text = "";
                HttpsProxHost.ReadOnly = true;
                HttpsProxPort.ReadOnly = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            TextWriter rpHistory = new StreamWriter("ac.xml");
            rpHistory.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?><i></i>");
            rpHistory.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                HttpProxHost.ReadOnly = false;
                HttpProxPort.ReadOnly = false;
                HttpProxPort.Text = "0";
                HttpsProxHost.ReadOnly = false;
                HttpsProxPort.ReadOnly = false;
                HttpsProxPort.Text = "0";
            }
            else
            {
                HttpProxHost.ReadOnly = true;
                HttpProxHost.Text = "";
                HttpProxPort.ReadOnly = true;
                HttpProxPort.Text = "0";
                HttpsProxHost.ReadOnly = true;
                HttpsProxHost.Text = "";
                HttpsProxPort.ReadOnly = true;
                HttpsProxPort.Text = "0";
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Enabled && checkBox1.Checked)
            {
                HttpProxHost.Text = "140.114.79.233";
                HttpProxPort.Text = "3124";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (checkBox1.Enabled && checkBox1.Checked)
            {
                HttpProxHost.Text = "128.10.19.52";
                HttpProxPort.Text = "3128";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (checkBox1.Enabled && checkBox1.Checked)
            {
                HttpProxHost.Text = "192.197.121.3";
                HttpProxPort.Text = "3128";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (checkBox1.Enabled && checkBox1.Checked)
            {
                HttpProxHost.Text = "141.24.249.130";
                HttpProxPort.Text = "3128";
            }
        }
    }
}
