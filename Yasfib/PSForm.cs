using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Yasfib
{
    public partial class PSForm : Form
    {
        public PSForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void PSForm_Load(object sender, EventArgs e)
        {
            foreach (string f in Directory.GetFiles(Application.StartupPath + "\\plugins"))
            {
                listBox1.Items.Add(Path.GetFileName(f).Replace(".cpxd", ".cpx (deactivated)"));
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                label5.Text = listBox1.SelectedItem.ToString();
                intelloTech.ProScript.Agent theAgent = new intelloTech.ProScript.Agent(Application.StartupPath + "\\plugins\\" + listBox1.SelectedItem.ToString().Replace(".cpx (deactivated)", ".cpxd"));
                label6.Text = theAgent.AgentName;
                label7.Text = theAgent.AgentAuthor;
                richTextBox1.Text = theAgent.AgentDescription;
                if (listBox1.SelectedItem.ToString().Replace(".cpx (deactivated)", ".cpxd").EndsWith(".cpxd"))
                {
                    glassButton1.Text = "Activate";
                }
                else
                {
                    glassButton1.Text = "Deactivate";
                }
            }
            catch { }
        }

        private void glassButton1_Click(object sender, EventArgs e)
        {
            if (glassButton1.Text == "Activate")
            {
                File.Copy(Application.StartupPath + "\\plugins\\" + listBox1.SelectedItem.ToString().Replace(".cpx (deactivated)", ".cpxd"), Application.StartupPath + "\\plugins\\" + listBox1.SelectedItem.ToString().Replace(".cpx (deactivated)", ".cpx"));
                File.Delete(Application.StartupPath + "\\plugins\\" + listBox1.SelectedItem.ToString().Replace(".cpx (deactivated)", ".cpxd"));
                listBox1.Items.Clear();
                foreach (string f in Directory.GetFiles(Application.StartupPath + "\\plugins"))
                {
                    listBox1.Items.Add(Path.GetFileName(f).Replace(".cpxd", ".cpx (deactivated)"));
                }
            }
            else
            {
                File.Copy(Application.StartupPath + "\\plugins\\" + listBox1.SelectedItem.ToString(), Application.StartupPath + "\\plugins\\" + listBox1.SelectedItem.ToString().Replace(".cpx", ".cpxd"));
                File.Delete(Application.StartupPath + "\\plugins\\" + listBox1.SelectedItem.ToString());
                listBox1.Items.Clear();
                foreach (string f in Directory.GetFiles(Application.StartupPath + "\\plugins"))
                {
                    listBox1.Items.Add(Path.GetFileName(f).Replace(".cpxd", ".cpx (deactivated)"));
                }
            }
        }
    }
}
