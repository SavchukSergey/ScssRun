using System.Collections.Generic;
using System.Text;

namespace ScssRun.Tokens {
    public class Tokenizer {

        public IList<Token> Read(string content) {
            return Read(new FileSource { Content = content, FileName = null });
        }


        public IList<Token> Read(FileSource source) {
            var context = new TokenizerContext(source);
            var content = source.Content;
            var res = new List<Token>();
            var len = content.Length;
            while (context.Position < len) {
                if (context.Position >= len) break;

                var position = context.CreatePosition();

                var ch = content[context.Position++];
                var preview = (char)(0xffff);
                if (context.Position < len) {
                    preview = content[context.Position];
                }
                if (ch == '>' && preview == '>') {
                    res.Add(new Token {
                        Type = TokenType.RightShift,
                        StringValue = ">",
                        Position = position
                    });
                    context.Position++;
                } else if (ch == '<' && preview == '<') {
                    res.Add(new Token {
                        Type = TokenType.LeftShift,
                        StringValue = "<",
                        Position = position
                    });
                    context.Position++;
                } else if (ch == '/' && preview == '/') {
                    context.Position++;
                    res.Add(ParseSingleLineComment(context));
                } else {
                    switch (ch) {
                        case '\r':
                        case '\n':
                        case '\t':
                        case ' ':
                            context.Position--;
                            res.Add(ParseWhitespace(context));
                            break;
                        case ';':
                            res.Add(new Token {
                                Position = position,
                                Type = TokenType.Semicolon
                            });
                            break;
                        case '"':
                        case '\'':
                            context.Position--;
                            res.Add(ParseString(context));
                            break;
                        default:
                            if (IsPunctuation(ch)) {
                                res.Add(new Token {
                                    Type = GetPunctuationTokenType(ch),
                                    StringValue = ch.ToString(),
                                    Position = position
                                });
                            } else if (char.IsDigit(ch)) {
                                context.Position--;
                                res.Add(ParseNumberToken(context));
                            } else {
                                context.Position--;
                                res.Add(ParseLiteral(context));
                            }
                            break;
                    }
                }
            }
            return res;
        }

        private static Token ParseSingleLineComment(TokenizerContext context) {
            var originalPos = context.Position;
            var len = context.File.Content.Length;
            var position = context.CreatePosition();
            while (context.Position < len) {
                var ch = context.File.Content[context.Position++];
                if (ch == '\r' || ch == '\n') {
                    if (context.Position < len) {
                        var nextCh = context.File.Content[context.Position];
                        if ((nextCh == '\r' || nextCh == '\n') && nextCh != ch) {
                            context.Position++;
                        }
                    }
                    context.IncrementLine();
                    break;
                }
            }
            var res = new Token {
                Type = TokenType.SingleLineComment,
                StringValue = context.File.Content.Substring(originalPos, context.Position - originalPos),
                Position = position
            };
            return res;
        }

        private static Token ParseWhitespace(TokenizerContext context) {
            var originalPos = context.Position;
            var len = context.File.Content.Length;
            var position = context.CreatePosition();
            while (context.Position < len) {
                var ch = context.File.Content[context.Position++];
                if (ch == '\r' || ch == '\n') {
                    if (context.Position < len) {
                        var nextCh = context.File.Content[context.Position];
                        if ((nextCh == '\r' || nextCh == '\n') && nextCh != ch) {
                            context.Position++;
                        }
                    }
                    context.IncrementLine();
                } else if (ch == ' ' || ch == '\t') {
                } else {
                    break;
                }
            }
            var res = new Token {
                Type = TokenType.Whitespace,
                StringValue = context.File.Content.Substring(originalPos, context.Position - originalPos),
                Position = position
            };
            return res;
        }

        private static Token ParseString(TokenizerContext context) {
            var quoteCh = context.File.Content[context.Position++];
            var token = new StringBuilder();
            var len = context.File.Content.Length;

            var position = context.CreatePosition();

            while (context.Position < len) {
                var ch = context.File.Content[context.Position++];
                if (ch == '\r' || ch == '\n') {
                    throw new TokenException("missing end quote", new Token { Position = position });
                }
                if (ch == '\\') {
                    if (context.Position >= len) throw new TokenException("missing end quote", new Token { Position = position });
                    ch = context.File.Content[context.Position++];
                    switch (ch) {
                        case 'n':
                            token.Append('\n');
                            break;
                        case 'r':
                            token.Append('\r');
                            break;
                        case 'a':
                            token.Append('\a');
                            break;
                        case 'b':
                            token.Append('\b');
                            break;
                        case 'f':
                            token.Append('\f');
                            break;
                        case 't':
                            token.Append('\t');
                            break;
                        case 'v':
                            token.Append('\v');
                            break;
                        case '\\':
                            token.Append('\\');
                            break;
                        case '\'':
                            token.Append('\'');
                            break;
                        case '\"':
                            token.Append('\"');
                            break;
                        case '0':
                            token.Append('\0');
                            break;
                        default:
                            throw new TokenException("invalid escape character " + ch, new Token { Position = position });
                    }
                } else if (ch == quoteCh) {
                    if (context.Position >= len)
                        return new Token {
                            Position = position,
                            Type = TokenType.String,
                            StringValue = token.ToString()
                        };
                    var next = context.File.Content[context.Position];
                    if (next == quoteCh) {
                        token.Append(ch);
                        context.Position++;
                    } else {
                        return new Token {
                            Position = position,
                            Type = TokenType.String,
                            StringValue = token.ToString()
                        };
                    }
                } else {
                    token.Append(ch);
                }
            }
            throw new TokenException("missing end quote", new Token { Position = position });
        }

        private static Token ParseLiteral(TokenizerContext context) {
            var token = new StringBuilder();
            var len = context.File.Content.Length;
            var position = context.CreatePosition();
            while (context.Position < len) {
                var ch = context.File.Content[context.Position++];
                if (IsPunctuation(ch)) {
                    context.Position--;
                    break;
                }
                token.Append(ch);
            }
            return new Token {
                Position = position,
                Type = TokenType.Literal,
                StringValue = token.ToString()
            };
        }

        private static Token ParseNumberToken(TokenizerContext context) {
            var literal = ParseLiteral(context);
            if (literal.StringValue.StartsWith("0x")) {
                return ParsePrefixedHexIntegerToken(ref literal);
            }
            if (literal.StringValue.EndsWith("h")) {
                return ParsePostfixedHexInteger(ref literal);
            }
            return ParseDecimalIntegerToken(ref literal);
        }

        private static Token ParseDecimalIntegerToken(ref Token token) {
            long val = 0;
            var pos = 0;
            var len = token.StringValue.Length;
            while (pos < len) {
                var ch = token.StringValue[pos++];
                if (char.IsDigit(ch)) {
                    val = val * 10 + (ch - '0');
                } else {
                    throw new TokenException("unexpected decimal symbol '" + ch + "'", token);
                }
            }
            return new Token {
                Type = TokenType.Number,
                NumberValue = val,
                StringValue = token.StringValue,
                Position = token.Position
            };
        }

        private static Token ParsePrefixedHexIntegerToken(ref Token token) {
            long val = 0;
            var pos = 2;
            var len = token.StringValue.Length;
            if (pos >= len) throw new TokenException("Unexpeced end of hex constant", token);
            while (pos < len) {
                var ch = token.StringValue[pos++];
                var hex = GetHexValue(ch);
                if (hex < 0) throw new TokenException("unexpected hex symbol '" + ch + "'", token);
                val = val * 16 + hex;
            }
            return new Token {
                Type = TokenType.Number,
                NumberValue = val,
                StringValue = token.StringValue,
                Position = token.Position
            };
        }

        private static Token ParsePostfixedHexInteger(ref Token token) {
            long val = 0;
            var pos = 0;
            var len = token.StringValue.Length;
            if (pos >= len) throw new TokenException("Unexpeced end of hex constant", token);
            while (pos < len - 1) {
                var ch = token.StringValue[pos++];
                var hex = GetHexValue(ch);
                if (hex < 0) throw new TokenException("unexpected hex symbol '" + ch + "'", token);
                val = val * 16 + hex;
            }
            return new Token {
                Type = TokenType.Number,
                NumberValue = val,
                StringValue = token.StringValue,
                Position = token.Position
            };
        }

        private static int GetHexValue(char ch) {
            if (ch >= '0' && ch <= '9') return ch - '0';
            if (ch >= 'A' && ch <= 'F') return ch - 'A' + 10;
            if (ch >= 'a' && ch <= 'f') return ch - 'a' + 10;
            return -1;
        }

        private static bool IsPunctuation(char ch) {
            switch (ch) {
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                case '+':
                case '-':
                case '/':
                case '*':
                case '%':
                case '=':
                case '<':
                case '>':
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case ':':
                case ',':
                case '|':
                case '&':
                case '^':
                case '~':
                case '#':
                case '`':
                case ';':
                case '\\':
                    return true;
                default:
                    return false;
            }
        }

        private static TokenType GetPunctuationTokenType(char ch) {
            switch (ch) {
                case ',': return TokenType.Comma;
                case ':': return TokenType.Colon;
                case '(': return TokenType.OpenParenthesis;
                case ')': return TokenType.CloseParenthesis;
                case '+': return TokenType.Plus;
                case '-': return TokenType.Minus;
                case '*': return TokenType.Multiply;
                case '/': return TokenType.Divide;
                case '%': return TokenType.Mod;
                case '<': return TokenType.Less;
                case '>': return TokenType.Greater;
                //case '|': return TokenType.BitOr;
                //case '&': return TokenType.BitAnd;
                //case '^': return TokenType.BitXor;
                case ' ':
                case '\t':
                case '\r':
                case '\n':
                    //case '=':
                    //case '[':
                    //case ']':
                    //case '{':
                    //case '}':
                    //case '~':
                    //case '#':
                    //case '`':
                    //case ';':
                    //case '\\':
                    return TokenType.None;
                default:
                    return TokenType.None;
            }
        }

    }
}
