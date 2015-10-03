namespace ScssRun.Expressions.Selectors.Combinators {
    public class ChildCombinator : Combinator {

        public override string Evaluate(ScssEnvironment env) {
            return Left.Evaluate(env) + ">" + Right.Evaluate(env);
        }

    }
}
