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
        public void AddTest(string expression, double val, CssValueType unit) {
            var expr = Expression.Parse(expression);
            var eval = expr.Evaluate(new ScssEnvironment());
            Assert.AreEqual(val, eval.Number);
            Assert.AreEqual(unit, eval.Type);
        }
    }
}
