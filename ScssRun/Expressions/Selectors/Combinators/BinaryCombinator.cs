using System;

namespace ScssRun.Expressions.Selectors.Combinators {
    public abstract class BinaryCombinator : Combinator {

        protected BinaryCombinator(SelectorExpression left, SelectorExpression right) {
            Left = left;
            Right = right;
            if (left == null) throw new ArgumentNullException(nameof(left));
            if (right == null) throw new ArgumentNullException(nameof(right));
        }

        public SelectorExpression Left { get; set; }

        public SelectorExpression Right { get; set; }

        public override bool HasExplicitParent => Left.HasExplicitParent || Right.HasExplicitParent;
    }
}
