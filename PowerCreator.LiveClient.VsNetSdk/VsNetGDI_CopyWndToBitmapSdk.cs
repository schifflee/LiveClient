using DirectShowLib;
using System;
using System.Runtime.InteropServices;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetGDI_CopyWndToBitmapSdk
    {
        [DllImport("lib\\VSNet.dll", EntryPoint = "GDI_CopyWndToBitmap", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool GDI_CopyWndToBitmap(IntPtr ptr, ref byte buff, ref BitmapInfo bitmapInfo);


        [DllImport("lib\\VSNet.dll", EntryPoint = "GDI_WriteBmp", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool GDI_WriteBmp(string savePath, BitmapInfoHeader bitmapInfo, ref byte buff);

        [DllImport("lib\\VSNet.dll", EntryPoint = "GDI_GetWndWH", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern bool GDI_GetWndWH(IntPtr intPtr, ref int w, ref int h);
    }
}
