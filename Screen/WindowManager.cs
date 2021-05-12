using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiceBowl.Screen
{
    class WindowManager
    {
        public Process Process { get; private set; }
        public IntPtr hWnd;

        public void SetPid(int pid)
        {
            try
            {
                Process = Process.GetProcessById(pid);
                hWnd = Process.MainWindowHandle;
            }
            catch (Exception e)
            {
                Process = null;
                Main.Log(e.Message);
            }
        }

        public void Click(int x, int y)
        {
            User32.SetForegroundWindow(hWnd);
            var window = GetWindowRect();
            MouseSimulator.ClickLeftMouseButton(x + window.left, y + window.top);
        }

        public Rect GetWindowRect()
        {
            Rect rect = new Rect();
            User32.GetWindowRect(hWnd, ref rect);
            return rect;
        }

        public Image CaptureScreen() => CaptureWindow(User32.GetDesktopWindow());

        public Image CaptureWindow()
        {
            return CaptureWindow(hWnd);
        }

        public Image CaptureWindow(int startX, int startY, int width, int height)
        {
            return CaptureWindow(hWnd, startX, startY, width, height);
        }

        private Image CaptureWindow(IntPtr handle)
        {
            IntPtr windowDc = User32.GetWindowDC(handle);
            Rect rect = new Rect();
            User32.GetWindowRect(handle, ref rect);
            int nWidth = rect.right - rect.left;
            int nHeight = rect.bottom - rect.top;
            IntPtr compatibleDc = GDI32.CreateCompatibleDC(windowDc);
            IntPtr compatibleBitmap = GDI32.CreateCompatibleBitmap(windowDc, nWidth, nHeight);
            IntPtr hObject = GDI32.SelectObject(compatibleDc, compatibleBitmap);
            GDI32.BitBlt(compatibleDc, 0, 0, nWidth, nHeight, windowDc, 0, 0, 13369376);
            GDI32.SelectObject(compatibleDc, hObject);
            GDI32.DeleteDC(compatibleDc);
            User32.ReleaseDC(handle, windowDc);
            Image image = Image.FromHbitmap(compatibleBitmap);
            GDI32.DeleteObject(compatibleBitmap);
            return image;
        }

        private Image CaptureWindow(IntPtr handle, int startX, int startY, int width, int height)
        {
            IntPtr windowDc = User32.GetWindowDC(handle);
            Rect rect = new Rect();
            User32.GetWindowRect(handle, ref rect);
            IntPtr compatibleDc = GDI32.CreateCompatibleDC(windowDc);
            IntPtr compatibleBitmap = GDI32.CreateCompatibleBitmap(windowDc, width, height);
            IntPtr hObject = GDI32.SelectObject(compatibleDc, compatibleBitmap);
            GDI32.BitBlt(compatibleDc, 0, 0, width, height, windowDc, startX, startY, 13369376);
            GDI32.SelectObject(compatibleDc, hObject);
            GDI32.DeleteDC(compatibleDc);
            User32.ReleaseDC(handle, windowDc);
            Image image = Image.FromHbitmap(compatibleBitmap);
            GDI32.DeleteObject(compatibleBitmap);
            return image;
        }
    }

    public class GDI32
    {
        public const int SRCCOPY = 13369376;

        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(
          IntPtr hObject,
          int nXDest,
          int nYDest,
          int nWidth,
          int nHeight,
          IntPtr hObjectSource,
          int nXSrc,
          int nYSrc,
          int dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }

    public class User32
    {
        public enum GWL
        {
            ExStyle = -20
        }

        public const uint WM_LBUTTONDOWN = 0x0201;
        public const uint WM_LBUTTONUP = 0x0202;

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(
          IntPtr hWnd,
          ref Rect rect);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);
    }
}
