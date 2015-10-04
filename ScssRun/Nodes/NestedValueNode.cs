using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class NestedValueNode : BaseValueNode {

        public IList<ScssDeclarationNode> Rules { get; } = new List<ScssDeclarationNode>();

        public static NestedValueNode Parse(ScssParserContext context) {
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
                        var rule = ScssDeclarationNode.Parse(context);
                        res.Rules.Add(rule);
                        break;
                }
            }
            throw new TokenException("unexpected end of file", context.Tokens.LastReadToken);
        }

        public override void Compile(ScssEnvironment env) {
            foreach (var rule in Rules) {
                rule.Compile(env);
            }
        }
    }
}
