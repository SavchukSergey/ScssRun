using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ValuesNode : BaseValueNode {

        public IList<ValueNode> Values { get; } = new List<ValueNode>();

        public new static ValuesNode Parse(ScssParserContext context) {
            var res = new ValuesNode();
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
                        break;
                    case TokenType.Semicolon:
                    case TokenType.CloseCurlyBracket:
                        if (res.Values.Count == 0) {
                            throw new TokenException("value expected", context.Tokens.LastReadToken);
                        }
                        return res;
                    case TokenType.Literal:
                        res.Values.Add(ValueNode.Parse(context));
                        break;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            throw new TokenException("unexpected end of file", context.Tokens.LastReadToken);
        }
    }
}
