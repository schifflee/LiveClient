using LiveClientDesktop.Models;
using LiveClientDesktop.ServiceIntefaces;
using PowerCreatorDotCom.Sdk.Core;
using System;
using System.Collections.Generic;

namespace LiveClientDesktop.Services
{
    public class StorageProvider
    {
        private readonly LiveInfo _liveInfo;
        private readonly IServiceClient _serviceClient;
        private readonly WebPlatformApiFactory _webPlatformApiFactory;
        public StorageProvider(LiveInfo liveInfo, IServiceClient serviceClient, WebPlatformApiFactory webPlatformApiFactory)
        {
            _liveInfo = liveInfo;
            _serviceClient = serviceClient;
            _webPlatformApiFactory = webPlatformApiFactory;
        }

        public Tuple<bool, Tuple<List<IStorage>, string>> GetStorages()
        {
            var rsp = _serviceClient.GetResponse(_webPlatformApiFactory.CreateGetStoragesRequest(_liveInfo.ScheduleID));
            List<IStorage> storages = new List<IStorage>();
            if (!rsp.Success)
                return WarpperResult(rsp.Success, rsp.Message, null);
            if (rsp.Value != null)
            {
                foreach (var storageInfo in rsp.Value.Storages)
                {
                    storages.Add(new VodStorageInstance(new VodStorageInfo()
                    {
                        Address = storageInfo.Address,
                        ID = storageInfo.ID,
                        Name = storageInfo.Name,
                        Port = storageInfo.Port,
                    }, _serviceClient, _webPlatformApiFactory));
                }
            }

            storages.Add(new AlibabaOssStorageInstance(_serviceClient, _webPlatformApiFactory));

            return WarpperResult(true, null, storages);
        }
        private Tuple<bool, Tuple<List<IStorage>, string>> WarpperResult(bool success, string msg, List<IStorage> storages)
        {
            return new Tuple<bool, Tuple<List<IStorage>, string>>(success, new Tuple<List<IStorage>, string>(storages, msg));
        }
    }
}
