using System.Drawing;
using Bazger.Tools.App.Pages;
using Telerik.WinControls;
using Telerik.WinControls.RichTextEditor.UI;

namespace Bazger.Tools.App
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe = new Telerik.WinControls.RootRadElement();
            this.object_33836c11_b0e5_4111_89c3_e07e43b27c2c = new Telerik.WinControls.RootRadElement();
            this.copyrightLabel = new Telerik.WinControls.UI.RadLabel();
            this.clickerPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.clickerControl = new Bazger.Tools.App.Pages.ClickerControl();
            this.positionClickerPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.positionClickerControl = new Bazger.Tools.App.Pages.PositionClickerControl();
            this.downloaderPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.youTubeDownloaderControl = new Bazger.Tools.App.Pages.YouTubeDownloaderControl();
            this.toolControlsPager = new Telerik.WinControls.UI.RadPageView();
            this.visualStudio2012DarkTheme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.visualStudio2012LightTheme = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.logTxtBox = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).BeginInit();
            this.clickerPage.SuspendLayout();
            this.positionClickerPage.SuspendLayout();
            this.downloaderPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolControlsPager)).BeginInit();
            this.toolControlsPager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logTxtBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe
            // 
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe.Name = "object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe";
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe.StretchHorizontally = true;
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe.StretchVertically = true;
            // 
            // object_33836c11_b0e5_4111_89c3_e07e43b27c2c
            // 
            this.object_33836c11_b0e5_4111_89c3_e07e43b27c2c.Name = "object_33836c11_b0e5_4111_89c3_e07e43b27c2c";
            this.object_33836c11_b0e5_4111_89c3_e07e43b27c2c.StretchHorizontally = true;
            this.object_33836c11_b0e5_4111_89c3_e07e43b27c2c.StretchVertically = true;
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyrightLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.copyrightLabel.Location = new System.Drawing.Point(0, 437);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(146, 19);
            this.copyrightLabel.TabIndex = 46;
            this.copyrightLabel.Text = "© Vanya Zolotaryov 2015";
            // 
            // clickerPage
            // 
            this.clickerPage.Controls.Add(this.clickerControl);
            this.clickerPage.ItemSize = new System.Drawing.SizeF(165F, 24F);
            this.clickerPage.Location = new System.Drawing.Point(5, 30);
            this.clickerPage.Name = "clickerPage";
            this.clickerPage.Size = new System.Drawing.Size(600, 188);
            this.clickerPage.Text = "Clicker";
            // 
            // clickerControl
            // 
            this.clickerControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerControl.Location = new System.Drawing.Point(68, 13);
            this.clickerControl.Name = "clickerControl";
            this.clickerControl.ParentPage = this.clickerPage;
            this.clickerControl.Size = new System.Drawing.Size(465, 164);
            this.clickerControl.TabIndex = 0;
            // 
            // positionClickerPage
            // 
            this.positionClickerPage.Controls.Add(this.positionClickerControl);
            this.positionClickerPage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.positionClickerPage.ItemSize = new System.Drawing.SizeF(204F, 24F);
            this.positionClickerPage.Location = new System.Drawing.Point(5, 30);
            this.positionClickerPage.Name = "positionClickerPage";
            this.positionClickerPage.Size = new System.Drawing.Size(600, 188);
            this.positionClickerPage.Text = "Multiply Clicks";
            // 
            // positionClickerControl
            // 
            this.positionClickerControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.positionClickerControl.Location = new System.Drawing.Point(68, 13);
            this.positionClickerControl.Name = "positionClickerControl";
            this.positionClickerControl.ParentPage = this.positionClickerPage;
            this.positionClickerControl.Size = new System.Drawing.Size(465, 164);
            this.positionClickerControl.TabIndex = 0;
            // 
            // downloaderPage
            // 
            this.downloaderPage.Controls.Add(this.youTubeDownloaderControl);
            this.downloaderPage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloaderPage.ItemSize = new System.Drawing.SizeF(240F, 24F);
            this.downloaderPage.Location = new System.Drawing.Point(5, 30);
            this.downloaderPage.Name = "downloaderPage";
            this.downloaderPage.Size = new System.Drawing.Size(600, 188);
            this.downloaderPage.Text = "YouTube Downloader";
            // 
            // youTubeDownloaderControl
            // 
            this.youTubeDownloaderControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.youTubeDownloaderControl.Location = new System.Drawing.Point(-5, 0);
            this.youTubeDownloaderControl.Name = "youTubeDownloaderControl";
            this.youTubeDownloaderControl.ParentPage = this.downloaderPage;
            this.youTubeDownloaderControl.Size = new System.Drawing.Size(610, 188);
            this.youTubeDownloaderControl.TabIndex = 0;
            // 
            // toolControlsPager
            // 
            this.toolControlsPager.Controls.Add(this.clickerPage);
            this.toolControlsPager.Controls.Add(this.positionClickerPage);
            this.toolControlsPager.Controls.Add(this.downloaderPage);
            this.toolControlsPager.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolControlsPager.Location = new System.Drawing.Point(0, 0);
            this.toolControlsPager.Name = "toolControlsPager";
            this.toolControlsPager.SelectedPage = this.clickerPage;
            this.toolControlsPager.Size = new System.Drawing.Size(610, 223);
            this.toolControlsPager.TabIndex = 0;
            this.toolControlsPager.ThemeName = "VisualStudio2012Light";
            this.toolControlsPager.SelectedPageChanged += new System.EventHandler(this.toolControlsPager_SelectedPageChanged);
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.toolControlsPager.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.toolControlsPager.GetChildAt(0))).ItemAlignment = Telerik.WinControls.UI.StripViewItemAlignment.Center;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.toolControlsPager.GetChildAt(0))).ItemFitMode = ((Telerik.WinControls.UI.StripViewItemFitMode)((Telerik.WinControls.UI.StripViewItemFitMode.Shrink | Telerik.WinControls.UI.StripViewItemFitMode.Fill)));
            // 
            // logTxtBox
            // 
            this.logTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTxtBox.AutoScroll = true;
            this.logTxtBox.AutoSize = false;
            this.logTxtBox.Location = new System.Drawing.Point(0, 229);
            this.logTxtBox.Multiline = true;
            this.logTxtBox.Name = "logTxtBox";
            this.logTxtBox.ReadOnly = true;
            this.logTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTxtBox.Size = new System.Drawing.Size(610, 202);
            this.logTxtBox.TabIndex = 44;
            this.logTxtBox.ThemeName = "VisualStudio2012Light";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 455);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.logTxtBox);
            this.Controls.Add(this.toolControlsPager);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(473, 454);
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "BazgerTools";
            this.ThemeName = "VisualStudio2012Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).EndInit();
            this.clickerPage.ResumeLayout(false);
            this.positionClickerPage.ResumeLayout(false);
            this.downloaderPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolControlsPager)).EndInit();
            this.toolControlsPager.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logTxtBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.RootRadElement object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe;
        private Telerik.WinControls.RootRadElement object_33836c11_b0e5_4111_89c3_e07e43b27c2c;
        private Telerik.WinControls.UI.RadLabel copyrightLabel;
        private Telerik.WinControls.UI.RadPageViewPage clickerPage;
        private Telerik.WinControls.UI.RadPageViewPage positionClickerPage;
        private Telerik.WinControls.UI.RadPageView toolControlsPager;
        private ClickerControl clickerControl;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme;
        private PositionClickerControl positionClickerControl;
        private Telerik.WinControls.UI.RadPageViewPage downloaderPage;
        private YouTubeDownloaderControl youTubeDownloaderControl;
        private Telerik.WinControls.UI.RadTextBox logTxtBox;
    }
}