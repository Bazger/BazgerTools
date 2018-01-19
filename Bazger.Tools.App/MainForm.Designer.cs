using System.Windows.Forms;
using Bazger.Tools.App.Pages;
using Telerik.WinControls.UI;

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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn1 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewHyperlinkColumn gridViewHyperlinkColumn1 = new Telerik.WinControls.UI.GridViewHyperlinkColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn8 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn1 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewDecimalColumn gridViewDecimalColumn2 = new Telerik.WinControls.UI.GridViewDecimalColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn9 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.copyrightLabel = new Telerik.WinControls.UI.RadLabel();
            this.clickerPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.positionClickerPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.downloaderPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.toolControlsPager = new Telerik.WinControls.UI.RadPageView();
            this.visualStudio2012DarkTheme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.visualStudio2012LightTheme = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.downloderVideosStagePage = new Telerik.WinControls.UI.RadPageViewPage();
            this.videoStageStatsGrid = new Telerik.WinControls.UI.RadGridView();
            this.videoStageGrid = new Telerik.WinControls.UI.RadGridView();
            this.logPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.logTxtBox = new Telerik.WinControls.UI.RadTextBox();
            this.infoPager = new Telerik.WinControls.UI.RadPageView();
            this.clickerControl = new Bazger.Tools.App.Pages.ClickerControl();
            this.positionClickerControl = new Bazger.Tools.App.Pages.PositionClickerControl();
            this.youTubeDownloaderControl = new Bazger.Tools.App.Pages.YouTubeDownloaderControl();
            ((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).BeginInit();
            this.clickerPage.SuspendLayout();
            this.positionClickerPage.SuspendLayout();
            this.downloaderPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolControlsPager)).BeginInit();
            this.toolControlsPager.SuspendLayout();
            this.downloderVideosStagePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.videoStageStatsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStageStatsGrid.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStageGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStageGrid.MasterTemplate)).BeginInit();
            this.logPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logTxtBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPager)).BeginInit();
            this.infoPager.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyrightLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.copyrightLabel.Location = new System.Drawing.Point(0, 530);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(146, 19);
            this.copyrightLabel.TabIndex = 46;
            this.copyrightLabel.Text = "© Vanya Zolotaryov 2015";
            // 
            // clickerPage
            // 
            this.clickerPage.Controls.Add(this.clickerControl);
            this.clickerPage.ItemSize = new System.Drawing.SizeF(120F, 24F);
            this.clickerPage.Location = new System.Drawing.Point(204, 5);
            this.clickerPage.Name = "clickerPage";
            this.clickerPage.Size = new System.Drawing.Size(610, 229);
            this.clickerPage.Text = "Clicker";
            // 
            // positionClickerPage
            // 
            this.positionClickerPage.Controls.Add(this.positionClickerControl);
            this.positionClickerPage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.positionClickerPage.ItemSize = new System.Drawing.SizeF(120F, 24F);
            this.positionClickerPage.Location = new System.Drawing.Point(204, 5);
            this.positionClickerPage.Name = "positionClickerPage";
            this.positionClickerPage.Size = new System.Drawing.Size(610, 213);
            this.positionClickerPage.Text = "Multiply Clicks";
            // 
            // downloaderPage
            // 
            this.downloaderPage.Controls.Add(this.youTubeDownloaderControl);
            this.downloaderPage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloaderPage.ItemSize = new System.Drawing.SizeF(120F, 24F);
            this.downloaderPage.Location = new System.Drawing.Point(204, 5);
            this.downloaderPage.Name = "downloaderPage";
            this.downloaderPage.Size = new System.Drawing.Size(610, 229);
            this.downloaderPage.Text = "YouTube Downloader";
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
            this.toolControlsPager.Size = new System.Drawing.Size(819, 239);
            this.toolControlsPager.TabIndex = 0;
            this.toolControlsPager.ThemeName = "VisualStudio2012Light";
            this.toolControlsPager.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            this.toolControlsPager.SelectedPageChanged += new System.EventHandler(this.toolControlsPager_SelectedPageChanged);
            // 
            // downloderVideosStagePage
            // 
            this.downloderVideosStagePage.Controls.Add(this.videoStageStatsGrid);
            this.downloderVideosStagePage.Controls.Add(this.videoStageGrid);
            this.downloderVideosStagePage.ItemSize = new System.Drawing.SizeF(77F, 24F);
            this.downloderVideosStagePage.Location = new System.Drawing.Point(204, 5);
            this.downloderVideosStagePage.Name = "downloderVideosStagePage";
            this.downloderVideosStagePage.Size = new System.Drawing.Size(610, 270);
            this.downloderVideosStagePage.Text = "Videos Stage";
            // 
            // videoStageStatsGrid
            // 
            this.videoStageStatsGrid.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.videoStageStatsGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.videoStageStatsGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.videoStageStatsGrid.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.videoStageStatsGrid.ForeColor = System.Drawing.Color.Black;
            this.videoStageStatsGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.videoStageStatsGrid.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.videoStageStatsGrid.MasterTemplate.AllowAddNewRow = false;
            this.videoStageStatsGrid.MasterTemplate.AllowColumnReorder = false;
            this.videoStageStatsGrid.MasterTemplate.AllowDeleteRow = false;
            this.videoStageStatsGrid.MasterTemplate.AllowEditRow = false;
            this.videoStageStatsGrid.MasterTemplate.AllowRowResize = false;
            this.videoStageStatsGrid.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn1.AllowResize = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.HeaderText = "Downloading";
            gridViewTextBoxColumn1.Name = "downloading";
            gridViewTextBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn1.Width = 96;
            gridViewTextBoxColumn2.AllowResize = false;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.HeaderText = "Converting";
            gridViewTextBoxColumn2.Name = "converting";
            gridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn2.Width = 84;
            gridViewTextBoxColumn3.AllowResize = false;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.HeaderText = "Completed";
            gridViewTextBoxColumn3.Name = "completed";
            gridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn3.Width = 90;
            gridViewTextBoxColumn4.AllowResize = false;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.HeaderText = "Errors";
            gridViewTextBoxColumn4.Name = "errors";
            gridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn4.Width = 76;
            gridViewTextBoxColumn5.AllowResize = false;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.HeaderText = "Url Problem";
            gridViewTextBoxColumn5.Name = "url_problem";
            gridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn5.Width = 93;
            gridViewTextBoxColumn6.AllowResize = false;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.HeaderText = "Waiting";
            gridViewTextBoxColumn6.Name = "waiting";
            gridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn6.Width = 79;
            gridViewTextBoxColumn7.AllowResize = false;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.HeaderText = "Moving";
            gridViewTextBoxColumn7.Name = "moving";
            gridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn7.Width = 76;
            this.videoStageStatsGrid.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7});
            this.videoStageStatsGrid.MasterTemplate.EnableGrouping = false;
            this.videoStageStatsGrid.MasterTemplate.EnableSorting = false;
            this.videoStageStatsGrid.MasterTemplate.ShowRowHeaderColumn = false;
            this.videoStageStatsGrid.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.videoStageStatsGrid.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.videoStageStatsGrid.Name = "videoStageStatsGrid";
            this.videoStageStatsGrid.ReadOnly = true;
            this.videoStageStatsGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.videoStageStatsGrid.ShowGroupPanel = false;
            this.videoStageStatsGrid.Size = new System.Drawing.Size(605, 50);
            this.videoStageStatsGrid.TabIndex = 41;
            this.videoStageStatsGrid.ThemeName = "VisualStudio2012Light";
            // 
            // videoStageGrid
            // 
            this.videoStageGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoStageGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.videoStageGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.videoStageGrid.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.videoStageGrid.ForeColor = System.Drawing.Color.Black;
            this.videoStageGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.videoStageGrid.Location = new System.Drawing.Point(3, 59);
            // 
            // 
            // 
            this.videoStageGrid.MasterTemplate.AllowAddNewRow = false;
            this.videoStageGrid.MasterTemplate.AllowColumnReorder = false;
            this.videoStageGrid.MasterTemplate.AllowDeleteRow = false;
            this.videoStageGrid.MasterTemplate.AllowRowResize = false;
            this.videoStageGrid.MasterTemplate.AutoGenerateColumns = false;
            this.videoStageGrid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewDecimalColumn1.DataType = typeof(int);
            gridViewDecimalColumn1.EnableExpressionEditor = false;
            gridViewDecimalColumn1.HeaderText = "Id";
            gridViewDecimalColumn1.Name = "id";
            gridViewDecimalColumn1.Width = 38;
            gridViewDecimalColumn1.ReadOnly = true;
            gridViewHyperlinkColumn1.EnableExpressionEditor = false;
            gridViewHyperlinkColumn1.HeaderText = "Url";
            gridViewHyperlinkColumn1.Name = "url";
            gridViewHyperlinkColumn1.ReadOnly = true;
            gridViewHyperlinkColumn1.Width = 117;
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.HeaderText = "Title";
            gridViewTextBoxColumn8.ReadOnly = true;
            gridViewTextBoxColumn8.Name = "title";
            gridViewTextBoxColumn8.Width = 189;
            gridViewComboBoxColumn1.EnableExpressionEditor = false;
            gridViewComboBoxColumn1.HeaderText = "Video types";
            gridViewComboBoxColumn1.Name = "video_types";
            gridViewComboBoxColumn1.ReadOnly = false;
            gridViewComboBoxColumn1.Width = 99;
            gridViewDecimalColumn2.EnableExpressionEditor = false;
            gridViewDecimalColumn2.HeaderText = "Progress";
            gridViewDecimalColumn2.Name = "progress";
            gridViewDecimalColumn2.ReadOnly = true;
            gridViewDecimalColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewDecimalColumn2.Width = 59;
            gridViewTextBoxColumn9.EnableExpressionEditor = false;
            gridViewTextBoxColumn9.HeaderText = "Stage";
            gridViewTextBoxColumn9.ReadOnly = true;
            gridViewTextBoxColumn9.Name = "stage";
            gridViewTextBoxColumn9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn9.Width = 86;
            this.videoStageGrid.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewDecimalColumn1,
            gridViewHyperlinkColumn1,
            gridViewTextBoxColumn8,
            gridViewComboBoxColumn1,
            gridViewDecimalColumn2,
            gridViewTextBoxColumn9});
            this.videoStageGrid.MasterTemplate.EnableGrouping = false;
            this.videoStageGrid.MasterTemplate.EnableSorting = false;
            this.videoStageGrid.MasterTemplate.ShowRowHeaderColumn = false;
            this.videoStageGrid.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.videoStageGrid.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.videoStageGrid.Name = "videoStageGrid";
            this.videoStageGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.videoStageGrid.ShowGroupPanel = false;
            this.videoStageGrid.Size = new System.Drawing.Size(605, 211);
            this.videoStageGrid.TabIndex = 39;
            this.videoStageGrid.ThemeName = "VisualStudio2012Light";
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.logTxtBox);
            this.logPage.ItemSize = new System.Drawing.SizeF(77F, 24F);
            this.logPage.Location = new System.Drawing.Point(204, 5);
            this.logPage.Name = "logPage";
            this.logPage.Size = new System.Drawing.Size(610, 270);
            this.logPage.Text = "Log";
            // 
            // logTxtBox
            // 
            this.logTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTxtBox.AutoSize = false;
            this.logTxtBox.Location = new System.Drawing.Point(0, 1);
            this.logTxtBox.Multiline = true;
            this.logTxtBox.Name = "logTxtBox";
            this.logTxtBox.ReadOnly = true;
            this.logTxtBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTxtBox.Size = new System.Drawing.Size(610, 267);
            this.logTxtBox.TabIndex = 51;
            // 
            // infoPager
            // 
            this.infoPager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.infoPager.Controls.Add(this.logPage);
            this.infoPager.Controls.Add(this.downloderVideosStagePage);
            this.infoPager.Location = new System.Drawing.Point(0, 244);
            this.infoPager.Name = "infoPager";
            this.infoPager.SelectedPage = this.downloderVideosStagePage;
            this.infoPager.Size = new System.Drawing.Size(819, 280);
            this.infoPager.TabIndex = 49;
            this.infoPager.ThemeName = "VisualStudio2012Light";
            this.infoPager.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
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
            // positionClickerControl
            // 
            this.positionClickerControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.positionClickerControl.Location = new System.Drawing.Point(68, 13);
            this.positionClickerControl.Name = "positionClickerControl";
            this.positionClickerControl.ParentPage = this.positionClickerPage;
            this.positionClickerControl.Size = new System.Drawing.Size(465, 164);
            this.positionClickerControl.TabIndex = 0;
            // 
            // youTubeDownloaderControl
            // 
            this.youTubeDownloaderControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.youTubeDownloaderControl.Location = new System.Drawing.Point(0, 0);
            this.youTubeDownloaderControl.Name = "youTubeDownloaderControl";
            this.youTubeDownloaderControl.ParentPage = this.downloaderPage;
            this.youTubeDownloaderControl.Size = new System.Drawing.Size(610, 233);
            this.youTubeDownloaderControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 548);
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
            this.downloderVideosStagePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.videoStageStatsGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStageStatsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStageGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.videoStageGrid)).EndInit();
            this.logPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logTxtBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infoPager)).EndInit();
            this.infoPager.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private Telerik.WinControls.UI.RadPageViewPage downloderVideosStagePage;
        public Telerik.WinControls.UI.RadGridView videoStageStatsGrid;
        public Telerik.WinControls.UI.RadGridView videoStageGrid;
        private Telerik.WinControls.UI.RadPageViewPage logPage;
        private Telerik.WinControls.UI.RadTextBox logTxtBox;
        private Telerik.WinControls.UI.RadPageView infoPager;
    }
}