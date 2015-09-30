using System.Text;

namespace ScssRun.Tokens {
    public struct TokenPosition {

        public int Line { get; set; }

        public int Offset { get; set; }

        public int LineStart { get; set; }

        public FileSource File { get; set; }

        public string GetLine() {
            var sb = new StringBuilder();
            for (var i = LineStart; i < File.Content.Length; i++) {
                var ch = File.Content[i];
                if (ch == '\r' || ch == '\n') break;
                sb.Append(ch);
            }
            return sb.ToString();
        }

        public int Column {
            get {
                var pos = LineStart;
                var col = 1;
                while (pos < Offset) {
                    var ch = File.Content[pos++];
                    if (ch == '\t') col += 8;
                    else col++;
                }
                return col;
            }
        }
    }
}
