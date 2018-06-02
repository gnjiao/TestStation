using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;
using Hardware;
using System.Threading;

namespace HardwareTest
{
    [TestClass]
    public class EquipmentTest
    {
        [TestMethod]
        public void EquipmentTest_Basic()
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

            HardwareSrv hardware = HardwareSrv.GetInstance();
            hardware.Add(e);
        }
        [Ignore]/*Need connect M8051*/
        [TestMethod]
        public void EquipmentTest_Camera()
        {
            Camera c = new Camera("M8051");
            c.Execute(new Command("Open"));
            Thread.Sleep(3 * 1000);
            var result = c.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Raw" } }));
            result = c.Execute(new Command("Read", new Dictionary<string, string> { { "Type", "Bmp" } }));
            c.Execute(new Command("Close"));
        }
    }
}