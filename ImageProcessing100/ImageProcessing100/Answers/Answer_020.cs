using OpenCvSharp;
using OpenCvSharp.Extensions;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_020
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori_dark.jpg");

            var hist = Util.MakeHistogram(img);

            //Cv2.ImWrite("out.jpg", output);
            Cv2.ImShow("sample", hist);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
