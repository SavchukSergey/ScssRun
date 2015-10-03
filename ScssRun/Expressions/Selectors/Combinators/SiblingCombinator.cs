namespace ScssRun.Expressions.Selectors.Combinators {
    public class SiblingCombinator : Combinator {

        public override string Evaluate(ScssEnvironment env) {
            return Left.Evaluate(env) + "+" + Right.Evaluate(env);
        }

    }
}
