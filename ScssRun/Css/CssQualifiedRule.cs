using System.Text;

namespace ScssRun.Css {
    public class CssQualifiedRule : CssRule {

        public string Selector { get; set; }

        public CssDeclarationList Declarations { get; } = new CssDeclarationList();

        public override StringBuilder WriteTo(StringBuilder sb, CssWriterOptions options) {
            sb.Append(Selector);
            sb.Append('{');
            foreach (var declaration in Declarations) {
                declaration.WriteTo(sb, options);
            }
            sb.Append('}');
            return sb;
        }
    }
}
