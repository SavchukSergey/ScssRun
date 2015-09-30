using System;

namespace ScssRun.Tokens {
    public class TokenException : Exception {

        public TokenException(string message, Token token)
            : base($"{message} at line: {token.Position.Line}, column: {token.Position.Column}") {
            Token = token;
        }

        public Token Token { get; }

    }
}
