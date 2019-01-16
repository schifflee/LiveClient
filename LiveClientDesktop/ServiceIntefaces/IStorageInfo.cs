using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.ServiceIntefaces
{
    public interface IStorageInfo
    {
        string Name { get; }
        int ID { get; }
    }
}
