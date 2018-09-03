using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using UrlTextSearcher.Interfaces;

namespace UrlTextSearcher
{
    public partial class FormMain : Form
    {
        private static FormMain formMain = null;
        private delegate void EnableDelegate(bool enable);
        private ILogger _logger;
        private int comboBoxValueCount = 10;    //count of numbers for Combo Boxes (threads, URL depth)

        private System.Windows.Forms.Timer timer1;
        private DateTime _startTime = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;
        private bool _timerRunning = false;

        public FormMain()
        {
            InitializeComponent();
            _logger = new Logger();
            comboBoxUrlDepth.KeyPress += new KeyPressEventHandler(OnKeyPress);            
            comboBoxThreadCount.KeyPress += new KeyPressEventHandler(OnKeyPress);
            textBoxTimer.KeyPress += new KeyPressEventHandler(OnKeyPress);
            formMain = this;
            timer1 = new System.Windows.Forms.Timer();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
        }
        public static void EnableStaticTextBox(bool enable) //to enable use textbox from another class
        {
            if (formMain != null)
            {
                formMain.EnableTextBox(enable);
            }
        }
        private void EnableTextBox(bool enable)
        {
            if (textBoxMessageOut != null)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new EnableDelegate(EnableTextBox), new object[] { enable });
                    return;
                }
                textBoxMessageOut.Enabled = enable;
            }
        }

        public static void AppendStaticTextBox(string value)    //to enable use textbox from another thread
        {
            if (formMain != null)
                formMain.AppendTextBox(value);
        }

        private void AppendTextBox(string value)
        {
            if (textBoxMessageOut != null)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                    return;
                }
                textBoxMessageOut.AppendText(Environment.NewLine);
                textBoxMessageOut.AppendText(value);
            }           
            Application.DoEvents();
        }

        public void BtnStopPerformClick()   //enable Button Stop event form other thread
        {
            if (BtnStop != null)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action(BtnStopPerformClick), new object[] { });
                    return;
                }
                BtnStop.PerformClick();
            }            
            Application.DoEvents();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e) //disble input to comboboxes except array values
        {
            e.Handled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(600, 480);
            BtnStop.Enabled = false;
            textBoxMessageOut.ReadOnly = true;
            textBoxMessageOut.ScrollBars = ScrollBars.Vertical;
            
            object[] comboBoxItemCollection = new object[comboBoxValueCount];
            for (int i = 0; i < comboBoxValueCount; i++)
            {
                comboBoxItemCollection[i] = i;
            }
            comboBoxThreadCount.Items.AddRange(comboBoxItemCollection);
            comboBoxThreadCount.SelectedIndex = 1;
            comboBoxUrlDepth.Items.AddRange(comboBoxItemCollection);
            comboBoxUrlDepth.SelectedIndex = 0;
        }
        private void BtnStart_Click(object sender, EventArgs e) 
        {
            BtnStart.Enabled = false;
            BtnStop.Enabled = true;
            PanelUrl.Enabled = false;
            PanelWord.Enabled = false;
            PanelThread.Enabled = false;
            PanelDepth.Enabled = false;
            textBoxTimer.IsAccessible = true;
            textBoxMessageOut.Clear();

            string _searchingUrl = textBoxUrl.Text;
            string _searchingWord = textBoxWord.Text;
            Int32.TryParse(comboBoxUrlDepth.SelectedItem.ToString(), out int _depthOfLinkDisplay);
            Int32.TryParse(comboBoxThreadCount.SelectedItem.ToString(), out int _treadCount);

            if (!_timerRunning)
            {
                _startTime = DateTime.Now;
                _totalElapsedTime = _currentElapsedTime;
                timer1.Start();
                _timerRunning = true;
            }

            if (!ConnectionChecker.IsConnectedToInternet())       //check internet connection
            {
                textBoxMessageOut.AppendText(Resource.NoInternetConnectionMessage);
                BtnStop.PerformClick();
            }

            try
            {
                Validator validator = new Validator();
                if (!validator.ValidateUrl(_searchingUrl))
                {
                    textBoxMessageOut.AppendText(Resource.InvalidURLMessage);
                    BtnStopPerformClick();
                }
                else if (!validator.ValidateSearchWord(_searchingWord))
                {
                    textBoxMessageOut.AppendText(Resource.InvalidSearchWordMessage);
                    BtnStopPerformClick();
                }
                else
                {
                    ThreadCreator tc = new ThreadCreator(_logger,_treadCount, _depthOfLinkDisplay, _searchingWord, _searchingUrl);
                    tc.StartMultithreading();
                    

                    Thread th1 = new Thread(() =>               //checks while working threads are Alive 
                    {
                        while (ThreadCreator.ThreadList.Any(t => t.IsAlive))
                        {
                            Thread.Sleep(1000);
                        }
                        LogScanningAccomplished();
                    });
                    th1.Start();                    
                }
            }
            catch (Exception ex)
            {
                textBoxMessageOut.AppendText(ex.Message);
                BtnStopPerformClick();
            }

        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (ThreadCreator.ThreadList != null && ThreadCreator.ThreadList.Count > 0)
            {
                for (byte i = 0; i < ThreadCreator.ThreadList.Count; i++)
                {
                    if (ThreadCreator.ThreadList[i].IsAlive)
                    {
                        ThreadCreator.ThreadList[i].Abort();
                    }
                }
            }
            if(_timerRunning)
            {
                timer1.Stop();
                _timerRunning = false;
            }

            BtnStart.Enabled = true;
            BtnStop.Enabled = false;
            PanelUrl.Enabled = true;
            PanelWord.Enabled = true;
            PanelThread.Enabled = true;
            PanelDepth.Enabled = true;
        }

        private void comboBoxThreadCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            var CBValue = Convert.ToInt32((sender as ComboBox).Text);
        }

        private void comboBoxUrlDepth_SelectedIndexChanged(object sender, EventArgs e)
        {
            var CBValue = Convert.ToInt32((sender as ComboBox).Text);
        }

        private void textBoxUrl_Validated(object sender, EventArgs e)
        {
            
        }

        private void textBoxWord_Validated(object sender, EventArgs e)
        {
           
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ThreadCreator.ThreadList != null && ThreadCreator.ThreadList.Count > 0)
            {
                for (byte i = 0; i < ThreadCreator.ThreadList.Count; i++)
                {
                    ThreadCreator.ThreadList[i].Abort();
                }
            }
        }

        private void LogScanningAccomplished()
        {
            _logger.SearchAccomplished();
            BtnStopPerformClick();
        }
                
        private void timer1_Tick(object sender, EventArgs e)    //Timer to get visual information about searching time
        {
            var timeSinceStartTime = DateTime.Now - _startTime;
            timeSinceStartTime = new TimeSpan(timeSinceStartTime.Hours,
                                              timeSinceStartTime.Minutes,
                                              timeSinceStartTime.Seconds);
            
            textBoxTimer.Text = timeSinceStartTime.ToString();
        }
    }
}
