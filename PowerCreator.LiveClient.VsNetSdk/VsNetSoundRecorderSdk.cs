using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetSoundRecorderSdk
    {
        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_CreateInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SoundRecorder_CreateInstance();


        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_FreeInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SoundRecorder_FreeInstance(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_OpenRecorder", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SoundRecorder_OpenRecorder(IntPtr intPtr, int index, int ptr);


        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_CloseRecorder", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SoundRecorder_CloseRecorder(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_GetFormat", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SoundRecorder_GetFormat(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_GetDataSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SoundRecorder_GetDataSize(IntPtr intPtr);


        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_GetData", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SoundRecorder_GetData(IntPtr intPtr, ref byte pBuffer, int nBuffSize);


        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_GetSoundRecordCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern int SoundRecorder_GetSoundRecordCount(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "SoundRecorder_GetSoundRecordName", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        public static extern IntPtr SoundRecorder_GetSoundRecordName(IntPtr intPtr, int index);


        [DllImport("lib\\VSNet.dll", EntryPoint = "GetAudioLevel", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetAudioLevel(IntPtr pFormat, int pBuffer, int nBuffSize, ref int pLeftLevel, ref int pRightLevel);
    }
}
