namespace SpirePptxToVideo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVoices = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonStartConvert = new System.Windows.Forms.Button();
            this.buttonOpenInExplorer = new System.Windows.Forms.Button();
            this.textBoxSlideText = new System.Windows.Forms.TextBox();
            this.panelTextEdit = new System.Windows.Forms.Panel();
            this.labelCount = new System.Windows.Forms.Label();
            this.buttonSkipAll = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.textBoxElevenLabsAPIKey = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panelTextEdit.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AllowDrop = true;
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 266);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 181);
            this.panel1.TabIndex = 0;
            this.panel1.DragDrop += new System.Windows.Forms.DragEventHandler(this.panel1_DragDrop);
            this.panel1.DragEnter += new System.Windows.Forms.DragEventHandler(this.panel1_DragEnter);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.Location = new System.Drawing.Point(361, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Drag and Drop";
            // 
            // comboBoxVoices
            // 
            this.comboBoxVoices.FormattingEnabled = true;
            this.comboBoxVoices.Location = new System.Drawing.Point(12, 37);
            this.comboBoxVoices.Name = "comboBoxVoices";
            this.comboBoxVoices.Size = new System.Drawing.Size(121, 21);
            this.comboBoxVoices.TabIndex = 1;
            this.comboBoxVoices.SelectedIndexChanged += new System.EventHandler(this.comboBoxVoices_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Voices";
            // 
            // buttonStartConvert
            // 
            this.buttonStartConvert.Location = new System.Drawing.Point(12, 74);
            this.buttonStartConvert.Name = "buttonStartConvert";
            this.buttonStartConvert.Size = new System.Drawing.Size(98, 23);
            this.buttonStartConvert.TabIndex = 3;
            this.buttonStartConvert.Text = "Start convert";
            this.buttonStartConvert.UseVisualStyleBackColor = true;
            this.buttonStartConvert.Visible = false;
            this.buttonStartConvert.Click += new System.EventHandler(this.buttonStartConvert_Click);
            // 
            // buttonOpenInExplorer
            // 
            this.buttonOpenInExplorer.Location = new System.Drawing.Point(13, 104);
            this.buttonOpenInExplorer.Name = "buttonOpenInExplorer";
            this.buttonOpenInExplorer.Size = new System.Drawing.Size(97, 23);
            this.buttonOpenInExplorer.TabIndex = 4;
            this.buttonOpenInExplorer.Text = "Open in explorer";
            this.buttonOpenInExplorer.UseVisualStyleBackColor = true;
            this.buttonOpenInExplorer.Visible = false;
            this.buttonOpenInExplorer.Click += new System.EventHandler(this.buttonOpenInExplorer_Click);
            // 
            // textBoxSlideText
            // 
            this.textBoxSlideText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSlideText.Location = new System.Drawing.Point(0, 0);
            this.textBoxSlideText.Multiline = true;
            this.textBoxSlideText.Name = "textBoxSlideText";
            this.textBoxSlideText.Size = new System.Drawing.Size(609, 222);
            this.textBoxSlideText.TabIndex = 5;
            this.textBoxSlideText.TextChanged += new System.EventHandler(this.textBoxSlideText_TextChanged);
            // 
            // panelTextEdit
            // 
            this.panelTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTextEdit.Controls.Add(this.labelCount);
            this.panelTextEdit.Controls.Add(this.buttonSkipAll);
            this.panelTextEdit.Controls.Add(this.buttonNext);
            this.panelTextEdit.Controls.Add(this.textBoxSlideText);
            this.panelTextEdit.Location = new System.Drawing.Point(186, 3);
            this.panelTextEdit.Name = "panelTextEdit";
            this.panelTextEdit.Size = new System.Drawing.Size(609, 257);
            this.panelTextEdit.TabIndex = 6;
            this.panelTextEdit.Visible = false;
            // 
            // labelCount
            // 
            this.labelCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(166, 235);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(35, 13);
            this.labelCount.TabIndex = 9;
            this.labelCount.Text = "Count";
            this.labelCount.Click += new System.EventHandler(this.labelCount_Click);
            // 
            // buttonSkipAll
            // 
            this.buttonSkipAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSkipAll.Location = new System.Drawing.Point(85, 230);
            this.buttonSkipAll.Name = "buttonSkipAll";
            this.buttonSkipAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSkipAll.TabIndex = 8;
            this.buttonSkipAll.Text = "Skip all";
            this.buttonSkipAll.UseVisualStyleBackColor = true;
            this.buttonSkipAll.Click += new System.EventHandler(this.buttonSkipAll_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNext.Location = new System.Drawing.Point(4, 230);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(75, 23);
            this.buttonNext.TabIndex = 7;
            this.buttonNext.Text = "Next";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // textBoxElevenLabsAPIKey
            // 
            this.textBoxElevenLabsAPIKey.Location = new System.Drawing.Point(12, 158);
            this.textBoxElevenLabsAPIKey.Name = "textBoxElevenLabsAPIKey";
            this.textBoxElevenLabsAPIKey.Size = new System.Drawing.Size(168, 20);
            this.textBoxElevenLabsAPIKey.TabIndex = 7;
            this.textBoxElevenLabsAPIKey.TextChanged += new System.EventHandler(this.textBoxElevenLabsAPIKey_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "ElevenLabs API key";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxElevenLabsAPIKey);
            this.Controls.Add(this.panelTextEdit);
            this.Controls.Add(this.buttonOpenInExplorer);
            this.Controls.Add(this.buttonStartConvert);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxVoices);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.panel1.ResumeLayout(false);
            this.panelTextEdit.ResumeLayout(false);
            this.panelTextEdit.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxVoices;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonStartConvert;
        private System.Windows.Forms.Button buttonOpenInExplorer;
        private System.Windows.Forms.TextBox textBoxSlideText;
        private System.Windows.Forms.Panel panelTextEdit;
        private System.Windows.Forms.Button buttonSkipAll;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label labelCount;
        private System.Windows.Forms.TextBox textBoxElevenLabsAPIKey;
        private System.Windows.Forms.Label label3;
    }
}

