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
                var combinatorType = GetCombinatorType(tokens);

                var tokenPriority = GetPriority(combinatorType);
                if (tokenPriority < priority) {
                    return left;
                }
                switch (combinatorType) {
                    case CombinatorType.Stop:
                        return left;
                    case CombinatorType.Child:
                    case CombinatorType.Sibling:
                    case CombinatorType.Group:
                    case CombinatorType.Descendant:
                        tokens.Read();
                        left = ProcessBinaryExpression(combinatorType, left, tokens);
                        break;
                    default:
                        left = ProcessBinaryExpression(CombinatorType.Combine, left, tokens);
                        break;
                }
            }

            return left;
        }

        private static CombinatorType GetCombinatorType(TokensQueue queue) {
            if (queue.Empty) return CombinatorType.Stop;
            var preview = queue.Peek();
            if (preview.Type != TokenType.Whitespace) {
                var res = GetCombinatorType(preview.Type);
                return res != CombinatorType.None ? res : CombinatorType.Combine;
            } else {
                var q = queue.Moment().SkipWhiteAndComments();
                if (q.Empty) return CombinatorType.None;
                var t = q.Peek();
                var res = GetCombinatorType(t.Type);
                return res != CombinatorType.None ? res : CombinatorType.Descendant;
            }
        }

        private static CombinatorType GetCombinatorType(TokenType type) {
            switch (type) {
                case TokenType.Plus: return CombinatorType.Sibling;
                case TokenType.Greater: return CombinatorType.Child;
                case TokenType.Comma: return CombinatorType.Group;
                case TokenType.OpenCurlyBracket: return CombinatorType.Stop;
                default:
                    return CombinatorType.None;
            }
        }

        private static SelectorExpression ProcessBinaryExpression(CombinatorType type, SelectorExpression left, TokensQueue tokens) {
            var tokenPriority = GetPriority(type);
            var other = ParseWithPriority(tokens, tokenPriority + 1);
            switch (type) {
                case CombinatorType.Combine: return new CombineCombinator(left, other);
                case CombinatorType.Child: return new ChildCombinator(left, other);
                case CombinatorType.Sibling: return new SiblingCombinator(left, other);
                case CombinatorType.Descendant: return new DescendantCombinator(left, other);
                case CombinatorType.Group: return new GroupCombinator(left, other);
                default:
                    throw new TokenException("unexpected operator", tokens.LastReadToken);
            }
        }
        private static SelectorExpression ParseOperand(TokensQueue tokens) {
            tokens.SkipWhiteAndComments();
            var token = tokens.Read();
            switch (token.Type) {
                case TokenType.Literal: return ParseTypeSelector(token, tokens);
                case TokenType.Dot: return ParseClassSelector(token, tokens);
                case TokenType.Hash: return ParseIdSelector(token, tokens);
                default:
                    throw new TokenException("unexpected token " + token.StringValue, token);
            }
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
