   M S B A M L   `   `   `  ���� F  BPptxToVideo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PptxToVideoPptxToVideo      PptxToVideo.MainWindow   _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:PptxToVideo     -   
������   � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � AllowsTransparency$
 True�� � Title$ 
MainWindow��$	��600��$	��920��.���� ���#272530 �� CornerRadius$ 10��._��  � ColumnDefinitions �� $	��200 ���  � RowDefinitions �� $��75 ��� .+��� $��PptxToVideo��
�������$��Center=�$��Center=�$��0��$��0��$��22(�]� -   $��False��$��1��$	��Top=�
������ .+��� -    ��Name$ CloseBtn�� "  MaterialDesignRaisedDarkButton#��[   � ToolTip$ Close Program��$��55��$��55��$��1��$��Right=�$��Right=�$��10q�
������ $��0q�{��� *DataContext.ControlsEnabled+	 �� RelativeSource	 �� *FindAncestor+
 �� AncestorType

 Window.��'  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�$��20��$��20���� $��1��.+��� -    ��Name$ textBoxElevenLabsAPIKey��$	��400�����#CECECE$	��Top=�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ ElevenLabs API�� �� AcceptsReturn$
 True��  MaterialDesignFilledTextBox#��[  �� TextWrapping$
 Wrap=�.������  �� Path$
 Name� �� UpdateSourceTrigger$ PropertyChanged=� �� ValidationRules (   "PptxToVideo.NotEmptyValidationRule  ,  &System.Windows.Controls.ValidationRule   ValidatesOnTargetUpdated$
 True��.���� -    ��Name$ comboBoxVoices�����#CECECE$	��199��$��0, 20, 0, 0q�$  Validation test (editable)�� �� DisplayMemberPath$
 name��   MaterialDesignFilledComboBox#��[ ���� *voices+�� -   $ buttonStartConvert��$��Start Convert��
����� �$��0q�$��0, 20, 0, 0q�#��[  $) #Start Convert Presentation to Video��{��� *DataContext.ControlsEnabled+	 �� *FindAncestor+

 Window�� .+��� $��
0, 5, 0, 0q�$��
File Data:��$��Center=��� -    ��Name$ FileData�����#CECECE$��15 ,0, 0, 0q��� -    ��Name$ DragRectangle�� J� 	AllowDrop$
 True��$��10q�$��1��$��1�����#1F1D28 �� RadiusX$ 10�� �� RadiusY$ 10��$��2�� �� StrokeDashArray$ 10R� �� StrokeDashCap$ Round=����#3A374B�� -	   $ 	DropLabel��$��1��$��1��$��18(�$��Center=�$��Center=�$��Drag And Drop��
��������� -
    ��Name$ panelTextEdit��$��1��$��1��$y�Hidden=�.+��� $��10q�.+��� -   $ textBoxSlideText��$	��450��$	��Top=����#CECECE$��2q�$ This is a text area��$
 True�� ! MaterialDesignOutlinedTextBox#��[ $
 Wrap=�" �� VerticalScrollBarVisibility$
 Auto=�{��� $ 	IsChecked� �� ElementName$2 ,MaterialDesignOutlinedTextBoxEnabledComboBox���� -   $ 
buttonNext��$
��Next��
����� �$��0q�$��10q�$��90��$
��Left=�#��[  $ Next Slide Text��{��� *DataContext.ControlsEnabled+	 �� *FindAncestor+

 Window�� -   $ buttonSkipAll��$��Skip All��
����� �$��0q�$��110,-52, 0 ,0q�$��90��$
��Left=�#��[  $ Next Slide Text��{��� *DataContext.ControlsEnabled+	 �� *FindAncestor+

 Window�� -   $ 
labelCount��$��Count��$��20(����#CECECE$��210 ,-40 , 0 ,0q�