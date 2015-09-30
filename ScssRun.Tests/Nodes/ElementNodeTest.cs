using NUnit.Framework;
using ScssRun.Nodes;
using ScssRun.Tokens;

namespace ScssRun.Tests.Nodes {
    [TestFixture]
    public class ElementNodeTest {

        [Test]
        public void ParseTest() {
            const string css = "div.primary p.secondary a .first.second #elementId {color: red;}";
            var tokens = new Tokenizer().Read(css);
            var context = new ScssParserContext(new TokensQueue(tokens));
            var node = ElementNode.Parse(context);
            Assert.AreEqual("div.primary", node.Value);

            node = ElementNode.Parse(context);
            Assert.AreEqual("p.secondary", node.Value);

            node = ElementNode.Parse(context);
            Assert.AreEqual("a", node.Value);

            node = ElementNode.Parse(context);
            Assert.AreEqual(".first.second", node.Value);

            node = ElementNode.Parse(context);
            Assert.AreEqual("#elementId", node.Value);

        }
    }
}
