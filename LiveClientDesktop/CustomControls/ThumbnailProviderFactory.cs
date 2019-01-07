using LiveClientDesktop.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveClientDesktop.CustomControls
{
    public class ThumbnailProviderFactory : System.Utility.Patterns.ISimpleFactory<EnumThumbnail, IThumbnailProvider>
    {
        /// <summary>
        /// 根据key获取实例
        /// </summary>
        public virtual IThumbnailProvider GetInstance(EnumThumbnail key)
        {
            switch (key)
            {
                case EnumThumbnail.Image:
                    return Singleton<ImageThumbnailProvider>.GetInstance();
            }
            return null;
        }
    }
}
