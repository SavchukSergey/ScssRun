namespace ScssRun.Expressions.Selectors {
    public class NotExpression : SelectorExpression {

        public SimpleSelector Inner { get; }

        public NotExpression(SimpleSelector inner) {
            Inner = inner;
        }

        public override string Evaluate(ScssEnvironment env) {
            return $":not({Inner.Evaluate(env)})";
        }
    }
}
