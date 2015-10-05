using ScssRun.Tokens;

namespace ScssRun {
    public class ScssParserContext {

        public TokensQueue Tokens { get; }

        public ScssParserContext(TokensQueue tokens) {
            Tokens = tokens;
        }

    }
}
