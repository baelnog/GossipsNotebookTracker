using Antlr4.Runtime.Tree;
using ChecklistTracker.ANTLR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static ChecklistTracker.ANTLR.Python3Parser;

namespace ChecklistTracker.LogicProvider
{
    internal partial class LogicHelpers : IPython3ParserVisitor<bool>
    {
        public bool VisitExpr_stmt([Antlr4.Runtime.Misc.NotNull] Expr_stmtContext context)
        {
            /*
            : testlist_star_expr (
                annassign // No assignments in logic
                | augassign (yield_expr | testlist) // No assignments in logic
                | ('=' (yield_expr | testlist_star_expr))* // No assignments in logic
            )
            */
            var testlist_star_expr = context.testlist_star_expr();
            if (testlist_star_expr.Length > 1 || context.annassign() != null || context.augassign() != null || (context.yield_expr()?.Any() ?? false))
            {
                throw new NotImplementedException($"Unexpected complexe expr_stmt {context.GetText()}");
            }
            return testlist_star_expr[0].Accept(this);
        }

        public bool VisitTestlist_star_expr([Antlr4.Runtime.Misc.NotNull] Testlist_star_exprContext context)
        {
            // (test | star_expr) (',' (test | star_expr))* ','?
            // no star_expr expected.
            var tests = context.test();
            if (tests.Length > 1 || context.star_expr().Any())
            {
                throw new NotImplementedException($"Unexpected complex testlist_star_expr {context.GetText()}");
            }
            return tests[0].Accept(this);
        }

        public bool VisitTest([Antlr4.Runtime.Misc.NotNull] TestContext context)
        {
            // : or_test ('if' or_test 'else' test) ? // just the one or_test
            // | lambdef // lol no
            var or_test = context.or_test();

            if (or_test.Length > 1)
            {
                throw new NotImplementedException($"Unexpectd if/else test {context.GetText()}");
            }

            var result = or_test[0].Accept(this);
            return result;
        }

        public bool VisitOr_test([Antlr4.Runtime.Misc.NotNull] Or_testContext context)
        {
            // : and_test ('or' and_test)*
            var result = false;
            foreach (var child in context.and_test())
            {
                // TODO: add switch to early return
                result |= child.Accept(this);
                if (result) return true;
            }
            return result;
        }

        public bool VisitAnd_test([Antlr4.Runtime.Misc.NotNull] And_testContext context) 
        {
            // not_test ('and' not_test)*
            var result = true;
            foreach (var child in context.not_test())
            {
                // TODO: add switch to early return
                result &= child.Accept(this);
                if (!result) return false;
            }
            return result;
        }

        public bool VisitNot_test([Antlr4.Runtime.Misc.NotNull] Not_testContext context)
        {
            // 'not' not_test
            if (context.NOT() != null)
            {
                return !context.not_test().Accept(this);
            }

            // | comparison
            var result = context.comparison().Accept(this);
            return result;
        }

        public bool VisitComparison([Antlr4.Runtime.Misc.NotNull] ComparisonContext context)
        {
            //     : expr (comp_op expr)*
            var exprs = context.expr();
            if (exprs.Length == 1)
            {
                var result = exprs[0].Accept(this);
                return result;
            }
            if (exprs.Length == 2)
            {
                var left = exprs[0];
                var right = exprs[1];
                // Join with spaces to handle 'not in' and 'is not'
                var comps = string.Join(" ", context.comp_op().Select(comp_op => comp_op.GetText()));
                var result = EvalBinaryComparison(left.GetText(), right.GetText(), context.comp_op()[0].GetText(), VisitingAge.Peek());
                return result;

            }
            // Logic doesn't leverage chained comparisons atm, so don't worry about them.
            throw new NotImplementedException($"Comparison with {exprs.Length} elements, {context.GetText}");
        }

        public bool VisitExpr([Antlr4.Runtime.Misc.NotNull] ExprContext context)
        {
            /*
             * expr
                : atom_expr
                | expr '**' expr
                | ('+' | '-' | '~')+ expr
                | expr ('*' | '@' | '/' | '%' | '//') expr
                | expr ('+' | '-') expr
                | expr ('<<' | '>>') expr
                | expr '&' expr
                | expr '^' expr
                | expr '|' expr
                ;
             */
            if (context.atom_expr() is Atom_exprContext atom_expr)
            {
                return atom_expr.Accept(this);
            }
            
            // Logic *could* use &, ^, and |, but they don't currently, so don't guess at impl.

            throw new NotImplementedException($"Unsupported expr {context.GetText()}");
        }

        public bool VisitComp_op([Antlr4.Runtime.Misc.NotNull] Comp_opContext context)
        {
            // Being handled directly in VisitComparison
            throw new NotImplementedException();
        }

        public bool VisitTestlist_comp([Antlr4.Runtime.Misc.NotNull] Python3Parser.Testlist_compContext context)
        {
            // (test | star_expr) (comp_for | (',' (test | star_expr))* ','?)

            // star_expr is not used by logic, so simplify to:
            // (test) (comp_for (',' (test))* ','?)
            if (context.test() is TestContext[] tests)
            {
                if (tests.Length == 1)
                {
                    var result = tests[0].Accept(this);
                    return result;
                }
                if (tests.Length == 2)
                {
                    var result = EvalCountCheck(tests[0].GetText(), tests[1].GetText());
                    return result;
                }
                throw new NotImplementedException($"TestList with [{tests.Length}] tests {context.GetText()}");
            }
            if (context.comp_for() is Comp_forContext compFor)
            {
                throw new NotImplementedException($"Unexpected for expression {context.GetText}");
            }
            throw new NotImplementedException();
        }

        public bool VisitAtom([Antlr4.Runtime.Misc.NotNull] AtomContext context)
        {
            /*
                atom
                    : '(' (yield_expr | testlist_comp)? ')'
                    | '[' testlist_comp? ']'
                    | '{' dictorsetmaker? '}'
                    | name
                    | NUMBER
                    | STRING+
                    | '...'
                    | 'None'
                    | 'True'
                    | 'False'
                    ;
             */

            if (context.yield_expr() != null)
            {
                throw new NotImplementedException($"Unexpected yield_expr {context.GetText()}");
            }

            if (context.dictorsetmaker() != null)
            {
                throw new NotImplementedException($"Unexpected dictorsetmaker {context.GetText()}");
            }

            if (context.OPEN_BRACE() != null || context.OPEN_BRACK() != null)
            {
                throw new NotImplementedException($"Unexpected brace/bracket {context.GetText()}");
            }

            if (context.testlist_comp() is Testlist_compContext testlist_comp)
            {
                var res = testlist_comp.Accept(this);
                return res;
            }

            var result = EvalIdentifier(context.GetText(), VisitingAge.Peek());
            return result;
        }

        public bool VisitAtom_expr([Antlr4.Runtime.Misc.NotNull] Atom_exprContext context)
        {
            var trailer = context.trailer();
            if (!trailer.Any())
            {
                var result = context.atom().Accept(this);
                return result;
            }

            if (trailer.Length == 1)
            {
                var args = trailer[0].arglist();
                var subscript = trailer[0].subscriptlist();
                var atom = context.atom();
                var name = atom.GetText();

                if (args != null)
                {
                    var result = EvalCall(name, args.argument());
                    return result;
                }
                else if (subscript != null)
                {
                    var result = EvalLookup(name, subscript.subscript_());
                    return result;
                }
            }

            throw new NotImplementedException();
        }

        public bool VisitArgument([Antlr4.Runtime.Misc.NotNull] ArgumentContext context)
        {
            // (test comp_for? | test '=' test | '**' test | '*' test)
            // We only need to handle the raw test.
            if (context.comp_for() != null || context.ASSIGN() != null || context.POWER() != null || context.STAR() != null)
            {
                throw new NotImplementedException($"Unexpected Argument format {context.GetText()}");
            }

            return context.test()[0].Accept(this);
        }

    }
}
