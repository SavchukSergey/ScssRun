using NUnit.Framework;

namespace ScssRun.Tests {
    [TestFixture]
    public class ScssParserTest {

        [Test]
        public void ParserTest() {

            var parser = new ScssParser();
            var doc = parser.Parse("p { color :red; }");
        }
    }
}
