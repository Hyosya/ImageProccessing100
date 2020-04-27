using OpenCvSharp;
using System;
using System.Numerics;

namespace ImageProcessing100.Answers
{
    public static class Answer_017
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");
            var gray = Util.GBRToGray(img);
            var output = LaplacianFilter(gray);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat LaplacianFilter(Mat img)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC1).ToMat();
            var kernel = new int[3, 3]
            {
                    { 0, 1, 0},
                    { 1, -4, 1},
                    { 0, 1, 0},
            };

            var imgIndexer = img.GetGenericIndexer<byte>();
            var outIndexer = outMat.GetGenericIndexer<byte>();
            for (int y = 0; y < img.Height; y++)
                for (int x = 0; x < img.Width; x++)
                {
                    var v = 0;
                    for (int dy = -1; dy < 2; dy++)
                        for (int dx = -1; dx < 2; dx++)
                        {
                            if ((x + dx < 0) || (y + dy < 0) || (x + dx > img.Height) || (y + dy > img.Width)) continue;
                            var pixel = imgIndexer[y + dy, x + dx];
                            v += pixel * kernel[dy + 1, dx + 1];
                        }
                    outIndexer[y, x] = (byte)Math.Max(Math.Min(v, 255), 0);
                }
            return outMat;
        }
    }
}
