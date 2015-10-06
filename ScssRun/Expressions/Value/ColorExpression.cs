using System.Drawing;

namespace ScssRun.Expressions.Value {
    public class ColorExpression : Expression {

        public Color Color { get; }

        public ColorExpression(Color color) {
            Color = color;
        }

        public ColorExpression(int r, int g, int b) {
            Color = Color.FromArgb(r, g, b);
        }

        public override CssValue Evaluate(ScssEnvironment env) {
            return new CssValue {
                Color = Color,
                Type = CssValueType.Color
            };
        }

    }
}
