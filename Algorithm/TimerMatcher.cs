using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rect = NiceBowl.Screen.Rect;

namespace NiceBowl.Algorithm
{
    class TimerMatcher: MatcherBase<Rect[]>
    {
        private Position minSize;

        private MatcherStats Validate(Position[] visited)
        {
            var stats = new MatcherStats(visited);
            stats.TrimX();
            stats.TrimY();
            stats.Calc();
            if (stats.rangeX <= minSize.x || stats.rangeY <= minSize.y)
            {
                return null;
            }
            double ratio = (double)stats.rangeX / stats.rangeY;
            if (!Approximately(ratio, 6, 1))
            {
                return null;
            }
            double minRatio = stats.distY.Min();
            double maxRatio = stats.distY.Max();
            if (maxRatio / minRatio >= 1.5)
            {
                return null;
            }
            return stats;
        }

        public static bool ValidPixel(Color color)
        {
            return color.R >= 220 && color.G >= 220 && color.B >= 220;
        }

        public override Rect[] Match()
        {
            minSize = GetPos(0.3, 0.2);
            var globalVisited = new HashSet<Position>();
            var ret = new List<Rect>();
            using (var bitmap = new Bitmap(mImage))
            {
                var delta = GetPos(0.4, 0.1);
                var x = delta.x;
                for (int j = 0; j < height; j += delta.y)
                {
                    var pos = new Position(x, j);
                    if (globalVisited.Contains(pos) || !ValidPixel(bitmap.GetPixel(x, j)))
                    {
                        continue;
                    }
                    var visited = BFS.Run(pos, (from, to) =>
                    {
                        if (IsOut(to) || globalVisited.Contains(to))
                        {
                            return false;
                        }
                        var color = bitmap.GetPixel(to.x, to.y);
                        return ValidPixel(color);
                    });
                    foreach (var p in visited)
                    {
                        globalVisited.Add(p);
                    }
                    var stats = Validate(visited);
                    if (stats != null)
                    {
                        ret.Add(stats.ToRect());
                    }
                }
            }
            return ret.ToArray();
        }
    }
}
