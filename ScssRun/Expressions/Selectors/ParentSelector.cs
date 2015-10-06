namespace ScssRun.Expressions.Selectors {
    public class ParentSelector : SelectorExpression {

        public override string Evaluate(ScssEnvironment env) {
            return env.ScssRule.Evaluate(env);
        }

        public override bool HasExplicitParent => true;
    }
}
