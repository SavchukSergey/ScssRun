namespace ScssRun.Expressions.Selectors {
    public abstract class AttributeSelector : SimpleSelector {

        public string AttributeName { get; }

        protected AttributeSelector(string attributeName) {
            AttributeName = attributeName;
        }
    }
}
