namespace ScssRun.Expressions.Selectors {
    public class TypeSelector : SimpleSelector {

        public string Value { get; set; }

        public override string Evaluate(ScssEnvironment env) {
            return Value;
        }
    }
}
