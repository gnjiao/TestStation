using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Timers;
using Thorlabs.TSI.TLCamera;
using Thorlabs.TSI.TLCameraInterfaces;
using Utils;
using System.Threading;

namespace Hardware
{
    public class M8051 : Camera
    {
        public static string TriggerType = "SoftwareTrigger";

        public M8051(object param) : base(param)
        {
            Type = "Camera";
            TriggerType = param as string;
            AssignLogger();
        }

        protected override Result Open()
        {
            this._tlCameraSDK = TLCameraSDK.OpenTLCameraSDK();
            var serialNumbers = this._tlCameraSDK.DiscoverAvailableCameras();

            if (serialNumbers.Count > 0)
            {
                this._tlCamera = this._tlCameraSDK.OpenCamera(serialNumbers.First(), false);

                DbgReadCameraInfo();

                //this._tlCamera.IsColorOperationEnabled = false;
                this._tlCamera.ExposureTime_us = 500000;
                if (this._tlCamera.GainRange.Maximum > 0)
                {
                    this._tlCamera.Gain = 90;
                }
                if (this._tlCamera.BlackLevelRange.Maximum > 0)
                {
                    this._tlCamera.BlackLevel = 48;
                }
                _tlCamera.IsColorOperationEnabled = false;

                if (TriggerType == "SoftwareTrigger")
                {
                    this._tlCamera.OperationMode = OperationMode.SoftwareTriggered;
                    this._tlCamera.Arm();
                    this._tlCamera.IssueSoftwareTrigger();
                }
                else
                {
                    this._tlCamera.OperationMode = OperationMode.HardwareTriggered;
                    this._tlCamera.TriggerPolarity = TriggerPolarity.ActiveHigh;
                    this._tlCamera.Arm();
                }

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
                while (this._tlCamera.NumberOfQueuedFrames > 0)
                {
                    var frame = this._tlCamera.GetPendingFrameOrNull();
                    if (frame != null)
                    {
                        _latestFrame = frame;
                    }
                }
            }
        }
        protected override Result Read(Dictionary<string, string> param)
        {
            _latestFrame = null;
            if (TriggerType == "HardwareTrigger")
            {
                //power on the product
            }

            int maxWait = 30;
            while (_latestFrame==null)
            {
                if (maxWait-- < 0)
                {
                    return new Result("Fail");
                }
                Thread.Sleep(1000);
            }

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
        protected override Result Config(Dictionary<string, string> param)
        {
            ROIAndBin value = _tlCamera.ROIAndBin;

            foreach (var key in param.Keys)
            {
                switch (key)
                {
                    case "Gain":
                        _tlCamera.Gain = UInt32.Parse(param[key]);
                        return new Result("Ok");
                    case "Exposure":
                        _tlCamera.ExposureTime_us = 1000 * UInt32.Parse(param[key]);
                        return new Result("Ok");
                    #region Color Config
                    case "IsColorOperationEnabled":
                        _tlCamera.IsColorOperationEnabled = bool.Parse(param[key]);
                        return new Result("Ok");
                    #endregion
                    #region ROIAndBin Config
                    case "RoiOriginX":
                        value.ROIOriginX_pixels += (uint)(double.Parse(param[key]) * value.ROIWidth_pixels);
                        break;
                    case "RoiOriginY":
                        value.ROIOriginY_pixels += (uint)(double.Parse(param[key]) * value.ROIWidth_pixels);
                        break;
                    case "RoiWidth":
                        value.ROIWidth_pixels = (uint)(double.Parse(param[key]) * value.ROIWidth_pixels);
                        break;
                    case "RoiHeight":
                        value.ROIHeight_pixels = (uint)(double.Parse(param[key]) * value.ROIHeight_pixels);
                        break;
                    case "BinX":
                        value.BinX = UInt32.Parse(param[key]);
                        break;
                    case "BinY":
                        value.BinX = UInt32.Parse(param[key]);
                        break;
                    #endregion
                    default:
                        return new Result("Fail", $"Error parameter {key}");
                }
            }

            _tlCamera.ROIAndBin = value;

            return new Result("Ok");
        }
        protected override Result Close()
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

        private void DbgReadCameraInfo()
        {
            _log.Info("SerialNumber:" + _tlCamera.SensorPixelSize_um);

            _log.Info("ROIHeightRange:" + _tlCamera.ROIHeightRange);
            _log.Info("ROIWidthRange:" + _tlCamera.ROIWidthRange);
            _log.Info("ROIAndBin:");
            _log.Info("ROIOriginX_pixels:" + _tlCamera.ROIAndBin.ROIOriginX_pixels);
            _log.Info("ROIOriginY_pixels:" + _tlCamera.ROIAndBin.ROIOriginY_pixels);
            _log.Info("ROIWidth_pixels:" + _tlCamera.ROIAndBin.ROIWidth_pixels);
            _log.Info("ROIHeight_pixels:" + _tlCamera.ROIAndBin.ROIHeight_pixels);
            _log.Info("BinX:" + _tlCamera.ROIAndBin.BinX);
            _log.Info("BinY:" + _tlCamera.ROIAndBin.BinY);

            _log.Info("SensorPixelSize_um:" + _tlCamera.SensorPixelSize_um);
            _log.Info("SensorHeight_pixels:" + _tlCamera.SensorHeight_pixels);
            _log.Info("SensorWidth_pixels:" + _tlCamera.SensorWidth_pixels);
        }

        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private Bitmap _latestDisplayBitmap;
        private ITLCameraSDK _tlCameraSDK;
        private ITLCamera _tlCamera;
        private Frame _latestFrame;
    }
}