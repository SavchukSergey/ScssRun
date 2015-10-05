using System.Linq;
using System.Text;

namespace ScssRun.Expressions.Value {
    public class SpaceGroupExpression : Expression {

        public Expression[] Expressions { get; }

        public SpaceGroupExpression(params Expression[] exprs) {
            Expressions = exprs;
        }

        public SpaceGroupExpression Add(params Expression[] expressions) {
            return new SpaceGroupExpression(Expressions.Concat(expressions).ToArray());
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            var sb = new StringBuilder();
            foreach (var expr in Expressions) {
                if (sb.Length != 0) sb.Append(' ');
                sb.Append(expr.Evaluate(env));
            }
            return new CssValue {
                String = sb.ToString(),
                Type = CssValueType.String
            };
        }
    }
}
