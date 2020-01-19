namespace Bl0g.Hook.Communication.Core.Shared
{
    public class FileContent
    {
        public string FileName { get; }
        public string Content { get; }

        public FileContent(string fileName, string content)
        {
            FileName = fileName;
            Content = content;
        }
    }
}
