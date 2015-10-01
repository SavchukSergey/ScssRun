using System.Text;
using ScssRun.Css;
using ScssRun.Tokens;

namespace ScssRun.Nodes {
    public class CommentNode : BaseNode {

        public CommentNode(Token token) {
            Token = token;
        }

        public Token Token { get; }

        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            //TODO:
            throw new System.NotImplementedException();
        }
    }
}
