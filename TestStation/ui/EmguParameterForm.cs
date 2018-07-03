using System;
using System.Windows.Forms;
using JbImage;

namespace TestStation.ui
{
    public partial class EmguParameterForm : Form
    {
        public EmguParameterForm()
        {
            InitializeComponent();
            LoadEmguParameters();
        }

        private void BTN_Set_Click(object sender, EventArgs e)
        {
            EmguParameters.Item["BinThreshold"] = tbBinThreshold.Text;

            EmguParameters.Item["Canny1Threshold1"] = tbCanny1Threshold1.Text;
            EmguParameters.Item["Canny1Threshold2"] = tbCanny1Threshold2.Text;
            EmguParameters.Item["Canny1ApertureSize"] = tbCanny1ApertureSize.Text;
            EmguParameters.Item["Canny1I2Gradient"] = tbCanny1I2Gradient.Text;

            EmguParameters.Item["Hough1Dp"] = tbHough1Dp.Text;
            EmguParameters.Item["Hough1MinDist"] = tbHough1MinDist.Text;
            EmguParameters.Item["Hough1Param1"] = tbHough1Param1.Text;
            EmguParameters.Item["Hough1Param2"] = tbHough1Param2.Text;
            EmguParameters.Item["Hough1MinRadius"] = tbHough1MinRadius.Text;
            EmguParameters.Item["Hough1MaxRadius"] = tbHough1MaxRadius.Text;

            EmguParameters.Item["Canny2Threshold1"] = tbCanny2Threshold1.Text;
            EmguParameters.Item["Canny2Threshold2"] = tbCanny2Threshold2.Text;
            EmguParameters.Item["Canny2ApertureSize"] = tbCanny2ApertureSize.Text;
            EmguParameters.Item["Canny2I2Gradient"] = tbCanny2I2Gradient.Text;

            EmguParameters.Item["Hough2Dp"] = tbHough2Dp.Text;
            EmguParameters.Item["Hough2MinDist"] = tbHough2MinDist.Text;
            EmguParameters.Item["Hough2Param1"] = tbHough2Param1.Text;
            EmguParameters.Item["Hough2Param2"] = tbHough2Param2.Text;
            EmguParameters.Item["Hough2MinRadius"] = tbHough2MinRadius.Text;
            EmguParameters.Item["Hough2MaxRadius"] = tbHough2MaxRadius.Text;

            EmguParameters.Item["SaveFile"] = CB_Save.Checked.ToString();
            EmguParameters.Item["UseCanny"] = CB_UseCanny.Checked.ToString();
            EmguParameters.Item["ShowFirstResult"] = CB_ShowFirstResult.Checked.ToString();
        }

        private void BTN_Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.BinThreshold = EmguParameters.Item["BinThreshold"];

            Properties.Settings.Default.Canny1Threshold1 = EmguParameters.Item["Canny1Threshold1"];
            Properties.Settings.Default.Canny1Threshold2 = EmguParameters.Item["Canny1Threshold2"];
            Properties.Settings.Default.Canny1ApertureSize = EmguParameters.Item["Canny1ApertureSize"];
            Properties.Settings.Default.Canny1I2Gradient = EmguParameters.Item["Canny1I2Gradient"];

            Properties.Settings.Default.Hough1Dp = EmguParameters.Item["Hough1Dp"];
            Properties.Settings.Default.Hough1MinDist = EmguParameters.Item["Hough1MinDist"];
            Properties.Settings.Default.Hough1Param1 = EmguParameters.Item["Hough1Param1"];
            Properties.Settings.Default.Hough1Param2 = EmguParameters.Item["Hough1Param2"];
            Properties.Settings.Default.Hough1MinRadius = EmguParameters.Item["Hough1MinRadius"];
            Properties.Settings.Default.Hough1MaxRadius = EmguParameters.Item["Hough1MaxRadius"];

            Properties.Settings.Default.Canny2Threshold1 = EmguParameters.Item["Canny2Threshold1"];
            Properties.Settings.Default.Canny2Threshold2 = EmguParameters.Item["Canny2Threshold2"];
            Properties.Settings.Default.Canny2ApertureSize = EmguParameters.Item["Canny2ApertureSize"];
            Properties.Settings.Default.Canny2I2Gradient = EmguParameters.Item["Canny2I2Gradient"];

            Properties.Settings.Default.Hough2Dp = EmguParameters.Item["Hough2Dp"];
            Properties.Settings.Default.Hough2MinDist = EmguParameters.Item["Hough2MinDist"];
            Properties.Settings.Default.Hough2Param1 = EmguParameters.Item["Hough2Param1"];
            Properties.Settings.Default.Hough2Param2 = EmguParameters.Item["Hough2Param2"];
            Properties.Settings.Default.Hough2MinRadius = EmguParameters.Item["Hough2MinRadius"];
            Properties.Settings.Default.Hough2MaxRadius = EmguParameters.Item["Hough2MaxRadius"];

            Properties.Settings.Default.SaveFile = EmguParameters.Item["SaveFile"];
            Properties.Settings.Default.UseCanny = EmguParameters.Item["UseCanny"];
            Properties.Settings.Default.ShowFirstResult = EmguParameters.Item["ShowFirstResult"];

            Properties.Settings.Default.Save();
        }
        private void BTN_Reset_Click(object sender, EventArgs e)
        {
            LoadEmguParameters();
        }
        private void LoadEmguParameters()
        {
            tbBinThreshold.Text = EmguParameters.Item["BinThreshold"];

            tbCanny1Threshold1.Text = EmguParameters.Item["Canny1Threshold1"];
            tbCanny1Threshold2.Text = EmguParameters.Item["Canny1Threshold2"];
            tbCanny1ApertureSize.Text = EmguParameters.Item["Canny1ApertureSize"];
            tbCanny1I2Gradient.Text = EmguParameters.Item["Canny1I2Gradient"];

            tbHough1Dp.Text = EmguParameters.Item["Hough1Dp"];
            tbHough1MinDist.Text = EmguParameters.Item["Hough1MinDist"];
            tbHough1Param1.Text = EmguParameters.Item["Hough1Param1"];
            tbHough1Param2.Text = EmguParameters.Item["Hough1Param2"];
            tbHough1MinRadius.Text = EmguParameters.Item["Hough1MinRadius"];
            tbHough1MaxRadius.Text = EmguParameters.Item["Hough1MaxRadius"];

            tbCanny2Threshold1.Text = EmguParameters.Item["Canny2Threshold1"];
            tbCanny2Threshold2.Text = EmguParameters.Item["Canny2Threshold2"];
            tbCanny2ApertureSize.Text = EmguParameters.Item["Canny2ApertureSize"];
            tbCanny2I2Gradient.Text = EmguParameters.Item["Canny2I2Gradient"];

            tbHough2Dp.Text = EmguParameters.Item["Hough2Dp"];
            tbHough2MinDist.Text = EmguParameters.Item["Hough2MinDist"];
            tbHough2Param1.Text = EmguParameters.Item["Hough2Param1"];
            tbHough2Param2.Text = EmguParameters.Item["Hough2Param2"];
            tbHough2MinRadius.Text = EmguParameters.Item["Hough2MinRadius"];
            tbHough2MaxRadius.Text = EmguParameters.Item["Hough2MaxRadius"];

            CB_UseCanny.Checked = bool.Parse(EmguParameters.Item["UseCanny"]);
            CB_Save.Checked = bool.Parse(EmguParameters.Item["SaveFile"]);
            CB_ShowFirstResult.Checked = bool.Parse(EmguParameters.Item["ShowFirstResult"]);
        }
    }
}
