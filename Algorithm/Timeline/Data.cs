using NiceBowl.Event;
using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm.Timeline
{
    class CharaInfo
    {
        public string name;

        public Position pos = Position.MinValue;

        public void CalcPosition(EventContext context, int index)
        {
            var window = context.Window.GetWindowRect();
            var timer = context.Timer.TimerRect;
            int height = window.bottom - window.top;
            int playHeight = (int)((height - timer.bottom) / 0.93333);
            int playWidth = (int)(timer.left / 0.74375);
            int charaWidth = playWidth / 8;
            int startX = playWidth / 4;
            int startY = height - (int)(playHeight * 0.18333);
            pos = new Position(startX + index * charaWidth, startY);
        }

        public override string ToString()
        {
            return name;
        }
    }

    class CharaUbTime
    {
        public bool isRelative = false;
        public IIntExpr time;
        public IIntExpr end;

        public bool IsContinuous => end != null;

        public override string ToString()
        {
            var ret = time.ToTimeString();
            if (IsContinuous)
                ret += "~" + end.ToTimeString();
            return ret;
        }

        public CharaUbTime SetIsOffset(bool val) {
            isRelative = val;
            return this;
        }
    }
}
