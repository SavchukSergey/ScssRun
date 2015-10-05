using System.Collections.Generic;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class NestedValueNode : BaseValueNode {

        public IList<ScssDeclarationNode> Rules { get; } = new List<ScssDeclarationNode>();

        public static NestedValueNode Parse(ScssParserContext context) {
            var res = new NestedValueNode();
            context.Tokens.SkipWhiteAndComments();
            context.Tokens.Read(TokenType.OpenCurlyBracket);
            while (!context.Tokens.Empty) {
                context.Tokens.SkipWhiteAndComments();
                var preview = context.Tokens.Peek();
                if (preview.Type == TokenType.CloseCurlyBracket) break;
                if (preview.Type == TokenType.Semicolon) {
                    context.Tokens.Read();
                    continue;
                }
                var rule = ScssDeclarationNode.Parse(context);
                res.Rules.Add(rule);
            }
            context.Tokens.Read(TokenType.CloseCurlyBracket);
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            foreach (var rule in Rules) {
                rule.Compile(env);
            }
        }
    }
}
