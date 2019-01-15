using LiveClientDesktop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ServiceIntefaces
{
    public interface IStorageService
    {
        
        
        void UploadFile(List<UploadFileInfo> uploadList);
    }
}
