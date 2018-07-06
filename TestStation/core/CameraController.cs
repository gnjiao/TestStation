using Hardware;
using JbImage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using Utils;

namespace TestStation.core
{
    public class CameraController
    {
        private Logger _log = new Logger(typeof(CameraController));
        public Camera mCamera;

        public Bitmap LatestImage;
        public Bitmap AnalyzedImage;

        public CameraController()
        {
            _log.Debug(Config.ToString());
        }

        string _filePath;
        private List<double> _distances = new List<double>();
        private List<EmguCircleImage> _imgs = new List<EmguCircleImage>();
        private string _testType = "";
        public Result Open(string cameraType)
        {
            _testType = cameraType;

            if (mCamera == null)
            {
                if (!string.IsNullOrEmpty(Config.ForceCameraType))
                {
                    HardwareSrv.GetInstance().Add(new M8051("Camera"));
                }
                else
                {
                    switch (cameraType)
                    {
                        case "NFT":
                            HardwareSrv.GetInstance().Add(new M8051("Camera"));
                            break;
                        case "FFT":
                            HardwareSrv.GetInstance().Add(new Vcxu("Camera"));
                            break;
                        default:
                            return new Result("Fail", "Unknown camera type");
                    }
                }

                mCamera = HardwareSrv.GetInstance().Get("Camera") as Camera;

                try
                {
                    Result ret = mCamera.Execute(new Command("Open"));
                    if (ret.Id != "Ok")
                    {
                        mCamera = null;
                    }
                    return ret;
                }
                catch (Exception ex)
                {
                    _log.Error("Failed to open the camera", ex);
                    mCamera = null;
                    return new Result("Fail", ex.ToString());
                }
            }
            else
            {
                return new Result("Dummy", $"Camera({mCamera.Name}) is already opened");
            }
        }
        public Result Read(double distance = double.NaN)
        {
            if (mCamera != null)
            {
                Result ret = mCamera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Bmp" } }));
                if (ret.Id == "Ok")
                {
                    InsertImg(ret.Param as Bitmap, distance, true);
                }

                return ret;
            }
            else
            {
                return new Result("Fail", "Please open camera first");
            }
        }
        public Result Load(string filename, double distance = double.NaN)
        {
            LatestImage = new Bitmap(filename);
            InsertImg(LatestImage, distance, false);
            return new Result("Ok");
        }
        public Result Analyze(string testType, double distance)
        {
            return Analyze(testType, Config.RadiusLimit(distance));
        }
        public Result Analyze(string testType, int[] radiusLimit)
        {
            EmguCircleImage image = new EmguCircleImage(_filePath, testType, radiusLimit);
            _imgs.Add(image);

            //image.FilterOnStrength(Config.CountThreshold);
            AnalyzedImage = image.DrawCircles(testType);
            _log.Debug($"Analyze image {_filePath} use " +
                $"CountThreshold {Config.CountThreshold} " +
                $"RadiusLimits {radiusLimit[0]} {radiusLimit[1]}");
            _log.Debug(Utils.String.Flatten(image.StatisticInfo()));

            return new Result("Ok");
        }
        private Result _calculate()
        {
            double[] result = new double[_imgs[0].FilteredCircles.Count];
            for (int circleId = 0; circleId < result.Length; circleId++)
            {
                try
                {
                    double[] radius = new double[_imgs.Count];

                    for (int imgId = 0; imgId < radius.Length; imgId++)
                    {
                        radius[imgId] = _imgs[imgId].FilteredCircles[circleId].Radius;
                    }
                    string output = "";
                    for (int i = 0; i < radius.Length; i++)
                    {
                        output += radius[i].ToString("F2") + ",";
                    }
                    if (output.Length > 0)
                    {
                        output = output.Substring(0, output.Length - 1);
                    }
                    _log.Debug($"Circle{circleId} radius: {output}");

                    result[circleId] = Matlab.CalcWeist2(radius, _distances.ToArray());
                    _log.Info($"{circleId}: {result[circleId]}" + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    result[circleId] = double.NaN;
                    _log.Error($"Failed to calcute weist for {circleId}", ex);
                }
            }

            return new Result("Ok");
        }
        public Result Calculate()
        {
            Result ret;

            if (_imgs.Count > 0 && _imgs.Count == _distances.Count)
            {
                ret =  _calculate();
            }
            else if (_imgs.Count == 0)
            {
                ret = new Result("Fail", "No analyzed images found");
            }
            else
            {
                ret = new Result("Fail", "Calculation can only apply to images with distance parameter, please re-do the test");
            }

            _distances.Clear();
            _imgs.Clear();

            return ret;
        }
        public Result Close()
        {
            mCamera?.Execute(new Command("Close"));
            mCamera = null;
            return new Result("Ok");
        }
        public Result SetGain(int gain)
        {
            return mCamera.Execute(new Command("Config", new Dictionary<string, string>() {
                { "Gain", gain.ToString() }
            }));
        }
        public Result SetExposure(int ms)
        {
            return mCamera.Execute(new Command("Config", new Dictionary<string, string>() {
                { "Exposure", ms.ToString() }
            }));
        }
        private void InsertImg(Bitmap img, double distance, bool shouldSave = true)
        {
            if (!double.IsNaN(distance))
            {
                _distances.Add(distance);
            }

            _filePath = @"data/" + $"Img_{distance}_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.bmp";
            img.Save(_filePath, ImageFormat.Bmp);
        }
        private class Config
        {
            public static string ForceCameraType
            {
                get
                {
                    return ConfigurationManager.AppSettings["ForceCameraType"];
                }
            }
            public static double CountThreshold
            {
                get
                {
                    return double.Parse(ConfigurationManager.AppSettings["CountThreshold"]);
                }
            }
            public static int[] RadiusLimit(double distance)
            {
                int[] ret = new int[2];

                double value = distance;
                if (!double.IsNaN(value))
                {
                    string index = "Min" + ((int)value).ToString("D2");
                    ret[0] = Int32.Parse(ConfigurationManager.AppSettings[index]);
                    index = "Max" + ((int)value).ToString("D2");
                    ret[1] = Int32.Parse(ConfigurationManager.AppSettings[index]);
                }
                else
                {
                    ret[0] = Int32.Parse(ConfigurationManager.AppSettings["Min"]);
                    ret[1] = Int32.Parse(ConfigurationManager.AppSettings["Max"]);
                }

                return ret;
            }
            public static new string ToString()
            {
                string output = "";
                foreach (string key in ConfigurationManager.AppSettings.Keys)
                {
                    output += $"{key}:{ConfigurationManager.AppSettings[key]}" + ",";
                }
                output.Substring(0, output.Length - 1);
                return output;
            }
        }
        private int[] RadiusLimits(double distance)
        {
            int[] ret = new int[2];

            double value = distance;
            if (!double.IsNaN(value))
            {
                string index = "Min" + ((int)value).ToString("D2");
                ret[0] = Int32.Parse(ConfigurationManager.AppSettings[index]);
                index = "Max" + ((int)value).ToString("D2");
                ret[1] = Int32.Parse(ConfigurationManager.AppSettings[index]);
            }
            else
            {
                ret[0] = Int32.Parse(ConfigurationManager.AppSettings["Min"]);
                ret[1] = Int32.Parse(ConfigurationManager.AppSettings["Max"]);
            }

            return ret;
        }

        /* to be obsoleted */
        private void ProcessWithCircleFinder()
        {
            //var rawData = mCamera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Raw" } })).Param as Bitmap;

            string resultBmp = Utils.String.FilePostfix(_filePath, "-result");
            string resultTxt = resultBmp.Replace(".bmp", ".txt");

            Bitmap bmpFile = ImgProcess.Binarize(_filePath);
            CirclesFinder f = new CirclesFinder(bmpFile);

            LatestImage = f.Draw(resultBmp);

            ImgProcess.Count(f.Rounds);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(resultTxt, true))
            {
                file.Write(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}",
                    "ID", "X", "Y",
                    "LengthOnX", "DeviationOnX",
                    "LengthOnY", "DeviationOnY",
                    "Weight", "DeviationOnWeight") + Environment.NewLine);

                foreach (var round in f.Rounds)
                {
                    file.Write(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}",
                        round.Id.ToString("D3"), round.CenterX, round.CenterY,
                        round.MaxLenLine.Length, round.LenXDiff.ToString("F4"),
                        round.EndY - round.StartY, round.LenYDiff.ToString("F4"),
                        round.Weight.ToString(), round.WeightDiff.ToString("F4")));
                    file.Write(Environment.NewLine);
                }

                double radiusStdEv = Utils.Math.StdEv(f.Rounds.Select(x => (double)x.MaxLenLine.Length).ToList());
                file.Write(string.Format("StdEv of Radius: {0}", radiusStdEv));
                file.Write(Environment.NewLine);

                double weightStdEv = Utils.Math.StdEv(f.Rounds.Select(x => (double)x.Weight).ToList());
                file.Write(string.Format("StdEv of Weight: {0}", weightStdEv));
                file.Write(Environment.NewLine);
            }
        }
    }
}