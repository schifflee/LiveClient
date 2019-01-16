using LiveClientDesktop.Models;
using LiveClientDesktop.ServiceIntefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.Services
{
    public class AlibabaOssStorage : IStorage
    {
        public IStorageInfo StorageInfo { get; }

        public AlibabaOssStorage()
        {
            StorageInfo = new AlibabaVodStorageInfo { Name = "AlibabaVodServer", ID = 9999 };
        }
        public void UploadFile(UploadTaskInfo taskInfo)
        {

        }
    }
}
