   M S B A M L   `   `   `  ����  TaskPlannerApp%    TaskPlannerApp.Views.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  TaskPlanner��5   	   $	��600��6   $	��900��6*   ���#1d2125.��5      ]� .+�5
   
   �� $��TaskPlanner��6   $��24(�5      $
��Bold"�6"   
�������$��20q�5      $	��Top=�6    � 5   
   $��20q�6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $��* �6#   �� 5      $
��0.5* �6#   .+�5      #  MaterialDesignThemes.Wpf.Card  $��0��6"   $��10q�62   $��10q�6>   ���#161a1d.��5      �� .+�5      �� $��TO DO��6    $��18(�6.   ���#c2c2c26<   �� 5      -   5         ��Name$ TaskStackPanel��5   !   $��	0,10,0,10q�69   �� 5      -   5         ��Name$ AddTaskButton��5   �   $��➕ Add Task��6   
�������
������ $��16(�6^   $��5q�6l   $��1q�6�   
������ $|�False��6�    � FocusVisualStyle 6�   Z� ��5       �� 5!       �� 
TargetType" ��6$   .�5"   "   ��  �� Property � �� �� Value
 ���� 6?   �� 5#   "    � ��
 ���� 6@   �� 5$   "    � ��$ 1q�6D   �� 5%   "    � ��
 �����6?    �� Triggers 5&   "   P� 5'   &   	 P� Property	 �@J�IsMouseOver
 P� Value$

 True��6E   . �5(   *   ��  � ��
 �����6H   5)   '   5+      5-      52      53        56      $��1��6"   $��10q�62   $��10q�6>   ���#161a1d.��57      �� .+�58      �� $��IN PROGRESS��6    $��18(�64   ���#c2c2c26B   59      5:        5=      $��2��6"   $��10q�62   $��10q�6>   ���#161a1d.��5>      �� .+�5?      �� $
��DONE��6    $��18(�6-   ���#c2c2c26;   5@      5A        5B      $��3��6"   $��10q�62   $��10q�6>   ���#161a1d.��5C      �� 5E      5F      5G      5H      5I      