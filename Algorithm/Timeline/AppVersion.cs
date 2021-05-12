using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm.Timeline
{
    public class AppVersion
    {
        public readonly List<int> version;

        public AppVersion(string version)
        {
            this.version = version.Split('.').Select(num => int.Parse(num)).ToList();
            if (this.version.Count > 4)
                this.version.RemoveRange(4, this.version.Count - 4);
            while (this.version.Count < 4)
                this.version.Add(0);
        }

        public ulong ToNumber()
        {
            ulong ret = 0;
            foreach (int val in version) {
                ret = (ret << 8) | (uint)val;
            }
            return ret;
        }

        public override bool Equals(object obj)
        {
            if (obj is AppVersion rhs)
                return this == rhs;
            return false;
        }

        public override int GetHashCode()
        {
            return (int)ToNumber();
        }

        public override string ToString()
        {
            return string.Join(".", version);
        }

        public static bool operator > (AppVersion lhs, AppVersion rhs)
        {
            return lhs.ToNumber() > rhs.ToNumber();
        }

        public static bool operator >= (AppVersion lhs, AppVersion rhs)
        {
            return lhs.ToNumber() >= rhs.ToNumber();
        }

        public static bool operator < (AppVersion lhs, AppVersion rhs)
        {
            return lhs.ToNumber() < rhs.ToNumber();
        }

        public static bool operator <= (AppVersion lhs, AppVersion rhs)
        {
            return lhs.ToNumber() <= rhs.ToNumber();
        }

        public static bool operator == (AppVersion lhs, AppVersion rhs)
        {
            return lhs.ToNumber() == rhs.ToNumber();
        }

        public static bool operator != (AppVersion lhs, AppVersion rhs)
        {
            return lhs.ToNumber() != rhs.ToNumber();
        }
    }
}
