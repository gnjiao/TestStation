using Database;
using Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Utils;

namespace TestStation
{
    public partial class FormCameraCtrl : Form
    {
        DatabaseSrv _database;
        Camera _camera;

        public FormCameraCtrl()
        {
            InitializeComponent();

            Logger log = new Logger("TestStation");
            log.Debug(string.Format("TestStation(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));

            _database = DatabaseSrv.GetInstance();
            _camera = HardwareSrv.GetInstance().Get("Camera") as Camera;
            _camera.Execute(new Command("Open"));
        }

        private void BTN_Calculate_Click(object sender, EventArgs e)
        {
            foreach (var img in _imgs)
            {
            }

            double temp = Utils.Math.Tan(45);
            temp = Utils.Math.Atan(1);
        }

        private List<Bitmap> _imgs = new List<Bitmap>();
        private void BTN_Read_Click(object sender, EventArgs e)
        {
            Bitmap img = _camera.Execute(new Command("Read")).Param as Bitmap;
            _imgs.Add(img);
        }

        private void FormCameraCtrl_FormClosing(object sender, FormClosingEventArgs e)
        {
            _camera.Execute(new Command("Close"));
        }
    }
}
