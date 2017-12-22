using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bazger.Tools.App.State
{
    [Serializable]
    public class PositionClickerControlState : IControlState
    {
        [JsonProperty("DelaySpin")]
        public int DelaySpin { get; set; }

        [JsonProperty("PositionsGrid")]
        public List<string> PositionsGrid { get; set; }
    }
}
