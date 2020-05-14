using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_027
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var output = Bicubic(img, 1.5, 1.5);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("source", img);
            Cv2.ImShow("result", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat Bicubic(Mat img, double resizeX, double resizeY)
        {
            static int round(in double value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
            var outMat = Mat.Zeros(round(img.Rows * resizeX), round(img.Cols * resizeY), img.Type()).ToMat();
            var imgIndexer = img.GetGenericIndexer<Vec3b>();
            var outIndexer = outMat.GetGenericIndexer<Vec3b>();
            for (int y = 0; y < outMat.Rows; y++)
            {
                var dy = y / resizeY;
                var sourceY = (int)Math.Floor(dy);
                for (int x = 0; x < outMat.Cols; x++)
                {
                    var dx = x / resizeX;
                    var sourceX = (int)Math.Floor(dx);
                    var newPixel = new Vec3b();
                    for (int c = 0; c < outMat.Channels(); c++)
                    {
                        var w_sum = 0d;
                        var val = 0d;
                        for (int j = -1; j < 3; j++)
                        {
                            var _y = Clip(sourceY + j, 0, img.Rows - 1);
                            var wy = CalcWeight(Math.Abs(dy - _y));
                            for (int i = -1; i < 3; i++)
                            {
                                var _x = Clip(sourceX + i, 0, img.Cols - 1);
                                var wx = CalcWeight(Math.Abs(dx - _x));
                                w_sum += wy * wx;
                                val += imgIndexer[_x, _y][c] * wx * wy;
                            }
                        }
                        var channelValue = Clip((int)(val / w_sum), 0, 255);
                        newPixel[c] = (byte)channelValue;
                    }
                    outIndexer[x, y] = newPixel;
                }
            }
            return outMat;
        }

        private static double CalcWeight(double t)
        {
            var a = 01d;
            var tAbs = Math.Abs(t);
            if (Math.Abs(t) <= 1)
            {
                return (a + 2d) * Math.Pow(tAbs, 3) - (a + 3) * Math.Pow(t, 2) + 1;
            }
            else if (tAbs <= 2)
            {
                return a * Math.Pow(tAbs, 3) - 5 * a * Math.Pow(tAbs, 2) + 8 * a * tAbs - 4 * a;
            }
            return 0;
        }

        private static int Clip(int value, int min, int max) => Math.Min(Math.Max(value, min), max);
    }
}
