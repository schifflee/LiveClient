   M S B A M L   `   `   `  ����  LiveClientLancher"    LiveClientLancher.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -   5          � Title$  
MainWindow��5   	   $	��350��5   	    � AllowsTransparency . 
������  � WindowStyle$
 None=�5   	   $	��280��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ��5	   	   Z�  � 	Resources 5       �� Color �� Opacity   OpacityBrush �� MergedDictionaries �� Source�� 5   
    5      �� 5      $P Jpack://application:,,,/LiveClientLancher;component/Styles/ProgressBar.xaml?�6%   6{   %<   &	        �� 5      $ White��6   $ .3Q�6,   69   5      .��5      � 
������ .+�5   
   �� 	 �� CornerRadius$	 5��6   $��1q�6#   
���������5      �� 5      -   5        ImageControl��
 �� ImageSource$@
 :pack://siteoforigin:,,,/Resources/MainWindowBackground.jpg��5   3   6~   5      � 5   
    � RowDefinitions 5       �� 5!      $��40 �6    �� 5"      $��40 �6    �� 5#      $��30 �6    �� 5$      $	��130 �6    �� 5%      $��23 �6    �� 5&      $��30 �6    .+�5(      �� $��1q�6   $	 5 5 0 0��6)   ���#80BA45._�5)      �� $'��!密云直播云课堂在线升级��6   
�������$��14(�6B   $��Center=�6Q   $��Center=�6n   6�   5*      � 5+      $��1��6    � ColumnDefinitions 5-      �� 5.      $��0.26* �6'   �� 5/      $
��0.5* �6'   .+�51      �� $��当前软件版本：��6   $��	10 10 0 0q�6*   
�������$��Center=�6P   �� 52      -   5         ��Name$ CurrentVersion��52   N   $��1��6   $��	2.0.0.0.1��6(   $��0 10 0 0q�6<   
�������$��Center=�6y   $
��Left=�6�   53      � 54      $��2��6    55      �� 56      $��0.26* �6'   �� 57      $
��0.5* �6'   .+�59      �� $��2��6   $��最新软件版本：��6%   $��10 0 0 0q�67   
�������$��Center=�6\   �� 5:      -   5        $ LastCurrentVersion��5:   =   $��1��6   $��	2.0.0.0.1��6(   
�������$��Center=�6l   $
��Left=�6�   5;      �� 5<      $��3��6   $	 5��6#   $��1q�5=      $��	10 0 10 0q�6   #��[  6)   ._�5>      �� -   5         ��Name$ 	ChangeLog��5>   C   $��	10 10 0 0q�6   $
��Wrap=�6/    �� 
LineHeight$ 20��6V   
���   �$��2��6y   5A      5B      � 5C      -   5         �Name$ DownloadProgressBar��5C   �   $��
10 20 10 0q�6   $��4��6.     ProgressBarStyle#��[ 6;   ���#B39DDB���#512DA8 � Value$ 10Q�6�    � Maximum$	 100Q�6�   � 5D      $��5��6   $��
10 10 10 0q�6    $��Center=�64   $��Center=�6O    5E      �� 5F      $
��0.9* �6'   �� 5G      $
��0.1* �6'   .+�5I      �� -   5        $ DownloadVersionText��5I   Y   $��0��6   $,��&正在下载V1.3.2.2版本的更新包��6,   
�������6x   �� 5J      -   5        $ DownloadProgressText��5J   8   $��1��6   $
��100%��6,   $��Center=�6W   
�������$
��Left=�6�   6�   5K      5M      5N      5O      