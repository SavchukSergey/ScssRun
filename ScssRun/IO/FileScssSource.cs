using System.IO;

namespace ScssRun.IO {
    public class FileScssSource : IScssSource {

        public virtual string LoadContent(string fileName) {
            using (var reader = new StreamReader(fileName)) {
                return reader.ReadToEnd();
            }
        }

        public virtual string ResolveFile(string fileName, string referrer) {
            var basePath = referrer != null ? Path.GetDirectoryName(referrer) : null;
            if (basePath != null) {
                var path = Path.Combine(basePath, fileName);
                if (FileExists(path)) return path;
            }

            return null;
        }

        private bool FileExists(string fileName) {
            return File.Exists(fileName);
        }

    }
}
