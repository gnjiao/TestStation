using System;
using System.Configuration;
using System.Windows.Forms;
using TestStation.core;
using Utils;

namespace TestStation.ui
{
    public partial class MotorCtrlUC : UserControl
    {
        public static MotorController Device;
        public MotorCtrlUC()
        {
            InitializeComponent();
            Device = new MotorController(ConfigurationManager.AppSettings["DS102Port"]);
            Device.Observer = UpdatePosition;
        }
        private void UpdatePosition(string z, double value)
        {
            switch (z)
            {
                case "Z1":
                    LB_Z1Position.Text = $"{value:F2} mm";
                    break;
                case "Z2":
                    LB_Z2Position.Text = $"{value:F2} mm";
                    break;
            }

            Refresh();
        }
        private void BTN_Z1MoveUp_Click(object sender, EventArgs e)
        {
            Device?.MoveZ1(Z1Distance);
        }
        private void BTN_Z1MoveDown_Click(object sender, EventArgs e)
        {
            Device?.MoveZ1(0 - Z1Distance);
        }
        private void BTN_Z2MoveUp_Click(object sender, EventArgs e)
        {
            Device?.MoveZ2(0 - Z2Distance);
        }
        private void BTN_Z2MoveDown_Click(object sender, EventArgs e)
        {
            Device?.MoveZ2(Z2Distance);
        }
        private void BTN_Z1GoHome_Click(object sender, EventArgs e)
        {
            BTN_Z1GoHome.Enabled = false;
            BTN_Z1GoHome.Refresh();

            Device?.ResetZ1();

            BTN_Z1GoHome.Enabled = true;
        }
        private void BTN_Z2GoHome_Click(object sender, EventArgs e)
        {
            BTN_Z2GoHome.Enabled = false;
            BTN_Z2GoHome.Refresh();

            Device?.ResetZ2();

            BTN_Z2GoHome.Enabled = true;
        }
        public static void OnClose()
        {
            Device?.Close();
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
    }
}
