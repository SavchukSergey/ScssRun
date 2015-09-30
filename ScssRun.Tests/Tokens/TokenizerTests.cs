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
            Assert.AreEqual("comment one\r\n", token.StringValue);

            token = queue.Read();
            Assert.AreEqual(TokenType.SingleLineComment, token.Type);
            Assert.AreEqual("comment two", token.StringValue);
        }

        [Test]
        public void NumberTest() {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.Read("123 456");
            Assert.AreEqual(3, tokens.Count);
            var queue = new TokensQueue(tokens);
            var token = queue.Read();
            Assert.AreEqual(TokenType.Number, token.Type);
            Assert.AreEqual(123.0, token.NumberValue);

            token = queue.Read();
            Assert.AreEqual(TokenType.Whitespace, token.Type);

            token = queue.Read();
            Assert.AreEqual(TokenType.Number, token.Type);
            Assert.AreEqual(456.0, token.NumberValue);
        }
    }
}
