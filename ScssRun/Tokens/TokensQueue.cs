using System.Collections.Generic;

namespace ScssRun.Tokens {
    public class TokensQueue {

        private readonly Queue<Token> _queue;

        public TokensQueue(IEnumerable<Token> tokens) {
            _queue = new Queue<Token>(tokens);
        }

        public Token Read() {
            LastReadToken = _queue.Dequeue();
            return LastReadToken;
        }

        public Token Read(TokenType type) {
            if (_queue.Count == 0) throw new TokenException("unexpected end of file", LastReadToken); //TODO: what if file is empty??
            if (_queue.Count == 0 || Peek().Type != type) {
                var tkn = _queue.Count > 0 ? Read() : LastReadToken;
                switch (type) {
                    case TokenType.Comma:
                        throw new TokenException("comma expected", tkn);
                    case TokenType.Colon:
                        throw new TokenException("colon expected", tkn);
                    case TokenType.OpenCurlyBracket:
                        throw new TokenException("open curly bracket expected", tkn);
                    case TokenType.CloseCurlyBracket:
                        throw new TokenException("close curly bracket expected", tkn);
                    case TokenType.CloseParenthesis:
                        throw new TokenException("closing parenthesis expected", tkn);
                    default:
                        throw new TokenException("unexpected token", tkn);
                }
            }
            return Read();
        }

        public TokensQueue SkipWhitespaceAndComments() {
            while (!Empty) {
                var preview = _queue.Peek();
                if (preview.Type == TokenType.Whitespace || preview.Type == TokenType.SingleLineComment || preview.Type == TokenType.MultiLineComment) {
                    break;
                }
                _queue.Dequeue();
            }
            return this;
        }

        public Token Peek() {
            if (_queue.Count == 0) throw new TokenException("unexpected end of file", LastReadToken); //TODO: what if file is empty??
            return _queue.Peek();
        }

        public bool Empty => _queue.Count == 0;

        public int Count => _queue.Count;

        public Token LastReadToken { get; private set; }

    }
}
