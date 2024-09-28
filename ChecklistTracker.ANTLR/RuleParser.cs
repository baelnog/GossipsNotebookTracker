using Antlr4.Runtime;
using System.Collections.Concurrent;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChecklistTracker.ANTLR
{
    public class RuleParser
    {
        private static ConcurrentDictionary<string, ParserRuleContext> cache = new ConcurrentDictionary<string, ParserRuleContext>();

        public static ParserRuleContext Parse(string rule)
        {
            return cache.GetOrAdd(rule, (rule) =>
            {
                var stream = CharStreams.fromString(rule.Trim());

                var lexer = new Python3Lexer(stream);
                var tokens = new CommonTokenStream(lexer);
                var parser = new Python3Parser(tokens);


                var listener_lexer = new ConsoleErrorListener<int>();
                var listener_parser = new ConsoleErrorListener<IToken>();
                lexer.RemoveErrorListeners();
                parser.RemoveErrorListeners();
                lexer.AddErrorListener(listener_lexer);
                parser.AddErrorListener(listener_parser);

                var program = parser.expr_input().expr_stmt();
                //parser.
                //var tree = parser.statement();
                //var statementList = parser.statementList();



                return program;
            });
        }
    }
}
