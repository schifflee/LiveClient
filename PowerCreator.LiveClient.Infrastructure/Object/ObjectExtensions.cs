using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Infrastructure.Object
{
    public static class ObjectExtensions
    {
        public static IntPtr ToIntPtrHandle(this object obj)
        {
            GCHandle hObject = GCHandle.Alloc(obj, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();

            if (hObject.IsAllocated)
                hObject.Free();

            return pObject;
        }
        public static int ToIntHandle(this object obj)
        {
            return obj.ToIntPtrHandle().ToInt32();
        }
    }
}
