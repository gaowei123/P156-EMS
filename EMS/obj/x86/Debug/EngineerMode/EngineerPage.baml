   M S B A M L   `   `   `  ����>  :EMS, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:EMS.EngineerModeEMS.EngineerMode  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   #    EMS.EngineerMode.EngineerPage   5   	   R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     8x,http://schemas.microsoft.com/winfx/2006/xaml    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   Hmc;http://schemas.openxmlformats.org/markup-compatibility/2006    6F   .localclr-namespace:EMS.EngineerMode    5   	   7System&clr-namespace:System;assembly=mscorlib    5   	     � Title$   ��5   	    � 
ResizeMode$ NoResize=�6    � WindowStartupLocation$ CenterScreen=�6)    � WindowStyle$
 None=�6N   ���	#4C000000 � AllowsTransparency . $	��850��6�   $	��700��6�    � Topmost .  � 	Resources 5	         	MM_Homing��%(   &	        ��5
      
Homing��6,   5      .��5      �  � RowDefinitions 5      �� 5      $��0.106* �6   �� 5      $��0.894* �6   .+�5      �� $��5q�6   
���   �$��2��6+   ���	#FFF7F3F36<   �� 5      $��
8,-1.2,8,8q�6   $��1��6&     Linear_Yellow#���  6I   $��1��6w   ��63   Z� .@�5      �� $��User DB��6     
Linear_Red#���  6'   
���   �$��13.333(�6e   $��
-2,0,-2,-1q�6w   ��5   	   �� 5   
    �� EndPoint$ 0.5,0&�6   	 �� 
StartPoint$	 0.5,1&�6/   .��5      � 
 � Color$
 	#FFEDF0F3��6    � Offset$ 0.336Q�6*   � 5      $
 	#FF12FF0A��6   $ 1Q�6*   5      .��5   	   � $��0,0,0,0q�6   .+�5   
      EMS.EngineerMode.UserDB  $��0q�6   5   
   5   	   �� 5      $��Event DB��6   #���  6"   $��13.333(�6M   ��5   	   �� 5    
   $ 0.5,0&�6   $	 0.5,1&�6/   .��5!      � $
 	#FFF4FBF5��6   $ 0.355Q�6*   � 5"      $
 	#FF08F717��6   $ 1Q�6*   5#      .��5%   	   � $��0,0,0,-3q�6   .+�5&   
      EMS.EngineerMode.EventDB  $��1,0.266,0,-0.666q�6   5'   
   5(   	   �� 5)      $��
History DB��6   #���  6!   $��13.333(�6L   ��5*   	   �� 5+   
   $ 0.5,0&�6   $	 0.5,1&�6/   .��5,      � $
 	#FFF4FBF5��6   $ 0.355Q�6*   � 5-      $
 	#FF08F717��6   $ 1Q�6*   5.      .��50   	   � $��0,0,0,-3q�6   .+�51   
       EMS.EngineerMode.HistoryDB  $��0,0.266,0,-0.666q�6   53   
   54   	   �� 55      $��Tracking DB��6   #���  6"   $��13.333(�6M   $��
-2,0,-2,-1q�6_   ��56      �� 57      $ 0.5,0&�6   $	 0.5,1&�6,   .��58      � $
 	#FFF4FBF5��6   $ 0.355Q�6'   � 59      $
 	#FF08F717��6   $ 1Q�6'   5:      .��5<      � $��0,0,0,-3q�6   .+�5=      !   EMS.EngineerMode.TrackingDB  $��0,0.266,0,-0.535q�6   5?      5@      �� 5A      $��Equipment DB��6   #���  6#   $��13.333(�6N   $��
-2,0,-2,-1q�6`   ��5B      �� 5C      $ 0.5,0&�6   $	 0.5,1&�6,   .��5D      � $
 	#FFF4FBF5��6   $ 0.355Q�6'   � 5E      $
 	#FF08F717��6   $ 1Q�6'   5F      .��5H      � $��0,0,0,-3q�6   .+�5I          EMS.EngineerMode.MachineDB  $��0,0.266,0,-0.269q�6   5K      5L      5M      �� 5O      $��
20,8,0,5.2q�6   $
��Wrap=�6%   $��Engineer Mode��69   $��50.667(�6N   $��Arial)�6`   $
��Left=�6s   $	��369��6�   ��5P      �� 5Q   	   $ 0.5,1&�6   $	 0.5,0&�6.   .��5R   
   � $
 	#FF047C8B��6   � 5S   
   $
 	#FF85F7F7��6   $ 0.722Q�6)   5T   
    J� Effect 5V      3  -System.Windows.Media.Effects.DropShadowEffect  5W   	      ShadowDepth$ 3Q�6   5Y      �� 5Z      -   5         ��Name$ 	btn_close��5Z      $��62.25��6!   $��Right=�6/   $��0,15.549,16.25,6.917q�6K     ButtonNormalBackground#���  6i     ButtonStyle-Zoom#���  6�   
���   �|�.  ��.   � Cursor$
 Hand��6  .��5[      �� $��44.667��6   $��56.083��6   $.��(/EMS;component/Resources/Image/Close.png��6-   5\      5^      5_      