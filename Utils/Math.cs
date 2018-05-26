
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
        public static double StdEv(List<int> values)
        {
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

            return System.Math.Sqrt(deviationSum / (values.Count - 1));
        }
    }
}