using System;
using System.Text;

namespace ScssRun.Css {
    public class CssWriter {
        private readonly CssWriterOptions _options;
        private readonly StringBuilder _sb = new StringBuilder();
        private int _selectors;
        private bool _ruleSetOpened = false;

        public CssWriter(CssWriterOptions options) {
            _options = options;
        }


        public CssWriter Append(string val) {
            _sb.Append(val);
            return this;
        }

        public CssWriter Append(char val) {
            _sb.Append(val);
            return this;
        }

        public string Result => _sb.ToString();

        public void StartRuleSet() {
            _selectors = 0;
            _ruleSetOpened = false;
        }

        public void AppendSelector(string sel) {
            if (_selectors > 0) {
                _sb.Append(',');
            }
            _sb.Append(sel);
            _selectors++;
        }

        public void AppendRule(string name, string value) {
            if (!_ruleSetOpened) {
                _sb.Append('{');
                _ruleSetOpened = true;
            }
            _sb.Append(name).Append(':').Append(value).Append(';');
        }

        public void EndRuleSet() {
            if (!_ruleSetOpened) {
                throw new Exception("unexpected rule set end");
            }
            _sb.Append('}');
        }
    }
}
