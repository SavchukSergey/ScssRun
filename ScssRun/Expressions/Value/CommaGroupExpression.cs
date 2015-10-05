using System.Linq;
using System.Text;

namespace ScssRun.Expressions.Value {
    public class CommaGroupExpression : Expression {

        public Expression[] Expressions { get; }

        public CommaGroupExpression(params Expression[] exprs) {
            Expressions = exprs;
        }

        public CommaGroupExpression Add(params Expression[] expressions) {
            return new CommaGroupExpression(Expressions.Concat(expressions).ToArray());
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            var sb = new StringBuilder();
            foreach (var expr in Expressions) {
                if (sb.Length != 0) sb.Append(',');
                sb.Append(expr.Evaluate(env));
            }
            return new CssValue {
                String = sb.ToString(),
                Type = CssValueType.String
            };
        }
    }
}
