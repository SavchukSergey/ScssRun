using ScssRun.Expressions.Value;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ValueNode : BaseNode {

        public Expression Value { get; set; }

        public static ValueNode Parse(ScssParserContext context) {
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
                    case TokenType.Number:
                        res.Value = Expression.Parse(context.Tokens);
                        return res;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            throw new TokenException("unexpected end of file", context.Tokens.LastReadToken);
        }

        public override void Compile(ScssEnvironment env) {
            //writer.Append(Value.Evaluate(env).ToString());
        }
    }
}
