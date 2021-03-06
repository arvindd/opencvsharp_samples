﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenCvSharp;

namespace TutorialCode
{
    /// <summary>
    /// Load and display images
    /// <para>
    /// https://docs.opencv.org/master/db/deb/tutorial_display_image.html
    /// </para>
    /// </summary>
    class DisplayImages : ISample
    {
        public void Run(string[] args, string tutRoot)
        {
            // Default input image
            string imageName = $"{tutRoot}data\\HappyFish.jpg";

            if (args.Length > 1)
            {
                imageName = args[0];
            }

            Mat image = new Mat(imageName, ImreadModes.Color);

            if (image.Empty())
            {
                Console.WriteLine("Could not open or find the image");
                return;
            }

            new Window("Display window", WindowMode.AutoSize, image);

            Cv2.WaitKey();
        }
    }
}
