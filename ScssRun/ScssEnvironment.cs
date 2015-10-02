using System.Collections.Generic;
using ScssRun.Css;

namespace ScssRun {
    public class ScssEnvironment {

        private readonly Stack<string> _nestedProperties = new Stack<string>();
        private readonly Stack<CssQualifiedRule> _rules = new Stack<CssQualifiedRule>();

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

        public void PushRule(CssQualifiedRule rule) {
            _rules.Push(rule);
        }

        public void PopRule() {
            _rules.Pop();
        }

        public CssQualifiedRule Rule => _rules.Count > 0 ? _rules.Peek() : null;

        public CssDocument Document { get; } = new CssDocument();
    }
}
