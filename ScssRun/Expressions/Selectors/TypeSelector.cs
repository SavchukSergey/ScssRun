namespace ScssRun.Expressions.Selectors {
    public class TypeSelector : SimpleSelector {

        public string TypeName { get; set; }

        public TypeSelector(string typeName) {
            TypeName = typeName;
        }

        public override string Evaluate(ScssEnvironment env) {
            return TypeName;
        }
    }
}
