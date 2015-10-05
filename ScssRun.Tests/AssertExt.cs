using System.Text;
using NUnit.Framework;
using ScssRun.Css;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;
using ScssRun.Expressions.Value;
using ScssRun.Nodes;

namespace ScssRun.Tests {
    public static class AssertExt {

        public static void AreEqual(CssDocument expected, CssDocument actual) {
            var exp = expected.WriteTo(new StringBuilder(), CssWriterOptions.Minified).ToString();
            var act = actual.WriteTo(new StringBuilder(), CssWriterOptions.Minified).ToString();
            Assert.AreEqual(exp, act);
        }

        public static void AreEqual(RuleSetNode expected, RuleSetNode actual) {
            Assert.AreEqual(expected.RuleSets.Nodes.Count, actual.RuleSets.Nodes.Count);
            for (var i = 0; i < expected.RuleSets.Nodes.Count; i++) {
                var expectedRuleSet = expected.RuleSets.Nodes[i];
                AreEqual(expectedRuleSet, actual.RuleSets.Nodes[i]);
            }
            for (var i = 0; i < expected.Rules.Nodes.Count; i++) {
                var expectedRule = expected.Rules.Nodes[i];
                AreEqual(expectedRule, actual.Rules.Nodes[i]);
            }
            AreEqual(expected.Selector, actual.Selector);
        }

        public static void AreEqual(ScssDeclarationNode expected, ScssDeclarationNode actual) {
            Assert.AreEqual(expected.Property, actual.Property);
            AreEqual(expected.Value, actual.Value);
        }

        public static void AreEqual(BaseValueNode expected, BaseValueNode actual) {
            Assert.AreEqual(expected.GetType(), actual.GetType());
            if (expected is ValuesNode) {
                AreEqual((ValuesNode)expected, (ValuesNode)actual);
            } else if (expected is NestedValueNode) {
                AreEqual((NestedValueNode)expected, (NestedValueNode)actual);
            } else {
                throw new AssertionException("unknown value type");
            }
            //todo: compare
        }

        public static void AreEqual(ValuesNode expected, ValuesNode actual) {
            AreEqual(expected.Value, actual.Value);
        }

        public static void AreEqual(NestedValueNode expected, NestedValueNode actual) {
            Assert.AreEqual(expected.Rules.Count, actual.Rules.Count);
            for (var i = 0; i < expected.Rules.Count; i++) {
                var expectedValue = expected.Rules[i];
                AreEqual(expectedValue, actual.Rules[i]);
            }
        }

        #region Value expressions

        public static void AreEqual(Expression expected, Expression actual) {
            Assert.AreEqual(expected.GetType(), actual.GetType());
            if (expected is SpaceGroupExpression) {
                AreEqual((SpaceGroupExpression)expected, (SpaceGroupExpression)actual);
            } else if (expected is CommaGroupExpression) {
                AreEqual((CommaGroupExpression)expected, (CommaGroupExpression)actual);
            } else if (expected is UnitExpression) {
                AreEqual((UnitExpression)expected, (UnitExpression)actual);
            } else if (expected is NumberExpression) {
                AreEqual((NumberExpression)expected, (NumberExpression)actual);
            } else if (expected is LiteralExpression) {
                AreEqual((LiteralExpression)expected, (LiteralExpression)actual);
            } else if (expected is AddExpression) {
                AreEqual((AddExpression)expected, (AddExpression)actual);
            } else {
                throw new AssertionException("unknown value type " + expected.GetType().Name);
            }
        }

        public static void AreEqual(SpaceGroupExpression expected, SpaceGroupExpression actual) {
            Assert.AreEqual(expected.Expressions.Length, actual.Expressions.Length);
            for (var i = 0; i < expected.Expressions.Length; i++) {
                AreEqual(expected.Expressions[i], actual.Expressions[i]);
            }
        }

        public static void AreEqual(CommaGroupExpression expected, CommaGroupExpression actual) {
            Assert.AreEqual(expected.Expressions.Length, actual.Expressions.Length);
            for (var i = 0; i < expected.Expressions.Length; i++) {
                AreEqual(expected.Expressions[i], actual.Expressions[i]);
            }
        }

        public static void AreEqual(UnitExpression expected, UnitExpression actual) {
            Assert.AreEqual(expected.Type, actual.Type);
            AreEqual(expected.Inner, actual.Inner);
        }

        public static void AreEqual(NumberExpression expected, NumberExpression actual) {
            Assert.AreEqual(expected.Value, actual.Value);
        }

        public static void AreEqual(LiteralExpression expected, LiteralExpression actual) {
            Assert.AreEqual(expected.Value, actual.Value);
        }

        public static void AreEqual(AddExpression expected, AddExpression actual) {
            AreEqual(expected.Left, actual.Left);
            AreEqual(expected.Right, actual.Right);
        }

        #endregion

        #region Selector expressions

        public static void AreEqual(SelectorExpression expected, SelectorExpression actual) {
            Assert.AreEqual(expected.GetType(), actual.GetType());
            if (expected is GroupCombinator) {
                AreEqual((GroupCombinator)expected, (GroupCombinator)actual);
            } else if (expected is DescendantCombinator) {
                AreEqual((DescendantCombinator)expected, (DescendantCombinator)actual);
            } else if (expected is CombineCombinator) {
                AreEqual((CombineCombinator)expected, (CombineCombinator)actual);
            } else if (expected is TypeSelector) {
                AreEqual((TypeSelector)expected, (TypeSelector)actual);
            } else if (expected is ClassSelector) {
                AreEqual((ClassSelector)expected, (ClassSelector)actual);
            } else if (expected is IdSelector) {
                AreEqual((IdSelector)expected, (IdSelector)actual);
            } else {
                throw new AssertionException("unknown expression type " + expected.GetType());
            }
        }

        public static void AreEqual(GroupCombinator expected, GroupCombinator actual) {
            Assert.AreEqual(expected.Expressions.Length, actual.Expressions.Length);
            for (var i = 0; i < expected.Expressions.Length; i++) {
                AreEqual(expected.Expressions[i], actual.Expressions[i]);
            }
        }

        public static void AreEqual(DescendantCombinator expected, DescendantCombinator actual) {
            AreEqual(expected.Left, actual.Left);
            AreEqual(expected.Right, actual.Right);
        }

        public static void AreEqual(CombineCombinator expected, CombineCombinator actual) {
            AreEqual(expected.Left, actual.Left);
            AreEqual(expected.Right, actual.Right);
        }

        public static void AreEqual(TypeSelector expected, TypeSelector actual) {
            Assert.AreEqual(expected.TypeName, actual.TypeName);
        }

        public static void AreEqual(ClassSelector expected, ClassSelector actual) {
            Assert.AreEqual(expected.ClassName, actual.ClassName);
        }

        public static void AreEqual(IdSelector expected, IdSelector actual) {
            Assert.AreEqual(expected.Id, actual.Id);
        }

        #endregion

    }
}
