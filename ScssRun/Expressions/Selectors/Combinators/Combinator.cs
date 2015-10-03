namespace ScssRun.Expressions.Selectors.Combinators {
    public abstract class Combinator : SelectorExpression {

        protected Combinator() {
        }

        protected Combinator(SelectorExpression left, SelectorExpression right) {
            Left = left;
            Right = right;
        }

        public SelectorExpression Left { get; set; }

        public SelectorExpression Right { get; set; }

    }
}
