using System;
using System.Windows.Forms;

namespace Bazger.Tools.App
{
    static class Program
    {
        public static MainForm MainForm;
        public static SphlashScreen SphlashForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SphlashForm = new SphlashScreen();
            MainForm = new MainForm();
            Application.Run(SphlashForm);
            if (SphlashForm.ChangeForm)
                Application.Run(MainForm);
        }
    }
}