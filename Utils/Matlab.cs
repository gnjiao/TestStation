using MathWorks.MATLAB.NET.Arrays;
using weist;
using weist2;

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
        public static double CalcWeist2(double[] radius, double[] distance)
        {
            Logger log = new Logger();
            Weist2 w = new Weist2();

            MWNumericArray matlabX = new MWNumericArray(MWArrayComplexity.Real, radius.Length, 1);
            MWNumericArray matlabY = new MWNumericArray(MWArrayComplexity.Real, distance.Length, 1);
            MWArray[] agrsOut = new MWArray[2];
            MWArray[] agrsIn = new MWArray[] { (MWNumericArray)matlabX, (MWNumericArray)matlabY };

            for (int i = 0; i < distance.Length; i++)
            {
                matlabX[i + 1] = radius[i];
                matlabY[i + 1] = distance[i];
            }
            w.weist2(2, ref agrsOut, agrsIn);

            double o = double.Parse(agrsOut[0].ToString());
            string info = agrsOut[1].ToString();

            return o;
        }
    }
}