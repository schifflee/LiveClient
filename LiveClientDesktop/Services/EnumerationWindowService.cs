using LiveClientDesktop.Models;
using PowerCreator.LiveClient.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PowerCreator.LiveClient.Core.User32Api;

namespace LiveClientDesktop.Services
{
    public class EnumerationWindowService
    {
        private readonly IEnumerable<string> _prohibitedProcess = new List<string> { "explorer", "taskmgr", "miyunclient", "mmc", "qqpcrealtimespeedup", "scm", "systemsettings", "mstsc", "applicationframehost", "shellexperiencehost" };
        private readonly ObservableCollection<PreviewWindowInfo> _windowList;
        private readonly string _tempImageSavePath;
        public EnumerationWindowService(SystemConfig config)
        {
            _windowList = new ObservableCollection<PreviewWindowInfo>();
            _tempImageSavePath = config.TempImageSavePath;
        }
        public ObservableCollection<PreviewWindowInfo> GetWindowList()
        {
            _windowList.Clear();
            MessageWindwos();
            return _windowList;
        }
        private void MessageWindwos()
        {
            EnumWindowsProc eunmWindows = new EnumWindowsProc(NetEnumWindows);
            EnumWindows(eunmWindows, 0);
        }
        private bool NetEnumWindows(IntPtr p_Handle, int p_Param)
        {
            if (!IsWindowVisible(p_Handle) || GetParent(p_Handle.ToInt32()) != 0) return true;

            int pid = 0;
            GetWindowThreadProcessId(p_Handle, ref pid);
            var p = Process.GetProcessById(pid);
            if (_prohibitedProcess.Any(proc => proc == p.ProcessName.ToLower()))
            {
                return true;
            }

            STRINGBUFFER _TitleString = new STRINGBUFFER();
            GetWindowText(p_Handle, out _TitleString, 256);
            if (!string.IsNullOrEmpty(_TitleString.szText))
            {
                string fileName = Guid.NewGuid() + ".jpg";
                _windowList.Add(new PreviewWindowInfo { HWD = p_Handle, WindowTitle = $"[{p.ProcessName}]{_TitleString.szText}", PreviewWindowImagePath = $"{Path.Combine(_tempImageSavePath, fileName)}" });
                CaptureWindow(p_Handle, fileName);
            }
            return true;
        }

        private void ConvertTo32bpp(Image img, string fileName)
        {
            try
            {
                var bmp = new Bitmap(280, 160, PixelFormat.Format24bppRgb);
                using (var gr = Graphics.FromImage(bmp))
                    gr.DrawImage(img, 0, 0, 280, 160);
                if (!Directory.Exists(_tempImageSavePath))
                {
                    Directory.CreateDirectory(_tempImageSavePath);
                }
                bmp.Save(Path.Combine(_tempImageSavePath, fileName), ImageFormat.Jpeg);
                bmp.Dispose();
            }
            catch (Exception e) { }

        }
        private void CaptureWindow(IntPtr handle, string fileName)
        {
            IntPtr hdcSrc = GetWindowDC(handle);
            RECT windowRect = new RECT();
            GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            width -= (width % 4);
            IntPtr hdcDest = GDI32Api.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32Api.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = GDI32Api.SelectObject(hdcDest, hBitmap);
            GDI32Api.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32Api.SRCCOPY);
            GDI32Api.SelectObject(hdcDest, hOld);
            GDI32Api.DeleteDC(hdcDest);
            User32Api.ReleaseDC(handle, hdcSrc);

            Image img = Image.FromHbitmap(hBitmap);
            ConvertTo32bpp(img, fileName);
            img.Dispose();

            GDI32Api.DeleteObject(hBitmap);
        }
        public void ClearPreviewImage()
        {
            if (Directory.Exists(_tempImageSavePath))
                Directory.Delete(_tempImageSavePath, true);
        }
    }
}
