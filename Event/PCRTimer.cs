using NiceBowl.Algorithm;
using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiceBowl.Event
{

    class PCRTimer : IDisposable
    {
        private class TimeRecord
        {
            public long time;
            public int sec;

            public TimeRecord(long time, int sec)
            {
                this.time = time;
                this.sec = sec;
            }
        }

        public bool Ready { get; private set; } = false;
        public Rect TimerRect { get; private set; } = new Rect();
        public long Time { get; private set; }
        public Action<long, long> OnUpdate;

        private const int ANALYZE_WINDOW = 60;
        private readonly WindowManager mWindowManager;
        private Rect mWindowRect = new Rect();
        private CancellationTokenSource mCancelWaitTaskPosition;
        private CancellationTokenSource mCancelUpdate = new CancellationTokenSource();

        private Stopwatch mWatch;
        private TimeRecord mCurrent;
        private List<TimeRecord> mTime = new List<TimeRecord>();

        private readonly TimerMatcher mMatcher = new TimerMatcher();
        private readonly TimeRecognizer mRecoginzer = new TimeRecognizer();

        public PCRTimer(WindowManager window)
        {
            mWindowManager = window;
            Update().ContinueWith(task =>
            {
                if (!task.IsCanceled && task.IsFaulted)
                {
                    foreach (var e in task.Exception.InnerExceptions)
                    {
                        Console.WriteLine(e.StackTrace);
                        throw e;
                    }
                }
            });
        }

        public void Dispose()
        {
            if (mCancelWaitTaskPosition != null)
            {
                mCancelWaitTaskPosition.Cancel();
                mCancelWaitTaskPosition.Dispose();
                mCancelWaitTaskPosition = null;
            }
            if (mCancelUpdate != null)
            {
                mCancelUpdate.Cancel();
                mCancelUpdate.Dispose();
                mCancelUpdate = null;
            }
        }

        private async Task WaitForTimerLocation()
        {
            var token = mCancelWaitTaskPosition.Token;
            int width = (int)((mWindowRect.right - mWindowRect.left) * 0.3);
            int height = (int)((mWindowRect.bottom - mWindowRect.top) * 0.15);
            int left = mWindowRect.right - mWindowRect.left - width;
            TimerRect = new Rect();
            while (true)
            {
                token.ThrowIfCancellationRequested();
                var image = mWindowManager.CaptureWindow(
                    left, 0, width, height
                );
                var rect = mMatcher.SetImage(image).Match();
                image.Dispose();
                if (rect.Length == 0)
                {
                    await Task.Delay(8, token);
                    continue;
                }
                Main.Log("找到了时间条");
                rect[0].left += left;
                rect[0].right += left;
                TimerRect = rect[0];
                break;
            }
            token.ThrowIfCancellationRequested();
            var watch = new Stopwatch();
            watch.Start();
            Time = long.MaxValue;
            mCurrent = new TimeRecord(watch.ElapsedMilliseconds, 90);
            mTime.Clear();
            UpdateTime(watch, token);
            mWatch = watch;
        }

        private int GetValidTimeText(int text)
        {
            if (text > 130 || text < 0)
                return -1;
            if (text < 100 && text >= 60)
                return -1;
            return text >= 100 ? text - 40 : text;
        }

        private void SanitizeTime(List<TimeRecord> time)
        {
            if (time.Count < 20)
                return;
            int[] count = new int[128];
            time.ForEach((t) => count[t.sec]++);
            int threshold = time.Count / 4;
            int limit = 2;
            for (int i = time.Count - 1; i >= 0; i--)
            {
                int sec = time[i].sec;
                if (count[sec] >= threshold)
                {
                    limit--;
                    if (limit <= 0)
                    {
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (time[j].sec != sec && time[j].sec != sec + 1)
                            {
                                time.RemoveAt(j);
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void UpdateTime(Stopwatch watch, CancellationToken token)
        {
            int width = (TimerRect.right - TimerRect.left) / 3;
            var image = mWindowManager.CaptureWindow(
                TimerRect.right - width, TimerRect.top, width, TimerRect.bottom - TimerRect.top
            );
            long time = watch.ElapsedMilliseconds;
            int text = mRecoginzer.SetImage(image).Match();
            image.Dispose();
            int result = GetValidTimeText(text);
            //Console.WriteLine($"Recognized time: " + text);
            if (result >= 0)
            {
                mTime.Add(new TimeRecord(time, result));
                while (mTime.Count > ANALYZE_WINDOW)
                {
                    mTime.RemoveAt(0);
                }
            }
            TimeRecord selected = null;

            // Sanitize mTime
            SanitizeTime(mTime);

            foreach (var rec in mTime)
            {
                if (selected == null || rec.sec == selected.sec - 1)
                {
                    selected = rec;
                    continue;
                }
            }
            if (selected != null && mCurrent.sec != selected.sec)
            {
                mCurrent = selected;
            }
            long prev = Time;
            Time = (mCurrent.sec + 1) * 1000 -
                Math.Min(1000, watch.ElapsedMilliseconds - mCurrent.time);
            if (!Ready && mCurrent.sec < 90)
                Ready = true;
            token.ThrowIfCancellationRequested();
            if (prev != long.MaxValue)
                OnUpdate?.Invoke(prev, Time);
        }

        private async Task Update()
        {
            var token = mCancelUpdate.Token;
            while (true)
            {
                var watch = mWatch;
                if (watch == null)
                {
                    await Task.Delay(8, token);
                    continue;
                }
                await Task.Delay(1, token);
                UpdateTime(watch, token);
            }
        }

        private void CancelTask()
        {
            if (mCancelWaitTaskPosition != null)
            {
                mCancelWaitTaskPosition.Cancel();
                mCancelWaitTaskPosition.Dispose();
                mCancelWaitTaskPosition = null;
            }
        }

        public bool Start()
        {
            if (mWindowManager.Process == null)
            {
                Main.Log($"没有指定的窗口，无法开始");
                return false;
            }
            Ready = false;
            CancelTask();
            mCancelWaitTaskPosition = new CancellationTokenSource();
            mWindowRect = mWindowManager.GetWindowRect();
            WaitForTimerLocation().ContinueWith((task) =>
            {
                if (!task.IsCanceled && task.IsFaulted)
                {
                    foreach (var e in task.Exception.InnerExceptions)
                    {
                        Console.WriteLine(e.StackTrace);
                        throw e;
                    }
                }
            });
            return true;
        }

        public void Stop()
        {
            CancelTask();
            mWatch = null;
            Ready = false;
        }

        public void SetPid(int pid)
        {
            Stop();
            mWindowManager.SetPid(pid);
            if (mWindowManager.Process != null)
            {
                Main.Log($"获取到了窗口：{mWindowManager.Process.MainWindowTitle}");
            }
        }
    }
}
