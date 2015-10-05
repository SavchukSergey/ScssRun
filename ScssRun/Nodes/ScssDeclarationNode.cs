using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ScssDeclarationNode : BaseNode {

        public string Property { get; set; }

        public BaseValueNode Value { get; set; }

        public static ScssDeclarationNode Parse(ScssParserContext context) {
            var res = new ScssDeclarationNode();
            context.Tokens.SkipWhiteAndComments();
            res.Property = context.Tokens.Read(TokenType.Literal).StringValue;
            context.Tokens.SkipWhiteAndComments();
            context.Tokens.Read(TokenType.Colon);
            context.Tokens.SkipWhiteAndComments();
            var preview = context.Tokens.Peek();
            if (preview.Type == TokenType.OpenCurlyBracket) {
                res.Value = NestedValueNode.Parse(context);
            } else {
                res.Value = ValuesNode.Parse(context);
            }
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            env.PushProperty(Property);
            Value.Compile(env);
            env.PopProperty();
        }
    }
}
