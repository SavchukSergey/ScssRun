using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class CommentNode : BaseNode {

        public CommentNode(Token token) {
            Token = token;
        }

        public Token Token { get; }

    }
}
