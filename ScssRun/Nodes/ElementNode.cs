using System.Collections.Generic;
using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ElementNode : BaseNode {

        public string Value { get; set; }

        public new static ElementNode Parse(ScssParserContext context) {
            var res = new ElementNode();
            var sb = new StringBuilder();
            var stop = false;
            while (!context.Tokens.Empty && !stop) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        context.Tokens.Read();
                        res.Comments.Add(new CommentNode(preview));
                        break;
                    case TokenType.Literal:
                    case TokenType.Hash:
                        context.Tokens.Read();
                        sb.Append(preview.StringValue);
                        break;
                    case TokenType.Whitespace:
                        if (sb.Length == 0) {
                            context.Tokens.Read();
                        } else {
                            stop = true;
                        }
                        break;
                    default:
                        stop = true;
                        break;
                }
            }
            res.Value = sb.ToString();
            return res;
        }

        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            throw new System.NotImplementedException();
        }
    }
}
