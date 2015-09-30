using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ValuesNode : BaseValueNode {

        public IList<ValueNode>  Values { get; } = new List<ValueNode>();

        public new static ValuesNode Parse(ScssParserContext context) {
            var res = new ValuesNode();
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
                    case TokenType.Semicolon:
                        context.Tokens.Read();
                        break;
                    case TokenType.Literal:
                        res.Values.Add(ValueNode.Parse(context));
                        break;
                    case TokenType.CloseCurlyBracket:
                        stop = true;
                        break;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            return res;
        }
    }
}
