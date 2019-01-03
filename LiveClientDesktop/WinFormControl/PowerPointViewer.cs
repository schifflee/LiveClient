using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OFFICECORE = Microsoft.Office.Core;
using POWERPOINT = Microsoft.Office.Interop.PowerPoint;
using THREAD = System.Threading;
using System.Runtime.InteropServices;
using LiveClientDesktop.Services;

namespace LiveClientDesktop.WinFormControl
{
    public partial class PowerPointViewer: UserControl
    {
        POWERPOINT.Application objApp = null;

        POWERPOINT.SlideShowSettings objSSS;

        POWERPOINT.SlideShowWindow objSSWs;

        IntPtr pptIntPtr;
        private bool isManualClose;
        private Action<bool> PPTViewClose;

        private THREAD.Timer _movePPTWindowTimer;
        public PowerPointViewer()
        {
            InitializeComponent();

            _movePPTWindowTimer = new THREAD.Timer((state) => moveWindow(), null, THREAD.Timeout.Infinite, THREAD.Timeout.Infinite);
        }

        public void OpenPPT(string filePath)
        {
            if (this.objApp == null)
            {
                objApp = new POWERPOINT.Application();
                objApp.PresentationCloseFinal += ObjApp_PresentationCloseFinal;
            }

            try
            {
                var objPresSet= PresentationInstanceRepository.Instance[filePath];
                if (objPresSet == null)
                {
                    objPresSet = objApp.Presentations.Open(filePath, OFFICECORE.MsoTriState.msoCTrue, OFFICECORE.MsoTriState.msoFalse, OFFICECORE.MsoTriState.msoFalse);
                    PresentationInstanceRepository.Instance[filePath] = objPresSet;
                    objPresSet.Windows.Application.PresentationClose += Application_PresentationClose; ;

                }
                objSSS = objPresSet.SlideShowSettings;
                objSSS.ShowType = POWERPOINT.PpSlideShowType.ppShowTypeSpeaker;
                objSSS.LoopUntilStopped = OFFICECORE.MsoTriState.msoCTrue;
                objSSS.RangeType = POWERPOINT.PpSlideShowRangeType.ppShowSlideRange;

                objSSWs = objSSS.Run();
                pptIntPtr = (IntPtr)objSSWs.HWND;
                SetParent(pptIntPtr, this.Handle);
                isManualClose = true;
                _movePPTWindowTimer.Change(300, 300);
            }
            catch (Exception ex)
            {
                try
                {
                    this.objApp.Quit();
                }
                catch (Exception) { }
            }
        }

        private void Application_PresentationClose(POWERPOINT.Presentation Pres)
        {
            throw new NotImplementedException();
        }

        private void ObjApp_PresentationCloseFinal(POWERPOINT.Presentation Pres)
        {
            throw new NotImplementedException();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr childIntPtr, IntPtr parentIntPtr);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr childIntPtr, int x, int y, int w, int h, bool b);

        private void PPTViewer_Resize(object sender, EventArgs e)
        {
            moveWindow();
        }

        private void moveWindow()
        {

            if (_movePPTWindowTimer != null)
                _movePPTWindowTimer.Change(THREAD.Timeout.Infinite, THREAD.Timeout.Infinite);
            if (pptIntPtr != IntPtr.Zero)
            {
                MoveWindow(pptIntPtr, 0, 0, this.Width, this.Height, true);
            }
        }
    }
}
