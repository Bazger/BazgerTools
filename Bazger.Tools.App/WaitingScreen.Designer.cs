namespace Bazger.Tools.App
{
    partial class WaitingScreen
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
            this.visualStudio2012DarkTheme = new Telerik.WinControls.Themes.VisualStudio2012DarkTheme();
            this.visualStudio2012LightTheme = new Telerik.WinControls.Themes.VisualStudio2012LightTheme();
            this.closingAppWaitingBar = new Telerik.WinControls.UI.RadWaitingBar();
            this.waitingBarIndicatorElement1 = new Telerik.WinControls.UI.WaitingBarIndicatorElement();
            this.closingAppLbl = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.closingAppWaitingBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closingAppLbl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // closingAppWaitingBar
            // 
            this.closingAppWaitingBar.Location = new System.Drawing.Point(12, 25);
            this.closingAppWaitingBar.Name = "closingAppWaitingBar";
            this.closingAppWaitingBar.Size = new System.Drawing.Size(320, 24);
            this.closingAppWaitingBar.TabIndex = 0;
            this.closingAppWaitingBar.Text = "radWaitingBar1";
            this.closingAppWaitingBar.ThemeName = "VisualStudio2012Light";
            this.closingAppWaitingBar.WaitingIndicators.Add(this.waitingBarIndicatorElement1);
            this.closingAppWaitingBar.WaitingIndicatorSize = new System.Drawing.Size(100, 14);
            this.closingAppWaitingBar.WaitingSpeed = 100;
            this.closingAppWaitingBar.WaitingStep = 4;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.closingAppWaitingBar.GetChildAt(0))).WaitingIndicatorSize = new System.Drawing.Size(100, 14);
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.closingAppWaitingBar.GetChildAt(0))).WaitingSpeed = 100;
            ((Telerik.WinControls.UI.RadWaitingBarElement)(this.closingAppWaitingBar.GetChildAt(0))).WaitingStep = 4;
            ((Telerik.WinControls.UI.WaitingBarSeparatorElement)(this.closingAppWaitingBar.GetChildAt(0).GetChildAt(0).GetChildAt(0))).ProgressOrientation = Telerik.WinControls.ProgressOrientation.Right;
            ((Telerik.WinControls.UI.WaitingBarSeparatorElement)(this.closingAppWaitingBar.GetChildAt(0).GetChildAt(0).GetChildAt(0))).Dash = false;
            // 
            // waitingBarIndicatorElement1
            // 
            this.waitingBarIndicatorElement1.Name = "waitingBarIndicatorElement1";
            this.waitingBarIndicatorElement1.StretchHorizontally = false;
            // 
            // closingAppLbl
            // 
            this.closingAppLbl.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.closingAppLbl.Location = new System.Drawing.Point(118, 0);
            this.closingAppLbl.Name = "closingAppLbl";
            this.closingAppLbl.Size = new System.Drawing.Size(128, 21);
            this.closingAppLbl.TabIndex = 34;
            this.closingAppLbl.Text = "Closing application...";
            this.closingAppLbl.ThemeName = "VisualStudio2012Light";
            // 
            // WaitingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 53);
            this.ControlBox = false;
            this.Controls.Add(this.closingAppLbl);
            this.Controls.Add(this.closingAppWaitingBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "WaitingScreen";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "";
            this.ThemeName = "VisualStudio2012Light";
            ((System.ComponentModel.ISupportInitialize)(this.closingAppWaitingBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closingAppLbl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.Themes.VisualStudio2012DarkTheme visualStudio2012DarkTheme;
        private Telerik.WinControls.Themes.VisualStudio2012LightTheme visualStudio2012LightTheme;
        private Telerik.WinControls.UI.RadWaitingBar closingAppWaitingBar;
        private Telerik.WinControls.UI.WaitingBarIndicatorElement waitingBarIndicatorElement1;
        private Telerik.WinControls.UI.RadLabel closingAppLbl;
    }
}