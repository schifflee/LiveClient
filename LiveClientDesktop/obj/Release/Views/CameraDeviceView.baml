   M S B A M L   `   `   `  ����   LiveClientDesktop.    (LiveClientDesktop.Views.CameraDeviceView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .���   � ColumnDefinitions  �� $	��30* �.+�o� 
������ $��Center=�.@�m� ���#323232$�� ��   TransparentHeaderMenuItem#��[   m� Icon$	 ������ *CameraDeviceList+����  �� 
TargetType" m�.���  �� Property � �� �� Value �� *OwnerVideoDevice.Name+��  � {� �� *OwnerVideoDevice.IsEnable+��  �@m�Command ��  �� Path$% DataContext.ChangeCameraCommand� �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType" m���  �@m�CommandParameter �� $ OwnerVideoDevice.ID���  � ��$ 95����  � �� �� 	 �� 
TargetType"	 m�.R�� 
 � ToolTip#
 z��$��24��#��z��  �� $��15 ��� $��* �.+���  ��Name$ menuIcon��$	�������#188CFF 	 FIcon#��[ y��� *ShowSelectIcon+��  ��Name$ _border��$��2���� $
 _txt��$��5 0 0 0q�$��WordEllipsis=�$��1��$��Center=����� *
Foreground+���� *Header+ �� *TemplatedParent+ �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  �� 
TargetName$ _border�� �� *
Background+ � ���� $
 _txt�� � �� �#188CFF�� $ menuIcon�� � �� �#188CFF�� $ _border�� �@�Cursor$
 Hand���� $
 _txt�� �@�Cursor$
 Hand��P�  � {� .  . ��� $
 _txt�� � ��
 �����