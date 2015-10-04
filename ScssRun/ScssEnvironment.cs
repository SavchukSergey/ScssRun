using System.Collections.Generic;
using ScssRun.Css;
using ScssRun.Expressions.Selectors;
using ScssRun.Expressions.Selectors.Combinators;

namespace ScssRun {
    public class ScssEnvironment {

        private readonly Stack<string> _nestedProperties = new Stack<string>();
        private readonly Stack<CssQualifiedRule> _cssRules = new Stack<CssQualifiedRule>();
        private readonly Stack<SelectorExpression>  _scssRules = new Stack<SelectorExpression>();

        public void PushProperty(string propertyName) {
            if (_nestedProperties.Count > 0) {
                propertyName = _nestedProperties.Peek() + "-" + propertyName;
            }
            _nestedProperties.Push(propertyName);
        }

        public void PopProperty() {
            _nestedProperties.Pop();
        }

        public string FormatProperty() {
            return _nestedProperties.Count > 0 ? _nestedProperties.Peek() : string.Empty;
        }

        public void PopRule() {
            _cssRules.Pop();
            _scssRules.Pop();
        }

        public void PushRule(SelectorExpression expression) {
            if (_scssRules.Count > 0) {
                expression = new NestedCombinator(_scssRules.Peek(), expression);
            }
            _scssRules.Push(expression);
            var rule = new CssQualifiedRule { Selector =  expression.Evaluate(this) };
            _cssRules.Push(rule);
        }

        public CssQualifiedRule CssRule => _cssRules.Count > 0 ? _cssRules.Peek() : null;

        public CssDocument Document { get; } = new CssDocument();
    }
}
