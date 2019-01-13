namespace LiveClientDesktop
{
    public class StartupParameters
    {
        public string LiveId { get; set; }

        public string Guid { get; set; }
        public string Domain { get; set; }
        public string UserIdentity { get; set; }
        public int HttpServerPort { get; private set; }

        public StartupParameters(string liveId, string guid, string domain,int httpServerPort)
        {
            LiveId = liveId;
            Guid = guid;
            Domain = domain;
            HttpServerPort = httpServerPort;
        }
    }
}
