// Generated from e:/Projects/Squeak/Grammar/SqueakSpeak.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link SqueakSpeakParser}.
 */
public interface SqueakSpeakListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#program}.
	 * @param ctx the parse tree
	 */
	void enterProgram(SqueakSpeakParser.ProgramContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#program}.
	 * @param ctx the parse tree
	 */
	void exitProgram(SqueakSpeakParser.ProgramContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#adorableStatement}.
	 * @param ctx the parse tree
	 */
	void enterAdorableStatement(SqueakSpeakParser.AdorableStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#adorableStatement}.
	 * @param ctx the parse tree
	 */
	void exitAdorableStatement(SqueakSpeakParser.AdorableStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#extraStatement}.
	 * @param ctx the parse tree
	 */
	void enterExtraStatement(SqueakSpeakParser.ExtraStatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#extraStatement}.
	 * @param ctx the parse tree
	 */
	void exitExtraStatement(SqueakSpeakParser.ExtraStatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#nativeCall}.
	 * @param ctx the parse tree
	 */
	void enterNativeCall(SqueakSpeakParser.NativeCallContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#nativeCall}.
	 * @param ctx the parse tree
	 */
	void exitNativeCall(SqueakSpeakParser.NativeCallContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#squeakNetGet}.
	 * @param ctx the parse tree
	 */
	void enterSqueakNetGet(SqueakSpeakParser.SqueakNetGetContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#squeakNetGet}.
	 * @param ctx the parse tree
	 */
	void exitSqueakNetGet(SqueakSpeakParser.SqueakNetGetContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#squeakIn}.
	 * @param ctx the parse tree
	 */
	void enterSqueakIn(SqueakSpeakParser.SqueakInContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#squeakIn}.
	 * @param ctx the parse tree
	 */
	void exitSqueakIn(SqueakSpeakParser.SqueakInContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#squeakMathCall}.
	 * @param ctx the parse tree
	 */
	void enterSqueakMathCall(SqueakSpeakParser.SqueakMathCallContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#squeakMathCall}.
	 * @param ctx the parse tree
	 */
	void exitSqueakMathCall(SqueakSpeakParser.SqueakMathCallContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#squeakOut}.
	 * @param ctx the parse tree
	 */
	void enterSqueakOut(SqueakSpeakParser.SqueakOutContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#squeakOut}.
	 * @param ctx the parse tree
	 */
	void exitSqueakOut(SqueakSpeakParser.SqueakOutContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#hugThis}.
	 * @param ctx the parse tree
	 */
	void enterHugThis(SqueakSpeakParser.HugThisContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#hugThis}.
	 * @param ctx the parse tree
	 */
	void exitHugThis(SqueakSpeakParser.HugThisContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#snugLoop}.
	 * @param ctx the parse tree
	 */
	void enterSnugLoop(SqueakSpeakParser.SnugLoopContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#snugLoop}.
	 * @param ctx the parse tree
	 */
	void exitSnugLoop(SqueakSpeakParser.SnugLoopContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#fluffMagic}.
	 * @param ctx the parse tree
	 */
	void enterFluffMagic(SqueakSpeakParser.FluffMagicContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#fluffMagic}.
	 * @param ctx the parse tree
	 */
	void exitFluffMagic(SqueakSpeakParser.FluffMagicContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#snuggleIf}.
	 * @param ctx the parse tree
	 */
	void enterSnuggleIf(SqueakSpeakParser.SnuggleIfContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#snuggleIf}.
	 * @param ctx the parse tree
	 */
	void exitSnuggleIf(SqueakSpeakParser.SnuggleIfContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#snipChoose}.
	 * @param ctx the parse tree
	 */
	void enterSnipChoose(SqueakSpeakParser.SnipChooseContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#snipChoose}.
	 * @param ctx the parse tree
	 */
	void exitSnipChoose(SqueakSpeakParser.SnipChooseContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#snipCase}.
	 * @param ctx the parse tree
	 */
	void enterSnipCase(SqueakSpeakParser.SnipCaseContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#snipCase}.
	 * @param ctx the parse tree
	 */
	void exitSnipCase(SqueakSpeakParser.SnipCaseContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#snipDefault}.
	 * @param ctx the parse tree
	 */
	void enterSnipDefault(SqueakSpeakParser.SnipDefaultContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#snipDefault}.
	 * @param ctx the parse tree
	 */
	void exitSnipDefault(SqueakSpeakParser.SnipDefaultContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#bringWarmth}.
	 * @param ctx the parse tree
	 */
	void enterBringWarmth(SqueakSpeakParser.BringWarmthContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#bringWarmth}.
	 * @param ctx the parse tree
	 */
	void exitBringWarmth(SqueakSpeakParser.BringWarmthContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#invokeWhimsy}.
	 * @param ctx the parse tree
	 */
	void enterInvokeWhimsy(SqueakSpeakParser.InvokeWhimsyContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#invokeWhimsy}.
	 * @param ctx the parse tree
	 */
	void exitInvokeWhimsy(SqueakSpeakParser.InvokeWhimsyContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#purrMath}.
	 * @param ctx the parse tree
	 */
	void enterPurrMath(SqueakSpeakParser.PurrMathContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#purrMath}.
	 * @param ctx the parse tree
	 */
	void exitPurrMath(SqueakSpeakParser.PurrMathContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#leftExpr}.
	 * @param ctx the parse tree
	 */
	void enterLeftExpr(SqueakSpeakParser.LeftExprContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#leftExpr}.
	 * @param ctx the parse tree
	 */
	void exitLeftExpr(SqueakSpeakParser.LeftExprContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#objectCreation}.
	 * @param ctx the parse tree
	 */
	void enterObjectCreation(SqueakSpeakParser.ObjectCreationContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#objectCreation}.
	 * @param ctx the parse tree
	 */
	void exitObjectCreation(SqueakSpeakParser.ObjectCreationContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#fieldAssignment}.
	 * @param ctx the parse tree
	 */
	void enterFieldAssignment(SqueakSpeakParser.FieldAssignmentContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#fieldAssignment}.
	 * @param ctx the parse tree
	 */
	void exitFieldAssignment(SqueakSpeakParser.FieldAssignmentContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#purrOperation}.
	 * @param ctx the parse tree
	 */
	void enterPurrOperation(SqueakSpeakParser.PurrOperationContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#purrOperation}.
	 * @param ctx the parse tree
	 */
	void exitPurrOperation(SqueakSpeakParser.PurrOperationContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#purrOperator}.
	 * @param ctx the parse tree
	 */
	void enterPurrOperator(SqueakSpeakParser.PurrOperatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#purrOperator}.
	 * @param ctx the parse tree
	 */
	void exitPurrOperator(SqueakSpeakParser.PurrOperatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#purrTerm}.
	 * @param ctx the parse tree
	 */
	void enterPurrTerm(SqueakSpeakParser.PurrTermContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#purrTerm}.
	 * @param ctx the parse tree
	 */
	void exitPurrTerm(SqueakSpeakParser.PurrTermContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#nativeCallExpr}.
	 * @param ctx the parse tree
	 */
	void enterNativeCallExpr(SqueakSpeakParser.NativeCallExprContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#nativeCallExpr}.
	 * @param ctx the parse tree
	 */
	void exitNativeCallExpr(SqueakSpeakParser.NativeCallExprContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#baseTerm}.
	 * @param ctx the parse tree
	 */
	void enterBaseTerm(SqueakSpeakParser.BaseTermContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#baseTerm}.
	 * @param ctx the parse tree
	 */
	void exitBaseTerm(SqueakSpeakParser.BaseTermContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#arrayLiteral}.
	 * @param ctx the parse tree
	 */
	void enterArrayLiteral(SqueakSpeakParser.ArrayLiteralContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#arrayLiteral}.
	 * @param ctx the parse tree
	 */
	void exitArrayLiteral(SqueakSpeakParser.ArrayLiteralContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#condition}.
	 * @param ctx the parse tree
	 */
	void enterCondition(SqueakSpeakParser.ConditionContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#condition}.
	 * @param ctx the parse tree
	 */
	void exitCondition(SqueakSpeakParser.ConditionContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#paramList}.
	 * @param ctx the parse tree
	 */
	void enterParamList(SqueakSpeakParser.ParamListContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#paramList}.
	 * @param ctx the parse tree
	 */
	void exitParamList(SqueakSpeakParser.ParamListContext ctx);
	/**
	 * Enter a parse tree produced by {@link SqueakSpeakParser#param}.
	 * @param ctx the parse tree
	 */
	void enterParam(SqueakSpeakParser.ParamContext ctx);
	/**
	 * Exit a parse tree produced by {@link SqueakSpeakParser#param}.
	 * @param ctx the parse tree
	 */
	void exitParam(SqueakSpeakParser.ParamContext ctx);
}