using System.Collections.Generic;

namespace ScssRun.Nodes {
    public class NodeList : BaseNode {

        public IList<BaseNode> Nodes { get; } = new List<BaseNode>();

        public override void Compile(ScssEnvironment env) {
            foreach (var node in Nodes) {
                node.Compile( env);
            }
        }
    }

    public class NodeList<T> : BaseNode where T : BaseNode{

        public IList<T> Nodes { get; } = new List<T>();

        public override void Compile(ScssEnvironment env) {
            foreach (var node in Nodes) {
                node.Compile(env);
            }
        }
    }
}
