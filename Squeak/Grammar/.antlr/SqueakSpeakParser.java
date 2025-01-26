// Generated from e:/Projects/SqueakLanguage/Squeak/Grammar/SqueakSpeak.g4 by ANTLR 4.13.1
import org.antlr.v4.runtime.atn.*;
import org.antlr.v4.runtime.dfa.DFA;
import org.antlr.v4.runtime.*;
import org.antlr.v4.runtime.misc.*;
import org.antlr.v4.runtime.tree.*;
import java.util.List;
import java.util.Iterator;
import java.util.ArrayList;

@SuppressWarnings({"all", "warnings", "unchecked", "unused", "cast", "CheckReturnValue"})
public class SqueakSpeakParser extends Parser {
	static { RuntimeMetaData.checkVersion("4.13.1", RuntimeMetaData.VERSION); }

	protected static final DFA[] _decisionToDFA;
	protected static final PredictionContextCache _sharedContextCache =
		new PredictionContextCache();
	public static final int
		T__0=1, T__1=2, T__2=3, T__3=4, T__4=5, T__5=6, T__6=7, T__7=8, T__8=9, 
		T__9=10, T__10=11, T__11=12, T__12=13, T__13=14, T__14=15, T__15=16, T__16=17, 
		T__17=18, T__18=19, T__19=20, T__20=21, T__21=22, T__22=23, T__23=24, 
		T__24=25, T__25=26, T__26=27, T__27=28, T__28=29, T__29=30, T__30=31, 
		T__31=32, T__32=33, T__33=34, T__34=35, T__35=36, T__36=37, T__37=38, 
		T__38=39, T__39=40, STRING=41, ID=42, FLOAT=43, NUMBER=44, WS=45, LINE_COMMENT=46, 
		BLOCK_COMMENT=47;
	public static final int
		RULE_program = 0, RULE_adorableStatement = 1, RULE_extraStatement = 2, 
		RULE_nativeCall = 3, RULE_squeakNetGet = 4, RULE_squeakIn = 5, RULE_squeakMathCall = 6, 
		RULE_squeakOut = 7, RULE_hugThis = 8, RULE_snugLoop = 9, RULE_fluffMagic = 10, 
		RULE_snuggleIf = 11, RULE_snipChoose = 12, RULE_snipCase = 13, RULE_snipDefault = 14, 
		RULE_bringWarmth = 15, RULE_invokeWhimsy = 16, RULE_purrMath = 17, RULE_leftExpr = 18, 
		RULE_objectCreation = 19, RULE_fieldAssignment = 20, RULE_returnStatement = 21, 
		RULE_purrOperation = 22, RULE_purrOperator = 23, RULE_fieldAccess = 24, 
		RULE_purrTerm = 25, RULE_nativeCallExpr = 26, RULE_baseTerm = 27, RULE_arrayLiteral = 28, 
		RULE_condition = 29, RULE_paramList = 30, RULE_param = 31;
	private static String[] makeRuleNames() {
		return new String[] {
			"program", "adorableStatement", "extraStatement", "nativeCall", "squeakNetGet", 
			"squeakIn", "squeakMathCall", "squeakOut", "hugThis", "snugLoop", "fluffMagic", 
			"snuggleIf", "snipChoose", "snipCase", "snipDefault", "bringWarmth", 
			"invokeWhimsy", "purrMath", "leftExpr", "objectCreation", "fieldAssignment", 
			"returnStatement", "purrOperation", "purrOperator", "fieldAccess", "purrTerm", 
			"nativeCallExpr", "baseTerm", "arrayLiteral", "condition", "paramList", 
			"param"
		};
	}
	public static final String[] ruleNames = makeRuleNames();

	private static String[] makeLiteralNames() {
		return new String[] {
			null, "'NativeCall'", "'('", "')'", "'->'", "';'", "'BeepBoop'", "'Listen'", 
			"'Brain'", "','", "'Squeak'", "'Cuddle'", "'='", "'Nuzzle'", "'{'", "'}'", 
			"'FluffMagic'", "'Peek'", "'Purr'", "'SnipChoose'", "'SnipCase'", "':'", 
			"'SnipDefault'", "'BringWarmth'", "'['", "']'", "'SnuggleObject'", "'PawReturn'", 
			"'+'", "'-'", "'*'", "'/'", "'%'", "'^'", "'NativeFunc'", "'=='", "'!='", 
			"'<'", "'>'", "'<='", "'>='"
		};
	}
	private static final String[] _LITERAL_NAMES = makeLiteralNames();
	private static String[] makeSymbolicNames() {
		return new String[] {
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, null, null, null, null, null, null, null, null, null, 
			null, null, null, null, null, "STRING", "ID", "FLOAT", "NUMBER", "WS", 
			"LINE_COMMENT", "BLOCK_COMMENT"
		};
	}
	private static final String[] _SYMBOLIC_NAMES = makeSymbolicNames();
	public static final Vocabulary VOCABULARY = new VocabularyImpl(_LITERAL_NAMES, _SYMBOLIC_NAMES);

	/**
	 * @deprecated Use {@link #VOCABULARY} instead.
	 */
	@Deprecated
	public static final String[] tokenNames;
	static {
		tokenNames = new String[_SYMBOLIC_NAMES.length];
		for (int i = 0; i < tokenNames.length; i++) {
			tokenNames[i] = VOCABULARY.getLiteralName(i);
			if (tokenNames[i] == null) {
				tokenNames[i] = VOCABULARY.getSymbolicName(i);
			}

			if (tokenNames[i] == null) {
				tokenNames[i] = "<INVALID>";
			}
		}
	}

	@Override
	@Deprecated
	public String[] getTokenNames() {
		return tokenNames;
	}

	@Override

	public Vocabulary getVocabulary() {
		return VOCABULARY;
	}

	@Override
	public String getGrammarFileName() { return "SqueakSpeak.g4"; }

	@Override
	public String[] getRuleNames() { return ruleNames; }

	@Override
	public String getSerializedATN() { return _serializedATN; }

	@Override
	public ATN getATN() { return _ATN; }

	public SqueakSpeakParser(TokenStream input) {
		super(input);
		_interp = new ParserATNSimulator(this,_ATN,_decisionToDFA,_sharedContextCache);
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ProgramContext extends ParserRuleContext {
		public TerminalNode EOF() { return getToken(SqueakSpeakParser.EOF, 0); }
		public List<AdorableStatementContext> adorableStatement() {
			return getRuleContexts(AdorableStatementContext.class);
		}
		public AdorableStatementContext adorableStatement(int i) {
			return getRuleContext(AdorableStatementContext.class,i);
		}
		public ProgramContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_program; }
	}

	public final ProgramContext program() throws RecognitionException {
		ProgramContext _localctx = new ProgramContext(_ctx, getState());
		enterRule(_localctx, 0, RULE_program);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(67);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4398256958914L) != 0)) {
				{
				{
				setState(64);
				adorableStatement();
				}
				}
				setState(69);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(70);
			match(EOF);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class AdorableStatementContext extends ParserRuleContext {
		public SqueakOutContext squeakOut() {
			return getRuleContext(SqueakOutContext.class,0);
		}
		public HugThisContext hugThis() {
			return getRuleContext(HugThisContext.class,0);
		}
		public SnugLoopContext snugLoop() {
			return getRuleContext(SnugLoopContext.class,0);
		}
		public FluffMagicContext fluffMagic() {
			return getRuleContext(FluffMagicContext.class,0);
		}
		public SnuggleIfContext snuggleIf() {
			return getRuleContext(SnuggleIfContext.class,0);
		}
		public PurrMathContext purrMath() {
			return getRuleContext(PurrMathContext.class,0);
		}
		public SnipChooseContext snipChoose() {
			return getRuleContext(SnipChooseContext.class,0);
		}
		public BringWarmthContext bringWarmth() {
			return getRuleContext(BringWarmthContext.class,0);
		}
		public InvokeWhimsyContext invokeWhimsy() {
			return getRuleContext(InvokeWhimsyContext.class,0);
		}
		public SqueakNetGetContext squeakNetGet() {
			return getRuleContext(SqueakNetGetContext.class,0);
		}
		public SqueakInContext squeakIn() {
			return getRuleContext(SqueakInContext.class,0);
		}
		public SqueakMathCallContext squeakMathCall() {
			return getRuleContext(SqueakMathCallContext.class,0);
		}
		public NativeCallContext nativeCall() {
			return getRuleContext(NativeCallContext.class,0);
		}
		public ObjectCreationContext objectCreation() {
			return getRuleContext(ObjectCreationContext.class,0);
		}
		public FieldAssignmentContext fieldAssignment() {
			return getRuleContext(FieldAssignmentContext.class,0);
		}
		public ReturnStatementContext returnStatement() {
			return getRuleContext(ReturnStatementContext.class,0);
		}
		public AdorableStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_adorableStatement; }
	}

	public final AdorableStatementContext adorableStatement() throws RecognitionException {
		AdorableStatementContext _localctx = new AdorableStatementContext(_ctx, getState());
		enterRule(_localctx, 2, RULE_adorableStatement);
		try {
			setState(88);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,1,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(72);
				squeakOut();
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(73);
				hugThis();
				}
				break;
			case 3:
				enterOuterAlt(_localctx, 3);
				{
				setState(74);
				snugLoop();
				}
				break;
			case 4:
				enterOuterAlt(_localctx, 4);
				{
				setState(75);
				fluffMagic();
				}
				break;
			case 5:
				enterOuterAlt(_localctx, 5);
				{
				setState(76);
				snuggleIf();
				}
				break;
			case 6:
				enterOuterAlt(_localctx, 6);
				{
				setState(77);
				purrMath();
				}
				break;
			case 7:
				enterOuterAlt(_localctx, 7);
				{
				setState(78);
				snipChoose();
				}
				break;
			case 8:
				enterOuterAlt(_localctx, 8);
				{
				setState(79);
				bringWarmth();
				}
				break;
			case 9:
				enterOuterAlt(_localctx, 9);
				{
				setState(80);
				invokeWhimsy();
				}
				break;
			case 10:
				enterOuterAlt(_localctx, 10);
				{
				setState(81);
				squeakNetGet();
				}
				break;
			case 11:
				enterOuterAlt(_localctx, 11);
				{
				setState(82);
				squeakIn();
				}
				break;
			case 12:
				enterOuterAlt(_localctx, 12);
				{
				setState(83);
				squeakMathCall();
				}
				break;
			case 13:
				enterOuterAlt(_localctx, 13);
				{
				setState(84);
				nativeCall();
				}
				break;
			case 14:
				enterOuterAlt(_localctx, 14);
				{
				setState(85);
				objectCreation();
				}
				break;
			case 15:
				enterOuterAlt(_localctx, 15);
				{
				setState(86);
				fieldAssignment();
				}
				break;
			case 16:
				enterOuterAlt(_localctx, 16);
				{
				setState(87);
				returnStatement();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ExtraStatementContext extends ParserRuleContext {
		public SqueakNetGetContext squeakNetGet() {
			return getRuleContext(SqueakNetGetContext.class,0);
		}
		public SqueakInContext squeakIn() {
			return getRuleContext(SqueakInContext.class,0);
		}
		public SqueakMathCallContext squeakMathCall() {
			return getRuleContext(SqueakMathCallContext.class,0);
		}
		public ExtraStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_extraStatement; }
	}

	public final ExtraStatementContext extraStatement() throws RecognitionException {
		ExtraStatementContext _localctx = new ExtraStatementContext(_ctx, getState());
		enterRule(_localctx, 4, RULE_extraStatement);
		try {
			setState(93);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case T__5:
				enterOuterAlt(_localctx, 1);
				{
				setState(90);
				squeakNetGet();
				}
				break;
			case T__6:
				enterOuterAlt(_localctx, 2);
				{
				setState(91);
				squeakIn();
				}
				break;
			case T__7:
				enterOuterAlt(_localctx, 3);
				{
				setState(92);
				squeakMathCall();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class NativeCallContext extends ParserRuleContext {
		public List<TerminalNode> STRING() { return getTokens(SqueakSpeakParser.STRING); }
		public TerminalNode STRING(int i) {
			return getToken(SqueakSpeakParser.STRING, i);
		}
		public ParamListContext paramList() {
			return getRuleContext(ParamListContext.class,0);
		}
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public NativeCallContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_nativeCall; }
	}

	public final NativeCallContext nativeCall() throws RecognitionException {
		NativeCallContext _localctx = new NativeCallContext(_ctx, getState());
		enterRule(_localctx, 6, RULE_nativeCall);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(95);
			match(T__0);
			setState(96);
			match(STRING);
			setState(97);
			match(STRING);
			setState(98);
			match(T__1);
			setState(100);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 33002545479684L) != 0)) {
				{
				setState(99);
				paramList();
				}
			}

			setState(102);
			match(T__2);
			setState(105);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__3) {
				{
				setState(103);
				match(T__3);
				setState(104);
				match(ID);
				}
			}

			setState(107);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SqueakNetGetContext extends ParserRuleContext {
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public SqueakNetGetContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_squeakNetGet; }
	}

	public final SqueakNetGetContext squeakNetGet() throws RecognitionException {
		SqueakNetGetContext _localctx = new SqueakNetGetContext(_ctx, getState());
		enterRule(_localctx, 8, RULE_squeakNetGet);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(109);
			match(T__5);
			setState(110);
			match(T__1);
			setState(111);
			purrOperation();
			setState(112);
			match(T__2);
			setState(113);
			match(T__3);
			setState(114);
			match(ID);
			setState(115);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SqueakInContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public SqueakInContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_squeakIn; }
	}

	public final SqueakInContext squeakIn() throws RecognitionException {
		SqueakInContext _localctx = new SqueakInContext(_ctx, getState());
		enterRule(_localctx, 10, RULE_squeakIn);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(117);
			match(T__6);
			setState(118);
			match(T__1);
			setState(119);
			match(ID);
			setState(120);
			match(T__2);
			setState(121);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SqueakMathCallContext extends ParserRuleContext {
		public Token mathFunc;
		public List<TerminalNode> ID() { return getTokens(SqueakSpeakParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(SqueakSpeakParser.ID, i);
		}
		public List<PurrOperationContext> purrOperation() {
			return getRuleContexts(PurrOperationContext.class);
		}
		public PurrOperationContext purrOperation(int i) {
			return getRuleContext(PurrOperationContext.class,i);
		}
		public SqueakMathCallContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_squeakMathCall; }
	}

	public final SqueakMathCallContext squeakMathCall() throws RecognitionException {
		SqueakMathCallContext _localctx = new SqueakMathCallContext(_ctx, getState());
		enterRule(_localctx, 12, RULE_squeakMathCall);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(123);
			match(T__7);
			setState(124);
			match(T__1);
			setState(125);
			((SqueakMathCallContext)_localctx).mathFunc = match(ID);
			setState(132);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__8) {
				{
				setState(126);
				match(T__8);
				setState(127);
				purrOperation();
				setState(130);
				_errHandler.sync(this);
				_la = _input.LA(1);
				if (_la==T__8) {
					{
					setState(128);
					match(T__8);
					setState(129);
					purrOperation();
					}
				}

				}
			}

			setState(134);
			match(T__2);
			setState(135);
			match(T__3);
			setState(137);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==ID) {
				{
				setState(136);
				match(ID);
				}
			}

			setState(139);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SqueakOutContext extends ParserRuleContext {
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public SqueakOutContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_squeakOut; }
	}

	public final SqueakOutContext squeakOut() throws RecognitionException {
		SqueakOutContext _localctx = new SqueakOutContext(_ctx, getState());
		enterRule(_localctx, 14, RULE_squeakOut);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(141);
			match(T__9);
			setState(142);
			purrOperation();
			setState(143);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class HugThisContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public InvokeWhimsyContext invokeWhimsy() {
			return getRuleContext(InvokeWhimsyContext.class,0);
		}
		public HugThisContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_hugThis; }
	}

	public final HugThisContext hugThis() throws RecognitionException {
		HugThisContext _localctx = new HugThisContext(_ctx, getState());
		enterRule(_localctx, 16, RULE_hugThis);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(145);
			match(T__10);
			setState(146);
			match(ID);
			setState(152);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__11) {
				{
				setState(147);
				match(T__11);
				setState(150);
				_errHandler.sync(this);
				switch ( getInterpreter().adaptivePredict(_input,8,_ctx) ) {
				case 1:
					{
					setState(148);
					purrOperation();
					}
					break;
				case 2:
					{
					setState(149);
					invokeWhimsy();
					}
					break;
				}
				}
			}

			setState(154);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SnugLoopContext extends ParserRuleContext {
		public ConditionContext condition() {
			return getRuleContext(ConditionContext.class,0);
		}
		public List<AdorableStatementContext> adorableStatement() {
			return getRuleContexts(AdorableStatementContext.class);
		}
		public AdorableStatementContext adorableStatement(int i) {
			return getRuleContext(AdorableStatementContext.class,i);
		}
		public SnugLoopContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_snugLoop; }
	}

	public final SnugLoopContext snugLoop() throws RecognitionException {
		SnugLoopContext _localctx = new SnugLoopContext(_ctx, getState());
		enterRule(_localctx, 18, RULE_snugLoop);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(156);
			match(T__12);
			setState(157);
			match(T__1);
			setState(158);
			condition();
			setState(159);
			match(T__2);
			setState(160);
			match(T__13);
			setState(164);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4398256958914L) != 0)) {
				{
				{
				setState(161);
				adorableStatement();
				}
				}
				setState(166);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(167);
			match(T__14);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FluffMagicContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public ParamListContext paramList() {
			return getRuleContext(ParamListContext.class,0);
		}
		public List<AdorableStatementContext> adorableStatement() {
			return getRuleContexts(AdorableStatementContext.class);
		}
		public AdorableStatementContext adorableStatement(int i) {
			return getRuleContext(AdorableStatementContext.class,i);
		}
		public FluffMagicContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fluffMagic; }
	}

	public final FluffMagicContext fluffMagic() throws RecognitionException {
		FluffMagicContext _localctx = new FluffMagicContext(_ctx, getState());
		enterRule(_localctx, 20, RULE_fluffMagic);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(169);
			match(T__15);
			setState(170);
			match(ID);
			setState(171);
			match(T__1);
			setState(173);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 33002545479684L) != 0)) {
				{
				setState(172);
				paramList();
				}
			}

			setState(175);
			match(T__2);
			setState(176);
			match(T__13);
			setState(180);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4398256958914L) != 0)) {
				{
				{
				setState(177);
				adorableStatement();
				}
				}
				setState(182);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(183);
			match(T__14);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SnuggleIfContext extends ParserRuleContext {
		public AdorableStatementContext adorableStatement;
		public List<AdorableStatementContext> ifBlock = new ArrayList<AdorableStatementContext>();
		public List<AdorableStatementContext> elseBlock = new ArrayList<AdorableStatementContext>();
		public ConditionContext condition() {
			return getRuleContext(ConditionContext.class,0);
		}
		public List<AdorableStatementContext> adorableStatement() {
			return getRuleContexts(AdorableStatementContext.class);
		}
		public AdorableStatementContext adorableStatement(int i) {
			return getRuleContext(AdorableStatementContext.class,i);
		}
		public SnuggleIfContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_snuggleIf; }
	}

	public final SnuggleIfContext snuggleIf() throws RecognitionException {
		SnuggleIfContext _localctx = new SnuggleIfContext(_ctx, getState());
		enterRule(_localctx, 22, RULE_snuggleIf);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(185);
			match(T__16);
			setState(186);
			match(T__1);
			setState(187);
			condition();
			setState(188);
			match(T__2);
			setState(189);
			match(T__13);
			setState(193);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4398256958914L) != 0)) {
				{
				{
				setState(190);
				((SnuggleIfContext)_localctx).adorableStatement = adorableStatement();
				((SnuggleIfContext)_localctx).ifBlock.add(((SnuggleIfContext)_localctx).adorableStatement);
				}
				}
				setState(195);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(196);
			match(T__14);
			setState(206);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__17) {
				{
				setState(197);
				match(T__17);
				setState(198);
				match(T__13);
				setState(202);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4398256958914L) != 0)) {
					{
					{
					setState(199);
					((SnuggleIfContext)_localctx).adorableStatement = adorableStatement();
					((SnuggleIfContext)_localctx).elseBlock.add(((SnuggleIfContext)_localctx).adorableStatement);
					}
					}
					setState(204);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				setState(205);
				match(T__14);
				}
			}

			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SnipChooseContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public List<SnipCaseContext> snipCase() {
			return getRuleContexts(SnipCaseContext.class);
		}
		public SnipCaseContext snipCase(int i) {
			return getRuleContext(SnipCaseContext.class,i);
		}
		public SnipDefaultContext snipDefault() {
			return getRuleContext(SnipDefaultContext.class,0);
		}
		public SnipChooseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_snipChoose; }
	}

	public final SnipChooseContext snipChoose() throws RecognitionException {
		SnipChooseContext _localctx = new SnipChooseContext(_ctx, getState());
		enterRule(_localctx, 24, RULE_snipChoose);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(208);
			match(T__18);
			setState(209);
			match(T__1);
			setState(210);
			match(ID);
			setState(211);
			match(T__2);
			setState(212);
			match(T__13);
			setState(216);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__19) {
				{
				{
				setState(213);
				snipCase();
				}
				}
				setState(218);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(220);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if (_la==T__21) {
				{
				setState(219);
				snipDefault();
				}
			}

			setState(222);
			match(T__14);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SnipCaseContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public TerminalNode NUMBER() { return getToken(SqueakSpeakParser.NUMBER, 0); }
		public TerminalNode STRING() { return getToken(SqueakSpeakParser.STRING, 0); }
		public List<AdorableStatementContext> adorableStatement() {
			return getRuleContexts(AdorableStatementContext.class);
		}
		public AdorableStatementContext adorableStatement(int i) {
			return getRuleContext(AdorableStatementContext.class,i);
		}
		public SnipCaseContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_snipCase; }
	}

	public final SnipCaseContext snipCase() throws RecognitionException {
		SnipCaseContext _localctx = new SnipCaseContext(_ctx, getState());
		enterRule(_localctx, 26, RULE_snipCase);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(224);
			match(T__19);
			setState(225);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 24189255811072L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(226);
			match(T__20);
			setState(227);
			match(T__13);
			setState(231);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4398256958914L) != 0)) {
				{
				{
				setState(228);
				adorableStatement();
				}
				}
				setState(233);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(234);
			match(T__14);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class SnipDefaultContext extends ParserRuleContext {
		public List<AdorableStatementContext> adorableStatement() {
			return getRuleContexts(AdorableStatementContext.class);
		}
		public AdorableStatementContext adorableStatement(int i) {
			return getRuleContext(AdorableStatementContext.class,i);
		}
		public SnipDefaultContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_snipDefault; }
	}

	public final SnipDefaultContext snipDefault() throws RecognitionException {
		SnipDefaultContext _localctx = new SnipDefaultContext(_ctx, getState());
		enterRule(_localctx, 28, RULE_snipDefault);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(236);
			match(T__21);
			setState(237);
			match(T__20);
			setState(238);
			match(T__13);
			setState(242);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 4398256958914L) != 0)) {
				{
				{
				setState(239);
				adorableStatement();
				}
				}
				setState(244);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(245);
			match(T__14);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BringWarmthContext extends ParserRuleContext {
		public TerminalNode STRING() { return getToken(SqueakSpeakParser.STRING, 0); }
		public BringWarmthContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_bringWarmth; }
	}

	public final BringWarmthContext bringWarmth() throws RecognitionException {
		BringWarmthContext _localctx = new BringWarmthContext(_ctx, getState());
		enterRule(_localctx, 30, RULE_bringWarmth);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(247);
			match(T__22);
			setState(248);
			match(STRING);
			setState(249);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class InvokeWhimsyContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public ParamListContext paramList() {
			return getRuleContext(ParamListContext.class,0);
		}
		public InvokeWhimsyContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_invokeWhimsy; }
	}

	public final InvokeWhimsyContext invokeWhimsy() throws RecognitionException {
		InvokeWhimsyContext _localctx = new InvokeWhimsyContext(_ctx, getState());
		enterRule(_localctx, 32, RULE_invokeWhimsy);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(251);
			match(ID);
			setState(252);
			match(T__1);
			setState(254);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 33002545479684L) != 0)) {
				{
				setState(253);
				paramList();
				}
			}

			setState(256);
			match(T__2);
			setState(258);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,21,_ctx) ) {
			case 1:
				{
				setState(257);
				match(T__4);
				}
				break;
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PurrMathContext extends ParserRuleContext {
		public LeftExprContext leftExpr() {
			return getRuleContext(LeftExprContext.class,0);
		}
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public InvokeWhimsyContext invokeWhimsy() {
			return getRuleContext(InvokeWhimsyContext.class,0);
		}
		public PurrMathContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_purrMath; }
	}

	public final PurrMathContext purrMath() throws RecognitionException {
		PurrMathContext _localctx = new PurrMathContext(_ctx, getState());
		enterRule(_localctx, 34, RULE_purrMath);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(260);
			leftExpr();
			setState(261);
			match(T__11);
			setState(264);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,22,_ctx) ) {
			case 1:
				{
				setState(262);
				purrOperation();
				}
				break;
			case 2:
				{
				setState(263);
				invokeWhimsy();
				}
				break;
			}
			setState(266);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class LeftExprContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(SqueakSpeakParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(SqueakSpeakParser.ID, i);
		}
		public List<PurrOperationContext> purrOperation() {
			return getRuleContexts(PurrOperationContext.class);
		}
		public PurrOperationContext purrOperation(int i) {
			return getRuleContext(PurrOperationContext.class,i);
		}
		public LeftExprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_leftExpr; }
	}

	public final LeftExprContext leftExpr() throws RecognitionException {
		LeftExprContext _localctx = new LeftExprContext(_ctx, getState());
		enterRule(_localctx, 36, RULE_leftExpr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(268);
			match(ID);
			setState(273);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__3) {
				{
				{
				setState(269);
				match(T__3);
				setState(270);
				match(ID);
				}
				}
				setState(275);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(282);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__23) {
				{
				{
				setState(276);
				match(T__23);
				setState(277);
				purrOperation();
				setState(278);
				match(T__24);
				}
				}
				setState(284);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ObjectCreationContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public List<FieldAssignmentContext> fieldAssignment() {
			return getRuleContexts(FieldAssignmentContext.class);
		}
		public FieldAssignmentContext fieldAssignment(int i) {
			return getRuleContext(FieldAssignmentContext.class,i);
		}
		public ObjectCreationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_objectCreation; }
	}

	public final ObjectCreationContext objectCreation() throws RecognitionException {
		ObjectCreationContext _localctx = new ObjectCreationContext(_ctx, getState());
		enterRule(_localctx, 38, RULE_objectCreation);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(285);
			match(T__25);
			setState(286);
			match(ID);
			setState(287);
			match(T__13);
			setState(291);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==ID) {
				{
				{
				setState(288);
				fieldAssignment();
				}
				}
				setState(293);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(294);
			match(T__14);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FieldAssignmentContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(SqueakSpeakParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(SqueakSpeakParser.ID, i);
		}
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public FieldAssignmentContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fieldAssignment; }
	}

	public final FieldAssignmentContext fieldAssignment() throws RecognitionException {
		FieldAssignmentContext _localctx = new FieldAssignmentContext(_ctx, getState());
		enterRule(_localctx, 40, RULE_fieldAssignment);
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(296);
			match(ID);
			setState(297);
			match(T__3);
			setState(298);
			match(ID);
			setState(299);
			match(T__11);
			setState(300);
			purrOperation();
			setState(301);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ReturnStatementContext extends ParserRuleContext {
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public ReturnStatementContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_returnStatement; }
	}

	public final ReturnStatementContext returnStatement() throws RecognitionException {
		ReturnStatementContext _localctx = new ReturnStatementContext(_ctx, getState());
		enterRule(_localctx, 42, RULE_returnStatement);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(303);
			match(T__26);
			setState(305);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 33002545479684L) != 0)) {
				{
				setState(304);
				purrOperation();
				}
			}

			setState(307);
			match(T__4);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PurrOperationContext extends ParserRuleContext {
		public List<FieldAccessContext> fieldAccess() {
			return getRuleContexts(FieldAccessContext.class);
		}
		public FieldAccessContext fieldAccess(int i) {
			return getRuleContext(FieldAccessContext.class,i);
		}
		public List<PurrOperatorContext> purrOperator() {
			return getRuleContexts(PurrOperatorContext.class);
		}
		public PurrOperatorContext purrOperator(int i) {
			return getRuleContext(PurrOperatorContext.class,i);
		}
		public InvokeWhimsyContext invokeWhimsy() {
			return getRuleContext(InvokeWhimsyContext.class,0);
		}
		public PurrOperationContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_purrOperation; }
	}

	public final PurrOperationContext purrOperation() throws RecognitionException {
		PurrOperationContext _localctx = new PurrOperationContext(_ctx, getState());
		enterRule(_localctx, 44, RULE_purrOperation);
		int _la;
		try {
			setState(319);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,28,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(309);
				fieldAccess();
				setState(315);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while ((((_la) & ~0x3f) == 0 && ((1L << _la) & 16911433728L) != 0)) {
					{
					{
					setState(310);
					purrOperator();
					setState(311);
					fieldAccess();
					}
					}
					setState(317);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(318);
				invokeWhimsy();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PurrOperatorContext extends ParserRuleContext {
		public PurrOperatorContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_purrOperator; }
	}

	public final PurrOperatorContext purrOperator() throws RecognitionException {
		PurrOperatorContext _localctx = new PurrOperatorContext(_ctx, getState());
		enterRule(_localctx, 46, RULE_purrOperator);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(321);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 16911433728L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class FieldAccessContext extends ParserRuleContext {
		public PurrTermContext purrTerm() {
			return getRuleContext(PurrTermContext.class,0);
		}
		public List<TerminalNode> ID() { return getTokens(SqueakSpeakParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(SqueakSpeakParser.ID, i);
		}
		public FieldAccessContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_fieldAccess; }
	}

	public final FieldAccessContext fieldAccess() throws RecognitionException {
		FieldAccessContext _localctx = new FieldAccessContext(_ctx, getState());
		enterRule(_localctx, 48, RULE_fieldAccess);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(323);
			purrTerm();
			setState(328);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__3) {
				{
				{
				setState(324);
				match(T__3);
				setState(325);
				match(ID);
				}
				}
				setState(330);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class PurrTermContext extends ParserRuleContext {
		public BaseTermContext baseTerm() {
			return getRuleContext(BaseTermContext.class,0);
		}
		public List<PurrOperationContext> purrOperation() {
			return getRuleContexts(PurrOperationContext.class);
		}
		public PurrOperationContext purrOperation(int i) {
			return getRuleContext(PurrOperationContext.class,i);
		}
		public NativeCallExprContext nativeCallExpr() {
			return getRuleContext(NativeCallExprContext.class,0);
		}
		public PurrTermContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_purrTerm; }
	}

	public final PurrTermContext purrTerm() throws RecognitionException {
		PurrTermContext _localctx = new PurrTermContext(_ctx, getState());
		enterRule(_localctx, 50, RULE_purrTerm);
		int _la;
		try {
			setState(342);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case T__1:
			case T__23:
			case STRING:
			case ID:
			case FLOAT:
			case NUMBER:
				enterOuterAlt(_localctx, 1);
				{
				setState(331);
				baseTerm();
				setState(338);
				_errHandler.sync(this);
				_la = _input.LA(1);
				while (_la==T__23) {
					{
					{
					setState(332);
					match(T__23);
					setState(333);
					purrOperation();
					setState(334);
					match(T__24);
					}
					}
					setState(340);
					_errHandler.sync(this);
					_la = _input.LA(1);
				}
				}
				break;
			case T__33:
				enterOuterAlt(_localctx, 2);
				{
				setState(341);
				nativeCallExpr();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class NativeCallExprContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public ParamListContext paramList() {
			return getRuleContext(ParamListContext.class,0);
		}
		public NativeCallExprContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_nativeCallExpr; }
	}

	public final NativeCallExprContext nativeCallExpr() throws RecognitionException {
		NativeCallExprContext _localctx = new NativeCallExprContext(_ctx, getState());
		enterRule(_localctx, 52, RULE_nativeCallExpr);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(344);
			match(T__33);
			setState(345);
			match(ID);
			setState(346);
			match(T__1);
			setState(348);
			_errHandler.sync(this);
			_la = _input.LA(1);
			if ((((_la) & ~0x3f) == 0 && ((1L << _la) & 33002545479684L) != 0)) {
				{
				setState(347);
				paramList();
				}
			}

			setState(350);
			match(T__2);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class BaseTermContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public TerminalNode NUMBER() { return getToken(SqueakSpeakParser.NUMBER, 0); }
		public TerminalNode FLOAT() { return getToken(SqueakSpeakParser.FLOAT, 0); }
		public TerminalNode STRING() { return getToken(SqueakSpeakParser.STRING, 0); }
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public ArrayLiteralContext arrayLiteral() {
			return getRuleContext(ArrayLiteralContext.class,0);
		}
		public BaseTermContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_baseTerm; }
	}

	public final BaseTermContext baseTerm() throws RecognitionException {
		BaseTermContext _localctx = new BaseTermContext(_ctx, getState());
		enterRule(_localctx, 54, RULE_baseTerm);
		try {
			setState(361);
			_errHandler.sync(this);
			switch (_input.LA(1)) {
			case ID:
				enterOuterAlt(_localctx, 1);
				{
				setState(352);
				match(ID);
				}
				break;
			case NUMBER:
				enterOuterAlt(_localctx, 2);
				{
				setState(353);
				match(NUMBER);
				}
				break;
			case FLOAT:
				enterOuterAlt(_localctx, 3);
				{
				setState(354);
				match(FLOAT);
				}
				break;
			case STRING:
				enterOuterAlt(_localctx, 4);
				{
				setState(355);
				match(STRING);
				}
				break;
			case T__1:
				enterOuterAlt(_localctx, 5);
				{
				setState(356);
				match(T__1);
				setState(357);
				purrOperation();
				setState(358);
				match(T__2);
				}
				break;
			case T__23:
				enterOuterAlt(_localctx, 6);
				{
				setState(360);
				arrayLiteral();
				}
				break;
			default:
				throw new NoViableAltException(this);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ArrayLiteralContext extends ParserRuleContext {
		public List<PurrOperationContext> purrOperation() {
			return getRuleContexts(PurrOperationContext.class);
		}
		public PurrOperationContext purrOperation(int i) {
			return getRuleContext(PurrOperationContext.class,i);
		}
		public ArrayLiteralContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_arrayLiteral; }
	}

	public final ArrayLiteralContext arrayLiteral() throws RecognitionException {
		ArrayLiteralContext _localctx = new ArrayLiteralContext(_ctx, getState());
		enterRule(_localctx, 56, RULE_arrayLiteral);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(363);
			match(T__23);
			setState(364);
			purrOperation();
			setState(369);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__8) {
				{
				{
				setState(365);
				match(T__8);
				setState(366);
				purrOperation();
				}
				}
				setState(371);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			setState(372);
			match(T__24);
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ConditionContext extends ParserRuleContext {
		public List<TerminalNode> ID() { return getTokens(SqueakSpeakParser.ID); }
		public TerminalNode ID(int i) {
			return getToken(SqueakSpeakParser.ID, i);
		}
		public TerminalNode NUMBER() { return getToken(SqueakSpeakParser.NUMBER, 0); }
		public TerminalNode FLOAT() { return getToken(SqueakSpeakParser.FLOAT, 0); }
		public ConditionContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_condition; }
	}

	public final ConditionContext condition() throws RecognitionException {
		ConditionContext _localctx = new ConditionContext(_ctx, getState());
		enterRule(_localctx, 58, RULE_condition);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(374);
			match(ID);
			setState(375);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 2164663517184L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			setState(376);
			_la = _input.LA(1);
			if ( !((((_la) & ~0x3f) == 0 && ((1L << _la) & 30786325577728L) != 0)) ) {
			_errHandler.recoverInline(this);
			}
			else {
				if ( _input.LA(1)==Token.EOF ) matchedEOF = true;
				_errHandler.reportMatch(this);
				consume();
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ParamListContext extends ParserRuleContext {
		public List<ParamContext> param() {
			return getRuleContexts(ParamContext.class);
		}
		public ParamContext param(int i) {
			return getRuleContext(ParamContext.class,i);
		}
		public ParamListContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_paramList; }
	}

	public final ParamListContext paramList() throws RecognitionException {
		ParamListContext _localctx = new ParamListContext(_ctx, getState());
		enterRule(_localctx, 60, RULE_paramList);
		int _la;
		try {
			enterOuterAlt(_localctx, 1);
			{
			setState(378);
			param();
			setState(383);
			_errHandler.sync(this);
			_la = _input.LA(1);
			while (_la==T__8) {
				{
				{
				setState(379);
				match(T__8);
				setState(380);
				param();
				}
				}
				setState(385);
				_errHandler.sync(this);
				_la = _input.LA(1);
			}
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	@SuppressWarnings("CheckReturnValue")
	public static class ParamContext extends ParserRuleContext {
		public TerminalNode ID() { return getToken(SqueakSpeakParser.ID, 0); }
		public PurrOperationContext purrOperation() {
			return getRuleContext(PurrOperationContext.class,0);
		}
		public ParamContext(ParserRuleContext parent, int invokingState) {
			super(parent, invokingState);
		}
		@Override public int getRuleIndex() { return RULE_param; }
	}

	public final ParamContext param() throws RecognitionException {
		ParamContext _localctx = new ParamContext(_ctx, getState());
		enterRule(_localctx, 62, RULE_param);
		try {
			setState(388);
			_errHandler.sync(this);
			switch ( getInterpreter().adaptivePredict(_input,36,_ctx) ) {
			case 1:
				enterOuterAlt(_localctx, 1);
				{
				setState(386);
				match(ID);
				}
				break;
			case 2:
				enterOuterAlt(_localctx, 2);
				{
				setState(387);
				purrOperation();
				}
				break;
			}
		}
		catch (RecognitionException re) {
			_localctx.exception = re;
			_errHandler.reportError(this, re);
			_errHandler.recover(this, re);
		}
		finally {
			exitRule();
		}
		return _localctx;
	}

	public static final String _serializedATN =
		"\u0004\u0001/\u0187\u0002\u0000\u0007\u0000\u0002\u0001\u0007\u0001\u0002"+
		"\u0002\u0007\u0002\u0002\u0003\u0007\u0003\u0002\u0004\u0007\u0004\u0002"+
		"\u0005\u0007\u0005\u0002\u0006\u0007\u0006\u0002\u0007\u0007\u0007\u0002"+
		"\b\u0007\b\u0002\t\u0007\t\u0002\n\u0007\n\u0002\u000b\u0007\u000b\u0002"+
		"\f\u0007\f\u0002\r\u0007\r\u0002\u000e\u0007\u000e\u0002\u000f\u0007\u000f"+
		"\u0002\u0010\u0007\u0010\u0002\u0011\u0007\u0011\u0002\u0012\u0007\u0012"+
		"\u0002\u0013\u0007\u0013\u0002\u0014\u0007\u0014\u0002\u0015\u0007\u0015"+
		"\u0002\u0016\u0007\u0016\u0002\u0017\u0007\u0017\u0002\u0018\u0007\u0018"+
		"\u0002\u0019\u0007\u0019\u0002\u001a\u0007\u001a\u0002\u001b\u0007\u001b"+
		"\u0002\u001c\u0007\u001c\u0002\u001d\u0007\u001d\u0002\u001e\u0007\u001e"+
		"\u0002\u001f\u0007\u001f\u0001\u0000\u0005\u0000B\b\u0000\n\u0000\f\u0000"+
		"E\t\u0000\u0001\u0000\u0001\u0000\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001\u0001"+
		"\u0001\u0001\u0003\u0001Y\b\u0001\u0001\u0002\u0001\u0002\u0001\u0002"+
		"\u0003\u0002^\b\u0002\u0001\u0003\u0001\u0003\u0001\u0003\u0001\u0003"+
		"\u0001\u0003\u0003\u0003e\b\u0003\u0001\u0003\u0001\u0003\u0001\u0003"+
		"\u0003\u0003j\b\u0003\u0001\u0003\u0001\u0003\u0001\u0004\u0001\u0004"+
		"\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004\u0001\u0004"+
		"\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005\u0001\u0005"+
		"\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006\u0001\u0006"+
		"\u0001\u0006\u0003\u0006\u0083\b\u0006\u0003\u0006\u0085\b\u0006\u0001"+
		"\u0006\u0001\u0006\u0001\u0006\u0003\u0006\u008a\b\u0006\u0001\u0006\u0001"+
		"\u0006\u0001\u0007\u0001\u0007\u0001\u0007\u0001\u0007\u0001\b\u0001\b"+
		"\u0001\b\u0001\b\u0001\b\u0003\b\u0097\b\b\u0003\b\u0099\b\b\u0001\b\u0001"+
		"\b\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0001\t\u0005\t\u00a3\b\t\n"+
		"\t\f\t\u00a6\t\t\u0001\t\u0001\t\u0001\n\u0001\n\u0001\n\u0001\n\u0003"+
		"\n\u00ae\b\n\u0001\n\u0001\n\u0001\n\u0005\n\u00b3\b\n\n\n\f\n\u00b6\t"+
		"\n\u0001\n\u0001\n\u0001\u000b\u0001\u000b\u0001\u000b\u0001\u000b\u0001"+
		"\u000b\u0001\u000b\u0005\u000b\u00c0\b\u000b\n\u000b\f\u000b\u00c3\t\u000b"+
		"\u0001\u000b\u0001\u000b\u0001\u000b\u0001\u000b\u0005\u000b\u00c9\b\u000b"+
		"\n\u000b\f\u000b\u00cc\t\u000b\u0001\u000b\u0003\u000b\u00cf\b\u000b\u0001"+
		"\f\u0001\f\u0001\f\u0001\f\u0001\f\u0001\f\u0005\f\u00d7\b\f\n\f\f\f\u00da"+
		"\t\f\u0001\f\u0003\f\u00dd\b\f\u0001\f\u0001\f\u0001\r\u0001\r\u0001\r"+
		"\u0001\r\u0001\r\u0005\r\u00e6\b\r\n\r\f\r\u00e9\t\r\u0001\r\u0001\r\u0001"+
		"\u000e\u0001\u000e\u0001\u000e\u0001\u000e\u0005\u000e\u00f1\b\u000e\n"+
		"\u000e\f\u000e\u00f4\t\u000e\u0001\u000e\u0001\u000e\u0001\u000f\u0001"+
		"\u000f\u0001\u000f\u0001\u000f\u0001\u0010\u0001\u0010\u0001\u0010\u0003"+
		"\u0010\u00ff\b\u0010\u0001\u0010\u0001\u0010\u0003\u0010\u0103\b\u0010"+
		"\u0001\u0011\u0001\u0011\u0001\u0011\u0001\u0011\u0003\u0011\u0109\b\u0011"+
		"\u0001\u0011\u0001\u0011\u0001\u0012\u0001\u0012\u0001\u0012\u0005\u0012"+
		"\u0110\b\u0012\n\u0012\f\u0012\u0113\t\u0012\u0001\u0012\u0001\u0012\u0001"+
		"\u0012\u0001\u0012\u0005\u0012\u0119\b\u0012\n\u0012\f\u0012\u011c\t\u0012"+
		"\u0001\u0013\u0001\u0013\u0001\u0013\u0001\u0013\u0005\u0013\u0122\b\u0013"+
		"\n\u0013\f\u0013\u0125\t\u0013\u0001\u0013\u0001\u0013\u0001\u0014\u0001"+
		"\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001\u0014\u0001"+
		"\u0015\u0001\u0015\u0003\u0015\u0132\b\u0015\u0001\u0015\u0001\u0015\u0001"+
		"\u0016\u0001\u0016\u0001\u0016\u0001\u0016\u0005\u0016\u013a\b\u0016\n"+
		"\u0016\f\u0016\u013d\t\u0016\u0001\u0016\u0003\u0016\u0140\b\u0016\u0001"+
		"\u0017\u0001\u0017\u0001\u0018\u0001\u0018\u0001\u0018\u0005\u0018\u0147"+
		"\b\u0018\n\u0018\f\u0018\u014a\t\u0018\u0001\u0019\u0001\u0019\u0001\u0019"+
		"\u0001\u0019\u0001\u0019\u0005\u0019\u0151\b\u0019\n\u0019\f\u0019\u0154"+
		"\t\u0019\u0001\u0019\u0003\u0019\u0157\b\u0019\u0001\u001a\u0001\u001a"+
		"\u0001\u001a\u0001\u001a\u0003\u001a\u015d\b\u001a\u0001\u001a\u0001\u001a"+
		"\u0001\u001b\u0001\u001b\u0001\u001b\u0001\u001b\u0001\u001b\u0001\u001b"+
		"\u0001\u001b\u0001\u001b\u0001\u001b\u0003\u001b\u016a\b\u001b\u0001\u001c"+
		"\u0001\u001c\u0001\u001c\u0001\u001c\u0005\u001c\u0170\b\u001c\n\u001c"+
		"\f\u001c\u0173\t\u001c\u0001\u001c\u0001\u001c\u0001\u001d\u0001\u001d"+
		"\u0001\u001d\u0001\u001d\u0001\u001e\u0001\u001e\u0001\u001e\u0005\u001e"+
		"\u017e\b\u001e\n\u001e\f\u001e\u0181\t\u001e\u0001\u001f\u0001\u001f\u0003"+
		"\u001f\u0185\b\u001f\u0001\u001f\u0000\u0000 \u0000\u0002\u0004\u0006"+
		"\b\n\f\u000e\u0010\u0012\u0014\u0016\u0018\u001a\u001c\u001e \"$&(*,."+
		"02468:<>\u0000\u0004\u0002\u0000)*,,\u0001\u0000\u001c!\u0001\u0000#("+
		"\u0001\u0000*,\u019e\u0000C\u0001\u0000\u0000\u0000\u0002X\u0001\u0000"+
		"\u0000\u0000\u0004]\u0001\u0000\u0000\u0000\u0006_\u0001\u0000\u0000\u0000"+
		"\bm\u0001\u0000\u0000\u0000\nu\u0001\u0000\u0000\u0000\f{\u0001\u0000"+
		"\u0000\u0000\u000e\u008d\u0001\u0000\u0000\u0000\u0010\u0091\u0001\u0000"+
		"\u0000\u0000\u0012\u009c\u0001\u0000\u0000\u0000\u0014\u00a9\u0001\u0000"+
		"\u0000\u0000\u0016\u00b9\u0001\u0000\u0000\u0000\u0018\u00d0\u0001\u0000"+
		"\u0000\u0000\u001a\u00e0\u0001\u0000\u0000\u0000\u001c\u00ec\u0001\u0000"+
		"\u0000\u0000\u001e\u00f7\u0001\u0000\u0000\u0000 \u00fb\u0001\u0000\u0000"+
		"\u0000\"\u0104\u0001\u0000\u0000\u0000$\u010c\u0001\u0000\u0000\u0000"+
		"&\u011d\u0001\u0000\u0000\u0000(\u0128\u0001\u0000\u0000\u0000*\u012f"+
		"\u0001\u0000\u0000\u0000,\u013f\u0001\u0000\u0000\u0000.\u0141\u0001\u0000"+
		"\u0000\u00000\u0143\u0001\u0000\u0000\u00002\u0156\u0001\u0000\u0000\u0000"+
		"4\u0158\u0001\u0000\u0000\u00006\u0169\u0001\u0000\u0000\u00008\u016b"+
		"\u0001\u0000\u0000\u0000:\u0176\u0001\u0000\u0000\u0000<\u017a\u0001\u0000"+
		"\u0000\u0000>\u0184\u0001\u0000\u0000\u0000@B\u0003\u0002\u0001\u0000"+
		"A@\u0001\u0000\u0000\u0000BE\u0001\u0000\u0000\u0000CA\u0001\u0000\u0000"+
		"\u0000CD\u0001\u0000\u0000\u0000DF\u0001\u0000\u0000\u0000EC\u0001\u0000"+
		"\u0000\u0000FG\u0005\u0000\u0000\u0001G\u0001\u0001\u0000\u0000\u0000"+
		"HY\u0003\u000e\u0007\u0000IY\u0003\u0010\b\u0000JY\u0003\u0012\t\u0000"+
		"KY\u0003\u0014\n\u0000LY\u0003\u0016\u000b\u0000MY\u0003\"\u0011\u0000"+
		"NY\u0003\u0018\f\u0000OY\u0003\u001e\u000f\u0000PY\u0003 \u0010\u0000"+
		"QY\u0003\b\u0004\u0000RY\u0003\n\u0005\u0000SY\u0003\f\u0006\u0000TY\u0003"+
		"\u0006\u0003\u0000UY\u0003&\u0013\u0000VY\u0003(\u0014\u0000WY\u0003*"+
		"\u0015\u0000XH\u0001\u0000\u0000\u0000XI\u0001\u0000\u0000\u0000XJ\u0001"+
		"\u0000\u0000\u0000XK\u0001\u0000\u0000\u0000XL\u0001\u0000\u0000\u0000"+
		"XM\u0001\u0000\u0000\u0000XN\u0001\u0000\u0000\u0000XO\u0001\u0000\u0000"+
		"\u0000XP\u0001\u0000\u0000\u0000XQ\u0001\u0000\u0000\u0000XR\u0001\u0000"+
		"\u0000\u0000XS\u0001\u0000\u0000\u0000XT\u0001\u0000\u0000\u0000XU\u0001"+
		"\u0000\u0000\u0000XV\u0001\u0000\u0000\u0000XW\u0001\u0000\u0000\u0000"+
		"Y\u0003\u0001\u0000\u0000\u0000Z^\u0003\b\u0004\u0000[^\u0003\n\u0005"+
		"\u0000\\^\u0003\f\u0006\u0000]Z\u0001\u0000\u0000\u0000][\u0001\u0000"+
		"\u0000\u0000]\\\u0001\u0000\u0000\u0000^\u0005\u0001\u0000\u0000\u0000"+
		"_`\u0005\u0001\u0000\u0000`a\u0005)\u0000\u0000ab\u0005)\u0000\u0000b"+
		"d\u0005\u0002\u0000\u0000ce\u0003<\u001e\u0000dc\u0001\u0000\u0000\u0000"+
		"de\u0001\u0000\u0000\u0000ef\u0001\u0000\u0000\u0000fi\u0005\u0003\u0000"+
		"\u0000gh\u0005\u0004\u0000\u0000hj\u0005*\u0000\u0000ig\u0001\u0000\u0000"+
		"\u0000ij\u0001\u0000\u0000\u0000jk\u0001\u0000\u0000\u0000kl\u0005\u0005"+
		"\u0000\u0000l\u0007\u0001\u0000\u0000\u0000mn\u0005\u0006\u0000\u0000"+
		"no\u0005\u0002\u0000\u0000op\u0003,\u0016\u0000pq\u0005\u0003\u0000\u0000"+
		"qr\u0005\u0004\u0000\u0000rs\u0005*\u0000\u0000st\u0005\u0005\u0000\u0000"+
		"t\t\u0001\u0000\u0000\u0000uv\u0005\u0007\u0000\u0000vw\u0005\u0002\u0000"+
		"\u0000wx\u0005*\u0000\u0000xy\u0005\u0003\u0000\u0000yz\u0005\u0005\u0000"+
		"\u0000z\u000b\u0001\u0000\u0000\u0000{|\u0005\b\u0000\u0000|}\u0005\u0002"+
		"\u0000\u0000}\u0084\u0005*\u0000\u0000~\u007f\u0005\t\u0000\u0000\u007f"+
		"\u0082\u0003,\u0016\u0000\u0080\u0081\u0005\t\u0000\u0000\u0081\u0083"+
		"\u0003,\u0016\u0000\u0082\u0080\u0001\u0000\u0000\u0000\u0082\u0083\u0001"+
		"\u0000\u0000\u0000\u0083\u0085\u0001\u0000\u0000\u0000\u0084~\u0001\u0000"+
		"\u0000\u0000\u0084\u0085\u0001\u0000\u0000\u0000\u0085\u0086\u0001\u0000"+
		"\u0000\u0000\u0086\u0087\u0005\u0003\u0000\u0000\u0087\u0089\u0005\u0004"+
		"\u0000\u0000\u0088\u008a\u0005*\u0000\u0000\u0089\u0088\u0001\u0000\u0000"+
		"\u0000\u0089\u008a\u0001\u0000\u0000\u0000\u008a\u008b\u0001\u0000\u0000"+
		"\u0000\u008b\u008c\u0005\u0005\u0000\u0000\u008c\r\u0001\u0000\u0000\u0000"+
		"\u008d\u008e\u0005\n\u0000\u0000\u008e\u008f\u0003,\u0016\u0000\u008f"+
		"\u0090\u0005\u0005\u0000\u0000\u0090\u000f\u0001\u0000\u0000\u0000\u0091"+
		"\u0092\u0005\u000b\u0000\u0000\u0092\u0098\u0005*\u0000\u0000\u0093\u0096"+
		"\u0005\f\u0000\u0000\u0094\u0097\u0003,\u0016\u0000\u0095\u0097\u0003"+
		" \u0010\u0000\u0096\u0094\u0001\u0000\u0000\u0000\u0096\u0095\u0001\u0000"+
		"\u0000\u0000\u0097\u0099\u0001\u0000\u0000\u0000\u0098\u0093\u0001\u0000"+
		"\u0000\u0000\u0098\u0099\u0001\u0000\u0000\u0000\u0099\u009a\u0001\u0000"+
		"\u0000\u0000\u009a\u009b\u0005\u0005\u0000\u0000\u009b\u0011\u0001\u0000"+
		"\u0000\u0000\u009c\u009d\u0005\r\u0000\u0000\u009d\u009e\u0005\u0002\u0000"+
		"\u0000\u009e\u009f\u0003:\u001d\u0000\u009f\u00a0\u0005\u0003\u0000\u0000"+
		"\u00a0\u00a4\u0005\u000e\u0000\u0000\u00a1\u00a3\u0003\u0002\u0001\u0000"+
		"\u00a2\u00a1\u0001\u0000\u0000\u0000\u00a3\u00a6\u0001\u0000\u0000\u0000"+
		"\u00a4\u00a2\u0001\u0000\u0000\u0000\u00a4\u00a5\u0001\u0000\u0000\u0000"+
		"\u00a5\u00a7\u0001\u0000\u0000\u0000\u00a6\u00a4\u0001\u0000\u0000\u0000"+
		"\u00a7\u00a8\u0005\u000f\u0000\u0000\u00a8\u0013\u0001\u0000\u0000\u0000"+
		"\u00a9\u00aa\u0005\u0010\u0000\u0000\u00aa\u00ab\u0005*\u0000\u0000\u00ab"+
		"\u00ad\u0005\u0002\u0000\u0000\u00ac\u00ae\u0003<\u001e\u0000\u00ad\u00ac"+
		"\u0001\u0000\u0000\u0000\u00ad\u00ae\u0001\u0000\u0000\u0000\u00ae\u00af"+
		"\u0001\u0000\u0000\u0000\u00af\u00b0\u0005\u0003\u0000\u0000\u00b0\u00b4"+
		"\u0005\u000e\u0000\u0000\u00b1\u00b3\u0003\u0002\u0001\u0000\u00b2\u00b1"+
		"\u0001\u0000\u0000\u0000\u00b3\u00b6\u0001\u0000\u0000\u0000\u00b4\u00b2"+
		"\u0001\u0000\u0000\u0000\u00b4\u00b5\u0001\u0000\u0000\u0000\u00b5\u00b7"+
		"\u0001\u0000\u0000\u0000\u00b6\u00b4\u0001\u0000\u0000\u0000\u00b7\u00b8"+
		"\u0005\u000f\u0000\u0000\u00b8\u0015\u0001\u0000\u0000\u0000\u00b9\u00ba"+
		"\u0005\u0011\u0000\u0000\u00ba\u00bb\u0005\u0002\u0000\u0000\u00bb\u00bc"+
		"\u0003:\u001d\u0000\u00bc\u00bd\u0005\u0003\u0000\u0000\u00bd\u00c1\u0005"+
		"\u000e\u0000\u0000\u00be\u00c0\u0003\u0002\u0001\u0000\u00bf\u00be\u0001"+
		"\u0000\u0000\u0000\u00c0\u00c3\u0001\u0000\u0000\u0000\u00c1\u00bf\u0001"+
		"\u0000\u0000\u0000\u00c1\u00c2\u0001\u0000\u0000\u0000\u00c2\u00c4\u0001"+
		"\u0000\u0000\u0000\u00c3\u00c1\u0001\u0000\u0000\u0000\u00c4\u00ce\u0005"+
		"\u000f\u0000\u0000\u00c5\u00c6\u0005\u0012\u0000\u0000\u00c6\u00ca\u0005"+
		"\u000e\u0000\u0000\u00c7\u00c9\u0003\u0002\u0001\u0000\u00c8\u00c7\u0001"+
		"\u0000\u0000\u0000\u00c9\u00cc\u0001\u0000\u0000\u0000\u00ca\u00c8\u0001"+
		"\u0000\u0000\u0000\u00ca\u00cb\u0001\u0000\u0000\u0000\u00cb\u00cd\u0001"+
		"\u0000\u0000\u0000\u00cc\u00ca\u0001\u0000\u0000\u0000\u00cd\u00cf\u0005"+
		"\u000f\u0000\u0000\u00ce\u00c5\u0001\u0000\u0000\u0000\u00ce\u00cf\u0001"+
		"\u0000\u0000\u0000\u00cf\u0017\u0001\u0000\u0000\u0000\u00d0\u00d1\u0005"+
		"\u0013\u0000\u0000\u00d1\u00d2\u0005\u0002\u0000\u0000\u00d2\u00d3\u0005"+
		"*\u0000\u0000\u00d3\u00d4\u0005\u0003\u0000\u0000\u00d4\u00d8\u0005\u000e"+
		"\u0000\u0000\u00d5\u00d7\u0003\u001a\r\u0000\u00d6\u00d5\u0001\u0000\u0000"+
		"\u0000\u00d7\u00da\u0001\u0000\u0000\u0000\u00d8\u00d6\u0001\u0000\u0000"+
		"\u0000\u00d8\u00d9\u0001\u0000\u0000\u0000\u00d9\u00dc\u0001\u0000\u0000"+
		"\u0000\u00da\u00d8\u0001\u0000\u0000\u0000\u00db\u00dd\u0003\u001c\u000e"+
		"\u0000\u00dc\u00db\u0001\u0000\u0000\u0000\u00dc\u00dd\u0001\u0000\u0000"+
		"\u0000\u00dd\u00de\u0001\u0000\u0000\u0000\u00de\u00df\u0005\u000f\u0000"+
		"\u0000\u00df\u0019\u0001\u0000\u0000\u0000\u00e0\u00e1\u0005\u0014\u0000"+
		"\u0000\u00e1\u00e2\u0007\u0000\u0000\u0000\u00e2\u00e3\u0005\u0015\u0000"+
		"\u0000\u00e3\u00e7\u0005\u000e\u0000\u0000\u00e4\u00e6\u0003\u0002\u0001"+
		"\u0000\u00e5\u00e4\u0001\u0000\u0000\u0000\u00e6\u00e9\u0001\u0000\u0000"+
		"\u0000\u00e7\u00e5\u0001\u0000\u0000\u0000\u00e7\u00e8\u0001\u0000\u0000"+
		"\u0000\u00e8\u00ea\u0001\u0000\u0000\u0000\u00e9\u00e7\u0001\u0000\u0000"+
		"\u0000\u00ea\u00eb\u0005\u000f\u0000\u0000\u00eb\u001b\u0001\u0000\u0000"+
		"\u0000\u00ec\u00ed\u0005\u0016\u0000\u0000\u00ed\u00ee\u0005\u0015\u0000"+
		"\u0000\u00ee\u00f2\u0005\u000e\u0000\u0000\u00ef\u00f1\u0003\u0002\u0001"+
		"\u0000\u00f0\u00ef\u0001\u0000\u0000\u0000\u00f1\u00f4\u0001\u0000\u0000"+
		"\u0000\u00f2\u00f0\u0001\u0000\u0000\u0000\u00f2\u00f3\u0001\u0000\u0000"+
		"\u0000\u00f3\u00f5\u0001\u0000\u0000\u0000\u00f4\u00f2\u0001\u0000\u0000"+
		"\u0000\u00f5\u00f6\u0005\u000f\u0000\u0000\u00f6\u001d\u0001\u0000\u0000"+
		"\u0000\u00f7\u00f8\u0005\u0017\u0000\u0000\u00f8\u00f9\u0005)\u0000\u0000"+
		"\u00f9\u00fa\u0005\u0005\u0000\u0000\u00fa\u001f\u0001\u0000\u0000\u0000"+
		"\u00fb\u00fc\u0005*\u0000\u0000\u00fc\u00fe\u0005\u0002\u0000\u0000\u00fd"+
		"\u00ff\u0003<\u001e\u0000\u00fe\u00fd\u0001\u0000\u0000\u0000\u00fe\u00ff"+
		"\u0001\u0000\u0000\u0000\u00ff\u0100\u0001\u0000\u0000\u0000\u0100\u0102"+
		"\u0005\u0003\u0000\u0000\u0101\u0103\u0005\u0005\u0000\u0000\u0102\u0101"+
		"\u0001\u0000\u0000\u0000\u0102\u0103\u0001\u0000\u0000\u0000\u0103!\u0001"+
		"\u0000\u0000\u0000\u0104\u0105\u0003$\u0012\u0000\u0105\u0108\u0005\f"+
		"\u0000\u0000\u0106\u0109\u0003,\u0016\u0000\u0107\u0109\u0003 \u0010\u0000"+
		"\u0108\u0106\u0001\u0000\u0000\u0000\u0108\u0107\u0001\u0000\u0000\u0000"+
		"\u0109\u010a\u0001\u0000\u0000\u0000\u010a\u010b\u0005\u0005\u0000\u0000"+
		"\u010b#\u0001\u0000\u0000\u0000\u010c\u0111\u0005*\u0000\u0000\u010d\u010e"+
		"\u0005\u0004\u0000\u0000\u010e\u0110\u0005*\u0000\u0000\u010f\u010d\u0001"+
		"\u0000\u0000\u0000\u0110\u0113\u0001\u0000\u0000\u0000\u0111\u010f\u0001"+
		"\u0000\u0000\u0000\u0111\u0112\u0001\u0000\u0000\u0000\u0112\u011a\u0001"+
		"\u0000\u0000\u0000\u0113\u0111\u0001\u0000\u0000\u0000\u0114\u0115\u0005"+
		"\u0018\u0000\u0000\u0115\u0116\u0003,\u0016\u0000\u0116\u0117\u0005\u0019"+
		"\u0000\u0000\u0117\u0119\u0001\u0000\u0000\u0000\u0118\u0114\u0001\u0000"+
		"\u0000\u0000\u0119\u011c\u0001\u0000\u0000\u0000\u011a\u0118\u0001\u0000"+
		"\u0000\u0000\u011a\u011b\u0001\u0000\u0000\u0000\u011b%\u0001\u0000\u0000"+
		"\u0000\u011c\u011a\u0001\u0000\u0000\u0000\u011d\u011e\u0005\u001a\u0000"+
		"\u0000\u011e\u011f\u0005*\u0000\u0000\u011f\u0123\u0005\u000e\u0000\u0000"+
		"\u0120\u0122\u0003(\u0014\u0000\u0121\u0120\u0001\u0000\u0000\u0000\u0122"+
		"\u0125\u0001\u0000\u0000\u0000\u0123\u0121\u0001\u0000\u0000\u0000\u0123"+
		"\u0124\u0001\u0000\u0000\u0000\u0124\u0126\u0001\u0000\u0000\u0000\u0125"+
		"\u0123\u0001\u0000\u0000\u0000\u0126\u0127\u0005\u000f\u0000\u0000\u0127"+
		"\'\u0001\u0000\u0000\u0000\u0128\u0129\u0005*\u0000\u0000\u0129\u012a"+
		"\u0005\u0004\u0000\u0000\u012a\u012b\u0005*\u0000\u0000\u012b\u012c\u0005"+
		"\f\u0000\u0000\u012c\u012d\u0003,\u0016\u0000\u012d\u012e\u0005\u0005"+
		"\u0000\u0000\u012e)\u0001\u0000\u0000\u0000\u012f\u0131\u0005\u001b\u0000"+
		"\u0000\u0130\u0132\u0003,\u0016\u0000\u0131\u0130\u0001\u0000\u0000\u0000"+
		"\u0131\u0132\u0001\u0000\u0000\u0000\u0132\u0133\u0001\u0000\u0000\u0000"+
		"\u0133\u0134\u0005\u0005\u0000\u0000\u0134+\u0001\u0000\u0000\u0000\u0135"+
		"\u013b\u00030\u0018\u0000\u0136\u0137\u0003.\u0017\u0000\u0137\u0138\u0003"+
		"0\u0018\u0000\u0138\u013a\u0001\u0000\u0000\u0000\u0139\u0136\u0001\u0000"+
		"\u0000\u0000\u013a\u013d\u0001\u0000\u0000\u0000\u013b\u0139\u0001\u0000"+
		"\u0000\u0000\u013b\u013c\u0001\u0000\u0000\u0000\u013c\u0140\u0001\u0000"+
		"\u0000\u0000\u013d\u013b\u0001\u0000\u0000\u0000\u013e\u0140\u0003 \u0010"+
		"\u0000\u013f\u0135\u0001\u0000\u0000\u0000\u013f\u013e\u0001\u0000\u0000"+
		"\u0000\u0140-\u0001\u0000\u0000\u0000\u0141\u0142\u0007\u0001\u0000\u0000"+
		"\u0142/\u0001\u0000\u0000\u0000\u0143\u0148\u00032\u0019\u0000\u0144\u0145"+
		"\u0005\u0004\u0000\u0000\u0145\u0147\u0005*\u0000\u0000\u0146\u0144\u0001"+
		"\u0000\u0000\u0000\u0147\u014a\u0001\u0000\u0000\u0000\u0148\u0146\u0001"+
		"\u0000\u0000\u0000\u0148\u0149\u0001\u0000\u0000\u0000\u01491\u0001\u0000"+
		"\u0000\u0000\u014a\u0148\u0001\u0000\u0000\u0000\u014b\u0152\u00036\u001b"+
		"\u0000\u014c\u014d\u0005\u0018\u0000\u0000\u014d\u014e\u0003,\u0016\u0000"+
		"\u014e\u014f\u0005\u0019\u0000\u0000\u014f\u0151\u0001\u0000\u0000\u0000"+
		"\u0150\u014c\u0001\u0000\u0000\u0000\u0151\u0154\u0001\u0000\u0000\u0000"+
		"\u0152\u0150\u0001\u0000\u0000\u0000\u0152\u0153\u0001\u0000\u0000\u0000"+
		"\u0153\u0157\u0001\u0000\u0000\u0000\u0154\u0152\u0001\u0000\u0000\u0000"+
		"\u0155\u0157\u00034\u001a\u0000\u0156\u014b\u0001\u0000\u0000\u0000\u0156"+
		"\u0155\u0001\u0000\u0000\u0000\u01573\u0001\u0000\u0000\u0000\u0158\u0159"+
		"\u0005\"\u0000\u0000\u0159\u015a\u0005*\u0000\u0000\u015a\u015c\u0005"+
		"\u0002\u0000\u0000\u015b\u015d\u0003<\u001e\u0000\u015c\u015b\u0001\u0000"+
		"\u0000\u0000\u015c\u015d\u0001\u0000\u0000\u0000\u015d\u015e\u0001\u0000"+
		"\u0000\u0000\u015e\u015f\u0005\u0003\u0000\u0000\u015f5\u0001\u0000\u0000"+
		"\u0000\u0160\u016a\u0005*\u0000\u0000\u0161\u016a\u0005,\u0000\u0000\u0162"+
		"\u016a\u0005+\u0000\u0000\u0163\u016a\u0005)\u0000\u0000\u0164\u0165\u0005"+
		"\u0002\u0000\u0000\u0165\u0166\u0003,\u0016\u0000\u0166\u0167\u0005\u0003"+
		"\u0000\u0000\u0167\u016a\u0001\u0000\u0000\u0000\u0168\u016a\u00038\u001c"+
		"\u0000\u0169\u0160\u0001\u0000\u0000\u0000\u0169\u0161\u0001\u0000\u0000"+
		"\u0000\u0169\u0162\u0001\u0000\u0000\u0000\u0169\u0163\u0001\u0000\u0000"+
		"\u0000\u0169\u0164\u0001\u0000\u0000\u0000\u0169\u0168\u0001\u0000\u0000"+
		"\u0000\u016a7\u0001\u0000\u0000\u0000\u016b\u016c\u0005\u0018\u0000\u0000"+
		"\u016c\u0171\u0003,\u0016\u0000\u016d\u016e\u0005\t\u0000\u0000\u016e"+
		"\u0170\u0003,\u0016\u0000\u016f\u016d\u0001\u0000\u0000\u0000\u0170\u0173"+
		"\u0001\u0000\u0000\u0000\u0171\u016f\u0001\u0000\u0000\u0000\u0171\u0172"+
		"\u0001\u0000\u0000\u0000\u0172\u0174\u0001\u0000\u0000\u0000\u0173\u0171"+
		"\u0001\u0000\u0000\u0000\u0174\u0175\u0005\u0019\u0000\u0000\u01759\u0001"+
		"\u0000\u0000\u0000\u0176\u0177\u0005*\u0000\u0000\u0177\u0178\u0007\u0002"+
		"\u0000\u0000\u0178\u0179\u0007\u0003\u0000\u0000\u0179;\u0001\u0000\u0000"+
		"\u0000\u017a\u017f\u0003>\u001f\u0000\u017b\u017c\u0005\t\u0000\u0000"+
		"\u017c\u017e\u0003>\u001f\u0000\u017d\u017b\u0001\u0000\u0000\u0000\u017e"+
		"\u0181\u0001\u0000\u0000\u0000\u017f\u017d\u0001\u0000\u0000\u0000\u017f"+
		"\u0180\u0001\u0000\u0000\u0000\u0180=\u0001\u0000\u0000\u0000\u0181\u017f"+
		"\u0001\u0000\u0000\u0000\u0182\u0185\u0005*\u0000\u0000\u0183\u0185\u0003"+
		",\u0016\u0000\u0184\u0182\u0001\u0000\u0000\u0000\u0184\u0183\u0001\u0000"+
		"\u0000\u0000\u0185?\u0001\u0000\u0000\u0000%CX]di\u0082\u0084\u0089\u0096"+
		"\u0098\u00a4\u00ad\u00b4\u00c1\u00ca\u00ce\u00d8\u00dc\u00e7\u00f2\u00fe"+
		"\u0102\u0108\u0111\u011a\u0123\u0131\u013b\u013f\u0148\u0152\u0156\u015c"+
		"\u0169\u0171\u017f\u0184";
	public static final ATN _ATN =
		new ATNDeserializer().deserialize(_serializedATN.toCharArray());
	static {
		_decisionToDFA = new DFA[_ATN.getNumberOfDecisions()];
		for (int i = 0; i < _ATN.getNumberOfDecisions(); i++) {
			_decisionToDFA[i] = new DFA(_ATN.getDecisionState(i), i);
		}
	}
}