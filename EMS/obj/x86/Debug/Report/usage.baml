   M S B A M L   `   `   `  ����Y  UWPFVisifire.Charts, Version=4.0.2.2, Culture=neutral, PublicKeyToken=9f1c752402bc35c9M9clr-namespace:Visifire.Charts;assembly=WPFVisifire.ChartsVisifire.Charts  5       EMS   EMS.Report.Usage   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      SVisifire_Charts9clr-namespace:Visifire.Charts;assembly=WPFVisifire.Charts    5      :Custom-http://schemas.microsoft.com/wpf/2008/toolkit  5      .��5
      �   � ColumnDefinitions  5      �� 5       � RowDefinitions 5      �� 5      .+�5      � -   5         �Name$ 
LayoutRoot��5        5      �� 5   	   .+�5      � -   5        $ 	gd_output��5      $��1.5,1.5,0,0.5q�6    .+�5   	   +  %Microsoft.Windows.Controls.DatePicker  -   5          Name$ dp_from��5      $��130.5,0,0,49.994q�6-   $��Bottom=�6G   $
��Left=�6b   $��18.667(�6}   $��33.83��6�   $��191.217��6�    J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5      5   
     5   	   -   5        $ dp_to��5      $��129.5,0,0,9.663q�6*   $��Bottom=�6C   $
��Left=�6^   $��18.667(�6y   $��33.83��6�   $��191.217��6�    5   
     5      5   
   �� 5    	   $��	Date From��6   $
��Left=�6#   $��38.5,0,0,52.534q�6>   $��Bottom=�6W   $��16(�6�   ���	#00EEFB05���	#FF0E0E0E6�   �� 5!   	   $��Date To��6   $
��Left=�6!   $��56.833,0,0,13.203q�6<   $��Bottom=�6W   $��16(�6r   ���	#FF040404$��69.957��6�   �� 5"   	   $��0,0,85.329,4.83q�6    �� Orientation$ 
Horizontal=�6-   $��42.794��6F   $��Bottom=�6V   $��Right=�6q   $��104.666��6�   .+�5#   
   �� -   5         ��Name$ 
btn_search��5#         ButtonStyle-Zoom#���   6%   $	��100��6P   $��13.333(�6\   $��1,1,1,1q�6n   $��0q�6�   $��Right=�6�   $��38��6�   $	��Top=�6�    � Cursor$
 Hand��6�     ButtonNormalBackground#���  6�   ���	#FFF3F2F2 5$        5%         ShadowDepth$ 0Q�6   	   
BlurRadius$	 8Q�6-   .��5'      � $��31.874��6   $��93.832��6    .+�5(      ��   	EP_search#���  6   $ ��38.334,-1.425,-2.168,4.664q�68   $��Center=�6\   $��16(�6�   ���	#FF0606066�   �� 5)      $��2.166,1.898,0,1.602q�6   $
��Left=�6/   $
��38.5��6J   $/��)/EMS;component/Resources/Image/search.png��6W   5*      5+      5,   
   �� 5-   	   $��Report Type��6   $
��Left=�6%   $��353.833,0,0,10.863q�6@   $��Bottom=�6\   $��16(�6w   ���	#000C0C0C���	#FF0C0C0C6�   �� 5.   	   -   5        
 ��Name$
 cb_reportType��5.      $��450.499,0,420.001,12.203q�6)   $��Bottom=�6K   $��16(�6f    5/   
     50      $ 0Q�6   $	 10Q�6,   .@�52      �� $��Alarm Pareto��6   �� 53      $��Alarm Trend��6   �� 54      $��Usage Trend��6   55   
      Visifire.Charts.Chart  56   	   -   5          Name$ MyChart��56         Theme$ Theme1��60      
Background
 ����    View3D . $��2.5,-0.716,0.288,91.716q�6}    J� RenderTransformOrigin$ 0.5,0.5&�6�   ��6f   Z� z�57   
   V� 58      .~�59      �� �� 5:      �� 5;      T� 5<      5=         Series 5@          Visifire.Charts.DataSeries     RenderAs$ Column=�6&   $��0,0,0,12q�68      
DataPoints 5B         Visifire.Charts.DataPoint     
AxisXLabel$ 1��6'      YValue0   *Visifire.Commons.Converters+ValueConverter$ 50 66     5C      $ 2��6'   $ 35 66     5D      $ 3��6'   $ 27 66     5E      $ 4��6'   $ 17 66     5F      $ 5��6'   $ 26 66     5G      $ 6��6'   $ 36 66     5H      $ 7��6'   $ 6 66     5I      $ 8��6'   $ 46 66   5K         Titles 5N         Visifire.Charts.Title     Text$ Charting��6!   5P   
   5Q   	   5R      � 5S      -   5         �Name$ Inventory_Pie��5S      $��0,0,0,0q�6(   $ 0.5,0.5&�69   $��Center=�6Y   $��Center=�6v   z�5T      V� 5U   	   .~�5V   
   �� �� 5W   
   �� 5X   
   T� 5Y   
   5Z   
   5\      5^      5_      