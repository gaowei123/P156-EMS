   M S B A M L   `   `   `  ����>  :EMS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:EMS.AdminModeEMS.AdminMode  5      clr-namespace:EMSEMS  5      -clr-namespace:EMS.AdminModeEMS.AdminMode  5          EMS.AdminMode.AdminPage   5   	   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    5      +localclr-namespace:EMS.AdminMode    5      >d2http://schemas.microsoft.com/expression/blend/2008    5      Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    5      EMSclr-namespace:EMS    5      6EMS_EngineerModeclr-namespace:EMS.AdminMode    5        � Title$   ��5
   	    � 
ResizeMode$ NoResize=�6    � WindowStartupLocation$ CenterScreen=�6:    � WindowStyle$
 None=�6_   ���	#4C000000 � AllowsTransparency . $	��850��6�   $	��700��6�    � Topmost . .��5      �  � RowDefinitions 5      �� 5      $��0.096* �6   �� 5      $��0.904* �6   .+�5      �� $��5q�6   
���   �$��2��6+   ���	#FFFDFCFC6<   �� 5      $��8,0.766,8,8q�6   $��1��6'      Linear_Yellow#���   64   ��6b   Z� .@�5      ��   AP_SAPDB#���  6     
Linear_Red#���  6:   
���   �$��13.333(�6x   ��5   	   �� 5   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      � 	 � Color$	 	#FFF0F3F3��6   
 � Offset$

 0.37Q�6*   � 5      $	 	#FF37FD02��6   $
 1Q�6*   5      .��5   	   � $��0,0,0,-3q�6   .+�5   
      EMS.AdminMode.SapDB  $��-1,-0.7,-2,-2.999q�6!   5   
   5   	   �� 5        AP_Department DB#���  6   #���  6B   
���   �$��13.333(�6�   $��-2,-2,-2,-1q�6�   ��5   	   �� 5   
   $ 0.5,1&�6   $ 0.5,0&�6/   .��5       � $	 	#FFF0F3F3��6   $

 0.37Q�6*   � 5!      $	 	#FF37FD02��6   $
 1Q�6*   5"      .��5$   	   � $��0,0,0,-3q�6   .+�5%   
       EMS.AdminMode.DepartmentDB  $��	-1,5,0,-8q�6(   5&   
   5(   	   �� 5)        AP_systemSet#���  6   #���  68   
���   �$��13.333(�6v   $��-2,-2,-2,-1q�6�   ��5*   	   �� 5+   
   $ 0.5,1&�6   $ 0.5,0&�6/   .��5,      � $	 	#FFF0F3F3��6   $

 0.37Q�6*   � 5-      $	 	#FF37FD02��6   $
 1Q�6*   5.      .��50   	      EMS.AdminMode.SystemSet     EMS.AdminMode.SystemSet   Loaded$ SystemSet_Loaded��6[   $
��Auto��6$   $
��Auto��62   $��-3,-2.35,-5,-7.349q�6?   6[   52      53      �� 55      $��
10,8,276,3q�6   $
��Wrap=�6%     AP_adminPage#���  69   $��40(�6_   $��Arial)�6m   ��56      �� 57   	   $ 0.5,1&�6   $ 0.5,0&�6.   .��58   
   � $	 	#FF0A7E8D��6   � 59   
   $	 	#FF89F7F7��6   $

 0.73Q�6)   5:   
    J� Effect 5<      3  -System.Windows.Media.Effects.DropShadowEffect  5=   	      ShadowDepth$ 3Q�6   5?      �� 5@      -   5         ��Name$ 	btn_close��5@      $��63��6!   $��Right=�6,   $��0,13.966,16,3.234q�6H     ButtonStyle-Zoom#���  6y   
���   �|�.  ��.   � Cursor$
 Hand��6�   ��6c   Z� .��5A      �� $��42��6   $��55��6   $.��(/EMS;component/Resources/Image/Close.png��6%   5B      5D      5E      