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
        Mod,
        LeftShift,
        RightShift,

        Less,
        Greater,
        Equal,
        GreaterOrEqual,
        LessOrEqual,
        Hash
    }
}
