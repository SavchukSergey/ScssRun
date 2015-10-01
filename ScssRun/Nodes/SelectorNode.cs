using System.Collections.Generic;
using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class SelectorNode : BaseNode {

        public IList<ElementNode> Elements { get; } = new List<ElementNode>();

        public new static SelectorNode Parse(ScssParserContext context) {
            var res = new SelectorNode();
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
                        var el = ElementNode.Parse(context);
                        res.Elements.Add(el);
                        break;
                    case TokenType.Whitespace:
                        context.Tokens.Read();
                        break;
                    case TokenType.OpenCurlyBracket:
                        stop = true;
                        break;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            return res;
        }

        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            var sb = new StringBuilder();
            for (var i = 0; i < Elements.Count; i++) {
                var el = Elements[i];
                if (i != 0) {
                    writer.Append(' ');
                }
                sb.Append(el.Value);
            }
            writer.AppendSelector(sb.ToString());
        }
    }
}
