namespace NiceBowl.Event
{
    class EventScheduler
    {
        private EventContext mContext;

        public void OnTimerUpdate(long prevRealTime, long realTime, long prev, long time)
        {
            var context = mContext;
            if (context == null)
                return;

            context.prevRealTime = prevRealTime;
            context.realTime = realTime;
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

            context.Events.RemoveAll(e => e.Run() == EventState.Stopped);
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
