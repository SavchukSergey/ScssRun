using System;

namespace ScssRun.Tokens {
    public class TokenException : Exception {

        public TokenException(string message, Token token)
            : base(message) {
            Token = token;
        }

        public Token Token { get; }

    }
}
