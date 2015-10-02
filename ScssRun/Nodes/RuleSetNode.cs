using System.Collections.Generic;
using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class RuleSetNode : BaseNode {

        public IList<SelectorNode> Selectors { get; } = new List<SelectorNode>();

        public NodeList<ScssDeclarationNode> Rules { get; } = new NodeList<ScssDeclarationNode>();

        public new static RuleSetNode Parse(ScssParserContext context) {
            var res = new RuleSetNode();
            var stop = false;
            while (!context.Tokens.Empty && !stop) {
                var preview = context.Tokens.Peek();
                switch (preview.Type) {
                    case TokenType.SingleLineComment:
                    case TokenType.MultiLineComment:
                        context.Tokens.Read();
                        res.Comments.Add(new CommentNode(preview));
                        break;
                    case TokenType.Whitespace:
                        context.Tokens.Read();
                        break;
                    case TokenType.Comma:
                        if (res.Selectors.Count == 0) throw new TokenException("selector expected", preview);
                        var nextSelector = SelectorNode.Parse(context);
                        res.Selectors.Add(nextSelector);
                        break;
                    case TokenType.OpenCurlyBracket:
                        stop = true;
                        break;
                    default:
                        if (res.Selectors.Count != 0) {
                            throw new TokenException("unexpected token", preview);
                        }
                        var firstSelector = SelectorNode.Parse(context);
                        res.Selectors.Add(firstSelector);
                        break;
                }
            }
            context.PushRuleSet(res);
            ParseBlock(context);
            context.PopRuleSet();
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            var selectors = new StringBuilder();
            foreach (var sel in Selectors) {
                if (selectors.Length != 0) selectors.Append(',');
                selectors.Append(sel.FullSelector);
            }
            var rule = new CssQualifiedRule {Selector = selectors.ToString()};
            env.PushRule(rule);

            foreach (var node in Rules.Nodes) {
                node.Compile(env);
            }

            env.PopRule();

            env.Document.Rules.Add(rule);
        }
    }
}
