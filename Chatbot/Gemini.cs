using ChatAIze.GenerativeCS.Clients;
using ChatAIze.GenerativeCS.Models;
using ChatAIze.GenerativeCS.Options.Gemini;


namespace TasteFinder.Chatbot
{
    public class Gemini
    {
        private readonly GeminiClient client;
        private readonly string apiKey;
        public Gemini()
        {
            apiKey = Environment.GetEnvironmentVariable("gemini-api-key");
            client = new GeminiClient(apiKey);
        }


        public Gemini(ChatCompletionOptions options)
        {
            apiKey = Environment.GetEnvironmentVariable("gemini-api-key");
            client = new GeminiClient(apiKey, options);
        }


        public async Task<bool> CheckNonOffensive(string text)
        {
            

            try
            {
                string response = await client.CompleteAsync($"Check if this review is offensive (Respond with Just YES or NO): {text}");
                if (response.ToLower().Contains("no"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> CheckNegative(string text)
        {
            

            try
            {
                string response = await client.CompleteAsync($"Check if this review is negative (Respond with Just YES or NO): {text}");
                if (response.ToLower().Contains("no"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

        }

        public async Task<string> Getresponse(string text)
        {
            string response = await client.CompleteAsync($"{text}");
            return response;
        }

        public async Task<string> Getresponse(ChatConversation conversation)
        {
            string response = await client.CompleteAsync(conversation);
            return response;
        }
    }
}
