using NUnit.Framework;
using ScssRun.Css;

namespace ScssRun.Tests {
    [TestFixture]
    public class ScssParserTest {

        [Test]
        public void ParserTest() {
            var parser = new ScssParser();
            var doc = parser.Parse("p { width: 20px; }");
            var writer = new CssWriter(CssWriterOptions.Minified);
            doc.ToCss(writer, new ScssEnvironment());
            Assert.AreEqual("p{width:20px;}", writer.Result);
        }
    }
}
