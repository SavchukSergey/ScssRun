using System.Collections.Generic;
using ScssRun.Css;

namespace ScssRun.Nodes {
    public class ScssDocumentNode : BaseNode {

        public IList<BaseNode> Nodes { get; } = new List<BaseNode>();


        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            foreach (var node in Nodes) {
                node.ToCss(writer, env);
            }
        }
    }
}
