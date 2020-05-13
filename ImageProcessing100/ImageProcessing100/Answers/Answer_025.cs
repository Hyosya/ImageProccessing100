using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_025
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var output = NearestNeighbor(img, 1.5, 1.5);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("source", img);
            Cv2.ImShow("result", output);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat NearestNeighbor(Mat img, double resizeX, double resizeY)
        {
            static int round(in double value) => (int)Math.Round(value, MidpointRounding.AwayFromZero);
            var outMat = Mat.Zeros(round(img.Rows * resizeX), round(img.Cols * resizeY), img.Type()).ToMat();
            var imgIndexer = img.GetGenericIndexer<Vec3b>();
            unsafe
            {
                outMat.ForEachAsVec3b((value, position) =>
                {
                    var x = round(position[0] / resizeX);
                    var y = round(position[1] / resizeY);
                    *value = imgIndexer[x, y];
                });
            }
            return outMat;
        }
    }
}
