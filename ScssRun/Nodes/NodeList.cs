using System.Collections.Generic;
using ScssRun.Css;

namespace ScssRun.Nodes {
    public class NodeList : BaseNode {

        public IList<BaseNode> Nodes { get; } = new List<BaseNode>();

        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            foreach (var node in Nodes) {
                node.ToCss(writer, env);
            }
        }
    }

    public class NodeList<T> : BaseNode where T : BaseNode{

        public IList<T> Nodes { get; } = new List<T>();

        public override void ToCss(CssWriter writer, ScssEnvironment env) {
            foreach (var node in Nodes) {
                node.ToCss(writer, env);
            }
        }
    }
}
