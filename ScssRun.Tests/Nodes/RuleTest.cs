using NUnit.Framework;
using ScssRun.Css;
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
            var node = ScssDeclarationNode.Parse(context);
            Assert.AreEqual("color", node.Property);
            Assert.AreEqual("red", ((ValuesNode)node.Value).Values[0].Value.Evaluate(new ScssEnvironment()).ToString());

        }

        [Test]
        public void NestedParseTest() {
            const string css = "border: { style: solid; color: red;}";
            var tokens = new Tokenizer().Read(css);
            var context = new ScssParserContext(new TokensQueue(tokens));
            var node = ScssDeclarationNode.Parse(context);
            Assert.AreEqual("border", node.Property);
            var nested = (NestedValueNode) node.Value;
            Assert.AreEqual(2, nested.Rules.Count);
            var rule1 = nested.Rules[0];
            var rule2 = nested.Rules[1];
            Assert.AreEqual("style", rule1.Property);
            Assert.AreEqual("color", rule2.Property);
        }

        [Test]
        public void ParserTest() {
            var parser = new ScssParser();
            var doc = parser.Parse(@"
p {
    border: {
        right: {
            style: solid;
            width: 1px;
            color: red;
        };
        left: {
            style: dashed;
            width: 2px;
            color: green;
        };
    };
    font: {
        family: Arial;
    }
}");
            var env = new ScssEnvironment();
            doc.Compile(env);
            Assert.AreEqual("p{border-right-style:solid;border-right-width:1px;border-right-color:red;border-left-style:dashed;border-left-width:2px;border-left-color:green;font-family:Arial;}", env.Document.ToString());
        }
    }
}
