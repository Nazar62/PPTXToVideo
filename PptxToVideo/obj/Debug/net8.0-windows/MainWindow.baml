   M S B A M L   `   `   `  ����F  BPptxToVideo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PptxToVideoPptxToVideo  5   	       PptxToVideo.MainWindow   5   	   _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:PptxToVideo     5   	   -   5        
������   � WindowStyle$
  None=�5
   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency$
 True��5   	    � Title$ PptxToVideo��5   	   $	��600��6   $	��920��6*   .��5      �� ���#272530 �� CornerRadius$ 10��5      ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5       � RowDefinitions 5      �� 5      $��75 �6    �� 5      .+�5      �� $��PptxToVideo��6   
�������$��Center=�5      $��Center=�5      $��0��5      $��0��5       $��22(�5!      ]� 5"      -   5        $��False��5"      $��1��6.   $	��Top=�6>   
������ .+�5#      �� -   5         ��Name$ CloseBtn��52       "  MaterialDesignRaisedDarkButton#��[  5%       � ToolTip$ Close Program��5&      $��55��5'      $��55��5(      $��1��5)      $��Right=�5*      $��Right=�5+      $��10q�5,      
������ $��0q�5.      {�5$      �� *DataContext.ControlsEnabled+	 �� RelativeSource	 �� *FindAncestor+
 �� AncestorType

 Window.��53      '  !MaterialDesignThemes.Wpf.PackIcon  
�������   Kind$ Close=�6A   $��20��6N   $��20��6Z   54      55      �� 56      $��1��6   $��5, 10, 1, 5q�6&   .+�57      �� -   5         ��Name$ textBoxElevenLabsAPIKey��58      $	��400��5:      ���#CECECE$	��Top=�5<      )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ ElevenLabs API��5=       �� AcceptsReturn$
 True��5>        MaterialDesignFilledTextBox#��[ 5?       �� TextWrapping$
 Wrap=�5@      .��5A      ��5B      �� 5C       �� Path$
 Name�5D       �� UpdateSourceTrigger$ PropertyChanged=�5E       �� ValidationRules 5F      (   "PptxToVideo.NotEmptyValidationRule  5G   "   ,  &System.Windows.Controls.ValidationRule   ValidatesOnTargetUpdated$
 True��5H   %   5J      .��5K   $   5L      �� 5M      -   5         ��Name$ comboBoxVoices��5N      ���#CECECE$	��199��5P      $��0, 20, 0, 0q�5Q      $  Validation test (editable)��5R       �� DisplayMemberPath$
 name��5U         MaterialDesignFilledComboBox#��[ 5V      ��5T      �� *voices+5X      �� 5Y      -   5        $ buttonStartConvert��5Z      $��Start Convert��5\      
����� �$��0q�5^      $��0, 20, 0, 0q�5_      #��[  5a      $) #Start Convert Presentation to Video��5b      {�5`      �� *DataContext.ControlsEnabled+	 �� *FindAncestor+

 Window5b      �� 5d      $	��200��6   .+�5e      �� $��
0, 5, 0, 0q�5f      $��
File Data:��5g      $��Center=�5h      �� 5i      -   5         ��Name$ FileData��5j      ���#CECECE$��15 ,0, 0, 0q�5l      �� 5m      -   5         ��Name$ progressLabel��5n      $y�Hidden=�5o      $��
0, 5, 0, 0q�5p      $��Progress��5q      $��Center=�5r      � 5s      -	   5         �Name$ progressBar��5t      $��10��5u       � Value$ 0Q�5v      ���#1F1D28$��0q�5x      $	��180��5y      $y�Hidden=�5z      5|      5}      �� 5~      -
   5         ��Name$ DragRectangle��5       J� 	AllowDrop$
 True��5�      $��10q�5�      $��1��5�      $��1��5�      ���#1F1D28 �� RadiusX$ 10��5�       �� RadiusY$ 10��5�      $��2��5�       �� StrokeDashArray$ 10R�5�       �� StrokeDashCap$ Round=�5�      ���#3A374B5�      �� 5�      -   5        $ 	DropLabel��5�      $��1��5�      $��1��5�      $��18(�5�      $��Center=�5�      $��Center=�5�      $��Drag And Drop��5�      
�������5�      �� 5�      -   5          ��Name$  panelTextEdit��5�      $��1��5�      $��1��5�      $y�Hidden=�5�      .+�5�      �� $��10q�6   .+�5�      �� -   5        $ textBoxSlideText��5�      $	��450��5�      $	��Top=�5�      ���#CECECE$��2q�5�      $ This is a text area��5�      $
 True��5�       ! MaterialDesignOutlinedTextBox#��[ 5�      $
 Wrap=�5�      "! �� VerticalScrollBarVisibility$
! Auto=�5�      {�5�      �� $ 	IsChecked�" �� ElementName$2" ,MaterialDesignOutlinedTextBoxEnabledComboBox��5�      �� 5�      -   5        $ 
buttonNext��5�      $
��Next��5�      
����� �$��0q�5�      $��10q�5�      $��90��5�      $
��Left=�5�      #��[  5�      $ Next Slide Text��5�      {�5�      �� *DataContext.ControlsEnabled+	 �� *FindAncestor+

 Window5�      �� 5�      -   5        $ buttonSkipAll��5�      $��Skip All��5�      
����� �$��0q�5�      $��110,-52, 0 ,0q�5�      $��90��5�      $
��Left=�5�      #��[  5�      $ Next Slide Text��5�      {�5�      �� *DataContext.ControlsEnabled+	 �� *FindAncestor+

 Window5�      �� 5�      -   5        $ 
labelCount��5�       $��Count��5�       $��20(�5�       ���#CECECE$��210 ,-40 , 0 ,0q�5�       5�      5�      5�      5�      5�      