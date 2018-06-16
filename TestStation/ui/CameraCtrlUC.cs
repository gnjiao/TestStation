using Database;
using Hardware;
using JbImage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace TestStation
{
    public delegate void UpdateImage(object param);

    public partial class CameraCtrlUC : UserControl
    {
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

        private string _filePath = @"./../../Samples/Test-24b.bmp";

        private List<Bitmap> _imgs = new List<Bitmap>();
        private void BTN_Read_Click(object sender, EventArgs e)
        {
            if (_camera == null)
            {
                BTN_Open_Click(sender, e);
            }

            Bitmap img = _camera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Bmp" } })).Param as Bitmap;

            _imgs.Add(img);
            _filePath = @"data/" + "Img-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bmp";
            img.Save(_filePath, ImageFormat.Bmp);
            Observer?.Invoke(img);
        }
        private void ProcessWithEmgu()
        {
            if (_loadedImg == null)
            {
                _loadedImg = _filePath;
            }

            _log.Info("ProcessWithEgmu " + _loadedImg);
            EmguCircleImage image = new EmguCircleImage(_loadedImg);
            image.Count();
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
        private void BTN_Calculate_Click(object sender, EventArgs e)
        {
            ProcessWithEmgu();
        }

        private void BTN_Open_Click(object sender, EventArgs e)
        {
            switch (CMB_CameraType.Text)
            {
                case "Type A":
                    HardwareSrv.GetInstance().Add(new M8051("Camera"));
                    break;
                case "Type B":
                    HardwareSrv.GetInstance().Add(new Vcxu("Camera"));
                    break;
                default:
                    MessageBox.Show("Please choose a valid camera type");
                    return;
            }
            _camera = HardwareSrv.GetInstance().Get("Camera") as Camera;

            try
            {
                _camera.Execute(new Command("Open"));
            }
            catch (Exception ex)
            {
                _log.Error("Failed to open the camera", ex);
                MessageBox.Show("Failed to open the camera");
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
            toolTip1.SetToolTip(BTN_Read, "Capture an image via the camera");
            toolTip1.SetToolTip(BTN_Load, "Load an exsiting image for calculation");
            toolTip1.SetToolTip(BTN_Calculate, "Calculate the read or loaded image");
            toolTip1.SetToolTip(BTN_Close, "Close the camera");

            toolTip1.SetToolTip(CB_Color, "Camera color mode: color or monochrome");
            toolTip1.SetToolTip(CB_SetRoi, "Camera range of interesting");
            toolTip1.SetToolTip(BTN_SetBin, "Camera sample rate");
            toolTip1.SetToolTip(BTN_ResetRoi, "Reset camera configuration");
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
