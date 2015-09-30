using ScssRun.IO;
using ScssRun.Nodes;

namespace ScssRun {
    public class ScssParser {

        private readonly IScssSource _source;

        public ScssParser(IScssSource source) {
            _source = source;
        }

        public ScssParser() : this(new NullScssSource()) {
        }

        public ScssDocumentNode Parse(string scss) {
            return new ScssDocumentNode();
        }
    }
}
