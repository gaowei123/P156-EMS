   M S B A M L   `   `   `  ����>  :EMS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:EMS.Views	EMS.Views  5   	        EMS.Transaction.ScrapClean   5   	   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5   	   +	EMS_Viewsclr-namespace:EMS.Views    5   	     � Title$   ��5	   	    � 
ResizeMode$ NoResize=�6    � WindowStartupLocation$ CenterScreen=�6)    � WindowStyle$
 None=�6N   ���	#4C000000 � AllowsTransparency . $	��828��6�   $	��550��6�    � Topmost . .��5
      � $��0,0,8,0q�6    � RowDefinitions 5      �� 5      .+�5      �� $��8,8,-2,8q�6   
���   �   ButtonNormalBackground#���   62    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5   	   5      �� 5      -   5         ��Name$ 	btn_reset��5      $��186.937,0,0,40.957q�6     ButtonStyle-Zoom#���  6;   	 � Cursor$
	 Hand��6f   $��40��6t   $��Bottom=�6�   #���   6�   $
��Left=�6-  $	��125��6H   5        5   	   
   ShadowDepth$
 0Q�6      
BlurRadius$ 8Q�6*   .��5      � $
��38.5��6   $	��121��6   .+�5   	   �� $��Reset��6   $��Right=�6    $��18(�6C   $
��Bold"�6Q   $ ��25.664,-1.667,32.836,5.167q�6c   5   	   5      �� 5      -   5        $ 	btn_close��5      $��0,0,191.459,43.123q�6   #���  6;   $
	 Hand��6f   $��40��6t   $��Bottom=�6�   #���   6�   $��Right=�6  $	��125��61   5        5      $
 0Q�6   $ 8Q�6(   .��5      � $��33.333��6   $��118.667��6   .+�5       �� $��Close��6   $��18(�6   $��Right=�6+   $
��Bold"�6N   $ ��27.999,-1.499,29.334,2.499q�6`   5!      5"      �� 5#      -   5         ��Name$ txt_currentScrapQty��5#      $��268.333,203.666,104.78,0q�6*    �� TextWrapping$
 Wrap=�6L    J� RenderTransformOrigin$ 0.453,0.425&�6`   $��16(�6�   {�.  $��30.627��6�   $	��Top=�6�   .��6�    5$        5%      $
 0Q�6   $ 8Q�6(   .��5&      5'      �� 5(      -   5        $ txt_scrapQtyLimit��5(      $��269.999,0,105.781,234.705q�6(   $
 Wrap=�6K   $ 0.453,0.425&�6_   $��16(�6�   {�.  $��30.627��6�   $��Bottom=�6�   .��6�    5)        5*      $
 0Q�6   $ 8Q�6(   .��5+      5,      �� 5-      $��Current Scrap Qty :��6   $
��Left=�6)   $��112,204,0,0q�6D   $��146.993��6Y   $��16(�6i   ���	#FF020202$��34.96��6�   $	��Top=�6�   ���	#000A0A0A6�   �� 5.      $��Scrap Qty Limit :��6   $
��Left=�6'   $��130.667,0,0,233.371q�6B   $��130.326��6_   $��16(�6o   ���	#FF0A0A0A$��30.96��6�   $��Bottom=�6�   �� 5/      $��Reset Current Scrap Qty ��6   $
��Left=�6.   $��Center=�6I   $��110.697,18,0,0q�6m   $��52(�6�   $
��Bold"�6�   $��88��6�   $��616.697��6�   $	��Top=�6�    50        51      $
 3Q�6   ��53      �� 54       �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6,   .��55      �  � Color$ 	#FF017269��6    � Offset$ 0Q�6'   � 56      $ 	#FF82F5EE��6   $ 0.763Q�6'   57      59      5:      5;      