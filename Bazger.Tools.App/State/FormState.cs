using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Tools.App.State
{
    [Serializable]
    public class FormState
    {
        public Dictionary<string, IControlState> ControlStates { get; }

        public FormState()
        {
            ControlStates = new Dictionary<string, IControlState>();
        }
    }
}
