using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Util
    {
        public static Mat SwapRchAndBch(Mat mat)
        {
            if (mat.Channels() != 3) throw new ArgumentException();
            var newMat = Mat.Zeros(mat.Rows, mat.Cols, MatType.CV_8UC3).ToMat();
            unsafe
            {
                mat.ForEachAsVec3b((value, position) =>
                {
                    var newPixel = (Vec3b*)newMat.Ptr(position[0], position[1]);
                    newPixel->Item0 = value->Item2;
                    newPixel->Item1 = value->Item1;
                    newPixel->Item2 = value->Item0;
                });
            }
            return newMat;
        }

        public static Mat GBRToGray(Mat mat)
        {
            if (mat.Channels() != 3) throw new ArgumentException();
            var newMat = Mat.Zeros(mat.Rows, mat.Cols, MatType.CV_8UC1).ToMat();
            unsafe
            {
                mat.ForEachAsVec3b((value, position) =>
                {
                    var newPixel = (byte*)newMat.Ptr(position[0], position[1]);
                    *newPixel = (byte)(0.2126f * value->Item2 + 0.7152f * value->Item1 + 0.0722f * value->Item0);
                });
            }
            return newMat;
        }
    }
}
