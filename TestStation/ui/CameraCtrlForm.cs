using JbImage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestStation.core;
using TestStation.ui;
using Utils;

namespace TestStation
{
    public partial class CameraCtrlForm : Form
    {
        private Logger _log = new Logger("CameraCtrlForm");
        public CameraCtrlForm()
        {
            InitializeComponent();

            UC_CameraCtrl.UpdateImage += LoadImg;
            UC_CameraCtrl.TypeChanged += UC_Result.SetType;
            UC_CameraCtrl.UpdateResult += UC_Result.Update;
            SetRectangle = SetRoi;
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
        private void CameraCtrlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (UC_CameraCtrl.Device.mCamera != null)
            {
                UC_CameraCtrl.Device.Close();
            }

            MotorCtrlUC.OnClose();
        }
        private void UpdateImg(Bitmap img)
        {
            UC_CameraCtrl.UpdateImage?.Invoke(img);
            UC_CameraCtrl.Refresh();
            Application.DoEvents();
        }
        private void BTN_StartTest_Click(object sender, EventArgs e)
        {
            BTN_StartTest.Enabled = false;

            /*parameter initialize*/
            List<double> z2Positions;
            try
            {
                z2Positions = Properties.Settings.Default.MotorZ2Positions.Split(',').ToList().Select(x => double.Parse(x)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to retrieve test positions, please check the configuration file");
                BTN_StartTest.Enabled = true;
                return;
            }

            /*device initialize*/
            CameraController camera = UC_CameraCtrl.Device;
            Result ret = camera.Open(UC_CameraCtrl.TestType);
            if (ret.Id != "Ok" && ret.Id != "Dummy")
            {
                MessageBox.Show($"Failed to open camera for {UC_CameraCtrl.TestType}({ret.Id})");
                //return;
            }

            MotorController motor = MotorCtrlUC.Device;
            if (motor == null)
            {
                MessageBox.Show($"Failed to open motor for {UC_CameraCtrl.TestType}");
            }

            for (int i = 0; i<z2Positions.Count; i++)
            {
                double offset = 0;
                if (i == 0)
                {
                    offset = z2Positions[0];
                }
                else
                {
                    offset = z2Positions[i] - z2Positions[i - 1];
                }
                motor.MoveZ2(offset);

                Thread.Sleep(2000);

                ret = camera.Read(z2Positions[i].ToString());
                UpdateImg(ret.Param as Bitmap);

                ret = camera.Analyze(UC_CameraCtrl.TestType, z2Positions[i]);
                UpdateImg(ret.Param as Bitmap);

                if (camera.Imgs.Last().Circles.Count == 0)
                {
                    MessageBox.Show("Failed to find any circles in recent images");
                    break;
                }

                Thread.Sleep(1000);
            }

            ret = camera.Calculate(UC_CameraCtrl.TestType);
            UC_Result.Update(ret.Param as Dictionary<string, string>);

            BTN_StartTest.Enabled = true;
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
    }
}
