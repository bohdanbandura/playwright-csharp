using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

namespace PlaywrighNunit.Helpers
{
    class ImageComparer
    {
        public static void CompareImages(string imagePath1, string imagePath2, string diffImagePath)
        {
            using (var img1 = Image.Load<Rgba32>(imagePath1))
            using (var img2 = Image.Load<Rgba32>(imagePath2))
            using (var diffImage = new Image<Rgba32>(img1.Width, img1.Height))
            {
                if (img1.Width != img2.Width || img1.Height != img2.Height)
                {
                    Console.WriteLine("Images dimensions do not match.");
                    return;
                }

                bool imagesAreIdentical = true;

                for (int y = 0; y < img1.Height; y++)
                {
                    for (int x = 0; x < img1.Width; x++)
                    {
                        if (img1[x, y] != img2[x, y])
                        {
                            imagesAreIdentical = false;
                            diffImage[x, y] = new Rgba32(255, 0, 0); // Highlight differences in red
                        }
                        else
                        {
                            diffImage[x, y] = img1[x, y];
                        }
                    }
                }

                if (imagesAreIdentical)
                {
                    Console.WriteLine("Images are identical.");
                }
                else
                {
                    if(!Directory.Exists("../../../diffScreenshots"))
                    {
                        Directory.CreateDirectory("../../../diffScreenshots");
                    }
                    Console.WriteLine($"Images differ. Diff image saved to {diffImagePath}");
                    diffImage.Save(diffImagePath);
                }
            }
        }
    }
}
