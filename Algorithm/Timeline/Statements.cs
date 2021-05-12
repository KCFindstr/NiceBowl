using NiceBowl.Event;
using NiceBowl.Screen;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm.Timeline
{
    class CharaDefStmt : IStatement
    {
        public List<CharaInfo> charas;

        public void Run(EventContext context)
        {
            if (charas.Count > 5)
                charas.RemoveRange(5, charas.Count - 5);
            context.Charas = charas;
        }

        public override string ToString()
        {
            return $"定义角色：{string.Join(",", charas)}";
        }
    }

    class VersionStmt : IStatement
    {
        public AppVersion version;

        public void Run(EventContext context) { }

        public override string ToString()
        {
            return $"指定最低版本：{version}";
        }
    }

    class CharaUbStmt : IStatement
    {
        public CharaUbTime time;
        public IStrExpr action;

        public void Run(EventContext ctx)
        {
            ctx.AddEvent(new EventData(time.time, (data, context) =>
            {
                var chara = context.FindChara(action.Eval());
                if (chara == null)
                {
                    Main.Log($"找不到角色：{chara}");
                    return EventState.Stopped;
                }
                context.Window.Click(chara.pos.x, chara.pos.y);
                if (time.end == null || context.time <= time.end.Eval())
                {
                    return EventState.Stopped;
                }
                return EventState.Running;
            }));
        }

        public override string ToString()
        {
            return $"在{time}{(time.IsContinuous ?"连点" : "点击")}：{action}";
        }
    }

    class EmptyStmt : IStatement
    {
        public void Run(EventContext _) { }

        public override string ToString()
        {
            return $"空指令";
        }
    }

    class CommentStmt : EmptyStmt
    {
        public override string ToString()
        {
            return $"注释";
        }
    }
}
