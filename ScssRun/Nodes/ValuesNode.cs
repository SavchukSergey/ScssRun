using ScssRun.Css;
using ScssRun.Expressions.Value;

namespace ScssRun.Nodes {
    public class ValuesNode : BaseValueNode {

        public Expression Value { get; set; }

        public static ValuesNode Parse(ScssParserContext context) {
            var res = new ValuesNode { Value = Expression.Parse(context.Tokens) };
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            env.CssRule.Declarations.Add(new CssDeclaration { Name = env.FormatProperty(), Value = Value.Evaluate(env).ToString() });
        }

    }
}
