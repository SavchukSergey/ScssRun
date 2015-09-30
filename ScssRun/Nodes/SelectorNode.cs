using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class SelectorNode : BaseNode {

        public IList<ElementNode> Elements { get; } = new List<ElementNode>();

        public static SelectorNode Parse(SscsParserContext context) {
            var res = new SelectorNode();
            while (!context.Tokens.Empty) {
                var preview = context.Tokens.Peek();
                if (preview.Type != TokenType.OpenCurlyBracket) {
                    var el = ElementNode.Parse(context);
                    res.Elements.Add(el);
                }
            }
            return res;
        }

    }
}
