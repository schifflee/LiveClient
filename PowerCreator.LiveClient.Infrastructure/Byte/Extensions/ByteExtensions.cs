using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.Infrastructure.Byte.Extensions
{
    public static class ByteExtensions
    {
        public static string GetString(this byte[] b)
        {
            return Encoding.GetEncoding("GB2312").GetString(b).TrimEnd('\0');
        }
    }
}
