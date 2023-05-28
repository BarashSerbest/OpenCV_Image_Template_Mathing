using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenCvSharp;

// Bu algoritmayı kullanabilmek için projeye "OpenCvSharp4" kütüphanesinin eklenmesi gerekiyor.

namespace OpenCV_Image_Template_Mathing
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath = @"C:\Users\const\Desktop\AlgoritmaAnaliziKS2-Odev2\OpenCV_Image_Template_Mathing\OpenCV_Image_Template_Mathing\ALGORTIMA_ANALIZI_ODEV_SOURCES\ALGORTIMA_ANALIZI_ODEV_SOURCES";
            string[] imageFiles = Directory.GetFiles(folderPath, "*.png");

            List<string> similarImages = FindSimilarImages(imageFiles);

            Console.WriteLine("Birbirine en çok benzeyen görseller:");
            foreach (string imagePath in similarImages)
            {
                Console.WriteLine(imagePath);
            }

            Console.ReadLine();
        }

        static List<string> FindSimilarImages(string[] imageFiles)
        {
            HashSet<string> similarImages = new HashSet<string>();
            Dictionary<string, double> similarities = new Dictionary<string, double>();

            foreach (string imagePath1 in imageFiles)
            {
                Mat template = Cv2.ImRead(imagePath1, ImreadModes.Grayscale);
                double maxSimilarity = 0;
                string mostSimilarImage = "";

                foreach (string imagePath2 in imageFiles)
                {
                    if (imagePath1 == imagePath2)
                        continue;

                    Mat source = Cv2.ImRead(imagePath2, ImreadModes.Grayscale);

                    Mat result = new Mat();
                    Cv2.MatchTemplate(source, template, result, TemplateMatchModes.CCoeffNormed);

                    Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out _);

                    if (maxVal > maxSimilarity)
                    {
                        maxSimilarity = maxVal;
                        mostSimilarImage = imagePath2;
                    }

                    source.Dispose();
                    result.Dispose();
                }

                template.Dispose();

                if (!similarImages.Contains(mostSimilarImage))
                {
                    similarImages.Add(mostSimilarImage);
                    similarities.Add(mostSimilarImage, maxSimilarity);
                }
            }

            // Benzerlik oranlarına göre sıralama
            List<string> sortedImages = similarImages.OrderByDescending(img => similarities[img]).ToList();

            return sortedImages;
        }
    }
}
