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
        DatabaseSrv _database;
        Camera _camera;

        public FormCameraCtrl()
        {
            InitializeComponent();

            Logger log = new Logger("TestStation");
            log.Debug(string.Format("TestStation(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            _database = DatabaseSrv.GetInstance();

            HardwareSrv.GetInstance().Add(new Camera("M8051"));
            _camera = HardwareSrv.GetInstance().Get("Camera") as Camera;
        }

        private string _filePath = @"./../../Samples/Test-24b.bmp";

        private List<Bitmap> _imgs = new List<Bitmap>();
        private void BTN_Read_Click(object sender, EventArgs e)
        {
            Bitmap img = _camera.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Bmp"} })).Param as Bitmap;
            PB_Preview.Image = img;

            _imgs.Add(img);

            _filePath = @"data/" + "Img-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".bmp";
            img.Save(_filePath, ImageFormat.Bmp);
        }

        private void BTN_Calculate_Click(object sender, EventArgs e)
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

                double radiusStdEv = Utils.Math.StdEv(f.Rounds.Select(x => x.MaxLenLine.Length).ToList());
                file.Write(string.Format("StdEv of Radius: {0}", radiusStdEv));
                file.Write(Environment.NewLine);

                double weightStdEv = Utils.Math.StdEv(f.Rounds.Select(x => x.Weight).ToList());
                file.Write(string.Format("StdEv of Weight: {0}", weightStdEv));
                file.Write(Environment.NewLine);
            }
        }

        private void FormCameraCtrl_FormClosing(object sender, FormClosingEventArgs e)
        {
            _camera.Execute(new Command("Close"));
        }

        private void BTN_Open_Click(object sender, EventArgs e)
        {
            _camera.Execute(new Command("Open"));
        }

        private void BTN_Close_Click(object sender, EventArgs e)
        {
            Close();
        }

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
