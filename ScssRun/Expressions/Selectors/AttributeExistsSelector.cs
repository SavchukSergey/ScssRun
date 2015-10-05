namespace ScssRun.Expressions.Selectors {
    public class AttributeExistsSelector : AttributeSelector {

        public AttributeExistsSelector(string attributeName) : base(attributeName) {
        }

        public override string Evaluate(ScssEnvironment env) {
            return $"[{AttributeName}]";
        }

    }
}
