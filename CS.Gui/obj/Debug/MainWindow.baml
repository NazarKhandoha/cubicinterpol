   M S B A M L   `   `   `  ����A  =AP.Gui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:CS.GuiCS.Gui  5   	       CS.Gui.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   W SOxyPlot.Wpf, Version=2014.1.546.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5   	   :d2http://schemas.microsoft.com/expression/blend/2008  6,   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	   guiclr-namespace:CS.Gui  5   	     � Title$  
MainWindow��5   	   $��593.856��6   $��852.373��6-   .��5	      � $��0,0,0,-2q�6    � ColumnDefinitions 5
   
   �� 5      $��200px �6   �� 5      $��* �6   .+�5   
   � $��0��6   $��	0,82,0,91q�6    � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      �� 5       5      �� 5      $
��auto �6#   �� 5      $��* �6#   .+�5      �� $��0��6   $��0��6$   .��61   )'Кількість інтервалів6G   �� 5      $��1��6   $��0��6'   ��64   �� *NumberOfIntervals+6Y   �� 5      $��0��6   $��1��6$   .��61   Ліва межа6<   �� 5      $��1��6   $��1��6'   ��64   �� *	LeftPoint+6Q   �� 5       $��0��6   $��2��6$   .��61   Права межа6=   �� 5!      $��1��6   $��2��6'   ��64   �� *
RightPoint+6R   �� 5#      $��2��6   $��0��6*   $��5��6:   $��25��6m   $	��Top=�6y   $��
27,23,34,0q�6�   $'��!Побудувати сплайн��6�   ��6G   �� *DrawSplineCommand+6�   �� 5$      $��5��6   $��0��6&   $��0,152,10,-37q�66   $��2��6L   $��2��6`   .+�5&      	� $��16(�6I   ��6   �� *SetWorkFunctionCommand+.��6W   exp(x)6_   	� 5'      $��16(�6N   ��6   �� *SetWorkFunctionCos10Command+.��6\   cos(x)6d   	� 5(      $��16(�6O   ��6   �� *SetWorkFunctionCos100Command+.��6]   sin(x)6e   5)      �� 5*      $��Вибір функції��6   $
��Left=�6,   $��
19,116,0,0q�6G   $��5��6[   $	��Top=�6h   $��2��6�   $	��147��6�   $��16(�6�   �� 5+      $,��&Похибка на інтервалі��6   $
��Left=�63   $��	31,81,0,0q�6N   $��6��6a   $	��Top=�6n   $	��135��6�   $��2��6�   5-      �� 5.   
   $��1��6   .@�5/      �� $3��-Графік функції і сплайна��6   .��58        OxyPlot.Wpf.PlotView     Model 6    �� *
SplinePlot+5=      �� 5>      $+��%Коефіцієнти сплайна��6   .��5?      &   System.Windows.Controls.DataGrid     
IsReadOnly .    CanUserSortColumns .  ��6   �� *SplineViewModel.Coefficients+5A      5B      5C      �� 5D   
   $8��2Побудова кубічних сплайнів��6   $
��Left=�65   $��	15,18,0,0q�6P   $	��Top=�6c    J� RenderTransformOrigin$ -0.249,1.427&�6{   $	��167��6�   �� 5E   
   $��img/difference.png��6   $��15,424,35,103q�6,   �� 5F   
   $��22,456,0,57q�6F   ��6   �� *#!ComparisonTable.MaximumDifference+6F   5H      5I      