namespace ScssRun.Expressions.Value {
    public class UnitExpression : Expression {
        private readonly Expression _inner;
        private readonly CssValueType _type;

        public UnitExpression(Expression inner, CssValueType type) {
            _inner = inner;
            _type = type;
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            var val = _inner.Evaluate(env);
            return new CssValue {
                Number = val.Number,
                Type = _type
            };
        }
    }
}
