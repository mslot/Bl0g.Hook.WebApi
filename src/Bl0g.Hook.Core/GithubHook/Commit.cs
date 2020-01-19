using System;
using System.Collections.Generic;

namespace Bl0g.Hook.Core.GithubHook
{
    public partial class Commit
    {
        public string Id { get; set; }
        public string TreeId { get; set; }
        public bool Distinct { get; set; }
        public string Message { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public Uri Url { get; set; }
        public Author Author { get; set; }
        public Author Committer { get; set; }
        public List<string> Added { get; set; }
        public List<string> Removed { get; set; }
        public List<string> Modified { get; set; }
    }
}
