using PowerCreator.LiveClient.Core;

namespace LiveClientDesktop.Models
{
    public class LiveStreamAddressInfo : ILiveStreamAddressInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string AppName { get; set; }
        public string StreamName { get; set; }
        public string Type { get; set; }

        public override string ToString()
        {
            return $"{Host}:{Port}/{AppName}/{StreamName}/{Type}";
        }
    }
}
