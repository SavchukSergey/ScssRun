using System;

namespace ScssRun.Expressions.Value {
    public class NegateExpression : UnaryExpression {
        public NegateExpression(Expression inner)
            : base(inner) {
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            var l = Inner.Evaluate(env);
            switch (l.Type) {
                case CssValueType.Number:
                case CssValueType.Percentage:
                case CssValueType.Pixel:
                case CssValueType.Em:
                case CssValueType.Rem:
                case CssValueType.ViewportWidth:
                case CssValueType.ViewportHeight:
                case CssValueType.Inch:
                    return new CssValue {
                        Type = l.Type,
                        Number = -l.Number
                    };
                default:
                    throw new Exception("unit mismatch");
            }
        }
    }
}
