using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Diagnostics;
namespace Yasfib
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            if (Yasfib.MainForm.isChinese == true)
            {
                cuv.Text="已安装版本：";
                lav.Text = "最新版本：";
                button1.Text = "升级！";
            }
            vers.Text = Yasfib.MainForm.versionNumber;
        }
        public string uri;
        private void button1_Click(object sender, EventArgs e)
        {
            WebClient Client = new WebClient();
            if (Yasfib.MainForm.isChinese == true)
            {
                uri = "http://www.pisoft.tk/daedl.php?version=chinese";
            }
            else
            {
                uri = "http://www.pisoft.tk/daedl.php?version=english";
            }
            Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            Client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            Client.DownloadFileAsync(new Uri(uri), @"a.exe");
            button1.Enabled = false;
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            //MessageBox.Show(Math.Truncate(percentage).ToString());
            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Update Completed");
            proc("a.exe");
            this.Close();
        }
        void proc(string filename)
        {
            Process dahProcess = new Process();
            dahProcess.StartInfo.FileName = filename;
            dahProcess.Start();
        }
    }
}
