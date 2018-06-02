using JbImage;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ImageTool
{
    public partial class FormImgTool : Form
    {
        public FormImgTool()
        {
            InitializeComponent();
        }

        private void BTN_Open_Click(object sender, EventArgs e)
        {
            /* only jpg format is supported now */
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                TB_OutputPath.Text = "";
                TB_Filepath.Text = d.FileName;
            }
        }

        private void BTN_Process_Click(object sender, EventArgs e)
        {
            string bmpFile = ImgProcess.FormatBmp(TB_Filepath.Text);

            CirclesFinder f = new CirclesFinder(bmpFile);
            TB_OutputPath.Text = Utils.String.FilePostfix(TB_Filepath.Text, "-result").Replace(".jpg",".bmp");

            int width = PB_Result.Width;
            int height = PB_Result.Height;

            Bitmap b = f.Draw(TB_OutputPath.Text);
            b = new Bitmap(b, width, height);
            PB_Result.Image = b;

            ImgProcess.Count(f.Rounds);
            string txtFile = TB_OutputPath.Text.Replace(".bmp", ".txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(txtFile, true))
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
    }
}
