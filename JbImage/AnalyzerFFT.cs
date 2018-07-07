
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

            Path = path;
            RawImg = EmguIntfs.Load(path);

            Image<Gray, Byte> _grayedUmat = EmguIntfs.ToImage(EmguIntfs.Grayed(RawImg));

            int multiple = 1;
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(_grayedUmat, pyrDown);
            multiple *= 2;
            CvInvoke.PyrDown(pyrDown, _grayedUmat);
            multiple *= 2;

            Image<Gray, Byte> _bin = EmguIntfs.Binarize(param.BinThreshold, _grayedUmat);
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

            #region skip
            FilteredCircles = new List<CircleF>();
            FilteredCircles2nd = new List<CircleF>();
            foreach (var c in Circles)
            {
                FilteredCircles.Add(c);
                FilteredCircles2nd.Add(c);
            }
            #endregion

            #region draw
            Mat _result = _edged.Mat;

            for (int i = 0; i < FilteredCircles2nd.Count; i++)
            {
                Point center = Point.Round(FilteredCircles2nd[i].Center);
                center.X *= 1;
                center.Y *= 1;
                int radius = (int)FilteredCircles2nd[i].Radius * 1;

                //if (2 * radius < _result.Size.Height && 2 * radius < _result.Size.Width)
                {
                    CvInvoke.Circle(_result, center, radius, new Bgr(Color.White).MCvScalar, 1);
                    CvInvoke.PutText(_result, i.ToString("D3"), new Point((int)FilteredCircles2nd[i].Center.X, (int)FilteredCircles2nd[i].Center.Y), Emgu.CV.CvEnum.FontFace.HersheyScriptComplex, 1, new MCvScalar(255, 255, 0), 1);
                }
            }
            _result.Save(Utils.String.FilePostfix(Path, "-result"));
            #endregion

            CircleImage ret = new CircleImage();
            ret.Path = Path;
            ret.Circles = FilteredCircles2nd;
            ret.RetImg = _result.Bitmap;

            return ret;
        }
        private double CalcDivergenceAngle(List<CircleImage> img)
        {
            /* arctan(光斑半径 / 芯片到透镜的距离) */
            return double.NaN;
        }
        private double CalcPowerDensity(List<CircleImage> img)
        {
            /* 激光器发光功率 / 光斑面积 */
            CircleImage i = img[0];
            double result = i.Circles[0].Area;
            return double.NaN;
        }
        public override Result Calculate(List<CircleImage> img, List<double> distance)
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();

            ret["Emitter Divergence Angle"] = CalcDivergenceAngle(img).ToString("F3");
            ret["Power Density"] = CalcDivergenceAngle(img).ToString("F3");

            return new Result("Ok", null, ret);
        }
    }
}