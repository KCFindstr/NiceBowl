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
    public partial class Overlay : Form
    {
        public Overlay SetPosition(Rect rect)
        {
            Console.WriteLine("set location " + rect);
            Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            Top = rect.top;
            Left = rect.left;
            return this;
        }

        public Overlay()
        {
            InitializeComponent();
        }

        private void Overlay_Load(object sender, EventArgs e)
        {
            int wl = User32.GetWindowLong(Handle, User32.GWL.ExStyle);
            wl = wl | 0x80000 | 0x20;
            User32.SetWindowLong(Handle,User32.GWL.ExStyle, wl);
        }
    }
}
