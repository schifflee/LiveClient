namespace PowerCreator.LiveClient.Desktop
{
    public class StartupParameters : IStartupParameters
    {
        public string LiveId { get; private set; }

        public string Guid { get; private set; }

        public string Domain { get; private set; }

        public string Cookie { get; private set; }

        public StartupParameters(string liveId, string guid, string domain)
        {
            LiveId = liveId;
            Guid = guid;
            Domain = domain;
        }
    }
}
