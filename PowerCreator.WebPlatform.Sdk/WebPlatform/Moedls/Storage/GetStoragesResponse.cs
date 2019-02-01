using PowerCreatorDotCom.Sdk.Core;
using System.Collections.Generic;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls.Storage
{
    public class GetStoragesResponse : ServiceResponse
    {
        public List<StorageInfo> Storages { get; set; }
    }
}
