using System;
using System.Drawing;
using System.Windows.Forms;
using Bazger.Tools.App.Properties;

namespace Bazger.Tools.App
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

        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
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
