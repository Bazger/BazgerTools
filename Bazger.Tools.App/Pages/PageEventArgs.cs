using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Pages
{
    public class PageEventArgs : EventArgs
    {
        public RadPageView ToolControlsPager { get; }
        public RadPageViewPage SelectedPage => ToolControlsPager.SelectedPage;


        public PageEventArgs(RadPageView toolControlsPager)
        {
            ToolControlsPager = toolControlsPager;
        }
    }
}
