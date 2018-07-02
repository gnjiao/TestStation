
using System.Collections.Generic;

namespace Utils
{
    public class Math
    {
        public static double Rad(double angle)
        {
            return System.Math.PI / (180 / angle);
        }
        public static double Angle(double rad)
        {
            return 180 / (System.Math.PI / rad);
        }
        public static double Tan(double angle)
        {
            return System.Math.Tan(Rad(angle));
        }
        public static double Atan(double value)
        {
            return Math.Angle(System.Math.Atan(value));
        }
        public static double StdEv(List<double> values)
        {
            double stdEv = 0;
            double average = 0;
            foreach (var value in values)
            {
                average += value;
            }
            average /= values.Count;

            double deviationSum = 0;
            foreach (var value in values)
            {
                deviationSum += ((double)value - average)*((double)value - average);
            }

            stdEv = System.Math.Sqrt(deviationSum / (values.Count - 1));
            return stdEv * 3 / average;
        }
    }
}