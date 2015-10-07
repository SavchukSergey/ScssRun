namespace ScssRun.Expressions.Selectors {
    public class ParentSelector : SelectorExpression {

        public SelectorExpression Inner { get; }

        public ParentSelector(SelectorExpression inner) {
            Inner = inner;
        }

        public override string Evaluate(ScssEnvironment env) {
            return Inner.Evaluate(env);
        }

        public override bool HasExplicitParent => true;
    }
}
