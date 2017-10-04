namespace Bazger.Tools.App.Pages
{
    partial class ClickerControl
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
            this.startBtn = new Telerik.WinControls.UI.RadButton();
            this.countLbl = new Telerik.WinControls.UI.RadLabel();
            this.allCountLbl = new Telerik.WinControls.UI.RadLabel();
            this.delayLbl = new Telerik.WinControls.UI.RadLabel();
            this.deleySpin = new Telerik.WinControls.UI.RadSpinEditor();
            this.ctrlsPnl = new Telerik.WinControls.UI.RadPanel();
            this.visualStudio2012DarkTheme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.visualStudio2012LightTheme = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.countLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allCountLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleySpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctrlsPnl)).BeginInit();
            this.ctrlsPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.startBtn.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.startBtn.Location = new System.Drawing.Point(6, 3);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(284, 158);
            this.startBtn.TabIndex = 27;
            this.startBtn.Text = "Test Clicking!";
            this.startBtn.ThemeName = "VisualStudio2012Light";
            this.startBtn.Click += new System.EventHandler(this.clickerStartButton_Click);
            // 
            // countLbl
            // 
            this.countLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.countLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.countLbl.Location = new System.Drawing.Point(7, 42);
            this.countLbl.Name = "countLbl";
            this.countLbl.Size = new System.Drawing.Size(60, 21);
            this.countLbl.TabIndex = 26;
            this.countLbl.Text = "Count:  0";
            this.countLbl.ThemeName = "VisualStudio2012Light";
            // 
            // allCountLbl
            // 
            this.allCountLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.allCountLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.allCountLbl.Location = new System.Drawing.Point(7, 59);
            this.allCountLbl.Name = "allCountLbl";
            this.allCountLbl.Size = new System.Drawing.Size(79, 21);
            this.allCountLbl.TabIndex = 27;
            this.allCountLbl.Text = "All Count:  0";
            this.allCountLbl.ThemeName = "VisualStudio2012Light";
            // 
            // delayLbl
            // 
            this.delayLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.delayLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.delayLbl.Location = new System.Drawing.Point(7, 9);
            this.delayLbl.Name = "delayLbl";
            this.delayLbl.Size = new System.Drawing.Size(43, 21);
            this.delayLbl.TabIndex = 28;
            this.delayLbl.Text = "Delay:";
            this.delayLbl.ThemeName = "VisualStudio2012Light";
            // 
            // deleySpin
            // 
            this.deleySpin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.deleySpin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.deleySpin.Location = new System.Drawing.Point(56, 7);
            this.deleySpin.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.deleySpin.Name = "deleySpin";
            this.deleySpin.Size = new System.Drawing.Size(106, 27);
            this.deleySpin.TabIndex = 29;
            this.deleySpin.TabStop = false;
            this.deleySpin.ThemeName = "VisualStudio2012Light";
            this.deleySpin.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // ctrlsPnl
            // 
            this.ctrlsPnl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ctrlsPnl.Controls.Add(this.deleySpin);
            this.ctrlsPnl.Controls.Add(this.delayLbl);
            this.ctrlsPnl.Controls.Add(this.allCountLbl);
            this.ctrlsPnl.Controls.Add(this.countLbl);
            this.ctrlsPnl.Location = new System.Drawing.Point(293, 3);
            this.ctrlsPnl.Name = "ctrlsPnl";
            this.ctrlsPnl.Size = new System.Drawing.Size(165, 158);
            this.ctrlsPnl.TabIndex = 28;
            this.ctrlsPnl.ThemeName = "VisualStudio2012Light";
            // 
            // ClickerControl
            // 
            this.Controls.Add(this.ctrlsPnl);
            this.Controls.Add(this.startBtn);
            this.Name = "ClickerControl";
            this.Size = new System.Drawing.Size(463, 164);
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.countLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allCountLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delayLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleySpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctrlsPnl)).EndInit();
            this.ctrlsPnl.ResumeLayout(false);
            this.ctrlsPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton startBtn;
        private Telerik.WinControls.UI.RadLabel countLbl;
        private Telerik.WinControls.UI.RadLabel allCountLbl;
        private Telerik.WinControls.UI.RadLabel delayLbl;
        private Telerik.WinControls.UI.RadSpinEditor deleySpin;
        private Telerik.WinControls.UI.RadPanel ctrlsPnl;
        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme;
    }
}
