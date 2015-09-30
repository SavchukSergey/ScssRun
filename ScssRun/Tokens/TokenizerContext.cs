namespace ScssRun.Tokens {

    public class TokenizerContext {

        private int _lineStart;
        private int _lineNumber = 1;

        public TokenizerContext(FileSource file) {
            File = file;
        }

        public int Position { get; set; }

        public FileSource File { get; }

        public void IncrementLine() {
            _lineStart = Position;
            _lineNumber++;
        }

        public TokenPosition CreatePosition() {
            return new TokenPosition {
                File = File,
                Line = _lineNumber,
                LineStart = _lineStart
            };
        }
    }
}
