using System.Linq;
using System.Text;

namespace ScssRun.Expressions.Selectors.Combinators {
    public class GroupCombinator : Combinator {

        public SelectorExpression[] Expressions { get; }

        public GroupCombinator(params SelectorExpression[] exprs) {
            Expressions = exprs;
        }

        public GroupCombinator Add(params SelectorExpression[] exprs) {
            return new GroupCombinator(Expressions.Concat(exprs).ToArray());
        }

        public override string Evaluate(ScssEnvironment env) {
            var sb = new StringBuilder();
            foreach (var expr in Expressions) {
                if (sb.Length != 0) sb.Append(',');
                sb.Append(expr.Evaluate(env));
            }
            return sb.ToString();
        }

        public override SelectorExpression WrapWithParent(ScssEnvironment env) {
            return new GroupCombinator(Expressions.Select(e => e.WrapWithParent(env)).ToArray());
        }
    }
}
