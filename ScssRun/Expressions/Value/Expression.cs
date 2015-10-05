using System;
using System.Collections.Generic;
using System.Linq;
using ScssRun.Expressions.Functions;
using ScssRun.Tokens;

namespace ScssRun.Expressions.Value {
    public abstract class Expression {

        public abstract CssValue Evaluate(ScssEnvironment env);

        protected static Expression ParseLiteral(Token token, TokensQueue queue) {
            if (!queue.Empty) {
                var preview = queue.Peek();
                if (preview.Type == TokenType.OpenParenthesis) {
                    return Func(token, queue);
                }
            }
            return new LiteralExpression(token.StringValue);
        }

        protected static Expression Func(Token nameToken, TokensQueue tokens) {
            tokens.Read(TokenType.OpenParenthesis);
            var args = ParseArguments(tokens);
            tokens.Read(TokenType.CloseParenthesis);
            switch (nameToken.StringValue.ToLower()) {
                case "round":
                    if (args.Count != 1) {
                        throw new TokenException("expected 1 argument", nameToken);
                    }
                    return new RoundFunctionExpression(args.First());
                default:
                    throw new TokenException("unknown function " + nameToken.StringValue, nameToken);
            }
        }

        public static Expression Parse(string source) {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read(source);
            var queue = new TokensQueue(tokens);
            return Parse(queue);
        }

        public static Expression Parse(TokensQueue tokens) {
            return ParseWithPriority(tokens, -10);
        }

        public static IList<Expression> ParseArguments(TokensQueue tokens) {
            var res = new List<Expression>();
            while (!tokens.Empty) {
                var preview = tokens.Peek();
                if (preview.Type == TokenType.CloseParenthesis) return res;
                var arg = Parse(tokens);
                res.Add(arg);
                preview = tokens.Peek();
                if (preview.Type != TokenType.Comma) break;
                tokens.Read(TokenType.Comma);
            }
            return res;
        }

        private static Expression ParseNumber(ref Token token, TokensQueue queue) {
            var inner = new NumberExpression(token.NumberValue);
            if (!queue.Empty) {
                var preview = queue.Peek();
                if (preview.Type == TokenType.Literal || preview.Type == TokenType.Percentage) {
                    var unitToken = queue.Read();
                    var unit = ParseUnit(ref unitToken);
                    return new UnitExpression(inner, unit);
                }
            }
            return inner;
        }

        private static CssValueType ParseUnit(ref Token token) {
            switch (token.StringValue) {
                case "px": return CssValueType.Pixel;
                case "em": return CssValueType.Em;
                case "rem": return CssValueType.Rem;
                case "in": return CssValueType.Inch;
                case "cm": return CssValueType.Centimeter;
                case "%": return CssValueType.Percentage;
                case "vh": return CssValueType.ViewportHeight;
                case "vw": return CssValueType.ViewportWidth;
                default:
                    throw new TokenException("invalid unit", token);
            }
        }

        private static Expression ParseOperand(TokensQueue tokens) {
            tokens.SkipWhiteAndComments();
            var token = tokens.Read();
            switch (token.Type) {
                case TokenType.Number: return ParseNumber(ref token, tokens);
                case TokenType.Literal: return ParseLiteral(token, tokens);
                case TokenType.Minus: return new NegateExpression(ParseOperand(tokens));
                case TokenType.OpenParenthesis:
                    var inner = Parse(tokens);
                    tokens.Read(TokenType.CloseParenthesis);
                    return inner;
                default:
                    throw new TokenException("unexpected token " + token.StringValue, token);
            }
        }

        private static Expression ParseWithPriority(TokensQueue tokens, int priority) {
            var left = ParseOperand(tokens);
            Token? whiteToken = null;
            while (!tokens.Empty) {
                tokens.SkipComments();
                var preview = tokens.Peek();
                switch (preview.Type) {
                    case TokenType.Semicolon:
                    case TokenType.CloseParenthesis:
                    case TokenType.ExclamationPoint:
                        return left;
                }

                var tokenPriority = GetPriority(preview.Type);
                if (tokenPriority < priority) {
                    return left;
                }

                switch (preview.Type) {
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Multiply:
                    case TokenType.Divide:
                    case TokenType.Percentage:
                    case TokenType.LeftShift:
                    case TokenType.RightShift:
                    case TokenType.Comma:
                        var token = tokens.Read();
                        left = ProcessBinaryExpression(token, left, tokens);
                        whiteToken = null;
                        break;
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        tokens.Read();
                        break;
                    case TokenType.Whitespace:
                        whiteToken = tokens.Read();
                        break;
                    default:
                        if (whiteToken.HasValue) {
                            left = ProcessBinaryExpression(whiteToken.Value, left, tokens);
                            whiteToken = null;
                            break;
                        }
                        throw new TokenException("unexpected token " + preview.StringValue, preview);
                }
            }

            return left;
        }

        private static Expression ProcessBinaryExpression(Token opToken, Expression left, TokensQueue tokens) {
            var tokenPriority = GetPriority(opToken.Type);
            var other = ParseWithPriority(tokens, tokenPriority + 1);
            switch (opToken.Type) {
                case TokenType.Plus: return new AddExpression(left, other);
                case TokenType.Minus: return new SubExpression(left, other);
                case TokenType.Multiply: return new MulExpression(left, other);
                case TokenType.Divide: return new DivExpression(left, other);
                case TokenType.Percentage: return new ModExpression(left, other);
                case TokenType.Whitespace:
                    if (left is SpaceGroupExpression) {
                        return ((SpaceGroupExpression)left).Add(other);
                    }
                    return new SpaceGroupExpression(left, other);
                case TokenType.Comma:
                    if (left is CommaGroupExpression) {
                        return ((CommaGroupExpression)left).Add(other);
                    }
                    return new CommaGroupExpression(left, other);
                default:
                    throw new TokenException("unexpected operator " + opToken.Type, opToken);
            }
        }

        private static int GetPriority(TokenType type) {
            switch (type) {
                case TokenType.Comma:
                    return -2;
                case TokenType.Whitespace:
                    return -1;
                case TokenType.Plus:
                case TokenType.Minus:
                    return 0;
                case TokenType.Multiply:
                case TokenType.Divide:
                    return 1;
                case TokenType.Percentage:
                    return 2;

                default:
                    return -1;
            }
        }
    }
}
