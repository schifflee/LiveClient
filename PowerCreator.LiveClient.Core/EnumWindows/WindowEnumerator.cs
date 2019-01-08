using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerCreator.LiveClient.Core.Models;
using PowerCreator.LiveClient.VsNetSdk;
using static PowerCreator.LiveClient.VsNetSdk.User32Sdk;

namespace PowerCreator.LiveClient.Core.EnumWindows
{
    public class WindowEnumerator : IWindowEnumerator
    {
        private readonly IEnumerable<string> _prohibitedProcess = new List<string> { "explorer", "taskmgr", "miyunclient", "mmc", "qqpcrealtimespeedup", "scm", "systemsettings", "mstsc", "applicationframehost", "shellexperiencehost" };
        private readonly ICollection<WindowInfo> _windowList;

        public WindowEnumerator()
        {
            _windowList = new List<WindowInfo>();
        }
        public ICollection<WindowInfo> GetWindowList()
        {
            _windowList.Clear();
            EnumWindwos();
            return _windowList;
        }
        private void EnumWindwos()
        {
            EnumWindowsProc eunmWindows = new EnumWindowsProc(StartEnumWindows);
            User32Sdk.EnumWindows(eunmWindows, 0);
        }
        private bool StartEnumWindows(IntPtr p_Handle, int p_Param)
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
                _windowList.Add(new WindowInfo { HWD = p_Handle, WindowTitle = $"[{p.ProcessName}]{_TitleString.szText}" });
            }
            return true;
        }
    }
}
