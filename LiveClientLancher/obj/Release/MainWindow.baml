   M S B A M L   `   `   `  ����   LiveClientLancher"    LiveClientLancher.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$  
MainWindow��$	��350�� � AllowsTransparency . 
������  � WindowStyle$
 None=�$	��280�� � WindowStartupLocation$ CenterScreen=���Z�  � 	Resources  �� Color �� Opacity   OpacityBrush �� MergedDictionaries �� Source��  �� $P Jpack://application:,,,/LiveClientLancher;component/Styles/ProgressBar.xaml?�%$   &	        �� $ White��$ .3Q�.��� 
������ .+��� 	 �� CornerRadius$	 5��$��1q�
����������� -   ImageControl��
 �� ImageSource$@
 :pack://siteoforigin:,,,/Resources/MainWindowBackground.jpg���  � RowDefinitions �� $��40 ��� $��40 ��� $��30 ��� $	��130 ��� $��23 ��� $��30 �.+��� $��1q�$	 5 5 0 0�����#80BA45._��� $'��!密云直播云课堂在线升级��
�������$��14(�$��Center=�$��Center=�� $��1�� � ColumnDefinitions �� $��0.26* ��� $
��0.5* �.+��� $��当前软件版本：��$��	10 10 0 0q�
�������$��Center=��� -    ��Name$ CurrentVersion��$��1��$��	2.0.0.0.1��$��0 10 0 0q�
�������$��Center=�$
��Left=�� $��2�� �� $��0.26* ��� $
��0.5* �.+��� $��2��$��最新软件版本：��$��10 0 0 0q�
�������$��Center=��� -   $ LastCurrentVersion��$��1��$��	2.0.0.0.1��
�������$��Center=�$
��Left=��� $��3��$	 5��$��1q�$��	10 0 10 0q�#��[  ._��� -    ��Name$ 	ChangeLog��$��	10 10 0 0q�$
��Wrap=� �� 
LineHeight$ 20��
���   �$��2��� -    �Name$ DownloadProgressBar��$��
10 20 10 0q�$��4��  ProgressBarStyle#��[ ���#B39DDB���#512DA8 � Value$ 10Q� � Maximum$	 100Q�� $��5��$��
10 10 10 0q�$��Center=�$��Center=� �� $
��0.9* ��� $
��0.1* �.+��� -   $ DownloadVersionText��$��0��$,��&正在下载V1.3.2.2版本的更新包��
��������� -   $ DownloadProgressText��$��1��$
��100%��$��Center=�
�������$
��Left=�