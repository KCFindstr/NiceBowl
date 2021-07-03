using NiceBowl.Event;
using System.Collections.Generic;
using System.Text;

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
            IIntExpr refTime = null;
            if (time.isRelative) {
                refTime = ctx.Events.FindLast(e => !e.IsRelative).timeExpr;
                if (refTime == null) {
                    refTime = new ConstIntExpr(1000 * 91); // 1:31
                }
            }

            ctx.AddEvent(new EventData(time.time, (data, context) =>
            {
                var chara = context.FindChara(action.Eval());
                if (chara == null)
                {
                    Main.Log($"找不到角色：{chara}");
                    return EventState.Stopped;
                }
                context.Window.Click(chara.pos.x, chara.pos.y);
                return EventState.Running;
            }, time.end, refTime));
        }

        public override string ToString()
        {
            var timeStr = new StringBuilder().Append(time);
            if (time.isRelative) {
                timeStr.Append("后");
            }
            timeStr.Append(time.IsContinuous ? "连点" : "点击");
            return $"在{timeStr}：{action}";
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
