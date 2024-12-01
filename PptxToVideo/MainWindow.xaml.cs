using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Win32;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PptxToVideo.Repository.Repository;
using System.Collections.ObjectModel;
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
        List<string> slidesText = new List<string>();
        Settings appSettings;
        private const int CHUNK_SIZE = 1024;
        private static string XI_API_KEY;
        private static ElevenLabsRepository _elevenLabsRepository;
        private static PowerPointRepository _powerPointRepository;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _elevenLabsRepository = new ElevenLabsRepository();
            _powerPointRepository = new PowerPointRepository();

            if (File.Exists($"{programPath}/settings.json"))
            {
                appSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText($"{programPath}/settings.json"));
            }
        }

        public ObservableCollection<string> apiKeys = new ObservableCollection<string>();

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
            if (path != "")
            {
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
            } else if(comboBoxApiKeys.Text.Trim() == "")
            {
                new CustomMessageBox("Api key is null or whitespace", "Api key not correct").ShowDialog();
            } else if (!(_elevenLabsRepository.IsApiKeyCorrect(XI_API_KEY).Result))
            {
                new CustomMessageBox("Api key is uncorrect", "Api key not correct").ShowDialog();
            } else
            {
                progressLabel.Content = "Getting Text For Voice-Over";
                var texts = _powerPointRepository.GetTextFromPressentation(filePath);
                slidesCount = texts.Count;
                slidesText = texts;
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
                    progressBar.Value += 20;
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            var voices = await _elevenLabsRepository.GetVoices();
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
            if (appSettings != null)
                {
                    comboBoxApiKeys.SelectedItem = appSettings.RecentElevenLabsApiKey;
                    apiKeys = appSettings.ElevenLabsApiKeys;
            } else
                {
                    apiKeys.Add("Add ApiKey");
                }
                comboBoxApiKeys.ItemsSource = apiKeys;
            //}
            //catch
            //{

            //}
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
            if (comboBoxApiKeys.SelectedItem == null)
            {
                return;
            }
            string apiKey = comboBoxApiKeys.SelectedItem.ToString().ToString();
            Settings settings = new Settings()
            {
                RecentElevenLabsApiKey = apiKey,
                SelectedVoice = selectedVoice
            };

            if (settings.ElevenLabsApiKeys.Count == 0)
            {
                settings.ElevenLabsApiKeys.Add(apiKey);
            }

            if(settings.ElevenLabsApiKeys != apiKeys)
            {
                settings.ElevenLabsApiKeys = apiKeys;
            }

            var json = JsonConvert.SerializeObject(settings);
            File.WriteAllText($"{programPath}/settings.json", json);
        }

        private async void ContinueConvert()
        {
            progressLabel.Content = "Converting text to speech";
            var responce = await _elevenLabsRepository.TTSAll(ProjectName, slidesCount, slidesText, selectedVoice.voice_id, XI_API_KEY);
            progressBar.Value += 20;
            if (responce)
            {
                progressLabel.Content = "Adding Narration";
                string presentationPath = $"{programPath}/{ProjectName}/press.pptx";
                _powerPointRepository.AddNarration($"{programPath}/{ProjectName}/wavs/", filePath, slidesText, presentationPath);
                progressBar.Value += 20;
                progressLabel.Content = "Exporting to MP4";
                var output = await _powerPointRepository.ExportToMp4($"{programPath}/{ProjectName}/press.mp4", presentationPath);
                progressBar.Value = 120;
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

        #region Narration


        #endregion

        private void comboBoxApiKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxApiKeys.SelectedItem != null)
            {
                if (comboBoxApiKeys.SelectedItem.ToString() != "Add ApiKey")
                {
                    XI_API_KEY = comboBoxApiKeys.SelectedItem.ToString(); ;
                }
                else
                {
                    var newApiKey = new AddApiKey();
                    var dialogResult = newApiKey.ShowDialog();

                    if (dialogResult.Value == true)
                    {
                        if (apiKeys.Contains(newApiKey.ApiKey))
                        {
                            new CustomMessageBox("This API key is already in use");
                        }
                        if (_elevenLabsRepository.IsApiKeyCorrect(newApiKey.ApiKey).Result)
                        {
                            apiKeys.Add(newApiKey.ApiKey);
                            comboBoxApiKeys.SelectedIndex = 0;
                            apiKeys.Remove("Add ApiKey");
                            apiKeys.Add("Add ApiKey");
                        }
                        else
                        {
                            new CustomMessageBox("This API key is incorrect and can't be used for TTS", "Incorrect API key").ShowDialog();
                        }
                    }
                    comboBoxApiKeys.SelectedIndex = 0;
                }
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    var textBlock = contextMenu.PlacementTarget as TextBlock;
                    if (textBlock != null)
                    {
                        string itemToRemove = textBlock.Text;
                        apiKeys.Remove(itemToRemove);
                    }
                }
            }
            comboBoxApiKeys.SelectedIndex = 0;
        }

        private async void MenuItemCheck_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var contextMenu = menuItem.Parent as ContextMenu;
                if (contextMenu != null)
                {
                    var textBlock = contextMenu.PlacementTarget as TextBlock;
                    if (textBlock != null)
                    {
                        var apiKey = textBlock.Text;
                        var j = await _elevenLabsRepository.GetCharterStats(apiKey);
                        new CustomMessageBox($"Charters this key have: {j}", "Charters").ShowDialog();
                    }
                }
            }
        }
    }
}