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
using TestStation.ui;

namespace TestStation
{
    public partial class CameraCtrlForm : Form
    {
        public CameraCtrlForm()
        {
            InitializeComponent();

            LoadEmguParameters();

            UC_CameraCtrl.UpdateImage += LoadImg;
            UC_CameraCtrl.TypeChanged += UC_Result.SetType;
            UC_CameraCtrl.UpdateResult += UC_Result.Update;
            SetRectangle = SetRoi;
        }
        private void LoadEmguParameters()
        {
            EmguParameters.Item["UseCanny"] = Properties.Settings.Default.UseCanny;
            EmguParameters.Item["SaveFile"] = Properties.Settings.Default.SaveFile;
            EmguParameters.Item["ShowFirstResult"] = Properties.Settings.Default.ShowFirstResult;

            EmguParameters.Item["BinThreshold"] = Properties.Settings.Default.BinThreshold;

            EmguParameters.Item["Canny1Threshold1"] = Properties.Settings.Default.Canny1Threshold1;
            EmguParameters.Item["Canny1Threshold2"] = Properties.Settings.Default.Canny1Threshold2;
            EmguParameters.Item["Canny1ApertureSize"] = Properties.Settings.Default.Canny1ApertureSize;
            EmguParameters.Item["Canny1I2Gradient"] = Properties.Settings.Default.Canny1I2Gradient;

            EmguParameters.Item["Hough1Dp"] = Properties.Settings.Default.Hough1Dp;
            EmguParameters.Item["Hough1MinDist"] = Properties.Settings.Default.Hough1MinDist;
            EmguParameters.Item["Hough1Param1"] = Properties.Settings.Default.Hough1Param1;
            EmguParameters.Item["Hough1Param2"] = Properties.Settings.Default.Hough1Param2;
            EmguParameters.Item["Hough1MinRadius"] = Properties.Settings.Default.Hough1MinRadius;
            EmguParameters.Item["Hough1MaxRadius"] = Properties.Settings.Default.Hough1MaxRadius;

            EmguParameters.Item["Canny2Threshold1"] = Properties.Settings.Default.Canny2Threshold1;
            EmguParameters.Item["Canny2Threshold2"] = Properties.Settings.Default.Canny2Threshold2;
            EmguParameters.Item["Canny2ApertureSize"] = Properties.Settings.Default.Canny2ApertureSize;
            EmguParameters.Item["Canny2I2Gradient"] = Properties.Settings.Default.Canny2I2Gradient;

            EmguParameters.Item["Hough2Dp"] = Properties.Settings.Default.Hough2Dp;
            EmguParameters.Item["Hough2MinDist"] = Properties.Settings.Default.Hough2MinDist;
            EmguParameters.Item["Hough2Param1"] = Properties.Settings.Default.Hough2Param1;
            EmguParameters.Item["Hough2Param2"] = Properties.Settings.Default.Hough2Param2;
            EmguParameters.Item["Hough2MinRadius"] = Properties.Settings.Default.Hough2MinRadius;
            EmguParameters.Item["Hough2MaxRadius"] = Properties.Settings.Default.Hough2MaxRadius;
        }
        private void LoadImg(object img)
        {
            if (img != null)
            {
                Bitmap resize = new Bitmap(img as Bitmap, PB_Preview.Width, PB_Preview.Height);
                PB_Preview.Image = resize;
            }
        }
        private void SetRoi(object sender, Rectangle rect)
        {
            PictureBox pb = sender as PictureBox;
            Point start = pb.PointToScreen(pb.Location);

            double xoffset = (rect.Location.X > start.X) ? (double)(rect.Location.X - start.X) / pb.Width : 0;
            double yoffset = (rect.Location.Y > start.Y) ? (double)(rect.Location.Y - start.Y) / pb.Height : 0;
            //UC_CameraCtrl.SetRoi(xoffset, yoffset, (double)rect.Width/pb.Width, (double)rect.Height/pb.Height);
        }
        #region ROI DRAW
        private Rectangle m_MouseRect = Rectangle.Empty;
        public delegate void SelectRectangle(object sender, Rectangle e);
        public event SelectRectangle SetRectangle;
        private bool m_MouseIsDown = false;

        private void PB_Preview_MouseDown(object sender, MouseEventArgs e)
        {
            m_MouseIsDown = true;
            Point start = (sender as PictureBox).PointToScreen(e.Location);
            m_MouseRect = new Rectangle(start.X, start.Y, 0, 0);
        }
        private void PB_Preview_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_MouseIsDown)
            {
                Point end = (sender as PictureBox).PointToScreen(e.Location);
                ResizeToRectangle((sender as PictureBox).PointToScreen(e.Location));
            }
        }
        private void PB_Preview_MouseUp(object sender, MouseEventArgs e)
        {
            m_MouseIsDown = false;
            Point end = (sender as PictureBox).PointToScreen(e.Location);
            DrawRectangle();
            if (m_MouseRect.X == 0 || m_MouseRect.Y == 0 || m_MouseRect.Width == 0 || m_MouseRect.Height == 0)
            {
            }
            else
            {
                SetRectangle?.Invoke(PB_Preview, m_MouseRect);
            }
            DrawRectangle();
        }
        private void ResizeToRectangle(Point p_Point)
        {
            DrawRectangle();
            m_MouseRect.Width = p_Point.X - m_MouseRect.Left;
            m_MouseRect.Height = p_Point.Y - m_MouseRect.Top;
            DrawRectangle();
        }
        private void DrawRectangle()
        {
            ControlPaint.DrawReversibleFrame(m_MouseRect, Color.White, FrameStyle.Dashed);
        }
        #endregion

        private void CameraCtrlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MotorCtrlUC.OnClose();
        }
    }
}
