using NUnit.Framework;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;

namespace ScssRun.Tests.Expressions.Selectors {
    [TestFixture]
    public class DescendantSelectorTests {

        [Test]
        public void ParseTest() {
            var expr = SelectorExpression.Parse("div p span");
            AssertExt.AreEqual(new DescendantCombinator(
                new TypeSelector("div"),
                new TypeSelector("p"),
                new TypeSelector("span")
                ), expr);
        }
    }
}
