using System;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ValueNode : BaseNode {

        public string Value { get; set; }

        public new static ValueNode Parse(ScssParserContext context) {
            var res = new ValueNode();
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
                        res.Value += " ";
                        break;
                    case TokenType.Literal:
                        context.Tokens.Read();
                        res.Value += preview.StringValue;
                        break;
                    case TokenType.Semicolon:
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
