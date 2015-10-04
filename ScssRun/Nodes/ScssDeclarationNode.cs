using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ScssDeclarationNode : BaseNode {

        public string Property { get; set; }

        public BaseValueNode Value { get; set; }

        public static ScssDeclarationNode Parse(ScssParserContext context) {
            var res = new ScssDeclarationNode();
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
                        if (!string.IsNullOrWhiteSpace(res.Property)) {
                            throw new TokenException("colon expected", preview);
                        }
                        context.Tokens.Read();
                        res.Property = preview.StringValue;
                        break;
                    case TokenType.Colon:
                        if (res.Value != null) {
                            throw new TokenException("semicolon expected", preview);
                        }
                        context.Tokens.Read();
                        context.PushRule(res);
                        res.Value = ParseValue(context);
                        context.PopRule();
                        break;
                    case TokenType.Semicolon:
                        context.Tokens.Read();
                        stop = true;
                        break;
                    case TokenType.CloseCurlyBracket:
                        stop = true;
                        break;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            if (string.IsNullOrWhiteSpace(res.Property)) {
                throw new TokenException("property name expected", context.Tokens.LastReadToken);
            }
            if (res.Value == null) {
                throw new TokenException("property value expected", context.Tokens.LastReadToken);
            }
            return res;
        }

        private static BaseValueNode ParseValue(ScssParserContext context) {
            var tokens = context.Tokens;
            context.Tokens.SkipWhiteAndComments();
            //todo: comments are lost. create aggregating node
            while (!tokens.Empty) {
                var preview = tokens.Peek();
                switch (preview.Type) {
                    case TokenType.Literal:
                    case TokenType.Number:
                        return ValuesNode.Parse(context);
                    case TokenType.OpenCurlyBracket:
                        tokens.Read(TokenType.OpenCurlyBracket);
                        var res = NestedValueNode.Parse(context);
                        tokens.Read(TokenType.CloseCurlyBracket);
                        return res;
                    default:
                        throw new TokenException("unexpected token", preview);
                }
            }
            throw new TokenException("unexpected end of file", context.Tokens.LastReadToken);
        }

        public override void Compile(ScssEnvironment env) {
            env.PushProperty(Property);
            Value.Compile(env);
            env.PopProperty();
        }
    }
}
