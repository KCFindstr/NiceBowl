using NiceBowl.Algorithm;
using NiceBowl.Algorithm.Timeline;
using NiceBowl.Event;
using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NiceBowl
{
    public partial class Main : Form
    {
        public static Main instance;
        public static readonly AppVersion version = new AppVersion(Application.ProductVersion);
        public static void Log(object message)
        {
            Console.WriteLine(message);
            instance.textBoxLog.Invoke(new Action(() => {
                instance.textBoxLog.AppendText(message.ToString() + "\r\n");
            }));
        }

        private readonly WindowManager mWindow = new WindowManager();
        private EventContext mContext;
        private readonly PCRTimer mTimer;
        private readonly EventScheduler mScheduler = new EventScheduler();
        private readonly TimelineParser mParser = new TimelineParser();

        public Main()
        {
            instance = this;
            mTimer = new PCRTimer(mWindow);
            mContext = new EventContext(mWindow, mTimer);
            InitializeComponent();
        }

        private void OnTimerUpdate(long _1, long _2, long _3, long time)
        {
            textTime.Invoke(new Action(() =>
            {
                if (!mTimer.Ready)
                {
                    textTime.Text = "未就绪";
                    return;
                }
                textTime.Text = Util.MsToString(time);
            }));
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Text += " " + Application.ProductVersion;
            Icon = (Icon)Properties.Resources.ResourceManager.GetObject("icon");
            mTimer.OnUpdate += OnTimerUpdate;
            //textBox轴.Text = System.IO.File.ReadAllText("../../dev/C5圣千.txt");
            /*
            for (int i = -4; i <= 8; i++)
            {
                using (var image = Image.FromFile($"../../dev/{i}.bmp"))
                {
                    int ret = new TimeRecognizer().SetImage(image).Match();
                    Console.WriteLine($"{i}: {ret}");
                }
            }
            */
        }

        private void button获取窗口_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxPID.Text, out int pid))
            {
                Log("获取窗口失败");
                return;
            }
            mTimer.SetPid(pid);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "开始")
            {
                var text = textBox轴.Text;
                var timeline = mParser.Run(text);
                if (timeline == null)
                    return;
                if (mTimer.Start())
                {
                    textBox轴.ReadOnly = true;
                    mContext = new EventContext(mWindow, mTimer);
                    timeline.Run(mContext);
                    mScheduler.SetContext(mContext);
                    mScheduler.Start();
                    buttonStart.Text = "停止";
                }
            }
            else
            {
                textBox轴.ReadOnly = false;
                buttonStart.Text = "开始";
                mContext = null;
                mTimer.Stop();
                mScheduler.Stop();
            }
        }

        private void buttonCheck_Click(object sender, EventArgs e)
        {
            var text = textBox轴.Text;
            var timeline = mParser.Run(text);
            if (timeline != null)
                Log(timeline.ToString());
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            mTimer.Dispose();
        }
    }
}
