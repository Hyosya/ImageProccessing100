using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_030
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");

            var answer1 = Affine(img, 1d, 0d, 0d, 1d, 0d, 0d, -30d);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("answer1", answer1);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }

        private static Mat Affine(Mat img, double a, double b, double c, double d, double tx, double ty, double theta)
        {
            // get detriment
            var det = a * d - b * c;
            var resized_width = (int)(img.Width * a);
            var resized_height = (int)(img.Height * d);

            if (theta != 0)
            {
                var rad = theta / 180d * Math.PI;
                a = Math.Cos(rad);
                b = -Math.Sin(rad);
                c = Math.Sin(rad);
                d = Math.Cos(rad);

                det = a * d - b * c;

                // center transition
                var cx = img.Width / 2d;
                var cy = img.Height / 2d;
                var new_cx = (d * cx - b * cy) / det;
                var new_cy = (-c * cx + a * cy) / det;
                tx = new_cx - cx;
                ty = new_cy - cy;

                resized_width = img.Width;
                resized_height = img.Height;
            }

            var outMat = Mat.Zeros(resized_height, resized_width, img.Type()).ToMat();

            var outIndexer = outMat.GetGenericIndexer<Vec3b>();
            var imgIndexer = img.GetGenericIndexer<Vec3b>();
            for (int y = 0; y < resized_height; y++)
                for (int x = 0; x < resized_width; x++)
                {
                    // get original position x
                    var x_before = (int)((d * x - b * y) / det - tx);

                    if ((x_before < 0) || (x_before >= img.Width)) continue;

                    // get original position y
                    var y_before = (int)((-c * x + a * y) / det - ty);
                    if ((y_before < 0) || (y_before >= img.Height)) continue;

                    outIndexer[y, x] = imgIndexer[y_before, x_before];
                }
            return outMat;
        }
    }
}
