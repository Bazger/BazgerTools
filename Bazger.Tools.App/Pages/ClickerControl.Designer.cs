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
            this.clickerStartButton = new Telerik.WinControls.UI.RadButton();
            this.clickerCountLbl = new Telerik.WinControls.UI.RadLabel();
            this.clickerAllCountLbl = new Telerik.WinControls.UI.RadLabel();
            this.clickerDelayLbl = new Telerik.WinControls.UI.RadLabel();
            this.clickerDeleySpin = new Telerik.WinControls.UI.RadSpinEditor();
            this.clickerCtrlsPnl = new Telerik.WinControls.UI.RadPanel();
            ((System.ComponentModel.ISupportInitialize)(this.clickerStartButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerCountLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerAllCountLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerDelayLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerDeleySpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerCtrlsPnl)).BeginInit();
            this.clickerCtrlsPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // clickerStartButton
            // 
            this.clickerStartButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerStartButton.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clickerStartButton.Location = new System.Drawing.Point(6, 3);
            this.clickerStartButton.Name = "clickerStartButton";
            this.clickerStartButton.Size = new System.Drawing.Size(284, 158);
            this.clickerStartButton.TabIndex = 27;
            this.clickerStartButton.Text = "Test Clicking!";
            this.clickerStartButton.ThemeName = "VisualStudio2012Light";
            this.clickerStartButton.Click += new System.EventHandler(this.clickerStartButton_Click);
            // 
            // clickerCountLbl
            // 
            this.clickerCountLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerCountLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clickerCountLbl.Location = new System.Drawing.Point(7, 42);
            this.clickerCountLbl.Name = "clickerCountLbl";
            this.clickerCountLbl.Size = new System.Drawing.Size(60, 21);
            this.clickerCountLbl.TabIndex = 26;
            this.clickerCountLbl.Text = "Count:  0";
            this.clickerCountLbl.ThemeName = "VisualStudio2012Light";
            // 
            // clickerAllCountLbl
            // 
            this.clickerAllCountLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerAllCountLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clickerAllCountLbl.Location = new System.Drawing.Point(7, 59);
            this.clickerAllCountLbl.Name = "clickerAllCountLbl";
            this.clickerAllCountLbl.Size = new System.Drawing.Size(79, 21);
            this.clickerAllCountLbl.TabIndex = 27;
            this.clickerAllCountLbl.Text = "All Count:  0";
            this.clickerAllCountLbl.ThemeName = "VisualStudio2012Light";
            // 
            // clickerDelayLbl
            // 
            this.clickerDelayLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerDelayLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clickerDelayLbl.Location = new System.Drawing.Point(7, 9);
            this.clickerDelayLbl.Name = "clickerDelayLbl";
            this.clickerDelayLbl.Size = new System.Drawing.Size(43, 21);
            this.clickerDelayLbl.TabIndex = 28;
            this.clickerDelayLbl.Text = "Delay:";
            this.clickerDelayLbl.ThemeName = "VisualStudio2012Light";
            // 
            // clickerDeleySpin
            // 
            this.clickerDeleySpin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerDeleySpin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.clickerDeleySpin.Location = new System.Drawing.Point(56, 7);
            this.clickerDeleySpin.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.clickerDeleySpin.Name = "clickerDeleySpin";
            this.clickerDeleySpin.Size = new System.Drawing.Size(106, 27);
            this.clickerDeleySpin.TabIndex = 29;
            this.clickerDeleySpin.TabStop = false;
            this.clickerDeleySpin.ThemeName = "VisualStudio2012Light";
            this.clickerDeleySpin.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // clickerCtrlsPnl
            // 
            this.clickerCtrlsPnl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.clickerCtrlsPnl.Controls.Add(this.clickerDeleySpin);
            this.clickerCtrlsPnl.Controls.Add(this.clickerDelayLbl);
            this.clickerCtrlsPnl.Controls.Add(this.clickerAllCountLbl);
            this.clickerCtrlsPnl.Controls.Add(this.clickerCountLbl);
            this.clickerCtrlsPnl.Location = new System.Drawing.Point(293, 3);
            this.clickerCtrlsPnl.Name = "clickerCtrlsPnl";
            this.clickerCtrlsPnl.Size = new System.Drawing.Size(165, 158);
            this.clickerCtrlsPnl.TabIndex = 28;
            // 
            // ClickerControl
            // 
            this.Controls.Add(this.clickerCtrlsPnl);
            this.Controls.Add(this.clickerStartButton);
            this.Name = "ClickerControl";
            this.Size = new System.Drawing.Size(463, 164);
            ((System.ComponentModel.ISupportInitialize)(this.clickerStartButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerCountLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerAllCountLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerDelayLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerDeleySpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clickerCtrlsPnl)).EndInit();
            this.clickerCtrlsPnl.ResumeLayout(false);
            this.clickerCtrlsPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton clickerStartButton;
        private Telerik.WinControls.UI.RadLabel clickerCountLbl;
        private Telerik.WinControls.UI.RadLabel clickerAllCountLbl;
        private Telerik.WinControls.UI.RadLabel clickerDelayLbl;
        private Telerik.WinControls.UI.RadSpinEditor clickerDeleySpin;
        private Telerik.WinControls.UI.RadPanel clickerCtrlsPnl;
    }
}
