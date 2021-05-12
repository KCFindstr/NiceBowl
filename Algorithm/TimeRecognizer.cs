using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace NiceBowl.Algorithm
{
    class TimeRecognizer : MatcherBase<int>
    {
        private Bitmap mBitmap;
        private int[,] mLabels;
        private int mMinSize;
        private int mCurrentLabel;
        private Position mMaxSize;

        public static bool ValidPixel(Color color)
        {
            return color.R <= 190 && color.G <= 190 && color.B <= 190;
        }

        private void Prepare()
        {
            mBitmap = new Bitmap(mImage);
            mCurrentLabel = 1;
            mLabels = new int[width, height];
            mMinSize = height * 4 / 5;
            mMaxSize = GetPos(0.3, 0.6);
        }

        private void Shutdown()
        {
            mBitmap?.Dispose();
            mBitmap = null;
        }

        private List<MatcherStats> GetHoles(MatcherStats stats)
        {
            var ret = new List<MatcherStats>();
            foreach (var cur in stats.data)
            {
                foreach (var to in cur.Neighbors)
                {
                    if (IsOut(to) || mLabels[to.x, to.y] != 0
                        || !TimerMatcher.ValidPixel(mBitmap.GetPixel(to.x, to.y)))
                    {
                        continue;
                    }
                    var visited = BFS.Run(to, (f, t) =>
                    {
                        if (IsOut(t) || mLabels[t.x, t.y] != 0)
                        {
                            return false;
                        }
                        return TimerMatcher.ValidPixel(mBitmap.GetPixel(t.x, t.y));
                    });
                    mCurrentLabel++;
                    foreach (var p in visited)
                    {
                        mLabels[p.x, p.y] = -mCurrentLabel;
                    }
                    if (visited.Length <= tolerance)
                    {
                        continue;
                    }
                    ret.Add(new MatcherStats(visited));
                }
            }
            return ret;
        }

        public bool Is0(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count != 1)
                return false;
            var hole = holes[0];
            if (!Approximately(stats.center, hole.center))
                return false;
            if (hole.data.Length < tolerance * 4)
                return false;
            return true;
        }
            
        private bool Is1(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count > 0)
                return false;
            double ratio = (double)stats.rangeY / stats.rangeX;
            return ratio > 2.0;
        }

        private bool Is3(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count > 0)
                return false;
            var toMatchX = new int[] { 2, 3, 1, 2 };
            var toMatchY = new int[] { 1, 2, 1, 2, 1 };
            return Matches(toMatchX, stats.segCntX, 1) && Matches(toMatchY, stats.segCntY);
        }

        private bool Matches(IEnumerable<int> expected, IEnumerable<int> actual, int eps = -1)
        {
            if (eps == -1)
            {
                eps = tolerance / 2;
            }
            var list = new List<int>();
            var count = new List<int>();
            foreach (var val in actual)
            {
                if (list.Count == 0 || val != list[list.Count - 1])
                {
                    list.Add(val);
                    count.Add(1);
                }
                else
                {
                    count[count.Count - 1]++;
                }
            }
            var iterExpected = expected.GetEnumerator();
            iterExpected.MoveNext();
            for (int i = 0; i < list.Count; i++)
            {
                int val = list[i];
                if (iterExpected.Current == val)
                {
                    if (!iterExpected.MoveNext())
                    {
                        for (int j = i + 1; j < list.Count; j++)
                        {
                            eps -= count[j];
                        }
                        if (eps < 0)
                            return false;
                        break;
                    }
                }
                else
                {
                    eps -= count[i];
                    if (eps < 0)
                        return false;
                }
            }
            return !iterExpected.MoveNext();
        }

        private bool Is2(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count > 0)
                return false;
            var toMatchX = new int[] { 2, 3, 2 };
            var toMatchY = new int[] { 1, 2, 1 };
            return Matches(toMatchX, stats.segCntX) && Matches(toMatchY, stats.segCntY);
        }

        private bool Is4(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count != 1)
                return false;
            var hole = holes[0];
            if (hole.data.Length >= tolerance * 3)
                return false;
            var toMatchX = new int[] { 1, 2, 1 };
            var toMatchY = new int[] { 1, 2, 1 };
            return Matches(toMatchX, stats.segCntX) && Matches(toMatchY, stats.segCntY);
        }

        private bool Is5(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count > 0)
                return false;
            var toMatchX = new int[] { 2, 3, 2 };
            if (!Matches(toMatchX, stats.segCntX))
                return false;
            if (Math.Abs(stats.groupY[0].Count - stats.groupY[1].Count) > 2)
                return false;
            var lastRow = stats.groupY[stats.rangeY - 1];
            var secondRow = stats.groupY[stats.rangeY - 2];
            if (lastRow[0].x <= secondRow[0].x)
                return false;
            if (lastRow[lastRow.Count - 1].x >= secondRow[secondRow.Count - 1].x)
                return false;
            int limit = Math.Min(stats.rangeY - 1, tolerance);
            for (int i = 1; i <= limit; i++)
            {
                if (stats.groupY[i-1].Count > stats.groupY[i].Count * 2)
                    return true;
            }
            return false;
        }

        private bool Is6(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count != 1)
                return false;
            var hole = holes[0];
            int centerX = (stats.maxx.x + stats.minx.x) / 2;
            int centerY = (stats.maxy.y + stats.miny.y) / 2;
            if (hole.data.Length < tolerance * 3)
                return false;
            if (!Approximately(hole.center.x, centerX))
                return false;
            if (hole.center.y <= centerY)
                return false;
            return true;
        }

        private bool Is7(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count > 0)
                return false;
            int jumpIndex = -1;
            for (int i = 1; i < stats.rangeY; i++)
            {
                if (jumpIndex >= 0)
                {
                    if (Math.Abs(stats.groupY[i].Count - stats.groupY[i-1].Count) > tolerance / 2
                        && stats.groupY[i].Count >= tolerance && stats.groupY[i - 1].Count >= tolerance)
                        return false;
                    if (stats.groupY[jumpIndex - 1].Count <= stats.groupY[i].Count * 2)
                        return false;
                }
                else if (stats.groupY[i-1].Count > stats.groupY[i].Count * 2)
                {
                    jumpIndex = i;
                }
            }
            return jumpIndex >= 0;
        }

        private bool Is8(MatcherStats _, List<MatcherStats> holes)
        {
            if (holes.Count != 2)
                return false;
            var hole1 = holes[0];
            var hole2 = holes[1];
            if (hole1.center.y > hole2.center.y)
                (hole1, hole2) = (hole2, hole1);
            if (!Approximately(hole1.center.x, hole2.center.x))
                return false;
            if (hole2.data.Length <= hole1.data.Length)
                return false;
            if (hole2.data.Length >= hole1.data.Length * 2)
                return false;
            return true;
        }

        private bool Is9(MatcherStats stats, List<MatcherStats> holes)
        {
            if (holes.Count != 1)
                return false;
            var hole = holes[0];
            int centerX = (stats.maxx.x + stats.minx.x) / 2;
            int centerY = (stats.maxy.y + stats.miny.y) / 2;
            if (hole.data.Length < tolerance * 3)
                return false;
            if (!Approximately(hole.center.x, centerX))
                return false;
            if (hole.center.y >= centerY)
                return false;
            return true;
        }

        private int GetDigit(Position[] visited)
        {
            if (visited.Length < mMinSize)
            {
                return -1;
            }
            var stats = new MatcherStats(visited);
            if (stats.rangeX > mMaxSize.x || stats.rangeY > mMaxSize.y)
            {
                return -1;
            }
            var holes = GetHoles(stats);
            if (Is1(stats, holes))
                return 1;
            if (Is7(stats, holes))
                return 7;
            if (Is0(stats, holes))
                return 0;
            if (Is8(stats, holes))
                return 8;
            if (Is4(stats, holes))
                return 4;
            if (Is6(stats, holes))
                return 6;
            if (Is9(stats, holes))
                return 9;
            if (Is3(stats, holes))
                return 3;
            if (Is2(stats, holes))
                return 2;
            if (Is5(stats, holes))
                return 5;
            return -1;
        }

        public override int Match()
        {
            Prepare();
            int y = height >> 1;
            int result = 0;
            int cnt = 0;
            // Fill in blanks
            var visited = BFS.Run(new Position(0, y), (from, to) =>
            {
                if (IsOut(to))
                    return false;
                return TimerMatcher.ValidPixel(mBitmap.GetPixel(to.x, to.y));
            });
            if (width * height * 0.7 > visited.Length)
            {
                Shutdown();
                return -1;
            }
            visited = BFS.Run(new Position(0, y), (from, to) =>
            {
                if (IsOut(to) || mLabels[to.x, to.y] != 0)
                    return false;
                return !ValidPixel(mBitmap.GetPixel(to.x, to.y));
            });
            foreach (var p in visited)
            {
                mLabels[p.x, p.y] = -mCurrentLabel;
            }

            // Recognize digits
            for (int x = 0; x < width; x++)
            {
                if (mLabels[x, y] != 0 || !ValidPixel(mBitmap.GetPixel(x, y))) continue;
                visited = BFS.Run(new Position(x, y), (from, to) =>
                {
                    if (IsOut(to) || mLabels[to.x, to.y] != 0)
                        return false;
                    return ValidPixel(mBitmap.GetPixel(to.x, to.y));
                });
                mCurrentLabel++;
                foreach (var p in visited)
                {
                    mLabels[p.x, p.y] = mCurrentLabel;
                }
                int digit = GetDigit(visited);
                if (digit != -1)
                {
                    result = result * 10 + digit;
                    cnt++;
                }
            }
            Shutdown();
            return cnt == 3 ? result : -1;
        }
    }
}
