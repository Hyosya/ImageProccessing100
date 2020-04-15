using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessing100.Answers
{
    public static class Answer_2
    {
        public static void Solve()
        {
            var img = Cv2.ImRead("imori.jpg");
            var outImg = Util.GBRToGray(img);

            //Cv2.ImWrite("out.jpg", img);
            Cv2.ImShow("sample", outImg);
            Cv2.WaitKey(0);
            Cv2.DestroyAllWindows();
        }
    }
}
