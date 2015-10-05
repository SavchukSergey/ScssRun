namespace ScssRun.Expressions.Value {
    public class NumberExpression : Expression {

        public double Value { get; }

        public NumberExpression(double value) {
            Value = value;
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            return new CssValue {
                Number = Value,
                Type = CssValueType.Number
            };
        }
    }
}
