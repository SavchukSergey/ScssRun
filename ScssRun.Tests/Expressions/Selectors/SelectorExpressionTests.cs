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
            AssertExt.AreEqual(new TypeSelector("div"), expr);
        }

        [Test]
        [TestCase("div p")]
        [TestCase("div   p")]
        [TestCase("div /*qwe*/ p")]
        public void DescendantTest(string expression) {
            var expr = SelectorExpression.Parse(expression);
            AssertExt.AreEqual(new DescendantCombinator(
                new TypeSelector("div"),
                new TypeSelector("p")
                ), expr);
        }

        [Test]
        [TestCase("#test")]
        [TestCase("#test {")]
        public void IdTest(string expression) {
            var expr = SelectorExpression.Parse(expression);
            AssertExt.AreEqual(new IdSelector("test"),  expr);
        }

        [Test]
        public void PseudoClassTest() {
            var expr = SelectorExpression.Parse(":before");
            AssertExt.AreEqual(new PseudoClassSelector("before"), expr);

            expr = SelectorExpression.Parse("div:before");
            AssertExt.AreEqual(new CombineCombinator(
                new TypeSelector("div"), 
                new PseudoClassSelector("before")
            ), expr);
        }

        [Test]
        public void NotExpressionTest() {
            var expr = SelectorExpression.Parse("div:not(.primary)");
            AssertExt.AreEqual(new CombineCombinator(
                new TypeSelector("div"),
                new NotExpression(new ClassSelector("primary"))
            ), expr);
        }

        [Test]
        public void AttributeExistsTest() {
            var expr = SelectorExpression.Parse("div[data-value]");
            AssertExt.AreEqual(new CombineCombinator(
                new TypeSelector("div"),
                new AttributeExistsSelector("data-value")
            ), expr);
        }

        [Test]
        public void AttributeEqualsTest() {
            var expr = SelectorExpression.Parse("div[data-value=qwe]");
            AssertExt.AreEqual(new CombineCombinator(
                new TypeSelector("div"),
                new AttributeEqualsSelector("data-value", "qwe")
            ), expr);
        }

        [Test]
        public void PriorityTest() {
            const string expression = "div.wrapper p.msg, div.header a.link";
            var expr = SelectorExpression.Parse(expression);
            AssertExt.AreEqual(new GroupCombinator(
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

    }
}
