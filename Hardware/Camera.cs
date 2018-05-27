using System;
using Utils;

namespace Hardware
{
    public class Camera : Equipment
    {
        public Camera(object param) : base(param)
        {
            Type = "Camera";
            AssignLogger();
        }
        protected override Result _execute(Command cmd)
        {
            switch (cmd.Id)
            {
                case "Open":
                    return Open();
                case "Close":
                    return Close();
                case "Read":
                    return Read();
                default:
                    return new Result("Command doesn't support");
            }
        }

        private uc480.Camera _camera;
        IntPtr displayHandle = IntPtr.Zero;
        private bool bLive = false;

        private Result Open()
        {
            uc480.Defines.Status statusRet = 0;
            Int32 s32MemID;

            _camera = new uc480.Camera();//Use only the empty constructor, the one with cameraID has a bug

            if ((statusRet = _camera.Init()) != uc480.Defines.Status.SUCCESS)
            {
                _log.Error("Failed to initialize(" + statusRet.ToString() + ")");
                return new Result("Fail", "Failed to initialize");
            }
            if ((statusRet = _camera.Memory.Allocate(out s32MemID, true)) != uc480.Defines.Status.SUCCESS)
            {
                _log.Error("Allocate Memory failed(" + statusRet.ToString() + ")");
                return new Result("Fail", "Failed to allocate memory");
            }

            #region Start Live Video
            //statusRet = _camera.Acquisition.Capture();
            //if (statusRet != uc480.Defines.Status.SUCCESS)
            //{
            //      _log.Error("Allocate Memory failed");
            //}
            //else
            //{
            //    bLive = true;
            //}
            #endregion

            _camera.EventFrame += onFrameEvent;

            return new Result("Ok");
        }
        private void onFrameEvent(object sender, EventArgs e)
        {
            uc480.Camera camera = sender as uc480.Camera;

            Int32 s32MemID;
            camera.Memory.GetActive(out s32MemID);
            camera.Memory.Lock(s32MemID);

            camera.Display.Render(s32MemID, displayHandle, uc480.Defines.DisplayRenderMode.FitToWindow);

            camera.Memory.Unlock(s32MemID);
        }

        private Result Close()
        {
            _camera.Exit();
            return new Result("Ok");
        }
        private Result Read()
        {
            uc480.Defines.Status result = _camera.Acquisition.Freeze();
            if (result == uc480.Defines.Status.SUCCESS)
            {
                bLive = false;
                return new Result("Ok");
            }

            return new Result("Fail", result.ToString());
        }
    }
}
