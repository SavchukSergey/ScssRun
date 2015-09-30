using System.Collections.Generic;

namespace ScssRun.Nodes {
    public class NodeList : BaseNode {

        public IList<BaseNode> Nodes { get; } = new List<BaseNode>();

    }
}
