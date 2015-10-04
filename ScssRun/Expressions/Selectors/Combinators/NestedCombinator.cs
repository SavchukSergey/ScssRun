using System.Collections.Generic;

namespace ScssRun.Expressions.Selectors.Combinators {
    public class NestedCombinator : BinaryCombinator {

        private readonly SelectorExpression _inner;

        public NestedCombinator(SelectorExpression parent, SelectorExpression child) : base(parent, child) {
            _inner = Nest(parent, child);
        }

        public override string Evaluate(ScssEnvironment env) {
            return _inner.Evaluate(env);
        }

        public static SelectorExpression Nest(SelectorExpression parent, SelectorExpression child) {
            var list = new List<SelectorExpression>();
            var parentList = ExpandGroups(parent);
            var childList = ExpandGroups(child);
            foreach (var parentItem in parentList) {
                foreach (var childItem in childList) {
                    list.Add(Combinate(parentItem, childItem));
                }
            }
            if (list.Count == 1) return list[0];
            return new GroupCombinator(list.ToArray());
        }

        private static SelectorExpression Combinate(SelectorExpression parent, SelectorExpression child) {
            return new DescendantCombinator(parent, child);
        }

        private static List<SelectorExpression> ExpandGroups(SelectorExpression expression) {
            var target = new List<SelectorExpression>();
            ExpandGroups(expression, target);
            return target;
        }

        private static void ExpandGroups(SelectorExpression expression, List<SelectorExpression> target) {
            var gr = expression as GroupCombinator;
            if (gr != null) {
                foreach (var sub in gr.Expressions) {
                    ExpandGroups(sub, target);
                }
            } else {
                target.Add(expression);
            }
        }
    }
}
