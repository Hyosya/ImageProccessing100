using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    class Answer_026
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var output = Bilinear(img, 1.5, 1.5);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("source", img);
            Cv2.ImShow("result", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat Bilinear(Mat img, double resizeX, double resizeY)
        {
            static int round(in double value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
            var outMat = Mat.Zeros(round(img.Rows * resizeX), round(img.Cols * resizeY), img.Type()).ToMat();
            var imgIndexer = img.GetGenericIndexer<Vec3b>();
            unsafe
            {
                outMat.ForEachAsVec3b((value, position) =>
                {
                    var sourceX = Math.Min(round(position[0] / resizeX), img.Cols - 1);
                    var sourceY = Math.Min(round(position[1] / resizeY), img.Rows - 1);
                    var dx = position[0] / resizeX - sourceX;
                    var dy = position[1] / resizeY - sourceY;
                    var newValue = new Vec3b();
                    for (int i = 0; i < 3; i++)
                    {
                        var channelValue = (1d - dx) * (1d - dy) * imgIndexer[sourceX, sourceY][i]
                        + dx * (1d - dy) * imgIndexer[sourceX + 1, sourceY][i]
                        + (1d - dx) * dy * imgIndexer[sourceX, sourceY + 1][i]
                        + dx * dy * imgIndexer[sourceX + 1, sourceY + 1][i];
                        newValue[i] = (byte)Math.Min(channelValue, byte.MaxValue);
                    }
                    *value = newValue;
                });
            }
            //var outIndexer = outMat.GetGenericIndexer<Vec3b>();
            //for (int y = 0; y < outMat.Rows; y++)
            //{
            //    var sourceY = Math.Min(round(y / resizeY), img.Rows - 1);
            //    var dy = y / resizeY - sourceY;
            //    for (int x = 0; x < outMat.Cols; x++)
            //    {
            //        var sourceX = Math.Min(round(x / resizeX), img.Cols - 1);
            //        var dx = x / resizeX - sourceX;
            //        var newValue = new Vec3b();
            //        for (int i = 0; i < 3; i++)
            //        {
            //            var channelValue = (1d - dx) * (1d - dy) * imgIndexer[sourceX, sourceY][i]
            //            + dx * (1d - dy) * imgIndexer[sourceX + 1, sourceY][i]
            //            + (1d - dx) * dy * imgIndexer[sourceX, sourceY + 1][i]
            //            + dx * dy * imgIndexer[sourceX + 1, sourceY + 1][i];
            //            newValue[i] = (byte)Math.Min(channelValue, byte.MaxValue);
            //        }
            //        outIndexer[x, y] = newValue;
            //    }
            //}

            return outMat;

        }
    }
}
