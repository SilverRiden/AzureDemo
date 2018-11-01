using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputerVision
{
    class Program
    {
        private const string subscriptionkey = "0fbf485ff73844e3a7a122ab85658c30";
        private const string endpoint = "https://southeastasia.api.cognitive.microsoft.com";
        private const string imgfile = "imgs/3.jpg";
        private static readonly List<VisualFeatureTypes> features = new List<VisualFeatureTypes>()
        {
            VisualFeatureTypes.Categories, VisualFeatureTypes.Description,
            VisualFeatureTypes.Faces, VisualFeatureTypes.ImageType,
            VisualFeatureTypes.Tags, VisualFeatureTypes.Adult
        };

        static async Task Main(string[] args)
        {
            ComputerVisionClient computerClient = new ComputerVisionClient(new ApiKeyServiceClientCredentials(subscriptionkey), new System.Net.Http.DelegatingHandler[] { });
            computerClient.Endpoint = endpoint;
            Console.WriteLine("影像辨識中...");
            await AnalyzeAsync(computerClient);
            Console.ReadLine();
        }

        static async Task AnalyzeAsync(ComputerVisionClient computerClient)
        {
            if(File.Exists(imgfile))
            {
                try
                {
                    using (Stream imgstream = File.OpenRead(imgfile))
                    {
                        ImageAnalysis analysis = await computerClient.AnalyzeImageInStreamAsync(imgstream,features);
                        var info = analysis.Description.Captions[0].Text + System.Environment.NewLine;

                        foreach (var item in analysis.Tags)
                        {
                            info += $"({item.Name}) \n";
                        }

                        info += "is AdultContent : " + analysis.Adult.IsAdultContent;
                        Console.WriteLine(info + "\n");
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                Console.WriteLine("No File !!");
            }
        }
    
    }
}

