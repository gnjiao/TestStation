using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace TestStation.core
{
    public class MotorController
    {
        public delegate void PositionUpdate(string z, double position);

        private static Logger _log = new Logger("MotorController");
        public DS102 Device;
        public PositionUpdate Observer;
        public MotorController(string portName)
        {
            Device = new DS102();
            try
            {
                Device.Port = Int32.Parse(portName);
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
        public void Close()
        {
            Device?.CloseDS102();
            Device = null;
        }

        public Result Reset()
        {
            ResetZ1();
            ResetZ2();
            return new Result("Ok");
        }
        public Result ResetZ1()
        {
            _log.Debug("Z1 go original position");
            Device?.GoOrigin(DS102.AXIS_Z1);
            Z1Position = 0;
            Observer?.Invoke("Z1", Z1Position);
            return new Result("Ok");
        }
        public Result ResetZ2()
        {
            _log.Debug("Z2 go original position");
            Device?.GoOrigin(DS102.AXIS_Z2);
            Z2Position = 0;
            Observer?.Invoke("Z2", Z2Position);
            return new Result("Ok");
        }
        public Result MoveZ1(double value)
        {
            if (value > 0)
            {
                Device?.ZAxisGoPositive(DS102.AXIS_Z1, value);
            }
            else
            {
                Device?.ZAxisGoNegative(DS102.AXIS_Z1, System.Math.Abs(value));
            }

            Z1Position += value;
            _log.Debug($"Z1 go {(value > 0 ? "Up" : "Down")} {System.Math.Abs(value)} to {Z1Position:F2}");
            Observer?.Invoke("Z1", Z1Position);

            return new Result("Ok");
        }
        public Result MoveZ2(double value)
        {
            if (value > 0)
            {
                Device?.ZAxisGoPositive(DS102.AXIS_Z2, value);
            }
            else
            {
                Device?.ZAxisGoNegative(DS102.AXIS_Z2, System.Math.Abs(value));
            }

            Z2Position += value;
            _log.Debug($"Z2 go {(value < 0 ? "Up" : "Down")} {System.Math.Abs(value)} to {Z2Position:F2}");
            Observer?.Invoke("Z2", Z2Position);

            return new Result("Ok");
        }
        public double Z1Position;
        public double Z2Position;
    }
}