using OpenCvSharp;
using System;
using System.Numerics;

namespace ImageProcessing100.Answers
{
    public static class Answer_015
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");
            var gray = Util.GBRToGray(img);
            var output = PrewittFilter(gray, true);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat PrewittFilter(Mat img, bool isVertical)
        {
            var outMat = Mat.Zeros(img.Rows, img.Height, MatType.CV_8UC1).ToMat();
            var kernel = new int[3, 3];

            if (isVertical)
            {
                kernel[0, 0] = kernel[0, 1] = kernel[0, 2] = 1;
                kernel[2, 0] = kernel[2, 1] = kernel[2, 2] = -1;
            }
            else
            {
                kernel[0, 0] = kernel[1, 0] = kernel[2, 0] = 1;
                kernel[0, 2] = kernel[1, 2] = kernel[2, 2] = -1;
            }

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
