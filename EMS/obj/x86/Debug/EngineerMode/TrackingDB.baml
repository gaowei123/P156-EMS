   M S B A M L   `   `   `  ����>  :EMS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:EMS.Views	EMS.Views  5   K   !    EMS.EngineerMode.TrackingDB   5      R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      :Custom-http://schemas.microsoft.com/wpf/2008/toolkit  5      '	EMS_Viewsclr-namespace:EMS.Views  6K   .��5	      � .+�5      )  #Microsoft.Windows.Controls.DataGrid  -   5           Name$  dg_list��5   )      
IsReadOnly . $��0,0,0,268.536q�6:      AlternatingRowBackground �	#FFC1F5EA   Linear_RainySky#���   6v   F� 5      ���� $� ��ь�vI��	���	#FFF4F4F5$��2��6=   $��0,0,0,176.388q�6H    �� Stretch$
 Fill=�6_   $��Bottom=�6n   $��2��6�     Linear_Blue#���  6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5   	      Color$ 	#FFB2B2B2��6      ShadowDepth$ 2Q�6,   5      �� 5      $��0,0,8,183.388q�6    �� Orientation$ 
Horizontal=�6)   $��75.128��6B   $��Bottom=�6R   $��Right=�6m   $	��109��6�   .+�5      �� -   5         ��Name$ 
btn_search��5        ButtonStyle-Zoom#���  6#   $	��100��6N   $��13.333(�6Z   $��1,1,1,1q�6l   $��0q�6�   $��Right=�6�   $��72��6�   $	��Top=�6�   	 � Cursor$
	 Hand��6�     ButtonNormalBackground#���  6�   ���	#FFF3F2F2 5   	     5   
   $ 0Q�6   
   
BlurRadius$
 8Q�6+   .��5   	   � $��63.874��6   $��92��6   .+�5   
   �� $��	2,0,-2,12q�6   $/��)/EMS;component/Resources/Image/search.png��6$   �� 5   
     	EP_search#���  6   $��	2,0,-2,-4q�66   $��Center=�6I   $��16(�6m   $��27.968��6{   $��Bottom=�6�   ���	#FF0A0A096�   5   
   5   	   5      �� 5        	EP_partID#���  6   $
��Left=�63   $��26,0,0,225.923q�6N   $��14.667(�6f   ���	#FF0A0A09$��Bottom=�6�   �� 5      -   5         ��Name$ 
txt_partID��5      $
��Left=�6#    �� CharacterCasing$ Upper=�6>   $��117.486,0,0,228.62q�6V    �� TextWrapping$ NoWrap=�6r   $��173.487��6�   $��Bottom=�6�   .��6�    5        5   	   $ 0Q�6   $
 8Q�6*   .��5       5!      �� 5"        EP_mcID#���  6   $
��Left=�61   $��24.579,0,0,187.78q�6L   $��14.667(�6g   ���	#FF0A0A09$��Bottom=�6�   $��92.907��6�   �� 5#      -   5        $ txt_mcID��5#      $
��Left=�6!   $ Upper=�6<   $��117.486,0,0,190.783q�6T   $ NoWrap=�6q   $��173.487��6�   $��Bottom=�6�   .��6�    5$        5%   	   $ 0Q�6   $
 8Q�6*   .��5&      5'         EMS.Views.KeyBoard  5(      -   5          Name$ kb��5(      $��5,0,10,1q�6&   $��Bottom=�68   5+      5,      