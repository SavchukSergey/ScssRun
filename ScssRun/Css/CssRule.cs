using System.Text;

namespace ScssRun.Css {
    public abstract class CssRule {

        public abstract StringBuilder WriteTo(StringBuilder sb, CssWriterOptions options);

        public override string ToString() {
            return WriteTo(new StringBuilder(), CssWriterOptions.Normal).ToString();
        }

    }
}
