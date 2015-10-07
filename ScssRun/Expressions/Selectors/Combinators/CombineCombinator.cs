using System.Linq;
using System.Text;

namespace ScssRun.Expressions.Selectors.Combinators {
    public class CombineCombinator : Combinator {

        public SelectorExpression[] Expressions { get; }

        public CombineCombinator(params SelectorExpression[] exprs) {
            Expressions = exprs;
        }

        public CombineCombinator Add(params SelectorExpression[] exprs) {
            return new CombineCombinator(Expressions.Concat(exprs).ToArray());
        }

        public override string Evaluate(ScssEnvironment env) {
            var sb = new StringBuilder();
            foreach (var expr in Expressions) {
                sb.Append(expr.Evaluate(env));
            }
            return sb.ToString();
        }
    }
}
