using NUnit.Framework;
using ScssRun.Css;
using ScssRun.Nodes;

namespace ScssRun.Tests {
    [TestFixture]
    public class ScssParserTest {

        [Test]
        public void SimpleParseTest() {
            const string input = @"
#main p {
  color: #eeffdd;
  width: 97%;

  .redbox {
    background-color: #ffbca8;
    color: #bfadef;
  }
}";
            var doc = new ScssParser().Parse(input);
            var env = new ScssEnvironment();
            doc.Compile(env);
            var css = env.Document.ToString();
        }

        [Test]
        public void ParserTest() {
            var parser = new ScssParser();
            var doc = parser.Parse("p { width: 20px; }");
            var env = new ScssEnvironment();
            doc.Compile(env);
            Assert.AreEqual("p{width:20px;}", env.Document.ToString());
        }

        [Test]
        public void NestedRuleParserTest() {
            var parser = new ScssParser();
            var doc = parser.Parse("div { p { width: 20px; } span {color: red; } }");
            var env = new ScssEnvironment();
            doc.Compile(env);
            AssertExt.AreEqual(new CssDocument {
                Rules = {
                    new CssQualifiedRule ("div"),
                    new CssQualifiedRule ("div p") {
                        Declarations = {
                            new CssDeclaration("width", "20px")
                        }
                    },
                    new CssQualifiedRule ("div span") {
                        Declarations = {
                            new CssDeclaration("color", "red")
                        }
                    },
                }
            }, env.Document);
        }
    }
}
