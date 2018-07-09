
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
        public override CircleImage FindCircle(string path, Parameters param = null)
        {
            _log.Debug(param.ToString());

            Path = path;
            RawImg = EmguIntfs.Load(path);

            bool strengthen = param.ExtraStrengthen;
            bool saveFile = param.SaveFile;
            bool useCanny = param.UseCanny;

            #region 1st with constant filter and find circles
            Image <Gray, Byte> _grayedUmat = EmguIntfs.ToImage(EmguIntfs.Grayed(RawImg));

            if (strengthen)
            {
                for (int x = 0; x < _grayedUmat.Width; x++)
                {
                    for (int y = 0; y < _grayedUmat.Height; y++)
                    {
                        int value = _grayedUmat.Data[y, x, 0] * 2;
                        _grayedUmat.Data[y, x, 0] = (byte)(value > 255 ? 255 : value);
                    }
                }
                if (saveFile)
                {
                    _grayedUmat.Save(Utils.String.FilePostfix(Path, "-0-strength"));
                }
            }

            Image<Gray, Byte> _edged;
            if (useCanny)
            {
                _edged = EmguIntfs.Canny(_grayedUmat,
                    param.Canny1Threshold1,
                    param.Canny1Threshold2,
                    param.Canny1ApertureSize,
                    param.Canny1I2Gradient);
                if (saveFile)
                {
                    _edged.Save(Utils.String.FilePostfix(Path, "-1-edge"));
                }
            }
            else
            {
                _edged = _grayedUmat;
            }

            Image<Gray, Byte> image = EmguIntfs.Binarize(param.BinThreshold, _edged);

            Circles = CvInvoke.HoughCircles(image, HoughType.Gradient,
                param.Hough1Dp,
                param.Hough1MinDist,
                param.Hough1Param1,
                param.Hough1Param2,
                param.Hough1MinRadius, param.Hough1MaxRadius);
            Circles = Sort(Circles);
            #endregion

            #region filter 86.3%
            FilteredCircles = new List<CircleF>();
            FilteredLights = new List<int>();
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

            CircleImage ret = new CircleImage();
            ret.Path = Path;
            ret.Circles = FilteredCircles2nd;
            ret.Brightness = brightness;
            ret.RetImg = DrawCircle(param);

            return ret;
        }
        private double[] CalcWeist(List<CircleImage> img, List<double> distance)
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
                }
                catch (Exception ex)
                {
                    result[circleId] = double.NaN;
                    _log.Error($"Failed to calcute weist for {circleId}", ex);
                }
            }

            return result;
        }
        private double CalcDivergenceAngle(List<CircleImage> img, double[] weists)
        {/* weist * sita = lambda / pi */
            _log.Info($"DivergenceAngle");

            double[] angles = new double[weists.Length];
            for (int i = 0; i<angles.Length; i++)
            {
                try
                {
                    angles[i] = (double)940 / System.Math.PI / weists[i];
                }
                catch (Exception ex)
                {
                    angles[i] = double.NaN;
                }
                _log.Info($"Circle{i:D3}: {angles[i]:F3}");
            }

            return angles.ToList().FindAll(x => !double.IsNaN(x)).ToList().Average();
        }
        private double CalcUniformity(List<CircleImage> img)
        {
            return Utils.Math.StdEv(img[0].Brightness.Select(x => (double)x).ToList());
        }
        public override Result Calculate(List<CircleImage> img, List<double> distance)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            double[] weists = CalcWeist(img, distance);
            _log.Info($"Weist");
            for (int i = 0; i < weists.Length; i++)
            {
                _log.Info($"Circle{i:D3}: {weists[i]:F3}");
            }

            ret["Dead Emitter Count"] = img[0].Circles.Count.ToString();
            ret["Dead Cluster Count"] = img[0].Circles.Count.ToString();
            ret["Emitter Divergence Angle"] = CalcDivergenceAngle(img, weists).ToString("F3");
            ret["Beam Waist Diameter(um)"] = (5.5 * weists.ToList().FindAll(x => x < 30 && x > 15).ToList().Average()).ToString("F3");
            ret["Emission Uniformity"] = CalcUniformity(img).ToString("F3");

            return new Result("Ok", null, ret);
        }

        private Bitmap DrawCircle(Parameters param)
        {
            bool showFirstResult = param.ShowFirstResult;

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