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

        [Test]
        public void NestedParseTest() {
            const string css = "border: { style: solid; color: red;}";
            var tokens = new Tokenizer().Read(css);
            var context = new ScssParserContext(new TokensQueue(tokens));
            var node = RuleNode.Parse(context);
            Assert.AreEqual("border", node.Property);
            var nested = (NestedValueNode) node.Value;
            Assert.AreEqual(2, nested.Rules.Count);
            var rule1 = nested.Rules[0];
            var rule2 = nested.Rules[1];
            Assert.AreEqual("style", rule1.Property);
            Assert.AreEqual("color", rule2.Property);
        }

    }
}
