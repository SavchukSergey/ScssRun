namespace ScssRun.Expressions.Value {
    public class NumberExpression : Expression {

        public double Value { get; set; }

        public override CssValue Evaluate(ScssEnvironment env) {
            return new CssValue {
                Number = Value,
                Type = CssValueType.Number
            };
        }
    }
}
