namespace ScssRun.Expressions.Selectors.Combinators {
    public abstract class BinaryCombinator : Combinator {

        protected BinaryCombinator(SelectorExpression left, SelectorExpression right) {
            Left = left;
            Right = right;
        }

        public SelectorExpression Left { get; set; }

        public SelectorExpression Right { get; set; }

    }
}
