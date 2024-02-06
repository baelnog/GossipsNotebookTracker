using Antlr4.Runtime.Tree;
using ChecklistTracker.ANTLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChecklistTracker.ANTLR.Python3Parser;
using static System.Net.Mime.MediaTypeNames;

namespace ChecklistTracker.LogicProvider
{
    /// <summary>
    /// NIE implementations of visitor functions we don't need to handle.
    /// There is a Python3ParserBaseVisitor class that also provides default impls.
    /// The default implementation often doesn't work the way we want though so it is safer to just
    /// explicitly throw on unexpected code paths.
    /// </summary>
    internal partial class LogicHelpers : IPython3ParserVisitor<Accessibility>
    {
        public Accessibility VisitSingle_input([Antlr4.Runtime.Misc.NotNull] Single_inputContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitFile_input([Antlr4.Runtime.Misc.NotNull] File_inputContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitEval_input([Antlr4.Runtime.Misc.NotNull] Eval_inputContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitExpr_input([Antlr4.Runtime.Misc.NotNull] Expr_inputContext context)
        {
            return context.expr_stmt().Accept(this);
        }

        public Accessibility VisitDecorator([Antlr4.Runtime.Misc.NotNull] DecoratorContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDecorators([Antlr4.Runtime.Misc.NotNull] DecoratorsContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDecorated([Antlr4.Runtime.Misc.NotNull] DecoratedContext context)
        {
            throw new NotImplementedException();
        }
        public Accessibility VisitAsync_funcdef([Antlr4.Runtime.Misc.NotNull] Async_funcdefContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitFuncdef([Antlr4.Runtime.Misc.NotNull] FuncdefContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitParameters([Antlr4.Runtime.Misc.NotNull] ParametersContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitTypedargslist([Antlr4.Runtime.Misc.NotNull] TypedargslistContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitTfpdef([Antlr4.Runtime.Misc.NotNull] TfpdefContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitVarargslist([Antlr4.Runtime.Misc.NotNull] VarargslistContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitVfpdef([Antlr4.Runtime.Misc.NotNull] VfpdefContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitStmt([Antlr4.Runtime.Misc.NotNull] StmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSimple_stmts([Antlr4.Runtime.Misc.NotNull] Simple_stmtsContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSimple_stmt([Antlr4.Runtime.Misc.NotNull] Simple_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitAnnassign([Antlr4.Runtime.Misc.NotNull] AnnassignContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitAugassign([Antlr4.Runtime.Misc.NotNull] AugassignContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDel_stmt([Antlr4.Runtime.Misc.NotNull] Del_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitPass_stmt([Antlr4.Runtime.Misc.NotNull] Pass_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitFlow_stmt([Antlr4.Runtime.Misc.NotNull] Flow_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitBreak_stmt([Antlr4.Runtime.Misc.NotNull] Break_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitContinue_stmt([Antlr4.Runtime.Misc.NotNull] Continue_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitReturn_stmt([Antlr4.Runtime.Misc.NotNull] Return_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitYield_stmt([Antlr4.Runtime.Misc.NotNull] Yield_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitRaise_stmt([Antlr4.Runtime.Misc.NotNull] Raise_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitImport_stmt([Antlr4.Runtime.Misc.NotNull] Import_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitImport_name([Antlr4.Runtime.Misc.NotNull] Import_nameContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitImport_from([Antlr4.Runtime.Misc.NotNull] Import_fromContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitImport_as_name([Antlr4.Runtime.Misc.NotNull] Import_as_nameContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDotted_as_name([Antlr4.Runtime.Misc.NotNull] Dotted_as_nameContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitImport_as_names([Antlr4.Runtime.Misc.NotNull] Import_as_namesContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDotted_as_names([Antlr4.Runtime.Misc.NotNull] Dotted_as_namesContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDotted_name([Antlr4.Runtime.Misc.NotNull] Dotted_nameContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitGlobal_stmt([Antlr4.Runtime.Misc.NotNull] Global_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitNonlocal_stmt([Antlr4.Runtime.Misc.NotNull] Nonlocal_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitAssert_stmt([Antlr4.Runtime.Misc.NotNull] Assert_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitCompound_stmt([Antlr4.Runtime.Misc.NotNull] Compound_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitAsync_stmt([Antlr4.Runtime.Misc.NotNull] Async_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitIf_stmt([Antlr4.Runtime.Misc.NotNull] If_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitWhile_stmt([Antlr4.Runtime.Misc.NotNull] While_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitFor_stmt([Antlr4.Runtime.Misc.NotNull] For_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitTry_stmt([Antlr4.Runtime.Misc.NotNull] Try_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitWith_stmt([Antlr4.Runtime.Misc.NotNull] With_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitWith_item([Antlr4.Runtime.Misc.NotNull] With_itemContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitExcept_clause([Antlr4.Runtime.Misc.NotNull] Except_clauseContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitBlock([Antlr4.Runtime.Misc.NotNull] BlockContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitMatch_stmt([Antlr4.Runtime.Misc.NotNull] Match_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSubject_expr([Antlr4.Runtime.Misc.NotNull] Subject_exprContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitStar_named_expressions([Antlr4.Runtime.Misc.NotNull] Star_named_expressionsContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitStar_named_expression([Antlr4.Runtime.Misc.NotNull] Star_named_expressionContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitCase_block([Antlr4.Runtime.Misc.NotNull] Case_blockContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitGuard([Antlr4.Runtime.Misc.NotNull] GuardContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitPatterns([Antlr4.Runtime.Misc.NotNull] PatternsContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitPattern([Antlr4.Runtime.Misc.NotNull] PatternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitAs_pattern([Antlr4.Runtime.Misc.NotNull] As_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitOr_pattern([Antlr4.Runtime.Misc.NotNull] Or_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitClosed_pattern([Antlr4.Runtime.Misc.NotNull] Closed_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitLiteral_pattern([Antlr4.Runtime.Misc.NotNull] Literal_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitLiteral_expr([Antlr4.Runtime.Misc.NotNull] Literal_exprContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitComplex_number([Antlr4.Runtime.Misc.NotNull] Complex_numberContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSigned_number([Antlr4.Runtime.Misc.NotNull] Signed_numberContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSigned_real_number([Antlr4.Runtime.Misc.NotNull] Signed_real_numberContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitReal_number([Antlr4.Runtime.Misc.NotNull] Real_numberContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitImaginary_number([Antlr4.Runtime.Misc.NotNull] Imaginary_numberContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitCapture_pattern([Antlr4.Runtime.Misc.NotNull] Capture_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitPattern_capture_target([Antlr4.Runtime.Misc.NotNull] Pattern_capture_targetContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitWildcard_pattern([Antlr4.Runtime.Misc.NotNull] Wildcard_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitValue_pattern([Antlr4.Runtime.Misc.NotNull] Value_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitAttr([Antlr4.Runtime.Misc.NotNull] AttrContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitName_or_attr([Antlr4.Runtime.Misc.NotNull] Name_or_attrContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitGroup_pattern([Antlr4.Runtime.Misc.NotNull] Group_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSequence_pattern([Antlr4.Runtime.Misc.NotNull] Sequence_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitOpen_sequence_pattern([Antlr4.Runtime.Misc.NotNull] Open_sequence_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitMaybe_sequence_pattern([Antlr4.Runtime.Misc.NotNull] Maybe_sequence_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitMaybe_star_pattern([Antlr4.Runtime.Misc.NotNull] Maybe_star_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitStar_pattern([Antlr4.Runtime.Misc.NotNull] Star_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitMapping_pattern([Antlr4.Runtime.Misc.NotNull] Mapping_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitItems_pattern([Antlr4.Runtime.Misc.NotNull] Items_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitKey_value_pattern([Antlr4.Runtime.Misc.NotNull] Key_value_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDouble_star_pattern([Antlr4.Runtime.Misc.NotNull] Double_star_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitClass_pattern([Antlr4.Runtime.Misc.NotNull] Class_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitPositional_patterns([Antlr4.Runtime.Misc.NotNull] Positional_patternsContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitKeyword_patterns([Antlr4.Runtime.Misc.NotNull] Keyword_patternsContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitKeyword_pattern([Antlr4.Runtime.Misc.NotNull] Keyword_patternContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitTest_nocond([Antlr4.Runtime.Misc.NotNull] Test_nocondContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitLambdef([Antlr4.Runtime.Misc.NotNull] LambdefContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitLambdef_nocond([Antlr4.Runtime.Misc.NotNull] Lambdef_nocondContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitStar_expr([Antlr4.Runtime.Misc.NotNull] Star_exprContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitName([Antlr4.Runtime.Misc.NotNull] NameContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitTrailer([Antlr4.Runtime.Misc.NotNull] TrailerContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSubscriptlist([Antlr4.Runtime.Misc.NotNull] SubscriptlistContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSubscript_([Antlr4.Runtime.Misc.NotNull] Subscript_Context context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitSliceop([Antlr4.Runtime.Misc.NotNull] SliceopContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitExprlist([Antlr4.Runtime.Misc.NotNull] ExprlistContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitTestlist([Antlr4.Runtime.Misc.NotNull] TestlistContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitDictorsetmaker([Antlr4.Runtime.Misc.NotNull] DictorsetmakerContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitClassdef([Antlr4.Runtime.Misc.NotNull] ClassdefContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitArglist([Antlr4.Runtime.Misc.NotNull] ArglistContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitComp_iter([Antlr4.Runtime.Misc.NotNull] Comp_iterContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitComp_for([Antlr4.Runtime.Misc.NotNull] Comp_forContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitComp_if([Antlr4.Runtime.Misc.NotNull] Comp_ifContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitEncoding_decl([Antlr4.Runtime.Misc.NotNull] Encoding_declContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitYield_expr([Antlr4.Runtime.Misc.NotNull] Yield_exprContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitYield_arg([Antlr4.Runtime.Misc.NotNull] Yield_argContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitStrings([Antlr4.Runtime.Misc.NotNull] StringsContext context)
        {
            throw new NotImplementedException();
        }

        public Accessibility Visit(IParseTree tree)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitChildren(IRuleNode node)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitTerminal(ITerminalNode node)
        {
            throw new NotImplementedException();
        }

        public Accessibility VisitErrorNode(IErrorNode node)
        {
            throw new NotImplementedException();
        }
    }
}
