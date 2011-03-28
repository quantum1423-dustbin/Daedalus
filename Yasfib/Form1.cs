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
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; private set; }
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
        [DllImport("winmm.dll")]
        public static extern long PlaySound(String lpszName, long hModule, long dwFlags);

        
        public MainForm()
        {
            InitializeComponent();
            if (Instance != null)
                throw new Exception("Only one instance of Form1 is allowed");

            Instance = this;
            Skybound.Gecko.Xpcom.Initialize("xulrunner-o");
            //Skybound.Gecko.Xpcom.ProfileDirectory = "%APPDATA%\\";
            ProcessStartInfo theProcess = new ProcessStartInfo("privoxy.exe");
            theProcess.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(theProcess);
            Skybound.Gecko.GeckoPreferences.User["general.useragent.override"]="Mozilla/5.0 (Windows; U; en-US; rv:1.9.0.10) Gecko/2009042316 Firefox/3.0.10 (fake; Yasfib 4.0.x; Windows; U; .NET CLR 2.0)";
            //lowToolStripMenuItem.Enabled = true;
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
            Skybound.Gecko.GeckoPreferences.User["javascript.enabled"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining.ssl"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.proxy.pipelining"] = true;
            Skybound.Gecko.GeckoPreferences.User["nglayout.initialpaint.delay"] = 100;
            //MessageBox.Show("You are now using high-grade security. Privoxy is helping you block phishing websites and advertisements.");
            //geckoWebBrowser1.Navigate("http://w3.org");
            addGeckoTab();
            if (isChinese == true)
            {
                translate2CN();
            }
            getautocomplete();

        }
        public bool isShell()
        {
            if (((Form)(this.tabControl1.SelectedForm)).Tag != "A")
            {
                return true;
            }
            else return false;
        }
        void nv(string url)
        {
            if (((Form)(this.tabControl1.SelectedForm)).Tag != "A")
            {
                if (url != "about:easteregg")
                {
                    gwb.Navigate(url);
                }
                else
                {
                    gradientDown.Enabled = true;
                    textBox1.BackColor = Color.White;
                }
            }
            else
            {
                MessageBox.Show("");
                if (url != "about:easteregg")
                {
                    swb.Navigate(url);
                }
                else
                {
                    gradientDown.Enabled = true;
                    textBox1.BackColor = Color.White;
                }
            }

        }

        public System.Collections.Generic.List<string> badwords()
        {
            const string f = "badwords.txt";

            // 1
            // Declare new List.
            List<string> lines = new List<string>();

            // 2
            // Use using StreamReader for disposing.
            using (StreamReader r = new StreamReader(f))
            {
                // 3
                // Use while != null pattern for loop
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    // 4
                    // Insert logic here.
                    // ...
                    // "line" is a line in the file. Add it to our List.
                    lines.Add(line);
                }
            }
            return lines;
        }
        public System.Collections.Generic.List<string> iaKeywords()
        {
            const string f = "iakeywords.txt";

            // 1
            // Declare new List.
            List<string> lines = new List<string>();

            // 2
            // Use using StreamReader for disposing.
            using (StreamReader r = new StreamReader(f))
            {
                // 3
                // Use while != null pattern for loop
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    // 4
                    // Insert logic here.
                    // ...
                    // "line" is a line in the file. Add it to our List.
                    lines.Add(line);
                }
            }
            return lines;
        }
        public static bool isChinese = false;
        public static string versionNumber = "4.2.0-r1";
        void getautocomplete()
        {
            try
            {
            // Create an isntance of XmlTextReader and call Read method to read the file
            XmlTextReader textReader = new XmlTextReader("ac.xml");
            textReader.Read();
            // If the node has value
            while (textReader.Read())
            {
                // Move to fist element
                textReader.MoveToElement();
                if (textReader.Name == "url")
                {
                    //MessageBox.Show("");
                    textBox1.AutoCompleteCustomSource.Add(textReader.ReadString());
                    //textBox1.Items.Add(textReader.ReadString());
                    //MessageBox.Show(textReader.ReadString());
                }
            }
            }
            catch { }
        }
        public int aBlockPortNumber = 50029;
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
        private void select(object sender, EventArgs e)
        {
                textBox1.Text = Convert.ToString(gwb.Url);
                updateTitle();
            rtab();
            textBox1.BackColor = Color.White;
        }
        private void selectS(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = Convert.ToString(swb.Url);
                updateTitleS();
                rtab();
            }
            catch { }
        }
        void readBM()

        {
            try
            {
                bool running = true;
                string end = "";
                TextReader tr = new StreamReader("bookmarks.txt");
                while (end == end)
                {
                    end = tr.ReadLine();
                    if (end == null) { break; }
                    string name = end;
                    end = tr.ReadLine();
                    if (end == null) { break; }
                    string url = end;
                    addtoBookmarksMenu(url, name);
                }
                tr.Close();
            }
            catch { }
        }
        void addtoBookmarksMenu(string url, string name){
            ToolStripMenuItem newmi = new ToolStripMenuItem();
            newmi.Tag = url;
            newmi.Text = name;
            newmi.Click += new EventHandler(newmi_Click);
            ToolStripMenuItem del = new ToolStripMenuItem();
            del.Text = "Delete/删除";
            del.Click += new EventHandler(del_Click);
            newmi.DropDownItems.Add(del);
            bookmarksToolStripMenuItem1.DropDownItems.Add((ToolStripItem)newmi);
        }
        private void del_Click(object sender, EventArgs e)
        {
            ((ToolStripMenuItem)sender).OwnerItem.Dispose();
        }
        void newmi_Click(object sender, EventArgs e)
        {
            nv(((ToolStripMenuItem)sender).Tag.ToString());
        }
        public void menu(object sender, Skybound.Gecko.GeckoContextMenuEventArgs e)
        {

        }
        void menu_openinnewtab(object sender, EventArgs e)
        {
            string url = gwb.StatusText;
            addGeckoTab();
            nv(url);
        }
        void translate2CN()
        {
            this.Text = "灵智浏览器 "+versionNumber;
            fileToolStripMenuItem1.Text = "文件";
            editToolStripMenuItem.Text = "工具";
            bookmarksToolStripMenuItem1.Text = "收藏";
            printPageToolStripMenuItem1.Text = "打印...";
            exitToolStripMenuItem1.Text = "退出";
            bookmarkThisPageToolStripMenuItem.Text = "收藏此页";
            //showBookmarksToolStripMenuItem.Text = "显示所有收藏";
            viewSourceToolStripMenuItem.Text = "本页源代码";
            optionsToolStripMenuItem.Text = "快速选项";
            pipeliningToolStripMenuItem.Text = "网络加速";
            extremeToolStripMenuItem.Text = "疯狂";
            normalToolStripMenuItem.Text = "普通";
            offToolStripMenuItem1.Text = "关";
            //举报本网页ToolStripMenuItem.Text = "举报本网页";
            //升级Privoxy过滤信息ToolStripMenuItem.Text = "升级Privoxy过滤信息";
            downloadManagerToolStripMenuItem.Text = "下载管理器";
            //button5.Text = "举报本网页";
            //linkLabel1.Text = "不兼容？使用IE壳模式";
            //enablePublicAntiBlockingToolStripMenuItem.Text = "一键访问被封杀网页";
            enableAntiblockingToolStripMenuItem1.Text = "开始反封杀";
            添加新标签ToolStripMenuItem.Text = "添加新标签 (Ctrl + T)";
            addTabCtrlTToolStripMenuItem.Text = "添加新标签";
            manageBookmarksToolStripMenuItem.Text = "手动编辑收藏夹";
            toolStripSplitButton1.Text = "主菜单";
            //button7.Text = "压";
            //setHomePageToolStripMenuItem.Text = "将本页设为主页";
            upgradeAntiblockingModuleToolStripMenuItem.Text = "升级反封杀模块";
            button8.Text = "无痕";
            deleteBrowsingHistoryToolStripMenuItem.Text = "删除浏览记录";
            fileSharingToolStripMenuItem.Text = "AAS极速网盘";
            editBadWordListToolStripMenuItem.Text = "编辑脏词数据库";
            editInappropriateContentKeywordListToolStripMenuItem.Text = "编辑黄色内容关键词";
            //buttonX2.Text = "可能为钓鱼网站！";
            //printPreviewToolStripMenuItem.Text = "打印预览";
            //checkBoxX1.Text = "语音提示";
            optionsToolStripMenuItem1.Text = "首选项...";
            //enableExperimentalFeaturesToolStripMenuItem.Text = "使用测试功能";
            findbox.Text = "查找";
            button6.Text = "下一个";
            openInNewTabToolStripMenuItem1.Text = "在新标签中打开链接";
            reloadToolStripMenuItem.Text = "刷新";
            copyToolStripMenuItem.Text = "复制";
            selectAllToolStripMenuItem.Text = "全选";
            updateYasfibToolStripMenuItem.Text = "升级浏览器";
            bookmarkThisPageToolStripMenuItem1.Text = "收藏此页面";
            viewSourceToolStripMenuItem1.Text = "源代码";
        }
        //Shortcut for browser
        public Skybound.Gecko.GeckoWebBrowser gwb
        {
            get
            {
                return ((Skybound.Gecko.GeckoWebBrowser)(((Form)(this.tabControl1.SelectedForm)).Controls[0]));
            }
        }
        public WebBrowser swb
        {
            get
            {
                return ((WebBrowser)(((Form)(this.tabControl1.SelectedForm)).Controls[0]));
            }
        }
        public string weburl
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }
        public void report()
        {
            nv("http://sb.allaboutstuff.net/report.php?url=" + Convert.ToString(gwb.Url));
        }
        public void checkphish()
        {
        }
        public void checkphishie()
        {
            /*try
            {
                // Create an isntance of XmlTextReader and call Read method to read the file
                XmlTextReader textReader = new XmlTextReader("http://checkurl.phishtank.com.nyud.net/checkurl/index.php?app_key=03432834ef95fa478034e81ca1cbdebc222bb7be07ad5830122fa1abf3e3d7fc&url=" + textBox1.Text);
                textReader.Read();
                // If the node has value
                while (textReader.Read())
                {
                    // Move to fist element
                    textReader.MoveToElement();
                    if (textReader.Name == "in_database")
                    {
                        if (textReader.ReadString() == "true")
                        {
                            if (checkBox1.Checked == true)
                            {
                                MessageBox.Show("警告！本网站为钓鱼网站！");
                                ((WebBrowser)(this.tabControl1s.SelectedTab.Controls[0])).Navigate("about:blank");
                                break;
                            }
                            else
                            {
                                MessageBox.Show("Warning! This is a phishing site!");
                                ((WebBrowser)(this.tabControl1s.SelectedTab.Controls[0])).Navigate("about:blank");
                                break;
                            }
                        }
                    }
                }
            }
            catch { }*/
        }
        public void addGeckoTab()
        {
            //mditabcontrol code
                Form foobar = new Form();
                //foobar.Location.X = 5;
                //foobar.Location.Y = 502;
                tabControl1.TabPages.Add(foobar);
            //tabControl2.TabPages.Add(foobar);
            Skybound.Gecko.GeckoWebBrowser browser1 = new Skybound.Gecko.GeckoWebBrowser();
            foobar.Controls.Add(browser1);
            browser1.Dock = DockStyle.Fill;
            browser1.Navigated += new
            Skybound.Gecko.GeckoNavigatedEventHandler(nav);
            browser1.ProgressChanged += new
            Skybound.Gecko.GeckoProgressEventHandler(loading);
            browser1.CreateWindow += new
            Skybound.Gecko.GeckoCreateWindowEventHandler(geckoWebBrowser1_CreateWindow);
            browser1.ShowContextMenu += new
Skybound.Gecko.GeckoContextMenuEventHandler(menu);
            browser1.StatusTextChanged += new EventHandler(changing);
            browser1.BackColor = System.Drawing.Color.White;
            browser1.ContextMenuStrip = browserCM;
            browser1.NoDefaultContextMenu = true;
            browser1.DomMouseDown += new Skybound.Gecko.GeckoDomMouseEventHandler(browser1_DomMouseDown);
            //browser1.Navigate("about:blank");
            foobar.GotFocus+= new
            EventHandler(select);
            foobar.Disposed +=
                new EventHandler(dd);

            //browser1.AllowDnsPrefetch = false;
            //browser1.BlockPopups = true;
            textBox1.Text = "about:blank";
            foobar.Focus();
            rtab();
        }

        void browser1_DomMouseDown(object sender, Skybound.Gecko.GeckoDomMouseEventArgs e)
        {
            if (e.Button.ToString()=="2")
            {
                mainCM.Show(Cursor.Position);
                if (gwb.StatusText.StartsWith("http://") ||
                    gwb.StatusText.StartsWith("about:") ||
                    gwb.StatusText.StartsWith("https://"))
                {
                    openInNewTabToolStripMenuItem1.Enabled = true;
                }
                else { openInNewTabToolStripMenuItem1.Enabled = false; }
            }
        }
        private void dd(object sender, EventArgs e)
        {
            rtab();
        }
        void changing(object sender, EventArgs e)
        {
            label1.Text = gwb.StatusText;
            //updateStatusText();
            if (label1.Text == "" || label1.Text == null)
            {
                panel2.Visible = false;
            }
            else
            {
                panel2.Visible = true;
            }
        }
        void changingS(object sender, EventArgs e)
        {
            label1.Text = swb.StatusText;
            //updateStatusText();
            if (label1.Text == "" || label1.Text == null)
            {
                panel2.Visible = false;
            }
            else
            {
                panel2.Visible = true;
            }
        }
        void button1_Click_1(object sender, EventArgs e)
        {
            addGeckoTab();
        }
        public bool isPrivacyMode = false;
        void nav(object sender, Skybound.Gecko.GeckoNavigatedEventArgs e)
        {
            try
            {
                textBox1.Text = Convert.ToString(gwb.Url);
                string text = Convert.ToString(gwb.Url);
                //System.IO.File.AppendAllText(@"ac.xml", "<url>" + text + "</url>");
                textBox1.AutoCompleteCustomSource.Add(Convert.ToString(gwb.Url));
                label1.Text = "Ready";
                System.Threading.Thread sampleThread;
                System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
                ThreadStart myThreadDelegate = new ThreadStart(checkphish);
                sampleThread = new Thread(myThreadDelegate);
                sampleThread.IsBackground = true;
                sampleThread.SetApartmentState(ApartmentState.STA);
                sampleThread.Start();
                if (isPrivacyMode == false)
                {
                    XmlDocument originalXml = new XmlDocument();
                    string s = System.IO.File.ReadAllText("ac.xml");
                    originalXml.LoadXml(s);
                    XmlNode menu = originalXml.SelectSingleNode("i");
                    XmlNode newSub = originalXml.CreateNode(XmlNodeType.Element, "url", null);
                    XmlText fff = originalXml.CreateTextNode(textBox1.Text);
                    newSub.AppendChild(fff);
                    menu.AppendChild(newSub);
                    System.IO.File.WriteAllText(@"ac.xml", originalXml.OuterXml.ToString());
                }
                phishLock = false;
                //Bitmap bmpIcon = ((Bitmap)gwb.FaviconAsImage);
                //((Form)(this.tabControl1.SelectedForm)).Icon = Icon.FromHandle(bmpIcon.GetHicon());
            }
            catch { }
        }
        void navIE(object sender, WebBrowserNavigatedEventArgs e)
        {
            textBox1.Text = Convert.ToString(((WebBrowser)sender).Url);
            string text = Convert.ToString(gwb.Url);
            //System.IO.File.AppendAllText(@"ac.xml", "<url>" + text + "</url>");
            textBox1.AutoCompleteCustomSource.Add(Convert.ToString(gwb.Url));
            label1.Text = "Ready";
            System.Threading.Thread sampleThread;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            ThreadStart myThreadDelegate = new ThreadStart(checkphish);
            sampleThread = new Thread(myThreadDelegate);
            sampleThread.IsBackground = true;
            sampleThread.SetApartmentState(ApartmentState.STA);
            sampleThread.Start();
            if (isPrivacyMode == false)
            {
                XmlDocument originalXml = new XmlDocument();
                string s = System.IO.File.ReadAllText("ac.xml");
                originalXml.LoadXml(s);
                XmlNode menu = originalXml.SelectSingleNode("i");
                XmlNode newSub = originalXml.CreateNode(XmlNodeType.Element, "url", null);
                XmlText fff = originalXml.CreateTextNode(textBox1.Text);
                newSub.AppendChild(fff);
                menu.AppendChild(newSub);
                System.IO.File.WriteAllText(@"ac.xml", originalXml.OuterXml.ToString());
            }
            phishLock = false;
        }
        void nav(object sender, EventArgs e)
        {
            //textBox1.Text = Convert.ToString(browser2.Url);
        }
        void loading(object sender, Skybound.Gecko.GeckoProgressEventArgs e)
        {
            try
            {
                progressBar1.Maximum = e.MaximumProgress;
                progressBar1.Value = e.CurrentProgress;
                if (e.CurrentProgress + "/" + e.MaximumProgress == "100/100")
                {
                    progressBar1.Visible = false;
                    button10.Visible = false;
                    button1.Enabled = true;
                }
                else {
                    progressBar1.Visible = true;
                    button10.Visible = true;
                    button1.Enabled = false;
                }
                if (gwb.DocumentTitle == "")
                {
                    ((Form)(this.tabControl1.SelectedForm)).Text = Convert.ToString(gwb.Url.DnsSafeHost);
                }
                else
                {
                    ((Form)(this.tabControl1.SelectedForm)).Text = gwb.DocumentTitle;

                    //textBox1.Text = Convert.ToString(gwb.Url);
                }
                //}
                //else { label1.Text = "Connecting to the web page: " + e.CurrentProgress + " out of " + e.MaximumProgress; progressBar1.Value = e.CurrentProgress; }
                //updateStatusText();
                updateTitle();
            }
            catch { }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "about:daedalus")
            {
                nv("javascript:document.write(\'<html><head><title>About Daedalus</title><meta content=text/html; charset=UTF-8 http-equiv=Content-Type></head><body><font face=Arial><h1>Daedalus "+versionNumber+"</h1>This browser is released under the GNU General Public License v3. If you have downloaded this browser under a restrictive or commercial license, please report abuse to <a href=mailto:ericcesium133@aol.com>the developer</a>.<hr>The entire program is Copyright &copy; 2010~2012 Eric Dong (Quantum1423). Many thanks to the GeckoFX community for helping out with code snippets. The developer hereby releases the product to the open source community under the terms of the GNU General Public License.<hr>\');" + "document.write(\"" + gplv3 + "\");");
                /*nv("javascript:document.write('" + gplv3 + "')");*/
            }
            else if (textBox1.Text == "about:hack:edit")
            {
                nv("JavaScript:document.body.contentEditable='true'; document.designMode='on'; void 0");
            }
            else{
                nv(textBox1.Text);
                if (((Form)(this.tabControl1.SelectedForm)).Tag != "A")
                {
                    gwb.Focus();
                }
                else { swb.Focus(); }
            }
         
        }
        public bool abWorking = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo deProcess = new ProcessStartInfo("abd.exe");
                deProcess.WindowStyle = ProcessWindowStyle.Minimized;
                Process.Start(deProcess);
            }
            catch { MessageBox.Show("Error: corrupted/missing anti-blocking module. Upgrade module immediately \n 错误：丢失反封杀模块。请立即升级反封杀模块。"); abWorking = false; } foreach (Process clsProcess in Process.GetProcesses())
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
                    tabControl1.BackHighColor = System.Drawing.Color.BlanchedAlmond;
                    tabControl1.BackLowColor = System.Drawing.Color.BlanchedAlmond;
                    this.BackgroundImage = null;
                    tabControl1.TabBackHighColorDisabled = Color.LightSkyBlue;
                    //label1.ForeColor = Color.White;
                }
            }
            readBM();
            TextReader gpl = new StreamReader("LICENSE.txt");
            //this.FormBorderStyle = FormBorderStyle.None;
        }
        
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                gwb.GoBack();
            }
            catch { nonFatalError(); }
        }
        void nonFatalError()
        {
            MessageBox.Show("Sorry, an error occured. But don't worry, Daedalus hasn't crashed! \n 对不起，程序出现错误。但不要担心，程序没有崩溃！"); 
        }
        private void button4_Click(object sender, EventArgs e)
        {

            gwb.GoForward();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
                gwb.Reload();

        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gwb.GoBack();
        }

        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gwb.GoForward();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ether 2.1 Under GNU GPLv3. Some Rights Reserved.");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void goToConfigPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nv("about:config");
        }

        private void printDocumentTitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(gwb.DocumentTitle);
            if (gwb.DocumentTitle == "")
            {
                ((Form)(this.tabControl1.SelectedForm)).Text = Convert.ToString(gwb.Url.DnsSafeHost);
                //((Form)(this.tabControl1.SelectedForm)).Icon = "http://" + Convert.ToString(gwb.Url.DnsSafeHost) + "/favicon.ico";
                //textBox1.Text = Convert.ToString(gwb.Url);
                //this.Text = Convert.ToString(gwb.Url) + " - Yasfib 2.0 ETHER";
            }
            else
            {
                ((Form)(this.tabControl1.SelectedForm)).Text = gwb.DocumentTitle;
                //textBox1.Text = Convert.ToString(gwb.Url);
            }
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        
        {
            try
            {
                ((Form)(this.tabControl1.SelectedForm)).Text = gwb.DocumentTitle;
                if (gwb.DocumentTitle == "")
                {
                    ((Form)(this.tabControl1.SelectedForm)).Text = Convert.ToString(gwb.Url.DnsSafeHost);
                }
            }
            catch { }

        }
        void getIcon()
        {

        }

        private void downloadsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Skybound.Gecko.ChromeDialog dialog = new Skybound.Gecko.ChromeDialog();
            dialog.Name = "DownloadsForm";
            dialog.Show();
            dialog.WebBrowser.Navigate("chrome://mozapps/content/downloads/downloads.xul");
        }

        private void securityLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void highToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offToolStripMenuItem1.Enabled = true;
            normalToolStripMenuItem.Enabled = false;
            //lowToolStripMenuItem.Enabled = true;
            Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 1;
            Skybound.Gecko.GeckoPreferences.User["network.proxy.http"] = "127.0.0.1";
            Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl"] = "127.0.0.1";
            Skybound.Gecko.GeckoPreferences.User["network.proxy.http_port"] = 8118;
            Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl_port"] = 8118;
            Skybound.Gecko.GeckoPreferences.User["javascript.enabled"] = true;
            //MessageBox.Show("You are now using high-grade security. Privoxy is helping you block phishing websites and advertisements.");
        }

        private void lowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            offToolStripMenuItem1.Enabled = true;
            normalToolStripMenuItem.Enabled = true;
            //lowToolStripMenuItem.Enabled = false;
            Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 0;
            Skybound.Gecko.GeckoPreferences.User["javascript.enabled"] = true;
            //MessageBox.Show("You are now using low-grade security. This mode should only be used in cases of incompatibility or broken websites. No ad-blocking or anti-phishing in action.");
        }

        private void paranoidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //lowToolStripMenuItem.Enabled = true;
            Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 1;
            Skybound.Gecko.GeckoPreferences.User["network.proxy.http"] = "127.0.0.1";
            Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl"] = "127.0.0.1";
            Skybound.Gecko.GeckoPreferences.User["network.proxy.http_port"] = 8118;
            Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl_port"] = 8118;
            Skybound.Gecko.GeckoPreferences.User["javascript.enabled"] = false;
            //MessageBox.Show("Paranoid security will remove JS and will break most websites.");
        }

        private void Form1_Leave(object sender, EventArgs e)
        {

        }
        void rpriv()
        {
            Process[] processes = Process.GetProcessesByName("privoxy");
            foreach (Process process in processes)
            {
                process.Kill();
            }
            ProcessStartInfo theProcess = new ProcessStartInfo("privoxy.exe");
            theProcess.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(theProcess);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            proc("privoxy\\no.bat");
            Process[] processes = Process.GetProcessesByName("privoxy");
            foreach (Process process in processes)
            {
                process.Kill();
            }
            Process[] processio = Process.GetProcessesByName("abd");
            foreach (Process process in processio)
            {
                process.Kill();
            }
            int i = 0;
            TextWriter tr = new StreamWriter("bookmarks.txt");
            foreach (ToolStripMenuItem mark in bookmarksToolStripMenuItem1.DropDownItems)
            {
                if (i >= 3)
                {
                    tr.WriteLine(mark.Text);
                    tr.WriteLine(mark.Tag);
                }
                i++;
            }
            tr.Close();
            Instance = null;
        }
        void updateTitle()
        {
            if (isChinese == false)
            {
                if (gwb.DocumentTitle != "")
                {
                    this.Text = gwb.DocumentTitle + " - Daedalus " + versionNumber;
                }
                else
                {
                    this.Text = gwb.Url.ToString()+ " - Daedalus " + versionNumber;
                }
            }
            else
            {
                if (gwb.DocumentTitle != "")
                {
                    this.Text = gwb.DocumentTitle + " - 灵智浏览器 " + versionNumber;
                }
                else
                {
                    this.Text = gwb.Url.ToString() + " - 灵智浏览器 " + versionNumber;
                }
            }
            textBox1.Text = Convert.ToString(gwb.Url);
        }
        void updateTitleS()
        {
            if (isChinese == false)
            {
                if (swb.DocumentTitle != "")
                {
                    this.Text = swb.DocumentTitle + " - Daedalus " + versionNumber;
                }
                else
                {
                    this.Text = swb.Url.ToString() + " - Daedalus " + versionNumber;
                }
            }
            else
            {
                if (swb.DocumentTitle != "")
                {
                    this.Text = swb.DocumentTitle + " - 灵智浏览器 " + versionNumber;
                }
                else
                {
                    try
                    {
                        this.Text = swb.Url.ToString() + " - 灵智浏览器 " + versionNumber;
                    }
                    catch { }
                }
            }
            textBox1.Text = Convert.ToString(gwb.Url);
        }
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            updateTitle();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 fm = new AboutBox1();
            fm.Show();
        }

        private void addBookmarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addtoBookmarksMenu(gwb.Url.ToString(), gwb.DocumentTitle);
            /*StreamWriter tr;
            tr = File.AppendText("bookmarks.txt");
            tr.WriteLine(gwb.DocumentTitle);
            tr.WriteLine(gwb.Url.ToString());
            tr.Close();*/
        }

        private void showBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nv("file:///" + Application.StartupPath.Replace("\\", "/") + "/fav.html");
            //MessageBox.Show("file:///" + Application.StartupPath + "fav.html");
        }

        private void tabControl1s_Selecting(object sender, TabControlEventArgs e)
        {

            
        }

        private void tabControl1s_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        void fgab()
        {
            if (rf("ab") == "a")
            {
                Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 1;
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http"] = "localhost";
                Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl"] = "localhost";
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http_port"] = aBlockPortNumber;
                Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl_port"] = aBlockPortNumber;
                Skybound.Gecko.GeckoPreferences.User["nglayout.initialpaint.delay"] = 800;
                optionsToolStripMenuItem1.Enabled = false;
                Skybound.Gecko.GeckoPreferences.User["javascript.enabled"] = true;
            }
            else
            {
                Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 1;
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http"] = "localhost";
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http_port"] = 8118;
                Skybound.Gecko.GeckoPreferences.User["nglayout.initialpaint.delay"] = 100;
                optionsToolStripMenuItem1.Enabled = false;
                Skybound.Gecko.GeckoPreferences.User["javascript.enabled"] = true;
                proc("privoxy\\ab.bat");
                rpriv();
            }
        }
        void ab()
        {
            if (abWorking == false)
            {
                MessageBox.Show("Error: corrupted/missing anti-blocking module. Upgrade module immediately \n 错误：丢失反封杀模块。请立即升级反封杀模块。");
            }
            else
            {
                if (enableAntiblockingToolStripMenuItem1.Text == "Enable Anti-blocking" || enableAntiblockingToolStripMenuItem1.Text == "开始反封杀")
                {
                    button7.Enabled = false;
                    if (isChinese == true)
                    {
                        fgab();
                        button10.Enabled = false;
                        enableAntiblockingToolStripMenuItem1.Text = "停止反封杀";
                    }
                    else
                    {
                        fgab();
                        button10.Enabled = false;
                        enableAntiblockingToolStripMenuItem1.Text = "Disable Anti-blocking";
                    }
                }
                else
                {
                    button7.Enabled = true;
                    //button10.Enabled = true;
                    proc("privoxy\\no.bat");
                    optionsToolStripMenuItem1.Enabled = true;
                    //lowToolStripMenuItem.Enabled = true;
                    if (rf("config/proxybool.conf") == "1")
                    {
                        Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = Convert.ToInt32(rf("config/proxybool.conf"));
                        Skybound.Gecko.GeckoPreferences.User["network.proxy.http"] = rf("config/uproxyip.conf");
                        Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl"] = rf("config/sproxyip.conf");
                        Skybound.Gecko.GeckoPreferences.User["network.proxy.http_port"] = Convert.ToInt32(rf("config/uproxyport.conf"));
                        Skybound.Gecko.GeckoPreferences.User["network.proxy.ssl_port"] = Convert.ToInt32(rf("config/sproxyport.conf"));
                        Skybound.Gecko.GeckoPreferences.User["nglayout.initialpaint.delay"] = 100;
                    }
                    else
                    {
                        Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 0;
                    }
                    if (isChinese == false)
                    {
                        enableAntiblockingToolStripMenuItem1.Text = "Enable Anti-blocking";
                    }
                    else
                    {
                        enableAntiblockingToolStripMenuItem1.Text = "开始反封杀";
                    }
                }
            }
        }
        private void premiumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ab();
        }
        void setProxy()
        {
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
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void printPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form3 fma = new Form3();
                fma.webBrowser1.Navigate(Convert.ToString(gwb.Url));
                fma.webBrowser1.AllowNavigation = false;
                fma.webBrowser1.IsWebBrowserContextMenuEnabled = false;
                fma.Show();
            }
            catch { }
        }

        private void qToolBar1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining.ssl"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.proxy.pipelining"] = true;
            normalToolStripMenuItem.Enabled = false;
            extremeToolStripMenuItem.Enabled = true;
            offToolStripMenuItem1.Enabled = true;
        }

        private void extremeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining.ssl"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.proxy.pipelining"] = true;
            Skybound.Gecko.GeckoPreferences.User["network.http.proxy.pipelining.maxrequests"] = "100";
            Skybound.Gecko.GeckoPreferences.User["network.http.keep-alive.timeout"] = 3000;
            Skybound.Gecko.GeckoPreferences.User["network.http.max-connections"] = 100;
            Skybound.Gecko.GeckoPreferences.User["network.http.max-connections-per-server"] = 50;
            Skybound.Gecko.GeckoPreferences.User["network.http.max-persistent-connections-per-server"] = 20;
            Skybound.Gecko.GeckoPreferences.User["network.http.max-persistent-connections-per-proxy"] = 20;
            normalToolStripMenuItem.Enabled = true;
            extremeToolStripMenuItem.Enabled = false;
            offToolStripMenuItem1.Enabled = true;
        }

        private void offToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining"] = false;
            Skybound.Gecko.GeckoPreferences.User["network.http.pipelining.ssl"] = false;
            Skybound.Gecko.GeckoPreferences.User["network.http.proxy.pipelining"] = false;
            normalToolStripMenuItem.Enabled = true;
            extremeToolStripMenuItem.Enabled = true;
            offToolStripMenuItem1.Enabled = false;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            //toolTip1.Show("fsdjfkds", this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("");
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 本页源代码ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = textBox1.Text;
            addGeckoTab();
            nv
                ("view-source:" + url);
        }

        private void printProgressMaxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(progressBar1.Maximum));
        }

        private void printProgressValueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(progressBar1.Value));
        }

        private void 升级PhishTank数据库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("get_phishtank.bat");
        }

        private void 升级Privoxy过滤信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("get_privoxy.bat");
        }

        private void bookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 全面升级ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("get_all.bat");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //linkLabel1.Visible = false;
        }

        private void printDNSSafeHostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString(gwb.Url.DnsSafeHost));
        }

        private void 举报本网页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            report();
        }

        private void 下载管理器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Skybound.Gecko.ChromeDialog dialog = new Skybound.Gecko.ChromeDialog();
            dialog.Name = "DownloadsForm";
            dialog.Show();
            dialog.WebBrowser.Navigate("chrome://mozapps/content/downloads/downloads.xul");
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            addGeckoTab();
        }

        private void 添加新标签ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addGeckoTab();
        }

        private void tabControl1_Load(object sender, EventArgs e)
        {
        }

        private void 确定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            report();
        }

        private void geckoWebBrowser1_CreateWindow(object sender, Skybound.Gecko.GeckoCreateWindowEventArgs e)
        {
            addGeckoTab();
            e.WebBrowser = gwb;
            gwb.Show();
        }

        private void enablePublicAntiBlockingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nv("http://127.0.0.1:50000/do/Qa_k/vDDvw0ba3ab66LYNa/3at/a.html");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            nv("http://www.google.com/search?q=" + textBox2.Text);
        }

        private void googleThisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nv("http://www.google.com/search?q=" + textBox2.Text);
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            this.AcceptButton = button2;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button2;
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void tabControl1_Load_1(object sender, EventArgs e)
        {
            TextReader tr = new StreamReader("home");
            nv(tr.ReadLine());
            tr.Close();
            
        }

        private void textBox1_TextUpdate(object sender, EventArgs e)
        {

            if (gwb.DocumentTitle == "")
            {
                ((Form)(this.tabControl1.SelectedForm)).Text = Convert.ToString(gwb.Url.DnsSafeHost);
                updateTitle();
            }
            else
            {
                ((Form)(this.tabControl1.SelectedForm)).Text = gwb.DocumentTitle;
                updateTitle();
            }
        }

        private void tabControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            rtab();
        }

        private void tabControl1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void tabControl1_GetTabRegion(object sender, MdiTabControl.TabControl.GetTabRegionEventArgs e)
        {
            // you can create a new point array or just modify the existing one
            e.Points[1] = new Point(7, 0);
            e.Points[4] = new Point(e.TabWidth-7, 0);
        }

        private void caretBrowsingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

       

        private void textBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.FlatStyle==FlatStyle.Flat)
            {
                setProxy();
                button7.FlatStyle = FlatStyle.System;
                button7.ForeColor = Color.Black;
                optionsToolStripMenuItem.Enabled = true;
                optionsToolStripMenuItem1.Enabled = true;
            }
            else
            {
                button7.BackColor = System.Drawing.Color.Green;
                button7.ForeColor = Color.White;
                Skybound.Gecko.GeckoPreferences.User["network.proxy.type"] = 1;
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http"] = "yasfib-web-compressor.dyndns.org";
                Skybound.Gecko.GeckoPreferences.User["network.proxy.http_port"] = 8080;
                button7.FlatStyle = FlatStyle.Flat;
                optionsToolStripMenuItem.Enabled = false;
                optionsToolStripMenuItem1.Enabled = false;
            }
        }

        private void addTabCtrlTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addGeckoTab();
            nv("about:blank");
        }

        private void manageBookmarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "bookmarks.txt");
        }

        private void setHomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to set this page as home? \n 您是否想将此页当作首页？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                TextWriter tr = new StreamWriter("home");
                tr.WriteLine(gwb.Url.ToString());
                tr.Close();
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }
        void proc(string filename)
        {
            Process dahProcess = new Process();
            dahProcess.StartInfo.FileName = filename;
            dahProcess.Start();
        }
        public bool i = true;
        public bool phishLock = false;
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            try
            {
                if (gwb.Url.Port == 443)
                {
                    textBox1.BackColor = Color.PaleGreen;
                }
                else
                {
                    if (i == true)
                    {
                        Process dahProcess = new Process();
                        dahProcess.StartInfo.FileName = "sab.bat";
                        dahProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        dahProcess.Start();
                    }
                    if (phishLock == false)
                    {
                        if (Yasfib.FishPhish.checkAll(gwb.Document.Body.InnerHtml, textBox1.Text) == true)
                        {
                            textBox1.BackColor = System.Drawing.Color.Red;
                            //buttonX2.Visible = true;
                            phishLock = true;
                            warnPhish();
                        }
                        else { textBox1.BackColor = Color.White; }
                    }
                }
            }
            catch { }
        }
        void warnPhish()
        {
                MessageBox.Show("Phishing site detected. \n 检测出钓鱼网站！", "Warning!");
        }
        private void upgradeAntiblockingModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebClient Client = new WebClient();
            Client.DownloadFileAsync(new Uri("http://www.bcdmmtq.tk/~yasfib/abd.exe"), @"abd.exe");
            Client.DownloadFileCompleted += new AsyncCompletedEventHandler(uac);
            MessageBox.Show("Upgrade may take a few minutes. Please wait until you are notified. \n 升级可能需要几分钟，请耐心等候提示。");
        }
        private void uac(object sender, EventArgs e)
        {
            MessageBox.Show("Upgrade Completed! \n 升级完毕！");
            abWorking = true;
            i = false;
            try
            {
                ProcessStartInfo deProcess = new ProcessStartInfo("abd.exe");
                deProcess.WindowStyle = ProcessWindowStyle.Minimized;
                Process.Start(deProcess);
            }
            catch { MessageBox.Show("Error: corrupted/missing anti-blocking module. Upgrade module immediately \n 错误：丢失反封杀模块。请立即升级反封杀模块。"); abWorking = false; }
        }

        private void Form1_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.FlatStyle==FlatStyle.Flat)
            {
                isPrivacyMode = false;
                button8.FlatStyle = FlatStyle.System;
                button8.ForeColor = Color.Black;
            }
            else
            {
                isPrivacyMode=true;
                button8.FlatStyle = FlatStyle.Flat;
                button8.ForeColor = Color.White;
            }
        }

        private void deleteBrowsingHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextWriter rpHistory = new StreamWriter("ac.xml");
            rpHistory.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?><i></i>");
            rpHistory.Close();
            textBox1.AutoCompleteCustomSource.Clear();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (findbox.Visible)
            {
                findbox.Visible = false;
                
            }
            else
            {
                findbox.Visible = true;
                tosearch.Focus();
            }
        }
        public string gplv3 = File.ReadAllText("LICENSE.txt");

        private void button9_Click_1(object sender, EventArgs e)
        {
            addGeckoTab();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            //toolStripSplitButton1.DropDown.Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void autoDotComToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control == true)
            {
                if (e.KeyValue==13)
                {
                    nv("www." + textBox1.Text + ".com");
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox1_MouseCaptureChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
           
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }



        private void qTabControl1_Click(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {
            this.AcceptButton = go;
        }

        private void go_Click(object sender, EventArgs e)
        {
            nv(textBox1.Text);
        }

        private void fileSharingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo deProcess = new ProcessStartInfo("FtpUp.exe");
            Process.Start(deProcess);
        }
        public bool isFiltering = false;

        private void editBadWordListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "badwords.txt");
        }

        private void editInappropriateContentKeywordListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "iakeywords.txt");
        }

        private void buttonX3_Click(object sender, EventArgs e)
        {
            //groupPanel1.Visible = false;
        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void paypalToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void buttonX2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The FishPhish filter has detected a possible phish heuristically. \n AAS自行研制的FishPhish系统智能检出可能的钓鱼网站。\n The content does not seem to match with the website, "+gwb.Url.DnsSafeHost.ToString()+".");
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gwb.CopySelection();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gwb.SelectAll();
        }

        private void reloadToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            gwb.Reload();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nv("javascript:window.print();");
        }

        private void viewSourceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            gwb.ViewSource();
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //gwb.ShowPrintPreview();
        }


        private void button5_Click_2(object sender, EventArgs e)
        {
            TextReader tr = new StreamReader("home");
            nv(tr.ReadLine());
            tr.Close();
        }
        public void addShellTab()
        {
            //mditabcontrol code
            Form foobar = new Form();
            //foobar.Location.X = 5;
            //foobar.Location.Y = 502;
            tabControl1.TabPages.Add(foobar);
            //tabControl2.TabPages.Add(foobar);
            WebBrowser browser1 = new WebBrowser();
            foobar.Controls.Add(browser1);
            browser1.Dock = DockStyle.Fill;
            browser1.Navigated += new
            WebBrowserNavigatedEventHandler(navIE);
            browser1.NewWindow += new CancelEventHandler(browser1_NewWindow);
            browser1.StatusTextChanged += new EventHandler(changingS);
            browser1.DocumentTitleChanged += new EventHandler(browser1_DocumentTitleChanged);
            browser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser1_DocumentCompleted);
            browser1.BackColor = System.Drawing.Color.White;
            browser1.GotFocus += new EventHandler(selectS);
            browser1.ScriptErrorsSuppressed = true;
            browser1.ContextMenuStrip = browserCM;
            //browser1.Navigate("about:blank");
            foobar.GotFocus += new
            EventHandler(selectS);
            foobar.Disposed +=
                new EventHandler(dd);

            //browser1.AllowDnsPrefetch = false;
            //browser1.BlockPopups = true;
            textBox1.Text = "about:blank";
            foobar.Tag = "A";
            foobar.Text = "IE Shell/IE壳";
            foobar.Focus();
            rtab();
        }

        void browser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            HtmlElementCollection links = (((WebBrowser)(sender)).Document.Links);
            foreach (HtmlElement var in links)
            {
                var.AttachEventHandler("onclick", LinkClicked);
            }
        }
        void browser1_DocumentTitleChanged(object sender, EventArgs e)
        {
            updateTitleS();
        }
        public string url = "";
        void browser1_NewWindow(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            nv(url);
        }
        private void tabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(tabControl1.TabPages.Count.ToString());

        }
        private void LinkClicked(object sender, EventArgs e)
        {
            HtmlElement link = swb.Document.ActiveElement;
            url = link.GetAttribute("href");
        }
        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        void rtab()
        {
            try
            {
                if (this.Width <= tabControl1.TabPages.Count * tabControl1.TabMaximumWidth)
                {
                    tabControl1.TabMaximumWidth = (this.Width - 62) / tabControl1.TabPages.Count - 2;
                    tabControl1.TabMinimumWidth = (this.Width - 62) / tabControl1.TabPages.Count - 2;
                }
                else if (this.Width > tabControl1.TabPages.Count * 250)
                {
                    tabControl1.TabMaximumWidth = 250;
                    tabControl1.TabMinimumWidth = 250;
                }
                if (tabControl1.TabPages.Count == 1)
                {
                    tabControl1.TabCloseButtonVisible = false;
                }
                else
                {
                    tabControl1.TabCloseButtonVisible = true;
                }
                buttonX1.Left = (tabControl1.TabMaximumWidth - 14) * tabControl1.TabPages.Count + 20;
            }
            catch { }
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                Form2 fma = new Form2();
                fma.Show();
            }
            catch { }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            addGeckoTab();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
        }

        private void takeScreenshotToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            { this.WindowState = FormWindowState.Normal; }
            else { this.WindowState = FormWindowState.Maximized; }
        }
        private bool drag = false;
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            this.drag = false;
        }
        private Point startPoint = new Point(0, 0); // also for the moving
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            this.startPoint = e.Location;
            this.drag = true;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.drag)
            { // if we should be dragging it, we need to figure out some movement
                Point p1 = new Point(e.X, e.Y);
                Point p2 = this.PointToScreen(p1);
                Point p3 = new Point(p2.X - this.startPoint.X,
                                     p2.Y - this.startPoint.Y);
                this.Location = p3;
            }
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            rtab();
        }

        private void tabControl1_TabPaintBorder(object sender, MdiTabControl.TabControl.TabPaintEventArgs e)
        {
        }

        private void tabControl1_ControlRemoved(object sender, ControlEventArgs e)
        {

        }
        void find()
        {

        }
        private void tosearch_TextChanged(object sender, EventArgs e)
        {
            this.AcceptButton = button6;
        }

        private void button6_Click_2(object sender, EventArgs e)
        {
            if (tosearch.Text != "")
            {
                nv("javascript:window.find(\"" + tosearch.Text + "\", false, false, true, false, true, false); void(0);");
            }
        }

        private void button9_Click_2(object sender, EventArgs e)
        {
            findbox.Visible = false;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkBoxX1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            gwb.Stop();
            button10.Visible = false;
            button1.Enabled = true;
        }

        private void gradientDown_Tick(object sender, EventArgs e)
        {
            if (this.Opacity >= 0.05)
            {
                this.Opacity = this.Opacity - 0.05;
            }
            else
            {
                gradientUp.Enabled = true;
                gradientDown.Enabled = false;
            }
        }

        private void gradientUp_Tick(object sender, EventArgs e)
        {
            if (this.Opacity <= 1)
            {
                this.Opacity = this.Opacity + 0.05;
            }
            else
            {
                gradientDown.Enabled = false;
                gradientUp.Enabled = false;
            }
        }

        private void updateYasfibToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form4 fma = new Form4();
                fma.Show();
            }
            catch { }
        }

        private void openInNewTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string url = gwb.StatusText;
            addGeckoTab();
            nv(url);
        }

        private void textBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.SelectAll();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == Keys.Enter.GetHashCode())
            {
            }
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void phishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (phishLock == false)
            {
                if (Yasfib.FishPhish.checkAll(gwb.Document.DocumentElement.InnerHtml.ToString(), textBox1.Text) == true)
                {
                    textBox1.BackColor = System.Drawing.Color.Red;
                    //buttonX2.Visible = true;
                    phishLock = true;
                    warnPhish();
                }
                else { MessageBox.Show(""); }
            }
        }
        private void documentTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gwb.SaveDocument("C:\\");
            MessageBox.Show(rf("fo.html"));
        }

        private void tabControl1_ForeColorChanged(object sender, EventArgs e)
        {

        }

        private void addShellTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addShellTab();
        }

        private void buttonX1_DoubleClick(object sender, EventArgs e)
        {
            addShellTab();
        }

        private void openANewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addGeckoTab();
        }

        private void openANewIEShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addShellTab();
        }

        private void duplicateIntoIEShellToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void buttonX1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                contextMenuStrip3.Show(Cursor.Position);
            }
            else
            {
                addGeckoTab();
                nv("about:blank");
            }
        }

   }

}







/*
A WORD FROM THE DEVELOPER:
 * 
 * My real name is Eric Dong, though I am known on the Net as Quantum1423. Daedalus is largely a
 * part-time hacking hobby. However, eventually this is going to be something big! And notice,
 * my coding is really bad!
 * Email: ericcesium133@aol.com
 * 
*/