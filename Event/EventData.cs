using NiceBowl.Algorithm.Timeline;
using NiceBowl.Screen;
using System;
using System.Text;

namespace NiceBowl.Event
{
    enum EventState
    {
        Ready, Running, Stopped
    }
    class EventData
    {
        public EventContext context;
        public delegate EventState EventHandler(EventData data, EventContext context);
        public readonly EventHandler task;
        public readonly IIntExpr timeExpr;
        public readonly IIntExpr endTimeExpr;
        public readonly IIntExpr refTimeExpr;
        public long StartRealTime { get; private set; } = long.MaxValue;
        public long Duration => (StartRealTime == long.MaxValue ?
            0L : context.realTime - StartRealTime);
        public bool IsRelative => refTimeExpr != null;
        private long refRealTime = long.MaxValue;
        public EventState state { get; private set; } = EventState.Ready;

        public EventData(
            IIntExpr time,
            EventHandler task,
            IIntExpr endTime = null,
            IIntExpr refTime = null)
        {
            timeExpr = time;
            endTimeExpr = endTime;
            refTimeExpr = refTime;
            this.task = task;
        }

        private long GetRelativeTime(IIntExpr time) {
            if (refRealTime == long.MaxValue) {
                throw new InvalidOperationException(
                    "Trying to get relative time without reference");
            }
            return refRealTime + time.Eval();
        }

        private EventState RunTask() {
            if (StartRealTime == long.MaxValue) {
                StartRealTime = context.realTime;
            }
            state = task(this, context);
            if (state == EventState.Running) {
                if (endTimeExpr == null) {
                    state = EventState.Stopped;
                } else if (context.JustPassedReal(GetRelativeTime(endTimeExpr))) {
                    state = EventState.Stopped;
                } else if (Duration > 30 * 1000) {
                    Main.Log("警告：事件超过了30秒，强制终止。");
                    state = EventState.Stopped;
                }
            }
            return state;
        }

        public EventState Run()
        {
            if (state == EventState.Stopped) {
                return state;
            }
            if (state == EventState.Running) {
                return RunTask();
            }
            if (IsRelative) {
                if (context.JustPassedGame(refTimeExpr.Eval())) {
                    refRealTime = context.realTime;
                }
                if (refRealTime != long.MaxValue) {
                    if (context.JustPassedReal(GetRelativeTime(timeExpr))) {
                        return RunTask();
                    }
                }
            } else {
                if (context.JustPassedGame(timeExpr.Eval())) {
                    return RunTask();
                }
            }
            return state;
        }

        public override string ToString()
        {
            var str = new StringBuilder().Append("Task At ");
            if (refTimeExpr != null) {
                str.Append(refTimeExpr.ToTimeString() + " +");
            }
            str.Append(timeExpr.ToTimeString());
            return str.ToString();
        }
    }
}
