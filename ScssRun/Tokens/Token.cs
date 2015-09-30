using System.Globalization;

namespace ScssRun.Tokens {
    public struct Token {

        public TokenType Type { get; set; }

        public TokenPosition Position { get; set; }

        public string StringValue { get; set; }

        public double NumberValue { get; set; }

        public override string ToString() {
            switch (Type) {
                case TokenType.String:
                    return StringValue;
                case TokenType.Number:
                    return NumberValue.ToString(CultureInfo.InvariantCulture);
                default:
                    return Type.ToString();
            }
        }
    }
}
