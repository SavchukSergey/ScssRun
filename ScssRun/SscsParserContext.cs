using ScssRun.Tokens;

namespace ScssRun {
    public class SscsParserContext {

        public TokensQueue Tokens { get; }

        public SscsParserContext(TokensQueue tokens) {
            Tokens = tokens;
        }
    }
}
