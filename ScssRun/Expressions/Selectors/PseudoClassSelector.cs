namespace ScssRun.Expressions.Selectors {
    public class PseudoClassSelector : SimpleSelector {

        public string Name { get; }

        public PseudoClassSelector(string name) {
            Name = name;
        }

        public override string Evaluate(ScssEnvironment env) {
            return ":" + Name;
        }
    }
}
