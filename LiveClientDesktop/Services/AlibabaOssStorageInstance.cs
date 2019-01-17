using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.vod.Model.V20170321;
using Aliyun.OSS;
using LiveClientDesktop.Models;
using LiveClientDesktop.ServiceIntefaces;
using Newtonsoft.Json;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Services
{
    public class AlibabaOssStorageInstance : StorageInstance
    {
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        private const string _accessKeyId = "LTAIMIMfq3tWKw7R";
        private const string _accessKeySecret = "sfxbeB31Ifn4T8ZIqpH8jyJAiIMTUk";
        public AlibabaOssStorageInstance(IServiceClient serviceClient, WebPlatformApiFactory webPlatformApiFactory)
            : base(new AlibabaVodStorageInfo { Name = "AlibabaOss", ID = 9999 })
        {
            _serviceClient = serviceClient;
            _webPlatformApiFactory = webPlatformApiFactory;
        }

        protected override void StartUpload(RecordVideoInfo info)
        {
            //TODO: 凭证失效时需要刷新凭证
            var uploadAddressInfo = GetUploadAddress(info);

            var ossClient = new OssClient(uploadAddressInfo.UploadAddressInfo.Endpoint, uploadAddressInfo.UploadAuth.AccessKeyId, uploadAddressInfo.UploadAuth.AccessKeySecret, uploadAddressInfo.UploadAuth.SecurityToken);

            using (var fs = File.Open(Path.Combine(info.FileSavePath, info.FileName), FileMode.Open))
            {
                var putObjectRequest = new PutObjectRequest(uploadAddressInfo.UploadAddressInfo.Bucket, uploadAddressInfo.UploadAddressInfo.FileName, fs);
                putObjectRequest.StreamTransferProgress += StreamTransferProgress;
                ossClient.PutObject(putObjectRequest);
            }

            var result = _serviceClient.GetResponse(
                _webPlatformApiFactory.CreateSaveAlibabaVodVideoIdRequest(
                    uploadTaskInfo.RecordInfo.ScheduleId,
                    uploadAddressInfo.CreateUploadVideoResponse.VideoId,
                    info.VideoType.ToString(),
                    uploadTaskInfo.RecordInfo.Index
                    )
                );
            //TODO:videoid 保存成功时才能标记为上传完成
            if (!result.Success)
            {
                uploadTaskInfo.UploadStatus = result.Message;
                return;
            }
            MarkFileUploadCompleted(info);
        }

        private CreateUploadVideoResponseResult GetUploadAddress(RecordVideoInfo info)
        {
            DefaultAcsClient client = _initVodClient();
            CreateUploadVideoResponse response = client.GetAcsResponse(CreateUploadVideoRequest(info));
            return new CreateUploadVideoResponseResult()
            {
                CreateUploadVideoResponse = response,
                UploadAddressInfo = JsonConvert.DeserializeObject<UploadAddressInfo>(Encoding.UTF8.GetString(Convert.FromBase64String(response.UploadAddress))),
                UploadAuth = JsonConvert.DeserializeObject<UploadAuth>(Encoding.UTF8.GetString(Convert.FromBase64String(response.UploadAuth)))
            };
        }
        private CreateUploadVideoRequest CreateUploadVideoRequest(RecordVideoInfo info)
        {
            CreateUploadVideoRequest request = new CreateUploadVideoRequest
            {
                Title = uploadTaskInfo.RecordInfo.Title + info.VideoType,
                FileName = info.FileName
            };
            return request;
        }
        private DefaultAcsClient _initVodClient()
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-shanghai", _accessKeyId, _accessKeySecret);
            return new DefaultAcsClient(profile);
        }

        protected override void EndUpload()
        {
            MarkUploadCompleted();
        }
    }
}
