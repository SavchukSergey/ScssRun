using System.Collections.Generic;
using System.Text;

namespace ScssRun.Tokens {
    public class Tokenizer {

        public IList<Token> Read(string content) {
            return Read(new FileSource { Content = content, FileName = null });
        }


        public IList<Token> Read(FileSource source) {
            var context = new TokenizerContext(source);
            var res = new List<Token>();
            var len = source.Content.Length;
            while (context.Position < len) {
                var token = ParseToken(context);
                res.Add(token);
            }
            return res;
        }

        private static Token ParseToken(TokenizerContext context) {
            var position = context.CreatePosition();
            var len = context.File.Content.Length;
            var ch = context.File.Content[context.Position];
            var preview = (char)(0xffff);
            if (context.Position + 1 < len) {
                preview = context.File.Content[context.Position + 1];
            }
            if (ch == '>' && preview == '>') {
                context.Position++;
                return new Token {
                    Type = TokenType.RightShift,
                    StringValue = ">",
                    Position = position
                };
            }
            if (ch == '<' && preview == '<') {
                context.Position++;
                return new Token {
                    Type = TokenType.LeftShift,
                    StringValue = "<",
                    Position = position
                };
            }
            if (ch == '/' && preview == '/') {
                return ParseSingleLineComment(context);
            }
            if (ch == '/' && preview == '*') {
                return ParseMultiLineComment(context);
            }
            switch (ch) {
                case '\r':
                case '\n':
                case '\t':
                case ' ':
                    return ParseWhitespace(context);
                case '"':
                case '\'':
                    return ParseString(context);
                case '-':
                    var literal = ParseLiteral(context);
                    if (literal.StringValue != "-") return literal;
                    return new Token {
                        Position = literal.Position,
                        Type = TokenType.Minus,
                        StringValue = literal.StringValue
                    };
                default:
                    if (IsPunctuation(ch)) {
                        context.Position++;
                        return new Token {
                            Type = GetPunctuationTokenType(ch),
                            StringValue = ch.ToString(),
                            Position = position
                        };
                    }
                    if (char.IsDigit(ch)) {
                        return ParseNumberToken(context);
                    }
                    return ParseLiteral(context);
            }
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

        private static Token ParseMultiLineComment(TokenizerContext context) {
            var originalPos = context.Position;
            var len = context.File.Content.Length;
            var position = context.CreatePosition();
            while (context.Position < len - 1) {
                var ch = context.File.Content[context.Position++];
                if (ch == '\r' || ch == '\n') {
                    if (context.Position < len) {
                        var nextCh = context.File.Content[context.Position];
                        if ((nextCh == '\r' || nextCh == '\n') && nextCh != ch) {
                            context.Position++;
                        }
                    }
                    context.IncrementLine();
                } else if (ch == '*' && context.File.Content[context.Position] == '/') {
                    context.Position++;
                    return new Token {
                        Type = TokenType.MultiLineComment,
                        StringValue = context.File.Content.Substring(originalPos, context.Position - originalPos),
                        Position = position
                    };
                }
            }
            throw new TokenException("unclosed multiline comment", new Token {
                Type = TokenType.MultiLineComment,
                StringValue = context.File.Content.Substring(originalPos, context.Position - originalPos),
                Position = position
            });
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
                    context.Position--;
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
            var originalPosition = context.Position;
            var len = context.File.Content.Length;
            var position = context.CreatePosition();
            if (context.Position < len && context.File.Content[context.Position] == '-') {
                context.Position++;
            }
            while (context.Position < len) {
                var ch = context.File.Content[context.Position];
                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_') {
                    context.Position++;
                } else {
                    break;
                }
            }

            while (context.Position < len) {
                var ch = context.File.Content[context.Position];
                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9') || ch == '_' || ch == '-') {
                    context.Position++;
                } else {
                    break;
                }
            }
            return new Token {
                Position = position,
                Type = TokenType.Literal,
                StringValue = context.File.Content.Substring(originalPosition, context.Position - originalPosition)
            };
        }

        private static Token ParseNumberToken(TokenizerContext context) {
            var originalPosition = context.Position;
            var len = context.File.Content.Length;
            var position = context.CreatePosition();
            var val = 0.0;
            var dotExp = 0;
            while (context.Position < len) {
                var ch = context.File.Content[context.Position++];
                if (ch >= '0' && ch <= '9') {
                    val = val * 10 + (ch - '0');
                    if (dotExp > 0) dotExp *= 10;
                } else if (dotExp == 0 && ch == '.') {
                    dotExp = 1;
                } else {
                    context.Position--;
                    break;
                }
            }
            if (dotExp > 0) val /= dotExp;
            return new Token {
                Type = TokenType.Number,
                Position = position,
                StringValue = context.File.Content.Substring(originalPosition, context.Position - originalPosition),
                NumberValue = val
            };
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
                case '.':
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
                case '.': return TokenType.Dot;
                case ',': return TokenType.Comma;
                case ':': return TokenType.Colon;
                case ';': return TokenType.Semicolon;
                case '(': return TokenType.OpenParenthesis;
                case ')': return TokenType.CloseParenthesis;
                case '+': return TokenType.Plus;
                case '-': return TokenType.Minus;
                case '*': return TokenType.Multiply;
                case '/': return TokenType.Divide;
                case '%': return TokenType.Percentage;
                case '<': return TokenType.Less;
                case '>': return TokenType.Greater;
                case '{': return TokenType.OpenCurlyBracket;
                case '}': return TokenType.CloseCurlyBracket;
                //case '|': return TokenType.BitOr;
                //case '&': return TokenType.BitAnd;
                //case '^': return TokenType.BitXor;
                case '#': return TokenType.Hash;
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
