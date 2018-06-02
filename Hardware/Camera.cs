using System;
using System.Drawing;
using System.Linq;
using System.Timers;
using Thorlabs.TSI.TLCamera;
using Thorlabs.TSI.TLCameraInterfaces;
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

        private readonly Timer _timer = new Timer();
        private Bitmap _latestDisplayBitmap;
        private ITLCameraSDK _tlCameraSDK;
        private ITLCamera _tlCamera;
        private Result Open()
        {
            this._tlCameraSDK = TLCameraSDK.OpenTLCameraSDK();
            var serialNumbers = this._tlCameraSDK.DiscoverAvailableCameras();

            if (serialNumbers.Count > 0)
            {
                this._tlCamera = this._tlCameraSDK.OpenCamera(serialNumbers.First(), false);

                this._tlCamera.ExposureTime_us = 50000;
                if (this._tlCamera.GainRange.Maximum > 0)
                {
                    this._tlCamera.Gain = 90;
                }
                if (this._tlCamera.BlackLevelRange.Maximum > 0)
                {
                    this._tlCamera.BlackLevel = 48;
                }

                this._tlCamera.OperationMode = OperationMode.SoftwareTriggered;

                this._tlCamera.Arm();

                this._tlCamera.IssueSoftwareTrigger();

                this._timer.Interval = 50;
                this._timer.AutoReset = true;
                this._timer.Elapsed += this.DispatcherTimerUpdateUI_Tick;
                this._timer.Start();

                return new Result("Ok");
            }
            else
            {
                return new Result("Fail");
            }
        }

        private void DispatcherTimerUpdateUI_Tick(object sender, EventArgs e)
        {
            if (this._tlCamera != null)
            {
                // Check if a frame is available
                if (this._tlCamera.NumberOfQueuedFrames > 0)
                {
                    var frame = this._tlCamera.GetPendingFrameOrNull();
                    if (frame != null)
                    {
                        if (this._latestDisplayBitmap != null)
                        {
                            this._latestDisplayBitmap.Dispose();
                            this._latestDisplayBitmap = null;
                        }

                        this._latestDisplayBitmap = frame.ImageDataUShort1D.ToBitmap_Format24bppRgb();
                        //this.pictureBoxLiveImage.Invalidate();
                    }
                }
            }
        }

        private Result Close()
        {
            if (this._timer != null)
            {
                this._timer.Stop();
            }

            if (this._tlCameraSDK != null && this._tlCamera != null)
            {
                if (this._tlCamera.IsArmed)
                {
                    this._tlCamera.Disarm();
                }

                this._tlCamera.Dispose();
                this._tlCamera = null;

                this._tlCameraSDK.Dispose();
                this._tlCameraSDK = null;
            }

            return new Result("Ok");
        }
        private Result Read()
        {
            return new Result("Fail");
        }
    }
}
