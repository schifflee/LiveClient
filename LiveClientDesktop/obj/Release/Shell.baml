   M S B A M L   `   `   `  ���� L  HLiveClientDesktop, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:LiveClientDesktop.ViewsLiveClientDesktop.Views      LiveClientDesktop.Shell   H DMahApps.Metro, Version=1.6.5.1, Culture=neutral, PublicKeyToken=null;Controls,http://metro.mahapps.com/winfx/xaml/controls  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     7views%clr-namespace:LiveClientDesktop.Views     V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  -   (  "MahApps.Metro.Controls.MetroWindow    TitlebarHeight$  35��$	��720��$
��1280��$	��720��$
��1280�� � Title �� *WindowTitle+   RightWindowCommands +  %MahApps.Metro.Controls.WindowCommands     ShowLastSeparator .  .@�,   &LiveClientDesktop.Views.LivingTimeView   � DataContext �� *VMContext.LivingTimeViewModel+/   )LiveClientDesktop.Views.RecordingTimeView   �� *" VMContext.RecordingTimeViewModel+)   #LiveClientDesktop.Views.WelcomeView   �� *VMContext.WelcomeViewModel+d� -    � ToolTip$ 设置��.��'  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]   Kind$ Settings=� � 	Resources    MetroTabItem �� 
TargetType	 �� Property
 �� Value  GridSplitterPreviewStyle  GridSplitterStyle1 �� 
TargetType K� ZIndex � Cursor)  #MahApps.Metro.Controls.MetroTabItem ��Name �� Triggers P� Property P� Value �� 
TargetName�� %�  &	        &	 %     &	 \     7  '  _    �� " ��.��� 	 � ��

 ��� ��� .��� 	 � ��
 �� .R��� ���	#80000000�� " ��.��� 	 � ��#
 � ���� 	 �@��PreviewStyle8
   �� 	 � ��
 �� " ��.R��� #��z��#��z��#��z��._�� .+��� $��	btnExpend��$��10(�$��0q����#80BA45���#80BA45$
��Left=�$��>��$��Center=�$��80��$ 0��$
 Hand���� "  .��� 	 � ��
 �� "  .R�� .+��� $��Border�����#F2F2F2$��0 1 0 0q�._��� $ ContentSite��$��Center=�$��Center=�$��Header��$��	12,5,12,5q� P�  �@��
IsSelected . . ��� $ Border��	 � ��
 �#80BA45P�  �@��
IsSelected .  . ��� $ Border��	 � ��
 �#F2F2F2�� 	 � ��
 �#333.��� -    �Name$ grdWorkbench�� � ColumnDefinitions �� $
��944* �$	��944���� $��5 ��� $	��330 �$	��330��.+�� $��0��.+�(	   "LiveClientDesktop.Views.SpeechView	   �� *VMContext.SpeechViewModel+�� -    ��Name$ gsSplitterr��$��1��$��Stretch=����	#FF9F9F9F#���  � -   $ righta��$��2��$	��330�����#f2f2f2 � RowDefinitions �� $	��225 ��� $��30 ��� $
��390* �.+�� $��0��
�������.+�4
   .LiveClientDesktop.Views.PowerCreatorPlayerView
   �� *'%VMContext.PowerCreatorPlayerViewModel+� $��1��
����� � �� $	��100 ��� $��10 ��� $	��100 ��� $��10 ��� $��40 �.+�9   3LiveClientDesktop.Views.MicrophoneVolumeControlView  $��0�� �� *,*VMContext.MicrophoneVolumeControlViewModel+3   -LiveClientDesktop.Views.PlayVolumeControlView  $��2�� �� *&$VMContext.PlayVolumeControlViewModel+.   (LiveClientDesktop.Views.CameraDeviceView  $��4�� �� *!VMContext.CameraDeviceViewModel+,  &MahApps.Metro.Controls.MetroTabControl  $��2��-  'MahApps.Metro.Controls.TabControlHelper   
Underlined$ TabPanel=�   UnderlineSelectedBrush �#80BA45   UnderlineMouseOverBrush �#80BA45   IsUnderlined .     UnderlineBrush �#80BA45&   UnderlineMouseOverSelectedBrush �#80BA45 �� TabStripPlacement$ Bottom=�.@�  $��直播互动��+  %MahApps.Metro.Controls.ControlsHelper   HeaderFontSize$ 16Q�.��� .+��  �� $��5 ��� $	��100 ��� $��5 ��� $	��100 ��� $	��180 �.+�� $��1��.+�3   -LiveClientDesktop.Views.ClassRoomTeachingView   �� *&$VMContext.ClassRoomTeachingViewModel+� $��3�� �� $
��.49* ��� $
��.01* ��� $
��.49* �.+�-   'LiveClientDesktop.Views.LiveControlView  $��0�� �� * VMContext.LiveControlViewModel+2   ,LiveClientDesktop.Views.RecordingControlView  $��2�� �� *%#VMContext.RecordingControlViewModel+� $��4��.+�0   *LiveClientDesktop.Views.CourseContentsView   �� *#!VMContext.CourseContentsViewModel+  $��资料上传��$ 16Q�.��0   *LiveClientDesktop.Views.UploadDocumentView   �� *#!VMContext.UploadDocumentViewModel+  $��课件上传��$ 16Q�.��2   ,LiveClientDesktop.Views.UploadCoursewareView   �� *%#VMContext.UploadCoursewareViewModel+