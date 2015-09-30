namespace ScssRun.IO {
    public interface IScssSource {

        string LoadContent(string fileName);

        string ResolveFile(string fileName, string referrer);

    }
}
