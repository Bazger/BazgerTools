using System;
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
