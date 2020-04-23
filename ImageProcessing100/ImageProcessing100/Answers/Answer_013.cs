using OpenCvSharp;
using System;
using System.Numerics;

namespace ImageProcessing100.Answers
{
    public static class Answer_013
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");
            var gray = Util.GBRToGray(img);
            var output = MaxMinFilter(gray, 3);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat MaxMinFilter(Mat img, int kernelSize)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC1).ToMat();
            var pad = kernelSize / 2;
            var imgIndexer = img.GetGenericIndexer<byte>();
            var outIndexer = outMat.GetGenericIndexer<byte>();
            for (int y = 0; y < img.Height; y++)
                for (int x = 0; x < img.Cols; x++)
                {
                    var maxPixel = byte.MinValue;
                    var minPixel = byte.MaxValue;

                    for (int dy = -pad; dy < pad + 1; dy++)
                        for (int dx = -pad; dx < pad + 1; dx++)
                        {
                            if ((x + dx < 0) || (y + dy < 0) || (x + dx > img.Cols) || (y + dy > img.Height)) continue;
                            var pixel = imgIndexer[y + dy, x + dx];
                            maxPixel = Math.Max(pixel, maxPixel);
                            minPixel = Math.Min(pixel, minPixel);
                        }
                    outIndexer[y, x] = (byte)(maxPixel - minPixel);
                }
            return outMat;
        }
    }
}
