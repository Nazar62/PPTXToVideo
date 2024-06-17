using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using Application = Microsoft.Office.Interop.PowerPoint.Application;

namespace PressToVideo
{
    public partial class Form1 : Form
    {
        string filePath;
        string ProjectName;
        string programPath = Directory.GetCurrentDirectory();
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

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            filePath = files[0];
            ProjectName = System.IO.Path.GetFileNameWithoutExtension(filePath);

            var texts = GetTextFromPressentation(filePath);

            WriteTextToFiles(texts, $"{programPath}/{ProjectName}/txt/");

            MessageBox.Show("Fineshed!!!");
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
    }
}
