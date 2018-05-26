using MathWorks.MATLAB.NET.Arrays;
using weist;

namespace Utils
{
    public class Matlab
    {
        public static double CalcWeist(double[] x, double[] y)
        {
            Logger log = new Logger();
            log.Debug("Matlab prepare");
            Weist w = new Weist();

            MWNumericArray matlabX = new MWNumericArray(MWArrayComplexity.Real, x.Length, 1);
            MWNumericArray matlabY = new MWNumericArray(MWArrayComplexity.Real, y.Length, 1);
            MWArray[] agrsOut = new MWArray[3];
            MWArray[] agrsIn = new MWArray[] { (MWNumericArray)matlabX, (MWNumericArray)matlabY };

            for (int i = 0; i < x.Length; i++)
            {
                matlabX[i + 1] = x[i];
                matlabY[i + 1] = y[i];
            }
            log.Debug("Matlab calculate");
            w.weist(3, ref agrsOut, agrsIn);

            double o = double.Parse(agrsOut[0].ToString());
            double z = double.Parse(agrsOut[1].ToString());
            string info = agrsOut[2].ToString();

            log.Debug("Matlab done");

            return o;
        }
    }
}