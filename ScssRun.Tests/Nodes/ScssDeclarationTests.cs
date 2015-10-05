using NUnit.Framework;
using ScssRun.Css;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Value;
using ScssRun.Nodes;

namespace ScssRun.Tests.Nodes {
    [TestFixture]
    public class ScssDeclarationTests {

        [Test]
        public void CompileNestedPropertyTest() {
            var node = new RuleSetNode {
                Selector = SelectorExpression.Parse("div"),
                Rules = {
                    Nodes = {
                        new ScssDeclarationNode {
                            Property = "border",
                            Value = new NestedValueNode {
                                Rules = {
                                    new ScssDeclarationNode {
                                        Property = "right",
                                        Value = new NestedValueNode {
                                            Rules = {
                                                new ScssDeclarationNode {
                                                    Property = "width",
                                                    Value = new ValuesNode {
                                                        Value = Expression.Parse("2px")
                                                    }
                                                }
                                            }
                                        }
                                    }, new ScssDeclarationNode {
                                        Property = "left",
                                        Value = new NestedValueNode {
                                            Rules = {
                                                new ScssDeclarationNode {
                                                    Property = "width",
                                                    Value = new ValuesNode {
                                                        Value = Expression.Parse("1px")
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var env = new ScssEnvironment();
            node.Compile(env);
            AssertExt.AreEqual(new CssDocument {
                Rules = {
                    new CssQualifiedRule {
                        Selector = "div",
                        Declarations = {
                            new CssDeclaration ("border-right-width",  "2px"),
                            new CssDeclaration ("border-left-width",  "1px"),
                        }
                    }
                }
            }, env.Document);
        }
    }
}
