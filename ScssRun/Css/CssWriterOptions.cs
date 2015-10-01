namespace ScssRun.Css {
    public class CssWriterOptions {

        private static CssWriterOptions _minified;

        public static CssWriterOptions Minified {
            get { return _minified = _minified ?? new CssWriterOptions(); }
        }
    }
}
