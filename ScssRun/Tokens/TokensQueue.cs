﻿using System.Collections.Generic;
using System.Linq;

namespace ScssRun.Tokens {
    public class TokensQueue {

        private readonly IList<Token> _queue;
        private int _index;

        public TokensQueue(IEnumerable<Token> tokens) {
            _queue = tokens.ToList();
        }

        public TokensQueue(IList<Token> tokens) {
            _queue = tokens;
        }

        public Token Read() {
            LastReadToken = _queue[_index++];
            return LastReadToken;
        }

        public Token Read(TokenType type) {
            if (Empty) throw new TokenException("unexpected end of file", LastReadToken); //TODO: what if file is empty??
            if (Peek().Type != type) {
                var tkn = !Empty ? Read() : LastReadToken;
                switch (type) {
                    case TokenType.Literal:
                        throw new TokenException("literal expected", tkn);
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

        public Token Peek() {
            if (Empty) throw new TokenException("unexpected end of file", LastReadToken); //TODO: what if file is empty??
            return _queue[_index];
        }

        public TokensQueue Moment() {
            return new TokensQueue(_queue) {
                _index = _index
            };
        }

        public bool Empty => _index >= _queue.Count;

        public Token LastReadToken { get; private set; }

        public TokensQueue SkipWhite() {
            while (!Empty && Peek().Type == TokenType.Whitespace) Read();
            return this;
        }

        public TokensQueue SkipWhiteAndComments() {
            while (!Empty) {
                var preview = Peek();
                if (preview.Type == TokenType.Whitespace || preview.Type == TokenType.SingleLineComment ||
                    preview.Type == TokenType.MultiLineComment) {
                    Read();
                } else {
                    break;
                }
            }
            return this;
        }

        public TokensQueue SkipComments() {
            while (!Empty) {
                var preview = Peek();
                if (preview.Type == TokenType.SingleLineComment ||
                    preview.Type == TokenType.MultiLineComment) {
                    Read();
                } else {
                    break;
                }
            }
            return this;
        }

        public TokensQueue Skip(TokenType type) {
            if (!Empty) {
                var preview = Peek();
                if (preview.Type == type) Read();
            }
            return this;
        }
    }
}
