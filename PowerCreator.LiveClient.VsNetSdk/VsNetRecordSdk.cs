using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetRecordSdk
    {
        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_AllocInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr FileMuxer_AllocInstance();


        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_FreeInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FileMuxer_FreeInstance(IntPtr intPtr);


        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_SetFileType", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_SetFileType(IntPtr intPtr, int fileType);

        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_EnableSync", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_EnableSync(IntPtr intPtr, int enable);


        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_SetTsClipDuration", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_SetTsClipDuration(IntPtr intPtr, int duration);



        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_BeginWrite", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_BeginWrite(IntPtr intPtr, string savePath, int videoHeaderData, int videoHeaedrDataSize, int audioHeaderData, int audioHeaderSize);


        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_WriteVideo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_WriteVideo(IntPtr intPtr, int data, int dataSize, bool keyFrame, int timeStamp);

        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_WriteAudio", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_WriteAudio(IntPtr intPtr, int data, int dataSize, bool keyFrame);

        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_EndWrite", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_EndWrite(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_EnableSync", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_EnableSync(IntPtr intPtr, bool nEnable);

        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_Pause", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_Pause(IntPtr intPtr);



        [DllImport("lib\\VSNet.dll", EntryPoint = "FileMuxer_Resume", CallingConvention = CallingConvention.Cdecl)]
        public static extern int FileMuxer_Resume(IntPtr intPtr);
    }
}
