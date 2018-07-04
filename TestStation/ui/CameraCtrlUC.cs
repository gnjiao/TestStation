﻿
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
using TestStation.core;
using Timer = System.Timers.Timer;
using TestStation.ui;

namespace TestStation
{
    public delegate void EventHandler(object param);

    public partial class CameraCtrlUC : UserControl
    {
        private string _filePath = @"./../../Samples/Img_6_20180627-222513.bmp";

        public EventHandler UpdateImage;
        public EventHandler TypeChanged;
        public EventHandler UpdateResult;

        private Timer _updateTimer;
        private object _updateLock = new object();

        private Logger _log = new Logger(typeof(CameraCtrlUC));
        DatabaseSrv _database;
        private CameraController _cameraCtrl;
        public CameraCtrlUC()
        {
            InitializeComponent();
            InitializeHelpInfo();
            CMB_CameraType.SelectedIndex = 0;

            Logger log = new Logger("TestStation");
            log.Debug(string.Format("TestStation(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            _cameraCtrl = new CameraController();
            _database = DatabaseSrv.GetInstance();

            _updateTimer = new Timer(200);
            _updateTimer.Elapsed += RefreshImage;
            //_updateTimer.Start();
        }
        private void RefreshImage(object sender, EventArgs e)
        {
            lock (_updateLock)
            {
                _updateTimer.Stop();

                Bitmap image = null;

#if false
                image = dbgRefreshedImage();
#else
                if (_cameraCtrl.mCamera != null)
                {
                    Result ret = _cameraCtrl.mCamera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Bmp" } }));
                    if (ret.Id == "Ok")
                    {
                        image = ret.Param as Bitmap;
                    }
                }
#endif
                if (image != null)
                {
                    UpdateImage?.Invoke(image);
                }

                _updateTimer.Start();
            }
        }
        private void BTN_Read_Click(object sender, EventArgs e)
        {
            lock (_updateLock)
            {
                _updateTimer.Stop();

                _cameraCtrl.Read(Distance).ShowMessageBox();
                UpdateImage?.Invoke(_cameraCtrl.LatestImage);

                _updateTimer.Start();
            }
        }
        private void BTN_ImgAnalyze_Click(object sender, EventArgs e)
        {
            if (BTN_ImgAnalyze.Text == "Image Analyze")
            {
                if (_updateTimer.Enabled)
                {
                    lock (_updateLock)
                    {
                        _updateTimer.Stop();
                        BTN_ImgAnalyze.Text = "Resume";
                    }
                }

                if (!double.IsNaN(DbgMinRadius) && !double.IsNaN(DbgMaxRadius))
                {
                    _cameraCtrl.Analyze(new int[] { (int)DbgMinRadius, (int)DbgMaxRadius }).ShowMessageBox();
                }
                else
                {
                    _cameraCtrl.Analyze(Distance).ShowMessageBox();
                }
                UpdateImage?.Invoke(_cameraCtrl.AnalyzedImage);
            }
            else
            {
                BTN_ImgAnalyze.Text = "Image Analyze";
                _updateTimer.Start();
            }
        }
        private void BTN_Calculate_Click(object sender, EventArgs e)
        {
#if DEBUG
            //dbgAutoLoad();
#endif
            _cameraCtrl.Calculate().ShowMessageBox();
            UpdateResult?.Invoke(new Dictionary<string, string>() {
                { "TestKey1", "TestValue1" }, { "TestKey2", "TestValue2" }
            });
        }
        private void BTN_Open_Click(object sender, EventArgs e)
        {
            _cameraCtrl.Open(CMB_CameraType.Text).ShowMessageBox();
            if (_cameraCtrl.mCamera != null)
            {
                _updateTimer.Start();
            }
        }
        private void BTN_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                lock (_updateLock)
                {
                    _cameraCtrl.Load(d.FileName, Distance).ShowMessageBox();
                    UpdateImage?.Invoke(_cameraCtrl.LatestImage);
                }
            }
        }
        private void BTN_Close_Click(object sender, EventArgs e)
        {
            _cameraCtrl.Close().ShowMessageBox();
        }
        private void InitializeHelpInfo()
        {
            toolTip1.SetToolTip(BTN_Open, "Initialize the camera");
            toolTip1.SetToolTip(BTN_Read, "Capture an image via the camera, and queue this image into buffer for calculation");
            toolTip1.SetToolTip(BTN_Load, "Load an exsiting image, and queue this image into buffer for calculation");
            toolTip1.SetToolTip(BTN_ImgAnalyze, "Analyze the latest read or loaded image(Recognize circles and count pixels)");
            toolTip1.SetToolTip(BTN_Calculate, "Calculate weist based on the read or loaded images, the images queue will be cleared after this action");
            toolTip1.SetToolTip(BTN_Close, "Close the camera");

            //toolTip1.SetToolTip(CB_Color, "Camera color mode: color or monochrome");
            //toolTip1.SetToolTip(CB_SetRoi, "Camera range of interesting");
            //toolTip1.SetToolTip(BTN_SetBin, "Camera sample rate");
            //toolTip1.SetToolTip(BTN_ResetRoi, "Reset camera configuration");
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
        private double Distance
        {
            get
            {
                return ReadDouble(TB_Distance);
            }
        }
        private double DbgMinRadius
        {
            get
            {
                return ReadDouble(TB_DbgMinRadius);
            }
        }
        private double DbgMaxRadius
        {
            get
            {
                return ReadDouble(TB_DbgMaxRadius);
            }
        }
        private double ReadDouble(TextBox tb)
        {
            double value = double.NaN;
            if (double.TryParse(tb.Text, out value))
            {
                return value;
            }
            else
            {
                return double.NaN;
            }
        }
        /* debug suppport */
        private void dbgAutoLoad()
        {
            string[] files = new string[] {
                @"./../../Samples/Img_2_20180627-222551.bmp",
                @"./../../Samples/Img_4_20180627-222537.bmp",
                @"./../../Samples/Img_6_20180627-222513.bmp",
            };

            int i = 2;
            foreach (var file in files)
            {
                TB_Distance.Text = i.ToString();
                i += 2;

                _cameraCtrl.Load(file, Distance);
                _cameraCtrl.Analyze(Distance);
            }
        }
        private int dbgImageCount = 0;
        private Bitmap dbgRefreshedImage()
        {
            string[] files = new string[] {
                @"./../../Samples/Img_2_20180627-222551.bmp",
                @"./../../Samples/Img_4_20180627-222537.bmp",
                @"./../../Samples/Img_6_20180627-222513.bmp",
            };
            dbgImageCount++;

            return new Bitmap(files[dbgImageCount % 3]);
        }
        /* to be obsoleted */
        #region camera configuration
        //private bool _roiRectDraw = false;
        //private void CB_SetRoi_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (!_roiRectDraw)
        //    {
        //        _roiRectDraw = true;
        //    }
        //    else
        //    {
        //        _cameraCtrl.mCamera.Execute(new Command("Config",
        //            new Dictionary<string, string> { { "RoiOriginX", "0" }, { "RoiOriginY", "0" },
        //            { "RoiWidth", "1" }, { "RoiHeight", "1" } }));
        //    }
        //}
        //private void BTN_SetBin_Click(object sender, EventArgs e)
        //{
        //    _cameraCtrl.mCamera.Execute(new Command("Config",
        //        new Dictionary<string, string> { { "BinX", "2" }, { "BinY", "2" } }));
        //}
        //private void CB_Color_CheckedChanged(object sender, EventArgs e)
        //{
        //    _cameraCtrl.mCamera.Execute(new Command("Config",
        //        new Dictionary<string, string> { { "IsColorOperationEnabled", CB_Color.Checked.ToString() } }));
        //}
        //public void SetRoi(double xoffset, double yoffset, double width, double height)/* all parameters are determined via percentage */
        //{
        //    _cameraCtrl.mCamera.Execute(new Command("Config",
        //        new Dictionary<string, string> { { "RoiOriginX", xoffset.ToString("F2") }, { "RoiOriginY", yoffset.ToString("F2") },
        //            { "RoiWidth", width.ToString("F2") }, { "RoiHeight", height.ToString("F2") } }));
        //}
        #endregion
        private void ProcessWithCircleFinder()
        {
            //var rawData = _camera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Raw" } })).Param as Bitmap;

            string resultBmp = Utils.String.FilePostfix(_filePath, "-result");
            string resultTxt = resultBmp.Replace(".bmp", ".txt");

            Bitmap bmpFile = ImgProcess.Binarize(_filePath);
            CirclesFinder f = new CirclesFinder(bmpFile);

            UpdateImage?.Invoke(f.Draw(resultBmp));

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

        private void CMB_CameraType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CMB_CameraType.Text == "NFT" || CMB_CameraType.Text == "FFT")
            {
                TypeChanged?.Invoke(CMB_CameraType.Text);
            }
        }

        private void BTN_SetCamera_Click(object sender, EventArgs e)
        {
            double delay = ReadDouble(TB_DelayMs);
            if (double.IsNaN(delay))
            {
                _cameraCtrl.SetDelay((int)delay);
            }

            double exposure = ReadDouble(TB_DelayMs);
            if (double.IsNaN(exposure))
            {
                _cameraCtrl.SetExposure((int)exposure);
            }
        }

        private void TB_Parameters_Click(object sender, EventArgs e)
        {
            EmguParameterForm form = new EmguParameterForm();
            form.Show();
        }
    }
}
