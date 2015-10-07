using NUnit.Framework;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;

namespace ScssRun.Tests.Expressions.Selectors {
    [TestFixture]
    public class CombineSelectorTests {

        [Test]
        public void ParseTest() {
            var expr = SelectorExpression.Parse("div.primary.secondary");
            AssertExt.AreEqual(new CombineCombinator(
                new TypeSelector("div"),
                new ClassSelector("primary"),
                new ClassSelector("secondary")
                ), expr);
        }
    }
}
