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
                            text += textRange.Text + ".\n"; 
                        }
                    }
                }
                PresTexts.Add(text);
            }

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
                application.PresentationCloseFinal += Application_PresentationCloseFinal;
                Presentation pptPresentation = application.Presentations.Open(presentationPath, MsoTriState.msoFalse, MsoTriState.msoFalse);
                pptPresentation.SaveAs(outputPath, PpSaveAsFileType.ppSaveAsMP4, MsoTriState.msoCTrue);

                bool isExportComplete = false; 
                while (!isExportComplete)
                {
                    Thread.Sleep(1000);
                    if (File.Exists(outputPath) && pptPresentation.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusDone) isExportComplete = true;
                }
                var res = false;
                if (pptPresentation.Saved == MsoTriState.msoTrue) res = true; else res = false;
                pptPresentation.Save();
                pptPresentation.Close();
                application.Quit();
                return res;
            }
            catch
            {
                return false;
            }
        }

        private void Application_PresentationCloseFinal(Presentation Pres)
        {
            Pres.Close();
        }
    }
}
