using System.Linq;
using System.Text;

namespace ScssRun.Expressions.Selectors.Combinators {
    public class DescendantCombinator : Combinator {

        public SelectorExpression[] Expressions { get; }

        public DescendantCombinator(params SelectorExpression[] exprs) {
            Expressions = exprs;
        }

        public DescendantCombinator Add(params SelectorExpression[] exprs) {
            return new DescendantCombinator(Expressions.Concat(exprs).ToArray());
        }

        public override string Evaluate(ScssEnvironment env) {
            var sb = new StringBuilder();
            foreach (var expr in Expressions) {
                if (sb.Length != 0) sb.Append(' ');
                sb.Append(expr.Evaluate(env));
            }
            return sb.ToString();
        }
    }
}
