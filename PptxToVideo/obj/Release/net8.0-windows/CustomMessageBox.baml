   M S B A M L   `   `   `  ���� F  BPptxToVideo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:PptxToVideoPptxToVideo  "    PptxToVideo.CustomMessageBox   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:PptxToVideo     _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  
������   � WindowStyle$
  None=� � 
ResizeMode$ NoResize=� � AllowsTransparency$
 True�� � Title$ Message��$	��220��$	��500��.��� .+��� ���#272530 �� CornerRadius$ 10��._��  � RowDefinitions �� $��50 ��� .+�]� -   ���#1F1D28$��False��$	��Top=�.+��� -    ��Name$ CloseBtn�� "  MaterialDesignRaisedDarkButton#��[   � ToolTip$ Close Program��$��45��$��45��$��1��$��Right=�$��Right=�$��
2, 2, 8, 2q�
������ $��0q�{��� *DataContext.ControlsEnabled+ �� RelativeSource �� *FindAncestor+	 �� AncestorType
	 Window.��'  !MaterialDesignThemes.Wpf.PackIcon  $��Center=�
�������
   Kind$
 Close=�$��20��$��20���� -    ��Name$ txtTitle��$��hi�����#CECECE$��Center=�$��Center=�$��22(�]� $��1��$��10q�$��False��.+��� -   $ 
txtMessage��$��hi�����#CECECE$��Center=�$��Center=�$��18(�$��5q�$	��Top=�� $��1��$��0,20,0,5q�$��Bottom=� � ColumnDefinitions �� $��* ��� $
��auto ��� $
��auto ��� $��* �.+��� -   $ btnOk��$��1��  MaterialDesignRaisedButton#��[ $��5q�$	��100��$��OK��$ Ok���� -   $ 	btnCancel��$��2��#��[ $��5q�$	��100��$��Cancel��$ Cancel���� -   $ btnYes��$��1��#��[ $��5q�$	��100��$	��Yes��$	 Yes���� -   $ btnNo��$��2��#��[ $��5q�$	��100��$��No��$ No��