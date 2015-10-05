using NUnit.Framework;
using ScssRun.Expressions.Value;

namespace ScssRun.Tests.Expressions.Value {
    [TestFixture]
    public class ExpressionTest {

        [Test]
        [TestCase("10px", 10, CssValueType.Pixel)]
        [TestCase("10em", 10, CssValueType.Em)]
        [TestCase("10rem", 10, CssValueType.Rem)]
        [TestCase("10in", 10, CssValueType.Inch)]
        [TestCase("10cm", 10, CssValueType.Centimeter)]
        [TestCase("10vw", 10, CssValueType.ViewportWidth)]
        [TestCase("10vh", 10, CssValueType.ViewportHeight)]
        [TestCase("10%", 10, CssValueType.Percentage)]
        [TestCase("10", 10, CssValueType.Number)]
        [TestCase("-10px", -10, CssValueType.Pixel)]
        [TestCase("1.5", 1.5, CssValueType.Number)]
        [TestCase(".5", .5, CssValueType.Number)]
        [TestCase("1.5e2", 1.5e2, CssValueType.Number)]
        [TestCase("1.5e+2", 1.5e+2, CssValueType.Number)]
        [TestCase("1.5e-2", 1.5e-2, CssValueType.Number)]
        public void NumberUnitTest(string expression, double val, CssValueType unit) {
            var expr = Expression.Parse(expression);
            var eval = expr.Evaluate(new ScssEnvironment());
            Assert.AreEqual(val, eval.Number);
            Assert.AreEqual(unit, eval.Type);
        }

        [Test]
        [TestCase("10px + 20px", 30, CssValueType.Pixel)]
        [TestCase("10em + 20em", 30, CssValueType.Em)]
        [TestCase("10rem + 20rem", 30, CssValueType.Rem)]
        [TestCase("10in + 20in", 30, CssValueType.Inch)]
        [TestCase("10cm + 20cm", 30, CssValueType.Centimeter)]
        [TestCase("10vw + 20vw", 30, CssValueType.ViewportWidth)]
        [TestCase("10vh + 20vh", 30, CssValueType.ViewportHeight)]
        [TestCase("10% + 20%", 30, CssValueType.Percentage)]
        [TestCase("10 + 20", 30, CssValueType.Number)]
        [TestCase("10px - 20px", -10, CssValueType.Pixel)]
        [TestCase("10em - 20em", -10, CssValueType.Em)]
        [TestCase("10rem - 20rem", -10, CssValueType.Rem)]
        [TestCase("10in - 20in", -10, CssValueType.Inch)]
        [TestCase("10cm - 20cm", -10, CssValueType.Centimeter)]
        [TestCase("10vw - 20vw", -10, CssValueType.ViewportWidth)]
        [TestCase("10vh - 20vh", -10, CssValueType.ViewportHeight)]
        [TestCase("10% - 20%", -10, CssValueType.Percentage)]
        [TestCase("10 - 20", -10, CssValueType.Number)]
        [TestCase("10px * 20", 200, CssValueType.Pixel)]
        [TestCase("10em * 20", 200, CssValueType.Em)]
        [TestCase("10rem * 20", 200, CssValueType.Rem)]
        [TestCase("10in * 20", 200, CssValueType.Inch)]
        [TestCase("10cm * 20", 200, CssValueType.Centimeter)]
        [TestCase("10vw * 20", 200, CssValueType.ViewportWidth)]
        [TestCase("10vh * 20", 200, CssValueType.ViewportHeight)]
        [TestCase("10% * 20", 200, CssValueType.Percentage)]
        [TestCase("10 * 20", 200, CssValueType.Number)]
        [TestCase("20 * 10px", 200, CssValueType.Pixel)]
        [TestCase("20 * 10em", 200, CssValueType.Em)]
        [TestCase("20 * 10rem", 200, CssValueType.Rem)]
        [TestCase("20 * 10in", 200, CssValueType.Inch)]
        [TestCase("20 * 10cm", 200, CssValueType.Centimeter)]
        [TestCase("20 * 10vw", 200, CssValueType.ViewportWidth)]
        [TestCase("20 * 10vh", 200, CssValueType.ViewportHeight)]
        [TestCase("20 * 10%", 200, CssValueType.Percentage)]
        [TestCase("20 * 10", 200, CssValueType.Number)]
        public void MathTest(string expression, double val, CssValueType unit) {
            var expr = Expression.Parse(expression);
            var eval = expr.Evaluate(new ScssEnvironment());
            Assert.AreEqual(val, eval.Number);
            Assert.AreEqual(unit, eval.Type);
        }

        [TestCase("round(200.3px)", 200, CssValueType.Pixel)]
        [TestCase("round(200.3em)", 200, CssValueType.Em)]
        [TestCase("round(200.3rem)", 200, CssValueType.Rem)]
        [TestCase("round(200.3in)", 200, CssValueType.Inch)]
        [TestCase("round(200.3cm)", 200, CssValueType.Centimeter)]
        [TestCase("round(200.3vw)", 200, CssValueType.ViewportWidth)]
        [TestCase("round(200.3vh)", 200, CssValueType.ViewportHeight)]
        [TestCase("round(200.3%)", 200, CssValueType.Percentage)]
        [TestCase("round(200.3)", 200, CssValueType.Number)]
        public void FunctionsTest(string expression, double val, CssValueType unit) {
            var expr = Expression.Parse(expression);
            var eval = expr.Evaluate(new ScssEnvironment());
            Assert.AreEqual(val, eval.Number);
            Assert.AreEqual(unit, eval.Type);
        }

        [Test]
        public void GroupsTest() {
            var expr = Expression.Parse("10px + 20px 2px, solid chuck norris, red");
            AssertExt.AreEqual(
                new CommaGroupExpression(
                    new SpaceGroupExpression(
                        new AddExpression(
                            new UnitExpression(new NumberExpression(10), CssValueType.Pixel),
                            new UnitExpression(new NumberExpression(20), CssValueType.Pixel)
                        ),
                        new UnitExpression(new NumberExpression(2), CssValueType.Pixel)
                    ),
                    new SpaceGroupExpression(
                        new LiteralExpression("solid"),
                        new LiteralExpression("chuck"),
                        new LiteralExpression("norris")
                    ),
                    new LiteralExpression("red")), expr);
        }
    }
}
