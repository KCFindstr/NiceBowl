using NiceBowl.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm.Timeline
{
    class ConstIntExpr : IIntExpr
    {
        public readonly long value;

        public ConstIntExpr(long value)
        {
            this.value = value;
        }

        public long Eval()
        {
            return value;
        }

        public string ToTimeString()
        {
            return Util.MsToString(value);
        }

        public static ConstIntExpr Min => new ConstIntExpr(long.MinValue);
    }

    class ConstStrExpr : IStrExpr
    {
        public string value;

        public ConstStrExpr(string value)
        {
            this.value = value;
        }


        public string Eval()
        {
            return value;
        }

        public override string ToString()
        {
            return value;
        }
    }
}
