using ScssRun.Css;
using ScssRun.Expressions.Selectors;

namespace ScssRun.Nodes {
    public class RuleSetNode : BaseNode {

        public SelectorExpression Selector { get; set; }

        public NodeList<ScssDeclarationNode> Rules { get; } = new NodeList<ScssDeclarationNode>();

        public new static RuleSetNode Parse(ScssParserContext context) {
            var res = new RuleSetNode {
                Selector = SelectorExpression.Parse(context.Tokens)
            };
            context.PushRuleSet(res);
            ParseBlock(context);
            context.PopRuleSet();
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            var rule = new CssQualifiedRule { Selector = Selector.Evaluate(env) };
            env.PushRule(rule);

            foreach (var node in Rules.Nodes) {
                node.Compile(env);
            }

            env.PopRule();

            env.Document.Rules.Add(rule);
        }
    }
}
