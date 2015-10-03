using NUnit.Framework;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;

namespace ScssRun.Tests {
    public static  class AssertExt {

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
            AreEqual(expected.Left, actual.Left);
            AreEqual(expected.Right, actual.Right);
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
    }
}
