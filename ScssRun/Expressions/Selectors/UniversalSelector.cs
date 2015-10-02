namespace ScssRun.Expressions.Selectors {
    public class UniversalSelector : SimpleSelector {
        public override string Evaluate(ScssEnvironment env) {
            return "*";
        }
    }
}
