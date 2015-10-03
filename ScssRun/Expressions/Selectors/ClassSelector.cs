namespace ScssRun.Expressions.Selectors {
    public class ClassSelector : SimpleSelector {
        public string ClassName { get; set; }

        public ClassSelector(string className) {
            ClassName = className;
        }

        public override string Evaluate(ScssEnvironment env) {
            return "." + ClassName;
        }
    }
}
