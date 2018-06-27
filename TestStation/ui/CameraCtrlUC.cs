
#define REAL_CAMERA

using Database;
using Hardware;
using JbImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Utils;
using System.Configuration;

namespace TestStation
{
    public delegate void UpdateImage(object param);

    public partial class CameraCtrlUC : UserControl
    {
        private string _filePath = @"./../../Samples/Img_6_20180627-222513.bmp";

        public UpdateImage Observer;

        private Logger _log = new Logger(typeof(CameraCtrlUC));
        DatabaseSrv _database;
        public Camera _camera;
        public CameraCtrlUC()
        {
            InitializeComponent();
            InitializeHelpInfo();
            CMB_CameraType.SelectedIndex = 0;

            Logger log = new Logger("TestStation");
            log.Debug(string.Format("TestStation(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            _database = DatabaseSrv.GetInstance();
        }

        private List<double> _distances = new List<double>();
        private List<EmguCircleImage> _imgs = new List<EmguCircleImage>();

        private void BTN_Read_Click(object sender, EventArgs e)
        {
            Result ret;
#if REAL_CAMERA
            if (_camera == null)
            {
                ret = OpenCamera(CMB_CameraType.Text);
                if (ret.Id != "Ok")
                {
                    MessageBox.Show(ret.Desc);
                    return;
                }
            }
            Bitmap img = _camera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Bmp" } })).Param as Bitmap;
#else
            Bitmap img = new Bitmap(10, 10);
#endif

            double distance = ReadDistance(TB_Distance);
            _distances.Add(distance);

            _filePath = @"data/" + $"Img_{distance}_{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.bmp";
            img.Save(_filePath, ImageFormat.Bmp);

            Observer?.Invoke(img);
        }
        private void BTN_ImgAnalyze_Click(object sender, EventArgs e)
        {
            ProcessWithEmgu((_loadedImg==null) ? _filePath : _loadedImg);
        }
        private void BTN_Calculate_Click(object sender, EventArgs e)
        {
            if (_imgs.Count == _distances.Count)
            {
                double[] result = new double[_imgs[0].Circles.Length];
                for (int circleId = 0; circleId < result.Length; circleId++)
                {
                    double[] radius = new double[_imgs.Count];
                    for (int imgId = 0; imgId < radius.Length; imgId++)
                    {
                        radius[imgId] = _imgs[imgId].Circles[circleId].Radius;
                    }

                    result[circleId] = Matlab.CalcWeist(_distances.ToArray(), radius);
                    _log.Info($"{circleId}: {result[circleId]}" + Environment.NewLine);
                }
            }
            else
            {
                MessageBox.Show("Calculation can only apply to images with distance parameter, please re-do the test");
            }

            _distances.Clear();
            _imgs.Clear();
        }

        private void BTN_Open_Click(object sender, EventArgs e)
        {
            Result ret = OpenCamera(CMB_CameraType.Text);
            if (ret.Id != "Ok")
            {
                MessageBox.Show(ret.Desc);
            }
        }
        string _loadedImg;
        private void BTN_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                _loadedImg = d.FileName;
                Observer?.Invoke(new Bitmap(_loadedImg));
            }
        }
        private void BTN_Close_Click(object sender, EventArgs e)
        {
            _camera?.Execute(new Command("Close"));
            _camera = null;
        }
        private Result OpenCamera(string type)
        {
            switch (type)
            {
                case "Type A":
                    HardwareSrv.GetInstance().Add(new M8051("Camera"));
                    break;
                case "Type B":
                    HardwareSrv.GetInstance().Add(new Vcxu("Camera"));
                    break;
                default:
                    return new Result("Fail", "Unknown camera type");
            }

            _camera = HardwareSrv.GetInstance().Get("Camera") as Camera;

            try
            {
                return _camera.Execute(new Command("Open"));
            }
            catch (Exception ex)
            {
                _log.Error("Failed to open the camera", ex);
                return new Result("Fail", ex.ToString());
            }
        }
        private int[] RadiusLimits()
        {
            int[] ret = new int[2];
            
            double value = ReadDistance(TB_Distance);
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
        private void ProcessWithEmgu(string img)
        {
            _log.Info("ProcessWithEgmu " + img);
            EmguCircleImage image = new EmguCircleImage(img, RadiusLimits());
            _imgs.Add(image);

            image.Count(double.Parse(ConfigurationManager.AppSettings["CountThreshold"]));
            Dictionary<string, string> statInfo = image.StatisticInfo();
            _log.Info(Utils.String.Flatten(statInfo));

            LB_CircleInfo.Text = "";
            foreach (var k in statInfo.Keys)
            {
                LB_CircleInfo.Text += $"{k}: " + Environment.NewLine;
                LB_CircleInfo.Text += $"    { statInfo[k]}" + Environment.NewLine;
            }

            Observer?.Invoke(image.Draw());
        }
        #region camera configuration
        private bool _roiRectDraw = false;
        private void CB_SetRoi_CheckedChanged(object sender, EventArgs e)
        {
            if (!_roiRectDraw)
            {
                _roiRectDraw = true;
            }
            else
            {
                _camera?.Execute(new Command("Config",
                    new Dictionary<string, string> { { "RoiOriginX", "0" }, { "RoiOriginY", "0" },
                    { "RoiWidth", "1" }, { "RoiHeight", "1" } }));
            }
        }

        private void BTN_SetBin_Click(object sender, EventArgs e)
        {
            _camera?.Execute(new Command("Config",
                new Dictionary<string, string> { { "BinX", "2" }, { "BinY", "2" } }));
        }

        private void CB_Color_CheckedChanged(object sender, EventArgs e)
        {
            _camera?.Execute(new Command("Config", 
                new Dictionary<string, string> { { "IsColorOperationEnabled", CB_Color.Checked.ToString() } }));
        }
        public void SetRoi(double xoffset, double yoffset, double width, double height)/* all parameters are determined via percentage */
        {
            _camera?.Execute(new Command("Config",
                new Dictionary<string, string> { { "RoiOriginX", xoffset.ToString("F2") }, { "RoiOriginY", yoffset.ToString("F2") },
                    { "RoiWidth", width.ToString("F2") }, { "RoiHeight", height.ToString("F2") } }));
        }
        #endregion
        private void InitializeHelpInfo()
        {
            toolTip1.SetToolTip(BTN_Open, "Initialize the camera");
            toolTip1.SetToolTip(BTN_Read, "Capture an image via the camera, and queue this image into buffer for calculation");
            toolTip1.SetToolTip(BTN_Load, "Load an exsiting image, and queue this image into buffer for calculation");
            toolTip1.SetToolTip(BTN_ImgAnalyze, "Analyze the latest read or loaded image(Recognize circles and count pixels)");
            toolTip1.SetToolTip(BTN_Calculate, "Calculate weist based on the read or loaded images, the images queue will be cleared after this action");
            toolTip1.SetToolTip(BTN_Close, "Close the camera");

            toolTip1.SetToolTip(CB_Color, "Camera color mode: color or monochrome");
            toolTip1.SetToolTip(CB_SetRoi, "Camera range of interesting");
            toolTip1.SetToolTip(BTN_SetBin, "Camera sample rate");
            toolTip1.SetToolTip(BTN_ResetRoi, "Reset camera configuration");
        }
        private void TB_Distance_Click(object sender, EventArgs e)
        {
            TB_Distance.Text = "";
        }
        private void TB_Distance_Leave(object sender, EventArgs e)
        {
            double value = double.NaN;
            if (!double.TryParse(TB_Distance.Text, out value))
            {
                TB_Distance.Text = "Distance(mm)";
            }
        }
        private double ReadDistance(TextBox tb)
        {
            double value = double.NaN;

            if (double.TryParse(tb.Text, out value))
            {
                return value;
            }

            return double.NaN;
        }
        /* to be obsoleted */
        private void ProcessWithCircleFinder()
        {
            //var rawData = _camera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Raw" } })).Param as Bitmap;

            string resultBmp = Utils.String.FilePostfix(_filePath, "-result");
            string resultTxt = resultBmp.Replace(".bmp", ".txt");

            Bitmap bmpFile = ImgProcess.Binarize(_filePath);
            CirclesFinder f = new CirclesFinder(bmpFile);

            Observer?.Invoke(f.Draw(resultBmp));

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
