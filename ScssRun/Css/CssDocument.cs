using System.Runtime.InteropServices;
using System.Text;

namespace ScssRun.Css {
    public class CssDocument {

        public CssRulesList Rules { get; } = new CssRulesList();

        public override string ToString() {
            return WriteTo(new StringBuilder(), CssWriterOptions.Normal).ToString();
        }

        public StringBuilder WriteTo(StringBuilder sb, CssWriterOptions options) {
            foreach (var rule in Rules) {
                rule.WriteTo(sb, options);
            }
            return sb;
        }
    }
}
