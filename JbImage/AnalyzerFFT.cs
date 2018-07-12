
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
    public class AnalyzerFFT : AnalyzerIntf
    {
        public override CircleImage FindCircle(string path, Parameters param = null)
        {
            _log.Debug(param.ToString());

            bool saveFile = param.SaveFile;
            bool useCanny = param.UseCanny;

            Path = path;
            RawImg = EmguIntfs.Load(path);

            Image<Gray, Byte> _grayedUmat = EmguIntfs.ToImage(EmguIntfs.Grayed(RawImg));
            
            Image<Gray, Byte> _bin = EmguIntfs.Binarize(param.BinThreshold, _grayedUmat);
            _bin.Save(Utils.String.FilePostfix(Path, "-0-bin"));

            Image<Gray, Byte> _edged = EmguIntfs.Canny(_bin,
                param.Canny1Threshold1,
                param.Canny1Threshold2,
                param.Canny1ApertureSize,
                param.Canny1I2Gradient);
            _edged.Save(Utils.String.FilePostfix(Path, "-1-edge"));

            Circles = CvInvoke.HoughCircles(_edged, HoughType.Gradient, 
                param.Hough1Dp,
                param.Hough1MinDist,
                param.Hough1Param1,
                param.Hough1Param2,
                param.Hough1MinRadius, param.Hough1MaxRadius);
            Circles = Sort(Circles);

            #region filter 86.3%
            FilteredCircles = new List<CircleF>();
            FilteredLights = new List<int>();
            FilteredCircles2nd = new List<CircleF>();
            var raw = _grayedUmat;
            foreach (var circle in Circles)
            {
                int extra = param.FilterSquareExtra;

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
                    FilteredCircles2nd.Add(circle);

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
                    param.Canny2Threshold1,
                    param.Canny2Threshold2,
                    param.Canny2ApertureSize,
                    param.Canny2I2Gradient);
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
                param.Hough2Dp,
                param.Hough2MinDist,
                param.Hough2Param1,
                param.Hough2Param2,
                param.Hough2MinRadius, param.Hough2MaxRadius);
            Circles2nd = Sort(Circles2nd);
            FilteredCircles2nd = new List<CircleF>();
            List<int> brightness = new List<int>();

            _log.Info($"Circles Information");
            int i = 0;
            foreach (var circle in Circles2nd)
            {
                int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
                if (strength > 30)
                {
                    FilteredCircles2nd.Add(circle);
                    int b = CountPixels(_grayedUmat, circle);
                    brightness.Add(b);
                    _log.Info($"Circle{i:D3}: ({circle.Center.X},{circle.Center.Y}) {circle.Radius} {b}");

                    i++;
                }
            }

            #region draw
            Mat _result = RawImg.Mat;

            for (int c = 0; c < FilteredCircles2nd.Count; c++)
            {
                Point center = Point.Round(FilteredCircles2nd[c].Center);
                center.X *= 1;
                center.Y *= 1;
                int radius = (int)FilteredCircles2nd[c].Radius * 1;

                //if (2 * radius < _result.Size.Height && 2 * radius < _result.Size.Width)
                {
                    CvInvoke.Circle(_result, center, radius, new Bgr(Color.Red).MCvScalar, 1);
                    CvInvoke.PutText(_result, c.ToString("D3"), new Point((int)FilteredCircles2nd[c].Center.X, (int)FilteredCircles2nd[c].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
                }
            }
            _result.Save(Utils.String.FilePostfix(Path, "-result"));
            #endregion

            CircleImage ret = new CircleImage();
            ret.Path = Path;
            ret.Circles = FilteredCircles2nd;
            ret.Brightness = brightness;
            ret.RetImg = _result.Bitmap;

            return ret;
        }
        private double CalcDivergenceAngle(List<CircleImage> img)
        {
            /* arctan(光斑半径 / 芯片到透镜的距离) */
            double[] angles = new double[img[img.Count - 1].Circles.Count];
            for (int i = 0; i < angles.Length; i++)
            {
                angles[i] = Utils.Math.Atan(((double)img[img.Count - 1].Circles[i].Radius * 5.5 / 1000) / 30);
            }
            return angles.ToList().Average();
        }
        private double CalcPowerDensity(List<CircleImage> img)
        {
            /* 激光器发光功率 / 光斑面积 */
            double result = img[img.Count - 1].Circles[0].Area;
            return double.NaN;
        }
        public override Result Calculate(List<CircleImage> img, List<double> distance)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ret["Emitter Divergence Angle"] = CalcDivergenceAngle(img).ToString("F3");
            ret["Power Density"] = CalcPowerDensity(img).ToString("F3");

            return new Result("Ok", null, ret);
        }
    }
}