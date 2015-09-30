using System;

namespace ScssRun.IO {
    public class NullScssSource : IScssSource {

        public string LoadContent(string fileName) {
            throw new InvalidOperationException();
        }

        public string ResolveFile(string fileName, string referrer) {
            throw new InvalidOperationException();
        }
    }
}
