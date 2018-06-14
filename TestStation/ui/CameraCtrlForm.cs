using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestStation
{
    public partial class CameraCtrlForm : Form
    {
        public CameraCtrlForm()
        {
            InitializeComponent();
            UC_CameraCtrl.Observer += LoadImg;
        }
        private void LoadImg(object img)
        {
            Bitmap resize = new Bitmap(img as Bitmap, PB_Preview.Width, PB_Preview.Height);
            PB_Preview.Image = resize;
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
