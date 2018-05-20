using JbImage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string bmpFile = Preprocess.FormatBmp(TB_Filepath.Text);

            CirclesFinder f = new CirclesFinder(bmpFile);
            TB_OutputPath.Text = Utils.String.FilePostfix(TB_Filepath.Text, "-result").Replace(".jpg",".bmp");

            int width = PB_Result.Width;
            int height = PB_Result.Height;

            Bitmap b = f.Draw(TB_OutputPath.Text);
            b = new Bitmap(b, width, height);
            PB_Result.Image = b;
        }
    }
}
