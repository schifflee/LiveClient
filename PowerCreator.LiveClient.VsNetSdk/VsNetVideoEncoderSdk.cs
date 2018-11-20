using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetVideoEncoderSdk
    {
        [DllImport("lib\\VSNet.dll", EntryPoint = "VideoEncoderEx_AllocInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr VideoEncoderEx_AllocInstance();


        [DllImport("lib\\VSNet.dll", EntryPoint = "VideoEncoderEx_FreeInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VideoEncoderEx_FreeInstance(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "VideoEncoderEx_ForceKeyFrame", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VideoEncoderEx_ForceKeyFrame(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "VideoEncoderEx_StartEnc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VideoEncoderEx_StartEnc(IntPtr intPtr, IntPtr bitmapInfoHeader, int rate, int outputWidth, int outputHeight, BitmapInfoHeader convertBitmapInfoHeader, ref int size);

        [DllImport("lib\\VSNet.dll", EntryPoint = "VideoEncoderEx_EncData", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VideoEncoderEx_EncData(IntPtr intPtr, int data, int size, int tick, ref int outBuff, ref int outBuffSize, ref int outTick, ref bool frameKey);

        [DllImport("lib\\VSNet.dll", EntryPoint = "VideoEncoderEx_StopEnc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int VideoEncoderEx_StopEnc(IntPtr intPtr);
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct DataHeader
    {
        public int DataType;
        public int DataSize;
        public Int64 TimeStamp;
        public int KeyFrame;
    }
}
