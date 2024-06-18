using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace PressToVideo
{
    public partial class Form1 : Form
    {
        string filePath;
        string ProjectName;
        string programPath = Directory.GetCurrentDirectory();
        List<Voice> voices;
        Voice selectedVoice;
        int slidesCount;
        int txtsCount;
        List<string> slidesText = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private const int CHUNK_SIZE = 1024; // Size of chunks to read/write at a time
        private static readonly string XI_API_KEY = "d022dc9bd6e97b18bbc7bca0fbf6b1d7"; // Your API key for authentication
        //private static readonly string VOICE_ID = "21m00Tcm4TlvDq8ikWAM"; // ID of the voice model to use
        //private static readonly string TEXT_TO_SPEAK = "Привіт це Eleven labs API"; // Text you want to convert to speech
        //private static readonly string OUTPUT_PATH = "output.mp3"; // Path to save the output audio file

        private async Task TTSAll()
        {
            if(!Directory.Exists($"{programPath}/{ProjectName}/wavs/"))
            {
                Directory.CreateDirectory($"{programPath}/{ProjectName}/wavs/");
            }
            for(int i = 0; i < slidesCount; i++)
            {
                if (slidesText[i].Trim() != string.Empty)
                {
                    await TTS(slidesText[i], $"{programPath}/{ProjectName}/wavs/{i}.wav");
                }
            }
        }

        async Task TTS(string text, string outputPath)
        {
            // Construct the URL for the Text-to-Speech API request
            string ttsUrl = $"https://api.elevenlabs.io/v1/text-to-speech/{selectedVoice.voice_id}/stream";

            // Set up headers for the API request, including the API key for authentication
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("xi-api-key", XI_API_KEY);

            // Set up the data payload for the API request, including the text and voice settings
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

            // Make the POST request to the TTS API with headers and data, enabling streaming response
            var response = await client.PostAsync(ttsUrl, content);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response in chunks and write to the file
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
                // Inform the user of success
                //Console.WriteLine("Audio stream saved successfully.");
            }
            else
            {
                // Print the error message if the request was not successful
                MessageBox.Show(await response.Content.ReadAsStringAsync());
            }
            Task.Delay(1000);
        }

        private void WriteTextToFiles(List<string> text, string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            int y = 0;

            for(int i = 0; i < text.Count; i++)
            {
                if (text[i].Trim() != string.Empty)
                {
                    string txtPath = path + $"{y}.txt";
                    File.WriteAllText(txtPath, text[i]);
                    y++;
                }
            }
            txtsCount = y;

        }

        private List<string> GetTextFromPressentation(string path)
        {
            Application pptApplication = new Application();
            Presentation pptPresentation = pptApplication.Presentations.Open(filePath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);

            List<string> PresTexts = new List<string>();
            foreach (Slide slide in pptPresentation.Slides)
            {
                string text = "";
                foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
                {
                    if (shape.HasTextFrame == MsoTriState.msoTrue)
                    {
                        if (shape.TextFrame.HasText == MsoTriState.msoTrue)
                        {
                            var textRange = shape.TextFrame.TextRange;
                            text += textRange.Text + "\n"; // Or update a control on your form
                        }
                    }
                }
                //MessageBox.Show(text);
                PresTexts.Add(text);
            }

            slidesCount = PresTexts.Count;
            slidesText = PresTexts;
            //WriteTextToFiles(PresTexts, $"{programPath}/{ProjectName}/txt/");

            pptPresentation.Close();
            pptApplication.Quit();
            return PresTexts;
        }

        private async void GetVoices()
        {
            //string apiKey = "YOUR_API_KEY"; // Replace with your actual API key
            string url = "https://api.elevenlabs.io/v1/voices";

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var voicesData = JObject.Parse(content);

                    var voicesD = new List<Voice>();

                    foreach(var voice in voicesData["voices"])
                    {
                        voicesD.Add(new Voice()
                        {
                            voice_id = voice["voice_id"].ToString(),
                            name = voice["name"].ToString()
                        });
                    }

                    voices = voicesD.OrderBy(x => x.name).ToList();

                    comboBoxVoices.DataSource = voices;
                    comboBoxVoices.DisplayMember = "name";
                }
            }
        }

        public class Voice
        {
            public string voice_id {  get; set; }
            public string name { get; set; }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                GetVoices();
            } catch
            {

            }
        }

        private void panel1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            filePath = files[0];
            ProjectName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            if(!Directory.Exists($"{programPath}/{ProjectName}/"))
            {
                Directory.CreateDirectory($"{programPath}/{ProjectName}/");
            }

            buttonOpenInExplorer.Visible = true;
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void comboBoxVoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedVoice = comboBoxVoices.SelectedItem as Voice;
        }

        private void buttonStartConvert_Click(object sender, EventArgs e)
        {
            var texts = GetTextFromPressentation(filePath);

            //WriteTextToFiles(texts, $"{programPath}/{ProjectName}/txt/");

            var choice = MessageBox.Show("Want to check the correctness of the text for voice-over?", "Question", MessageBoxButtons.YesNo);

            if(choice == DialogResult.Yes)
            {
                //System.Diagnostics.Process.Start("explorer.exe", "/select," + $"{programPath}/{ProjectName}/txt/".Replace('/', '\\'));
                panelTextEdit.Visible = true;
                labelCount.Text = $"1/{slidesCount}";
                textBoxSlideText.Text = slidesText[0];
            }

            //MessageBox.Show("Fineshed!!!");
        }

        private void buttonOpenInExplorer_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "/select," + $"{programPath}/{ProjectName}/".Replace('/', '\\'));
        }

        private void labelCount_Click(object sender, EventArgs e)
        {

        }

        int currentTxt = 0;

        private void NextTxt()
        {
            if(currentTxt + 1 != slidesCount)
            {
                slidesText[currentTxt] = textBoxSlideText.Text;
                currentTxt++;
                textBoxSlideText.Text = slidesText[currentTxt];
                labelCount.Text = $"{currentTxt + 1}/{slidesCount}";
            } else
            {
                panelTextEdit.Visible = false;
                MessageBox.Show("All Text Edited");
                currentTxt = 0;
                TTSAll();
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            NextTxt();
        }

        private void buttonSkipAll_Click(object sender, EventArgs e)
        {
            panelTextEdit.Visible = false;
            currentTxt = 0;
            TTSAll();
        }
    }
}
