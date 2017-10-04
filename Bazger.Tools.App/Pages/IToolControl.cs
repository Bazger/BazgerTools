using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public interface IToolControl
    {
        List<string> Log { get; }
        RadPageViewPage ParentPage { get; set; }
        void IntializeControl(MainForm form);
    }
}
