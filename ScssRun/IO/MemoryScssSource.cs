using System;

namespace ScssRun.IO {
    public class MemoryScssSource : IScssSource {

        private readonly string _content;

        public MemoryScssSource(string content) {
            _content = content;
        }

        public string LoadContent(string fileName) {
            if (fileName == "main.scss") {
                return _content;
            }
            throw new InvalidOperationException();
        }

        public string ResolveFile(string fileName, string referrer) {
            return fileName;
        }
    }
}
