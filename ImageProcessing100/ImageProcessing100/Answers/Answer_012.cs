using OpenCvSharp;
using System;
using System.Numerics;

namespace ImageProcessing100.Answers
{
    public static class Answer_012
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var output = MeanFilter(img, 3);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat MeanFilter(Mat img, int kernelSize)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC3).ToMat();
            var pad = kernelSize / 2;

            for (int y = 0; y < img.Height; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    var sumBGR = Vector3.Zero;
                    for (int i = -pad; i < pad + 1; i++)
                    {
                        if ((x + i < 0) || (y + i < 0)) continue;
                        var pixel = img.GetGenericIndexer<Vec3b>()[y + i, x + i];
                        sumBGR += new Vector3(pixel.Item0, pixel.Item1, pixel.Item2);
                    }
                    sumBGR /= kernelSize;
                    outMat.GetGenericIndexer<Vec3b>()[y, x] =
                        new Vec3b((byte)sumBGR.X, (byte)sumBGR.Y, (byte)sumBGR.Z);
                }
            return outMat;
        }
    }
}
