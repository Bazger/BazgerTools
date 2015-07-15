namespace BazgerTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.ThemeLight = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe = new Telerik.WinControls.RootRadElement();
            this.clickersPageView = new Telerik.WinControls.UI.RadPageView();
            this.ClickerPage = new Telerik.WinControls.UI.RadPageViewPage();
            this.clickerLog = new Telerik.WinControls.UI.RadTextBox();
            this.clickerSpinEditor = new Telerik.WinControls.UI.RadSpinEditor();
            this.startClickerButton = new Telerik.WinControls.UI.RadButton();
            this.delayClickerLabel = new System.Windows.Forms.Label();
            this.allCountLabel = new System.Windows.Forms.Label();
            this.countLabel = new System.Windows.Forms.Label();
            this.PositionClicker = new Telerik.WinControls.UI.RadPageViewPage();
            this.posClickerPanel = new System.Windows.Forms.Panel();
            this.moveDownButton = new Telerik.WinControls.UI.RadButton();
            this.removeButton = new Telerik.WinControls.UI.RadButton();
            this.moveUpButton = new Telerik.WinControls.UI.RadButton();
            this.clearAllButton = new Telerik.WinControls.UI.RadButton();
            this.delayPosClickerLabel = new System.Windows.Forms.Label();
            this.posClickerSpinEditor = new Telerik.WinControls.UI.RadSpinEditor();
            this.posClickerLog = new Telerik.WinControls.UI.RadTextBox();
            this.posGridView = new Telerik.WinControls.UI.RadGridView();
            this.circlesLabel = new System.Windows.Forms.Label();
            this.copyrightLabel = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.clickersPageView)).BeginInit();
            this.clickersPageView.SuspendLayout();
            this.ClickerPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clickerLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerSpinEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startClickerButton)).BeginInit();
            this.PositionClicker.SuspendLayout();
            this.posClickerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moveDownButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.removeButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveUpButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearAllButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posClickerSpinEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posClickerLog)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe
            // 
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe.Name = "object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe";
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe.StretchHorizontally = true;
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe.StretchVertically = true;
            this.object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // clickersPageView
            // 
            this.clickersPageView.Controls.Add(this.ClickerPage);
            this.clickersPageView.Controls.Add(this.PositionClicker);
            this.clickersPageView.Dock = System.Windows.Forms.DockStyle.Top;
            this.clickersPageView.Location = new System.Drawing.Point(0, 0);
            this.clickersPageView.Name = "clickersPageView";
            this.clickersPageView.SelectedPage = this.PositionClicker;
            this.clickersPageView.Size = new System.Drawing.Size(343, 394);
            this.clickersPageView.TabIndex = 0;
            this.clickersPageView.Text = "radPageView1";
            this.clickersPageView.ThemeName = "VisualStudio2012Light";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.clickersPageView.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.clickersPageView.GetChildAt(0))).ItemAlignment = Telerik.WinControls.UI.StripViewItemAlignment.Center;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.clickersPageView.GetChildAt(0))).ItemFitMode = ((Telerik.WinControls.UI.StripViewItemFitMode)((Telerik.WinControls.UI.StripViewItemFitMode.Shrink | Telerik.WinControls.UI.StripViewItemFitMode.Fill)));
            // 
            // ClickerPage
            // 
            this.ClickerPage.Controls.Add(this.clickerLog);
            this.ClickerPage.Controls.Add(this.clickerSpinEditor);
            this.ClickerPage.Controls.Add(this.startClickerButton);
            this.ClickerPage.Controls.Add(this.delayClickerLabel);
            this.ClickerPage.Controls.Add(this.allCountLabel);
            this.ClickerPage.Controls.Add(this.countLabel);
            this.ClickerPage.ItemSize = new System.Drawing.SizeF(152F, 24F);
            this.ClickerPage.Location = new System.Drawing.Point(5, 30);
            this.ClickerPage.Name = "ClickerPage";
            this.ClickerPage.Size = new System.Drawing.Size(333, 359);
            this.ClickerPage.Text = "Clciker";
            // 
            // clickerLog
            // 
            this.clickerLog.AutoScroll = true;
            this.clickerLog.AutoSize = false;
            this.clickerLog.Location = new System.Drawing.Point(7, 103);
            this.clickerLog.Modified = true;
            this.clickerLog.Multiline = true;
            this.clickerLog.Name = "clickerLog";
            this.clickerLog.ReadOnly = true;
            this.clickerLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.clickerLog.Size = new System.Drawing.Size(319, 253);
            this.clickerLog.TabIndex = 37;
            // 
            // clickerSpinEditor
            // 
            this.clickerSpinEditor.Location = new System.Drawing.Point(202, 77);
            this.clickerSpinEditor.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.clickerSpinEditor.Name = "clickerSpinEditor";
            this.clickerSpinEditor.Size = new System.Drawing.Size(124, 20);
            this.clickerSpinEditor.TabIndex = 25;
            this.clickerSpinEditor.TabStop = false;
            this.clickerSpinEditor.ThemeName = "VisualStudio2012Light";
            this.clickerSpinEditor.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // startClickerButton
            // 
            this.startClickerButton.Location = new System.Drawing.Point(7, 6);
            this.startClickerButton.Name = "startClickerButton";
            this.startClickerButton.Size = new System.Drawing.Size(176, 91);
            this.startClickerButton.TabIndex = 23;
            this.startClickerButton.Text = "Test Clicking!";
            this.startClickerButton.ThemeName = "VisualStudio2012Light";
            this.startClickerButton.Click += new System.EventHandler(this.startClickerButton_Click);
            // 
            // delayClickerLabel
            // 
            this.delayClickerLabel.AutoSize = true;
            this.delayClickerLabel.Location = new System.Drawing.Point(199, 61);
            this.delayClickerLabel.Name = "delayClickerLabel";
            this.delayClickerLabel.Size = new System.Drawing.Size(38, 13);
            this.delayClickerLabel.TabIndex = 19;
            this.delayClickerLabel.Text = "Delay:";
            // 
            // allCountLabel
            // 
            this.allCountLabel.AutoSize = true;
            this.allCountLabel.Location = new System.Drawing.Point(199, 28);
            this.allCountLabel.Name = "allCountLabel";
            this.allCountLabel.Size = new System.Drawing.Size(70, 13);
            this.allCountLabel.TabIndex = 17;
            this.allCountLabel.Text = "All Count:  0";
            // 
            // countLabel
            // 
            this.countLabel.AutoSize = true;
            this.countLabel.Location = new System.Drawing.Point(199, 6);
            this.countLabel.Name = "countLabel";
            this.countLabel.Size = new System.Drawing.Size(54, 13);
            this.countLabel.TabIndex = 16;
            this.countLabel.Text = "Count:  0";
            // 
            // PositionClicker
            // 
            this.PositionClicker.Controls.Add(this.posClickerPanel);
            this.PositionClicker.Controls.Add(this.posClickerLog);
            this.PositionClicker.Controls.Add(this.posGridView);
            this.PositionClicker.Controls.Add(this.circlesLabel);
            this.PositionClicker.ItemSize = new System.Drawing.SizeF(191F, 24F);
            this.PositionClicker.Location = new System.Drawing.Point(5, 30);
            this.PositionClicker.Name = "PositionClicker";
            this.PositionClicker.Size = new System.Drawing.Size(333, 359);
            this.PositionClicker.Text = "Multiply Clicks";
            // 
            // posClickerPanel
            // 
            this.posClickerPanel.Controls.Add(this.moveDownButton);
            this.posClickerPanel.Controls.Add(this.removeButton);
            this.posClickerPanel.Controls.Add(this.moveUpButton);
            this.posClickerPanel.Controls.Add(this.clearAllButton);
            this.posClickerPanel.Controls.Add(this.delayPosClickerLabel);
            this.posClickerPanel.Controls.Add(this.posClickerSpinEditor);
            this.posClickerPanel.Location = new System.Drawing.Point(0, 8);
            this.posClickerPanel.Name = "posClickerPanel";
            this.posClickerPanel.Size = new System.Drawing.Size(180, 123);
            this.posClickerPanel.TabIndex = 37;
            // 
            // moveDownButton
            // 
            this.moveDownButton.Location = new System.Drawing.Point(95, 87);
            this.moveDownButton.Name = "moveDownButton";
            this.moveDownButton.Size = new System.Drawing.Size(72, 30);
            this.moveDownButton.TabIndex = 34;
            this.moveDownButton.Text = "Move Down";
            this.moveDownButton.ThemeName = "VisualStudio2012Light";
            this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(7, 51);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(72, 30);
            this.removeButton.TabIndex = 31;
            this.removeButton.Text = "Remove";
            this.removeButton.ThemeName = "VisualStudio2012Light";
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // moveUpButton
            // 
            this.moveUpButton.Location = new System.Drawing.Point(95, 51);
            this.moveUpButton.Name = "moveUpButton";
            this.moveUpButton.Size = new System.Drawing.Size(72, 30);
            this.moveUpButton.TabIndex = 33;
            this.moveUpButton.Text = "Move Up";
            this.moveUpButton.ThemeName = "VisualStudio2012Light";
            this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // clearAllButton
            // 
            this.clearAllButton.Location = new System.Drawing.Point(7, 87);
            this.clearAllButton.Name = "clearAllButton";
            this.clearAllButton.Size = new System.Drawing.Size(72, 30);
            this.clearAllButton.TabIndex = 32;
            this.clearAllButton.Text = "Clear All";
            this.clearAllButton.ThemeName = "VisualStudio2012Light";
            this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click);
            // 
            // delayPosClickerLabel
            // 
            this.delayPosClickerLabel.AutoSize = true;
            this.delayPosClickerLabel.Location = new System.Drawing.Point(4, 9);
            this.delayPosClickerLabel.Name = "delayPosClickerLabel";
            this.delayPosClickerLabel.Size = new System.Drawing.Size(38, 13);
            this.delayPosClickerLabel.TabIndex = 24;
            this.delayPosClickerLabel.Text = "Delay:";
            // 
            // posClickerSpinEditor
            // 
            this.posClickerSpinEditor.Location = new System.Drawing.Point(7, 25);
            this.posClickerSpinEditor.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.posClickerSpinEditor.Name = "posClickerSpinEditor";
            this.posClickerSpinEditor.Size = new System.Drawing.Size(72, 20);
            this.posClickerSpinEditor.TabIndex = 29;
            this.posClickerSpinEditor.TabStop = false;
            this.posClickerSpinEditor.ThemeName = "VisualStudio2012Light";
            this.posClickerSpinEditor.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // posClickerLog
            // 
            this.posClickerLog.AutoScroll = true;
            this.posClickerLog.AutoSize = false;
            this.posClickerLog.Location = new System.Drawing.Point(7, 164);
            this.posClickerLog.Multiline = true;
            this.posClickerLog.Name = "posClickerLog";
            this.posClickerLog.ReadOnly = true;
            this.posClickerLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.posClickerLog.Size = new System.Drawing.Size(319, 192);
            this.posClickerLog.TabIndex = 36;
            // 
            // posGridView
            // 
            this.posGridView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.posGridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.posGridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.posGridView.ForeColor = System.Drawing.Color.Black;
            this.posGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.posGridView.Location = new System.Drawing.Point(186, 8);
            // 
            // posGridView
            // 
            this.posGridView.MasterTemplate.AllowAddNewRow = false;
            this.posGridView.MasterTemplate.AllowColumnReorder = false;
            this.posGridView.MasterTemplate.AllowColumnResize = false;
            this.posGridView.MasterTemplate.AllowRowResize = false;
            this.posGridView.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.HeaderText = "Position";
            gridViewTextBoxColumn1.Name = "posColumn";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.Width = 123;
            this.posGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1});
            this.posGridView.MasterTemplate.EnableGrouping = false;
            this.posGridView.MasterTemplate.EnableSorting = false;
            this.posGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.posGridView.Name = "posGridView";
            this.posGridView.ReadOnly = true;
            this.posGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.posGridView.ShowGroupPanel = false;
            this.posGridView.Size = new System.Drawing.Size(140, 150);
            this.posGridView.TabIndex = 35;
            this.posGridView.Text = "radGridView1";
            this.posGridView.ThemeName = "VisualStudio2012Light";
            // 
            // circlesLabel
            // 
            this.circlesLabel.AutoSize = true;
            this.circlesLabel.Location = new System.Drawing.Point(41, 134);
            this.circlesLabel.Name = "circlesLabel";
            this.circlesLabel.Size = new System.Drawing.Size(90, 13);
            this.circlesLabel.TabIndex = 21;
            this.circlesLabel.Text = "Circles Count:  0";
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.copyrightLabel.Location = new System.Drawing.Point(0, 395);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(146, 19);
            this.copyrightLabel.TabIndex = 1;
            this.copyrightLabel.Text = "© Vanya Zolotaryov 2015";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 415);
            this.Controls.Add(this.copyrightLabel);
            this.Controls.Add(this.clickersPageView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "BazgerTools";
            this.ThemeName = "VisualStudio2012Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.clickersPageView)).EndInit();
            this.clickersPageView.ResumeLayout(false);
            this.ClickerPage.ResumeLayout(false);
            this.ClickerPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clickerLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerSpinEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startClickerButton)).EndInit();
            this.PositionClicker.ResumeLayout(false);
            this.PositionClicker.PerformLayout();
            this.posClickerPanel.ResumeLayout(false);
            this.posClickerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.moveDownButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.removeButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveUpButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearAllButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posClickerSpinEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posClickerLog)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.copyrightLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.Themes.VisualStudio2012LightTheme ThemeLight;
        private Telerik.WinControls.RootRadElement object_b5396cdc_c5a7_4301_882f_ea3e4dbfb3fe;
        private Telerik.WinControls.UI.RadPageView clickersPageView;
        private Telerik.WinControls.UI.RadPageViewPage ClickerPage;
        private Telerik.WinControls.UI.RadPageViewPage PositionClicker;
        private Telerik.WinControls.UI.RadSpinEditor clickerSpinEditor;
        private Telerik.WinControls.UI.RadButton startClickerButton;
        private System.Windows.Forms.Label delayClickerLabel;
        private System.Windows.Forms.Label allCountLabel;
        private System.Windows.Forms.Label countLabel;
        private System.Windows.Forms.Label circlesLabel;
        private Telerik.WinControls.UI.RadGridView posGridView;
        private Telerik.WinControls.UI.RadTextBox posClickerLog;
        private Telerik.WinControls.UI.RadLabel copyrightLabel;
        private Telerik.WinControls.UI.RadTextBox clickerLog;
        private System.Windows.Forms.Panel posClickerPanel;
        private Telerik.WinControls.UI.RadButton moveDownButton;
        private Telerik.WinControls.UI.RadButton removeButton;
        private Telerik.WinControls.UI.RadButton moveUpButton;
        private Telerik.WinControls.UI.RadButton clearAllButton;
        private System.Windows.Forms.Label delayPosClickerLabel;
        private Telerik.WinControls.UI.RadSpinEditor posClickerSpinEditor;
    }
}