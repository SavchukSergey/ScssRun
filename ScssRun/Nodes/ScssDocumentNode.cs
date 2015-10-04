namespace ScssRun.Nodes {
    public class ScssDocumentNode : NodeList {

        public static ScssDocumentNode Parse(ScssParserContext context) {
            var res = new ScssDocumentNode();
            while (!context.Tokens.Empty) {
                var node = RuleSetNode.Parse(context);
                res.Nodes.Add(node);
            }
            return res;
        }

        public override void Compile(ScssEnvironment env) {
            foreach (var node in Nodes) {
                node.Compile(env);
            }
        }

    }
}
