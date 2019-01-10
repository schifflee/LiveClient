using PowerCreatorDotCom.Sdk.Core;
using PowerCreatorDotCom.Sdk.Core.Transform;
using PowerCreatorDotCom.Sdk.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.WebPlatform.Sdk
{
    public class CreateUploadVideoRequest : ServiceRequest<CreateUploadVideoResponse>
    {
        public CreateUploadVideoRequest(string domin)
            : base("Vod", "11", "Live/ResumeLive")
        {
            Url = domin;
            MyProperty = 1111;
        }
        private int myProperty;
        public int MyProperty
        {
            get
            {
                return myProperty;
            }
            set
            {
                myProperty = value;
                DictionaryUtil.Add(QueryParameters, "ResourceOwnerId", value.ToString());
            }
        }
        public override CreateUploadVideoResponse GetResponse(UnmarshallerContext unmarshallerContext)
        {
            return null;
        }
    }
}
