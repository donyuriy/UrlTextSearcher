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
        public FormMain()
        {
            InitializeComponent();
            _logger = new Logger();
            comboBoxUrlDepth.KeyPress += new KeyPressEventHandler(OnKeyPress);
            comboBoxThreadCount.KeyPress += new KeyPressEventHandler(OnKeyPress);
            formMain = this;
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
            if (InvokeRequired)
            {
                this.Invoke(new EnableDelegate(EnableTextBox), new object[] { enable });
                return;
            }
            textBoxMessageOut.Enabled = enable;
        }

        public static void AppendStaticTextBox(string value)
        {
            if (formMain != null)
                formMain.AppendTextBox(value);
        }

        private void AppendTextBox(string value) //to enable use textbox from another thread
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            textBoxMessageOut.AppendText(Environment.NewLine);
            textBoxMessageOut.AppendText(value);
            Application.DoEvents();
        }

        public void BtnStopPerformClick()
        {
            if (InvokeRequired && this != null)
            {
                this.Invoke(new Action(BtnStopPerformClick), new object[] { });
                return;
            }
            BtnStop.PerformClick();
            Application.DoEvents();
        }

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(600, 480);
            BtnStop.Enabled = false;
            textBoxMessageOut.ReadOnly = true;
            textBoxMessageOut.ScrollBars = ScrollBars.Vertical;

            byte CBvalueCount = 10;
            object[] comboBoxItemCollection = new object[CBvalueCount];
            for (byte i = 0; i < CBvalueCount; i++)
            {
                comboBoxItemCollection[i] = i + 1;
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
            textBoxMessageOut.Clear();
            //comboBoxUrlDepth.DropDownStyle = ComboBoxStyle.DropDownList;
            //comboBoxThreadCount.DropDownStyle = ComboBoxStyle.DropDownList;

            string _searchingUrl = textBoxUrl.Text;
            string _searchingWord = textBoxWord.Text;
            Int32.TryParse(comboBoxUrlDepth.SelectedItem.ToString(), out int _depthOfLinkDisplay);
            Int32.TryParse(comboBoxThreadCount.SelectedItem.ToString(), out int _treadCount);

            if (!ConnectionCheck.IsConnectedToInternet())
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
                }
                else if (!validator.ValidateSearchWord(_searchingWord))
                {
                    textBoxMessageOut.AppendText(Resource.InvalidSearchWordMessage);
                }
                else
                {
                    ThreadCreator tc = new ThreadCreator(_logger,_treadCount, _depthOfLinkDisplay, _searchingWord, _searchingUrl);
                    tc.Start();

                    Thread th1 = new Thread(() =>
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
                textBoxMessageOut.AppendText(Environment.NewLine);
                textBoxMessageOut.AppendText(ex.Message);
                BtnStop.PerformClick();
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
            (sender as TextBox).Text = "https://stackoverflow.com/questions/17902882/how-to-enter-a-default-value-when-a-textbox-is-empty";
        }

        private void textBoxWord_Validated(object sender, EventArgs e)
        {
            (sender as TextBox).Text = "google";
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
    }
}
