using NiceBowl.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm.Timeline
{
    interface IExpr<T>
    {
        T Eval();
    }

    interface IIntExpr : IExpr<long>
    {
        string ToTimeString();
    }
    interface IStrExpr : IExpr<string> { }

    interface IStatement
    {
        void Run(EventContext context);
    }
}
