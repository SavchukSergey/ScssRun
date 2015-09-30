using NUnit.Framework;
using ScssRun.Tokens;

namespace ScssRun.Tests.Tokens {
    [TestFixture]
    public class TokenizerTests {

        [Test]
        public void SingleCommentTest() {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read("//comment one\r\n//comment two");
            Assert.AreEqual(2, tokens.Count);
            var queue = new TokensQueue(tokens);
            var token = queue.Read();
            Assert.AreEqual(TokenType.SingleLineComment, token.Type);
            Assert.AreEqual("//comment one\r\n", token.StringValue);

            token = queue.Read();
            Assert.AreEqual(TokenType.SingleLineComment, token.Type);
            Assert.AreEqual("//comment two", token.StringValue);
        }

        [Test]
        public void MultilineCommentTest() {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read("/* comment\r\none*/\r\n/* comment\r\ntwo*/");
            Assert.AreEqual(3, tokens.Count);
            var queue = new TokensQueue(tokens);
            var token = queue.Read();
            Assert.AreEqual(TokenType.MultiLineComment, token.Type);
            Assert.AreEqual("/* comment\r\none*/", token.StringValue);

            queue.Read(TokenType.Whitespace);

            token = queue.Read();
            Assert.AreEqual(TokenType.MultiLineComment, token.Type);
            Assert.AreEqual("/* comment\r\ntwo*/", token.StringValue);
        }

        [Test]
        public void NumberTest() {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read("123 456 789.52");
            Assert.AreEqual(5, tokens.Count);
            var queue = new TokensQueue(tokens);
            var token = queue.Read(TokenType.Number);
            Assert.AreEqual(123.0, token.NumberValue);

            queue.Read(TokenType.Whitespace);

            token = queue.Read(TokenType.Number);
            Assert.AreEqual(456.0, token.NumberValue);


            queue.Read(TokenType.Whitespace);

            token = queue.Read(TokenType.Number);
            Assert.AreEqual(789.52, token.NumberValue);
        }

        [Test]
        public void LiteralTest() {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read("abc def qwe;");
            Assert.AreEqual(6, tokens.Count);
            var queue = new TokensQueue(tokens);
            Assert.AreEqual("abc", queue.Read(TokenType.Literal).StringValue);
            Assert.AreEqual(" ", queue.Read(TokenType.Whitespace).StringValue);
            Assert.AreEqual("def", queue.Read(TokenType.Literal).StringValue);
            Assert.AreEqual(" ", queue.Read(TokenType.Whitespace).StringValue);
            Assert.AreEqual("qwe", queue.Read(TokenType.Literal).StringValue);
            Assert.AreEqual(";", queue.Read(TokenType.Semicolon).StringValue);
        }

        [Test]
        public void CssTest() {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read("p { color: red; }");
            Assert.AreEqual(11, tokens.Count);
            var queue = new TokensQueue(tokens);
            Assert.AreEqual("p", queue.Read(TokenType.Literal).StringValue);
            Assert.AreEqual(" ", queue.Read(TokenType.Whitespace).StringValue);
            Assert.AreEqual("{", queue.Read(TokenType.OpenCurlyBracket).StringValue);
            Assert.AreEqual(" ", queue.Read(TokenType.Whitespace).StringValue);
            Assert.AreEqual("color", queue.Read(TokenType.Literal).StringValue);
            Assert.AreEqual(":", queue.Read(TokenType.Colon).StringValue);
            Assert.AreEqual(" ", queue.Read(TokenType.Whitespace).StringValue);
            Assert.AreEqual("red", queue.Read(TokenType.Literal).StringValue);
            Assert.AreEqual(";", queue.Read(TokenType.Semicolon).StringValue);
            Assert.AreEqual(" ", queue.Read(TokenType.Whitespace).StringValue);
            Assert.AreEqual("}", queue.Read(TokenType.CloseCurlyBracket).StringValue);
        }
    }
}
