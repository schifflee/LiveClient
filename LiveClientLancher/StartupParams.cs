using System.Collections.Generic;

namespace LiveClientLancher
{
    public class StartupParams
    {
        public static StartupParams Instance { get; } = new StartupParams();

        public string ClientStartupParams { get; set; }

        public string AppCastUrl { get; set; }

        public List<VersionInfo> LastVersionList { get; set; }

        public int CurrentVersion { get; set; }
        public string CurrentVersionName { get; set; }

        public string appDir { get; set; }
        private StartupParams()
        {

        }
    }
}
