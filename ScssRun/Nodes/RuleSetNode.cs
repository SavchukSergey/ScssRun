using ScssRun.Expressions.Selectors;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class RuleSetNode : BaseNode {

        public SelectorExpression Selector { get; set; }

        public SelectorExpression RawSelector { get; set; }

        public NodeList<ScssDeclarationNode> Rules { get; } = new NodeList<ScssDeclarationNode>();

        public NodeList<RuleSetNode> RuleSets { get; } = new NodeList<RuleSetNode>();

        public static RuleSetNode Parse(ScssParserContext context)
        {
            var raw = SelectorExpression.Parse(context.Tokens, context.Selector);
            context.PushSelector(raw);
            var res = new RuleSetNode {
                RawSelector = raw,
                Selector = context.Selector
            };
            ParseBlock(context, res);
            context.PopSelector();
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            env.PushRule(Selector);
            foreach (var node in Rules.Nodes) {
                node.Compile(env);
            }
            env.Document.Rules.Add(env.CssRule);

            foreach (var subRule in RuleSets.Nodes) {
                subRule.Compile(env);
            }
            env.PopRule();
        }

        protected static void ParseBlock(ScssParserContext context, RuleSetNode node) {
            context.Tokens.SkipWhiteAndComments();
            context.Tokens.Read(TokenType.OpenCurlyBracket);
            var stop = false;

            while (!context.Tokens.Empty && !stop) {
                context.Tokens.SkipWhiteAndComments();
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.Semicolon:
                    case TokenType.Whitespace:
                        context.Tokens.Read();
                        break;
                    case TokenType.CloseCurlyBracket:
                        stop = true;
                        break;
                    default:
                        if (IsPropertyName(context)) {
                            var rule = ScssDeclarationNode.Parse(context);
                            node.Rules.Nodes.Add(rule);
                        } else {
                            var ruleSet = Parse(context);
                            node.RuleSets.Nodes.Add(ruleSet);
                        }
                        break;
                }
            }
            context.Tokens.Read(TokenType.CloseCurlyBracket);
        }
    }
}
