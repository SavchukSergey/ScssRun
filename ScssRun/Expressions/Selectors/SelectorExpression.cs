using ScssRun.Expressions.Selectors.Combinators;
using ScssRun.Tokens;

namespace ScssRun.Expressions.Selectors {
    //http://www.w3.org/TR/css3-selectors/
    public abstract class SelectorExpression {

        public virtual bool HasExplicitParent => false;

        public static SelectorExpression Parse(string source) {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read(source);
            var queue = new TokensQueue(tokens);
            return Parse(queue);
        }

        public static SelectorExpression Parse(TokensQueue tokens, SelectorExpression parent = null) {
            return ParseWithPriority(tokens, 0, parent);
        }

        private static SelectorExpression ParseWithPriority(TokensQueue tokens, int priority, SelectorExpression parent = null) {
            var left = ParseOperand(tokens, parent);
            while (!tokens.Empty) {
                tokens.SkipComments();
                var preview = tokens.Peek();
                switch (preview.Type) {
                    case TokenType.CloseParenthesis:
                    case TokenType.OpenCurlyBracket:
                        return left;
                }
                var combinatorType = PeekCombinatorType(tokens);

                var tokenPriority = GetPriority(combinatorType);
                if (tokenPriority < priority) {
                    return left;
                }
                combinatorType = ReadCombinatorType(tokens);
                if (combinatorType == CombinatorType.Stop) return left;
                left = ProcessBinaryExpression(combinatorType, left, tokens);
            }

            return left;
        }

        private static CombinatorType PeekCombinatorType(TokensQueue queue) {
            return ReadCombinatorType(queue.Moment());
        }

        private static CombinatorType ReadCombinatorType(TokensQueue queue) {
            var hasWhite = false;
            while (!queue.Empty) {
                queue.SkipComments();
                var preview = queue.Peek();
                switch (preview.Type) {
                    case TokenType.Whitespace:
                        queue.Read();
                        hasWhite = true;
                        break;
                    case TokenType.Plus:
                        queue.Read();
                        return CombinatorType.Sibling;
                    case TokenType.Greater:
                        queue.Read();
                        return CombinatorType.Child;
                    case TokenType.Comma:
                        queue.Read();
                        return CombinatorType.Group;
                    case TokenType.OpenCurlyBracket:
                    case TokenType.CloseParenthesis:
                        return CombinatorType.Stop;
                    default:
                        return hasWhite ? CombinatorType.Descendant : CombinatorType.Combine;
                }
            }
            return CombinatorType.Stop;
        }

        private static SelectorExpression ProcessBinaryExpression(CombinatorType type, SelectorExpression left, TokensQueue tokens) {
            var tokenPriority = GetPriority(type);
            var other = ParseWithPriority(tokens, tokenPriority + 1);
            switch (type) {
                case CombinatorType.Combine:
                    var combineCombinator = left as CombineCombinator;
                    return combineCombinator != null ? combineCombinator.Add(other) : new CombineCombinator(left, other);
                case CombinatorType.Child: return new ChildCombinator(left, other);
                case CombinatorType.Sibling: return new SiblingCombinator(left, other);
                case CombinatorType.Descendant:
                    var descendantCombinator = left as DescendantCombinator;
                    return descendantCombinator != null ? descendantCombinator.Add(other) : new DescendantCombinator(left, other);
                case CombinatorType.Group:
                    var groupCombinator = left as GroupCombinator;
                    return groupCombinator != null ? groupCombinator.Add(other) : new GroupCombinator(left, other);
                default:
                    throw new TokenException("unexpected operator", tokens.LastReadToken);
            }
        }

        private static SelectorExpression ParseOperand(TokensQueue tokens, SelectorExpression parent = null) {
            tokens.SkipWhiteAndComments();
            var token = tokens.Read();
            switch (token.Type) {
                case TokenType.Literal: return ParseTypeSelector(token, tokens);
                case TokenType.Dot: return ParseClassSelector(token, tokens);
                case TokenType.Hash: return ParseIdSelector(token, tokens);
                case TokenType.Colon: return ParsePseudoSelector(token, tokens);
                case TokenType.OpenSquareBracket: return ParseAttributeSelector(token, tokens);
                case TokenType.Ampersand: return new ParentSelector(parent);
                default:
                    throw new TokenException("unexpected token " + token.StringValue, token);
            }
        }

        private static SelectorExpression ParseAttributeSelector(Token token, TokensQueue tokens) {
            var attrName = tokens.Read(TokenType.Literal).StringValue;
            var operation = tokens.Read();
            if (operation.Type == TokenType.CloseSquareBracket) return new AttributeExistsSelector(attrName);
            if (operation.Type == TokenType.Equal) {
                var val = tokens.Read();
                if (val.Type != TokenType.Literal && val.Type != TokenType.String) {
                    throw new TokenException("expected literal or string token", val);
                }
                tokens.Read(TokenType.CloseSquareBracket);
                return new AttributeEqualsSelector(attrName, val.StringValue);
            }
            throw new TokenException("unknown attribute operator", operation);
        }

        protected static SelectorExpression ParseClassSelector(Token token, TokensQueue queue) {
            var next = queue.Read(TokenType.Literal);
            return new ClassSelector(next.StringValue);
        }

        protected static SelectorExpression ParseTypeSelector(Token token, TokensQueue queue) {
            return new TypeSelector(token.StringValue);
        }

        protected static SelectorExpression ParseIdSelector(Token token, TokensQueue queue) {
            var next = queue.Read(TokenType.Literal);
            return new IdSelector(next.StringValue);
        }

        protected static SelectorExpression ParsePseudoSelector(Token token, TokensQueue queue) {
            var next = queue.Read(TokenType.Literal);
            if (next.StringValue == "not") {
                var preview = queue.Peek();
                if (preview.Type == TokenType.OpenParenthesis) {
                    queue.Read(TokenType.OpenParenthesis);
                    var expr = Parse(queue);
                    if (!(expr is SimpleSelector)) throw new TokenException("simple selector expected", preview);
                    queue.Read(TokenType.CloseParenthesis);
                    return new NotExpression((SimpleSelector)expr);
                }
            }
            return new PseudoClassSelector(next.StringValue);
        }

        public abstract string Evaluate(ScssEnvironment env);

        private static int GetPriority(CombinatorType type) {
            switch (type) {
                case CombinatorType.Group: return 0;
                case CombinatorType.Descendant: return 1;
                default:
                    return 2;
            }
        }

    }
}
