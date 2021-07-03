using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NiceBowl.Algorithm
{
    class MatcherStats
    {
        public Position[] data { get; private set; }
        public Position minx = Position.MaxValue;
        public Position miny = Position.MaxValue;
        public Position maxx = Position.MinValue;
        public Position maxy = Position.MinValue;
        public Position center = Position.MinValue;

        public int rangeX;
        public int rangeY;

        public List<List<Position>> groupX;
        public List<List<Position>> groupY;
        public List<int> segCntX;
        public List<int> segCntY;
        public List<double> distX;
        public List<double> distY;

        public void Reset()
        {
            minx = Position.MaxValue;
            miny = Position.MaxValue;
            maxx = Position.MinValue;
            maxy = Position.MinValue;
            center = Position.MinValue;
            rangeX = 0;
            rangeY = 0;
            segCntX = new List<int>();
            segCntY = new List<int>();
            groupX = new List<List<Position>>();
            groupY = new List<List<Position>>();
            segCntX = new List<int>();
            segCntY = new List<int>();
            distX = new List<double>();
            distY = new List<double>();
        }

        public void TrimX()
        {
            var max = distX.Max();
            double threshold = max * 0.4;
            int startIndex = 0;
            while (startIndex < distX.Count && distX[startIndex] < threshold)
            {
                startIndex++;
            }
            int endIndex = distX.Count;
            while (endIndex > 0 && distX[endIndex - 1] < threshold)
            {
                endIndex--;
            }
            startIndex += minx.x;
            endIndex += minx.x;
            data = data.Where((pos) => pos.x >= startIndex && pos.x < endIndex).ToArray();
        }

        public void TrimY()
        {
            var max = distY.Max();
            double threshold = max * 0.4;
            int startIndex = 0;
            while (startIndex < distY.Count && distY[startIndex] < threshold)
            {
                startIndex++;
            }
            int endIndex = distY.Count;
            while (endIndex > 0 && distY[endIndex - 1] < threshold)
            {
                endIndex--;
            }
            startIndex += miny.y;
            endIndex += miny.y;
            data = data.Where((pos) => pos.y >= startIndex && pos.y < endIndex).ToArray();
        }

        public void Calc()
        {
            Reset();
            if (data == null || data.Length == 0)
            {
                return;
            }
            foreach (var p in data)
            {
                if (p.x > maxx.x) maxx = p;
                if (p.y > maxy.y) maxy = p;
                if (p.x < minx.x) minx = p;
                if (p.y < miny.y) miny = p;
            }
            rangeX = maxx.x - minx.x + 1;
            rangeY = maxy.y - miny.y + 1;
            for (int i = 0; i < rangeX; i++)
            {
                groupX.Add(new List<Position>());
            }
            for (int i = 0; i < rangeY; i++)
            {
                groupY.Add(new List<Position>());
            }
            distX.AddRange(Enumerable.Repeat(0.0, rangeX));
            distY.AddRange(Enumerable.Repeat(0.0, rangeY));
            foreach (var p in data)
            {
                groupX[p.x - minx.x].Add(p);
                groupY[p.y - miny.y].Add(p);
            }
            groupX.ForEach(g => g.Sort((a, b) => a.y - b.y));
            groupY.ForEach(g => g.Sort((a, b) => a.x - b.x));

            for (int i = 0; i < rangeX; i++)
            {
                var list = groupX[i];
                int segCnt = 1;
                for (int j = 1; j < list.Count; j++)
                {
                    if (list[j].y - list[j-1].y > 1)
                    {
                        segCnt++;
                    }
                }
                segCntX.Add(segCnt);
            }
            for (int i = 0; i < rangeY; i++)
            {
                var list = groupY[i];
                int segCnt = 1;
                for (int j = 1; j < list.Count; j++)
                {
                    if (list[j].x - list[j - 1].x > 1)
                    {
                        segCnt++;
                    }
                }
                segCntY.Add(segCnt);
            }

            int countSum = 0;
            for (int i = 0; i < rangeX; i++)
            {
                if (countSum <= data.Length / 2 &&
                    (countSum + groupX[i].Count > data.Length / 2))
                {
                    center.x = minx.x + i;
                }
                countSum += groupX[i].Count;
                distX[i] = (double)groupX[i].Count / data.Length;
            }
            countSum = 0;
            for (int i = 0; i < rangeY; i++)
            {
                if (countSum <= data.Length / 2 &&
                    (countSum + groupY[i].Count > data.Length / 2))
                {
                    center.y = miny.y + i;
                }
                countSum += groupY[i].Count;
                distY[i] = (double)groupY[i].Count / data.Length;
            }
        }

        public MatcherStats(Position[] list)
        {
            data = list;
            Calc();
        }

        public Rect ToRect()
        {
            return new Rect
            {
                left = minx.x,
                top = miny.y,
                right = maxx.x - 1,
                bottom = maxy.y - 1
            };
        }

        public override string ToString()
        {
            return $"Max X: {maxx}\n" +
                $"Max Y: {maxy}\n" +
                $"Min X: {minx}\n" +
                $"Min Y: {miny}\n" +
                $"Count X ({rangeX}): {string.Join(", ", groupX.Select(g => g.Count))}\n" +
                $"Count Y ({rangeY}): {string.Join(", ", groupY.Select(g => g.Count))}\n"
            ;
        }
    }

    abstract class MatcherBase<T>
    {
        public const double EPS = 1e-4;

        protected Image mImage;
        protected int width;
        protected int height;
        protected int tolerance;

        protected bool Approximately(double x, double y, double eps = EPS)
        {
            return Math.Abs(x - y) <= eps;
        }

        protected bool Approximately(int x, int y, int eps = -1)
        {
            if (eps < 0)
            {
                eps = Math.Max(2, tolerance / 2);
            }
            return Math.Abs(x - y) < eps;
        }

        protected bool Approximately(Position x, Position y, int eps = -1)
        {
            return Approximately(x.x, y.x, eps) && Approximately(x.y, y.y, eps);
        }

        protected int GetLength(double normalized)
        {
            return (int)Math.Round(normalized * (width + height) / 2);
        }

        protected Position GetPos(double x, double y)
        {
            return new Position((int)(x * width), (int)(y * height));
        }

        protected bool IsOut(Position pos)
        {
            return pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height;
        }

        public MatcherBase<T> SetImage(Image image)
        {
            mImage = image;
            width = image.Width;
            height = image.Height;
            tolerance = Util.Clamp((int)Math.Ceiling(height / 10.0), 2, 6);
            return this;
        }

        abstract public T Match();
    }
}
