using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaticRes;

namespace Hardware.IO_LIST
{
    public class Output
    {
        #region Y100_Tower_Light_Green_On/Off
        public static bool Y100_Tower_Light_Green_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 0, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y100_Tower_Light_Green_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 0, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y101_Tower_Light_Yellow_On/Off
        public static bool Y101_Tower_Light_Yellow_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 1, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y101_Tower_Light_Yellow_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 1, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y102_Tower_Light_Red_On/off
        public static bool Y102_Tower_Light_Red_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 2, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y102_Tower_Light_Red_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 2, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y103_Tower_Buzzer_On/off
        public static bool Y103_Tower_Buzzer_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 3, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y103_Tower_Buzzer_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 3, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y104_Gate_A_Open/Close
        public static bool Y104_Gate_A_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 4, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y104_Gate_A_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 4, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y105_Gate_B_Open/Close
        public static bool Y105_Gate_B_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 5, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y105_Gate_B_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 5, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y106_Gate_C_Open/Close
        public static bool Y106_Gate_C_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 6, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y106_Gate_C_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 6, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y107_Gate_D_Open/Close
        public static bool Y107_Gate_D_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 7, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y107_Gate_D_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(0, 7, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y108_Spare_On/Off
        public static bool Y108_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 0, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y108_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 0, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y109_Weihging_Door_Open/Close
        public static bool Y109_Weighing_Door_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 1, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y109_Weighing_Door_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 1, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y110_Weighing_Tray_FWD/BWD
        public static bool Y110_Weighing_Tray_Backward()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 2, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y110_Weighing_Tray_Forward()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 2, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y111_Scrap_Bin_Drawer_Chute_Open/Close
        public static bool Y111_Scrap_Bin_Drawer_Chute_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 3, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y111_Scrap_Bin_Drawer_Chute_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 3, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y112_Thawing_Cover_Open/Close
        public static bool Y112_Thawing_Cover_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 4, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y112_Thawing_Cover_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 4, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y113_Scrap_Bin_Drawer_1_Open/Close
        public static bool Y113_Scrap_Bin_Drawer_1_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 5, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y113_Scrap_Bin_Drawer_1_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 5, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y114_Scrap_Bin_Drawer_2_Open/Close
        public static bool Y114_Scrap_Bin_Drawer_2_Open()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 6, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y114_Scrap_Bin_Drawer_2_Close()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 6, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y115_Spare
        public static bool Y115_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 7, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y115_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(1, 7, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y200_Ejector_A_Up/Down
        public static bool Y200_Ejector_A_Up()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 0, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y200_Ejector_A_Down()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 0, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y201_Ejector_B_Up/Down
        public static bool Y201_Ejector_B_Up()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 1, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y201_Ejector_B_Down()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 1, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y202_Ejector_C_Up/Down
        public static bool Y202_Ejector_C_Up()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 2, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y202_Ejector_C_Down()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 2, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y203_Ejector_D_Up/Down
        public static bool Y203_Ejector_D_Up()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 3, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y203_Ejector_D_Down()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 3, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion
        




        #region Y204_Mix_Power On/Off
        public static bool Y204_MixPower_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 4, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y204_MixPower_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 4, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion





        #region Y205_Spare
        public static bool Y205_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 5, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y205_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 5, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y206_Spare
        public static bool Y206_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 6, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y206_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 6, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion
        
        #region Y207_Spare
        public static bool Y207_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 7, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y207_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(2, 7, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y208_Spare
        public static bool Y208_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 0, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y208_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 0, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y209_Spare
        public static bool Y209_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 1, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y209_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 1, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y210_Sapre
        public static bool Y210_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 2, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y210_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 2, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y211_Spare
        public static bool Y211_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 3, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y211_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 3, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y212_Spare
        public static bool Y212_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 4, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y212_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 4, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y213_Spare
        public static bool Y213_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 5, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y213_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 5, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y214_Spare
        public static bool Y214_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 6, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y214_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 6, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion

        #region Y215_Spare
        public static bool Y215_Spare_On()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 7, 1, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }

        public static bool Y215_Spare_Off()
        {
            if (IO_DLL.PCI_1758.Output_Excut(3, 7, 0, Global.Device_Handle_IO_PCI1756))
                return true;
            else
                return false;
        }
        #endregion
    }
}
