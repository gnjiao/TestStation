using System;
using System.Configuration;
using System.Windows.Forms;
using TestStation.core;

namespace TestStation.ui
{
    public partial class MotorCtrlUC : UserControl
    {
        private static DS102 ds102;
        public MotorCtrlUC()
        {
            InitializeComponent();

            ds102 = new DS102();
            try
            {
                ds102.Port = Int32.Parse(ConfigurationManager.AppSettings["DS102Port"]);
                if (!ds102.OpenDS102())
                {
                    ds102 = null;
                }
            }
            catch (Exception)
            {
                ds102 = null;
            }
        }
        private void BTN_Z1MoveUp_Click(object sender, EventArgs e)
        {
            ds102?.ZAxisGoPositive(DS102.AXIS_Z1, Z1Distance);
            Z1Position += Z1Distance;
            LB_Z1Position.Text = $"{Z1Position:F2} mm";
        }
        private void BTN_Z1MoveDown_Click(object sender, EventArgs e)
        {
            ds102?.ZAxisGoNegative(DS102.AXIS_Z1, Z1Distance);
            Z1Position -= Z1Distance;
            LB_Z1Position.Text = $"{Z1Position:F2} mm";
        }
        private void BTN_Z2MoveUp_Click(object sender, EventArgs e)
        {
            ds102?.ZAxisGoNegative(DS102.AXIS_Z2, Z2Distance);
            Z2Position -= Z2Distance;
            LB_Z2Position.Text = $"{Z2Position:F2} mm";
        }
        private void BTN_Z2MoveDown_Click(object sender, EventArgs e)
        {
            ds102?.ZAxisGoPositive(DS102.AXIS_Z2, Z2Distance);
            Z2Position += Z2Distance;
            LB_Z2Position.Text = $"{Z2Position:F2} mm";
        }
        private void BTN_Z1GoHome_Click(object sender, EventArgs e)
        {
            BTN_Z1GoHome.Enabled = false;
            BTN_Z1GoHome.Refresh();

            ds102?.GoOrigin(DS102.AXIS_Z1);
            LB_Z1Position.Text = $"0 mm";

            BTN_Z1GoHome.Enabled = true;
        }
        private void BTN_Z2GoHome_Click(object sender, EventArgs e)
        {
            BTN_Z2GoHome.Enabled = false;
            BTN_Z2GoHome.Refresh();

            ds102?.GoOrigin(DS102.AXIS_Z2);
            LB_Z2Position.Text = $"0 mm";

            BTN_Z2GoHome.Enabled = true;
        }
        public static void OnClose()
        {
            ds102?.CloseDS102();
        }
        private double Z1Distance
        {
            get
            {
                double value = double.NaN;
                if (double.TryParse(TB_Z1Distance.Text, out value))
                {
                    return value;
                }
                else
                {
                    return 0;
                }
            }
        }
        private double Z2Distance
        {
            get
            {
                double value = double.NaN;
                if (double.TryParse(TB_Z2Distance.Text, out value))
                {
                    return value;
                }
                else
                {
                    return 0;
                }
            }
        }
        private double Z1Position;
        private double Z2Position;
    }
}
