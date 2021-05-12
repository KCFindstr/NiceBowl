using System;
using System.Threading;
using System.Threading.Tasks;

namespace NiceBowl.Event
{
    class EventScheduler
    {
        private EventContext mContext;

        public void OnTimerUpdate(long prev, long time)
        {
            var context = mContext;
            if (context == null)
                return;

            context.prevTime = prev;
            context.time = time;

            var charas = context.Charas;
            if (charas != null)
            {
                int startIndex = 5 - charas.Count;
                for (int i = 0; i < charas.Count; i++)
                {
                    if (!charas[i].pos.Valid)
                    {
                        charas[i].CalcPosition(context, startIndex + i);
                    }
                }
            }

            foreach (var e in context.Events)
            {
                long eTime = e.time.Eval();
                if (e.state == EventState.Stopped)
                    continue;
                if (e.state == EventState.Running || eTime < prev && eTime >= time)
                    e.Run(context);
            }
        }

        public void SetContext(EventContext context)
        {
            mContext = context;
        }

        public void Start()
        {
            mContext.Timer.OnUpdate += OnTimerUpdate;
        }

        public void Stop()
        {
            mContext.Timer.OnUpdate -= OnTimerUpdate;
        }
    }
}
