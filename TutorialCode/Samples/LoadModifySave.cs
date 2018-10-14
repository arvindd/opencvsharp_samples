using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace TutorialCode
{
    /// <summary>
    /// Load, modify and save images.
    /// <para>
    /// https://docs.opencv.org/master/db/d64/tutorial_load_save_image.html
    /// </para>
    /// </summary>
    class LoadModifySave : ISample
    {
        public void Run(string[] args, string tutRoot)
        {
            // Output Image path
            string imagesDir = $"{tutRoot}images";

            // Default input image
            string imageName = $"{tutRoot}data\\butterfly.jpg";

            if (args.Length > 1)
            {
                imageName = args[0];
            }

            Mat image = new Mat(imageName, ImreadModes.Color);

            if (image.Data == IntPtr.Zero)
            {
                Console.WriteLine("No image data");
                Cv2.WaitKey();
                return;
            }

            Mat gray_image = image.CvtColor(ColorConversionCodes.BGR2GRAY);
            gray_image.ImWrite($"{imagesDir}\\Gray_Image.jpg");

            new Window(imageName, WindowMode.AutoSize, image);
            new Window("Gray Image", WindowMode.AutoSize, gray_image);

            Cv2.WaitKey();
        }
    }
}
