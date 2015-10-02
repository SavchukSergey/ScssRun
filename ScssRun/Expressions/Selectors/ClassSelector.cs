namespace ScssRun.Expressions.Selectors {
    public class ClassSelector : SimpleSelector {
        public string ClassName { get; set; }

        public override string Evaluate(ScssEnvironment env) {
            return "." + ClassName;
        }
    }
}
