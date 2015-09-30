using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class RuleSetNode : BaseNode {

        public IList<SelectorNode> Selectors { get; } = new List<SelectorNode>();

        public static RuleSetNode Parse(SscsParserContext context) {
            var res = new RuleSetNode();
            while (true) {
                context.Tokens.SkipWhitespaceAndComments();
                var selector = SelectorNode.Parse(context);
                res.Selectors.Add(selector);
                context.Tokens.SkipWhitespaceAndComments();
                var comma = !context.Tokens.Empty && context.Tokens.Peek().Type == TokenType.Comma;
                if (!comma) break;
            }
            return res;
        }

    }
}
