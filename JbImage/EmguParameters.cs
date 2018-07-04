using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JbImage
{
    public class EmguParameters
    {
        public static Dictionary<string, string> Item = new Dictionary<string, string>() {
            { "UseCanny", "false"},
            { "SaveFile", "false"},
            { "ShowFirstResult", "false"},

            { "BinThreshold", "30"},
            { "FilterSizeExtra", "10"},

            { "Canny1Threshold1", "90" },
            { "Canny1Threshold2", "180" },
            { "Canny1ApertureSize", "3" },
            { "Canny1I2Gradient", "false" },

            { "Hough1Dp", "2" },
            { "Hough1MinDist", "40" },
            { "Hough1Param1", "180" },
            { "Hough1Param2", "13" },
            { "Hough1MinRadius", "18" },
            { "Hough1MaxRadius", "25" },

            { "Canny2Threshold1", "90" },
            { "Canny2Threshold2", "180" },
            { "Canny2ApertureSize", "3" },
            { "Canny2I2Gradient", "false" },

            { "Hough2Dp", "2" },
            { "Hough2MinDist", "40" },
            { "Hough2Param1", "180" },
            { "Hough2Param2", "13" },
            { "Hough2MinRadius", "18" },
            { "Hough2MaxRadius", "25" },
        };
    };
}