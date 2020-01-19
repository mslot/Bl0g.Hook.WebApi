using System;
using System.Collections.Generic;

namespace Bl0g.Hook.Core.GithubHook
{
    public partial class CommitInfo
    {
        public string Ref { get; set; }
        public string Before { get; set; }
        public string After { get; set; }
        public Repository Repository { get; set; }
        public Pusher Pusher { get; set; }
        public Sender Sender { get; set; }
        public bool Created { get; set; }
        public bool Deleted { get; set; }
        public bool Forced { get; set; }
        public object BaseRef { get; set; }
        public Uri Compare { get; set; }
        public List<Commit> Commits { get; set; }
        public Commit HeadCommit { get; set; }
    }
}
