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
            log("---Program Started---");
            if (Instance != null)
                throw new Exception("Only one instance of Form1 is allowed");

            Instance = this;
            Skybound.Gecko.Xpcom.Initialize("xulrunner");
            //Skybound.Gecko.Xpcom.ProfileDirectory = "%APPDATA%\\";
            int moreThanOne = 0;
            Process[] processid = Process.GetProcessesByName("daedalus");
            foreach (Process dode in processid)
            {
                moreThanOne++;
            }
            if (moreThanOne < 2)
            {
                ProcessStartInfo theProcess = new ProcessStartInfo("privoxy.exe");
                theProcess.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(theProcess);
            }
            Skybound.Gecko.GeckoPreferences.User["general.useragent.override"] = "Mozilla/5.0 (U; Windows; en-US; rv:1.9.2) Gecko/20100127 Firefox/3.6 (fake; Daedalus 4.5.x; Windows; U; .NET CLR 2.0)";
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
            Skybound.Gecko.GeckoPreferences.User["nglayout.initialpaint.delay"] = 0;
            Skybound.Gecko.GeckoPreferences.User["browser.cache.memory.enable"] = false;
            //MessageBox.Show("You are now using high-grade security. Privoxy is helping you block phishing websites and advertisements.");
            //geckoWebBrowser1.Navigate("http://w3.org");
            addGeckoTab();
            if (isChinese == true)
            {
                translate2CN();
            }
            getautocomplete();
            log("Initialization complete");
        }
        public bool isShell()
        {
            if (((Form)(this.tabControl1.SelectedForm)).Tag.ToString() != "A")
            {
                return true;
            }
            else return false;
        }
        void nv(string url)
        {
            if (url.Contains("about:") != true)
            {
                log("Navigated to: " + url);
            }
            if (((Form)(this.tabControl1.SelectedForm)).Tag.ToString() != "A")
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
                //MessageBox.Show("");
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
        public static string versionNumber = "4.5.5-r2";
        void getautocomplete()
        {
            textBox1.AutoCompleteCustomSource.Clear();
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
        public static void wf(string filename, string content)
        {
            StreamWriter tr;
            tr = File.CreateText(filename);
            tr.Write(content);
            tr.Close();
        }
        public static string rf(string filename)
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
        void addtoBookmarksMenu(string url, string name)
        {
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
            this.Text = "灵智浏览器 " + versionNumber;
            ppToolStripMenuItem.Text = "打印预览";
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
            //manageBookmarksToolStripMenuItem.Text = "手动编辑收藏夹";
            toolStripSplitButton1.Text = "主菜单";
            //button7.Text = "压";
            //setHomePageToolStripMenuItem.Text = "将本页设为主页";
            upgradeAntiblockingModuleToolStripMenuItem.Text = "升级反封杀模块";
            //button8.Text = "无痕";
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
            openANewIEShellToolStripMenuItem.Text = "打开新的IE壳";
            openANewTabToolStripMenuItem.Text = "打开新标签";
            copyLinkLocationToolStripMenuItem.Text = "复制链接地址";
            programLogToolStripMenuItem.Text = "系统日志";
            findToolStripMenuItem.Text = "查找";
            helpToolStripMenuItem.Text = "帮助";
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
            log("Initialized new tab");
            try
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
                browser1.Navigating += new Skybound.Gecko.GeckoNavigatingEventHandler(browser1_Navigating);
                browser1.ProgressChanged += new
                Skybound.Gecko.GeckoProgressEventHandler(loading);
                browser1.CreateWindow += new
                Skybound.Gecko.GeckoCreateWindowEventHandler(geckoWebBrowser1_CreateWindow);
                browser1.StatusTextChanged += new EventHandler(changing);
                browser1.BackColor = System.Drawing.Color.White;
                browser1.DomMouseDown += new Skybound.Gecko.GeckoDomMouseEventHandler(browser1_DomMouseDown);
                browser1.DocumentCompleted += new EventHandler(browser1_DocumentCompleted);
                //browser1.DomClick += new Skybound.Gecko.GeckoDomEventHandler(browser1_DomClick);
                //1browser1.DomContextMenu += new Skybound.Gecko.GeckoDomMouseEventHandler(browser1_DomContextMenu);
                browser1.NoDefaultContextMenu = true;
                browser1.ContextMenuStrip = mainCM;
                foobar.GotFocus += new
                EventHandler(select);
                foobar.Disposed +=
                    new EventHandler(dd);

                //browser1.AllowDnsPrefetch = false;
                //browser1.BlockPopups = true;
                textBox1.Text = "about:blank";
                foobar.Focus();
                foobar.Tag = "-";
                rtab();
                Bitmap interbediate = new Bitmap(Yasfib.Properties.Resources._001_40);
                ((Form)(this.tabControl1.SelectedForm)).Icon = Icon.FromHandle(interbediate.GetHicon());
                //browser1.Navigate("about:blank");

            }
            catch { }
        }
        void getFavicon()
        {


            // Create the thread object, passing in the Alpha.Beta method
            // via a ThreadStart delegate. This does not start the thread.
            Thread oThread = new Thread(new ThreadStart(this.GF));

            // Start the thread
            oThread.Start();

            // Spin for a while waiting for the started thread to become
            // alive:
            while (!oThread.IsAlive) ;

            // Put the Main thread to sleep for 1 millisecond to allow oThread
            // to do some work:
            Thread.Sleep(1);

            // Request that oThread be stopped
            oThread.Abort();

            // Wait until oThread finishes. Join also has overloads
            // that take a millisecond interval or a TimeSpan object.
            oThread.Join();

        }
        void browser1_DocumentCompleted(object sender, EventArgs e)
        {
            updateTitle();
            //GF();
        }
        void GF()
        {
            
            phishLock = false;
            if (((Form)(this.tabControl1.SelectedForm)).Tag.ToString() != "A")
            {
                if (gwb.Favicon == null)
                {
                    Bitmap interbediate = new Bitmap(Yasfib.Properties.Resources._001_40);
                    ((Form)(this.tabControl1.SelectedForm)).Icon = Icon.FromHandle(interbediate.GetHicon());
                    //((Form)(this.tabControl1.SelectedForm)).Refresh();
                }
                else
                {
                    ((Form)(this.tabControl1.SelectedForm)).Icon = gwb.Favicon;
                    //((Form)(this.tabControl1.SelectedForm)).Refresh();
                }
            }
            else
            {

            }
 
        }
        void browser1_DomContextMenu(object sender, Skybound.Gecko.GeckoDomMouseEventArgs e)
        { }

        void browser1_DomClick(object sender, Skybound.Gecko.GeckoDomEventArgs e)
        {
        }

        void browser1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        void browser1_MouseWheel(object sender, MouseEventArgs e)
        {

        }

        void browser1_Navigating(object sender, Skybound.Gecko.GeckoNavigatingEventArgs e)
        {
            //dl.RunWorkerAsync(e.Uri.ToString());
        }

        void browser1_DomMouseDown(object sender, Skybound.Gecko.GeckoDomMouseEventArgs e)
        {
            cacheString = gwb.StatusText;
            if (e.Button.ToString() == "2")
            {
                mainCM.Show(Cursor.Position);
                if (cacheString.StartsWith("http://") ||
                    cacheString.StartsWith("about:") ||
                    cacheString.StartsWith("https://"))
                {
                    openInNewTabToolStripMenuItem1.Visible = true;
                    copyLinkLocationToolStripMenuItem.Visible = true;
                }
                else { openInNewTabToolStripMenuItem1.Visible = false; copyLinkLocationToolStripMenuItem.Visible = false; }
            }
        }
        private void dd(object sender, EventArgs e)
        {
            rtab();
        }
        void changing(object sender, EventArgs e)
        {

            label1.Text = gwb.StatusText;
            if (isChinese == true)
            {
                label1.Text = label1.Text.Replace("Waiting for", "正在等待");
                label1.Text = label1.Text.Replace("Connecting to", "正在连接");
                label1.Text = label1.Text.Replace("Transferring data from", "正在加载");
            }
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
        public static void log(string text)
        {
            wf("log.txt", rf("log.txt") + Environment.NewLine + DateTime.Now.ToString() + "    " + text);
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
                //label1.Text = "Ready";
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
                

            }
            catch { }

        }
        void navIE(object sender, WebBrowserNavigatedEventArgs e)
        {
            textBox1.Text = Convert.ToString(((WebBrowser)sender).Url);
            string text = Convert.ToString(swb.Url);
            //System.IO.File.AppendAllText(@"ac.xml", "<url>" + text + "</url>");
            textBox1.AutoCompleteCustomSource.Add(Convert.ToString(gwb.Url));
            //label1.Text = "Ready";
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
                //label1.Text = (Math.Floor(Math.Log10(e.CurrentProgress + 1) * 1000)).ToString() + "/" + (Math.Floor(Math.Log10(e.MaximumProgress + 1) * 1000)).ToString();
                //updateStatusText();
                progressBar1.Maximum = e.MaximumProgress;
                progressBar1.Value = e.CurrentProgress;
                if (e.CurrentProgress.ToString() == e.MaximumProgress.ToString() || e.CurrentProgress == 0)
                {
                    progressBar1.Visible = false;
                    button10.Visible = false;
                    button1.Enabled = true;
                    panel2.Visible = false;
                }
                else
                {
                    progressBar1.Visible = true;
                    button10.Visible = true;
                    button1.Enabled = false;
                    panel2.Visible = true;
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
                //updateTitle();
            }
            catch { }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "about:daedalus")
            {
                gwb.Document.Body.InnerHtml = gplv3;
                //nv("javascript:document.write('" + gplv3 + "')");
            }
            else if (textBox1.Text == "about:hack:edit")
            {
                nv("JavaScript:document.body.contentEditable='true'; document.designMode='on'; void 0");
            }
            else if (((Form)(this.tabControl1.SelectedForm)).Tag.ToString() != "A")
            {
                nv(textBox1.Text);
                gwb.Focus();
            }
            else
            {
                nv(textBox1.Text);
                swb.Focus();
            }

        }
        public bool abWorking = true;
        private void Form1_Load(object sender, EventArgs e)
        {
            toolTip1.Active = true;
            if (isChinese == false)
            {
                toolTip1.SetToolTip(this.button7, "Click here to enable WormHole, saving you up to 70% of bandwidth bills on 3G networks.");
                toolTip1.SetToolTip(this.button8, "Click here to enable Private Mode, which does not leave browsing history.");
            }
            else
            {
                toolTip1.SetToolTip(this.button7, "点击此处使用网络压缩，可省70% 3G流量费！");
                toolTip1.SetToolTip(this.button8, "点击此处打开无痕模式");
            }
            try
            {
                int moreThanOne = 0;
                Process[] processid = Process.GetProcessesByName("daedalus");
                foreach (Process dode in processid)
                {
                    moreThanOne++;
                }
                if (moreThanOne < 2)
                {
                    ProcessStartInfo deProcess = new ProcessStartInfo("abd.exe");
                    deProcess.WindowStyle = ProcessWindowStyle.Minimized;
                    Process.Start(deProcess);
                }
            }
            catch { MessageBox.Show("Error: corrupted/missing anti-blocking module. Upgrade module immediately \n 错误：丢失反封杀模块。请立即升级反封杀模块。"); abWorking = false; } foreach (Process clsProcess in Process.GetProcesses())
            {

                if (clsProcess.ProcessName.Contains("dwm"))
                {
                    //log("DWM service detected, enabling Aero");
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
            gwb.GoBack();
        }
        void nonFatalError()
        {
            //log("Non-fatal error");
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
            this.AcceptButton = go;
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
        {/*
            try
            {
                ((Form)(this.tabControl1.SelectedForm)).Text = gwb.DocumentTitle;
                if (gwb.DocumentTitle == "")
                {
                    ((Form)(this.tabControl1.SelectedForm)).Text = Convert.ToString(gwb.Url.DnsSafeHost);
                }
            }
            catch { }*/

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
            int moreThanOne = 0;
            Process[] processid = Process.GetProcessesByName("daedalus");
            foreach (Process dode in processid)
            {
                moreThanOne++;
            }
            if (moreThanOne < 2)
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
            }
            int i = 0;
            log("---Program Closed---");
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
            proc("exit.bat");
        }
        void updateTitle()
        {
            //log("Called updateTitle() function");
            if (isChinese == false)
            {
                if (gwb.DocumentTitle != "")
                {
                    this.Text = gwb.DocumentTitle + " - Daedalus " + versionNumber;
                }
                else
                {
                    this.Text = gwb.Url.ToString() + " - Daedalus " + versionNumber;
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
                log("Anti-blocking initialized; mode: aggressive");
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
                log("Anti-blocking initialized; mode: conservative");
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
                        //button10.Enabled = false;
                        enableAntiblockingToolStripMenuItem1.Text = "停止反封杀";
                    }
                    else
                    {
                        fgab();
                        //button10.Enabled = false;
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
            //x.Visible = false;
            gwb.Print();

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
            e.Points[4] = new Point(e.TabWidth - 7, 0);
        }

        private void caretBrowsingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }



        private void textBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.FlatStyle == FlatStyle.Flat)
            {
                setProxy();
                button7.BackColor = System.Drawing.Color.White;
                button7.FlatStyle = FlatStyle.Standard;
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
                if (gwb.IsPhish() == true)
                {
                    textBox1.BackColor = System.Drawing.Color.Red;
                    //buttonX2.Visible = true;
                    //phishLock = true;
                    //warnPhish();
                }
                else
                {
                    textBox1.BackColor = Color.White;
                }
                if (((Form)(this.tabControl1.SelectedForm)).Tag.ToString() != "A")
                {
                    if (gwb.Url.Port == 443)
                    {
                        textBox1.BackColor = Color.PaleGreen;
                    }
                    else
                    {
                        if (i == true)
                        {
                            Process dahProcess = new
                                Process();
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
            }
            catch { }
        }
        void warnPhish()
        {

            MessageBox.Show("Phishing site detected. \n 检测出钓鱼网站！", "Warning!");
            log("Phishing site detected at: " + gwb.Url);
            textBox1.BackColor = System.Drawing.Color.Red;
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
            if (isPrivacyMode)
            {
                isPrivacyMode = false;
                button8.BackColor = Color.White;
            }
            else
            {
                isPrivacyMode = true;
                button8.BackColor = Color.Green;
            }
        }

        private void deleteBrowsingHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool abort = false;
            log("Browsing history cleared");
            const string messagee =
        "Are you sure you want to delete the browsing history? \r 真的要删除浏览纪录吗？";
            const string captione = "Deleting history...";
            var resulte = MessageBox.Show(messagee, captione, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (resulte == DialogResult.No)
            {
                abort = true;
            }
            if (isChinese != true && abort == false)
            {

                const string message =
            "Would you want to also delete cookies?";
                const string caption = "Deleting history...";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    gwb.DeleteCookies();
                }
                const string message2 =
            "Would you want to also clear the log?";
                const string caption2 = "Deleting history...";
                var result2 = MessageBox.Show(message2, caption2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result2 == DialogResult.Yes)
                {
                    wf("log.txt", "");
                }
            }
            else if (abort == false)
            {
                const string message = "您是否也想删除Cookies？";
                const string caption = "";
                var result = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    gwb.DeleteCookies();
                }
                const string message2 =
            "您是否也想清空系统日志？?";
                const string caption2 = "Deleting history...";
                var result2 = MessageBox.Show(message2, caption2, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result2 == DialogResult.Yes)
                {
                    wf("log.txt", "");
                }
            }
            if (abort == false)
            {
                TextWriter rpHistory = new StreamWriter("ac.xml");
                rpHistory.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?><i></i>");
                rpHistory.Close();
                textBox1.AutoCompleteCustomSource.Clear();
            }

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
        public string gplv3
        {

            get { return "<div style=\"font-family: Arial\"><h3 style=\"text-align: center; font-family: Arial\">GNU GENERAL PUBLIC LICENSE</h3><p style=\"text-align: center;\">Version 3, 29 June 2007</p><p>Copyright © 2007 Free Software Foundation, Inc. </p><p>Everyone is permitted to copy and distribute verbatim copies of this license document, but changing it is not allowed.</p><h3><a name=\"preamble\"></a>Preamble</h3><p>The GNU General Public License is a free, copyleft license for software and other kinds of works.</p><p>The licenses for most software and other practical works are designed to take away your freedom to share and change the works. By contrast, the GNU General Public License is intended to guarantee your freedom to share and change all versions of a program--to make sure it remains free software for all its users. We, the Free Software Foundation, use the GNU General Public License for most of our software; it applies also to any other work released this way by its authors. You can apply it to your programs, too.</p><p>When we speak of free software, we are referring to freedom, not price. Our General Public Licenses are designed to make sure that you have the freedom to distribute copies of free software (and charge for them if you wish), that you receive source code or can get it if you want it, that you can change the software or use pieces of it in new free programs, and that you know you can do these things.</p><p>To protect your rights, we need to prevent others from denying you these rights or asking you to surrender the rights. Therefore, you have certain responsibilities if you distribute copies of the software, or if you modify it: responsibilities to respect the freedom of others.</p><p>For example, if you distribute copies of such a program, whether gratis or for a fee, you must pass on to the recipients the same freedoms that you received. You must make sure that they, too, receive or can get the source code. And you must show them these terms so they know their rights.</p><p>Developers that use the GNU GPL protect your rights with two steps: (1) assert copyright on the software, and (2) offer you this License giving you legal permission to copy, distribute and/or modify it.</p><p>For the developers' and authors' protection, the GPL clearly explains that there is no warranty for this free software. For both users' and authors' sake, the GPL requires that modified versions be marked as changed, so that their problems will not be attributed erroneously to authors of previous versions.</p><p>Some devices are designed to deny users access to install or run modified versions of the software inside them, although the manufacturer can do so. This is fundamentally incompatible with the aim of protecting users' freedom to change the software. The systematic pattern of such abuse occurs in the area of products for individuals to use, which is precisely where it is most unacceptable. Therefore, we have designed this version of the GPL to prohibit the practice for those products. If such problems arise substantially in other domains, we stand ready to extend this provision to those domains in future versions of the GPL, as needed to protect the freedom of users.</p><p>Finally, every program is threatened constantly by software patents. States should not allow patents to restrict development and use of software on general-purpose computers, but in those that do, we wish to avoid the special danger that patents applied to a free program could make it effectively proprietary. To prevent this, the GPL assures that patents cannot be used to render the program non-free.</p><p>The precise terms and conditions for copying, distribution and modification follow.</p><h3><a name=\"terms\"></a>TERMS AND CONDITIONS</h3><h4><a name=\"section0\"></a>0. Definitions.</h4><p>&ldquo;This License&rdquo; refers to version 3 of the GNU General Public License.</p><p>&ldquo;Copyright&rdquo; also means copyright-like laws that apply to other kinds of works, such as semiconductor masks.</p><p>&ldquo;The Program&rdquo; refers to any copyrightable work licensed under this License. Each licensee is addressed as &ldquo;you&rdquo;. &ldquo;Licensees&rdquo; and &ldquo;recipients&rdquo; may be individuals or organizations.</p><p>To &ldquo;modify&rdquo; a work means to copy from or adapt all or part of the work in a fashion requiring copyright permission, other than the making of an exact copy. The resulting work is called a &ldquo;modified version&rdquo; of the earlier work or a work &ldquo;based on&rdquo; the earlier work.</p><p>A &ldquo;covered work&rdquo; means either the unmodified Program or a work based on the Program.</p><p>To &ldquo;propagate&rdquo; a work means to do anything with it that, without permission, would make you directly or secondarily liable for infringement under applicable copyright law, except executing it on a computer or modifying a private copy. Propagation includes copying, distribution (with or without modification), making available to the public, and in some countries other activities as well.</p><p>To &ldquo;convey&rdquo; a work means any kind of propagation that enables other parties to make or receive copies. Mere interaction with a user through a computer network, with no transfer of a copy, is not conveying.</p><p>An interactive user interface displays &ldquo;Appropriate Legal Notices&rdquo; to the extent that it includes a convenient and prominently visible feature that (1) displays an appropriate copyright notice, and (2) tells the user that there is no warranty for the work (except to the extent that warranties are provided), that licensees may convey the work under this License, and how to view a copy of this License. If the interface presents a list of user commands or options, such as a menu, a prominent item in the list meets this criterion.</p><h4><a name=\"section1\"></a>1. Source Code.</h4><p>The &ldquo;source code&rdquo; for a work means the preferred form of the work for making modifications to it. &ldquo;Object code&rdquo; means any non-source form of a work.</p><p>A &ldquo;Standard Interface&rdquo; means an interface that either is an official standard defined by a recognized standards body, or, in the case of interfaces specified for a particular programming language, one that is widely used among developers working in that language.</p><p>The &ldquo;System Libraries&rdquo; of an executable work include anything, other than the work as a whole, that (a) is included in the normal form of packaging a Major Component, but which is not part of that Major Component, and (b) serves only to enable use of the work with that Major Component, or to implement a Standard Interface for which an implementation is available to the public in source code form. A &ldquo;Major Component&rdquo;, in this context, means a major essential component (kernel, window system, and so on) of the specific operating system (if any) on which the executable work runs, or a compiler used to produce the work, or an object code interpreter used to run it.</p><p>The &ldquo;Corresponding Source&rdquo; for a work in object code form means all the source code needed to generate, install, and (for an executable work) run the object code and to modify the work, including scripts to control those activities. However, it does not include the work's System Libraries, or general-purpose tools or generally available free programs which are used unmodified in performing those activities but which are not part of the work. For example, Corresponding Source includes interface definition files associated with source files for the work, and the source code for shared libraries and dynamically linked subprograms that the work is specifically designed to require, such as by intimate data communication or control flow between those subprograms and other parts of the work.</p><p>The Corresponding Source need not include anything that users can regenerate automatically from other parts of the Corresponding Source.</p><p>The Corresponding Source for a work in source code form is that same work.</p><h4><a name=\"section2\"></a>2. Basic Permissions.</h4><p>All rights granted under this License are granted for the term of copyright on the Program, and are irrevocable provided the stated conditions are met. This License explicitly affirms your unlimited permission to run the unmodified Program. The output from running a covered work is covered by this License only if the output, given its content, constitutes a covered work. This License acknowledges your rights of fair use or other equivalent, as provided by copyright law.</p><p>You may make, run and propagate covered works that you do not convey, without conditions so long as your license otherwise remains in force. You may convey covered works to others for the sole purpose of having them make modifications exclusively for you, or provide you with facilities for running those works, provided that you comply with the terms of this License in conveying all material for which you do not control copyright. Those thus making or running the covered works for you must do so exclusively on your behalf, under your direction and control, on terms that prohibit them from making any copies of your copyrighted material outside their relationship with you.</p><p>Conveying under any other circumstances is permitted solely under the conditions stated below. Sublicensing is not allowed; section 10 makes it unnecessary.</p><h4><a name=\"section3\"></a>3. Protecting Users' Legal Rights From Anti-Circumvention Law.</h4><p>No covered work shall be deemed part of an effective technological measure under any applicable law fulfilling obligations under article 11 of the WIPO copyright treaty adopted on 20 December 1996, or similar laws prohibiting or restricting circumvention of such measures.</p><p>When you convey a covered work, you waive any legal power to forbid circumvention of technological measures to the extent such circumvention is effected by exercising rights under this License with respect to the covered work, and you disclaim any intention to limit operation or modification of the work as a means of enforcing, against the work's users, your or third parties' legal rights to forbid circumvention of technological measures.</p><h4><a name=\"section4\"></a>4. Conveying Verbatim Copies.</h4><p>You may convey verbatim copies of the Program's source code as you receive it, in any medium, provided that you conspicuously and appropriately publish on each copy an appropriate copyright notice; keep intact all notices stating that this License and any non-permissive terms added in accord with section 7 apply to the code; keep intact all notices of the absence of any warranty; and give all recipients a copy of this License along with the Program.</p><p>You may charge any price or no price for each copy that you convey, and you may offer support or warranty protection for a fee.</p><h4><a name=\"section5\"></a>5. Conveying Modified Source Versions.</h4><p>You may convey a work based on the Program, or the modifications to produce it from the Program, in the form of source code under the terms of section 4, provided that you also meet all of these conditions:</p><ul><li>a) The work must carry prominent notices stating that you modified it, and giving a relevant date.</li><li>b) The work must carry prominent notices stating that it is released under this License and any conditions added under section 7. This requirement modifies the requirement in section 4 to &ldquo;keep intact all notices&rdquo;.</li><li>c) You must license the entire work, as a whole, under this License to anyone who comes into possession of a copy. This License will therefore apply, along with any applicable section 7 additional terms, to the whole of the work, and all its parts, regardless of how they are packaged. This License gives no permission to license the work in any other way, but it does not invalidate such permission if you have separately received it.</li><li>d) If the work has interactive user interfaces, each must display Appropriate Legal Notices; however, if the Program has interactive interfaces that do not display Appropriate Legal Notices, your work need not make them do so.</li></ul><p>A compilation of a covered work with other separate and independent works, which are not by their nature extensions of the covered work, and which are not combined with it such as to form a larger program, in or on a volume of a storage or distribution medium, is called an &ldquo;aggregate&rdquo; if the compilation and its resulting copyright are not used to limit the access or legal rights of the compilation's users beyond what the individual works permit. Inclusion of a covered work in an aggregate does not cause this License to apply to the other parts of the aggregate.</p><h4><a name=\"section6\"></a>6. Conveying Non-Source Forms.</h4><p>You may convey a covered work in object code form under the terms of sections 4 and 5, provided that you also convey the machine-readable Corresponding Source under the terms of this License, in one of these ways:</p><ul><li>a) Convey the object code in, or embodied in, a physical product (including a physical distribution medium), accompanied by the Corresponding Source fixed on a durable physical medium customarily used for software interchange.</li><li>b) Convey the object code in, or embodied in, a physical product (including a physical distribution medium), accompanied by a written offer, valid for at least three years and valid for as long as you offer spare parts or customer support for that product model, to give anyone who possesses the object code either (1) a copy of the Corresponding Source for all the software in the product that is covered by this License, on a durable physical medium customarily used for software interchange, for a price no more than your reasonable cost of physically performing this conveying of source, or (2) access to copy the Corresponding Source from a network server at no charge.</li><li>c) Convey individual copies of the object code with a copy of the written offer to provide the Corresponding Source. This alternative is allowed only occasionally and noncommercially, and only if you received the object code with such an offer, in accord with subsection 6b.</li><li>d) Convey the object code by offering access from a designated place (gratis or for a charge), and offer equivalent access to the Corresponding Source in the same way through the same place at no further charge. You need not require recipients to copy the Corresponding Source along with the object code. If the place to copy the object code is a network server, the Corresponding Source may be on a different server (operated by you or a third party) that supports equivalent copying facilities, provided you maintain clear directions next to the object code saying where to find the Corresponding Source. Regardless of what server hosts the Corresponding Source, you remain obligated to ensure that it is available for as long as needed to satisfy these requirements.</li><li>e) Convey the object code using peer-to-peer transmission, provided you inform other peers where the object code and Corresponding Source of the work are being offered to the general public at no charge under subsection 6d.</li></ul><p>A separable portion of the object code, whose source code is excluded from the Corresponding Source as a System Library, need not be included in conveying the object code work.</p><p>A &ldquo;User Product&rdquo; is either (1) a &ldquo;consumer product&rdquo;, which means any tangible personal property which is normally used for personal, family, or household purposes, or (2) anything designed or sold for incorporation into a dwelling. In determining whether a product is a consumer product, doubtful cases shall be resolved in favor of coverage. For a particular product received by a particular user, &ldquo;normally used&rdquo; refers to a typical or common use of that class of product, regardless of the status of the particular user or of the way in which the particular user actually uses, or expects or is expected to use, the product. A product is a consumer product regardless of whether the product has substantial commercial, industrial or non-consumer uses, unless such uses represent the only significant mode of use of the product.</p><p>&ldquo;Installation Information&rdquo; for a User Product means any methods, procedures, authorization keys, or other information required to install and execute modified versions of a covered work in that User Product from a modified version of its Corresponding Source. The information must suffice to ensure that the continued functioning of the modified object code is in no case prevented or interfered with solely because modification has been made.</p><p>If you convey an object code work under this section in, or with, or specifically for use in, a User Product, and the conveying occurs as part of a transaction in which the right of possession and use of the User Product is transferred to the recipient in perpetuity or for a fixed term (regardless of how the transaction is characterized), the Corresponding Source conveyed under this section must be accompanied by the Installation Information. But this requirement does not apply if neither you nor any third party retains the ability to install modified object code on the User Product (for example, the work has been installed in ROM).</p><p>The requirement to provide Installation Information does not include a requirement to continue to provide support service, warranty, or updates for a work that has been modified or installed by the recipient, or for the User Product in which it has been modified or installed. Access to a network may be denied when the modification itself materially and adversely affects the operation of the network or violates the rules and protocols for communication across the network.</p><p>Corresponding Source conveyed, and Installation Information provided, in accord with this section must be in a format that is publicly documented (and with an implementation available to the public in source code form), and must require no special password or key for unpacking, reading or copying.</p><h4><a name=\"section7\"></a>7. Additional Terms.</h4><p>&ldquo;Additional permissions&rdquo; are terms that supplement the terms of this License by making exceptions from one or more of its conditions. Additional permissions that are applicable to the entire Program shall be treated as though they were included in this License, to the extent that they are valid under applicable law. If additional permissions apply only to part of the Program, that part may be used separately under those permissions, but the entire Program remains governed by this License without regard to the additional permissions.</p><p>When you convey a copy of a covered work, you may at your option remove any additional permissions from that copy, or from any part of it. (Additional permissions may be written to require their own removal in certain cases when you modify the work.) You may place additional permissions on material, added by you to a covered work, for which you have or can give appropriate copyright permission.</p><p>Notwithstanding any other provision of this License, for material you add to a covered work, you may (if authorized by the copyright holders of that material) supplement the terms of this License with terms:</p><ul><li>a) Disclaiming warranty or limiting liability differently from the terms of sections 15 and 16 of this License; or</li><li>b) Requiring preservation of specified reasonable legal notices or author attributions in that material or in the Appropriate Legal Notices displayed by works containing it; or</li><li>c) Prohibiting misrepresentation of the origin of that material, or requiring that modified versions of such material be marked in reasonable ways as different from the original version; or</li><li>d) Limiting the use for publicity purposes of names of licensors or authors of the material; or</li><li>e) Declining to grant rights under trademark law for use of some trade names, trademarks, or service marks; or</li><li>f) Requiring indemnification of licensors and authors of that material by anyone who conveys the material (or modified versions of it) with contractual assumptions of liability to the recipient, for any liability that these contractual assumptions directly impose on those licensors and authors.</li></ul><p>All other non-permissive additional terms are considered &ldquo;further restrictions&rdquo; within the meaning of section 10. If the Program as you received it, or any part of it, contains a notice stating that it is governed by this License along with a term that is a further restriction, you may remove that term. If a license document contains a further restriction but permits relicensing or conveying under this License, you may add to a covered work material governed by the terms of that license document, provided that the further restriction does not survive such relicensing or conveying.</p><p>If you add terms to a covered work in accord with this section, you must place, in the relevant source files, a statement of the additional terms that apply to those files, or a notice indicating where to find the applicable terms.</p><p>Additional terms, permissive or non-permissive, may be stated in the form of a separately written license, or stated as exceptions; the above requirements apply either way.</p><h4><a name=\"section8\"></a>8. Termination.</h4><p>You may not propagate or modify a covered work except as expressly provided under this License. Any attempt otherwise to propagate or modify it is void, and will automatically terminate your rights under this License (including any patent licenses granted under the third paragraph of section 11).</p><p>However, if you cease all violation of this License, then your license from a particular copyright holder is reinstated (a) provisionally, unless and until the copyright holder explicitly and finally terminates your license, and (b) permanently, if the copyright holder fails to notify you of the violation by some reasonable means prior to 60 days after the cessation.</p><p>Moreover, your license from a particular copyright holder is reinstated permanently if the copyright holder notifies you of the violation by some reasonable means, this is the first time you have received notice of violation of this License (for any work) from that copyright holder, and you cure the violation prior to 30 days after your receipt of the notice.</p><p>Termination of your rights under this section does not terminate the licenses of parties who have received copies or rights from you under this License. If your rights have been terminated and not permanently reinstated, you do not qualify to receive new licenses for the same material under section 10.</p><h4><a name=\"section9\"></a>9. Acceptance Not Required for Having Copies.</h4><p>You are not required to accept this License in order to receive or run a copy of the Program. Ancillary propagation of a covered work occurring solely as a consequence of using peer-to-peer transmission to receive a copy likewise does not require acceptance. However, nothing other than this License grants you permission to propagate or modify any covered work. These actions infringe copyright if you do not accept this License. Therefore, by modifying or propagating a covered work, you indicate your acceptance of this License to do so.</p><h4><a name=\"section10\"></a>10. Automatic Licensing of Downstream Recipients.</h4><p>Each time you convey a covered work, the recipient automatically receives a license from the original licensors, to run, modify and propagate that work, subject to this License. You are not responsible for enforcing compliance by third parties with this License.</p><p>An &ldquo;entity transaction&rdquo; is a transaction transferring control of an organization, or substantially all assets of one, or subdividing an organization, or merging organizations. If propagation of a covered work results from an entity transaction, each party to that transaction who receives a copy of the work also receives whatever licenses to the work the party's predecessor in interest had or could give under the previous paragraph, plus a right to possession of the Corresponding Source of the work from the predecessor in interest, if the predecessor has it or can get it with reasonable efforts.</p><p>You may not impose any further restrictions on the exercise of the rights granted or affirmed under this License. For example, you may not impose a license fee, royalty, or other charge for exercise of rights granted under this License, and you may not initiate litigation (including a cross-claim or counterclaim in a lawsuit) alleging that any patent claim is infringed by making, using, selling, offering for sale, or importing the Program or any portion of it.</p><h4><a name=\"section11\"></a>11. Patents.</h4><p>A &ldquo;contributor&rdquo; is a copyright holder who authorizes use under this License of the Program or a work on which the Program is based. The work thus licensed is called the contributor's &ldquo;contributor version&rdquo;.</p><p>A contributor's &ldquo;essential patent claims&rdquo; are all patent claims owned or controlled by the contributor, whether already acquired or hereafter acquired, that would be infringed by some manner, permitted by this License, of making, using, or selling its contributor version, but do not include claims that would be infringed only as a consequence of further modification of the contributor version. For purposes of this definition, &ldquo;control&rdquo; includes the right to grant patent sublicenses in a manner consistent with the requirements of this License.</p><p>Each contributor grants you a non-exclusive, worldwide, royalty-free patent license under the contributor's essential patent claims, to make, use, sell, offer for sale, import and otherwise run, modify and propagate the contents of its contributor version.</p><p>In the following three paragraphs, a &ldquo;patent license&rdquo; is any express agreement or commitment, however denominated, not to enforce a patent (such as an express permission to practice a patent or covenant not to sue for patent infringement). To &ldquo;grant&rdquo; such a patent license to a party means to make such an agreement or commitment not to enforce a patent against the party.</p><p>If you convey a covered work, knowingly relying on a patent license, and the Corresponding Source of the work is not available for anyone to copy, free of charge and under the terms of this License, through a publicly available network server or other readily accessible means, then you must either (1) cause the Corresponding Source to be so available, or (2) arrange to deprive yourself of the benefit of the patent license for this particular work, or (3) arrange, in a manner consistent with the requirements of this License, to extend the patent license to downstream recipients. &ldquo;Knowingly relying&rdquo; means you have actual knowledge that, but for the patent license, your conveying the covered work in a country, or your recipient's use of the covered work in a country, would infringe one or more identifiable patents in that country that you have reason to believe are valid.</p><p>If, pursuant to or in connection with a single transaction or arrangement, you convey, or propagate by procuring conveyance of, a covered work, and grant a patent license to some of the parties receiving the covered work authorizing them to use, propagate, modify or convey a specific copy of the covered work, then the patent license you grant is automatically extended to all recipients of the covered work and works based on it.</p><p>A patent license is &ldquo;discriminatory&rdquo; if it does not include within the scope of its coverage, prohibits the exercise of, or is conditioned on the non-exercise of one or more of the rights that are specifically granted under this License. You may not convey a covered work if you are a party to an arrangement with a third party that is in the business of distributing software, under which you make payment to the third party based on the extent of your activity of conveying the work, and under which the third party grants, to any of the parties who would receive the covered work from you, a discriminatory patent license (a) in connection with copies of the covered work conveyed by you (or copies made from those copies), or (b) primarily for and in connection with specific products or compilations that contain the covered work, unless you entered into that arrangement, or that patent license was granted, prior to 28 March 2007.</p><p>Nothing in this License shall be construed as excluding or limiting any implied license or other defenses to infringement that may otherwise be available to you under applicable patent law.</p><h4><a name=\"section12\"></a>12. No Surrender of Others' Freedom.</h4><p>If conditions are imposed on you (whether by court order, agreement or otherwise) that contradict the conditions of this License, they do not excuse you from the conditions of this License. If you cannot convey a covered work so as to satisfy simultaneously your obligations under this License and any other pertinent obligations, then as a consequence you may not convey it at all. For example, if you agree to terms that obligate you to collect a royalty for further conveying from those to whom you convey the Program, the only way you could satisfy both those terms and this License would be to refrain entirely from conveying the Program.</p><h4><a name=\"section13\"></a>13. Use with the GNU Affero General Public License.</h4><p>Notwithstanding any other provision of this License, you have permission to link or combine any covered work with a work licensed under version 3 of the GNU Affero General Public License into a single combined work, and to convey the resulting work. The terms of this License will continue to apply to the part which is the covered work, but the special requirements of the GNU Affero General Public License, section 13, concerning interaction through a network will apply to the combination as such.</p><h4><a name=\"section14\"></a>14. Revised Versions of this License.</h4><p>The Free Software Foundation may publish revised and/or new versions of the GNU General Public License from time to time. Such new versions will be similar in spirit to the present version, but may differ in detail to address new problems or concerns.</p><p>Each version is given a distinguishing version number. If the Program specifies that a certain numbered version of the GNU General Public License &ldquo;or any later version&rdquo; applies to it, you have the option of following the terms and conditions either of that numbered version or of any later version published by the Free Software Foundation. If the Program does not specify a version number of the GNU General Public License, you may choose any version ever published by the Free Software Foundation.</p><p>If the Program specifies that a proxy can decide which future versions of the GNU General Public License can be used, that proxy's public statement of acceptance of a version permanently authorizes you to choose that version for the Program.</p><p>Later license versions may give you additional or different permissions. However, no additional obligations are imposed on any author or copyright holder as a result of your choosing to follow a later version.</p><h4><a name=\"section15\"></a>15. Disclaimer of Warranty.</h4><p>THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR OTHER PARTIES PROVIDE THE PROGRAM &ldquo;AS IS&rdquo; WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU. SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY SERVICING, REPAIR OR CORRECTION.</p><h4><a name=\"section16\"></a>16. Limitation of Liability.</h4><p>IN NO EVENT UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING WILL ANY COPYRIGHT HOLDER, OR ANY OTHER PARTY WHO MODIFIES AND/OR CONVEYS THE PROGRAM AS PERMITTED ABOVE, BE LIABLE TO YOU FOR DAMAGES, INCLUDING ANY GENERAL, SPECIAL, INCIDENTAL OR CONSEQUENTIAL DAMAGES ARISING OUT OF THE USE OR INABILITY TO USE THE PROGRAM (INCLUDING BUT NOT LIMITED TO LOSS OF DATA OR DATA BEING RENDERED INACCURATE OR LOSSES SUSTAINED BY YOU OR THIRD PARTIES OR A FAILURE OF THE PROGRAM TO OPERATE WITH ANY OTHER PROGRAMS), EVEN IF SUCH HOLDER OR OTHER PARTY HAS BEEN ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.</p><h4><a name=\"section17\"></a>17. Interpretation of Sections 15 and 16.</h4><p>If the disclaimer of warranty and limitation of liability provided above cannot be given local legal effect according to their terms, reviewing courts shall apply local law that most closely approximates an absolute waiver of all civil liability in connection with the Program, unless a warranty or assumption of liability accompanies a copy of the Program in return for a fee.</p>"; }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            addGeckoTab();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            toolStripSplitButton1.DropDown.Show(Cursor.Position);
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
                if (e.KeyValue == 13)
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
            MessageBox.Show("The FishPhish filter has detected a possible phish heuristically. \n AAS自行研制的FishPhish系统智能检出可能的钓鱼网站。\n The content does not seem to match with the website, " + gwb.Url.DnsSafeHost.ToString() + ".");
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
            swb.Navigate("http://www.pisoft.tk/ieshell.php");
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
        public string cacheString = null;
        private void openInNewTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string url = cacheString;
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

        private void dl_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gwb.Window.TextZoom += 0.1f;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try { gwb.Window.TextZoom -= 0.1f; }
            catch { }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try { gwb.Window.TextZoom += 0.1f; }
            catch { }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try { gwb.Window.TextZoom = 1; }
            catch { }
        }

        private void tabControl1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void copyLinkLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(cacheString);
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Form3 fma = new Form3();
                fma.Show();
            }
            catch { }
        }

        private void programLogToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void programLogToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                Form3 fma = new Form3();
                fma.Show();
            }
            catch { }
        }

        private void programLogToolStripMenuItem_Click_2(object sender, EventArgs e)
        {
            proc("log.txt");
        }

        private void button7_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void button7_MouseHover(object sender, EventArgs e)
        {

        }

        private void garbageCollectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void ppToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (((Form)(this.tabControl1.SelectedForm)).Tag.ToString() != "A")
            {
                gwb.Hack_PrintPreview();
            }
            else { swb.ShowPrintPreviewDialog(); }
        }


        private void fdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
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