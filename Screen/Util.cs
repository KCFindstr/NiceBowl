using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Screen
{
    public struct Rect
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public override string ToString()
        {
            return $"{left} {top} {right} {bottom}";
        }
    }

    static class Util
    {
        public static Image DownSample(Image image, int maxWidth, int maxHeight)
        {
            int scaleX = (int)Math.Ceiling((double)image.Width / maxWidth);
            int scaleY = (int)Math.Ceiling((double)image.Height / maxHeight);
            int scale = Math.Max(scaleX, scaleY);
            
            if (scale <= 1)
            {
                return image;
            }

            int width = image.Width / scale;
            int height = image.Height / scale;

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static string MsToString(long ms)
        {
            int s = (int)(ms / 1000);
            ms %= 1000;
            ms /= 10;
            int m = s / 60;
            s %= 60;
            return $"{m}:{s:D2}.{ms:D2}";
        }
    }
}
