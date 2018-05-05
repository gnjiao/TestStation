using Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            log.Debug("TestStation Started");
            log.Error("TestStation Started", new Exception("This is a test ex"));

            HardwareSrv hardware = new HardwareSrv();
            Equipment e = new Equipment("0");
            hardware.Add(e);

            e.Execute(new Command("PowerOn"));
        }
    }
}
