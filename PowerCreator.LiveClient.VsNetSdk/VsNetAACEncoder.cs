using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetAACEncoder
    {
        [DllImport("lib\\VSNet.dll", EntryPoint = "AACEncoder_AllocInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr AACEncoder_AllocInstance();

        [DllImport("lib\\VSNet.dll", EntryPoint = "AACEncoder_FreeInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AACEncoder_FreeInstance(IntPtr intPtr);


        [DllImport("lib\\VSNet.dll", EntryPoint = "AACEncoder_GenExtraData", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AACEncoder_GenExtraData(IntPtr intPtr, IntPtr waveFormatEx, WaveFormatEx pExtraData);


        [DllImport("lib\\VSNet.dll", EntryPoint = "AACEncoder_StartEnc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AACEncoder_StartEnc(IntPtr intPtr, IntPtr pWavFormat);


        [DllImport("lib\\VSNet.dll", EntryPoint = "AACEncoder_SetDataCallFunc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AACEncoder_SetDataCallFunc(IntPtr intPtr, DeviceDataCallBack action, int context);

        public delegate void DeviceDataCallBack(ref DataHeader dataHeader, IntPtr pData, int pContext);

        [DllImport("lib\\VSNet.dll", EntryPoint = "AACEncoder_EncData", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AACEncoder_EncData(IntPtr intPtr, int buffer, int size, int timeStamp);


        [DllImport("lib\\VSNet.dll", EntryPoint = "AACEncoder_StopEnc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int AACEncoder_StopEnc(IntPtr intPtr);
    }
}
