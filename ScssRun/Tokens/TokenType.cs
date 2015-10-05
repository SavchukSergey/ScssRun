namespace ScssRun.Tokens {
    public enum TokenType {
        None,
        Whitespace,
        String,
        Number,
        Literal,
        SingleLineComment,
        MultiLineComment,

        Comma,
        Colon,
        Semicolon,

        OpenParenthesis,
        CloseParenthesis,

        OpenCurlyBracket,
        CloseCurlyBracket,

        Plus,
        Minus,
        Multiply,
        Divide,
        Percentage,
        LeftShift,
        RightShift,

        Less,
        Greater,
        Equal,
        GreaterOrEqual,
        LessOrEqual,
        Hash,
        Dot,
        ExclamationPoint
    }
}
