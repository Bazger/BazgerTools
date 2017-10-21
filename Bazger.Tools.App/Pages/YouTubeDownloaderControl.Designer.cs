using Bazger.Tools.App.Properties;

namespace Bazger.Tools.App.Pages
{
    partial class YouTubeDownloaderControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.visualStudio2012DarkTheme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.visualStudio2012LightTheme = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.urlTxtBox = new Telerik.WinControls.UI.RadTextBox();
            this.threadPnl = new Telerik.WinControls.UI.RadPanel();
            this.converterThreadsSpin = new Telerik.WinControls.UI.RadSpinEditor();
            this.converterThreadsLbl = new Telerik.WinControls.UI.RadLabel();
            this.threadsLbl = new Telerik.WinControls.UI.RadLabel();
            this.threadsSpin = new Telerik.WinControls.UI.RadSpinEditor();
            this.convertionFormatsDropDownList = new Telerik.WinControls.UI.RadDropDownList();
            this.converterPnl = new Telerik.WinControls.UI.RadPanel();
            this.convertionEnabledChkBox = new Telerik.WinControls.UI.RadCheckBox();
            this.downloadProgressBar = new Telerik.WinControls.UI.RadProgressBar();
            this.startBtn = new Telerik.WinControls.UI.RadButton();
            this.pathsPnl = new Telerik.WinControls.UI.RadPanel();
            this.journalFileLbl = new Telerik.WinControls.UI.RadLabel();
            this.journalFileDropDown = new Telerik.WinControls.UI.RadDropDownList();
            this.outputFolderDropDown = new Telerik.WinControls.UI.RadDropDownList();
            this.outputFolderLbl = new Telerik.WinControls.UI.RadLabel();
            this.browseOutputBtn = new Telerik.WinControls.UI.RadButton();
            this.readFromFileChkBox = new Telerik.WinControls.UI.RadCheckBox();
            this.browseJournalBtn = new Telerik.WinControls.UI.RadButton();
            this.writeToFileChkBox = new Telerik.WinControls.UI.RadCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.urlTxtBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadPnl)).BeginInit();
            this.threadPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertionFormatsDropDownList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterPnl)).BeginInit();
            this.converterPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.convertionEnabledChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsPnl)).BeginInit();
            this.pathsPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileDropDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputFolderDropDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputFolderLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseOutputBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.readFromFileChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseJournalBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.writeToFileChkBox)).BeginInit();
            this.SuspendLayout();
            // 
            // urlTxtBox
            // 
            this.urlTxtBox.Location = new System.Drawing.Point(4, 2);
            this.urlTxtBox.Name = "urlTxtBox";
            this.urlTxtBox.Size = new System.Drawing.Size(603, 24);
            this.urlTxtBox.TabIndex = 53;
            this.urlTxtBox.Text = "Enter video or playlist url";
            this.urlTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.urlTxtBox.ThemeName = "VisualStudio2012Light";
            this.urlTxtBox.Enter += new System.EventHandler(this.urlTxtBox_Focus);
            this.urlTxtBox.Leave += new System.EventHandler(this.urlTxtBox_Leave);
            // 
            // threadPnl
            // 
            this.threadPnl.Controls.Add(this.converterThreadsSpin);
            this.threadPnl.Controls.Add(this.converterThreadsLbl);
            this.threadPnl.Controls.Add(this.threadsLbl);
            this.threadPnl.Controls.Add(this.threadsSpin);
            this.threadPnl.Location = new System.Drawing.Point(4, 27);
            this.threadPnl.Name = "threadPnl";
            this.threadPnl.Size = new System.Drawing.Size(189, 67);
            this.threadPnl.TabIndex = 55;
            this.threadPnl.ThemeName = "VisualStudio2012Light";
            // 
            // converterThreadsSpin
            // 
            this.converterThreadsSpin.Location = new System.Drawing.Point(136, 33);
            this.converterThreadsSpin.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.converterThreadsSpin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.converterThreadsSpin.Name = "converterThreadsSpin";
            this.converterThreadsSpin.Size = new System.Drawing.Size(50, 24);
            this.converterThreadsSpin.TabIndex = 35;
            this.converterThreadsSpin.TabStop = false;
            this.converterThreadsSpin.ThemeName = "VisualStudio2012Light";
            this.converterThreadsSpin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // converterThreadsLbl
            // 
            this.converterThreadsLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.converterThreadsLbl.Location = new System.Drawing.Point(3, 33);
            this.converterThreadsLbl.Name = "converterThreadsLbl";
            this.converterThreadsLbl.Size = new System.Drawing.Size(116, 21);
            this.converterThreadsLbl.TabIndex = 34;
            this.converterThreadsLbl.Text = "Converter threads:";
            this.converterThreadsLbl.ThemeName = "VisualStudio2012Light";
            // 
            // threadsLbl
            // 
            this.threadsLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.threadsLbl.Location = new System.Drawing.Point(3, 4);
            this.threadsLbl.Name = "threadsLbl";
            this.threadsLbl.Size = new System.Drawing.Size(129, 21);
            this.threadsLbl.TabIndex = 33;
            this.threadsLbl.Text = "Downloader threads:";
            this.threadsLbl.ThemeName = "VisualStudio2012Light";
            // 
            // threadsSpin
            // 
            this.threadsSpin.Location = new System.Drawing.Point(136, 3);
            this.threadsSpin.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.threadsSpin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.threadsSpin.Name = "threadsSpin";
            this.threadsSpin.Size = new System.Drawing.Size(50, 24);
            this.threadsSpin.TabIndex = 30;
            this.threadsSpin.TabStop = false;
            this.threadsSpin.ThemeName = "VisualStudio2012Light";
            this.threadsSpin.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // convertionFormatsDropDownList
            // 
            this.convertionFormatsDropDownList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.convertionFormatsDropDownList.Location = new System.Drawing.Point(3, 41);
            this.convertionFormatsDropDownList.Name = "convertionFormatsDropDownList";
            this.convertionFormatsDropDownList.Size = new System.Drawing.Size(183, 27);
            this.convertionFormatsDropDownList.TabIndex = 42;
            this.convertionFormatsDropDownList.Text = "Convertion formats\r\n";
            this.convertionFormatsDropDownList.ThemeName = "VisualStudio2012Light";
            // 
            // converterPnl
            // 
            this.converterPnl.Controls.Add(this.convertionFormatsDropDownList);
            this.converterPnl.Controls.Add(this.convertionEnabledChkBox);
            this.converterPnl.Location = new System.Drawing.Point(4, 100);
            this.converterPnl.Name = "converterPnl";
            this.converterPnl.Size = new System.Drawing.Size(189, 84);
            this.converterPnl.TabIndex = 58;
            this.converterPnl.ThemeName = "VisualStudio2012Light";
            // 
            // convertionEnabledChkBox
            // 
            this.convertionEnabledChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.convertionEnabledChkBox.Location = new System.Drawing.Point(3, 14);
            this.convertionEnabledChkBox.Name = "convertionEnabledChkBox";
            this.convertionEnabledChkBox.Size = new System.Drawing.Size(140, 21);
            this.convertionEnabledChkBox.TabIndex = 41;
            this.convertionEnabledChkBox.Text = "Convertion Enabled";
            this.convertionEnabledChkBox.ThemeName = "VisualStudio2012Light";
            // 
            // downloadProgressBar
            // 
            this.downloadProgressBar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloadProgressBar.Location = new System.Drawing.Point(199, 152);
            this.downloadProgressBar.Name = "downloadProgressBar";
            this.downloadProgressBar.Size = new System.Drawing.Size(340, 32);
            this.downloadProgressBar.TabIndex = 60;
            this.downloadProgressBar.Text = "0%";
            this.downloadProgressBar.ThemeName = "VisualStudio2012Light";
            // 
            // startBtn
            // 
            this.startBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startBtn.Location = new System.Drawing.Point(545, 152);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(62, 32);
            this.startBtn.TabIndex = 61;
            this.startBtn.Text = "Start";
            this.startBtn.ThemeName = "VisualStudio2012Light";
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // pathsPnl
            // 
            this.pathsPnl.Controls.Add(this.journalFileLbl);
            this.pathsPnl.Controls.Add(this.journalFileDropDown);
            this.pathsPnl.Controls.Add(this.outputFolderDropDown);
            this.pathsPnl.Controls.Add(this.outputFolderLbl);
            this.pathsPnl.Controls.Add(this.browseOutputBtn);
            this.pathsPnl.Controls.Add(this.readFromFileChkBox);
            this.pathsPnl.Controls.Add(this.browseJournalBtn);
            this.pathsPnl.Controls.Add(this.writeToFileChkBox);
            this.pathsPnl.Location = new System.Drawing.Point(196, 27);
            this.pathsPnl.Name = "pathsPnl";
            this.pathsPnl.Size = new System.Drawing.Size(411, 119);
            this.pathsPnl.TabIndex = 62;
            this.pathsPnl.ThemeName = "VisualStudio2012Light";
            // 
            // journalFileLbl
            // 
            this.journalFileLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.journalFileLbl.Location = new System.Drawing.Point(3, 6);
            this.journalFileLbl.Name = "journalFileLbl";
            this.journalFileLbl.Size = new System.Drawing.Size(74, 21);
            this.journalFileLbl.TabIndex = 69;
            this.journalFileLbl.Text = "Journal file:";
            // 
            // journalFileDropDown
            // 
            this.journalFileDropDown.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.journalFileDropDown.Location = new System.Drawing.Point(3, 33);
            this.journalFileDropDown.Name = "journalFileDropDown";
            this.journalFileDropDown.Size = new System.Drawing.Size(363, 27);
            this.journalFileDropDown.TabIndex = 68;
            this.journalFileDropDown.ThemeName = "VisualStudio2012Light";
            // 
            // outputFolderDropDown
            // 
            this.outputFolderDropDown.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.outputFolderDropDown.Location = new System.Drawing.Point(3, 89);
            this.outputFolderDropDown.Name = "outputFolderDropDown";
            this.outputFolderDropDown.Size = new System.Drawing.Size(363, 27);
            this.outputFolderDropDown.TabIndex = 67;
            this.outputFolderDropDown.ThemeName = "VisualStudio2012Light";
            // 
            // outputFolderLbl
            // 
            this.outputFolderLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.outputFolderLbl.Location = new System.Drawing.Point(3, 66);
            this.outputFolderLbl.Name = "outputFolderLbl";
            this.outputFolderLbl.Size = new System.Drawing.Size(90, 21);
            this.outputFolderLbl.TabIndex = 66;
            this.outputFolderLbl.Text = "Output folder:";
            // 
            // browseOutputBtn
            // 
            this.browseOutputBtn.Location = new System.Drawing.Point(372, 89);
            this.browseOutputBtn.Name = "browseOutputBtn";
            this.browseOutputBtn.Size = new System.Drawing.Size(36, 27);
            this.browseOutputBtn.TabIndex = 65;
            this.browseOutputBtn.Text = "...";
            this.browseOutputBtn.ThemeName = "VisualStudio2012Light";
            // 
            // readFromFileChkBox
            // 
            this.readFromFileChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.readFromFileChkBox.Location = new System.Drawing.Point(276, 6);
            this.readFromFileChkBox.Name = "readFromFileChkBox";
            this.readFromFileChkBox.Size = new System.Drawing.Size(132, 21);
            this.readFromFileChkBox.TabIndex = 62;
            this.readFromFileChkBox.Text = "Read from Journal";
            this.readFromFileChkBox.ThemeName = "VisualStudio2012Light";
            // 
            // browseJournalBtn
            // 
            this.browseJournalBtn.Location = new System.Drawing.Point(372, 33);
            this.browseJournalBtn.Name = "browseJournalBtn";
            this.browseJournalBtn.Size = new System.Drawing.Size(36, 27);
            this.browseJournalBtn.TabIndex = 60;
            this.browseJournalBtn.Text = "...";
            this.browseJournalBtn.ThemeName = "VisualStudio2012Light";
            // 
            // writeToFileChkBox
            // 
            this.writeToFileChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.writeToFileChkBox.Location = new System.Drawing.Point(152, 6);
            this.writeToFileChkBox.Name = "writeToFileChkBox";
            this.writeToFileChkBox.Size = new System.Drawing.Size(118, 21);
            this.writeToFileChkBox.TabIndex = 61;
            this.writeToFileChkBox.Text = "Write to Journal";
            this.writeToFileChkBox.ThemeName = "VisualStudio2012Light";
            // 
            // YouTubeDownloaderControl
            // 
            this.Controls.Add(this.pathsPnl);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.downloadProgressBar);
            this.Controls.Add(this.urlTxtBox);
            this.Controls.Add(this.threadPnl);
            this.Controls.Add(this.converterPnl);
            this.Name = "YouTubeDownloaderControl";
            this.Size = new System.Drawing.Size(610, 187);
            ((System.ComponentModel.ISupportInitialize)(this.urlTxtBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadPnl)).EndInit();
            this.threadPnl.ResumeLayout(false);
            this.threadPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertionFormatsDropDownList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterPnl)).EndInit();
            this.converterPnl.ResumeLayout(false);
            this.converterPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.convertionEnabledChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsPnl)).EndInit();
            this.pathsPnl.ResumeLayout(false);
            this.pathsPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileDropDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputFolderDropDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.outputFolderLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseOutputBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.readFromFileChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseJournalBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.writeToFileChkBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme;
        private Telerik.WinControls.UI.RadTextBox urlTxtBox;
        private Telerik.WinControls.UI.RadPanel threadPnl;
        private Telerik.WinControls.UI.RadSpinEditor converterThreadsSpin;
        private Telerik.WinControls.UI.RadLabel converterThreadsLbl;
        private Telerik.WinControls.UI.RadLabel threadsLbl;
        private Telerik.WinControls.UI.RadSpinEditor threadsSpin;
        private Telerik.WinControls.UI.RadDropDownList convertionFormatsDropDownList;
        private Telerik.WinControls.UI.RadPanel converterPnl;
        private Telerik.WinControls.UI.RadCheckBox convertionEnabledChkBox;
        private Telerik.WinControls.UI.RadProgressBar downloadProgressBar;
        private Telerik.WinControls.UI.RadButton startBtn;
        private Telerik.WinControls.UI.RadPanel pathsPnl;
        private Telerik.WinControls.UI.RadLabel journalFileLbl;
        private Telerik.WinControls.UI.RadDropDownList journalFileDropDown;
        private Telerik.WinControls.UI.RadDropDownList outputFolderDropDown;
        private Telerik.WinControls.UI.RadLabel outputFolderLbl;
        private Telerik.WinControls.UI.RadButton browseOutputBtn;
        private Telerik.WinControls.UI.RadCheckBox readFromFileChkBox;
        private Telerik.WinControls.UI.RadButton browseJournalBtn;
        private Telerik.WinControls.UI.RadCheckBox writeToFileChkBox;
    }
}
