using NiceBowl.Algorithm.Timeline;
using NiceBowl.Screen;
using System;
using System.Collections.Generic;

namespace NiceBowl.Event
{
    class EventContext
    {
        public readonly WindowManager Window;
        public readonly PCRTimer Timer;
        public readonly List<EventData> Events = new List<EventData>();
        public List<CharaInfo> Charas;
        public long prevRealTime;
        public long realTime;
        public long prevTime;
        public long time;

        public EventContext(WindowManager window, PCRTimer timer)
        {
            Window = window;
            Timer = timer;
        }

        public void AddEvent(EventData data)
        {
            Events.Add(data);
            data.context = this;
        }

        public void ClearEvents()
        {
            Events.Clear();
        }

        public long GameTimeToRealTime(long gameTime) {
            return Timer.RealTime + Timer.Time - gameTime;
        }

        public long RealTimeToGameTime(long realTime) {
            return Timer.Time - (realTime - Timer.RealTime);
        }

        public bool JustPassedReal(long realTime) {
            return realTime > prevRealTime && realTime <= this.realTime;
        }

        public bool JustPassedGame(long gameTime) {
            return gameTime < prevTime && gameTime >= time;
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
