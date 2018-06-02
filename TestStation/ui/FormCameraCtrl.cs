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
                file.Write(string.Format("{0} {1} {2} {3} {4} {5} {6}",
                    "ID",
                    "LengthOnX", "DeviationOnX",
                    "LengthOnY", "DeviationOnY",
                    "Weight", "DeviationOnWeight") + Environment.NewLine);

                foreach (var round in f.Rounds)
                {
                    file.Write(string.Format("{0} {1} {2} {3} {4} {5} {6}",
                        round.Id.ToString("D3"),
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
    }
}
