using Database;
using Hardware;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils;

namespace TestStation
{
    public partial class FormCameraCtrl : Form
    {
        public FormCameraCtrl()
        {
            InitializeComponent();

            Logger log = new Logger("TestStation");
            log.Debug(string.Format("TestStation(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));


            DatabaseSrv database = new DatabaseSrv();
        }

        private void BTN_Calculate_Click(object sender, EventArgs e)
        {
            double temp = Utils.Math.Tan(45);
            temp = Utils.Math.Atan(1);
        }

        private void BTN_Read_Click(object sender, EventArgs e)
        {
        }
    }
}
