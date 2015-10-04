using System.Text;

namespace ScssRun.Css {
    public class CssDeclaration {

        public string Name { get; set; }

        public string Value { get; set; }

        public bool Important { get; set; }

        public CssDeclaration() {
        }

        public CssDeclaration(string name, string value) {
            Name = name;
            Value = value;
        }

        public StringBuilder WriteTo(StringBuilder sb, CssWriterOptions options) {
            sb.Append(Name);
            sb.Append(':');
            sb.Append(Value);
            if (Important) {
                sb.Append(" !important");
            }
            sb.Append(';');
            return sb;
        }

        public override string ToString() {
            return WriteTo(new StringBuilder(), CssWriterOptions.Normal).ToString();
        }
    }
}
