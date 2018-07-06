
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
        public override CircleImage FindCircle(string path)
        {
            _log.Debug(Utils.String.Flatten(EmguParameters.Item));

            Path = path;
            RawImg = EmguIntfs.Load(path);

            Image<Gray, Byte> _grayedUmat = EmguIntfs.ToImage(EmguIntfs.Grayed(RawImg));

            int multiple = 1;
            UMat pyrDown = new UMat();
            CvInvoke.PyrDown(_grayedUmat, pyrDown);
            multiple *= 2;
            CvInvoke.PyrDown(pyrDown, _grayedUmat);
            multiple *= 2;

            Image<Gray, Byte> _bin = EmguIntfs.Binarize(Int32.Parse(EmguParameters.Item["BinThreshold"]),
                _grayedUmat);

            Image<Gray, Byte> _edged = EmguIntfs.Canny(_bin,
                double.Parse(EmguParameters.Item["Canny1Threshold1"]),
                double.Parse(EmguParameters.Item["Canny1Threshold2"]),
                Int32.Parse(EmguParameters.Item["Canny1ApertureSize"]),
                bool.Parse(EmguParameters.Item["Canny1I2Gradient"]));
            _edged.Save(Utils.String.FilePostfix(Path, "-1-edge"));

            Circles = CvInvoke.HoughCircles(_edged, HoughType.Gradient,
                double.Parse(EmguParameters.Item["Hough1Dp"]),
                Int32.Parse(EmguParameters.Item["Hough1MinDist"]),
                double.Parse(EmguParameters.Item["Hough1Param1"]),
                Int32.Parse(EmguParameters.Item["Hough1Param2"]),
                Int32.Parse(EmguParameters.Item["Hough1MinRadius"]),
                Int32.Parse(EmguParameters.Item["Hough1MaxRadius"]));
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
        public override Result Calculate(List<CircleImage> img, List<double> distance)
        {
            return new Result("Ok");
        }
    }
}