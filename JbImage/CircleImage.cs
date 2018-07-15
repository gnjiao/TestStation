using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace JbImage
{
    public class CircleImage
    {
        /* NFT */
        public List<CircleF> Circles;
        public List<int> Brightness;

        /* FFT */
        public RotatedRect Ellipse;
        public List<Point> Rect;

        /* Common */
        public Bitmap RetImg;
    }
}