using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StaticRes;
using System.Reflection;

namespace Hardware.IO_LIST
{
    public class Input
    {
        public static bool X100_Emergency_Stop()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 0, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X101_Pressure_Switch_CDA()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 1, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X102_Loading_Door_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 2, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X103_Thawing_Door_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 3, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X104_Syringe_A_Present()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 4, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X105_Syringe_B_Present()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 5, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X106_Syringe_C_Present()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 6, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X107_Syringe_D_Present()
        {
            if (IO_DLL.PCI_1756.Input_Status(0, 7, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X108_Gate_A_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 0, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X109_Gate_A_Opened()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 1, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X110_Gate_B_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 2, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X111_Gate_B_Opened()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 3, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X112_Gate_C_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 4, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X113_Gate_C_Opened()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 5, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X114_Gate_D_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 6, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X115_Gate_D_Opened()
        {
            if (IO_DLL.PCI_1756.Input_Status(1, 7, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X200_Scrap_Drawer_1_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 0, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X201_Scrap_Drawer_2_Closed()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 1, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X202_Scrap_Chute_CYL_To_Drawer_1()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 2, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X203_Scrap_Chute_CYL_To_Drawer_2()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 3, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X204_Weighing_Tray_CYL_FWD()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 4, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X205_Weighing_Tray_CYL_BWD()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 5, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X206_Mix_Cover_On()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 6, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X207_Mix_Material_In()
        {
            if (IO_DLL.PCI_1756.Input_Status(2, 7, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X208_Ejector_A_Up()
        {
            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In Function");

            int result = IO_DLL.PCI_1756.Input_Status(3, 0, Global.Device_Handle_IO_PCI1756);

            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "IO Result: " + result);

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X209_Ejector_A_Down()
        {
            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "In Function");

            int result = IO_DLL.PCI_1756.Input_Status(3, 1, Global.Device_Handle_IO_PCI1756);

            Common.Reports.LogFile.DebugLog(MethodBase.GetCurrentMethod(), "IO Result: "+ result);

            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }


          
        }

        public static bool X210_Ejector_B_Up()
        {
            if (IO_DLL.PCI_1756.Input_Status(3, 2, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X211_Ejector_B_Down()
        {
            if (IO_DLL.PCI_1756.Input_Status(3, 3, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X212_Ejector_C_Up()
        {
            if (IO_DLL.PCI_1756.Input_Status(3, 4, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X213_Ejector_C_Down()
        {
            if (IO_DLL.PCI_1756.Input_Status(3, 5, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X214_Ejector_D_Up()
        {
            if (IO_DLL.PCI_1756.Input_Status(3, 6, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X215_Ejector_D_Down()
        {
            if (IO_DLL.PCI_1756.Input_Status(3, 7, Global.Device_Handle_IO_PCI1756) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X300_Weighing_Door_Opened()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 0, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X301_Weighing_Door_Closed()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 1, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X302_Syringe_Top_Cover_Present()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 2, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X303_30cc_Syringe_Cap_Present()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 3, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X304_10cc_Syringe_Cap_Present()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 4, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X305_5cc_Syringe_Cap_Present()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 5, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X306_Syringe_Present()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 6, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool X307_Spare()
        {
            if (IO_DLL.PCI_1733.Input_Status(0, 7, Global.Device_Handle_IO_PCI1733) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static int[] IO_1756_Status()
        {
            return Hardware.IO_DLL.PCI_1756.Input_Status_Serial(Global.Device_Handle_IO_PCI1756);
        }

        public static int[] IO_1733_Status()
        {
            return Hardware.IO_DLL.PCI_1733.Input_Status_Serial(Global.Device_Handle_IO_PCI1733);
        }


    }
}