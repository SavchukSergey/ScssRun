using NUnit.Framework;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;
using ScssRun.Nodes;
using ScssRun.Tokens;

namespace ScssRun.Tests.Nodes {
    [TestFixture]
    public class RuleSetNodeTest {


        [Test]
        public void ParseTest() {
            const string css = "div {color: red;}";
            var tokens = new Tokenizer().Read(css);
            var context = new ScssParserContext(new TokensQueue(tokens));
            var node = RuleSetNode.Parse(context);
            AssertExt.AreEqual(new TypeSelector("div"), node.Selector);
            //todo: test declarations
        }
    }
}
