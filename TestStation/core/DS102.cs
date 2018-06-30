using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;

namespace TestStation.core
{
    public class DS102
    {
        public const int AXIS_Z1 = 1;
        public const int AXIS_Z2 = 2;
        public const int POSITIVE = 0;
        public const int NEGATIVE = 1;

        private SerialPort _ds102 = new SerialPort();

        private int _port = 1;

        public int Port
        {
            get { return _port; }
            set
            {
                if (value >= 1 && value <= 8)
                    _port = value;
                else
                    _port = 1;
            }
        }

        public bool OpenDS102()
        {
            bool ret = true;

            try
            {
                _ds102.PortName = "COM" + _port.ToString();
                _ds102.BaudRate = 38400;
                _ds102.DataBits = 8;
                _ds102.Parity = Parity.None;
                _ds102.StopBits = StopBits.One;

                _ds102.Open();
             
            }
            catch(Exception ex)
            {
                ret = false;
            }

            return ret;
        }

        public void CloseDS102()
        {
            _ds102.Close();
        }

        private void SendCommandToDS102(string cmd)
        {
            //Console.WriteLine("Send out command: " + cmd);
            _ds102.Write(cmd + "\r");
        }

        public void GoOrigin(int axisId)
        {
            switch(axisId)
            {
                case AXIS_Z1:
                    SendCommandToDS102("AXIS1:GO ORG");
                    break;
                case AXIS_Z2:
                    SendCommandToDS102("AXIS2:GO ORG");
                    break;
            }
        }

        public void ZAxisGoPositive(int axisId, double dist)
        {
            switch (axisId)
            {
                case AXIS_Z1:
                    SendCommandToDS102("AXIS1:PULS " + dist.ToString() + ":GO CW");
                    break;
                case AXIS_Z2:
                    SendCommandToDS102("AXIS2:PULS " + dist.ToString() + ":GO CW");
                    break;
            }
        }
        public void ZAxisGoNegative(int axisId, double dist)
        {
            switch (axisId)
            {
                case AXIS_Z1:
                    SendCommandToDS102("AXIS1:PULS " + dist.ToString() + ":GO CCW");
                    break;
                case AXIS_Z2:
                    SendCommandToDS102("AXIS2:PULS " + dist.ToString() + ":GO CCW");
                    break;
            }
        }

        public void StopAxis(int axisId)
        {
            switch (axisId)
            {
                case AXIS_Z1:
                    SendCommandToDS102("AXIS1:STOP 1");
                    break;
                case AXIS_Z2:
                    SendCommandToDS102("AXIS2:STOP 1");
                    break;
            }
        }
    }
}
