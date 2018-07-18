
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
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
            if (saveFile)
            {
                _bin.Save(Utils.String.FilePostfix(Path, "-0-bin"));
            }

            Image<Gray, byte> tempc = new Image<Gray, byte>(_bin.Width, _bin.Height);
            Image<Gray, byte> d = new Image<Gray, byte>(_bin.Width, _bin.Height);

            VectorOfVectorOfPoint con = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(_bin, con, tempc, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
            for (int conId = 0; conId < con.Size; conId++)
            {
                CvInvoke.DrawContours(d, con, conId, new MCvScalar(255, 0, 255, 255), 2);
            }
            if (saveFile)
            {
                d.Save(Utils.String.FilePostfix(Path, "-1-contour"));
            }

            List<RotatedRect> ellipses = new List<RotatedRect>();
            List<List<Point>> rects = new List<List<Point>>();
            for (int conId = 0; conId < con.Size; conId++)
            {
                if (con[conId].Size > 100)
                {
                    var ellipse = CvInvoke.FitEllipse(con[conId]);

                    PointF[] points = new PointF[4];
                    points = ellipse.GetVertices();
                    rects.Add(points.Select(x => Point.Truncate(x)).ToList());

                    ellipse.Angle = ellipse.Angle - 90;
                    ellipses.Add(ellipse);
                }
            }
            if (ellipses.Count > 1)
            {
                _log.Warn("More than 1 ellipse found, please check the image");
            }

            CircleImage ret = new CircleImage();
            ret.Ellipse = ellipses[0];
            ret.Rect = rects[0];
            ret.RetImg = Draw(RawImg, ellipses, rects);
            if (saveFile)
            {
                ret.RetImg.Save(Utils.String.FilePostfix(Path, "-2-ellipse"));
            }

            _log.Debug($"Ellipse X: {ret.Ellipse.Size.Height}, Y: {ret.Ellipse.Size.Width}");

            Find863(_grayedUmat, ret);

            return ret;
        }
        private void Find863(Image<Gray, byte> img, CircleImage info)
        {
            int centerX = (info.Rect[0].X + info.Rect[2].X) / 2;
            int centerY = (info.Rect[0].Y + info.Rect[2].Y) / 2;

            int leftX = (info.Rect[2].X + info.Rect[3].X) / 2;
            int leftY = (info.Rect[2].Y + info.Rect[3].Y) / 2;
            int rightX = (info.Rect[0].X + info.Rect[1].X) / 2;
            int rightY = (info.Rect[0].Y + info.Rect[1].Y) / 2;

            int minX = leftX < centerX ? leftX : centerX;
            int maxX = leftX > centerX ? leftX : centerX;
            int distance = System.Math.Abs(maxX - minX);
            if (distance > centerX)
            {
                _log.Warn("The picture locates too left");
            }

            int sum = 0;
            for (int x = 0; x <= distance; x++)
            {
                sum += img.Data[centerY, centerX - x, 0];
            }
            _log.Debug($"Total distance {distance:F3}");

            for (int i = distance / 2; i <= distance; i++)
            {
                int dsum = 0;
                for (int x = 0; x <= i; x++)
                {
                    dsum += img.Data[centerY, centerX - x, 0];
                }

                double ratio = (double)dsum / sum;
                _log.Debug($"radius({i}) dsum({dsum}) sum({sum}) ratio {ratio:F3}");
                if (ratio >= 0.863)
                {
                    info.X863 = i;
                    break;
                }
            }

        }
        private double CalcDivergenceAngle1(List<CircleImage> img)
        {
            /* arctan(光斑半径 / 芯片到透镜的距离) */
            return Utils.Math.Atan(img[0].X863 * 5.5 * 0.001 / 34);
        }
        private double CalcPowerDensity(List<CircleImage> img)
        {
            /* 激光器发光功率 / 光斑面积 */
            double area = System.Math.PI * img[0].Ellipse.Size.Width * img[0].Ellipse.Size.Height / 4;
            return area;
        }
        public override Result Calculate(List<CircleImage> img, List<double> distance)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ret["Emitter Divergence Angle"] = CalcDivergenceAngle1(img).ToString("F3");
            ret["Power Density"] = CalcPowerDensity(img).ToString("F3");

            return new Result("Ok", null, ret);
        }

        private Bitmap Draw(Image<Bgr, byte> raw, List<RotatedRect> ellipses, List<List<Point>> rects)
        {
            for (int i = 0; i < ellipses.Count; i++)
            {
                CvInvoke.Ellipse(raw, ellipses[i], new Bgr(Color.Red).MCvScalar, 2);

                for (int j = 0; j < 4; j++)
                {
                    CvInvoke.Line(raw, rects[i][j], rects[i][(j + 1) % 4], new Bgr(Color.Green).MCvScalar, 2);
                }
            }

            return raw.Bitmap;
        }
        #region obsolete
        private CircleImage FindRegularCircle(string path, Parameters param = null)
        {
            _log.Debug(param.ToString());

            bool saveFile = param.SaveFile;
            bool useCanny = param.UseCanny;

            Path = path;
            RawImg = EmguIntfs.Load(path);

            Image<Gray, Byte> _grayedUmat = EmguIntfs.ToImage(EmguIntfs.Grayed(RawImg));

            Image<Gray, Byte> _bin = EmguIntfs.Binarize(param.BinThreshold, _grayedUmat);
            _bin.Save(Utils.String.FilePostfix(Path, "-0-bin"));

            Image<Gray, byte> _edged = EmguIntfs.Canny(_bin,
                param.Canny1Threshold1,
                param.Canny1Threshold2,
                param.Canny1ApertureSize,
                param.Canny1I2Gradient);
            _edged.Save(Utils.String.FilePostfix(Path, "-1-edge"));

            Image<Gray, byte> tempc = new Image<Gray, byte>(_bin.Width, _bin.Height);
            Image<Gray, byte> d = new Image<Gray, byte>(_bin.Width, _bin.Height);
            VectorOfVectorOfPoint con = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(_edged, con, tempc, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
            for (int conId = 0; conId < con.Size; conId++)
                CvInvoke.DrawContours(d, con, conId, new MCvScalar(255, 0, 255, 255), 2);
            d.Save(Utils.String.FilePostfix(Path, "-1-contour"));

            List<RotatedRect> rects = new List<RotatedRect>();
            for (int conId = 0; conId < con.Size; conId++)
            {
                if (con[conId].Size > 5)
                {
                    rects.Add(CvInvoke.FitEllipse(con[conId]));
                }
            }

            foreach (var rect in rects)
            {
                CvInvoke.Ellipse(d, rect, new Bgr(Color.White).MCvScalar, 2);
            }
            d.Save(Utils.String.FilePostfix(Path, "-1-rects"));

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
            for(int i = 0; i<Circles2nd.Length; i++)
            {
                CircleF circle = Circles2nd[i];

                int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
                if (strength > 30)
                {
                    int b = CountPixels(_grayedUmat, ref circle);
                    brightness.Add(b);
                    _log.Info($"Circle{i:D3}: ({circle.Center.X},{circle.Center.Y}) {circle.Radius} {b}");

                    FilteredCircles2nd.Add(circle);
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
            ret.Circles = FilteredCircles2nd;
            ret.Brightness = brightness;
            ret.RetImg = _result.Bitmap;

            return ret;
        }
        #endregion
    }
}