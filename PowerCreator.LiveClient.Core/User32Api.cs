using System;
using System.Runtime.InteropServices;

namespace PowerCreator.LiveClient.Core
{
    public sealed class User32Api
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, UInt32 nFlags);



        public delegate bool EnumWindowsProc(IntPtr p_Handle, int p_Param);
        [DllImport("user32.dll")]
        public static extern int EnumWindows(EnumWindowsProc ewp, int lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hWnd, out STRINGBUFFER text, int nMaxCount);
        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, ref int lpdwProcessId);
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STRINGBUFFER
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string szText;
        }

        [DllImport("user32")]
        public static extern int GetParent(int hwnd);

        [DllImport("user32")]
        public static extern int SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    }
}
