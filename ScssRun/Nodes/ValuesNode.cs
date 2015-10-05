using ScssRun.Css;
using ScssRun.Expressions.Value;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class ValuesNode : BaseValueNode {

        public Expression Value { get; set; }
        public bool Important { get; set; }

        public static ValuesNode Parse(ScssParserContext context) {
            var res = new ValuesNode { Value = Expression.Parse(context.Tokens) };
            context.Tokens.SkipWhiteAndComments();
            var preview = context.Tokens.Peek();
            if (preview.Type == TokenType.ExclamationPoint) {
                context.Tokens.Read();
                var important = context.Tokens.Read(TokenType.Literal);
                if (important.StringValue != "important") throw new TokenException("!important expected", important);
                res.Important = true;
            }
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            env.CssRule.Declarations.Add(new CssDeclaration { Name = env.FormatProperty(), Value = Value.Evaluate(env).ToString() });
        }

    }
}
