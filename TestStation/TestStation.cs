using Database;
using Hardware;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Utils;

namespace TestStation
{
    public partial class TestStation : Form
    {
        public TestStation()
        {
            InitializeComponent();

            Logger log = new Logger();
            log.Debug(string.Format("TestStation(V{0}) Started", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()));
            log.Error("TestStation Started", new Exception("This is a test ex"));

            Equipment e = new Equipment("0");
            e.Execute(new Command("PowerOn"));

            Port b = new Port("0");
            b.Execute(
                new Command(
                    "Send", 
                    new Dictionary<string, string>()
                    {
                        {
                            "Bytes",
                            "00 01 02"
                        }
                    }
                )
            );

            PowerSupply ps = new PowerSupply("0");
            ps.Execute(new Command("PowerOn"));

            ps.PPort = new Port("0");
            ps.Execute(new Command("PowerOn"));
            ps.Execute(new Command("PowerOff"));

            HardwareSrv hardware = new HardwareSrv();
            hardware.Add(e);

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
