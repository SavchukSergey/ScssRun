using NUnit.Framework;
using ScssRun.Nodes;
using ScssRun.Tokens;

namespace ScssRun.Tests.Nodes {
    [TestFixture]
    public class SelectorNodeTest {

        [Test]
        public void ParseTest() {
            const string css = "div.primary p.secondary a .first.second #elementId {color: red;}";
            var tokens = new Tokenizer().Read(css);
            var context = new SscsParserContext(new TokensQueue(tokens));
            var node = SelectorNode.Parse(context);
            Assert.AreEqual(5, node.Elements.Count);

            Assert.AreEqual("div.primary", node.Elements[0].Value);
            Assert.AreEqual("p.secondary", node.Elements[1].Value);
            Assert.AreEqual("a", node.Elements[2].Value);
            Assert.AreEqual(".first.second", node.Elements[3].Value);
            Assert.AreEqual("#elementId", node.Elements[4].Value);
        }
    }
}
