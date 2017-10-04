using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bazger.Tools.App.Pages
{
    public interface IToolControl
    {
        List<string> Log { get; }

        void IntializeControl(MainForm form);
    }
}
