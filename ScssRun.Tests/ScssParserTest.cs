using NUnit.Framework;
using ScssRun.Css;

namespace ScssRun.Tests {
    [TestFixture]
    public class ScssParserTest {

        [Test]
        public void ParserTest() {
            var parser = new ScssParser();
            var doc = parser.Parse("p { width: 20px; }");
            var env = new ScssEnvironment();
            doc.Compile(env);
            Assert.AreEqual("p{width:20px;}", env.Document.ToString());
        }
    }
}
