namespace ScssRun.Expressions.Selectors.Combinators {
    public class DescendantCombinator : BinaryCombinator {

        public DescendantCombinator(SelectorExpression parent, SelectorExpression child) : base(parent, child) {
        }

        public override string Evaluate(ScssEnvironment env) {
            return Left.Evaluate(env) + " " + Right.Evaluate(env);
        }

    }
}
