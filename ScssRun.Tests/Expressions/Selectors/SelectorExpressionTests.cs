using System.Runtime.Remoting;
using NUnit.Framework;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;

namespace ScssRun.Tests.Expressions.Selectors {
    [TestFixture]
    public class SelectorExpressionTests {

        [Test]
        public void CurlyStopTest() {
            const string expression = "div {";
            var expr = SelectorExpression.Parse(expression);
            AssertAreEqual(new TypeSelector("div"), expr);
        }

        [Test]
        [TestCase("div p")]
        [TestCase("div   p")]
        [TestCase("div /*qwe*/ p")]
        public void DescendantTest(string expression) {
            var expr = SelectorExpression.Parse(expression);
            AssertAreEqual(new DescendantCombinator(
                new TypeSelector("div"),
                new TypeSelector("p")
                ), expr);
        }

        [Test]
        public void PriorityTest() {
            const string expression = "div.wrapper p.msg, div.header a.link";
            var expr = SelectorExpression.Parse(expression);
            AssertAreEqual(new GroupCombinator(
                new DescendantCombinator(
                    new CombineCombinator(
                        new TypeSelector("div"),
                        new ClassSelector("wrapper")),
                    new CombineCombinator(
                        new TypeSelector("p"),
                        new ClassSelector("msg"))),
                new DescendantCombinator(
                    new CombineCombinator(
                        new TypeSelector("div"),
                        new ClassSelector("header")),
                    new CombineCombinator(
                        new TypeSelector("a"),
                        new ClassSelector("link")
            ))), expr);
        }

        protected void AssertAreEqual(SelectorExpression expected, SelectorExpression actual) {
            Assert.AreEqual(expected.GetType(), actual.GetType());
            if (expected is GroupCombinator) {
                AssertAreEqual((GroupCombinator)expected, (GroupCombinator)actual);
            } else if (expected is DescendantCombinator) {
                AssertAreEqual((DescendantCombinator)expected, (DescendantCombinator)actual);
            } else if (expected is CombineCombinator) {
                AssertAreEqual((CombineCombinator)expected, (CombineCombinator)actual);
            } else if (expected is TypeSelector) {
                AssertAreEqual((TypeSelector)expected, (TypeSelector)actual);
            } else if (expected is ClassSelector) {
                AssertAreEqual((ClassSelector)expected, (ClassSelector)actual);
            } else {
                throw new AssertionException("unknown expression type " + expected.GetType());
            }
        }

        protected void AssertAreEqual(GroupCombinator expected, GroupCombinator actual) {
            AssertAreEqual(expected.Left, actual.Left);
            AssertAreEqual(expected.Right, actual.Right);
        }

        protected void AssertAreEqual(DescendantCombinator expected, DescendantCombinator actual) {
            AssertAreEqual(expected.Left, actual.Left);
            AssertAreEqual(expected.Right, actual.Right);
        }

        protected void AssertAreEqual(CombineCombinator expected, CombineCombinator actual) {
            AssertAreEqual(expected.Left, actual.Left);
            AssertAreEqual(expected.Right, actual.Right);
        }

        protected void AssertAreEqual(TypeSelector expected, TypeSelector actual) {
            Assert.AreEqual(expected.TypeName, actual.TypeName);
        }

        protected void AssertAreEqual(ClassSelector expected, ClassSelector actual) {
            Assert.AreEqual(expected.ClassName, actual.ClassName);
        }
    }
}
