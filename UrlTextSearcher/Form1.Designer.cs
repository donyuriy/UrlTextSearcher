
namespace UrlTextSearcher
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.BtnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.BtnStop = new System.Windows.Forms.Button();
            this.PanelUrl = new System.Windows.Forms.Panel();
            this.PanelThread = new System.Windows.Forms.Panel();
            this.comboBoxThreadCount = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PanelWord = new System.Windows.Forms.Panel();
            this.textBoxWord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.PanelDepth = new System.Windows.Forms.Panel();
            this.comboBoxUrlDepth = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PanelButtons = new System.Windows.Forms.Panel();
            this.textBoxMessageOut = new System.Windows.Forms.TextBox();
            this.PanelUrl.SuspendLayout();
            this.PanelThread.SuspendLayout();
            this.PanelWord.SuspendLayout();
            this.PanelDepth.SuspendLayout();
            this.PanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(3, 3);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(132, 33);
            this.BtnStart.TabIndex = 5;
            this.BtnStart.Text = "Start search";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Search URL";
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUrl.Location = new System.Drawing.Point(16, 31);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(739, 22);
            this.textBoxUrl.TabIndex = 1;
            this.textBoxUrl.Validated += new System.EventHandler(this.textBoxUrl_Validated);
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(3, 81);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(132, 33);
            this.BtnStop.TabIndex = 6;
            this.BtnStop.Text = "Stop search";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // PanelUrl
            // 
            this.PanelUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelUrl.Controls.Add(this.label1);
            this.PanelUrl.Controls.Add(this.textBoxUrl);
            this.PanelUrl.Location = new System.Drawing.Point(12, 12);
            this.PanelUrl.Name = "PanelUrl";
            this.PanelUrl.Size = new System.Drawing.Size(758, 71);
            this.PanelUrl.TabIndex = 1;
            // 
            // PanelThread
            // 
            this.PanelThread.Controls.Add(this.comboBoxThreadCount);
            this.PanelThread.Controls.Add(this.label2);
            this.PanelThread.Location = new System.Drawing.Point(12, 193);
            this.PanelThread.Name = "PanelThread";
            this.PanelThread.Size = new System.Drawing.Size(224, 89);
            this.PanelThread.TabIndex = 3;
            // 
            // comboBoxThreadCount
            // 
            this.comboBoxThreadCount.FormattingEnabled = true;
            this.comboBoxThreadCount.Location = new System.Drawing.Point(13, 52);
            this.comboBoxThreadCount.Name = "comboBoxThreadCount";
            this.comboBoxThreadCount.Size = new System.Drawing.Size(83, 24);
            this.comboBoxThreadCount.TabIndex = 3;
            this.comboBoxThreadCount.SelectedIndexChanged += new System.EventHandler(this.comboBoxThreadCount_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Thread count";
            // 
            // PanelWord
            // 
            this.PanelWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelWord.Controls.Add(this.textBoxWord);
            this.PanelWord.Controls.Add(this.label3);
            this.PanelWord.Location = new System.Drawing.Point(13, 89);
            this.PanelWord.Name = "PanelWord";
            this.PanelWord.Size = new System.Drawing.Size(757, 98);
            this.PanelWord.TabIndex = 2;
            // 
            // textBoxWord
            // 
            this.textBoxWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWord.Location = new System.Drawing.Point(15, 50);
            this.textBoxWord.Name = "textBoxWord";
            this.textBoxWord.Size = new System.Drawing.Size(739, 22);
            this.textBoxWord.TabIndex = 2;
            this.textBoxWord.Validated += new System.EventHandler(this.textBoxWord_Validated);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Word for search";
            // 
            // PanelDepth
            // 
            this.PanelDepth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelDepth.Controls.Add(this.comboBoxUrlDepth);
            this.PanelDepth.Controls.Add(this.label4);
            this.PanelDepth.Location = new System.Drawing.Point(508, 193);
            this.PanelDepth.Name = "PanelDepth";
            this.PanelDepth.Size = new System.Drawing.Size(262, 89);
            this.PanelDepth.TabIndex = 4;
            // 
            // comboBoxUrlDepth
            // 
            this.comboBoxUrlDepth.FormattingEnabled = true;
            this.comboBoxUrlDepth.Location = new System.Drawing.Point(6, 48);
            this.comboBoxUrlDepth.Name = "comboBoxUrlDepth";
            this.comboBoxUrlDepth.Size = new System.Drawing.Size(121, 24);
            this.comboBoxUrlDepth.TabIndex = 4;
            this.comboBoxUrlDepth.SelectedIndexChanged += new System.EventHandler(this.comboBoxUrlDepth_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "URL depth";
            // 
            // PanelButtons
            // 
            this.PanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelButtons.Controls.Add(this.BtnStart);
            this.PanelButtons.Controls.Add(this.BtnStop);
            this.PanelButtons.Location = new System.Drawing.Point(632, 424);
            this.PanelButtons.Name = "PanelButtons";
            this.PanelButtons.Size = new System.Drawing.Size(138, 117);
            this.PanelButtons.TabIndex = 9;
            // 
            // textBoxMessageOut
            // 
            this.textBoxMessageOut.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessageOut.Location = new System.Drawing.Point(12, 310);
            this.textBoxMessageOut.Multiline = true;
            this.textBoxMessageOut.Name = "textBoxMessageOut";
            this.textBoxMessageOut.Size = new System.Drawing.Size(614, 231);
            this.textBoxMessageOut.TabIndex = 10;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.textBoxMessageOut);
            this.Controls.Add(this.PanelButtons);
            this.Controls.Add(this.PanelDepth);
            this.Controls.Add(this.PanelWord);
            this.Controls.Add(this.PanelThread);
            this.Controls.Add(this.PanelUrl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "URL Text Searcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.PanelUrl.ResumeLayout(false);
            this.PanelUrl.PerformLayout();
            this.PanelThread.ResumeLayout(false);
            this.PanelThread.PerformLayout();
            this.PanelWord.ResumeLayout(false);
            this.PanelWord.PerformLayout();
            this.PanelDepth.ResumeLayout(false);
            this.PanelDepth.PerformLayout();
            this.PanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Panel PanelUrl;
        private System.Windows.Forms.Panel PanelThread;
        private System.Windows.Forms.ComboBox comboBoxThreadCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel PanelWord;
        private System.Windows.Forms.TextBox textBoxWord;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel PanelDepth;
        private System.Windows.Forms.ComboBox comboBoxUrlDepth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel PanelButtons;
        private System.Windows.Forms.TextBox textBoxMessageOut;
    }
}

