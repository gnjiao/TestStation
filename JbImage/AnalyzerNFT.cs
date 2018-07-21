
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
        public override CircleImage FindCircle(Image<Gray, ushort> bigImage, Parameters param = null)
        {
            _log.Debug(param.ToString());

            int picId = 0;

            Path = @"D:\CurImage.bmp";

            bool strengthen = param.ExtraStrengthen;
            bool saveFile = param.SaveFile;
            bool useCanny = param.UseCanny;

            #region 1st with constant filter and find circles
            if (strengthen)
            {
                for (int x = 0; x < bigImage.Width; x++)
                {
                    for (int y = 0; y < bigImage.Height; y++)
                    {
                        int value = bigImage.Data[y, x, 0] * 2;
                        bigImage.Data[y, x, 0] = (ushort)(value > 0x3fff ? 0x3fff : value);
                    }
                }
                if (saveFile)
                {
                    bigImage.Save(Utils.String.FilePostfix(Path, "-0-strength"));
                }
            }

            Image<Gray, ushort> image = EmguIntfs.Binarize(param.BinThreshold, bigImage);
            image = EmguIntfs.PyrRemoveNoise(image);
            if (saveFile)
            {
                image.Save(Utils.String.FilePostfix(Path, $"-{picId++}-filter"));
            }

            Image<Gray, Byte> _edged;
            if (useCanny)
            {
                _edged = EmguIntfs.Canny(image.Mat.ToImage<Gray, byte>(),
                    param.Canny1Threshold1,
                    param.Canny1Threshold2,
                    param.Canny1ApertureSize,
                    param.Canny1I2Gradient);
                if (saveFile)
                {
                    _edged.Save(Utils.String.FilePostfix(Path, $"-{picId++}-edge"));
                }
            }
            else
            {
                _edged = image.Mat.ToImage<Gray, byte>();
            }


            Circles = CvInvoke.HoughCircles(image.Mat.ToImage<Gray, byte>(), HoughType.Gradient,
                param.Hough1Dp,
                param.Hough1MinDist,
                param.Hough1Param1,
                param.Hough1Param2,
                param.Hough1MinRadius, param.Hough1MaxRadius);
            Circles = Sort(Circles);
            #endregion

            if (saveFile)
            {
                Bitmap circleOnFilter = DrawCircle(new Image<Bgr, byte>(image.Bitmap), Circles.ToList());
                circleOnFilter.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnFilter"));

                Bitmap circleOnEdge = DrawCircle(new Image<Bgr, byte>(_edged.Bitmap), Circles.ToList());
                circleOnEdge.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnEdge"));
            }


            #region filter 86.3%
            FilteredCircles = new List<CircleF>();
            FilteredLights = new List<int>();

            var raw = bigImage.Copy();
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
                if (strength >= 1600)/* filter fake circles */
                {
                    FilteredCircles.Add(circle);

                    int threshold = (int)((double)strength * 0.863);

                    raw.ROI = new Rectangle(startX, startY, len, len);
                    Image<Gray, ushort> oneCircle = EmguIntfs.Binarize(threshold, raw);
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
                _edged = EmguIntfs.Canny(raw.Mat.ToImage<Gray, byte>(),
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
                _edged = raw.Mat.ToImage<Gray, byte>();
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
                    int b = CountPixels(bigImage, circle);
                    brightness.Add(b);
                    _log.Info($"Circle{i:D3}: ({circle.Center.X},{circle.Center.Y}) {circle.Radius} {b}");

                    i++;
                }
            }

            if (saveFile)
            {
                Bitmap circleOnFilter = DrawCircle(new Image<Bgr, byte>(raw.Bitmap), FilteredCircles2nd, param.ShowFirstResult ? FilteredCircles : null);
                circleOnFilter.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnFilter"));

                Bitmap circleOnEdge = DrawCircle(new Image<Bgr, byte>(_edged.Bitmap), FilteredCircles2nd, param.ShowFirstResult ? FilteredCircles : null);
                circleOnEdge.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnEdge"));
            }

            CircleImage ret = new CircleImage();
            ret.Circles = FilteredCircles2nd;
            ret.Brightness = brightness;
            ret.RetImg = DrawCircle(bigImage.Mat.ToImage<Bgr, byte>(), FilteredCircles2nd, param.ShowFirstResult ? FilteredCircles : null);
            ret.RetImg.Save(Utils.String.FilePostfix(Path, $"-{picId++}-result"));

            return ret;
        }
        public override CircleImage FindCircle(string path, Parameters param = null)
        {
            _log.Debug(param.ToString());

            int picId = 0;

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

            Image<Gray, Byte> image = EmguIntfs.Binarize(param.BinThreshold, _grayedUmat);
            image = EmguIntfs.PyrRemoveNoise(image);
            if (saveFile)
            {
                image.Save(Utils.String.FilePostfix(Path, $"-{picId++}-filter"));
            }

            Image<Gray, Byte> _edged;
            if (useCanny)
            {
                _edged = EmguIntfs.Canny(image,
                    param.Canny1Threshold1,
                    param.Canny1Threshold2,
                    param.Canny1ApertureSize,
                    param.Canny1I2Gradient);
                if (saveFile)
                {
                    _edged.Save(Utils.String.FilePostfix(Path, $"-{picId++}-edge"));
                }
            }
            else
            {
                _edged = _grayedUmat;
            }


            Circles = CvInvoke.HoughCircles(image, HoughType.Gradient,
                param.Hough1Dp,
                param.Hough1MinDist,
                param.Hough1Param1,
                param.Hough1Param2,
                param.Hough1MinRadius, param.Hough1MaxRadius);
            Circles = Sort(Circles);

            List<CircleF> FilteredCircles = new List<CircleF>();
            List<int> brightness = new List<int>();

            for(int i = 0; i<Circles.Length; i++)
            {
                CircleF circle = Circles[i];

                int strength = image.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
                if (strength >= 30)/* filter fake circles */
                {
                    int b = CountPixels(_grayedUmat,ref circle, ValidRatio);
                    brightness.Add(b);
#if false
                    /*this value is used to compare with CountPixels*/
                    int s = CountPixelsSquare(_grayedUmat, circle);
                    if(System.Math.Abs((double)(b - s) / b) > 0.001)
                    {
                        _log.Warn($"CountPixels({b}) CountPixelsSquare({s}) diff too much");
                    }
#endif
                    //_log.Info($"Circle{i:D3}: ({circle.Center.X},{circle.Center.Y}) {circle.Radius} {b}");
                    FilteredCircles.Add(circle);
                }
            }
#endregion

            if (saveFile)
            {
                Bitmap circleOnFilter = DrawCircle(new Image<Bgr, byte>(image.Bitmap), Circles.ToList());
                circleOnFilter.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnFilter"));

                Bitmap circleOnEdge = DrawCircle(new Image<Bgr, byte>(_edged.Bitmap), Circles.ToList());
                circleOnEdge.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnEdge"));
            }

#region 2nd find circle
            //#region filter 86.3%
            //FilteredCircles = new List<CircleF>();
            //FilteredLights = new List<int>();
            //var raw = _grayedUmat;
            //foreach (var circle in Circles)
            //{
            //    int extra = param.FilterSquareExtra;

            //    int startX = (int)System.Math.Floor(circle.Center.X - circle.Radius - extra);
            //    int startY = (int)System.Math.Floor(circle.Center.Y - circle.Radius - extra);
            //    int len = (int)System.Math.Ceiling((double)circle.Radius * 2.0) + 2 * extra;
            //    if (startX < 0 || startY < 0)
            //    {
            //        _log.Warn("FilterSizeExtra may be too big, filter abandoned");
            //        continue;
            //    }

            //    int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
            //    if (strength >= 30)/* filter fake circles */
            //    {
            //        FilteredCircles.Add(circle);

            //        int threshold = (int)((double)strength * 0.863);

            //        raw.ROI = new Rectangle(startX, startY, len, len);
            //        Image<Gray, Byte> oneCircle = EmguIntfs.Binarize(threshold, raw);
            //        raw.ROI = Rectangle.Empty;

            //        for (int x = 0; x < len; x++)
            //        {
            //            for (int y = 0; y < len; y++)
            //            {
            //                raw.Data[startY + y, startX + x, 0] = oneCircle.Data[y, x, 0];
            //            }
            //        }
            //    }
            //}
            //if (saveFile)
            //{
            //    raw.Save(Utils.String.FilePostfix(Path, "-2-filter"));
            //}
            //#endregion

            //if (useCanny)
            //{
            //    _edged = EmguIntfs.Canny(raw,
            //        param.Canny2Threshold1,
            //        param.Canny2Threshold2,
            //        param.Canny2ApertureSize,
            //        param.Canny2I2Gradient);
            //    if (saveFile)
            //    {
            //        _edged.Save(Utils.String.FilePostfix(Path, "-3-edge"));
            //    }
            //}
            //else
            //{
            //    _edged = raw;
            //}

            //Circles2nd = CvInvoke.HoughCircles(_edged, HoughType.Gradient,
            //    param.Hough2Dp,
            //    param.Hough2MinDist,
            //    param.Hough2Param1,
            //    param.Hough2Param2,
            //    param.Hough2MinRadius, param.Hough2MaxRadius);
            //Circles2nd = Sort(Circles2nd);
            //FilteredCircles2nd = new List<CircleF>();
            //List<int> brightness = new List<int>();

            //_log.Info($"Circles Information");
            //int i = 0;
            //foreach (var circle in Circles2nd)
            //{
            //    int strength = raw.Data[(int)circle.Center.Y, (int)circle.Center.X, 0];
            //    if (strength > 30)
            //    {
            //        FilteredCircles2nd.Add(circle);
            //        int b = CountPixels(_grayedUmat, circle);
            //        brightness.Add(b);
            //        _log.Info($"Circle{i:D3}: ({circle.Center.X},{circle.Center.Y}) {circle.Radius} {b}");

            //        i++;
            //    }
            //}

            //if (saveFile)
            //{
            //    Bitmap circleOnFilter = DrawCircle(new Image<Bgr, byte>(raw.Bitmap), FilteredCircles2nd,  param.ShowFirstResult ? FilteredCircles : null);
            //    circleOnFilter.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnFilter"));

            //    Bitmap circleOnEdge = DrawCircle(new Image<Bgr, byte>(_edged.Bitmap), FilteredCircles2nd, param.ShowFirstResult ? FilteredCircles : null);
            //    circleOnEdge.Save(Utils.String.FilePostfix(Path, $"-{picId++}-circleOnEdge"));
            //}
#endregion

            CircleImage ret = new CircleImage();
            ret.Circles = FilteredCircles;
            ret.Brightness = brightness;
            ret.RetImg = DrawCircle(RawImg, FilteredCircles, Circles.ToList());
            ret.RetImg.Save(Utils.String.FilePostfix(Path, $"-{picId++}-result"));

            _log.Info($"Uniformity:{CalcUniformity(ret)}");

            if (saveFile)
            {
                Dictionary<string, string> data = new Dictionary<string, string>();

                data["Emitter Count"] = ret.Circles.Count.ToString();
                try
                {
                    data["Emission Uniformity"] = CalcUniformity(ret).ToString("F3");
                }
                catch (Exception ex)
                {
                    data["Emission Uniformity"] = "N/A";
                }
                data["Max Radius"] = ret.Circles.Max(c => c.Radius).ToString("F3");
                data["Min Radius"] = ret.Circles.Min(c => c.Radius).ToString("F3");

                ret.Data = data;
            }

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
                        radius[imgId] = img[imgId].Circles[circleId].Radius * 5.5;
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

            return result.ToList().Select(x => x/10).ToArray();
        }
        private double CalcDivergenceAngle(List<CircleImage> img, double[] weists)
        {/* weist * sita = lambda / pi */
            _log.Info($"DivergenceAngle");

            double[] angles = new double[weists.Length];
            for (int i = 0; i<angles.Length; i++)
            {
                try
                {
                    angles[i] = (0.94 / System.Math.PI / weists[i]) * 180 / System.Math.PI * 2;
                }
                catch (Exception ex)
                {
                    angles[i] = double.NaN;
                }
                _log.Info($"Circle{i:D3}: {angles[i]:F3}");
            }

            return angles.ToList().FindAll(x => !double.IsNaN(x)).ToList().Average();
        }
        private double CalcUniformity(CircleImage img)
        {
            return Utils.Math.StdEv(img.Brightness.Select(x => (double)x).ToList());
        }
        public override Result Calculate(List<CircleImage> img, List<double> distance)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            double[] weists = new double[0];
            if (img.Count > 3)
            {
                weists = CalcWeist(img, distance);
                _log.Info($"Weist");
                for (int i = 0; i < weists.Length; i++)
                {
                    _log.Info($"Circle{i:D3}: {weists[i]:F3}");
                }
            }

            ret["Emitter Count"] = img[0].Circles.Count.ToString();
            //ret["Dead Cluster Count"] = img[0].Circles.Count.ToString();
            try
            {
                ret["Emitter Divergence Angle"] = CalcDivergenceAngle(img, weists).ToString("F3");
            }
            catch (Exception ex)
            {
                ret["Emitter Divergence Angle"] = "N/A";
            }
            try
            {
                ret["Beam Waist Diameter(um)"] = weists.ToList().FindAll(x => 0 < x && x < 100).ToList().Average().ToString("F3");
            }
            catch (Exception ex)
            {
                ret["Beam Waist Diameter(um)"] = "N/A";
            }
            try
            {
                ret["Emission Uniformity"] = CalcUniformity(img[0]).ToString("F3");
            }
            catch (Exception ex)
            {
                ret["Emission Uniformity"] = "N/A";
            }

            return new Result("Ok", null, ret);
        }

        private Bitmap DrawCircle(Image<Bgr, byte> raw, List<CircleF> mainCircle, List<CircleF> auxCircle = null)
        {
            Mat circleImage = raw.Mat;

            if (mainCircle != null)
            {
                for (int i = 0; i < mainCircle.Count; i++)
                {
                    CvInvoke.Circle(circleImage, Point.Round(mainCircle[i].Center), (int)mainCircle[i].Radius, new Bgr(Color.Red).MCvScalar, 1);
                    CvInvoke.PutText(circleImage, i.ToString("D3"), new Point((int)mainCircle[i].Center.X, (int)mainCircle[i].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
                }
            }

            if (auxCircle != null)
            {
                for (int i = 0; i < auxCircle.Count; i++)
                {
                    CvInvoke.Circle(circleImage, Point.Round(auxCircle[i].Center), (int)auxCircle[i].Radius, new Bgr(Color.Yellow).MCvScalar, 1);
                }
            }

            return circleImage.Bitmap;
        }
    }
}