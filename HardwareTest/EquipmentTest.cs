using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using Hardware;

namespace HardwareTest
{
    [TestClass]
    public class EquipmentTest
    {
        [TestMethod]
        public void BasicTest()
        {
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
        }
        [TestMethod]
        public void CameraTest()
        {
            Camera c = new Camera("M8051");
            c.Execute(new Command("Open"));
            c.Execute(new Command("Read"));
            c.Execute(new Command("Close"));
        }
    }
}