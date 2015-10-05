namespace ScssRun.Expressions.Value {
    public class UnitExpression : Expression {

        public Expression Inner { get; }

        public CssValueType Type { get; }

        public UnitExpression(Expression inner, CssValueType type) {
            Inner = inner;
            Type = type;
        }


        public override CssValue Evaluate(ScssEnvironment env) {
            var val = Inner.Evaluate(env);
            return new CssValue {
                Number = val.Number,
                Type = Type
            };
        }
    }
}
