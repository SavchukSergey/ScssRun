using System;
using ScssRun.Expressions.Selectors.Combinators;
using ScssRun.Tokens;

namespace ScssRun.Expressions.Selectors {
    //http://www.w3.org/TR/css3-selectors/
    public abstract class SelectorExpression {

        public static SelectorExpression Parse(string source) {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read(source);
            var queue = new TokensQueue(tokens);
            return Parse(queue);
        }

        public static SelectorExpression Parse(TokensQueue tokens) {
            return ParseWithPriority(tokens, 0);

        }

        private static SelectorExpression ParseWithPriority(TokensQueue tokens, int priority) {
            var left = ParseOperand(tokens);
            while (!tokens.Empty) {
                tokens.SkipComments();
                var preview = tokens.Peek();
                if (preview.Type == TokenType.OpenCurlyBracket) {
                    return left;
                }

                var tokenPriority = GetPriority(preview.Type);
                if (tokenPriority >= 0 && tokenPriority < priority) {
                    return left;
                }

                var token = tokens.Read();
                switch (token.Type) {
                    case TokenType.Comma:
                        return new GroupCombinator {
                            Left = left,
                            Right = Parse(tokens)
                        };
                    case TokenType.Whitespace:
                        tokens.Read();
                        tokens.SkipComments();
                        if (!tokens.Empty && tokens.Peek().Type != TokenType.OpenCurlyBracket) {
                            return new DescendantCombinator {
                                Left = left,
                                Right = Parse(tokens)
                            };
                        }
                        break;
                    case TokenType.Greater:
                        return new  ChildCombinator {
                            Left = left,
                            Right = Parse(tokens)
                        };
                    case TokenType.Plus:
                        return new ChildCombinator {
                            Left = left,
                            Right = Parse(tokens)
                        };
                    default:
                        throw new TokenException("unexpected token " + token.StringValue, token);
                }
            }

            return left;
        }

        private static SelectorExpression ParseOperand(TokensQueue tokens) {
            tokens.SkipWhiteAndComments();
            var token = tokens.Read();
            switch (token.Type) {
                case TokenType.Literal: return ParseTypeSelector(token, tokens);
                default:
                    throw new TokenException("unexpected token " + token.StringValue, token);
            }
        }

        protected static SelectorExpression ParseTypeSelector(Token token, TokensQueue queue) {
            return new TypeSelector { Value = token.StringValue };
        }

        public abstract string Evaluate(ScssEnvironment env);

        private static int GetPriority(TokenType type) {
            switch (type) {
                case TokenType.Comma:
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
