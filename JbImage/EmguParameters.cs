using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace JbImage
{
    public class EmguParameters
    {
        public static List<Parameters> Params = new List<Parameters>();
        static EmguParameters()
        {
            Utils.XmlSerializer.Load(@"EmguParameters.xml", out Params);
            if (Params.Count == 0)
            {
                Params.Add(new Parameters());
            }
        }
    };
    public class Parameters
    {
        [XmlAttribute("Tag")]
        public string Tag
        { get; set; }


        [XmlAttribute("Gain")]
        public int Gain
        { get; set; }
        [XmlAttribute("ExposureTime")]/*ms*/
        public int ExposureTime
        { get; set; }

        [XmlAttribute("ExtraStrengthen")]
        public bool ExtraStrengthen
        { get; set; }
        [XmlAttribute("UseCanny")]
        public bool UseCanny
        { get; set; }
        [XmlAttribute("SaveFile")]
        public bool SaveFile
        { get; set; }
        [XmlAttribute("ShowFirstResult")]
        public bool ShowFirstResult
        { get; set; }

        [XmlAttribute("BinThreshold")]
        public int BinThreshold
        { get; set; }
        [XmlAttribute("FilterSquareExtra")]
        public int FilterSquareExtra
        { get; set; }

        [XmlAttribute("Canny1Threshold1")]
        public double Canny1Threshold1
        { get; set; }
        [XmlAttribute("Canny1Threshold2")]
        public double Canny1Threshold2
        { get; set; }
        [XmlAttribute("Canny1ApertureSize")]
        public int Canny1ApertureSize
        { get; set; }
        [XmlAttribute("Canny1I2Gradient")]
        public bool Canny1I2Gradient
        { get; set; }

        [XmlAttribute("Hough1Dp")]
        public double Hough1Dp
        { get; set; }
        [XmlAttribute("Hough1MinDist")]
        public double Hough1MinDist
        { get; set; }
        [XmlAttribute("Hough1Param1")]
        public double Hough1Param1
        { get; set; }
        [XmlAttribute("Hough1Param2")]
        public double Hough1Param2
        { get; set; }
        [XmlAttribute("Hough1MinRadius")]
        public int Hough1MinRadius
        { get; set; }
        [XmlAttribute("Hough1MaxRadius")]
        public int Hough1MaxRadius
        { get; set; }

        [XmlAttribute("Canny2Threshold1")]
        public double Canny2Threshold1
        { get; set; }
        [XmlAttribute("Canny2Threshold2")]
        public double Canny2Threshold2
        { get; set; }
        [XmlAttribute("Canny2ApertureSize")]
        public int Canny2ApertureSize
        { get; set; }
        [XmlAttribute("Canny2I2Gradient")]
        public bool Canny2I2Gradient
        { get; set; }

        [XmlAttribute("Hough2Dp")]
        public double Hough2Dp
        { get; set; }
        [XmlAttribute("Hough2MinDist")]
        public double Hough2MinDist
        { get; set; }
        [XmlAttribute("Hough2Param1")]
        public double Hough2Param1
        { get; set; }
        [XmlAttribute("Hough2Param2")]
        public double Hough2Param2
        { get; set; }
        [XmlAttribute("Hough2MinRadius")]
        public int Hough2MinRadius
        { get; set; }
        [XmlAttribute("Hough2MaxRadius")]
        public int Hough2MaxRadius
        { get; set; }

        public Parameters()
        {
            Tag = "Default";

            Gain = 90;
            ExposureTime = 500;

            ExtraStrengthen = true;
            UseCanny = true;
            SaveFile = true;
            ShowFirstResult = true;

            BinThreshold = 50;
            FilterSquareExtra = 10;

            Canny1Threshold1 = 90;
            Canny1Threshold2 = 180;
            Canny1ApertureSize = 3;
            Canny1I2Gradient = false;

            Hough1Dp = 2;
            Hough1MinDist = 40;
            Hough1Param1 = 180;
            Hough1Param2 = 13;
            Hough1MinRadius = 18;
            Hough1MaxRadius = 25;

            Canny2Threshold1 = 90;
            Canny2Threshold2 = 180;
            Canny2ApertureSize = 3;
            Canny2I2Gradient = false;

            Hough2Dp = 2;
            Hough2MinDist = 40;
            Hough2Param1 = 180;
            Hough2Param2 = 13;
            Hough2MinRadius = 18;
            Hough2MaxRadius = 25;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}