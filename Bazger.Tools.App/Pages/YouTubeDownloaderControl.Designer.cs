﻿using System.Windows.Forms;
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            this.visualStudio2012DarkTheme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.visualStudio2012LightTheme = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.urlTxtBox = new Telerik.WinControls.UI.RadTextBox();
            this.threadPnl = new Telerik.WinControls.UI.RadPanel();
            this.converterThreadsSpin = new Telerik.WinControls.UI.RadSpinEditor();
            this.converterThreadsLbl = new Telerik.WinControls.UI.RadLabel();
            this.threadsLbl = new Telerik.WinControls.UI.RadLabel();
            this.downloaderThreadsSpin = new Telerik.WinControls.UI.RadSpinEditor();
            this.convertionFormatsDropDownList = new Telerik.WinControls.UI.RadDropDownList();
            this.converterPnl = new Telerik.WinControls.UI.RadPanel();
            this.convertionEnabledChkBox = new Telerik.WinControls.UI.RadCheckBox();
            this.startBtn = new Telerik.WinControls.UI.RadButton();
            this.pathsPnl = new Telerik.WinControls.UI.RadPanel();
            this.overwriteFilesChkBox = new Telerik.WinControls.UI.RadCheckBox();
            this.journalFileLbl = new Telerik.WinControls.UI.RadLabel();
            this.journalFileDropDown = new Telerik.WinControls.UI.RadDropDownList();
            this.downloadsFolderDropDown = new Telerik.WinControls.UI.RadDropDownList();
            this.downloadsFolderLbl = new Telerik.WinControls.UI.RadLabel();
            this.browseDownloadsBtn = new Telerik.WinControls.UI.RadButton();
            this.readFromJournalChkBox = new Telerik.WinControls.UI.RadCheckBox();
            this.browseJournalBtn = new Telerik.WinControls.UI.RadButton();
            this.writeToJournalChkBox = new Telerik.WinControls.UI.RadCheckBox();
            this.goToFolderBtn = new Telerik.WinControls.UI.RadButton();
            this.downloadProgressBar = new Telerik.WinControls.UI.RadProgressBar();
            this.waitingBar = new Telerik.WinControls.UI.RadWaitingBar();
            ((System.ComponentModel.ISupportInitialize)(this.urlTxtBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadPnl)).BeginInit();
            this.threadPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloaderThreadsSpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertionFormatsDropDownList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterPnl)).BeginInit();
            this.converterPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.convertionEnabledChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsPnl)).BeginInit();
            this.pathsPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overwriteFilesChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileDropDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsFolderDropDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsFolderLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseDownloadsBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.readFromJournalChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseJournalBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.writeToJournalChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.goToFolderBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadProgressBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitingBar)).BeginInit();
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
            this.urlTxtBox.TextChanging += new Telerik.WinControls.TextChangingEventHandler(this.urlTxtBox_TextChanging);
            this.urlTxtBox.Enter += new System.EventHandler(this.urlTxtBox_Focus);
            this.urlTxtBox.Leave += new System.EventHandler(this.urlTxtBox_Leave);
            // 
            // threadPnl
            // 
            this.threadPnl.Controls.Add(this.converterThreadsSpin);
            this.threadPnl.Controls.Add(this.converterThreadsLbl);
            this.threadPnl.Controls.Add(this.threadsLbl);
            this.threadPnl.Controls.Add(this.downloaderThreadsSpin);
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
            this.converterThreadsSpin.NullableValue = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.converterThreadsSpin.Size = new System.Drawing.Size(50, 24);
            this.converterThreadsSpin.TabIndex = 35;
            this.converterThreadsSpin.TabStop = false;
            this.converterThreadsSpin.ThemeName = "VisualStudio2012Light";
            this.converterThreadsSpin.Value = new decimal(new int[] {
            4,
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
            // downloaderThreadsSpin
            // 
            this.downloaderThreadsSpin.Location = new System.Drawing.Point(136, 3);
            this.downloaderThreadsSpin.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.downloaderThreadsSpin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.downloaderThreadsSpin.Name = "downloaderThreadsSpin";
            this.downloaderThreadsSpin.NullableValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.downloaderThreadsSpin.Size = new System.Drawing.Size(50, 24);
            this.downloaderThreadsSpin.TabIndex = 30;
            this.downloaderThreadsSpin.TabStop = false;
            this.downloaderThreadsSpin.ThemeName = "VisualStudio2012Light";
            this.downloaderThreadsSpin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // convertionFormatsDropDownList
            // 
            this.convertionFormatsDropDownList.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            radListDataItem1.Selected = true;
            radListDataItem1.Text = ".wav";
            radListDataItem2.Text = ".mp3";
            this.convertionFormatsDropDownList.Items.Add(radListDataItem1);
            this.convertionFormatsDropDownList.Items.Add(radListDataItem2);
            this.convertionFormatsDropDownList.Location = new System.Drawing.Point(3, 30);
            this.convertionFormatsDropDownList.Name = "convertionFormatsDropDownList";
            this.convertionFormatsDropDownList.Size = new System.Drawing.Size(183, 27);
            this.convertionFormatsDropDownList.TabIndex = 42;
            this.convertionFormatsDropDownList.Text = ".wav";
            this.convertionFormatsDropDownList.ThemeName = "VisualStudio2012Light";
            // 
            // converterPnl
            // 
            this.converterPnl.Controls.Add(this.convertionFormatsDropDownList);
            this.converterPnl.Controls.Add(this.convertionEnabledChkBox);
            this.converterPnl.Location = new System.Drawing.Point(4, 100);
            this.converterPnl.Name = "converterPnl";
            this.converterPnl.Size = new System.Drawing.Size(189, 68);
            this.converterPnl.TabIndex = 58;
            this.converterPnl.ThemeName = "VisualStudio2012Light";
            // 
            // convertionEnabledChkBox
            // 
            this.convertionEnabledChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.convertionEnabledChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.convertionEnabledChkBox.Location = new System.Drawing.Point(3, 3);
            this.convertionEnabledChkBox.Name = "convertionEnabledChkBox";
            this.convertionEnabledChkBox.Size = new System.Drawing.Size(140, 21);
            this.convertionEnabledChkBox.TabIndex = 41;
            this.convertionEnabledChkBox.Text = "Convertion Enabled";
            this.convertionEnabledChkBox.ThemeName = "VisualStudio2012Light";
            this.convertionEnabledChkBox.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // startBtn
            // 
            this.startBtn.Enabled = false;
            this.startBtn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startBtn.Location = new System.Drawing.Point(545, 174);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(62, 32);
            this.startBtn.TabIndex = 61;
            this.startBtn.Text = "Start";
            this.startBtn.ThemeName = "VisualStudio2012Light";
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // pathsPnl
            // 
            this.pathsPnl.Controls.Add(this.overwriteFilesChkBox);
            this.pathsPnl.Controls.Add(this.journalFileLbl);
            this.pathsPnl.Controls.Add(this.journalFileDropDown);
            this.pathsPnl.Controls.Add(this.downloadsFolderDropDown);
            this.pathsPnl.Controls.Add(this.downloadsFolderLbl);
            this.pathsPnl.Controls.Add(this.browseDownloadsBtn);
            this.pathsPnl.Controls.Add(this.readFromJournalChkBox);
            this.pathsPnl.Controls.Add(this.browseJournalBtn);
            this.pathsPnl.Controls.Add(this.writeToJournalChkBox);
            this.pathsPnl.Location = new System.Drawing.Point(196, 27);
            this.pathsPnl.Name = "pathsPnl";
            this.pathsPnl.Size = new System.Drawing.Size(411, 141);
            this.pathsPnl.TabIndex = 62;
            this.pathsPnl.ThemeName = "VisualStudio2012Light";
            // 
            // overwriteFilesChkBox
            // 
            this.overwriteFilesChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overwriteFilesChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.overwriteFilesChkBox.Location = new System.Drawing.Point(152, 73);
            this.overwriteFilesChkBox.Name = "overwriteFilesChkBox";
            this.overwriteFilesChkBox.Size = new System.Drawing.Size(132, 21);
            this.overwriteFilesChkBox.TabIndex = 70;
            this.overwriteFilesChkBox.Text = "Overwrite Enabled";
            this.overwriteFilesChkBox.ThemeName = "VisualStudio2012Light";
            this.overwriteFilesChkBox.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
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
            this.journalFileDropDown.Text = "Journal.json";
            this.journalFileDropDown.ThemeName = "VisualStudio2012Light";
            // 
            // downloadsFolderDropDown
            // 
            //this.downloadsFolderDropDown.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.downloadsFolderDropDown.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloadsFolderDropDown.Location = new System.Drawing.Point(3, 103);
            this.downloadsFolderDropDown.Name = "downloadsFolderDropDown";
            this.downloadsFolderDropDown.Size = new System.Drawing.Size(363, 31);
            this.downloadsFolderDropDown.TabIndex = 67;
            this.downloadsFolderDropDown.Text = "Downloads";
            this.downloadsFolderDropDown.ThemeName = "VisualStudio2012Light";
            this.downloadsFolderDropDown.TextChanged += new System.EventHandler(this.downloadsFolderDropDown_TextChanged);
            // 
            // downloadsFolderLbl
            // 
            this.downloadsFolderLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.downloadsFolderLbl.Location = new System.Drawing.Point(3, 76);
            this.downloadsFolderLbl.Name = "downloadsFolderLbl";
            this.downloadsFolderLbl.Size = new System.Drawing.Size(114, 21);
            this.downloadsFolderLbl.TabIndex = 66;
            this.downloadsFolderLbl.Text = "Downloads folder:";
            // 
            // browseDownloadsBtn
            // 
            this.browseDownloadsBtn.Location = new System.Drawing.Point(372, 103);
            this.browseDownloadsBtn.Name = "browseDownloadsBtn";
            this.browseDownloadsBtn.Size = new System.Drawing.Size(36, 27);
            this.browseDownloadsBtn.TabIndex = 65;
            this.browseDownloadsBtn.Text = "...";
            this.browseDownloadsBtn.ThemeName = "VisualStudio2012Light";
            this.browseDownloadsBtn.Click += new System.EventHandler(this.browseDownloadsBtn_Click);
            // 
            // readFromJournalChkBox
            // 
            this.readFromJournalChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.readFromJournalChkBox.Location = new System.Drawing.Point(276, 6);
            this.readFromJournalChkBox.Name = "readFromJournalChkBox";
            this.readFromJournalChkBox.Size = new System.Drawing.Size(132, 21);
            this.readFromJournalChkBox.TabIndex = 62;
            this.readFromJournalChkBox.Text = "Read from Journal";
            this.readFromJournalChkBox.ThemeName = "VisualStudio2012Light";
            // 
            // browseJournalBtn
            // 
            this.browseJournalBtn.Location = new System.Drawing.Point(372, 33);
            this.browseJournalBtn.Name = "browseJournalBtn";
            this.browseJournalBtn.Size = new System.Drawing.Size(36, 27);
            this.browseJournalBtn.TabIndex = 60;
            this.browseJournalBtn.Text = "...";
            this.browseJournalBtn.ThemeName = "VisualStudio2012Light";
            this.browseJournalBtn.Click += new System.EventHandler(this.browseJournalBtn_Click);
            // 
            // writeToJournalChkBox
            // 
            this.writeToJournalChkBox.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.writeToJournalChkBox.Location = new System.Drawing.Point(152, 6);
            this.writeToJournalChkBox.Name = "writeToJournalChkBox";
            this.writeToJournalChkBox.Size = new System.Drawing.Size(118, 21);
            this.writeToJournalChkBox.TabIndex = 61;
            this.writeToJournalChkBox.Text = "Write to Journal";
            this.writeToJournalChkBox.ThemeName = "VisualStudio2012Light";
            // 
            // goToFolderBtn
            // 
            this.goToFolderBtn.Location = new System.Drawing.Point(4, 174);
            this.goToFolderBtn.Name = "goToFolderBtn";
            this.goToFolderBtn.Size = new System.Drawing.Size(71, 32);
            this.goToFolderBtn.TabIndex = 72;
            this.goToFolderBtn.Text = "Downloads";
            this.goToFolderBtn.ThemeName = "VisualStudio2012Light";
            this.goToFolderBtn.Click += new System.EventHandler(this.goToFolderBtn_Click);
            // 
            // downloadProgressBar
            // 
            this.downloadProgressBar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloadProgressBar.Location = new System.Drawing.Point(81, 174);
            this.downloadProgressBar.Name = "downloadProgressBar";
            this.downloadProgressBar.SeparatorColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            this.downloadProgressBar.Size = new System.Drawing.Size(458, 32);
            this.downloadProgressBar.TabIndex = 60;
            this.downloadProgressBar.Text = "0%";
            this.downloadProgressBar.ThemeName = "VisualStudio2012Light";
            this.downloadProgressBar.Visible = false;
            // 
            // waitingBar
            // 
            this.waitingBar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.waitingBar.Location = new System.Drawing.Point(81, 174);
            this.waitingBar.Name = "waitingBar";
            this.waitingBar.ShowText = true;
            this.waitingBar.Size = new System.Drawing.Size(458, 32);
            this.waitingBar.TabIndex = 75;
            this.waitingBar.Text = "0%";
            this.waitingBar.ThemeName = "VisualStudio2012Light";
            this.waitingBar.WaitingIndicatorSize = new System.Drawing.Size(0, 100);
            this.waitingBar.WaitingSpeed = 100;
            this.waitingBar.WaitingStep = 4;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.waitingBar.GetChildAt(0))).WaitingIndicatorSize = new System.Drawing.Size(0, 100);
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.waitingBar.GetChildAt(0))).WaitingSpeed = 100;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.waitingBar.GetChildAt(0))).WaitingStep = 4;
            // 
            // YouTubeDownloaderControl
            // 
            this.Controls.Add(this.waitingBar);
            this.Controls.Add(this.goToFolderBtn);
            this.Controls.Add(this.pathsPnl);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.downloadProgressBar);
            this.Controls.Add(this.urlTxtBox);
            this.Controls.Add(this.threadPnl);
            this.Controls.Add(this.converterPnl);
            this.Name = "YouTubeDownloaderControl";
            this.Size = new System.Drawing.Size(610, 208);
            ((System.ComponentModel.ISupportInitialize)(this.urlTxtBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadPnl)).EndInit();
            this.threadPnl.ResumeLayout(false);
            this.threadPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterThreadsLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.threadsLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloaderThreadsSpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.convertionFormatsDropDownList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.converterPnl)).EndInit();
            this.converterPnl.ResumeLayout(false);
            this.converterPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.convertionEnabledChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathsPnl)).EndInit();
            this.pathsPnl.ResumeLayout(false);
            this.pathsPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.overwriteFilesChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.journalFileDropDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsFolderDropDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadsFolderLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseDownloadsBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.readFromJournalChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.browseJournalBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.writeToJournalChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.goToFolderBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downloadProgressBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitingBar)).EndInit();
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
        private Telerik.WinControls.UI.RadSpinEditor downloaderThreadsSpin;
        private Telerik.WinControls.UI.RadDropDownList convertionFormatsDropDownList;
        private Telerik.WinControls.UI.RadPanel converterPnl;
        private Telerik.WinControls.UI.RadCheckBox convertionEnabledChkBox;
        private Telerik.WinControls.UI.RadButton startBtn;
        private Telerik.WinControls.UI.RadPanel pathsPnl;
        private Telerik.WinControls.UI.RadLabel journalFileLbl;
        private Telerik.WinControls.UI.RadDropDownList journalFileDropDown;
        private Telerik.WinControls.UI.RadDropDownList downloadsFolderDropDown;
        private Telerik.WinControls.UI.RadLabel downloadsFolderLbl;
        private Telerik.WinControls.UI.RadButton browseDownloadsBtn;
        private Telerik.WinControls.UI.RadCheckBox readFromJournalChkBox;
        private Telerik.WinControls.UI.RadButton browseJournalBtn;
        private Telerik.WinControls.UI.RadCheckBox writeToJournalChkBox;
        private Telerik.WinControls.UI.RadCheckBox overwriteFilesChkBox;
        private Telerik.WinControls.UI.RadButton goToFolderBtn;
        private Telerik.WinControls.UI.RadProgressBar downloadProgressBar;
        private Telerik.WinControls.UI.RadWaitingBar waitingBar;
    }
}
