using NUnit.Framework;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;

namespace ScssRun.Tests.Expressions.Selectors {
    [TestFixture]
    public class NestedCombinatorTests {

        [Test]
        public void SimpleTest() {
            var first = new TypeSelector("div");
            var second = new TypeSelector("p");
            var nested = NestedCombinator.Nest(first, second);
            AssertExt.AreEqual(new DescendantCombinator(first, second), nested);
        }
    }
}
