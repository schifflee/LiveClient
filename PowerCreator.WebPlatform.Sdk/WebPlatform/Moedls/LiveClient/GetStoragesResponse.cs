using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class GetStoragesResponse : ServiceResponse
    {
        public List<StorageInfo> Storages { get; set; }
    }
}
