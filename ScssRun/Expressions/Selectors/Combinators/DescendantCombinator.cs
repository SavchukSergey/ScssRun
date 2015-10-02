namespace ScssRun.Expressions.Selectors.Combinators {
    public class DescendantCombinator : Combinator {

        public SelectorExpression Left { get; set; }

        public SelectorExpression Right { get; set; }

        public override string Evaluate(ScssEnvironment env) {
            return Left.Evaluate(env) + " " + Right.Evaluate(env);
        }

    }
}
