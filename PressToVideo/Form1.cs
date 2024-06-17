using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        public Form1()
        {
            InitializeComponent();
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

            var texts = GetTextFromPressentation(filePath);

            WriteTextToFiles(texts, $"{programPath}/{ProjectName}/txt/");

            MessageBox.Show("Fineshed!!!");
        }

        private void panel1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
