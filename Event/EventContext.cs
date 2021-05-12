using NiceBowl.Algorithm.Timeline;
using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Event
{
    class EventContext
    {
        public readonly WindowManager Window;
        public readonly PCRTimer Timer;
        public readonly List<EventData> Events = new List<EventData>();
        public List<CharaInfo> Charas;
        public long prevTime;
        public long time;

        public EventContext(WindowManager window, PCRTimer timer)
        {
            Window = window;
            Timer = timer;
        }

        public void AddEvent(EventData data)
        {
            Console.WriteLine($"Add event at {data.time.ToTimeString()}");
            Events.Add(data);
        }

        public void ClearEvents()
        {
            Events.Clear();
        }

        public CharaInfo FindChara(string name)
        {
            if (Charas == null)
                return null;
            foreach (var chara in Charas)
            {
                if (chara.name == name)
                    return chara;
            }
            return null;
        }
    }
}
