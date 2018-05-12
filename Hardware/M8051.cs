using System;
using System.Windows.Forms;

namespace Hardware
{
    public class M8051
    {
        public void Open()
        {
            uc480.Defines.Status statusRet = 0;

            Camera = new uc480.Camera();//Use only the empty constructor, the one with cameraID has a bug
            statusRet = Camera.Init();//You can specify a particular cameraId here if you want to open a specific camera
            if (statusRet != uc480.Defines.Status.SUCCESS)
            {
                MessageBox.Show("Camera initializing failed");
            }

            // Allocate Memory
            Int32 s32MemID;
            statusRet = Camera.Memory.Allocate(out s32MemID, true);
            if (statusRet != uc480.Defines.Status.SUCCESS)
            {
                MessageBox.Show("Allocate Memory failed");
            }

            // Start Live Video
            statusRet = Camera.Acquisition.Capture();
            if (statusRet != uc480.Defines.Status.SUCCESS)
            {
                MessageBox.Show("Start Live Video failed");
            }
            else
            {
                bLive = true;
            }

            // Connect Event
            Camera.EventFrame += onFrameEvent;
        }
        public void Read()
        {
        }
        public void Close()
        {
            Camera.Exit();
        }

        private uc480.Camera Camera;
        IntPtr displayHandle = IntPtr.Zero;
        private bool bLive = false;

        private void onFrameEvent(object sender, EventArgs e)
        {
            uc480.Camera Camera = sender as uc480.Camera;

            Int32 s32MemID;
            Camera.Memory.GetActive(out s32MemID);

            Camera.Display.Render(s32MemID, displayHandle, uc480.Defines.DisplayRenderMode.FitToWindow);
        }
    }
}