using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bazger.Tools.App.State;
using NLog;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public interface IToolControl
    {
        //Title need to be different in all derived classes, because loggers using it to create different logger for each control
        string Title { get; }
        RadPageViewPage ParentPage { get; set; }
        void IntializeControl(MainForm form);
        void OnFormClosing(object sender, FormClosingEventArgs e);        
    }
}
