using NUnit.Framework;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Value;
using ScssRun.Nodes;
using ScssRun.Tokens;

namespace ScssRun.Tests.Nodes {
    [TestFixture]
    public class RuleSetNodeTest {


        [Test]
        public void ParseTest() {
            const string css = "div {color: red;}";
            var tokens = new Tokenizer().Read(css);
            var context = new ScssParserContext(new TokensQueue(tokens));
            var node = RuleSetNode.Parse(context);
            AssertExt.AreEqual(new RuleSetNode {
                Selector = new TypeSelector("div"),
                Rules = {
                    Nodes = {
                        new ScssDeclarationNode {
                            Property = "color",
                            Value = new ValuesNode {

                            }
                        }
                    }
                }
            }, node);
        }

        [Test]
        public void ParseSubRuleTest() {
            const string css = "div { p {color: red;} }";
            var tokens = new Tokenizer().Read(css);
            var context = new ScssParserContext(new TokensQueue(tokens));
            var node = RuleSetNode.Parse(context);
            AssertExt.AreEqual(new RuleSetNode {
                Selector = new TypeSelector("div"),
                RuleSets = {
                    Nodes = {
                        new RuleSetNode {
                            Selector = new TypeSelector("p"),
                            Rules = {
                                Nodes = {
                                    new ScssDeclarationNode {
                                        Property = "color",
                                        Value = new ValuesNode { Values = { new ValueNode { Value = Expression.Parse("red")} }}
                                    }
                                }
                            }
                        }
                    }
                }
            }, node);
        }
    }
}
