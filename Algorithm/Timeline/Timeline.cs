using NiceBowl.Event;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm.Timeline
{
    class Timeline
    {
        public List<IStatement> stmts;

        public Timeline(IEnumerable<IStatement> stmts)
        {
            this.stmts = stmts.ToList();
        }

        public Timeline Trim()
        {
            stmts = stmts.Where(stmt => !(stmt is EmptyStmt) && !(stmt is VersionStmt)).ToList();
            return this;
        }

        public override string ToString()
        {
            return string.Join("\r\n", stmts);
        }

        public void Run(EventContext context)
        {
            stmts.ForEach(stmt => stmt.Run(context));
        }
    }
}
