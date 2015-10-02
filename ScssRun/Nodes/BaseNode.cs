using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public abstract class BaseNode {

        public IList<CommentNode> Comments { get; } = new List<CommentNode>();

        protected static void ParseBlock(ScssParserContext context) {
            context.Tokens.Read(TokenType.OpenCurlyBracket);
            while (!context.Tokens.Empty) {
                var preview = context.Tokens.Peek();
                if (preview.Type == TokenType.CloseCurlyBracket) {
                    break;
                } else {
                    Parse(context);
                }
            }
            context.Tokens.Read(TokenType.CloseCurlyBracket);
        }

        public static BaseNode Parse(ScssParserContext context) {
            var res = new NodeList();
            var stop = false;
            while (!context.Tokens.Empty && !stop) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        context.Tokens.Read();
                        res.Comments.Add(new CommentNode(preview));
                        break;
                    case TokenType.Whitespace:
                        context.Tokens.Read();
                        break;
                    case TokenType.CloseCurlyBracket:
                        stop = true;
                        break;
                    default:
                        if (IsPropertyName(context)) {
                            var rule = ScssDeclarationNode.Parse(context);
                            context.PushRule(rule);
                        } else {
                            var ruleSet = RuleSetNode.Parse(context);
                            res.Nodes.Add(ruleSet);
                        }
                        break;
                }
            }
            return res;
        }

        protected static bool IsPropertyName(ScssParserContext context) {
            var tokens = context.Tokens.Moment();
            if (tokens.Empty) return false;
            var prev = tokens.Read();
            var prevNoWhite = TokenType.Whitespace;
            while (!tokens.Empty) {
                var token = tokens.Read();
                switch (token.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                    case TokenType.Whitespace:
                        break;
                    case TokenType.Colon:
                        if (prev.Type == TokenType.Colon) return false;
                        break;
                    case TokenType.OpenCurlyBracket:
                        if (prevNoWhite == TokenType.Colon) return true;
                        return false;
                    case TokenType.Semicolon:
                        return true;
                }
                if (token.Type != TokenType.Whitespace && token.Type != TokenType.SingleLineComment &&
                    token.Type != TokenType.MultiLineComment) {
                    prevNoWhite = token.Type;
                }
                prev = token;
            }
            return false;
        }

        protected BaseNode ParseSelf(ScssParserContext context) {
            var stop = false;
            while (!context.Tokens.Empty && !stop) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        context.Tokens.Read();
                        Comments.Add(new CommentNode(preview));
                        break;
                    default:
                        stop = !AcceptToken(ref preview);
                        break;
                }
            }
            return this;
        }

        protected virtual bool AcceptToken(ref Token token) {
            return false;
        }

        public abstract void Compile(ScssEnvironment env);

    }
}
