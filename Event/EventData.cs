using NiceBowl.Algorithm.Timeline;
using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Event
{
    enum EventState
    {
        Ready, Running, Stopped
    }
    class EventData
    {
        public delegate EventState EventHandler(EventData data, EventContext context);
        public readonly EventHandler task;
        public IIntExpr time;
        public EventState state { get; private set; } = EventState.Ready;

        public EventData(IIntExpr time, EventHandler task)
        {
            this.time = time;
            this.task = task;
        }

        public void Run(EventContext context)
        {
            state = task(this, context);
        }

        public override string ToString()
        {
            return $"Task At {Util.MsToString(time.Eval())}";
        }
    }
}
