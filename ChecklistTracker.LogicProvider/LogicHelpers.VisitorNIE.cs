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
    internal partial class LogicHelpers : IPython3ParserVisitor<bool>
    {
        public bool VisitSingle_input([Antlr4.Runtime.Misc.NotNull] Single_inputContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitFile_input([Antlr4.Runtime.Misc.NotNull] File_inputContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitEval_input([Antlr4.Runtime.Misc.NotNull] Eval_inputContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitExpr_input([Antlr4.Runtime.Misc.NotNull] Expr_inputContext context)
        {
            return context.expr_stmt().Accept(this);
        }

        public bool VisitDecorator([Antlr4.Runtime.Misc.NotNull] DecoratorContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDecorators([Antlr4.Runtime.Misc.NotNull] DecoratorsContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDecorated([Antlr4.Runtime.Misc.NotNull] DecoratedContext context)
        {
            throw new NotImplementedException();
        }
        public bool VisitAsync_funcdef([Antlr4.Runtime.Misc.NotNull] Async_funcdefContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitFuncdef([Antlr4.Runtime.Misc.NotNull] FuncdefContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitParameters([Antlr4.Runtime.Misc.NotNull] ParametersContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitTypedargslist([Antlr4.Runtime.Misc.NotNull] TypedargslistContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitTfpdef([Antlr4.Runtime.Misc.NotNull] TfpdefContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitVarargslist([Antlr4.Runtime.Misc.NotNull] VarargslistContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitVfpdef([Antlr4.Runtime.Misc.NotNull] VfpdefContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitStmt([Antlr4.Runtime.Misc.NotNull] StmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSimple_stmts([Antlr4.Runtime.Misc.NotNull] Simple_stmtsContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSimple_stmt([Antlr4.Runtime.Misc.NotNull] Simple_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitAnnassign([Antlr4.Runtime.Misc.NotNull] AnnassignContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitAugassign([Antlr4.Runtime.Misc.NotNull] AugassignContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDel_stmt([Antlr4.Runtime.Misc.NotNull] Del_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitPass_stmt([Antlr4.Runtime.Misc.NotNull] Pass_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitFlow_stmt([Antlr4.Runtime.Misc.NotNull] Flow_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitBreak_stmt([Antlr4.Runtime.Misc.NotNull] Break_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitContinue_stmt([Antlr4.Runtime.Misc.NotNull] Continue_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitReturn_stmt([Antlr4.Runtime.Misc.NotNull] Return_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitYield_stmt([Antlr4.Runtime.Misc.NotNull] Yield_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitRaise_stmt([Antlr4.Runtime.Misc.NotNull] Raise_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitImport_stmt([Antlr4.Runtime.Misc.NotNull] Import_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitImport_name([Antlr4.Runtime.Misc.NotNull] Import_nameContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitImport_from([Antlr4.Runtime.Misc.NotNull] Import_fromContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitImport_as_name([Antlr4.Runtime.Misc.NotNull] Import_as_nameContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDotted_as_name([Antlr4.Runtime.Misc.NotNull] Dotted_as_nameContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitImport_as_names([Antlr4.Runtime.Misc.NotNull] Import_as_namesContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDotted_as_names([Antlr4.Runtime.Misc.NotNull] Dotted_as_namesContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDotted_name([Antlr4.Runtime.Misc.NotNull] Dotted_nameContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitGlobal_stmt([Antlr4.Runtime.Misc.NotNull] Global_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitNonlocal_stmt([Antlr4.Runtime.Misc.NotNull] Nonlocal_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitAssert_stmt([Antlr4.Runtime.Misc.NotNull] Assert_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitCompound_stmt([Antlr4.Runtime.Misc.NotNull] Compound_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitAsync_stmt([Antlr4.Runtime.Misc.NotNull] Async_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitIf_stmt([Antlr4.Runtime.Misc.NotNull] If_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitWhile_stmt([Antlr4.Runtime.Misc.NotNull] While_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitFor_stmt([Antlr4.Runtime.Misc.NotNull] For_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitTry_stmt([Antlr4.Runtime.Misc.NotNull] Try_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitWith_stmt([Antlr4.Runtime.Misc.NotNull] With_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitWith_item([Antlr4.Runtime.Misc.NotNull] With_itemContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitExcept_clause([Antlr4.Runtime.Misc.NotNull] Except_clauseContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitBlock([Antlr4.Runtime.Misc.NotNull] BlockContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitMatch_stmt([Antlr4.Runtime.Misc.NotNull] Match_stmtContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSubject_expr([Antlr4.Runtime.Misc.NotNull] Subject_exprContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitStar_named_expressions([Antlr4.Runtime.Misc.NotNull] Star_named_expressionsContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitStar_named_expression([Antlr4.Runtime.Misc.NotNull] Star_named_expressionContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitCase_block([Antlr4.Runtime.Misc.NotNull] Case_blockContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitGuard([Antlr4.Runtime.Misc.NotNull] GuardContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitPatterns([Antlr4.Runtime.Misc.NotNull] PatternsContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitPattern([Antlr4.Runtime.Misc.NotNull] PatternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitAs_pattern([Antlr4.Runtime.Misc.NotNull] As_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitOr_pattern([Antlr4.Runtime.Misc.NotNull] Or_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitClosed_pattern([Antlr4.Runtime.Misc.NotNull] Closed_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitLiteral_pattern([Antlr4.Runtime.Misc.NotNull] Literal_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitLiteral_expr([Antlr4.Runtime.Misc.NotNull] Literal_exprContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitComplex_number([Antlr4.Runtime.Misc.NotNull] Complex_numberContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSigned_number([Antlr4.Runtime.Misc.NotNull] Signed_numberContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSigned_real_number([Antlr4.Runtime.Misc.NotNull] Signed_real_numberContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitReal_number([Antlr4.Runtime.Misc.NotNull] Real_numberContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitImaginary_number([Antlr4.Runtime.Misc.NotNull] Imaginary_numberContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitCapture_pattern([Antlr4.Runtime.Misc.NotNull] Capture_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitPattern_capture_target([Antlr4.Runtime.Misc.NotNull] Pattern_capture_targetContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitWildcard_pattern([Antlr4.Runtime.Misc.NotNull] Wildcard_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitValue_pattern([Antlr4.Runtime.Misc.NotNull] Value_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitAttr([Antlr4.Runtime.Misc.NotNull] AttrContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitName_or_attr([Antlr4.Runtime.Misc.NotNull] Name_or_attrContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitGroup_pattern([Antlr4.Runtime.Misc.NotNull] Group_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSequence_pattern([Antlr4.Runtime.Misc.NotNull] Sequence_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitOpen_sequence_pattern([Antlr4.Runtime.Misc.NotNull] Open_sequence_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitMaybe_sequence_pattern([Antlr4.Runtime.Misc.NotNull] Maybe_sequence_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitMaybe_star_pattern([Antlr4.Runtime.Misc.NotNull] Maybe_star_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitStar_pattern([Antlr4.Runtime.Misc.NotNull] Star_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitMapping_pattern([Antlr4.Runtime.Misc.NotNull] Mapping_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitItems_pattern([Antlr4.Runtime.Misc.NotNull] Items_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitKey_value_pattern([Antlr4.Runtime.Misc.NotNull] Key_value_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDouble_star_pattern([Antlr4.Runtime.Misc.NotNull] Double_star_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitClass_pattern([Antlr4.Runtime.Misc.NotNull] Class_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitPositional_patterns([Antlr4.Runtime.Misc.NotNull] Positional_patternsContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitKeyword_patterns([Antlr4.Runtime.Misc.NotNull] Keyword_patternsContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitKeyword_pattern([Antlr4.Runtime.Misc.NotNull] Keyword_patternContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitTest_nocond([Antlr4.Runtime.Misc.NotNull] Test_nocondContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitLambdef([Antlr4.Runtime.Misc.NotNull] LambdefContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitLambdef_nocond([Antlr4.Runtime.Misc.NotNull] Lambdef_nocondContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitStar_expr([Antlr4.Runtime.Misc.NotNull] Star_exprContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitName([Antlr4.Runtime.Misc.NotNull] NameContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitTrailer([Antlr4.Runtime.Misc.NotNull] TrailerContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSubscriptlist([Antlr4.Runtime.Misc.NotNull] SubscriptlistContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSubscript_([Antlr4.Runtime.Misc.NotNull] Subscript_Context context)
        {
            throw new NotImplementedException();
        }

        public bool VisitSliceop([Antlr4.Runtime.Misc.NotNull] SliceopContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitExprlist([Antlr4.Runtime.Misc.NotNull] ExprlistContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitTestlist([Antlr4.Runtime.Misc.NotNull] TestlistContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitDictorsetmaker([Antlr4.Runtime.Misc.NotNull] DictorsetmakerContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitClassdef([Antlr4.Runtime.Misc.NotNull] ClassdefContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitArglist([Antlr4.Runtime.Misc.NotNull] ArglistContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitComp_iter([Antlr4.Runtime.Misc.NotNull] Comp_iterContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitComp_for([Antlr4.Runtime.Misc.NotNull] Comp_forContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitComp_if([Antlr4.Runtime.Misc.NotNull] Comp_ifContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitEncoding_decl([Antlr4.Runtime.Misc.NotNull] Encoding_declContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitYield_expr([Antlr4.Runtime.Misc.NotNull] Yield_exprContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitYield_arg([Antlr4.Runtime.Misc.NotNull] Yield_argContext context)
        {
            throw new NotImplementedException();
        }

        public bool VisitStrings([Antlr4.Runtime.Misc.NotNull] StringsContext context)
        {
            throw new NotImplementedException();
        }

        public bool Visit(IParseTree tree)
        {
            throw new NotImplementedException();
        }

        public bool VisitChildren(IRuleNode node)
        {
            throw new NotImplementedException();
        }

        public bool VisitTerminal(ITerminalNode node)
        {
            throw new NotImplementedException();
        }

        public bool VisitErrorNode(IErrorNode node)
        {
            throw new NotImplementedException();
        }
    }
}
