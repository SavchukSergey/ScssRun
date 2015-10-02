namespace ScssRun.Expressions.Selectors.Combinators {
    public class GroupCombinator : Combinator {

        public SelectorExpression Left { get; set; }

        public SelectorExpression Right { get; set; }

        public override string Evaluate(ScssEnvironment env) {
            return Left.Evaluate(env) + "," + Right.Evaluate(env);
        }
    }
}
