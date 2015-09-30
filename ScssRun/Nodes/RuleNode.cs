using System;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class RuleNode : BaseNode {

        public string Property { get; set; }

        public BaseValueNode Value { get; set; }

        public new static RuleNode Parse(ScssParserContext context) {
            var res = new RuleNode();
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
                    case TokenType.Literal:
                        context.Tokens.Read();
                        res.Property = preview.StringValue;
                        break;
                    case TokenType.Colon:
                        context.Tokens.Read();
                        res.Value = ParseValue(context);
                        stop = true;
                        break;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            return res;
        }

        private static BaseValueNode ParseValue(ScssParserContext context) {
            var tokens = context.Tokens.Moment();

            while (!tokens.Empty) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.Literal:
                        return ValuesNode.Parse(context);
                    case TokenType.OpenCurlyBracket:
                        context.Tokens.Read(TokenType.OpenCurlyBracket);
                        var res = NestedValueNode.Parse(context);
                        context.Tokens.Read(TokenType.CloseCurlyBracket);
                        return res;
                    default:
                        context.Tokens.Read();
                        break;
                }
            }
            throw new TokenException("unexpected end of file", context.Tokens.LastReadToken);
        }
    }
}
