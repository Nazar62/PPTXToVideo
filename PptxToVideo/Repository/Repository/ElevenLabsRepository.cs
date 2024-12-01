using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace PptxToVideo.Repository.Repository
{
    public class ElevenLabsRepository
    {
        private const int CHUNK_SIZE = 1024;
        string programPath = Directory.GetCurrentDirectory();

        public async Task<bool> IsApiKeyCorrect(string apiKey)
        {
            // Construct the URL for the Text-to-Speech API request
            string ttsUrl = $"https://api.elevenlabs.io/v1/user";

            // Set up headers for the API request, including the API key for authentication
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

            var response = client.GetAsync(ttsUrl).Result;

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> TTS(string text, string outputPath, string voice, string apiKey)
        {
            string ttsUrl = $"https://api.elevenlabs.io/v1/text-to-speech/{voice}/stream";

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

            var data = new
            {
                text = text,
                model_id = "eleven_multilingual_v2",
                voice_settings = new
                {
                    stability = 0.5,
                    similarity_boost = 0.8,
                    style = 0.0,
                    use_speaker_boost = true
                }
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(ttsUrl, content);

            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[CHUNK_SIZE];
                    int read;
                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        fileStream.Write(buffer, 0, read);
                    }
                }
                return true;
            }
            else
            {
                new CustomMessageBox(await response.Content.ReadAsStringAsync()).ShowDialog();
                return false;
            }
            //Task.Delay(1000);
        }

        public async Task<bool> TTSAll(string ProjectName, int slidesCount, List<string> slidesText, string voice, string apiKey)
        {
            if (!Directory.Exists($"{programPath}/{ProjectName}/wavs/"))
            {
                Directory.CreateDirectory($"{programPath}/{ProjectName}/wavs/");
            }
            for (int i = 0; i < slidesCount; i++)
            {
                if (slidesText[i].Trim() != string.Empty)
                {
                    var responce = await TTS(slidesText[i], $"{programPath}/{ProjectName}/wavs/{i}.mp3", voice, apiKey);
                    if (responce == false)
                    {
                        return false;
                        break;
                    }
                }
            }
            return true;
        }

        public async Task<List<Voice>> GetVoices()
        {
            string url = "https://api.elevenlabs.io/v1/voices";
            List<Voice> voices = new List<Voice>();

            using (var client = new HttpClient())
            {

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var voicesData = JObject.Parse(content);

                    var voicesD = new List<Voice>();

                    foreach (var voice in voicesData["voices"])
                    {
                        voicesD.Add(new Voice()
                        {
                            voice_id = voice["voice_id"].ToString(),
                            name = voice["name"].ToString()
                        });
                    }

                    voices = voicesD.OrderBy(x => x.name).ToList();

                    return voices;

                }
            }
            return voices;
        }

        public async Task<int> GetCharterStats(string apiKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

                string url = "https://api.elevenlabs.io/v1/user/subscription";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(responseBody);

                int characterCount = (int)json["character_count"];
                int characterLimit = (int)json["character_limit"];
                int remainingCredits = characterLimit - characterCount;

                return remainingCredits;
            }
        }
    }
}
