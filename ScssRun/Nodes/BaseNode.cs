using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public abstract class BaseNode {

        public IList<CommentNode> Comments { get; } = new List<CommentNode>();


        protected static bool IsPropertyName(ScssParserContext context) {
            var tokens = context.Tokens.Moment();
            if (tokens.Empty) return false;
            var prev = tokens.Read();
            var prevNoWhite = TokenType.Whitespace;
            while (!tokens.Empty) {
                var token = tokens.Read();
                switch (token.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                    case TokenType.Whitespace:
                        break;
                    case TokenType.Colon:
                        if (prev.Type == TokenType.Colon) return false;
                        break;
                    case TokenType.OpenCurlyBracket:
                        if (prevNoWhite == TokenType.Colon) return true;
                        return false;
                    case TokenType.Semicolon:
                        return true;
                }
                if (token.Type != TokenType.Whitespace && token.Type != TokenType.SingleLineComment &&
                    token.Type != TokenType.MultiLineComment) {
                    prevNoWhite = token.Type;
                }
                prev = token;
            }
            return false;
        }

        protected BaseNode ParseSelf(ScssParserContext context) {
            var stop = false;
            while (!context.Tokens.Empty && !stop) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        context.Tokens.Read();
                        Comments.Add(new CommentNode(preview));
                        break;
                    default:
                        stop = !AcceptToken(ref preview);
                        break;
                }
            }
            return this;
        }

        protected virtual bool AcceptToken(ref Token token) {
            return false;
        }

        public abstract void Compile(ScssEnvironment env);

    }
}
