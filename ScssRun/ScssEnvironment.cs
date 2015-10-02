using System.Collections.Generic;

namespace ScssRun {
    public class ScssEnvironment {

        private readonly Stack<string> _nestedProperties = new Stack<string>();

        public void PushedNestedProperty(string propertyName) {
            _nestedProperties.Push(propertyName);
        }

        public void PopNestedProperty() {
            _nestedProperties.Pop();
        }

        protected string PropertyPrefix {
            get {
                if (_nestedProperties.Count == 0) return string.Empty;
                var val = "";
                foreach (var prop in _nestedProperties) {
                    val = prop + "-" + val;
                }
                return val;
            }
        }

        public string FormatProperty(string property) {
            return PropertyPrefix + property;
        }
    }
}
