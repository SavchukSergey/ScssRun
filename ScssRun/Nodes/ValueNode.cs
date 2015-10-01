using System;
using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ValueNode : BaseNode {

        public string Value { get; set; }

        public new static ValueNode Parse(ScssParserContext context) {
            var res = new ValueNode();
            while (!context.Tokens.Empty) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        context.Tokens.Read();
                        res.Comments.Add(new CommentNode(preview));
                        break;
                    case TokenType.Whitespace:
                        context.Tokens.Read();
                        return res;
                    case TokenType.Literal:
                        context.Tokens.Read();
                        res.Value = preview.StringValue;
                        return res;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            throw new TokenException("unexpected end of file", context.Tokens.LastReadToken);
        }

        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            writer.Append(Value);
        }
    }
}
