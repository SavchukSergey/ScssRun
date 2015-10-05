using System.Text;
using NUnit.Framework;
using ScssRun.Css;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;
using ScssRun.Expressions.Value;
using ScssRun.Nodes;

namespace ScssRun.Tests {
    public static class AssertExt {

        #region Css Document

        public static void AreEqual(CssDocument expected, CssDocument actual) {
            var exp = expected.WriteTo(new StringBuilder(), CssWriterOptions.Minified).ToString();
            var act = actual.WriteTo(new StringBuilder(), CssWriterOptions.Minified).ToString();
            Assert.AreEqual(exp, act);
        }

        #endregion

        public static void AreEqual(RuleSetNode expected, RuleSetNode actual, string message = "") {
            Assert.AreEqual(expected.RuleSets.Nodes.Count, actual.RuleSets.Nodes.Count, message + "/Count");
            for (var i = 0; i < expected.RuleSets.Nodes.Count; i++) {
                var expectedRuleSet = expected.RuleSets.Nodes[i];
                AreEqual(expectedRuleSet, actual.RuleSets.Nodes[i], message + "RuleSets[" + i + "]");
            }
            for (var i = 0; i < expected.Rules.Nodes.Count; i++) {
                var expectedRule = expected.Rules.Nodes[i];
                AreEqual(expectedRule, actual.Rules.Nodes[i], message + "Rules[" + i + "]");
            }
            AreEqual(expected.Selector, actual.Selector);
        }

        public static void AreEqual(ScssDeclarationNode expected, ScssDeclarationNode actual, string message = "") {
            Assert.AreEqual(expected.Property, actual.Property, message + "/Property");
            AreEqual(expected.Value, actual.Value, message + "/Value");
        }

        public static void AreEqual(BaseValueNode expected, BaseValueNode actual, string message = "") {
            Assert.AreEqual(expected.GetType(), actual.GetType(), message + "/Type");
            if (expected is ValuesNode) {
                AreEqual((ValuesNode)expected, (ValuesNode)actual, message);
            } else if (expected is NestedValueNode) {
                AreEqual((NestedValueNode)expected, (NestedValueNode)actual, message);
            } else {
                throw new AssertionException("unknown value type");
            }
            //todo: compare
        }

        public static void AreEqual(ValuesNode expected, ValuesNode actual, string message = "") {
            AreEqual(expected.Value, actual.Value, message + "/Value");
            Assert.AreEqual(expected.Important, actual.Important, message + "/Important");
        }

        public static void AreEqual(NestedValueNode expected, NestedValueNode actual, string message = "") {
            Assert.AreEqual(expected.Rules.Count, actual.Rules.Count, message + "/Count");
            for (var i = 0; i < expected.Rules.Count; i++) {
                var expectedValue = expected.Rules[i];
                AreEqual(expectedValue, actual.Rules[i], message + "Rules[" + i + "]");
            }
        }

        #region Value expressions

        public static void AreEqual(Expression expected, Expression actual, string message = "") {
            Assert.AreEqual(expected.GetType(), actual.GetType(), message + "/Type");
            if (expected is SpaceGroupExpression) {
                AreEqual((SpaceGroupExpression)expected, (SpaceGroupExpression)actual, message);
            } else if (expected is CommaGroupExpression) {
                AreEqual((CommaGroupExpression)expected, (CommaGroupExpression)actual, message);
            } else if (expected is UnitExpression) {
                AreEqual((UnitExpression)expected, (UnitExpression)actual, message);
            } else if (expected is NumberExpression) {
                AreEqual((NumberExpression)expected, (NumberExpression)actual, message);
            } else if (expected is LiteralExpression) {
                AreEqual((LiteralExpression)expected, (LiteralExpression)actual, message);
            } else if (expected is AddExpression) {
                AreEqual((AddExpression)expected, (AddExpression)actual, message);
            } else {
                throw new AssertionException("unknown value type " + expected.GetType().Name);
            }
        }

        public static void AreEqual(SpaceGroupExpression expected, SpaceGroupExpression actual, string message = "") {
            Assert.AreEqual(expected.Expressions.Length, actual.Expressions.Length);
            for (var i = 0; i < expected.Expressions.Length; i++) {
                AreEqual(expected.Expressions[i], actual.Expressions[i], message + "Expressions[" + i + "]");
            }
        }

        public static void AreEqual(CommaGroupExpression expected, CommaGroupExpression actual, string message = "") {
            Assert.AreEqual(expected.Expressions.Length, actual.Expressions.Length);
            for (var i = 0; i < expected.Expressions.Length; i++) {
                AreEqual(expected.Expressions[i], actual.Expressions[i], message + "Expressions[" + i + "]");
            }
        }

        public static void AreEqual(UnitExpression expected, UnitExpression actual, string message = "") {
            Assert.AreEqual(expected.Type, actual.Type, message + "/Type");
            AreEqual(expected.Inner, actual.Inner, message + "/Inner");
        }

        public static void AreEqual(NumberExpression expected, NumberExpression actual, string message = "") {
            Assert.AreEqual(expected.Value, actual.Value, message + "/Value");
        }

        public static void AreEqual(LiteralExpression expected, LiteralExpression actual, string message = "") {
            Assert.AreEqual(expected.Value, actual.Value, message + "/Value");
        }

        public static void AreEqual(AddExpression expected, AddExpression actual, string message = "") {
            AreEqual(expected.Left, actual.Left, message + "/Left");
            AreEqual(expected.Right, actual.Right, message + "/Right");
        }

        #endregion

        #region Selector expressions

        public static void AreEqual(SelectorExpression expected, SelectorExpression actual, string message = "") {
            Assert.AreEqual(expected.GetType(), actual.GetType(), message + "/Type");
            if (expected is GroupCombinator) {
                AreEqual((GroupCombinator)expected, (GroupCombinator)actual, message);
            } else if (expected is DescendantCombinator) {
                AreEqual((DescendantCombinator)expected, (DescendantCombinator)actual, message);
            } else if (expected is CombineCombinator) {
                AreEqual((CombineCombinator)expected, (CombineCombinator)actual, message);
            } else if (expected is TypeSelector) {
                AreEqual((TypeSelector)expected, (TypeSelector)actual, message);
            } else if (expected is ClassSelector) {
                AreEqual((ClassSelector)expected, (ClassSelector)actual, message);
            } else if (expected is IdSelector) {
                AreEqual((IdSelector)expected, (IdSelector)actual, message);
            } else if (expected is PseudoClassSelector) {
                AreEqual((PseudoClassSelector)expected, (PseudoClassSelector)actual, message);
            } else if (expected is NotExpression) {
                AreEqual((NotExpression)expected, (NotExpression)actual, message);
            } else if (expected is AttributeSelector) {
                AreEqual((AttributeSelector)expected, (AttributeSelector)actual, message);
            } else {
                throw new AssertionException(message + "/Type: unknown expression type " + expected.GetType());
            }
        }

        public static void AreEqual(GroupCombinator expected, GroupCombinator actual, string message) {
            Assert.AreEqual(expected.Expressions.Length, actual.Expressions.Length, message + "/Expressions/Count");
            for (var i = 0; i < expected.Expressions.Length; i++) {
                AreEqual(expected.Expressions[i], actual.Expressions[i], message + "/Expressions[" + i + "]");
            }
        }

        public static void AreEqual(DescendantCombinator expected, DescendantCombinator actual, string message = "") {
            AreEqual(expected.Left, actual.Left, message + "/Left");
            AreEqual(expected.Right, actual.Right, message + "/Right");
        }

        public static void AreEqual(CombineCombinator expected, CombineCombinator actual, string message = "") {
            AreEqual(expected.Left, actual.Left, message + "/Left");
            AreEqual(expected.Right, actual.Right, message + "/Right");
        }

        public static void AreEqual(TypeSelector expected, TypeSelector actual, string message = "") {
            Assert.AreEqual(expected.TypeName, actual.TypeName, message + "/TypeName");
        }

        public static void AreEqual(ClassSelector expected, ClassSelector actual, string message = "") {
            Assert.AreEqual(expected.ClassName, actual.ClassName, message + "/ClassName");
        }

        public static void AreEqual(IdSelector expected, IdSelector actual, string message = "") {
            Assert.AreEqual(expected.Id, actual.Id, message + "/Id");
        }

        public static void AreEqual(PseudoClassSelector expected, PseudoClassSelector actual, string message = "") {
            Assert.AreEqual(expected.Name, actual.Name, message + "/Name");
        }

        public static void AreEqual(NotExpression expected, NotExpression actual, string message = "") {
            AreEqual(expected.Inner, actual.Inner, message + "/Inner");
        }

        public static void AreEqual(AttributeSelector expected, AttributeSelector actual, string message = "") {
            Assert.AreEqual(expected.GetType(), actual.GetType(), message + "/Type");
            if (expected is AttributeExistsSelector) {
                AreEqual((AttributeExistsSelector)expected, (AttributeExistsSelector)actual, message);
            } else if (expected is AttributeEqualsSelector) {
                AreEqual((AttributeEqualsSelector)expected, (AttributeEqualsSelector)actual, message);
            } else {
                throw new AssertionException(message + "/Type: unknown expression type " + expected.GetType());
            }
        }

        public static void AreEqual(AttributeExistsSelector expected, AttributeExistsSelector actual, string message = "") {
            Assert.AreEqual(expected.AttributeName, actual.AttributeName, message + "/AttributeName");
        }

        public static void AreEqual(AttributeEqualsSelector expected, AttributeEqualsSelector actual, string message = "") {
            Assert.AreEqual(expected.AttributeName, actual.AttributeName, message + "/AttributeName");
            Assert.AreEqual(expected.Value, actual.Value, message + "/Value");
        }

        #endregion

    }
}
