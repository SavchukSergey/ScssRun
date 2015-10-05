namespace ScssRun.Expressions.Value {
    public class LiteralExpression : Expression {

        public string Value { get; }

        public LiteralExpression(string value) {
            Value = value;
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            return new CssValue {
                Type = CssValueType.String,
                String = Value
            };
        }
    }
}
