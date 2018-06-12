using Database;
using Hardware;
using JbImage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Utils;

namespace TestStation
{
    public partial class FormCameraCtrl : Form
    {
        private Logger _log = new Logger(typeof(FormCameraCtrl));
        DatabaseSrv _database;
        Camera _camera;

        public FormCameraCtrl()
        {
            InitializeComponent();

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

            Bitmap img = _camera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Bmp"} })).Param as Bitmap;
            PB_Preview.Image = img;

            _imgs.Add(img);

            _filePath = @"data/" + "Img-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bmp";
            img.Save(_filePath, ImageFormat.Bmp);
        }
        private void ProcessWithCircleFinder()
        {
            //var rawData = _camera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Raw" } })).Param as Bitmap;

            string resultBmp = Utils.String.FilePostfix(_filePath, "-result");
            string resultTxt = resultBmp.Replace(".bmp", ".txt");

            Bitmap bmpFile = ImgProcess.Binarize(_filePath);
            CirclesFinder f = new CirclesFinder(bmpFile);

            int width = PB_Preview.Width;
            int height = PB_Preview.Height;

            Bitmap b = f.Draw(resultBmp);
            b = new Bitmap(bmpFile, width, height);
            PB_Preview.Image = b;

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
        private void ProcessWithEmgu()
        {
            if (_loadedImg == null)
            {
                _loadedImg = _filePath;
            }

            _log.Info("ProcessWithEgmu " + _loadedImg);
            EmguCircleImage image = new EmguCircleImage(_loadedImg);
            PB_Preview.Image = new Bitmap(image.Draw(), PB_Preview.Width, PB_Preview.Height);
            image.Count();
            _log.Info(Utils.String.Flatten(image.StatisticInfo()));
        }
        private void BTN_Calculate_Click(object sender, EventArgs e)
        {
            ProcessWithEmgu();
        }

        private void FormCameraCtrl_FormClosing(object sender, FormClosingEventArgs e)
        {
            _camera?.Execute(new Command("Close"));
        }

        private void BTN_Open_Click(object sender, EventArgs e)
        {
            HardwareSrv.GetInstance().Add(new Camera("M8051"));
            _camera = HardwareSrv.GetInstance().Get("Camera") as Camera;
            _camera.Execute(new Command("Open"));
        }
        string _loadedImg;
        private void BTN_Load_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                _loadedImg = d.FileName;
                PB_Preview.Image = new Bitmap(new Bitmap(_loadedImg), PB_Preview.Width, PB_Preview.Height);
            }
        }
        private void BTN_Close_Click(object sender, EventArgs e)
        {
            Close();
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
                _camera.Execute(new Command("Config",
                    new Dictionary<string, string> { { "RoiOriginX", "0" }, { "RoiOriginY", "0" },
                    { "RoiWidth", "1" }, { "RoiHeight", "1" } }));
            }
        }

        private void BTN_SetBin_Click(object sender, EventArgs e)
        {
            _camera.Execute(new Command("Config",
                new Dictionary<string, string> { { "BinX", "2" }, { "BinY", "2" } }));
        }

        private void CB_Color_CheckedChanged(object sender, EventArgs e)
        {
            _camera.Execute(new Command("Config", 
                new Dictionary<string, string> { { "IsColorOperationEnabled", CB_Color.Checked.ToString() } }));
        }
        #endregion
        #region ROI DRAW
        private Rectangle m_MouseRect = Rectangle.Empty;
        public delegate void SelectRectangel(object sneder, Rectangle e);
        public event SelectRectangel SetRectangel;
        private bool m_MouseIsDown = false;

        private void PB_Preview_MouseDown(object sender, MouseEventArgs e)
        {
            m_MouseIsDown = true;
            Point start = e.Location;
            m_MouseRect = new Rectangle(start.X, start.Y, 0, 0);
        }

        private void PB_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_MouseIsDown)
                ResizeToRectangle(e.Location);
        }

        private void PB_Preview_MouseUp(object sender, MouseEventArgs e)
        {
            m_MouseIsDown = false;
            DrawRectangle();
            if (m_MouseRect.X == 0 || m_MouseRect.Y == 0 || m_MouseRect.Width == 0 || m_MouseRect.Height == 0)
            {
                //如果区域没0 就不执行委托 
            }
            else
            {
                if (SetRectangel != null) SetRectangel(PB_Preview, m_MouseRect);
            }
            DrawRectangle();
        }

        /// <summary> 
        /// 刷新绘制 
        /// </summary> 
        /// <param name="p"></param> 
        private void ResizeToRectangle(Point p_Point)
        {
            DrawRectangle();
            m_MouseRect.Width = p_Point.X - m_MouseRect.Left;
            m_MouseRect.Height = p_Point.Y - m_MouseRect.Top;
            DrawRectangle();
        }

        /// <summary> 
        /// 绘制区域 
        /// </summary> 
        private void DrawRectangle()
        {
            Rectangle _Rect = m_MouseRect;
            ControlPaint.DrawReversibleFrame(_Rect, Color.White, FrameStyle.Dashed);
        }
        #endregion
    }
}
