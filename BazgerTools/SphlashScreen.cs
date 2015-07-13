using System;
using System.Drawing;
using System.Windows.Forms;
using BazgerTools.Properties;

namespace BazgerTools
{
    public partial class SphlashScreen : Form
    {
        public bool ChangeForm;
        public SphlashScreen()
        {
            InitializeComponent();
            AllowTransparency = true;
            BackColor = Color.AliceBlue;//цвет фона  
            TransparencyKey = BackColor;//он же будет заменен на прозрачный цвет
            pictureBox.Image = Resources.Background;
        }

        public override sealed Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        private void timerOpacity_Tick(object sender, EventArgs e)
        {
            timerStart.Enabled = false;
            Opacity = Opacity - 0.05;
            if (Opacity == 0)
            {               
                timerOpacity.Enabled = false;
                ChangeForm = true;
                Close();
            }
        }

        private void timerStart_Tick(object sender, EventArgs e)
        {
            timerOpacity.Enabled = true;
        }
    }
}
