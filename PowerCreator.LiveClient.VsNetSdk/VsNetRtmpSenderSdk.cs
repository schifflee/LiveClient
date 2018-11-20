using System;
using System.Runtime.InteropServices;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetRtmpSenderSdk
    {

        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_AllocInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr RtmpSender_AllocInstance();



        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_FreeInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RtmpSender_FreeInstance(IntPtr intPtr);



        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_BeginWrite", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RtmpSender_BeginWrite(
            IntPtr intPtr,
            string serverIp,
            int serverPort,
            string channelName,
            string streamName,
            int videoHeaderData,
            int videoHeaderDataSize,
            int audioHeaderData,
            int audioHeaderDataSize
            );


        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_WriteVideo", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RtmpSender_WriteVideo(IntPtr intPtr, int data, int dataSize, bool keyFrame, int timeStamp);

        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_WriteAudio", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RtmpSender_WriteAudio(IntPtr intPtr, int data, int dataSize, int timeStamp);

        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_EndWrite", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RtmpSender_EndWrite(IntPtr intPtr);


        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_SendInThread", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RtmpSender_SendInThread(IntPtr intPtr, bool newVal);

        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_Pause", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RtmpSender_Pause(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "RtmpSender_Resume", CallingConvention = CallingConvention.Cdecl)]
        public static extern int RtmpSender_Resume(IntPtr intPtr);
    }
}
