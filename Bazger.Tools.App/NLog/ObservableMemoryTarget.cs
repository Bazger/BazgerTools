using System.Collections.Generic;
using System.Collections.ObjectModel;
using NLog;
using NLog.Targets;

namespace Bazger.Tools.App.NLog
{
    [Target("ObservableMemory")]
    public class ObservableMemoryTarget : TargetWithLayout
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTarget" /> class.
        /// </summary>
        /// <remarks>
        /// The default value of the layout is: <code>${longdate}|${level:uppercase=true}|${logger}|${message}</code>
        /// </remarks>
        public ObservableMemoryTarget()
        {
            Logs = new ObservableCollection<string>();
            OptimizeBufferReuse = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryTarget" /> class.
        /// </summary>
        /// <remarks>
        /// The default value of the layout is: <code>${longdate}|${level:uppercase=true}|${logger}|${message}</code>
        /// </remarks>
        /// <param name="name">Name of the target.</param>
        public ObservableMemoryTarget(string name) : this()
        {
            Name = name;
        }

        /// <summary>
        /// Gets the list of logs gathered in the <see cref="MemoryTarget"/>.
        /// </summary>
        public ObservableCollection<string> Logs { get; private set; }

        /// <summary>
        /// Renders the logging event message and adds it to the internal ArrayList of log messages.
        /// </summary>
        /// <param name="logEvent">The logging event.</param>
        protected override void Write(LogEventInfo logEvent)
        {
            Logs.Add(RenderLogEvent(Layout, logEvent));
        }
    }
}
