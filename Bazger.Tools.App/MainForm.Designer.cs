using System.Drawing;
using Bazger.Tools.App.Pages;
using Telerik.WinControls;

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
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewHyperlinkColumn gridViewHyperlinkColumn1 = new Telerik.WinControls.UI.GridViewHyperlinkColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn2 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
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
            this.infoPager = new Telerik.WinControls.UI.RadPageView();
            this.logPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.logTxtBox = new Telerik.WinControls.UI.RadTextBox();
            this.downloderVideosStagePage = new Telerik.WinControls.UI.RadPageViewPage();
            this.videosStageGrid = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).BeginInit();
            this.clickerPage.SuspendLayout();
            this.positionClickerPage.SuspendLayout();
            this.downloaderPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolControlsPager)).BeginInit();
            this.toolControlsPager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoPager)).BeginInit();
            this.infoPager.SuspendLayout();
            this.logPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logTxtBox)).BeginInit();
            this.downloderVideosStagePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videosStageGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videosStageGrid.MasterTemplate)).BeginInit();
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
            this.copyrightLabel.Location = new System.Drawing.Point(0, 502);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(146, 19);
            this.copyrightLabel.TabIndex = 46;
            this.copyrightLabel.Text = "© Vanya Zolotaryov 2015";
            // 
            // clickerPage
            // 
            this.clickerPage.Controls.Add(this.clickerControl);
            this.clickerPage.ItemSize = new System.Drawing.SizeF(146F, 29F);
            this.clickerPage.Location = new System.Drawing.Point(204, 5);
            this.clickerPage.Name = "clickerPage";
            this.clickerPage.Size = new System.Drawing.Size(685, 213);
            this.clickerPage.Text = "Clicker";
            // 
            // clickerControl
            // 
            this.clickerControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerControl.Location = new System.Drawing.Point(110, 13);
            this.clickerControl.Name = "clickerControl";
            this.clickerControl.ParentPage = this.clickerPage;
            this.clickerControl.Size = new System.Drawing.Size(465, 164);
            this.clickerControl.TabIndex = 0;
            // 
            // positionClickerPage
            // 
            this.positionClickerPage.Controls.Add(this.positionClickerControl);
            this.positionClickerPage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.positionClickerPage.ItemSize = new System.Drawing.SizeF(146F, 29F);
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
            this.downloaderPage.ItemSize = new System.Drawing.SizeF(146F, 29F);
            this.downloaderPage.Location = new System.Drawing.Point(204, 5);
            this.downloaderPage.Name = "downloaderPage";
            this.downloaderPage.Size = new System.Drawing.Size(685, 213);
            this.downloaderPage.Text = "YouTube Downloader";
            // 
            // youTubeDownloaderControl
            // 
            this.youTubeDownloaderControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.youTubeDownloaderControl.Location = new System.Drawing.Point(37, 0);
            this.youTubeDownloaderControl.Name = "youTubeDownloaderControl";
            this.youTubeDownloaderControl.ParentPage = this.downloaderPage;
            this.youTubeDownloaderControl.Size = new System.Drawing.Size(610, 210);
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
            this.toolControlsPager.SelectedPage = this.downloaderPage;
            this.toolControlsPager.Size = new System.Drawing.Size(894, 223);
            this.toolControlsPager.TabIndex = 0;
            this.toolControlsPager.ThemeName = "VisualStudio2012Light";
            this.toolControlsPager.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            this.toolControlsPager.SelectedPageChanged += new System.EventHandler(this.toolControlsPager_SelectedPageChanged);
            // 
            // infoPager
            // 
            this.infoPager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoPager.Controls.Add(this.logPage);
            this.infoPager.Controls.Add(this.downloderVideosStagePage);
            this.infoPager.Location = new System.Drawing.Point(0, 229);
            this.infoPager.Name = "infoPager";
            this.infoPager.SelectedPage = this.downloderVideosStagePage;
            this.infoPager.Size = new System.Drawing.Size(894, 267);
            this.infoPager.TabIndex = 49;
            this.infoPager.ThemeName = "VisualStudio2012Light";
            this.infoPager.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.logTxtBox);
            this.logPage.ItemSize = new System.Drawing.SizeF(94F, 29F);
            this.logPage.Location = new System.Drawing.Point(204, 5);
            this.logPage.Name = "logPage";
            this.logPage.Size = new System.Drawing.Size(685, 257);
            this.logPage.Text = "Log";
            // 
            // logTxtBox
            // 
            this.logTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTxtBox.AutoScroll = true;
            this.logTxtBox.AutoSize = false;
            this.logTxtBox.Location = new System.Drawing.Point(0, 1);
            this.logTxtBox.Multiline = true;
            this.logTxtBox.Name = "logTxtBox";
            this.logTxtBox.ReadOnly = true;
            this.logTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTxtBox.Size = new System.Drawing.Size(685, 254);
            this.logTxtBox.TabIndex = 51;
            this.logTxtBox.ThemeName = "VisualStudio2012Light";
            // 
            // downloderVideosStagePage
            // 
            this.downloderVideosStagePage.Controls.Add(this.videosStageGrid);
            this.downloderVideosStagePage.ItemSize = new System.Drawing.SizeF(94F, 29F);
            this.downloderVideosStagePage.Location = new System.Drawing.Point(204, 5);
            this.downloderVideosStagePage.Name = "downloderVideosStagePage";
            this.downloderVideosStagePage.Size = new System.Drawing.Size(685, 257);
            this.downloderVideosStagePage.Text = "Videos Stage";
            // 
            // videosStageGrid
            // 
            this.videosStageGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.videosStageGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.videosStageGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.videosStageGrid.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.videosStageGrid.ForeColor = System.Drawing.Color.Black;
            this.videosStageGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.videosStageGrid.Location = new System.Drawing.Point(40, 0);
            // 
            // 
            // 
            this.videosStageGrid.MasterTemplate.AllowAddNewRow = false;
            this.videosStageGrid.MasterTemplate.AllowColumnReorder = false;
            this.videosStageGrid.MasterTemplate.AllowRowResize = false;
            this.videosStageGrid.MasterTemplate.AutoGenerateColumns = false;
            gridViewDecimalColumn1.DataType = typeof(int);
            gridViewDecimalColumn1.EnableExpressionEditor = false;
            gridViewDecimalColumn1.HeaderText = "Id";
            gridViewDecimalColumn1.Name = "id";
            gridViewHyperlinkColumn1.EnableExpressionEditor = false;
            gridViewHyperlinkColumn1.HeaderText = "Url";
            gridViewHyperlinkColumn1.Name = "url";
            gridViewHyperlinkColumn1.Width = 302;
            gridViewDecimalColumn2.EnableExpressionEditor = false;
            gridViewDecimalColumn2.HeaderText = "Progress";
            gridViewDecimalColumn2.Name = "progress";
            gridViewDecimalColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewDecimalColumn2.Width = 110;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.HeaderText = "Stage";
            gridViewTextBoxColumn1.Name = "stage";
            gridViewTextBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn1.Width = 125;
            this.videosStageGrid.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewDecimalColumn1,
            gridViewHyperlinkColumn1,
            gridViewDecimalColumn2,
            gridViewTextBoxColumn1});
            this.videosStageGrid.MasterTemplate.EnableGrouping = false;
            this.videosStageGrid.MasterTemplate.EnableSorting = false;
            this.videosStageGrid.MasterTemplate.ShowRowHeaderColumn = false;
            this.videosStageGrid.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.videosStageGrid.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.videosStageGrid.Name = "videosStageGrid";
            this.videosStageGrid.ReadOnly = true;
            this.videosStageGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.videosStageGrid.ShowGroupPanel = false;
            this.videosStageGrid.Size = new System.Drawing.Size(605, 257);
            this.videosStageGrid.TabIndex = 39;
            this.videosStageGrid.ThemeName = "VisualStudio2012Light";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 520);
            this.Controls.Add(this.infoPager);
            this.Controls.Add(this.copyrightLabel);
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
            ((System.ComponentModel.ISupportInitialize)(this.infoPager)).EndInit();
            this.infoPager.ResumeLayout(false);
            this.logPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logTxtBox)).EndInit();
            this.downloderVideosStagePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videosStageGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videosStageGrid)).EndInit();
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
        private Telerik.WinControls.UI.RadPageView infoPager;
        private Telerik.WinControls.UI.RadPageViewPage logPage;
        private Telerik.WinControls.UI.RadTextBox logTxtBox;
        private Telerik.WinControls.UI.RadPageViewPage downloderVideosStagePage;
        public Telerik.WinControls.UI.RadGridView videosStageGrid;
    }
}