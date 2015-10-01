using System;
using System.Collections.Generic;
using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class NestedValueNode : BaseValueNode {


        public IList<RuleNode> Rules { get; } = new List<RuleNode>();

        public new static NestedValueNode Parse(ScssParserContext context) {
            var res = new NestedValueNode();
            while (!context.Tokens.Empty) {
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
                    case TokenType.CloseCurlyBracket:
                        return res;
                    default:
                        var rule = RuleNode.Parse(context);
                        res.Rules.Add(rule);
                        break;
                }
            }
            throw new TokenException("unexpected end of file", context.Tokens.LastReadToken);
        }

        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            throw new NotImplementedException();
        }
    }
}
