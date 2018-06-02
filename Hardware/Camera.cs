using System;
using System.Collections.Generic;
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
                    return Read(cmd.Param);
                case "Set":
                default:
                    return new Result("Command doesn't support");
            }
        }

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
                if (this._tlCamera.NumberOfQueuedFrames > 0)
                {
                    var frame = this._tlCamera.GetPendingFrameOrNull();
                    if (frame != null && _latestFrame==null)
                    {
                        _latestFrame = frame;
                    }
                }
            }
        }

        private Result Read(Dictionary<string, string> param)
        {
            string type = param["Type"];

            switch (type)
            {
                case "Bmp":
                    return new Result("Ok", "Bmp", _latestFrame.ImageDataUShort1D.ToBitmap_Format24bppRgb());
                case "Raw":
                    return new Result("Ok", "Raw", _latestFrame.ImageDataUShort1D.ImageData_monoOrBGR);
            }

            return new Result("Fail");
        }

        private Result Close()
        {
            this._timer?.Stop();

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

        private readonly Timer _timer = new Timer();
        private Bitmap _latestDisplayBitmap;
        private ITLCameraSDK _tlCameraSDK;
        private ITLCamera _tlCamera;
        private Frame _latestFrame;
    }
}
