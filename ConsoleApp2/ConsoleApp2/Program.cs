using System;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
       
        private const string subscriptionkey = "00970617256a4e809a1998702fe09671";
        private const string endpoint = "https://southeastasia.api.cognitive.microsoft.com/";

        private static readonly FaceAttributeType[] faceAttributes = { FaceAttributeType.Age, FaceAttributeType.Gender, FaceAttributeType.Emotion };
        private const string imgfile = "faceimg/2.jpg";

        static async Task Main (string[] args)
        {
            FaceClient faceClient = new FaceClient(new ApiKeyServiceClientCredentials(subscriptionkey),new System.Net.Http.DelegatingHandler[] { });
            faceClient.Endpoint = endpoint;
            Console.WriteLine("臉部辨識中...");
            await DetectLocalAsync(faceClient);
            Console.ReadLine();
        }

        static async Task DetectLocalAsync(FaceClient faceClient)
        {
            if(File.Exists(imgfile))
            {
                try
                {
                    using (Stream imgstream=File.OpenRead(imgfile))
                    {
                        IList<DetectedFace> faceList = await faceClient.Face.DetectWithStreamAsync(imgstream, true, false, faceAttributes);
                        var faceinfo = new StringBuilder();
                        var i = 0;
                        foreach(DetectedFace face in faceList)
                        {
                            i += 1;
                            faceinfo.AppendLine("=============" + i + "===================");
                            faceinfo.AppendLine("Age:" + face.FaceAttributes.Age);
                            faceinfo.AppendLine("Gender:" + face.FaceAttributes.Gender.ToString());
                            faceinfo.AppendLine("Emotion Happiness:" + face.FaceAttributes.Emotion.Happiness.ToString());
                            faceinfo.AppendLine("Left:" + face.FaceRectangle.Left);
                            faceinfo.AppendLine("Top:" + face.FaceRectangle.Top);
                            faceinfo.AppendLine("Width:" + face.FaceRectangle.Width);
                            faceinfo.AppendLine("Height:" + face.FaceRectangle.Height);
                           
                        }
                        Console.WriteLine(faceinfo.ToString());
                    }
                }
                catch(APIErrorException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("No file !!");
            }
        }

       
    }
}
