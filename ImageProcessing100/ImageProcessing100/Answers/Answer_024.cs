using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_024
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_gamma.jpg");

            var output = GammaCorrection(img);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat GammaCorrection(Mat img, double c = 1d, double g = 2.2d)
        {
            var outMat = Mat.Zeros(img.Rows, img.Cols, img.Type()).ToMat();
            var outIndexer = outMat.GetGenericIndexer<Vec3b>();
            unsafe
            {
                img.ForEachAsVec3b((value, position) =>
                {
                    byte correction(in byte value) => (byte)(Math.Pow(value / 255d / c, 1 / g) * 255);
                    var newValue = new Vec3b()
                    {
                        Item0 = correction(value->Item0),
                        Item1 = correction(value->Item1),
                        Item2 = correction(value->Item2),
                    };
                    outIndexer[position[0], position[1]] = newValue;
                });
            }
            return outMat;
        }
    }
}
