namespace ScssRun.Expressions.Selectors.Combinators {
    public class SiblingCombinator : BinaryCombinator {

        public SiblingCombinator(SelectorExpression left, SelectorExpression right) : base(left, right) {
        }

        public override string Evaluate(ScssEnvironment env) {
            return Left.Evaluate(env) + "+" + Right.Evaluate(env);
        }

    }
}
