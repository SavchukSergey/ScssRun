using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class RuleSetNode : BaseNode {

        public IList<SelectorNode> Selectors { get; } = new List<SelectorNode>();

        public NodeList Rules { get; private set; } = new NodeList();

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
            res.Rules = (NodeList) ParseBlock(context);
            return res;
        }

    }
}
