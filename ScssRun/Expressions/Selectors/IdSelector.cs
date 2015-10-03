namespace ScssRun.Expressions.Selectors {
    public class IdSelector : SimpleSelector {

        public string Id { get; set; }

        public IdSelector(string id) {
            Id = id;
        }

        public override string Evaluate(ScssEnvironment env) {
            return "#" + Id;
        }
    }
}
