namespace Bazger.Tools.App.Pages
{
    partial class PositionClickerControl
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.ctrlsPnl = new Telerik.WinControls.UI.RadPanel();
            this.circlesLbl = new Telerik.WinControls.UI.RadLabel();
            this.moveDownBtn = new Telerik.WinControls.UI.RadButton();
            this.delaySpin = new Telerik.WinControls.UI.RadSpinEditor();
            this.removeBtn = new Telerik.WinControls.UI.RadButton();
            this.delayLbl = new Telerik.WinControls.UI.RadLabel();
            this.moveUpBtn = new Telerik.WinControls.UI.RadButton();
            this.clearAllBtn = new Telerik.WinControls.UI.RadButton();
            this.positionsGrid = new Telerik.WinControls.UI.RadGridView();
            this.visualStudio2012DarkTheme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.visualStudio2012LightTheme = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            ((System.ComponentModel.ISupportInitialize)(this.ctrlsPnl)).BeginInit();
            this.ctrlsPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circlesLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveDownBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delaySpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.removeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveUpBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearAllBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionsGrid.MasterTemplate)).BeginInit();
            this.SuspendLayout();
            // 
            // ctrlsPnl
            // 
            this.ctrlsPnl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ctrlsPnl.Controls.Add(this.circlesLbl);
            this.ctrlsPnl.Controls.Add(this.moveDownBtn);
            this.ctrlsPnl.Controls.Add(this.delaySpin);
            this.ctrlsPnl.Controls.Add(this.removeBtn);
            this.ctrlsPnl.Controls.Add(this.delayLbl);
            this.ctrlsPnl.Controls.Add(this.moveUpBtn);
            this.ctrlsPnl.Controls.Add(this.clearAllBtn);
            this.ctrlsPnl.Location = new System.Drawing.Point(3, 3);
            this.ctrlsPnl.Name = "ctrlsPnl";
            this.ctrlsPnl.Size = new System.Drawing.Size(220, 158);
            this.ctrlsPnl.TabIndex = 36;
            this.ctrlsPnl.ThemeName = "VisualStudio2012Light";
            // 
            // circlesLbl
            // 
            this.circlesLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.circlesLbl.Location = new System.Drawing.Point(57, 127);
            this.circlesLbl.Name = "circlesLbl";
            this.circlesLbl.Size = new System.Drawing.Size(102, 21);
            this.circlesLbl.TabIndex = 36;
            this.circlesLbl.Text = "Circles Count:  0";
            this.circlesLbl.ThemeName = "VisualStudio2012Light";
            // 
            // moveDownBtn
            // 
            this.moveDownBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.moveDownBtn.Location = new System.Drawing.Point(120, 94);
            this.moveDownBtn.Name = "moveDownBtn";
            this.moveDownBtn.Size = new System.Drawing.Size(92, 30);
            this.moveDownBtn.TabIndex = 34;
            this.moveDownBtn.Text = "Move Down";
            this.moveDownBtn.ThemeName = "VisualStudio2012Light";
            this.moveDownBtn.Click += new System.EventHandler(this.moveDownButton_Click);
            // 
            // delaySpin
            // 
            this.delaySpin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.delaySpin.Location = new System.Drawing.Point(5, 25);
            this.delaySpin.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.delaySpin.Name = "delaySpin";
            this.delaySpin.NullableValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.delaySpin.Size = new System.Drawing.Size(92, 24);
            this.delaySpin.TabIndex = 29;
            this.delaySpin.TabStop = false;
            this.delaySpin.ThemeName = "VisualStudio2012Light";
            this.delaySpin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // removeBtn
            // 
            this.removeBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.removeBtn.Location = new System.Drawing.Point(5, 58);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(92, 30);
            this.removeBtn.TabIndex = 31;
            this.removeBtn.Text = "Remove";
            this.removeBtn.ThemeName = "VisualStudio2012Light";
            this.removeBtn.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // delayLbl
            // 
            this.delayLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.delayLbl.Location = new System.Drawing.Point(5, 3);
            this.delayLbl.Name = "delayLbl";
            this.delayLbl.Size = new System.Drawing.Size(43, 21);
            this.delayLbl.TabIndex = 24;
            this.delayLbl.Text = "Delay:";
            this.delayLbl.ThemeName = "VisualStudio2012Light";
            // 
            // moveUpBtn
            // 
            this.moveUpBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.moveUpBtn.Location = new System.Drawing.Point(120, 58);
            this.moveUpBtn.Name = "moveUpBtn";
            this.moveUpBtn.Size = new System.Drawing.Size(92, 30);
            this.moveUpBtn.TabIndex = 33;
            this.moveUpBtn.Text = "Move Up";
            this.moveUpBtn.ThemeName = "VisualStudio2012Light";
            this.moveUpBtn.Click += new System.EventHandler(this.moveUpButton_Click);
            // 
            // clearAllBtn
            // 
            this.clearAllBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clearAllBtn.Location = new System.Drawing.Point(5, 94);
            this.clearAllBtn.Name = "clearAllBtn";
            this.clearAllBtn.Size = new System.Drawing.Size(92, 30);
            this.clearAllBtn.TabIndex = 32;
            this.clearAllBtn.Text = "Clear All";
            this.clearAllBtn.ThemeName = "VisualStudio2012Light";
            this.clearAllBtn.Click += new System.EventHandler(this.clearAllButton_Click);
            // 
            // positionsGrid
            // 
            this.positionsGrid.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.positionsGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.positionsGrid.Cursor = System.Windows.Forms.Cursors.Default;
            this.positionsGrid.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.positionsGrid.ForeColor = System.Drawing.Color.Black;
            this.positionsGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.positionsGrid.Location = new System.Drawing.Point(226, 3);
            // 
            // 
            // 
            this.positionsGrid.MasterTemplate.AllowAddNewRow = false;
            this.positionsGrid.MasterTemplate.AllowColumnReorder = false;
            this.positionsGrid.MasterTemplate.AllowRowResize = false;
            this.positionsGrid.MasterTemplate.AutoGenerateColumns = false;
            gridViewTextBoxColumn1.EnableExpressionEditor = false;
            gridViewTextBoxColumn1.HeaderText = "Position";
            gridViewTextBoxColumn1.Name = "position";
            gridViewTextBoxColumn1.ReadOnly = true;
            gridViewTextBoxColumn1.Width = 215;
            this.positionsGrid.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1});
            this.positionsGrid.MasterTemplate.EnableGrouping = false;
            this.positionsGrid.MasterTemplate.EnableSorting = false;
            this.positionsGrid.MasterTemplate.ShowRowHeaderColumn = false;
            this.positionsGrid.MasterTemplate.VerticalScrollState = Telerik.WinControls.UI.ScrollState.AlwaysShow;
            this.positionsGrid.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.positionsGrid.Name = "positionsGrid";
            this.positionsGrid.ReadOnly = true;
            this.positionsGrid.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.positionsGrid.ShowGroupPanel = false;
            this.positionsGrid.Size = new System.Drawing.Size(233, 158);
            this.positionsGrid.TabIndex = 37;
            this.positionsGrid.ThemeName = "VisualStudio2012Light";
            // 
            // PositionClickerControl
            // 
            this.Controls.Add(this.ctrlsPnl);
            this.Controls.Add(this.positionsGrid);
            this.Name = "PositionClickerControl";
            this.Size = new System.Drawing.Size(463, 164);
            ((System.ComponentModel.ISupportInitialize)(this.ctrlsPnl)).EndInit();
            this.ctrlsPnl.ResumeLayout(false);
            this.ctrlsPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circlesLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveDownBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delaySpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.removeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moveUpBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clearAllBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionsGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positionsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel ctrlsPnl;
        private Telerik.WinControls.UI.RadLabel circlesLbl;
        private Telerik.WinControls.UI.RadButton moveDownBtn;
        private Telerik.WinControls.UI.RadSpinEditor delaySpin;
        private Telerik.WinControls.UI.RadButton removeBtn;
        private Telerik.WinControls.UI.RadLabel delayLbl;
        private Telerik.WinControls.UI.RadButton moveUpBtn;
        private Telerik.WinControls.UI.RadButton clearAllBtn;
        private Telerik.WinControls.UI.RadGridView positionsGrid;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme;
    }
}
