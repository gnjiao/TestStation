
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using Utils;

namespace JbImage
{
    public class AnalyzerNFT : AnalyzerIntf
    {
        public override CircleImage FindCircle(string path)
        {
            _log.Debug(Utils.String.Flatten(EmguParameters.Item));

            bool saveFile = bool.Parse(EmguParameters.Item["SaveFile"]);
            bool useCanny = bool.Parse(EmguParameters.Item["UseCanny"]);

            Path = path;
            RawImg = EmguIntfs.Load(path);

            #region 1st with constant filter and find circles
            Image<Gray, Byte> _grayedUmat = EmguIntfs.ToImage(EmguIntfs.Grayed(RawImg));
            Image<Gray, Byte> _edged;
            if (useCanny)
            {
                _edged = EmguIntfs.Canny(_grayedUmat,
                    double.Parse(EmguParameters.Item["Canny1Threshold1"]),
                    double.Parse(EmguParameters.Item["Canny1Threshold2"]),
                    Int32.Parse(EmguParameters.Item["Canny1ApertureSize"]),
                    bool.Parse(EmguParameters.Item["Canny1I2Gradient"]));
                if (saveFile)
                {
                    _edged.Save(Utils.String.FilePostfix(Path, "-1-edge"));
                }
            }
            else
            {
                _edged = _grayedUmat;
            }

            Image<Gray, Byte> image = EmguIntfs.Binarize(Int32.Parse(EmguParameters.Item["BinThreshold"]), _edged);

            Circles = CvInvoke.HoughCircles(image, HoughType.Gradient,
                double.Parse(EmguParameters.Item["Hough1Dp"]),
                double.Parse(EmguParameters.Item["Hough1MinDist"]),
                double.Parse(EmguParameters.Item["Hough1Param1"]),
                double.Parse(EmguParameters.Item["Hough1Param2"]),
                Int32.Parse(EmguParameters.Item["Hough1MinRadius"]),
                Int32.Parse(EmguParameters.Item["Hough1MaxRadius"]));
            Circles = Sort(Circles);
            #endregion

            #region filter 86.3%
            FilteredCircles = new List<CircleF>();
            FilteredLights = new List<int>();
            var raw = _grayedUmat;
            foreach (var circle in Circles)
            {
                int extra = Int32.Parse(EmguParameters.Item["FilterSizeExtra"]);

                int startX = (int)System.Math.Floor(circle.Center.X - circle.Radius - extra);
                int startY = (int)System.Math.Floor(circle.Center.Y - circle.Radius - extra);
                int len = (int)System.Math.Ceiling((double)circle.Radius * 2.0) + 2 * extra;
                if (startX < 0 || startY < 0)
                {
                    _log.Warn("FilterSizeExtra may be too big, filter abandoned");
                    continue;
                }

                int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
                if (strength >= 30)/* filter fake circles */
                {
                    FilteredCircles.Add(circle);

                    int threshold = (int)((double)strength * 0.863);

                    raw.ROI = new Rectangle(startX, startY, len, len);
                    Image<Gray, Byte> oneCircle = EmguIntfs.Binarize(threshold, raw);
                    raw.ROI = Rectangle.Empty;

                    for (int x = 0; x < len; x++)
                    {
                        for (int y = 0; y < len; y++)
                        {
                            raw.Data[startY + y, startX + x, 0] = oneCircle.Data[y, x, 0];
                        }
                    }
                }
            }
            if (saveFile)
            {
                raw.Save(Utils.String.FilePostfix(Path, "-2-filter"));
            }
            #endregion

            if (useCanny)
            {
                _edged = EmguIntfs.Canny(raw,
                    double.Parse(EmguParameters.Item["Canny2Threshold1"]),
                    double.Parse(EmguParameters.Item["Canny2Threshold2"]),
                    Int32.Parse(EmguParameters.Item["Canny2ApertureSize"]),
                    bool.Parse(EmguParameters.Item["Canny2I2Gradient"]));
                if (saveFile)
                {
                    _edged.Save(Utils.String.FilePostfix(Path, "-3-edge"));
                }
            }
            else
            {
                _edged = raw;
            }

            Circles2nd = CvInvoke.HoughCircles(_edged, HoughType.Gradient,
                double.Parse(EmguParameters.Item["Hough2Dp"]),
                double.Parse(EmguParameters.Item["Hough2MinDist"]),
                double.Parse(EmguParameters.Item["Hough2Param1"]),
                double.Parse(EmguParameters.Item["Hough2Param2"]),
                Int32.Parse(EmguParameters.Item["Hough2MinRadius"]),
                Int32.Parse(EmguParameters.Item["Hough2MaxRadius"]));
            Circles2nd = Sort(Circles2nd);
            FilteredCircles2nd = new List<CircleF>();
            foreach (var circle in Circles2nd)
            {
                int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
                if (strength > 30)
                {
                    FilteredCircles2nd.Add(circle);
                }
            }

            CircleImage ret = new CircleImage();
            ret.Path = Path;
            ret.Circles = FilteredCircles2nd;
            ret.RetImg = NFTDrawCircle();

            return ret;
        }
        public override Result Calculate(List<CircleImage> img, List<double> distance)
        {
            double[] result = new double[img[0].Circles.Count];
            for (int circleId = 0; circleId < result.Length; circleId++)
            {
                try
                {
                    double[] radius = new double[img.Count];

                    for (int imgId = 0; imgId < radius.Length; imgId++)
                    {
                        radius[imgId] = img[imgId].Circles[circleId].Radius;
                    }
                    string output = "";
                    for (int i = 0; i < radius.Length; i++)
                    {
                        output += radius[i].ToString("F2") + ",";
                    }
                    if (output.Length > 0)
                    {
                        output = output.Substring(0, output.Length - 1);
                    }
                    _log.Debug($"Circle{circleId} radius: {output}");

                    result[circleId] = Matlab.CalcWeist2(radius, distance.ToArray());
                    _log.Info($"{circleId}: {result[circleId]}" + Environment.NewLine);
                }
                catch (Exception ex)
                {
                    result[circleId] = double.NaN;
                    _log.Error($"Failed to calcute weist for {circleId}", ex);
                }
            }

            return new Result("Ok");
        }

        private Bitmap NFTDrawCircle()
        {
            bool showFirstResult = bool.Parse(EmguParameters.Item["ShowFirstResult"]);

            _log.Debug("Start DrawCircles");
            Mat circleImage = RawImg.Mat;

            if (showFirstResult)
            {
                for (int i = 0; i < FilteredCircles.Count; i++)
                {
                    CvInvoke.Circle(circleImage, Point.Round(FilteredCircles[i].Center), (int)FilteredCircles[i].Radius, new Bgr(Color.Yellow).MCvScalar, 1);
                }
            }

            for (int i = 0; i < FilteredCircles2nd.Count; i++)
            {
                CvInvoke.Circle(circleImage, Point.Round(FilteredCircles2nd[i].Center), (int)FilteredCircles2nd[i].Radius, new Bgr(Color.Red).MCvScalar, 1);
                CvInvoke.PutText(circleImage, i.ToString("D3"), new Point((int)FilteredCircles2nd[i].Center.X, (int)FilteredCircles2nd[i].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
            }
            circleImage.Save(Utils.String.FilePostfix(Path, "-result"));

            return circleImage.Bitmap;
        }
    }
}