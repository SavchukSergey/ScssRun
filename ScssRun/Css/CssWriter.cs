using System.Text;

namespace ScssRun.Css {
    public class CssWriter {
        private readonly CssWriterOptions _options;
        private readonly StringBuilder _sb = new StringBuilder();

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

    }
}
