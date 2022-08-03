using System;
using System.Text;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;

namespace testLuisSDK
{
    class Program
    {
        const string SubscriptionKey = "d70f6047ee1f4acbadebfefbc45c6dd2";
        const string EndPoint = "https://testalbeeluis20220729-authoring.cognitiveservices.azure.com/";
        const string ApplicationId = "76789f27-81e1-4b0d-a28c-95d080dd77c0";
        static void Main(string[] args)
        {
            while (true)
            {

                // Set console encoding to unicode
                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;

                Console.Write("please input utterance ('exit' for quit) : ");
                var utterance = Console.ReadLine();
                if (utterance == "exit") break;

                var ret = predict(utterance);
                foreach (var item in ret.Prediction.Entities)
                {
                    Console.WriteLine($"Entities : {item.Key} - {item.Value}");
                }
                foreach (var item in ret.Prediction.Intents)
                {
                    Console.WriteLine($"Intents : {item.Key} - {item.Value.Score}");
                }
                Console.WriteLine($"\nTopIntent : {ret.Prediction.TopIntent}");
            }
        }

        static PredictionResponse predict(string utterance)
        {
            var client = new LUISRuntimeClient(new ApiKeyServiceClientCredentials(SubscriptionKey));
            client.Endpoint = EndPoint;
            // Predict
            try
            {
                var predictionRequest = new PredictionRequest
                {
                    Query = utterance
                };

                var result = client.Prediction.GetSlotPredictionAsync(new Guid(ApplicationId), "production", predictionRequest).Result;
                return result;
            }
            catch (Exception)
            {
                Console.WriteLine("\nSomething went wrong. Please Make sure your app is published and try again.\n");
                return null;
            }
        }
    }
}
