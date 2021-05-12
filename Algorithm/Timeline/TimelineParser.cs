using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiceBowl.Algorithm.Timeline
{
    class TimelineParser
    {
        private static long TimeLiteralToLong(string mm, string ss, string ms)
        {
            long lmm = int.Parse(mm);
            long lss = int.Parse(ss);
            while (ms.Length < 3)
                ms += "0";
            ms = ms.Substring(0, 3);
            long lms = int.Parse(ms);
            return lmm * 60 * 1000 + lss * 1000 + lms;
        }

        private static AppVersion ValidateVersion(string input)
        {
            var version = new AppVersion(input);
            if (Main.version < version)
            {
                throw new NotSupportedException($"该轴要求{version}以上的蓝白碗");
            }
            return version;
        }

        private static readonly Parser<char> LINE_END = Parse.Chars("\n");
        private static readonly Parser<char> WHITE_SPACE = Parse.Chars(" \t\r");

        private static readonly Parser<AppVersion> VERSION_STR =
            from leading in WHITE_SPACE.Many()
            from v1 in Parse.Digit.AtLeastOnce().Text()
            from v2 in (
                from dot in Parse.Char('.')
                from num in Parse.Digit.AtLeastOnce().Text()
                select dot + num
                ).Many()
            select ValidateVersion(v1 + string.Join("", v2));

        private static readonly Parser<long> TimeLiteralWithMM =
            from leading in WHITE_SPACE.Many()
            from mm in Parse.Digit.AtLeastOnce().Text()
            from _1 in Parse.Char(':')
            from ss in Parse.Digit.AtLeastOnce().Text()
            from _2 in Parse.Char('.')
            from ms in Parse.Digit.Many().Text()
            select TimeLiteralToLong(mm, ss, ms);

        private static readonly Parser<long> TimeLiteral =
            from leading in WHITE_SPACE.Many()
            from ss in Parse.Digit.AtLeastOnce().Text()
            from _2 in Parse.Char('.')
            from ms in Parse.Digit.Many().Text()
            select TimeLiteralToLong("0", ss, ms);

        private static readonly Parser<string> StrLiteral =
            from leading in WHITE_SPACE.Many()
            from str in Parse.CharExcept("\r\n\t =.[]<>&|^#$~!+-*/\\").AtLeastOnce().Text()
            select str;

        private static readonly Parser<IIntExpr> IntExpr =
            from time in TimeLiteralWithMM.Or(TimeLiteral)
            select new ConstIntExpr(time);

        private static readonly Parser<IStrExpr> StrExpr =
            from str in StrLiteral
            select new ConstStrExpr(str);

        private static readonly Parser<IStrExpr> UbAction = StrExpr;

        private static readonly Parser<CharaUbTime> UbTimeWithCont =
            from start in IntExpr
            from _1 in WHITE_SPACE.Many()
            from _2 in Parse.Char('~')
            from end in IntExpr
            select new CharaUbTime { time = start, end = end };

        private static readonly Parser<CharaUbTime> UbTime =
            from start in IntExpr
            select new CharaUbTime { time = start, end = null };

        private static readonly Parser<IStatement> UbStatement =
            from time in UbTimeWithCont.Or(UbTime)
            from action in UbAction
            from _1 in WHITE_SPACE.Many()
            from _2 in LINE_END
            select new CharaUbStmt
            {
                time = time,
                action = action
            };

        private static readonly Parser<IStatement> VersionStatement =
            from _1 in Parse.String("ver")
            from ver in VERSION_STR
            from _2 in WHITE_SPACE.Many()
            from _3 in LINE_END
            select new VersionStmt { version = ver };

        private static readonly Parser<IStatement> CommentStatement =
            from _1 in Parse.String("//")
            from _2 in Parse.CharExcept("\n").Many()
            from _3 in LINE_END
            select new CommentStmt();

        private static readonly Parser<CharaInfo> CharaDesc =
            from name in StrLiteral
            select new CharaInfo { name = name };

        private static readonly Parser<IStatement> CharaDefStatement =
            from _1 in Parse.String("charas")
            from charas in CharaDesc.Many()
            from _2 in WHITE_SPACE.Many()
            from _3 in LINE_END
            select new CharaDefStmt { charas = charas.ToList() };

        private static readonly Parser<IStatement> EmptyStatement =
            from _1 in WHITE_SPACE.Many()
            from _2 in LINE_END
            select new EmptyStmt();

        private static readonly Parser<IStatement> AnyStatement =
            from stmt in CharaDefStatement
                .Or(VersionStatement)
                .Or(UbStatement)
                .Or(CommentStatement)
                .Or(EmptyStatement)
            select stmt;

        private static readonly Parser<Timeline> Program =
            from stmts in AnyStatement.Many().End()
            select new Timeline(stmts);

        public Timeline Run(string input)
        {
            try
            {
                var ret = Program.Parse(input + "\n").Trim();
                Main.Log("成功解析轴");
                return ret;
            }
            catch (Exception e)
            {
                Main.Log("解析轴失败：" + e.Message);
                return null;
            }
        }
    }
}
