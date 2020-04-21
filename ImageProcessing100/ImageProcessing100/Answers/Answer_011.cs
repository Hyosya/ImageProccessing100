using OpenCvSharp;
using System;
using System.Numerics;

namespace ImageProcessing100.Answers
{
    public static class Answer_011
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
                    for (int dy = -pad; dy < pad + 1; dy++)
                        for (int dx = -pad; dx < pad + 1; dx++)
                        {
                            if ((x + dx < 0) || (y + dy < 0)) continue;
                            var pixel = img.GetGenericIndexer<Vec3b>()[y + dy, x + dx];
                            sumBGR += new Vector3(pixel.Item0, pixel.Item1, pixel.Item2);
                        }
                    sumBGR /= kernelSize * kernelSize;
                    outMat.GetGenericIndexer<Vec3b>()[y, x] =
                        new Vec3b((byte)sumBGR.X, (byte)sumBGR.Y, (byte)sumBGR.Z);
                }
            return outMat;
        }
    }
}
