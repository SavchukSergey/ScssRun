using System;
using ScssRun.Expressions.Value;

namespace ScssRun.Expressions.Functions {
    public class RoundFunctionExpression : Expression {

        private readonly Expression _inner;

        public RoundFunctionExpression(Expression inner) {
            _inner = inner;
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            var l = _inner.Evaluate(env);
            switch (l.Type) {
                case CssValueType.Number:
                case CssValueType.Percentage:
                case CssValueType.Pixel:
                case CssValueType.Em:
                case CssValueType.Rem:
                case CssValueType.Inch:
                    return new CssValue {
                        Type = l.Type,
                        Number = Math.Round(l.Number)
                    };
                default:
                    throw new Exception("unit mismatch");
            }
        }
    }
}
