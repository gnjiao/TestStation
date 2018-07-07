using System;
using System.Configuration;
using System.Windows.Forms;
using TestStation.core;

namespace TestStation.ui
{
    public partial class MotorCtrlUC : UserControl
    {
        public static DS102 Device;
        public MotorCtrlUC()
        {
            InitializeComponent();

            Device = new DS102();
            try
            {
                Device.Port = Int32.Parse(ConfigurationManager.AppSettings["DS102Port"]);
                if (!Device.OpenDS102())
                {
                    MessageBox.Show($"Failed to open DS102, please make sure the COM{Device.Port} is available");
                    Device = null;
                }
            }
            catch (Exception)
            {
                Device = null;
            }
        }
        private void BTN_Z1MoveUp_Click(object sender, EventArgs e)
        {
            Device?.ZAxisGoPositive(DS102.AXIS_Z1, Z1Distance);
            Z1Position += Z1Distance;
            LB_Z1Position.Text = $"{Z1Position:F2} mm";
        }
        private void BTN_Z1MoveDown_Click(object sender, EventArgs e)
        {
            Device?.ZAxisGoNegative(DS102.AXIS_Z1, Z1Distance);
            Z1Position -= Z1Distance;
            LB_Z1Position.Text = $"{Z1Position:F2} mm";
        }
        private void BTN_Z2MoveUp_Click(object sender, EventArgs e)
        {
            Device?.ZAxisGoNegative(DS102.AXIS_Z2, Z2Distance);
            Z2Position -= Z2Distance;
            LB_Z2Position.Text = $"{Z2Position:F2} mm";
        }
        private void BTN_Z2MoveDown_Click(object sender, EventArgs e)
        {
            Device?.ZAxisGoPositive(DS102.AXIS_Z2, Z2Distance);
            Z2Position += Z2Distance;
            LB_Z2Position.Text = $"{Z2Position:F2} mm";
        }
        private void BTN_Z1GoHome_Click(object sender, EventArgs e)
        {
            BTN_Z1GoHome.Enabled = false;
            BTN_Z1GoHome.Refresh();

            Device?.GoOrigin(DS102.AXIS_Z1);
            LB_Z1Position.Text = $"0 mm";

            BTN_Z1GoHome.Enabled = true;
        }
        private void BTN_Z2GoHome_Click(object sender, EventArgs e)
        {
            BTN_Z2GoHome.Enabled = false;
            BTN_Z2GoHome.Refresh();

            Device?.GoOrigin(DS102.AXIS_Z2);
            LB_Z2Position.Text = $"0 mm";

            BTN_Z2GoHome.Enabled = true;
        }
        public static void OnClose()
        {
            Device?.CloseDS102();
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
