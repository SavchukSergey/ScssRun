using NUnit.Framework;
using ScssRun.Nodes;
using ScssRun.Tokens;

namespace ScssRun.Tests.Nodes {
    [TestFixture]
    public class RuleTest {

        [Test]
        public void ParseTest() {
            const string css = "color: red;";
            var tokens = new Tokenizer().Read(css);
            var context = new ScssParserContext(new TokensQueue(tokens));
            var node = RuleNode.Parse(context);
            Assert.AreEqual("color", node.Property);
            Assert.AreEqual("red", ((ValuesNode)node.Value).Values[0].Value);

        }

    }
}
