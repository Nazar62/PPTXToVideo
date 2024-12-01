using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Microsoft.Office.Interop.PowerPoint.Application;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;
using Shapes = Microsoft.Office.Interop.PowerPoint.Shapes;

namespace PptxToVideo.Repository.Repository
{
    public class PowerPointRepository
    {
        public List<string> GetTextFromPressentation(string filePath)
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

            //slidesCount = PresTexts.Count;
            //slidesText = PresTexts;
            //WriteTextToFiles(PresTexts, $"{programPath}/{ProjectName}/txt/");

            pptPresentation.Close();
            pptApplication.Quit();
            return PresTexts;
        }

        public void AddNarration(string wavFolderPath, string filePath, List<string> slidesText, string presentationPath)
        {

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
                    audioShape.Left = -100;
                    audioShape.Top = -100;
                    audioShape.Width = 1;
                    audioShape.Height = 1;
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

        public async Task<bool> ExportToMp4(string outputPath, string presentationPath)
        {
            try
            {
                Application application = new Application();
                Presentation pptPresentation = application.Presentations.Open(presentationPath, MsoTriState.msoFalse, MsoTriState.msoFalse);
                pptPresentation.SaveAs(outputPath, PpSaveAsFileType.ppSaveAsMP4, MsoTriState.msoCTrue);
                try
                {
                    while (application.Presentations.Count > 0)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch
                {

                }
                if (pptPresentation.Saved == MsoTriState.msoTrue) return true; else return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
