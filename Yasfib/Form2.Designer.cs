namespace Yasfib
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.homepage = new System.Windows.Forms.TextBox();
            this.general = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.abConservative = new System.Windows.Forms.RadioButton();
            this.abAggressive = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.abOff = new System.Windows.Forms.RadioButton();
            this.abOn = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.wsdf = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.HttpsProxPort = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.HttpsProxHost = new System.Windows.Forms.TextBox();
            this.HttpProxPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.HttpProxHost = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(498, 234);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.homepage);
            this.tabPage1.Controls.Add(this.general);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(490, 207);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Home page:";
            // 
            // homepage
            // 
            this.homepage.Location = new System.Drawing.Point(131, 105);
            this.homepage.Name = "homepage";
            this.homepage.Size = new System.Drawing.Size(266, 22);
            this.homepage.TabIndex = 2;
            // 
            // general
            // 
            this.general.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.general.Items.AddRange(new object[] {
            "Show my home page",
            "Show about:blank",
            "Show about:yasfib"});
            this.general.Location = new System.Drawing.Point(264, 68);
            this.general.Name = "general";
            this.general.Size = new System.Drawing.Size(142, 22);
            this.general.TabIndex = 1;
            this.general.SelectedIndexChanged += new System.EventHandler(this.general_SelectedIndexChanged);
            this.general.SelectedValueChanged += new System.EventHandler(this.general_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "On startup:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel1);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.button3);
            this.tabPage3.Controls.Add(this.abOff);
            this.tabPage3.Controls.Add(this.abOn);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(490, 207);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Privacy";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.abConservative);
            this.panel1.Controls.Add(this.abAggressive);
            this.panel1.Location = new System.Drawing.Point(216, 83);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(253, 24);
            this.panel1.TabIndex = 11;
            // 
            // abConservative
            // 
            this.abConservative.AutoSize = true;
            this.abConservative.Location = new System.Drawing.Point(14, 3);
            this.abConservative.Name = "abConservative";
            this.abConservative.Size = new System.Drawing.Size(94, 18);
            this.abConservative.TabIndex = 9;
            this.abConservative.TabStop = true;
            this.abConservative.Text = "Conservative";
            this.abConservative.UseVisualStyleBackColor = true;
            // 
            // abAggressive
            // 
            this.abAggressive.AutoSize = true;
            this.abAggressive.Location = new System.Drawing.Point(114, 3);
            this.abAggressive.Name = "abAggressive";
            this.abAggressive.Size = new System.Drawing.Size(83, 18);
            this.abAggressive.TabIndex = 10;
            this.abAggressive.TabStop = true;
            this.abAggressive.Text = "Aggressive";
            this.abAggressive.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(77, 83);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(116, 14);
            this.label8.TabIndex = 8;
            this.label8.Text = "Anti-blocking mode:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(150, 134);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(158, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "Delete browsing history";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // abOff
            // 
            this.abOff.AutoSize = true;
            this.abOff.Location = new System.Drawing.Point(229, 41);
            this.abOff.Name = "abOff";
            this.abOff.Size = new System.Drawing.Size(42, 18);
            this.abOff.TabIndex = 7;
            this.abOff.TabStop = true;
            this.abOff.Text = "Off";
            this.abOff.UseVisualStyleBackColor = true;
            this.abOff.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // abOn
            // 
            this.abOn.AutoSize = true;
            this.abOn.Location = new System.Drawing.Point(294, 41);
            this.abOn.Name = "abOn";
            this.abOn.Size = new System.Drawing.Size(41, 18);
            this.abOn.TabIndex = 6;
            this.abOn.TabStop = true;
            this.abOn.Text = "On";
            this.abOn.UseVisualStyleBackColor = true;
            this.abOn.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(119, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "Ad-blocking:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button7);
            this.tabPage4.Controls.Add(this.button6);
            this.tabPage4.Controls.Add(this.button5);
            this.tabPage4.Controls.Add(this.button4);
            this.tabPage4.Controls.Add(this.wsdf);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.HttpsProxPort);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.HttpsProxHost);
            this.tabPage4.Controls.Add(this.HttpProxPort);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.HttpProxHost);
            this.tabPage4.Controls.Add(this.checkBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(490, 207);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Proxy";
            this.tabPage4.UseVisualStyleBackColor = true;
            this.tabPage4.Click += new System.EventHandler(this.tabPage4_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(398, 157);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 15;
            this.button7.Text = "Germany";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(317, 157);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 14;
            this.button6.Text = "Canada";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(236, 157);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 13;
            this.button5.Text = "US";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(155, 157);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 12;
            this.button4.Text = "Taiwan";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // wsdf
            // 
            this.wsdf.AutoSize = true;
            this.wsdf.Location = new System.Drawing.Point(51, 161);
            this.wsdf.Name = "wsdf";
            this.wsdf.Size = new System.Drawing.Size(77, 14);
            this.wsdf.TabIndex = 11;
            this.wsdf.Text = "Quick select:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(407, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 14);
            this.label7.TabIndex = 10;
            this.label7.Text = "Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(215, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 14);
            this.label6.TabIndex = 9;
            this.label6.Text = "IP/Host";
            // 
            // HttpsProxPort
            // 
            this.HttpsProxPort.Location = new System.Drawing.Point(402, 114);
            this.HttpsProxPort.Name = "HttpsProxPort";
            this.HttpsProxPort.Size = new System.Drawing.Size(41, 22);
            this.HttpsProxPort.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 14);
            this.label5.TabIndex = 7;
            this.label5.Text = "HTTPS";
            // 
            // HttpsProxHost
            // 
            this.HttpsProxHost.Location = new System.Drawing.Point(112, 114);
            this.HttpsProxHost.Name = "HttpsProxHost";
            this.HttpsProxHost.Size = new System.Drawing.Size(266, 22);
            this.HttpsProxHost.TabIndex = 6;
            // 
            // HttpProxPort
            // 
            this.HttpProxPort.Location = new System.Drawing.Point(402, 83);
            this.HttpProxPort.Name = "HttpProxPort";
            this.HttpProxPort.Size = new System.Drawing.Size(41, 22);
            this.HttpProxPort.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 14);
            this.label4.TabIndex = 4;
            this.label4.Text = "HTTP";
            // 
            // HttpProxHost
            // 
            this.HttpProxHost.Location = new System.Drawing.Point(112, 83);
            this.HttpProxHost.Name = "HttpProxHost";
            this.HttpProxHost.Size = new System.Drawing.Size(266, 22);
            this.HttpProxHost.TabIndex = 3;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(41, 25);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(127, 18);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Use a proxy server";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(416, 236);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(335, 236);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Options";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.BlanchedAlmond;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.ComboBox general;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox homepage;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton abOff;
        private System.Windows.Forms.RadioButton abOn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox HttpsProxPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox HttpsProxHost;
        private System.Windows.Forms.TextBox HttpProxPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox HttpProxHost;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label wsdf;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton abAggressive;
        private System.Windows.Forms.RadioButton abConservative;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
    }
}