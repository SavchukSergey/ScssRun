using System.Xml;
using ScssRun.IO;
using ScssRun.Nodes;
using ScssRun.Tokens;

namespace ScssRun {
    public class ScssParser {

        private readonly IScssSource _source;

        public ScssParser(IScssSource source) {
            _source = source;
        }

        public ScssParser() : this(new NullScssSource()) {
        }

        public ScssDocumentNode Parse(string scss) {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read(scss);
            return ScssDocumentNode.Parse(new ScssParserContext(new TokensQueue(tokens)));
        }

    }
}
