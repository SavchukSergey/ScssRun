namespace ScssRun.Expressions.Selectors {
    public class AttributeEqualsSelector : AttributeSelector {

        public string Value { get; }

        public AttributeEqualsSelector(string attributeName, string value) : base(attributeName) {
            Value = value;
        }

        public override string Evaluate(ScssEnvironment env) {
            return $"[{AttributeName}=\"{Value}\"]";
        }
    }
}
