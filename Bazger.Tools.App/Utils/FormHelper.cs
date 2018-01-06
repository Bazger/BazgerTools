using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bazger.Tools.App.Utils
{
    public static class FormHelper
    {

        public static void ControlInvoker<T>(T control, Action<T> action) where T : ScrollableControl, new()
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new MethodInvoker(() => { action(control); }));
            }
            else
            {
                action(control);
            }
        }
    }
}
