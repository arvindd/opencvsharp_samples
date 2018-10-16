using System;
using System.IO;

namespace TutorialCode
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the root directory of the tutorials
            var tutRoot = AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("\\bin")+1);

            // Create output directory images. This is where samples will put their generated images in.
            string imagesDir = $"{tutRoot}images";
            if (!Directory.Exists(imagesDir))
                Directory.CreateDirectory(imagesDir);

            // Now, run the samples. Uncomment the sample to be run.
            ISample sample =
                // new DisplayImages();
                // new LoadModifySave();
                new HowToScanImages();

            sample.Run(args, tutRoot);
        }
    }
}
