using LiveClientDesktop.Models;

namespace LiveClientDesktop.ServiceIntefaces
{
    public interface IStorage
    {
        IStorageInfo StorageInfo { get; }
        void UploadFile(UploadTaskInfo taskInfo);
    }
}
