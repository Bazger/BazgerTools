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
            this.components = new System.ComponentModel.Container();
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
            this.VideoStageStatsGrid = new Telerik.WinControls.UI.RadGridView();
            this.VideoStageGrid = new Telerik.WinControls.UI.RadGridView();
            this.logPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.logTxtBox = new Telerik.WinControls.UI.RadTextBox();
            this.infoPager = new Telerik.WinControls.UI.RadPageView();
            this.logTxtBoxCntxMenu = new Telerik.WinControls.UI.RadContextMenu(this.components);
            this.clearLogs = new Telerik.WinControls.UI.RadMenuItem();
            this.clearAllLogs = new Telerik.WinControls.UI.RadMenuItem();
            this.cntxMenuManager = new Telerik.WinControls.UI.RadContextMenuManager();
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
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageStatsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageStatsGrid.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageGrid.MasterTemplate)).BeginInit();
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
            this.copyrightLabel.Location = new System.Drawing.Point(0, 549);
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
            this.clickerPage.Size = new System.Drawing.Size(604, 229);
            this.clickerPage.Text = "Clicker";
            // 
            // positionClickerPage
            // 
            this.positionClickerPage.Controls.Add(this.positionClickerControl);
            this.positionClickerPage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.positionClickerPage.ItemSize = new System.Drawing.SizeF(120F, 24F);
            this.positionClickerPage.Location = new System.Drawing.Point(204, 5);
            this.positionClickerPage.Name = "positionClickerPage";
            this.positionClickerPage.Size = new System.Drawing.Size(604, 229);
            this.positionClickerPage.Text = "Multiply Clicks";
            // 
            // downloaderPage
            // 
            this.downloaderPage.Controls.Add(this.youTubeDownloaderControl);
            this.downloaderPage.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.downloaderPage.ItemSize = new System.Drawing.SizeF(120F, 24F);
            this.downloaderPage.Location = new System.Drawing.Point(204, 5);
            this.downloaderPage.Name = "downloaderPage";
            this.downloaderPage.Size = new System.Drawing.Size(604, 229);
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
            this.toolControlsPager.Size = new System.Drawing.Size(813, 239);
            this.toolControlsPager.TabIndex = 0;
            this.toolControlsPager.ThemeName = "VisualStudio2012Light";
            this.toolControlsPager.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            this.toolControlsPager.SelectedPageChanged += new System.EventHandler(this.ToolControlsPager_SelectedPageChanged);
            // 
            // downloderVideosStagePage
            // 
            this.downloderVideosStagePage.Controls.Add(this.VideoStageStatsGrid);
            this.downloderVideosStagePage.Controls.Add(this.VideoStageGrid);
            this.downloderVideosStagePage.ItemSize = new System.Drawing.SizeF(77F, 24F);
            this.downloderVideosStagePage.Location = new System.Drawing.Point(204, 5);
            this.downloderVideosStagePage.Name = "downloderVideosStagePage";
            this.downloderVideosStagePage.Size = new System.Drawing.Size(604, 289);
            this.downloderVideosStagePage.Text = "Videos Stage";
            // 
            // videoStageStatsGrid
            // 
            this.VideoStageStatsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VideoStageStatsGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.VideoStageStatsGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.VideoStageStatsGrid.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.VideoStageStatsGrid.ForeColor = System.Drawing.Color.Black;
            this.VideoStageStatsGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.VideoStageStatsGrid.Location = new System.Drawing.Point(3, 3);
            // 
            // 
            // 
            this.VideoStageStatsGrid.MasterTemplate.AllowAddNewRow = false;
            this.VideoStageStatsGrid.MasterTemplate.AllowColumnReorder = false;
            this.VideoStageStatsGrid.MasterTemplate.AllowDeleteRow = false;
            this.VideoStageStatsGrid.MasterTemplate.AllowEditRow = false;
            this.VideoStageStatsGrid.MasterTemplate.AllowRowResize = false;
            this.VideoStageStatsGrid.MasterTemplate.AutoGenerateColumns = false;
            this.VideoStageStatsGrid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.HeaderText = "Downloading";
            gridViewTextBoxColumn1.Name = "downloading";
            gridViewTextBoxColumn1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn1.Width = 94;
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.HeaderText = "Converting";
            gridViewTextBoxColumn2.Name = "converting";
            gridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn2.Width = 82;
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.HeaderText = "Completed";
            gridViewTextBoxColumn3.Name = "completed";
            gridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn3.Width = 88;
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.HeaderText = "Errors";
            gridViewTextBoxColumn4.Name = "errors";
            gridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn4.Width = 74;
            gridViewTextBoxColumn5.EnableExpressionEditor = false;
            gridViewTextBoxColumn5.HeaderText = "Url Problem";
            gridViewTextBoxColumn5.Name = "url_problem";
            gridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn5.Width = 91;
            gridViewTextBoxColumn6.EnableExpressionEditor = false;
            gridViewTextBoxColumn6.HeaderText = "Waiting";
            gridViewTextBoxColumn6.Name = "waiting";
            gridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn6.Width = 77;
            gridViewTextBoxColumn7.EnableExpressionEditor = false;
            gridViewTextBoxColumn7.HeaderText = "Moving";
            gridViewTextBoxColumn7.Name = "moving";
            gridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn7.Width = 76;
            this.VideoStageStatsGrid.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7});
            this.VideoStageStatsGrid.MasterTemplate.EnableGrouping = false;
            this.VideoStageStatsGrid.MasterTemplate.EnableSorting = false;
            this.VideoStageStatsGrid.MasterTemplate.ShowRowHeaderColumn = false;
            this.VideoStageStatsGrid.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.VideoStageStatsGrid.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.VideoStageStatsGrid.Name = "VideoStageStatsGrid";
            this.VideoStageStatsGrid.ReadOnly = true;
            this.VideoStageStatsGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.VideoStageStatsGrid.ShowGroupPanel = false;
            this.VideoStageStatsGrid.Size = new System.Drawing.Size(599, 50);
            this.VideoStageStatsGrid.TabIndex = 41;
            this.VideoStageStatsGrid.ThemeName = "VisualStudio2012Light";
            // 
            // videoStageGrid
            // 
            this.VideoStageGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VideoStageGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.VideoStageGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.VideoStageGrid.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.VideoStageGrid.ForeColor = System.Drawing.Color.Black;
            this.VideoStageGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.VideoStageGrid.Location = new System.Drawing.Point(3, 59);
            // 
            // 
            // 
            this.VideoStageGrid.MasterTemplate.AllowAddNewRow = false;
            this.VideoStageGrid.MasterTemplate.AllowColumnReorder = false;
            this.VideoStageGrid.MasterTemplate.AllowDeleteRow = false;
            this.VideoStageGrid.MasterTemplate.AllowRowResize = false;
            this.VideoStageGrid.MasterTemplate.AutoGenerateColumns = false;
            this.VideoStageGrid.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewDecimalColumn1.DataType = typeof(int);
            gridViewDecimalColumn1.EnableExpressionEditor = false;
            gridViewDecimalColumn1.HeaderText = "Id";
            gridViewDecimalColumn1.Name = "id";
            gridViewDecimalColumn1.ReadOnly = true;
            gridViewDecimalColumn1.Width = 38;
            gridViewHyperlinkColumn1.EnableExpressionEditor = false;
            gridViewHyperlinkColumn1.HeaderText = "Url";
            gridViewHyperlinkColumn1.Name = "url";
            gridViewHyperlinkColumn1.Width = 116;
            gridViewTextBoxColumn8.EnableExpressionEditor = false;
            gridViewTextBoxColumn8.HeaderText = "Title";
            gridViewTextBoxColumn8.Name = "title";
            gridViewTextBoxColumn8.ReadOnly = true;
            gridViewTextBoxColumn8.Width = 187;
            gridViewComboBoxColumn1.EnableExpressionEditor = false;
            gridViewComboBoxColumn1.HeaderText = "Video types";
            gridViewComboBoxColumn1.Name = "video_types";
            gridViewComboBoxColumn1.ReadOnly = true;
            gridViewComboBoxColumn1.Width = 98;
            gridViewDecimalColumn2.EnableExpressionEditor = false;
            gridViewDecimalColumn2.HeaderText = "Progress";
            gridViewDecimalColumn2.Name = "progress";
            gridViewDecimalColumn2.ReadOnly = true;
            gridViewDecimalColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewDecimalColumn2.Width = 58;
            gridViewTextBoxColumn9.EnableExpressionEditor = false;
            gridViewTextBoxColumn9.HeaderText = "Stage";
            gridViewTextBoxColumn9.Name = "stage";
            gridViewTextBoxColumn9.ReadOnly = true;
            gridViewTextBoxColumn9.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn9.Width = 85;
            this.VideoStageGrid.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewDecimalColumn1,
            gridViewHyperlinkColumn1,
            gridViewTextBoxColumn8,
            gridViewComboBoxColumn1,
            gridViewDecimalColumn2,
            gridViewTextBoxColumn9});
            this.VideoStageGrid.MasterTemplate.EnableGrouping = false;
            this.VideoStageGrid.MasterTemplate.EnableSorting = false;
            this.VideoStageGrid.MasterTemplate.ShowRowHeaderColumn = false;
            this.VideoStageGrid.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.VideoStageGrid.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.VideoStageGrid.Name = "VideoStageGrid";
            this.VideoStageGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.VideoStageGrid.ShowGroupPanel = false;
            this.VideoStageGrid.Size = new System.Drawing.Size(599, 230);
            this.VideoStageGrid.TabIndex = 39;
            this.VideoStageGrid.ThemeName = "VisualStudio2012Light";
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.logTxtBox);
            this.logPage.ItemSize = new System.Drawing.SizeF(77F, 24F);
            this.logPage.Location = new System.Drawing.Point(204, 5);
            this.logPage.Name = "logPage";
            this.logPage.Size = new System.Drawing.Size(604, 289);
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
            this.logTxtBox.Size = new System.Drawing.Size(604, 286);
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
            this.infoPager.Size = new System.Drawing.Size(813, 299);
            this.infoPager.TabIndex = 49;
            this.infoPager.ThemeName = "VisualStudio2012Light";
            this.infoPager.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            // 
            // logTxtBoxCntxMenu
            // 
            this.logTxtBoxCntxMenu.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.clearLogs,
            this.clearAllLogs});
            // 
            // clearLogs
            // 
            this.clearLogs.Name = "clearLogs";
            this.clearLogs.Text = "Clear log";
            // 
            // clearAllLogs
            // 
            this.clearAllLogs.Name = "clearAllLogs";
            this.clearAllLogs.Text = "Clear all pages log";
            // 
            // clickerControl
            // 
            this.clickerControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerControl.Location = new System.Drawing.Point(65, 13);
            this.clickerControl.Name = "clickerControl";
            this.clickerControl.ParentPage = null;
            this.clickerControl.Size = new System.Drawing.Size(465, 164);
            this.clickerControl.TabIndex = 0;
            // 
            // positionClickerControl
            // 
            this.positionClickerControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.positionClickerControl.Location = new System.Drawing.Point(65, 13);
            this.positionClickerControl.Name = "positionClickerControl";
            this.positionClickerControl.ParentPage = null;
            this.positionClickerControl.Size = new System.Drawing.Size(465, 164);
            this.positionClickerControl.TabIndex = 0;
            // 
            // youTubeDownloaderControl
            // 
            this.youTubeDownloaderControl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.youTubeDownloaderControl.Location = new System.Drawing.Point(-3, 0);
            this.youTubeDownloaderControl.Name = "youTubeDownloaderControl";
            this.youTubeDownloaderControl.ParentPage = null;
            this.youTubeDownloaderControl.Size = new System.Drawing.Size(610, 233);
            this.youTubeDownloaderControl.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 567);
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
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageStatsGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageStatsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VideoStageGrid)).EndInit();
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
        public Telerik.WinControls.UI.RadGridView VideoStageStatsGrid;
        public Telerik.WinControls.UI.RadGridView VideoStageGrid;
        private Telerik.WinControls.UI.RadPageViewPage logPage;
        private Telerik.WinControls.UI.RadTextBox logTxtBox;
        private Telerik.WinControls.UI.RadPageView infoPager;
        private RadContextMenu logTxtBoxCntxMenu;
        private RadContextMenuManager cntxMenuManager;
        private RadMenuItem clearLogs;
        private RadMenuItem clearAllLogs;
    }
}