using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public abstract class BaseNode {

        public IList<CommentNode> Comments { get; } = new List<CommentNode>();

        protected static BaseNode ParseBlock(ScssParserContext context) {
            context.Tokens.Read(TokenType.OpenCurlyBracket);
            var node = Parse(context);
            context.Tokens.Read(TokenType.CloseCurlyBracket);
            return node;
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
                        var ruleSet = RuleSetNode.Parse(context);
                        res.Nodes.Add(ruleSet);
                        break;
                }
            }
            return res;
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

        public abstract void ToCss(CssWriter writer, ScssEnvironment env);

        public string ToCss(ScssEnvironment env) {
            var sb = new CssWriter(CssWriterOptions.Minified);
            ToCss(sb, env);
            return sb.ToString();
        }
    }
}
