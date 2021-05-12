using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm
{
    struct Position : IEquatable<Position>
    {
        public int x;
        public int y;

        public Position(int x = 0, int y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public bool Valid => this != MinValue && this != MaxValue;

        public IEnumerable<Position> Neighbors
        {
            get
            {
                foreach (var dir in Directions)
                {
                    yield return this + dir;
                }
            }
        }

        public static Position operator + (Position lhs, Position rhs)
        {
            return new Position(lhs.x + rhs.x, lhs.y + rhs.y);
        }

        public static Position operator - (Position lhs, Position rhs)
        {
            return new Position(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static Position operator * (Position lhs, double scalar)
        {
            return new Position((int)(lhs.x * scalar), (int)(lhs.y * scalar));
        }

        public static bool operator != (Position lhs, Position rhs)
        {
            return lhs.x != rhs.x || lhs.y != rhs.y;
        }

        public static bool operator == (Position lhs, Position rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Position rhs)
            {
                return this == rhs;
            }
            return false;
        }

        public override string ToString()
        {
            return $"({x},{y})";
        }

        public override int GetHashCode()
        {
            return (x << 16) | y;
        }

        public bool Equals(Position other)
        {
            return this == other;
        }

        public static readonly Position Up = new Position(0, -1);
        public static readonly Position Down = new Position(0, 1);
        public static readonly Position Left = new Position(-1, 0);
        public static readonly Position Right = new Position(1, 0);
        public static readonly Position MinValue = new Position(int.MinValue, int.MinValue);
        public static readonly Position MaxValue = new Position(int.MaxValue, int.MaxValue);
        public static readonly Position[] Directions = new Position[] { Up, Down, Left, Right };
    }

    static class BFS
    {
        public delegate bool EdgeOracle(Position from, Position To);

        public static Position[] Run(Position start, EdgeOracle edgeOracle)
        {
            var queue = new Queue<Position>();
            queue.Enqueue(start);
            var visited = new HashSet<Position>
            {
                start
            };
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                foreach (var next in current.Neighbors)
                {
                    if (visited.Contains(next) || !edgeOracle(current, next))
                    {
                        continue;
                    }
                    visited.Add(next);
                    queue.Enqueue(next);
                }
            }
            return visited.ToArray();
        }
    }
}
