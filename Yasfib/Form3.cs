using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Yasfib
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (Yasfib.MainForm.isChinese)
            {
                this.Text = "历史纪录";
                button1.Text = "清空";
            }
            try
            {
                XmlTextReader textReader = new XmlTextReader("ac.xml");
                textReader.Read();
                while (textReader.Read())
                {
                    textReader.MoveToElement();
                    if (textReader.Name == "url")
                    {
                        listBox1.Items.Add(textReader.ReadString());
                    }
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            System.IO.TextWriter rpHistory = new System.IO.StreamWriter("ac.xml");
            rpHistory.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?><i></i>");
            rpHistory.Close();
            MessageBox.Show("Daedalus will clean the history when it next starts up. \r 浏览器会在下次启动时彻底清空历史纪录。");
        }
    }
}
