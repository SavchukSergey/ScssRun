using System.Collections.Generic;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;
using ScssRun.Tokens;

namespace ScssRun {
    public class ScssParserContext {

        private readonly Stack<SelectorExpression> _selectors = new Stack<SelectorExpression>();

        public TokensQueue Tokens { get; }

        public ScssParserContext(TokensQueue tokens) {
            Tokens = tokens;
        }

        public void PushSelector(SelectorExpression expression) {
            if (_selectors.Count > 0) {
                var parent = _selectors.Peek();
                expression = CrossGroup(parent, expression);
            }
            _selectors.Push(expression);
        }

        public void PopSelector() {
            _selectors.Pop();
        }

        public SelectorExpression Selector => _selectors.Count > 0 ? _selectors.Peek() : null;


        private static SelectorExpression CrossGroup(SelectorExpression parent, SelectorExpression child) {
            var parentList = ExpandGroups(parent);
            var childList = ExpandGroups(child);
            var list = new SelectorExpression[parentList.Count * childList.Count];
            var index = 0;
            foreach (var parentItem in parentList) {
                foreach (var childItem in childList) {
                    list[index++] = Combinate(parentItem, childItem);
                }
            }
            if (list.Length == 1) return list[0];
            return new GroupCombinator(list);
        }

        private static SelectorExpression Combinate(SelectorExpression parent, SelectorExpression child) {
            if (child.HasExplicitParent) return child;
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
