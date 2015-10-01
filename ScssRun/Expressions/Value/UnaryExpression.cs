namespace ScssRun.Expressions.Value {
    public abstract class UnaryExpression : Expression {

        protected UnaryExpression(Expression inner) {
            Inner = inner;
        }

        public Expression Inner { get; }
    }
}
