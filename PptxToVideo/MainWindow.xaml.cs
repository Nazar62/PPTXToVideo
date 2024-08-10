using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Win32;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Application = Microsoft.Office.Interop.PowerPoint.Application;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;
using Shapes = Microsoft.Office.Interop.PowerPoint.Shapes;

namespace PptxToVideo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string filePath;
        string ProjectName;
        string programPath = Directory.GetCurrentDirectory();
        Voice selectedVoice;
        int slidesCount;
        int txtsCount;
        List<string> slidesText = new List<string>();
        Settings appSettings;
        private const int CHUNK_SIZE = 1024;
        private static string XI_API_KEY;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;


            if (File.Exists($"{programPath}/settings.json"))
            {
                appSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText($"{programPath}/settings.json"));
            }
        }

        public List<Voice> voices = new List<Voice>();

        private List<string> GetTextFromPressentation(string path)
        {
            Application pptApplication = new Application();
            Presentation pptPresentation = pptApplication.Presentations.Open(filePath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);

            List<string> PresTexts = new List<string>();
            foreach (Slide slide in pptPresentation.Slides)
            {
                string text = "";
                foreach (Shape shape in slide.Shapes)
                {
                    if (shape.HasTextFrame == MsoTriState.msoTrue)
                    {
                        if (shape.TextFrame.HasText == MsoTriState.msoTrue)
                        {
                            var textRange = shape.TextFrame.TextRange;
                            text += textRange.Text + ".\n"; // Or update a control on your form
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

                    foreach (var voice in voicesData["voices"])
                    {
                        voicesD.Add(new Voice()
                        {
                            voice_id = voice["voice_id"].ToString(),
                            name = voice["name"].ToString()
                        });
                    }

                    voices = voicesD.OrderBy(x => x.name).ToList();

                    comboBoxVoices.ItemsSource = voices;
                    comboBoxVoices.DisplayMemberPath = "name";

                    comboBoxVoices.SelectedIndex = 0;

                    if (appSettings != null)
                    {
                        if (voices.Any(x => x.name == appSettings.SelectedVoice.name))
                        {
                            var voice = voices.Where(x => x.name == appSettings.SelectedVoice.name).FirstOrDefault();
                            var voiceIndex = voices.IndexOf(voice);
                            comboBoxVoices.SelectedIndex = voiceIndex;
                            //comboBoxVoices.SelectedItem = appSettings.SelectedVoice;
                        }
                    }
                }
            }
        }

        private void CloseBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            CloseBtn.Background = new SolidColorBrush(Colors.DarkRed);
        }

        private void CloseBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            CloseBtn.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
             App.Current.Shutdown();
        }

        private void MoveForm_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    App.Current.MainWindow.DragMove();
                }
            }
        }
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void DragRectangle_Drop(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (Path.GetExtension(files[0]) == ".pptx")
                {
                    FileInfo fileInfo = new FileInfo(files[0]);
                    string fileName = fileInfo.Name;
                    filePath = files[0];
                    string fileData = $"File Name: {fileInfo.Name.Split('.')[0]} \n" +
                    $"Size (bytes): {fileInfo.Length} \n" +
                    $"Created: {fileInfo.CreationTime} \n" +
                    $"Last Edited: {fileInfo.LastWriteTime}  \n"+
                    $"Owner: {GetFileOwner(files[0]).Split('\\')[1]}";

                    FileData.Text = fileData;
                    DropLabel.Visibility = Visibility.Hidden;
                    DragRectangle.Visibility = Visibility.Hidden;
                    progressBar.Visibility = Visibility.Visible;
                    progressLabel.Visibility = Visibility.Visible;
                    progressBar.Value = 0;
                    ProjectName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                    if (!Directory.Exists($"{programPath}/{ProjectName}/"))
                    {
                        Directory.CreateDirectory($"{programPath}/{ProjectName}/");
                    }
                } else
                {
                    new CustomMessageBox("Drop pptx presentation").ShowDialog();
                }
            }
        }

        private void DragRectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "PowerPoint files (*.pptx)|*.pptx";
            fileDialog.ShowDialog();
            string path = fileDialog.FileName;
                FileInfo fileInfo = new FileInfo(path);
                string fileName = fileInfo.Name;
                filePath = path;
                string fileData = $"File Name: {fileInfo.Name.Split('.')[0]} \n" +
                $"Size (bytes): {fileInfo.Length} \n" +
                $"Created: {fileInfo.CreationTime} \n" +
                $"Last Edited: {fileInfo.LastWriteTime}  \n" +
                $"Owner: {GetFileOwner(fileDialog.FileName).Split('\\')[1]}";

                FileData.Text = fileData;
                DropLabel.Visibility = Visibility.Hidden;
                DragRectangle.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility.Visible;
            progressLabel.Visibility = Visibility.Visible;
            progressBar.Value = 0;
            ProjectName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                if (!Directory.Exists($"{programPath}/{ProjectName}/"))
                {
                    Directory.CreateDirectory($"{programPath}/{ProjectName}/");
                }
        }

        static string GetFileOwner(string path)
        {
            FileSecurity fileSecurity = new FileSecurity(path, AccessControlSections.Owner);
            var owner = fileSecurity.GetOwner(typeof(System.Security.Principal.NTAccount));
            return owner.ToString();
        }

        private void buttonStartConvert_Click(object sender, RoutedEventArgs e)
        {
            //var isApiCorrect = await IsApiKeyCorrect(XI_API_KEY);
            if (filePath == null)
            {
                new CustomMessageBox("No File selected! Please drag and drop file to Drag and Drop arrea in programm", "No file").ShowDialog();
            } else if(textBoxElevenLabsAPIKey.Text.Trim() == "")
            {
                new CustomMessageBox("Api key is null or whitespace", "Api key not correct").ShowDialog();
            } else if (!(IsApiKeyCorrect(XI_API_KEY).Result))
            {
                new CustomMessageBox("Api key is uncorrect", "Api key not correct").ShowDialog();
            } else
            {
                progressLabel.Content = "Getting Text For Voice-Over";
                var texts = GetTextFromPressentation(filePath);
                progressBar.Value = 20;
                //WriteTextToFiles(texts, $"{programPath}/{ProjectName}/txt/");

                var choice = new CustomMessageBox("Want to check the correctness of the text for voice-over?", "Question", CustomMessageBox.MessageButtons.YesNo).ShowDialog();

                if (choice.Value == true)
                {
                    //System.Diagnostics.Process.Start("explorer.exe", "/select," + $"{programPath}/{ProjectName}/txt/".Replace('/', '\\'));
                    progressLabel.Content = "Editing Text For Voice-Over";
                    panelTextEdit.Visibility = Visibility.Visible;
                    labelCount.Text = $"1/{slidesCount}";
                    textBoxSlideText.Text = slidesText[0];
                }
                else
                {
                    ContinueConvert();
                }
            }
        }

        int currentTxt = 0;

        private void NextTxt()
        {
            if (currentTxt + 1 != slidesCount)
            {
                slidesText[currentTxt] = textBoxSlideText.Text;
                currentTxt++;
                textBoxSlideText.Text = slidesText[currentTxt];
                labelCount.Text = $"{currentTxt + 1}/{slidesCount}";
            }
            else
            {
                panelTextEdit.Visibility = Visibility.Hidden;
                progressLabel.Content = "All Text Edited";
                progressBar.Value += 20;
                currentTxt = 0;
                ContinueConvert();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetVoices();
                textBoxElevenLabsAPIKey.Text = appSettings.ElevenLabsAPIKey;
            }
            catch
            {

            }
        }

        private void comboBoxVoices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedVoice = comboBoxVoices.SelectedItem as Voice;
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            NextTxt();
        }

        private void buttonSkipAll_Click(object sender, RoutedEventArgs e)
        {
            panelTextEdit.Visibility = Visibility.Hidden;
            currentTxt = 0;
            ContinueConvert();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings settings = new Settings()
            {
                ElevenLabsAPIKey = textBoxElevenLabsAPIKey.Text,
                SelectedVoice = selectedVoice
            };

            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText($"{programPath}/settings.json", json);
        }

        private void textBoxElevenLabsAPIKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            XI_API_KEY = textBoxElevenLabsAPIKey.Text;
        }
        private async void ContinueConvert()
        {
            progressLabel.Content = "Converting text to speech";
            var responce = await TTSAll();
            progressBar.Value += 20;
            if (responce)
            {
                progressLabel.Content = "Adding Narration";
                AddNarration($"{programPath}/{ProjectName}/wavs/");
                progressBar.Value += 20;
                progressLabel.Content = "Exporting to MP4";
                var output = await ExportToMp4($"{programPath}/{ProjectName}/press.mp4");
                progressBar.Value += 20;
                progressLabel.Content = "Finished";
                var result = new CustomMessageBox("Finished! \nWant to open in explorer?", "Question", CustomMessageBox.MessageButtons.YesNo).ShowDialog();
                if (result.Value == true)
                {
                    System.Diagnostics.Process.Start("explorer.exe", "/select," + $"{programPath}/{ProjectName}/".Replace('/', '\\'));
                }
            } else
            {
                new CustomMessageBox("Error while narrate slides").ShowDialog();
            }
        }

        async Task<bool> IsApiKeyCorrect(string apiKey)
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

        #region Narration
        private void AddNarration(string wavFolderPath)
        {
            string presentationPath = $"{programPath}/{ProjectName}/press.pptx";

            if (File.Exists(presentationPath))
            {
                File.Delete(presentationPath);
            }

            File.Copy(filePath, presentationPath);

            Application application = new Application();
            Presentation presentation = application.Presentations.Open(presentationPath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);

            for (int i = 0; i < presentation.Slides.Count; i++)
            {
                if (slidesText[i].Trim() != string.Empty)
                {
                    Slide slide = presentation.Slides[i + 1];
                    Shapes shapes = slide.Shapes;
                    string wavFilePath = wavFolderPath.Replace('/', '\\') + $"{i}.mp3";
                    var audioShape = slide.Shapes.AddMediaObject2(wavFilePath);
                    audioShape.Left = -100; // Set the left position
                    audioShape.Top = -100; // Set the top position
                    audioShape.Width = 1; // Set the width
                    audioShape.Height = 1; // Set the height
                    audioShape.AnimationSettings.PlaySettings.PlayOnEntry = MsoTriState.msoTrue;

                    //Shape narrationShape = shapes.AddMediaObject2(wavFilePath, MsoTriState.msoFalse, MsoTriState.msoTrue, 0, 0);
                    //// Set the playback settings for the audio
                    //narrationShape.AnimationSettings.PlaySettings.PlayOnEntry = MsoTriState.msoTrue;
                    //narrationShape.AnimationSettings.PlaySettings.HideWhileNotPlaying = MsoTriState.msoTrue;

                    ////set durration
                    //AudioFileReader wf = new AudioFileReader(wavFilePath);
                    //var wavDuration = (long)wf.TotalTime.TotalSeconds;
                    //slide.SlideShowTransition.AdvanceOnTime = MsoTriState.msoTrue;
                    //slide.SlideShowTransition.AdvanceTime = wavDuration;
                }
            }
            presentation.Save();
            presentation.Close();
            application.Quit();
        }

        private async Task<bool> ExportToMp4(string outputPath)
        {
            try
            {
                string presentationPath = $"{programPath}/{ProjectName}/press.pptx";
                Application application = new Application();
                Presentation pptPresentation = application.Presentations.Open(presentationPath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);
                pptPresentation.SaveAs(outputPath, PpSaveAsFileType.ppSaveAsMP4, MsoTriState.msoCTrue);
                return true;
            } catch(Exception ex)
            {
                new CustomMessageBox($"Error export. {ex}").ShowDialog();
                return false;
            }
        }

        #region TTS
        private async Task<bool> TTSAll()
        {
            if (!Directory.Exists($"{programPath}/{ProjectName}/wavs/"))
            {
                Directory.CreateDirectory($"{programPath}/{ProjectName}/wavs/");
            }
            for (int i = 0; i < slidesCount; i++)
            {
                if (slidesText[i].Trim() != string.Empty)
                {
                    var responce = await TTS(slidesText[i], $"{programPath}/{ProjectName}/wavs/{i}.mp3");
                    if(responce == false)
                    {
                        return false;
                        break;
                    }
                }
            }
            return true;
        }

        async Task<bool> TTS(string text, string outputPath)
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
                return true;
                // Inform the user of success
                //Console.WriteLine("Audio stream saved successfully.");
            }
            else
            {
                // Print the error message if the request was not successful
                new CustomMessageBox(await response.Content.ReadAsStringAsync()).ShowDialog();
                return false;
            }
            //Task.Delay(1000);
        }

        #endregion
        #endregion
    }
}