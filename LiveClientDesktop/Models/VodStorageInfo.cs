using LiveClientDesktop.ServiceIntefaces;
using PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls;

namespace LiveClientDesktop.Models
{
    public class VodStorageInfo : IStorageInfo
    {
        public string Name { get; set; }

        public int ID { get; set; }

        public string Address { get; set; }

        public int Port { get; set; }

        public string Endpoint
        {
            get
            {
                return $"{Address}:{Port}";
            }
        }
    }
}
