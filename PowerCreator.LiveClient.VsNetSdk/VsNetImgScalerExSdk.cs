using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetImgScalerExSdk
    {
        [DllImport("lib\\VSNet.dll", EntryPoint = "ImgScalerEx_AllocInstance", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr ImgScalerEx_AllocInstance();

        [DllImport("lib\\VSNet.dll", EntryPoint = "ImgScalerEx_FreeInstance", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern void ImgScalerEx_FreeInstance(IntPtr intPtr);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intPtr"></param>
        /// <param name="srcWidth">源宽</param>
        /// <param name="srcHeight">源长</param>
        /// <param name="srcFormat">0</param>
        /// <param name="dstWidht">输出宽</param>
        /// <param name="dstHeight">输出长</param>
        /// <param name="dstFormat">0</param>
        /// <param name="pRtSrc">0</param>
        /// <param name="pRtDst">0</param>
        /// <param name="nSrcLineSize">0</param>
        /// <param name="nDstLineSize">0</param>
        /// <returns></returns>
        [DllImport("lib\\VSNet.dll", EntryPoint = "ImgScalerEx_BeginConvert", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ImgScalerEx_BeginConvert(IntPtr intPtr, int srcWidth, int srcHeight, int srcFormat, int dstWidht, int dstHeight, int dstFormat, int pRtSrc, int pRtDst, int nSrcLineSize, int nDstLineSize);

        [DllImport("lib\\VSNet.dll", EntryPoint = "ImgScalerEx_Convert", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ImgScalerEx_Convert(IntPtr intPtr, ref byte inputBuff, int nInputBuffSize, ref byte pOutputBuff, int nOutputBuffSize);

        [DllImport("lib\\VSNet.dll", EntryPoint = "ImgScalerEx_EndConvert", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int ImgScalerEx_EndConvert(IntPtr intPtr);
    }
}
