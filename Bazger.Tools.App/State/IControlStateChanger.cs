using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazger.Tools.App.State
{
    public interface IControlStateChanger
    {
        void LoadState(IControlState controlState);
        IControlState SaveState();
    }
}
