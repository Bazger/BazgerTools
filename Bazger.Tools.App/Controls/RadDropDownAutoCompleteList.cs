using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.Design;
using Telerik.WinControls.UI;

namespace Bazger.Tools.App.Controls
{
    public class RadDropDownAutoCompleteList : RadDropDownList
    {

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // RadDropDownAutoCompleteList
            // 
            this.Text = "vAstasr";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        //        [RadDefaultValue("AutoCompleteSource", typeof(HostedTextBoxBase))]
        //        [EditorBrowsable(EditorBrowsableState.Always)]
        //        [Browsable(true)]
        //        [Description("Gets or sets a value specifying the source of complete strings used for automatic completion.")]
        //        public AutoCompleteSource AutoCompleteSource
        //        {
        //            get
        //            {
        //                return ((System.Windows.Forms.TextBox)this.CreateDropDownListElement().TextBoxItem.HostedControl).AutoCompleteSource;
        //            }
        //            set
        //            {
        //                ((System.Windows.Forms.TextBox)this.maskEditBoxElement.TextBoxItem.HostedControl).AutoCompleteSource = value;
        //            }
        //        }
    }
}
