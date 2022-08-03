using System;
using Azure;
using Microsoft.Extensions.Configuration;
using System.Text;
using Azure.AI.TextAnalytics;

namespace textananysis
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            Console.Write("請輸入句子:");
            var text = Console.ReadLine();
            var Language = TextAnalytics.GetLanguage(text);
            Console.WriteLine("Get Language:" + Language);
            var ret = TextAnalytics.GetSentiment(text);
            Console.WriteLine("Get Sentiment:" + ret);
            var KeyPhrases = TextAnalytics.ExtractKeyPhrases(text);
            foreach (var item in KeyPhrases.Value)
         {
                Console.WriteLine("KeyPhrases:" + item);
            }
        }
    }
        public class TextAnalytics
    {
        const string cogSvcKey = "3a8ea6dbeda141cda0166ab77527f356";
        const string cogSvcEndpoint = "https://testta20220728albee.cognitiveservices.azure.com/";

        public static string GetLanguage(string text)
        {

            // Create client using endpoint and key
            AzureKeyCredential credentials = new AzureKeyCredential(cogSvcKey);
            Uri endpoint = new Uri(cogSvcEndpoint);
            var client = new TextAnalyticsClient(endpoint, credentials);

            // Call the service to get the detected language
            DetectedLanguage detectedLanguage = client.DetectLanguage(text);
            return (detectedLanguage.Name);

        }

        public static string GetSentiment(string text)
        {
            // Create client using endpoint and key
            AzureKeyCredential credentials = new AzureKeyCredential(cogSvcKey);
            Uri endpoint = new Uri(cogSvcEndpoint);
            var client = new TextAnalyticsClient(endpoint, credentials);

            // Call the service to get the detected language
            var result = client.AnalyzeSentiment(text, "zh");
            return (result.Value.Sentiment.ToString());
        }

        public static Response<KeyPhraseCollection> ExtractKeyPhrases(string text)
        {

            // Create client using endpoint and key
            AzureKeyCredential credentials = new AzureKeyCredential(cogSvcKey);
            Uri endpoint = new Uri(cogSvcEndpoint);
            var client = new TextAnalyticsClient(endpoint, credentials);

            // Call the service to get the detected language
            var KeyPhrases = client.ExtractKeyPhrases(text, "zh");
            return (KeyPhrases);
        }
    }
}