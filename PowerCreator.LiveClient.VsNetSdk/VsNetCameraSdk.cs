using DirectShowLib;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace PowerCreator.LiveClient.VsNetSdk
{
    public sealed class VsNetCameraSdk
    {
        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_AllocInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Camera_AllocInstance();

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_GetCount", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Camera_GetCount(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_GetName", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Camera_GetName(IntPtr intPtr, int index, ref byte cameraName);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_GetInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Camera_GetInfo(IntPtr intPtr, ref int width, ref int height, ref int format);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_OpenCamera", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Camera_OpenCamera(IntPtr intPtr, int nCamID, bool bDisplayProperties, int width, int height);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_QueryFrame", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int Camera_QueryFrame(IntPtr intPtr, ref byte pBuffer, int size);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_GetInfoEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Camera_GetInfoEx(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_GetInfoEx", CallingConvention = CallingConvention.Cdecl)]
        public static extern BitmapInfoHeader Camera_GetInfoEx1(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_CloseCamera", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Camera_CloseCamera(IntPtr intPtr);

        [DllImport("lib\\VSNet.dll", EntryPoint = "Camera_FreeInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Camera_FreeInstance(IntPtr intPtr);
    }
}
