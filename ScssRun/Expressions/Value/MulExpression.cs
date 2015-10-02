using System;

namespace ScssRun.Expressions.Value {
    public class MulExpression : BinaryExpression {

        public MulExpression(Expression left, Expression right)
            : base(left, right) {
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            var l = Left.Evaluate(env);
            var r = Right.Evaluate(env);
            if (l.Type == CssValueType.Number) {
                var t = l;
                l = r;
                r = t;
            }
            if (r.Type == CssValueType.Number) {
                switch (l.Type) {
                    case CssValueType.Number:
                    case CssValueType.Percentage:
                    case CssValueType.Pixel:
                    case CssValueType.Em:
                    case CssValueType.Rem:
                    case CssValueType.ViewportWidth:
                    case CssValueType.ViewportHeight:
                    case CssValueType.Inch:
                    case CssValueType.Centimeter:
                        return new CssValue {
                            Type = l.Type,
                            Number = l.Number * r.Number
                        };
                }
            }
            throw new Exception("unit mismatch");
        }

    }
}
