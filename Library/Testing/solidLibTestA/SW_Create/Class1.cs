using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace SW_Interface;

public class SW_Create
{
    [DllImport("ole32.dll")]
        private static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        private ISldWorks sldApp = null;
        private Process process;
        private int processID;
        private string path = @"C:\Program Files\SOLIDWORKS Corp\SOLIDWORKS\SLDWORKS.exe";
        private int[] window = {0, 0, 1920, 1080};

        private bool taskPanePinned = false;

        public void setTaskPanePinned(bool pinned) {
            this.taskPanePinned = pinned;
            this.refreshTaskPane();
        }

        public void refreshTaskPane() {
            this.sldApp.TaskPaneIsPinned = this.taskPanePinned;
        }
        public void setWindow(int[] window) {
            this.window = window;
        }
        public void refreshWindow() {
            this.sldApp.FrameLeft = this.window[0] - 10;
            this.sldApp.FrameTop = this.window[1];
            this.sldApp.FrameWidth = this.window[2];
            this.sldApp.FrameHeight = this.window[3];
            this.refreshTaskPane();
        }

        public bool startFromArgs(string[] args) {
            if (args.Length > 0) {
                return this.connectToProcess(Int32.Parse(args[0]));
            } else {
                this.startSW();
                Thread.Sleep(30000);
                return this.connectToProcess(processID);
            }
        }
        public int startSW() {
            this.process = Process.Start(this.path);
            //TODO check file and return validity
            this.processID = this.process.Id;
            return this.processID;
        }

        public bool connectToProcess(int processId) {
            sldApp = this.GetSwAppFromProcess(processId);
            if (sldApp != null) {
                return true;
            }
            return false;
        }

        private ISldWorks GetSwAppFromProcess(int processId)
        {
            var monikerName = "SolidWorks_PID_" + processId.ToString();

            IBindCtx context = null;
            IRunningObjectTable rot = null;
            IEnumMoniker monikers = null;

            try
            {
                CreateBindCtx(0, out context);

                context.GetRunningObjectTable(out rot);
                rot.EnumRunning(out monikers);

                var moniker = new IMoniker[1];

                while (monikers.Next(1, moniker, IntPtr.Zero) == 0)
                {
                    var curMoniker = moniker.First();

                    string name = null;

                    if (curMoniker != null)
                    {
                        try
                        {
                            curMoniker.GetDisplayName(context, null, out name);
                        }
                        catch (UnauthorizedAccessException)
                        {
                        }
                    }

                    if (string.Equals(monikerName,
                        name, StringComparison.CurrentCultureIgnoreCase))
                    {
                        object app;
                        rot.GetObject(curMoniker, out app);
                        return app as ISldWorks;
                    }
                }
            }
            finally
            {
                if (monikers != null)
                {
                    Marshal.ReleaseComObject(monikers);
                }

                if (rot != null)
                {
                    Marshal.ReleaseComObject(rot);
                }

                if (context != null)
                {
                    Marshal.ReleaseComObject(context);
                }
            }

            return null;
        }
}
