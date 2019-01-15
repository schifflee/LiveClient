using PowerCreatorDotCom.Sdk.Core;
using System.Collections.Generic;

namespace PowerCreator.WebPlatform.Sdk.WebPlatform.Moedls
{
    public class GetLiveDocumentsResponse : ServiceResponse
    {
        public List<DocumentInfo> Documents { get; set; }
    }
}
