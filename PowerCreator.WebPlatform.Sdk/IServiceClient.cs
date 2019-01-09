using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.WebPlatform.Sdk
{
    public interface IServiceClient
    {
        T GetResponse<T>();
    }
}
