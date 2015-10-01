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
            return new LiteralExpression { Value = token.StringValue };
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
            return ParseWithPriority(tokens, 0);
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
            var inner = new NumberExpression { Value = token.NumberValue };
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

        private static Expression ParseOperand(TokensQueue tokens)
        {
            tokens.SkipWhite();
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
            while (!tokens.Empty) {
                var preview = tokens.Peek();
                switch (preview.Type) {
                    case TokenType.Comma:
                    case TokenType.CloseParenthesis:
                        return left;
                }

                var tokenPriority = GetPriority(preview.Type);
                if (tokenPriority >= 0 && tokenPriority < priority) {
                    if (left != null) return left;
                    throw new Exception("some case");
                }

                var token = tokens.Read();
                switch (token.Type) {
                    case TokenType.Plus:
                    case TokenType.Minus:
                    case TokenType.Multiply:
                    case TokenType.Divide:
                    case TokenType.Percentage:
                    case TokenType.LeftShift:
                    case TokenType.RightShift:
                        left = ProcessBinaryExpression(token, left, tokens);
                        break;
                    case TokenType.Whitespace:
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        break;
                    default:
                        throw new TokenException("unexpected token " + token.StringValue, token);
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
                default:
                    throw new TokenException("unexpected operator", opToken);
            }
        }

        private static int GetPriority(TokenType type) {
            switch (type) {
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
