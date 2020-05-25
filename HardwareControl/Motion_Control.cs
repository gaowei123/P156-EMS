using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advantech.Motion;
using System.Runtime.InteropServices;


namespace HardwareControl
{
    public class Motion_Control
    {
        public static bool Open_Connection()
        {
            uint Result;
            int result = 0;
            uint i = 0;
            uint deviceCount = 0;
            uint[] slaveDevs = new uint[16];
            uint AxesPerDev = new uint();
            uint AxisNumber;
            uint buffLen = 0;
            DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];
            result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
            //result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);
            if (result != (int)ErrorCode.SUCCESS)
                throw new System.Exception("Can not Get Available Device !!");

            Result = Motion.mAcm_DevOpen(CurAvailableDevs[0].DeviceNum, ref StaticRes.Global.Motor_Parameter.m_DeviceHandle);
            if (Result != (uint)ErrorCode.SUCCESS)
               throw new System.Exception("Can not open connection of motion card PCI-1465 !!");
            buffLen = 4;
            Result = Motion.mAcm_GetProperty(StaticRes.Global.Motor_Parameter.m_DeviceHandle, (uint)PropertyID.FT_DevAxesCount, ref AxesPerDev, ref buffLen);
            if (Result != (uint)ErrorCode.SUCCESS)
                throw new System.Exception("Can not get Property of motion card PCI-1465 !!");
            AxisNumber = AxesPerDev;
            buffLen = 64;
            Result = Motion.mAcm_GetProperty(StaticRes.Global.Motor_Parameter.m_DeviceHandle, (uint)PropertyID.CFG_DevSlaveDevs, slaveDevs, ref buffLen);
            if (Result == (uint)ErrorCode.SUCCESS)
            {
                i = 0;
                while (slaveDevs[i] != 0)
                {
                    AxisNumber += AxesPerDev;
                    i++;
                }
            }

            StaticRes.Global.Motor_Parameter.m_ulAxisCount = AxisNumber;

            for (i = 0; i < StaticRes.Global.Motor_Parameter.m_ulAxisCount; i++)
            {
                //Open every Axis and get the each Axis Handle
                //And Initial property for each Axis 		

                //Open Axis 
                Result = Motion.mAcm_AxOpen(StaticRes.Global.Motor_Parameter.m_DeviceHandle, (UInt16)i, ref StaticRes.Global.Motor_Parameter.m_Axishand[i]);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Open Axis Failed !!");

                //Reset Command Counter
                double cmdPosition = new double();
                cmdPosition = 0;
                Motion.mAcm_AxSetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[i], cmdPosition);
            }
            StaticRes.Global.Motor_Parameter.m_bInit = true;

            Result = Motion.mAcm_DevLoadConfig(StaticRes.Global.Motor_Parameter.m_DeviceHandle,
                System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "motion_setting.cfg");
            if (Result != (uint)ErrorCode.SUCCESS)
                throw new System.Exception("Load configure file failed !!");
            return true;
        }

        public static bool Close_Connection()
        {
            UInt16[] usAxisState = new UInt16[32];
            uint AxisNum;
            //Stop Every Axes
            if (StaticRes.Global.Motor_Parameter.m_bInit == true)
            {
                for (AxisNum = 0; AxisNum < StaticRes.Global.Motor_Parameter.m_ulAxisCount; AxisNum++)
                {
                    Motion.mAcm_AxGetState(StaticRes.Global.Motor_Parameter.m_Axishand[AxisNum], ref usAxisState[AxisNum]);
                    if (usAxisState[AxisNum] == (uint)AxisState.STA_AX_ERROR_STOP)
                    {
                        Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[AxisNum]);
                    }
                    Motion.mAcm_AxStopDec(StaticRes.Global.Motor_Parameter.m_Axishand[AxisNum]);
                }

                //Close Axes
                for (AxisNum = 0; AxisNum < StaticRes.Global.Motor_Parameter.m_ulAxisCount; AxisNum++)
                {
                    Motion.mAcm_AxClose(ref StaticRes.Global.Motor_Parameter.m_Axishand[AxisNum]);
                }
                StaticRes.Global.Motor_Parameter.m_ulAxisCount = 0;
                //Close Device
                Motion.mAcm_DevClose(ref StaticRes.Global.Motor_Parameter.m_DeviceHandle);
                StaticRes.Global.Motor_Parameter.m_DeviceHandle = IntPtr.Zero;
                StaticRes.Global.Motor_Parameter.m_bInit = false;
            }
            return true;
        }

        public static bool Rotary_Servo_On()
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxSetSvOn(StaticRes.Global.Motor_Parameter.m_Axishand[0], 1);

                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Turn on Rotary Servo failed !!");
            }
            return true;
        }

        public static bool Spare_Servo_On()
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxSetSvOn(StaticRes.Global.Motor_Parameter.m_Axishand[1], 1);

                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Turn on Arm Y Servo failed !!");
            }
            return true;
        }

        public static bool Rotary_MoveABS(double Position)
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxMoveAbs(StaticRes.Global.Motor_Parameter.m_Axishand[0], Position);

                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception(StaticRes.Global.Motion_Error.Rotary_Alarm);
            }
            return true;
        }

        public static bool Spare_MoveABS(double Position)
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxMoveAbs(StaticRes.Global.Motor_Parameter.m_Axishand[1], Position);

                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception(StaticRes.Global.Motion_Error.Y_Axis_Alarm);
            }
            return true;
        }

        public static bool Rotary_Move(double Distance)
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxMoveRel(StaticRes.Global.Motor_Parameter.m_Axishand[0], Distance);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception(StaticRes.Global.Motion_Error.Rotary_Alarm);
            }
            return true;
        }

        public static bool Spare_Move(double Distance)
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                if (Arm_Y_LimitPositive_Sensor_On() == true || Arm_Y_LimitNagative_Sensor_On() == true)
                    Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[1]);
                Result = Motion.mAcm_AxMoveRel(StaticRes.Global.Motor_Parameter.m_Axishand[1], Distance);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception(StaticRes.Global.Motion_Error.Y_Axis_Alarm);
            }
            return true;
        }

        public static bool Rotary_Continue_Move()
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxMoveVel(StaticRes.Global.Motor_Parameter.m_Axishand[0],0);//yakun.zhou 2015-07-06(0==>1)

                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception(StaticRes.Global.Motion_Error.Rotary_Alarm);
            }
            return true;
        }

        public static bool Rotary_Motion_Stop()
        {
            UInt16 AxState = new UInt16();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Motion.mAcm_AxGetState(StaticRes.Global.Motor_Parameter.m_Axishand[0], ref AxState);
                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[0]);
                }
                Motion.mAcm_AxStopDec(StaticRes.Global.Motor_Parameter.m_Axishand[0]);
            }
            return true;
        }

        public static bool Spare_Motion_Stop()
        {
            UInt16 AxState = new UInt16();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                //if axis is in error state , reset it first. then Stop Axes
                Motion.mAcm_AxGetState(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref AxState);
                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                { Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[1]); }

                Motion.mAcm_AxStopDec(StaticRes.Global.Motor_Parameter.m_Axishand[1]);
            }
            return true;
        }

        public static bool Rotary_EMG_Stop()
        {
            UInt16 AxState = new UInt16();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                //if axis is in error state , reset it first. then Stop Axes
                Motion.mAcm_AxGetState(StaticRes.Global.Motor_Parameter.m_Axishand[0], ref AxState);
                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                { Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[0]); }

                Motion.mAcm_AxStopEmg(StaticRes.Global.Motor_Parameter.m_Axishand[0]);
            }
            return true;
        }

        public static bool Spare_Emg_Stop()
        {
            UInt16 AxState = new UInt16();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                //if axis is in error state , reset it first. then Stop Axes
                Motion.mAcm_AxGetState(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref AxState);
                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                { Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[1]); }

                Motion.mAcm_AxStopEmg(StaticRes.Global.Motor_Parameter.m_Axishand[1]);
            }
            return true;
        }

        public bool Check_and_Reset_cmd_Position(int Axis_No)
        {
            try
            {
                double actualPosition = Got_Rotary_Position();
                double cmdPosition = Got_Rotary_cmdPosition();
                if (Math.Abs(actualPosition - cmdPosition) > 100)
                    Motion.mAcm_AxSetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[0], actualPosition);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static double Got_Rotary_cmdPosition()
        {
            double CurPosition = 0;
            Motion.mAcm_AxGetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[0], ref CurPosition);
            return CurPosition;
        }

        public static double Got_Rotary_Position()
        {
            double CurPosition = 0;
            Motion.mAcm_AxGetActualPosition(StaticRes.Global.Motor_Parameter.m_Axishand[0], ref CurPosition);
            return CurPosition;
        }
        public static bool Check_and_Reset_cmd_Position()
        {
            try
            {
                double actualPosition = Got_Rotary_Position();
                double cmdPosition = Got_Rotary_cmdPosition();
                if (Math.Abs(actualPosition - cmdPosition) > 100)
                    Motion.mAcm_AxSetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[0], actualPosition);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static double Got_Spare_Position()
        {
            double CurPosition = 0;
            Motion.mAcm_AxGetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref CurPosition);
            return CurPosition;
        }

        public static double Got_Rotary_Speed()
        {
            double Velocity = 0;
            Motion.mAcm_AxGetCmdVelocity(StaticRes.Global.Motor_Parameter.m_Axishand[0], ref Velocity);
            return Velocity;
        }

        public static double Got_Spare_Speed()
        {
            double Velocity = 0;
            Motion.mAcm_AxGetCmdVelocity(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref Velocity);
            return Velocity;
        }

        public static uint Got_Rotary_Status()
        {
            uint Status=0;
            Motion.mAcm_AxGetMotionStatus(StaticRes.Global.Motor_Parameter.m_Axishand[0], ref Status);
            return Status;
        }

        public static uint Got_Spare_Status()
        {
            uint Status = 0;
            Motion.mAcm_AxGetMotionStatus(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref Status);
            return Status;
        }

        public static void Motion_Speed_Checking()
        {
            double rotary_speed = 1;
            DateTime time_begin = System.DateTime.Now;
            do
            {
                try
                {
                    DateTime time_now = System.DateTime.Now;
                    TimeSpan ts = time_now - time_begin;
                    if (ts.TotalMinutes > 3)
                        return;
                    System.Threading.Thread.Sleep(200);
                    rotary_speed = Got_Rotary_Speed();
                    if (Hardware.IO_LIST.Input.X208_Ejector_A_Up()||Hardware.IO_LIST.Input.X210_Ejector_B_Up()||
                        Hardware.IO_LIST.Input.X212_Ejector_C_Up()||Hardware.IO_LIST.Input.X214_Ejector_D_Up())
                    {
                            Rotary_EMG_Stop();
                            throw new System.Exception("Ejector not go down !!");
                    }
                }
                catch (Exception ee)
                {
                    StaticRes.Global.Need_Homing = true;
                    throw ee;
                }
            }
            while (rotary_speed != 0);
        }

        public static bool Rotary_MoveTo_Slot(int Slot_ID)//yakun.2016-01-08
        {
            #region
            try
            {
                Rotary_Motion_Stop();
                Motion_Speed_Checking();
                System.Threading.Thread.Sleep(500);
                int Slot_Position = StaticRes.Global.Slot_Position[Slot_ID - 1];
                double Current_Position = Got_Rotary_Position();
                Common.Reports.LogFile.Log("Rotary_MoveTo_Slot - Current_Position[1]: " + Current_Position);
                double Zero_Position = Current_Position % StaticRes.Global.System_Setting.One_Cycle_Pulse;
                Common.Reports.LogFile.Log("Rotary_MoveTo_Slot - Zero_Position: " + Zero_Position);
                double Move_Position = (Slot_Position - Zero_Position);
                Common.Reports.LogFile.Log("Rotary_MoveTo_Slot - Move_Position: " + Move_Position);
                Rotary_Move(Move_Position - 10000);
                Motion_Speed_Checking();
                System.Threading.Thread.Sleep(500);
                Rotary_Move(10000);
                Motion_Speed_Checking();
                System.Threading.Thread.Sleep(500);
                Current_Position = Got_Rotary_Position();
                Common.Reports.LogFile.Log("Rotary_MoveTo_Slot - Current_Position[2]: " + Current_Position);
                if (Math.Abs((Current_Position % StaticRes.Global.System_Setting.One_Cycle_Pulse) - Slot_Position) < 1000)
                {
                    return true;
                }
                else
                {
                    Check_and_Reset_cmd_Position();
                    throw new System.Exception("Rotary Move Failed !!");
                }
                //int Slot_Position = StaticRes.Global.Slot_Position[Slot_ID - 1];
                //Motion_Speed_Checking();
                //System.Threading.Thread.Sleep(500);
                //Rotary_MoveABS(Slot_Position - 10000);
                //Motion_Speed_Checking();
                //System.Threading.Thread.Sleep(500);
                //Rotary_MoveABS(Slot_Position);
                //Motion_Speed_Checking();
                //System.Threading.Thread.Sleep(500);
                //double Current_Position = Got_Rotary_Position();
                //if (Current_Position >= (Slot_Position - 1000) && Current_Position <= (Slot_Position + 1000))
                //{
                //    return true;
                //}
                //else
                //{
                //    throw new System.Exception("Rotary Move Failed !!");
                //}                
            }
            catch (Exception ee)
            {
                throw ee;
            }
            #endregion
        }          

        public static bool Inventory_Rotary_Move(int Slot_No, int Slot_Level)
        {
            try
            {
                System.Threading.Thread.Sleep(500);
                int Slot_Position = StaticRes.Global.Rotary_Slot_Position[Slot_Level - 1, Slot_No - 1];
                Rotary_MoveABS(Slot_Position + 310000);
                return true;
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }



        public static bool Motor_Homing()
        {
            //UInt32 Result_Rotary;
            //UInt32 Result_Vertical;
            //if (StaticRes.Global.Motor_Parameter.m_bInit)
            //{
            //    if (Vertical_LimitNagative_Sensor_On())
            //        Vertical_Move(5000);
            //    Motion_Control.Motion_Speed_Checking();
            //    Result_Rotary = Motion.mAcm_AxHome(StaticRes.Global.Motor_Parameter.m_Axishand[0], 0, 0);
            //    Result_Vertical = Motion.mAcm_AxHome(StaticRes.Global.Motor_Parameter.m_Axishand[1], 0, 0);
            //    if (Result_Rotary != (uint)ErrorCode.SUCCESS)
            //        throw new System.Exception("Rotary Motor Homing Failed !!");
            //    if (Result_Vertical != (uint)ErrorCode.SUCCESS)
            //        throw new System.Exception("Vertical Motor Homing Failed !!");
            //}
            return true;
        }

        public static bool Rotary_Motor_Homing()
        {
            UInt32 Result_Rotary;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Save_Motion_Homing_Config();
                Result_Rotary = Motion.mAcm_AxHome(StaticRes.Global.Motor_Parameter.m_Axishand[0], 11, 1);
                Save_Motion_Config();
                if (Result_Rotary != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Rotary Motor Homing Failed !!");
            }
            return true;
        }

        public static bool Arm_Y_Motor_Homing()
        {
            UInt32 Result_Vertical;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Save_Motion_Homing_Config();
                Result_Vertical = Motion.mAcm_AxHome(StaticRes.Global.Motor_Parameter.m_Axishand[1], 11, 0);
                Save_Motion_Config();
                if (Result_Vertical != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Arm Y Motor Homing Failed !!");
            }
            return true;
        }
        public static bool Arm_Y_Motor_Homing_When_Limit()
        {
            UInt32 Result_Vertical;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                if (Arm_Y_LimitPositive_Sensor_On() == true || Arm_Y_LimitNagative_Sensor_On() == true)
                    Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[1]);
                Save_Motion_Homing_Config();
                Result_Vertical = Motion.mAcm_AxHome(StaticRes.Global.Motor_Parameter.m_Axishand[1], 11, 1);
                Save_Motion_Config();
                if (Result_Vertical != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Arm Y Motor Homing Failed !!");
            }
            return true;
        }

        public static bool Arm_X_Motor_Homing()
        {
            UInt32 Result_Vertical;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Save_Motion_Homing_Config();
                Result_Vertical = Motion.mAcm_AxHome(StaticRes.Global.Motor_Parameter.m_Axishand[2], 11, 0);
                Save_Motion_Config();
                if (Result_Vertical != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Arm X Motor Homing Failed !!");
            }
            return true;
        }
        public static bool Arm_X_Motor_Homing_When_Limit()
        {
            UInt32 Result_Vertical;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                if (Arm_X_LimitNagative_Sensor_On() == true || Arm_X_LimitPositive_Sensor_On() == true)
                    Motion.mAcm_AxResetError(StaticRes.Global.Motor_Parameter.m_Axishand[2]);
                Save_Motion_Homing_Config();
                Result_Vertical = Motion.mAcm_AxHome(StaticRes.Global.Motor_Parameter.m_Axishand[2], 11, 1);
                Save_Motion_Config();
                if (Result_Vertical != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Arm X Motor Homing Failed !!");
            }
            return true;
        }

        public static bool Set_Rotary_Zero_Position()
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxSetActualPosition(StaticRes.Global.Motor_Parameter.m_Axishand[0], 1);//yakun.zhou 2015-07-06(0==>1)
                Result = Motion.mAcm_AxSetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[0], 1);//yakun.zhou 2015-07-06(0==>1)
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Set Rotary Zero Position Failed !!");
            }
            return true;
        }

        public static bool Set_Arm_Y_Zero_Position()
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxSetActualPosition(StaticRes.Global.Motor_Parameter.m_Axishand[1], 0);
                Result = Motion.mAcm_AxSetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[1], 0);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Set Arm Y Zero Position Failed !!");
            }
            return true;
        }

        public static bool Set_Arm_X_Zero_Position()
        {
            UInt32 Result;
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxSetActualPosition(StaticRes.Global.Motor_Parameter.m_Axishand[2], 0);
                Result = Motion.mAcm_AxSetCmdPosition(StaticRes.Global.Motor_Parameter.m_Axishand[2], 0);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Set Arm X Zero Position Failed !!");
            }
            return true;
        }

        public static bool Rotary_Homing_Sensor_On()
        {
            UInt32 Result;
            UInt32 IOStatus = new uint();
            //if (StaticRes.Global.Motor_Parameter.m_bInit)//yakun.zhou 2016-01.21
            //{
                Result = Motion.mAcm_AxGetMotionIO(StaticRes.Global.Motor_Parameter.m_Axishand[0], ref IOStatus);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Read rotary homing sensor status failed !!");
                if ((IOStatus & 0x10) > 0)
                    return true;
                else
                    return false;
            //}
            //return true;
        }

        public static bool Arm_Y_Homing_Sensor_On()
        {
            UInt32 Result;
            UInt32 IOStatus = new uint();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxGetMotionIO(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref IOStatus);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Read arm Y homing sensor status failed !!");
                if ((IOStatus & 0x10) > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }

        public static bool Arm_X_Homing_Sensor_On()
        {
            UInt32 Result;
            UInt32 IOStatus = new uint();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxGetMotionIO(StaticRes.Global.Motor_Parameter.m_Axishand[2], ref IOStatus);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Read arm X homing sensor status failed !!");
                if ((IOStatus & 0x10) > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }

        public static bool Arm_Y_LimitPositive_Sensor_On()
        {
            UInt32 Result;
            UInt32 IOStatus = new uint();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxGetMotionIO(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref IOStatus);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Read Arm Y limit positive sensor status failed !!");
                if ((IOStatus & 0x4) > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }

        public static bool Arm_Y_LimitNagative_Sensor_On()
        {
            UInt32 Result;
            UInt32 IOStatus = new uint();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxGetMotionIO(StaticRes.Global.Motor_Parameter.m_Axishand[1], ref IOStatus);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Read Arm Y limit nagative sensor status failed !!");
                if ((IOStatus & 0x8) > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }

        public static bool Arm_X_LimitPositive_Sensor_On()
        {
            UInt32 Result;
            UInt32 IOStatus = new uint();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxGetMotionIO(StaticRes.Global.Motor_Parameter.m_Axishand[2], ref IOStatus);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Read Arm X limit positive sensor status failed !!");
                if ((IOStatus & 0x4) > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }

        public static bool Arm_X_LimitNagative_Sensor_On()
        {
            UInt32 Result;
            UInt32 IOStatus = new uint();
            if (StaticRes.Global.Motor_Parameter.m_bInit)
            {
                Result = Motion.mAcm_AxGetMotionIO(StaticRes.Global.Motor_Parameter.m_Axishand[2], ref IOStatus);
                if (Result != (uint)ErrorCode.SUCCESS)
                    throw new System.Exception("Read Arm X limit nagative sensor status failed !!");
                if ((IOStatus & 0x8) > 0)
                    return true;
                else
                    return false;
            }
            return true;
        }

        public static bool Load_Motion_Config()
        {
            uint Result = Motion.mAcm_DevLoadConfig(StaticRes.Global.Motor_Parameter.m_DeviceHandle,
               System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "motion_setting.cfg");
            if (Result != (uint)ErrorCode.SUCCESS)
                throw new System.Exception("Load config file to motion driver failed !!");
            return true;
        }

        public static void Save_Motion_Config()
        {
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(".\\motion_setting.cfg");
                sw.WriteLine("[AXIS 0 Config]");
                sw.WriteLine("PlsPerUnit=" + StaticRes.Global.Rotary_Motor_Parameter.PlsPerUnit.ToString());
                sw.WriteLine("MaxAcc=" + StaticRes.Global.Rotary_Motor_Parameter.MaxAcc.ToString());
                sw.WriteLine("MaxVel=" + StaticRes.Global.Rotary_Motor_Parameter.MaxVel.ToString());
                sw.WriteLine("MaxDec=" + StaticRes.Global.Rotary_Motor_Parameter.MaxDec.ToString());
                sw.WriteLine("MaxJerk=" + StaticRes.Global.Rotary_Motor_Parameter.MaxJerk.ToString());
                sw.WriteLine("VelHigh=" + StaticRes.Global.System_Setting.Rotary_Home_High_Velocity.ToString());
                sw.WriteLine("VelLow=" + StaticRes.Global.System_Setting.Rotary_Home_Low_Velocity.ToString());
                sw.WriteLine("Acc=" + StaticRes.Global.System_Setting.Rotary_Home_Acc.ToString());
                sw.WriteLine("Dec=" + StaticRes.Global.System_Setting.Rotary_Home_Dcc.ToString());
                sw.WriteLine("PlsInMde=" + StaticRes.Global.Rotary_Motor_Parameter.PlsInMde.ToString());
                sw.WriteLine("PlsInLogic=" + StaticRes.Global.Rotary_Motor_Parameter.PlsInLogic.ToString());
                sw.WriteLine("PlsInSrc=" + StaticRes.Global.Rotary_Motor_Parameter.PlsInSrc.ToString());
                sw.WriteLine("PlsOutMde=" + StaticRes.Global.Rotary_Motor_Parameter.PlsOutMde.ToString());
                sw.WriteLine("AlmEnable=" + StaticRes.Global.Rotary_Motor_Parameter.AlmEnable.ToString());
                sw.WriteLine("AlmLogic=" + StaticRes.Global.Rotary_Motor_Parameter.AlmLogic.ToString());
                sw.WriteLine("AlmReact=" + StaticRes.Global.Rotary_Motor_Parameter.AlmReact.ToString());
                sw.WriteLine("InpEnable=" + StaticRes.Global.Rotary_Motor_Parameter.InpEnable.ToString());
                sw.WriteLine("InpLogic=" + StaticRes.Global.Rotary_Motor_Parameter.InpLogic.ToString());
                sw.WriteLine("ErcLogic=" + StaticRes.Global.Rotary_Motor_Parameter.ErcLogic.ToString());
                sw.WriteLine("ErcOnTime=" + StaticRes.Global.Rotary_Motor_Parameter.ErcOnTime.ToString());
                sw.WriteLine("ErcOffTime=" + StaticRes.Global.Rotary_Motor_Parameter.ErcOffTime.ToString());
                sw.WriteLine("ErcEnMde=" + StaticRes.Global.Rotary_Motor_Parameter.ErcEnMde.ToString());
                sw.WriteLine("SdEnable=" + StaticRes.Global.Rotary_Motor_Parameter.SdEnable.ToString());
                sw.WriteLine("SdLogic=" + StaticRes.Global.Rotary_Motor_Parameter.SdLogic.ToString());
                sw.WriteLine("SdReact=" + StaticRes.Global.Rotary_Motor_Parameter.SdReact.ToString());
                sw.WriteLine("SdLatch=" + StaticRes.Global.Rotary_Motor_Parameter.SdLatch.ToString());
                sw.WriteLine("ElEnable=" + StaticRes.Global.Rotary_Motor_Parameter.ElEnable.ToString());
                sw.WriteLine("ElLogic=" + StaticRes.Global.Rotary_Motor_Parameter.ElLogic.ToString());
                sw.WriteLine("ElReact=" + StaticRes.Global.Rotary_Motor_Parameter.ElReact.ToString());
                sw.WriteLine("SwMelEnable=" + StaticRes.Global.Rotary_Motor_Parameter.SwMelEnable.ToString());
                sw.WriteLine("SwPelEnable=" + StaticRes.Global.Rotary_Motor_Parameter.SwPelEnable.ToString());
                sw.WriteLine("SwMelReact=" + StaticRes.Global.Rotary_Motor_Parameter.SwMelReact.ToString());
                sw.WriteLine("SwPelReact=" + StaticRes.Global.Rotary_Motor_Parameter.SwPelReact.ToString());
                sw.WriteLine("SwMelValue=" + StaticRes.Global.Rotary_Motor_Parameter.SwMelValue.ToString());
                sw.WriteLine("SwPelValue=" + StaticRes.Global.Rotary_Motor_Parameter.SwPelValue.ToString());
                sw.WriteLine("OrgLogic=" + StaticRes.Global.Rotary_Motor_Parameter.OrgLogic.ToString());
                sw.WriteLine("EzLogic=" + StaticRes.Global.Rotary_Motor_Parameter.EzLogic.ToString());
                sw.WriteLine("PosLagEn=" + StaticRes.Global.Rotary_Motor_Parameter.PosLagEn.ToString());
                sw.WriteLine("MaxPosLag=" + StaticRes.Global.Rotary_Motor_Parameter.MaxPosLag.ToString());
                sw.WriteLine("[AXIS 1 Config]");
                sw.WriteLine("PlsPerUnit=" + StaticRes.Global.Spare_Motor_Parameter.PlsPerUnit.ToString());
                sw.WriteLine("MaxAcc=" + StaticRes.Global.Spare_Motor_Parameter.MaxAcc.ToString());
                sw.WriteLine("MaxVel=" + StaticRes.Global.Spare_Motor_Parameter.MaxVel.ToString());
                sw.WriteLine("MaxDec=" + StaticRes.Global.Spare_Motor_Parameter.MaxDec.ToString());
                sw.WriteLine("MaxJerk=" + StaticRes.Global.Spare_Motor_Parameter.MaxJerk.ToString());
                sw.WriteLine("VelHigh=" + StaticRes.Global.Spare_Motor_Parameter.VelHigh.ToString());
                sw.WriteLine("VelLow=" + StaticRes.Global.Spare_Motor_Parameter.VelLow.ToString());
                sw.WriteLine("Acc=" + StaticRes.Global.Spare_Motor_Parameter.Acc.ToString());
                sw.WriteLine("Dec=" + StaticRes.Global.Spare_Motor_Parameter.Dec.ToString());
                sw.WriteLine("PlsInMde=" + StaticRes.Global.Spare_Motor_Parameter.PlsInMde.ToString());
                sw.WriteLine("PlsInLogic=" + StaticRes.Global.Spare_Motor_Parameter.PlsInLogic.ToString());
                sw.WriteLine("PlsInSrc=" + StaticRes.Global.Spare_Motor_Parameter.PlsInSrc.ToString());
                sw.WriteLine("PlsOutMde=" + StaticRes.Global.Spare_Motor_Parameter.PlsOutMde.ToString());
                sw.WriteLine("AlmEnable=" + StaticRes.Global.Spare_Motor_Parameter.AlmEnable.ToString());
                sw.WriteLine("AlmLogic=" + StaticRes.Global.Spare_Motor_Parameter.AlmLogic.ToString());
                sw.WriteLine("AlmReact=" + StaticRes.Global.Spare_Motor_Parameter.AlmReact.ToString());
                sw.WriteLine("InpEnable=" + StaticRes.Global.Spare_Motor_Parameter.InpEnable.ToString());
                sw.WriteLine("InpLogic=" + StaticRes.Global.Spare_Motor_Parameter.InpLogic.ToString());
                sw.WriteLine("ErcLogic=" + StaticRes.Global.Spare_Motor_Parameter.ErcLogic.ToString());
                sw.WriteLine("ErcOnTime=" + StaticRes.Global.Spare_Motor_Parameter.ErcOnTime.ToString());
                sw.WriteLine("ErcOffTime=" + StaticRes.Global.Spare_Motor_Parameter.ErcOffTime.ToString());
                sw.WriteLine("ErcEnMde=" + StaticRes.Global.Spare_Motor_Parameter.ErcEnMde.ToString());
                sw.WriteLine("SdEnable=" + StaticRes.Global.Spare_Motor_Parameter.SdEnable.ToString());
                sw.WriteLine("SdLogic=" + StaticRes.Global.Spare_Motor_Parameter.SdLogic.ToString());
                sw.WriteLine("SdReact=" + StaticRes.Global.Spare_Motor_Parameter.SdReact.ToString());
                sw.WriteLine("SdLatch=" + StaticRes.Global.Spare_Motor_Parameter.SdLatch.ToString());
                sw.WriteLine("ElEnable=" + StaticRes.Global.Spare_Motor_Parameter.ElEnable.ToString());
                sw.WriteLine("ElLogic=" + StaticRes.Global.Spare_Motor_Parameter.ElLogic.ToString());
                sw.WriteLine("ElReact=" + StaticRes.Global.Spare_Motor_Parameter.ElReact.ToString());
                sw.WriteLine("SwMelEnable=" + StaticRes.Global.Spare_Motor_Parameter.SwMelEnable.ToString());
                sw.WriteLine("SwPelEnable=" + StaticRes.Global.Spare_Motor_Parameter.SwPelEnable.ToString());
                sw.WriteLine("SwMelReact=" + StaticRes.Global.Spare_Motor_Parameter.SwMelReact.ToString());
                sw.WriteLine("SwPelReact=" + StaticRes.Global.Spare_Motor_Parameter.SwPelReact.ToString());
                sw.WriteLine("SwMelValue=" + StaticRes.Global.Spare_Motor_Parameter.SwMelValue.ToString());
                sw.WriteLine("SwPelValue=" + StaticRes.Global.Spare_Motor_Parameter.SwPelValue.ToString());
                sw.WriteLine("OrgLogic=" + StaticRes.Global.Spare_Motor_Parameter.OrgLogic.ToString());
                sw.WriteLine("EzLogic=" + StaticRes.Global.Spare_Motor_Parameter.EzLogic.ToString());
                sw.WriteLine("PosLagEn=" + StaticRes.Global.Spare_Motor_Parameter.PosLagEn.ToString());
                sw.WriteLine("MaxPosLag=" + StaticRes.Global.Spare_Motor_Parameter.MaxPosLag.ToString());
                sw.Close();
                Load_Motion_Config();
            }
            catch (Exception ee)
            {
                throw new System.Exception(ee.Message);
            }
        }

        public static void Save_Motion_Homing_Config()
        {
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(".\\motion_setting.cfg");
                sw.WriteLine("[AXIS 0 Config]");
                sw.WriteLine("PlsPerUnit=" + StaticRes.Global.Rotary_Motor_Parameter.PlsPerUnit.ToString());
                sw.WriteLine("MaxAcc=" + StaticRes.Global.Rotary_Motor_Parameter.MaxAcc.ToString());
                sw.WriteLine("MaxVel=" + StaticRes.Global.Rotary_Motor_Parameter.MaxVel.ToString());
                sw.WriteLine("MaxDec=" + StaticRes.Global.Rotary_Motor_Parameter.MaxDec.ToString());
                sw.WriteLine("MaxJerk=" + StaticRes.Global.Rotary_Motor_Parameter.MaxJerk.ToString());
                sw.WriteLine("VelHigh=" + StaticRes.Global.Rotary_Motor_Parameter.VelHigh.ToString());
                sw.WriteLine("VelLow=" + StaticRes.Global.Rotary_Motor_Parameter.VelLow.ToString());
                sw.WriteLine("Acc=" + StaticRes.Global.Rotary_Motor_Parameter.Acc.ToString());
                sw.WriteLine("Dec=" + StaticRes.Global.Rotary_Motor_Parameter.Dec.ToString());
                sw.WriteLine("PlsInMde=" + StaticRes.Global.Rotary_Motor_Parameter.PlsInMde.ToString());
                sw.WriteLine("PlsInLogic=" + StaticRes.Global.Rotary_Motor_Parameter.PlsInLogic.ToString());
                sw.WriteLine("PlsInSrc=" + StaticRes.Global.Rotary_Motor_Parameter.PlsInSrc.ToString());
                sw.WriteLine("PlsOutMde=" + StaticRes.Global.Rotary_Motor_Parameter.PlsOutMde.ToString());
                sw.WriteLine("AlmEnable=" + StaticRes.Global.Rotary_Motor_Parameter.AlmEnable.ToString());
                sw.WriteLine("AlmLogic=" + StaticRes.Global.Rotary_Motor_Parameter.AlmLogic.ToString());
                sw.WriteLine("AlmReact=" + StaticRes.Global.Rotary_Motor_Parameter.AlmReact.ToString());
                sw.WriteLine("InpEnable=" + StaticRes.Global.Rotary_Motor_Parameter.InpEnable.ToString());
                sw.WriteLine("InpLogic=" + StaticRes.Global.Rotary_Motor_Parameter.InpLogic.ToString());
                sw.WriteLine("ErcLogic=" + StaticRes.Global.Rotary_Motor_Parameter.ErcLogic.ToString());
                sw.WriteLine("ErcOnTime=" + StaticRes.Global.Rotary_Motor_Parameter.ErcOnTime.ToString());
                sw.WriteLine("ErcOffTime=" + StaticRes.Global.Rotary_Motor_Parameter.ErcOffTime.ToString());
                sw.WriteLine("ErcEnMde=" + StaticRes.Global.Rotary_Motor_Parameter.ErcEnMde.ToString());
                sw.WriteLine("SdEnable=" + StaticRes.Global.Rotary_Motor_Parameter.SdEnable.ToString());
                sw.WriteLine("SdLogic=" + StaticRes.Global.Rotary_Motor_Parameter.SdLogic.ToString());
                sw.WriteLine("SdReact=" + StaticRes.Global.Rotary_Motor_Parameter.SdReact.ToString());
                sw.WriteLine("SdLatch=" + StaticRes.Global.Rotary_Motor_Parameter.SdLatch.ToString());
                sw.WriteLine("ElEnable=" + StaticRes.Global.Rotary_Motor_Parameter.ElEnable.ToString());
                sw.WriteLine("ElLogic=" + StaticRes.Global.Rotary_Motor_Parameter.ElLogic.ToString());
                sw.WriteLine("ElReact=" + StaticRes.Global.Rotary_Motor_Parameter.ElReact.ToString());
                sw.WriteLine("SwMelEnable=" + StaticRes.Global.Rotary_Motor_Parameter.SwMelEnable.ToString());
                sw.WriteLine("SwPelEnable=" + StaticRes.Global.Rotary_Motor_Parameter.SwPelEnable.ToString());
                sw.WriteLine("SwMelReact=" + StaticRes.Global.Rotary_Motor_Parameter.SwMelReact.ToString());
                sw.WriteLine("SwPelReact=" + StaticRes.Global.Rotary_Motor_Parameter.SwPelReact.ToString());
                sw.WriteLine("SwMelValue=" + StaticRes.Global.Rotary_Motor_Parameter.SwMelValue.ToString());
                sw.WriteLine("SwPelValue=" + StaticRes.Global.Rotary_Motor_Parameter.SwPelValue.ToString());
                sw.WriteLine("OrgLogic=" + StaticRes.Global.Rotary_Motor_Parameter.OrgLogic.ToString());
                sw.WriteLine("EzLogic=" + StaticRes.Global.Rotary_Motor_Parameter.EzLogic.ToString());
                sw.WriteLine("PosLagEn=" + StaticRes.Global.Rotary_Motor_Parameter.PosLagEn.ToString());
                sw.WriteLine("MaxPosLag=" + StaticRes.Global.Rotary_Motor_Parameter.MaxPosLag.ToString());
                sw.WriteLine("[AXIS 1 Config]");
                sw.WriteLine("PlsPerUnit=" + StaticRes.Global.Spare_Motor_Parameter.PlsPerUnit.ToString());
                sw.WriteLine("MaxAcc=" + StaticRes.Global.Spare_Motor_Parameter.MaxAcc.ToString());
                sw.WriteLine("MaxVel=" + StaticRes.Global.Spare_Motor_Parameter.MaxVel.ToString());
                sw.WriteLine("MaxDec=" + StaticRes.Global.Spare_Motor_Parameter.MaxDec.ToString());
                sw.WriteLine("MaxJerk=" + StaticRes.Global.Spare_Motor_Parameter.MaxJerk.ToString());
                sw.WriteLine("VelHigh=" + StaticRes.Global.Spare_Motor_Parameter.VelHigh.ToString());
                sw.WriteLine("VelLow=" + StaticRes.Global.Spare_Motor_Parameter.VelLow.ToString());
                sw.WriteLine("Acc=" + StaticRes.Global.Spare_Motor_Parameter.Acc.ToString());
                sw.WriteLine("Dec=" + StaticRes.Global.Spare_Motor_Parameter.Dec.ToString());
                sw.WriteLine("PlsInMde=" + StaticRes.Global.Spare_Motor_Parameter.PlsInMde.ToString());
                sw.WriteLine("PlsInLogic=" + StaticRes.Global.Spare_Motor_Parameter.PlsInLogic.ToString());
                sw.WriteLine("PlsInSrc=" + StaticRes.Global.Spare_Motor_Parameter.PlsInSrc.ToString());
                sw.WriteLine("PlsOutMde=" + StaticRes.Global.Spare_Motor_Parameter.PlsOutMde.ToString());
                sw.WriteLine("AlmEnable=" + StaticRes.Global.Spare_Motor_Parameter.AlmEnable.ToString());
                sw.WriteLine("AlmLogic=" + StaticRes.Global.Spare_Motor_Parameter.AlmLogic.ToString());
                sw.WriteLine("AlmReact=" + StaticRes.Global.Spare_Motor_Parameter.AlmReact.ToString());
                sw.WriteLine("InpEnable=" + StaticRes.Global.Spare_Motor_Parameter.InpEnable.ToString());
                sw.WriteLine("InpLogic=" + StaticRes.Global.Spare_Motor_Parameter.InpLogic.ToString());
                sw.WriteLine("ErcLogic=" + StaticRes.Global.Spare_Motor_Parameter.ErcLogic.ToString());
                sw.WriteLine("ErcOnTime=" + StaticRes.Global.Spare_Motor_Parameter.ErcOnTime.ToString());
                sw.WriteLine("ErcOffTime=" + StaticRes.Global.Spare_Motor_Parameter.ErcOffTime.ToString());
                sw.WriteLine("ErcEnMde=" + StaticRes.Global.Spare_Motor_Parameter.ErcEnMde.ToString());
                sw.WriteLine("SdEnable=" + StaticRes.Global.Spare_Motor_Parameter.SdEnable.ToString());
                sw.WriteLine("SdLogic=" + StaticRes.Global.Spare_Motor_Parameter.SdLogic.ToString());
                sw.WriteLine("SdReact=" + StaticRes.Global.Spare_Motor_Parameter.SdReact.ToString());
                sw.WriteLine("SdLatch=" + StaticRes.Global.Spare_Motor_Parameter.SdLatch.ToString());
                sw.WriteLine("ElEnable=" + StaticRes.Global.Spare_Motor_Parameter.ElEnable.ToString());
                sw.WriteLine("ElLogic=" + StaticRes.Global.Spare_Motor_Parameter.ElLogic.ToString());
                sw.WriteLine("ElReact=" + StaticRes.Global.Spare_Motor_Parameter.ElReact.ToString());
                sw.WriteLine("SwMelEnable=" + StaticRes.Global.Spare_Motor_Parameter.SwMelEnable.ToString());
                sw.WriteLine("SwPelEnable=" + StaticRes.Global.Spare_Motor_Parameter.SwPelEnable.ToString());
                sw.WriteLine("SwMelReact=" + StaticRes.Global.Spare_Motor_Parameter.SwMelReact.ToString());
                sw.WriteLine("SwPelReact=" + StaticRes.Global.Spare_Motor_Parameter.SwPelReact.ToString());
                sw.WriteLine("SwMelValue=" + StaticRes.Global.Spare_Motor_Parameter.SwMelValue.ToString());
                sw.WriteLine("SwPelValue=" + StaticRes.Global.Spare_Motor_Parameter.SwPelValue.ToString());
                sw.WriteLine("OrgLogic=" + StaticRes.Global.Spare_Motor_Parameter.OrgLogic.ToString());
                sw.WriteLine("EzLogic=" + StaticRes.Global.Spare_Motor_Parameter.EzLogic.ToString());
                sw.WriteLine("PosLagEn=" + StaticRes.Global.Spare_Motor_Parameter.PosLagEn.ToString());
                sw.WriteLine("MaxPosLag=" + StaticRes.Global.Spare_Motor_Parameter.MaxPosLag.ToString());
                sw.Close();
                Load_Motion_Config();
            }
            catch (Exception ee)
            {
                throw new System.Exception(ee.Message);
            }
        }

        public static void Read_Motion_Config()
        {
            System.IO.StreamReader sw = new System.IO.StreamReader(".\\motion_setting.cfg");
            string R_PlsPerUnit = sw.ReadLine();

            StaticRes.Global.Rotary_Motor_Parameter.PlsPerUnit = int.Parse(R_PlsPerUnit.Substring(R_PlsPerUnit.IndexOf('=') + 1, R_PlsPerUnit.Length - R_PlsPerUnit.IndexOf('=') - 1));
            string R_MaxAcd = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.MaxAcc = int.Parse(R_MaxAcd.Substring(R_MaxAcd.IndexOf('=') + 1, R_MaxAcd.Length - R_MaxAcd.IndexOf('=') - 1));
            string R_MaxVel = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.MaxVel = int.Parse(R_MaxVel.Substring(R_MaxVel.IndexOf('=') + 1, R_MaxVel.Length - R_MaxVel.IndexOf('=') - 1));
            string R_MaxDec = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.MaxDec = int.Parse(R_MaxDec.Substring(R_MaxDec.IndexOf('=') + 1, R_MaxDec.Length - R_MaxDec.IndexOf('=') - 1));
            string R_MaxJerk = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.MaxJerk = int.Parse(R_MaxJerk.Substring(R_MaxJerk.IndexOf('=') + 1, R_MaxJerk.Length - R_MaxJerk.IndexOf('=') - 1));
            string R_VelHigh = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.VelHigh = int.Parse(R_VelHigh.Substring(R_VelHigh.IndexOf('=') + 1, R_VelHigh.Length - R_VelHigh.IndexOf('=') - 1));
            string R_VelLow = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.VelLow = int.Parse(R_VelLow.Substring(R_VelLow.IndexOf('=') + 1, R_VelLow.Length - R_VelLow.IndexOf('=') - 1));
            string R_Acc = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.Acc = int.Parse(R_Acc.Substring(R_Acc.IndexOf('=') + 1, R_Acc.Length - R_Acc.IndexOf('=') - 1));
            string R_Dec = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.Dec = int.Parse(R_Dec.Substring(R_Dec.IndexOf('=') + 1, R_Dec.Length - R_Dec.IndexOf('=') - 1));
            string R_PlsInMde = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.PlsInMde = int.Parse(R_PlsInMde.Substring(R_PlsInMde.IndexOf('=') + 1, R_PlsInMde.Length - R_PlsInMde.IndexOf('=') - 1));
            string R_PlsInLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.PlsInLogic = int.Parse(R_PlsInLogic.Substring(R_PlsInLogic.IndexOf('=') + 1, R_PlsInLogic.Length - R_PlsInLogic.IndexOf('=') - 1));
            string R_PlsInSrc = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.PlsInSrc = int.Parse(R_PlsInSrc.Substring(R_PlsInSrc.IndexOf('=') + 1, R_PlsInSrc.Length - R_PlsInSrc.IndexOf('=') - 1));
            string R_PlsOutMde = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.PlsOutMde = int.Parse(R_PlsOutMde.Substring(R_PlsOutMde.IndexOf('=') + 1, R_PlsOutMde.Length - R_PlsOutMde.IndexOf('=') - 1));
            string R_AlmEnable = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.AlmEnable = int.Parse(R_AlmEnable.Substring(R_AlmEnable.IndexOf('=') + 1, R_AlmEnable.Length - R_AlmEnable.IndexOf('=') - 1));
            string R_AlmLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.AlmLogic = int.Parse(R_AlmLogic.Substring(R_AlmLogic.IndexOf('=') + 1, R_AlmLogic.Length - R_AlmLogic.IndexOf('=') - 1));
            string R_AlmReact = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.AlmReact = int.Parse(R_AlmReact.Substring(R_AlmReact.IndexOf('=') + 1, R_AlmReact.Length - R_AlmReact.IndexOf('=') - 1));
            string R_InpEnable = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.InpEnable = int.Parse(R_InpEnable.Substring(R_InpEnable.IndexOf('=') + 1, R_InpEnable.Length - R_InpEnable.IndexOf('=') - 1));
            string R_InpLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.InpLogic = int.Parse(R_InpLogic.Substring(R_InpLogic.IndexOf('=') + 1, R_InpLogic.Length - R_InpLogic.IndexOf('=') - 1));
            string R_ErcLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.ErcLogic = int.Parse(R_ErcLogic.Substring(R_ErcLogic.IndexOf('=') + 1, R_ErcLogic.Length - R_ErcLogic.IndexOf('=') - 1));
            string R_ErcOnTime = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.ErcOnTime = int.Parse(R_ErcOnTime.Substring(R_ErcOnTime.IndexOf('=') + 1, R_ErcOnTime.Length - R_ErcOnTime.IndexOf('=') - 1));
            string R_ErcOffTime = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.ErcOffTime = int.Parse(R_ErcOffTime.Substring(R_ErcOffTime.IndexOf('=') + 1, R_ErcOffTime.Length - R_ErcOffTime.IndexOf('=') - 1));
            string R_ErcEnMde = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.ErcEnMde = int.Parse(R_ErcEnMde.Substring(R_ErcEnMde.IndexOf('=') + 1, R_ErcEnMde.Length - R_ErcEnMde.IndexOf('=') - 1));
            string R_SdEnable = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SdEnable = int.Parse(R_SdEnable.Substring(R_SdEnable.IndexOf('=') + 1, R_SdEnable.Length - R_SdEnable.IndexOf('=') - 1));
            string R_SdLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SdLogic = int.Parse(R_SdLogic.Substring(R_SdLogic.IndexOf('=') + 1, R_SdLogic.Length - R_SdLogic.IndexOf('=') - 1));
            string R_SdReact = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SdReact = int.Parse(R_SdReact.Substring(R_SdReact.IndexOf('=') + 1, R_SdReact.Length - R_SdReact.IndexOf('=') - 1));
            string R_SdLatch = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SdLatch = int.Parse(R_SdLatch.Substring(R_SdLatch.IndexOf('=') + 1, R_SdLatch.Length - R_SdLatch.IndexOf('=') - 1));
            string R_ElEnable = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.ElEnable = int.Parse(R_ElEnable.Substring(R_ElEnable.IndexOf('=') + 1, R_ElEnable.Length - R_ElEnable.IndexOf('=') - 1));
            string R_ElLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.ElLogic = int.Parse(R_ElLogic.Substring(R_ElLogic.IndexOf('=') + 1, R_ElLogic.Length - R_ElLogic.IndexOf('=') - 1));
            string R_ElReact = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.ElReact = int.Parse(R_ElReact.Substring(R_ElReact.IndexOf('=') + 1, R_ElReact.Length - R_ElReact.IndexOf('=') - 1));
            string R_SwMelEnable = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SwMelEnable = int.Parse(R_SwMelEnable.Substring(R_SwMelEnable.IndexOf('=') + 1, R_SwMelEnable.Length - R_SwMelEnable.IndexOf('=') - 1));
            string R_SwPelEnable = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SwPelEnable = int.Parse(R_SwPelEnable.Substring(R_SwPelEnable.IndexOf('=') + 1, R_SwPelEnable.Length - R_SwPelEnable.IndexOf('=') - 1));
            string R_SwMelReact = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SwMelReact = int.Parse(R_SwMelReact.Substring(R_SwMelReact.IndexOf('=') + 1, R_SwMelReact.Length - R_SwMelReact.IndexOf('=') - 1));
            string R_SwPelReact = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SwPelReact = int.Parse(R_SwPelReact.Substring(R_SwPelReact.IndexOf('=') + 1, R_SwPelReact.Length - R_SwPelReact.IndexOf('=') - 1));
            string R_SwMelValue = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SwMelValue = int.Parse(R_SwMelValue.Substring(R_SwMelValue.IndexOf('=') + 1, R_SwMelValue.Length - R_SwMelValue.IndexOf('=') - 1));
            string R_SwPelValue = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.SwPelValue = int.Parse(R_SwPelValue.Substring(R_SwPelValue.IndexOf('=') + 1, R_SwPelValue.Length - R_SwPelValue.IndexOf('=') - 1));
            string R_OrgLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.OrgLogic = int.Parse(R_OrgLogic.Substring(R_OrgLogic.IndexOf('=') + 1, R_OrgLogic.Length - R_OrgLogic.IndexOf('=') - 1));
            string R_EzLogic = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.EzLogic = int.Parse(R_EzLogic.Substring(R_EzLogic.IndexOf('=') + 1, R_EzLogic.Length - R_EzLogic.IndexOf('=') - 1));
            string R_PosLagEn = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.PosLagEn = int.Parse(R_PosLagEn.Substring(R_PosLagEn.IndexOf('=') + 1, R_PosLagEn.Length - R_PosLagEn.IndexOf('=') - 1));
            string R_MaxPosLag = sw.ReadLine();
            StaticRes.Global.Rotary_Motor_Parameter.MaxPosLag = int.Parse(R_MaxPosLag.Substring(R_MaxPosLag.IndexOf('=') + 1, R_MaxPosLag.Length - R_MaxPosLag.IndexOf('=') - 1));
            string y = sw.ReadLine();
            string V_PlsPerUnit = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.PlsPerUnit = int.Parse(V_PlsPerUnit.Substring(V_PlsPerUnit.IndexOf('=') + 1, V_PlsPerUnit.Length - V_PlsPerUnit.IndexOf('=') - 1));
            string V_MaxAcc = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.MaxAcc = int.Parse(V_MaxAcc.Substring(V_MaxAcc.IndexOf('=') + 1, V_MaxAcc.Length - V_MaxAcc.IndexOf('=') - 1));
            string V_MaxVel = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.MaxVel = int.Parse(V_MaxVel.Substring(V_MaxVel.IndexOf('=') + 1, V_MaxVel.Length - V_MaxVel.IndexOf('=') - 1));
            string V_MaxDec = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.MaxDec = int.Parse(V_MaxDec.Substring(V_MaxDec.IndexOf('=') + 1, V_MaxDec.Length - V_MaxDec.IndexOf('=') - 1));
            string V_MaxJerk = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.MaxJerk = int.Parse(V_MaxJerk.Substring(V_MaxJerk.IndexOf('=') + 1, V_MaxJerk.Length - V_MaxJerk.IndexOf('=') - 1));
            string V_VelHigh = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.VelHigh = int.Parse(V_VelHigh.Substring(V_VelHigh.IndexOf('=') + 1, V_VelHigh.Length - V_VelHigh.IndexOf('=') - 1));
            string V_VelLow = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.VelLow = int.Parse(V_VelLow.Substring(V_VelLow.IndexOf('=') + 1, V_VelLow.Length - V_VelLow.IndexOf('=') - 1));
            string V_Acc = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.Acc = int.Parse(V_Acc.Substring(V_Acc.IndexOf('=') + 1, V_Acc.Length - V_Acc.IndexOf('=') - 1));
            string V_Dec = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.Dec = int.Parse(V_Dec.Substring(V_Dec.IndexOf('=') + 1, V_Dec.Length - V_Dec.IndexOf('=') - 1));
            string V_PlsInMde = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.PlsInMde = int.Parse(V_PlsInMde.Substring(V_PlsInMde.IndexOf('=') + 1, V_PlsInMde.Length - V_PlsInMde.IndexOf('=') - 1));
            string V_PlsInLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.PlsInLogic = int.Parse(V_PlsInLogic.Substring(V_PlsInLogic.IndexOf('=') + 1, V_PlsInLogic.Length - V_PlsInLogic.IndexOf('=') - 1));
            string V_PlsInSrc = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.PlsInSrc = int.Parse(V_PlsInSrc.Substring(V_PlsInSrc.IndexOf('=') + 1, V_PlsInSrc.Length - V_PlsInSrc.IndexOf('=') - 1));
            string V_PlsOutMde = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.PlsOutMde = int.Parse(V_PlsOutMde.Substring(V_PlsOutMde.IndexOf('=') + 1, V_PlsOutMde.Length - V_PlsOutMde.IndexOf('=') - 1));
            string V_AlmEnable = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.AlmEnable = int.Parse(V_AlmEnable.Substring(V_AlmEnable.IndexOf('=') + 1, V_AlmEnable.Length - V_AlmEnable.IndexOf('=') - 1));
            string V_AlmLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.AlmLogic = int.Parse(V_AlmLogic.Substring(V_AlmLogic.IndexOf('=') + 1, V_AlmLogic.Length - V_AlmLogic.IndexOf('=') - 1));
            string V_AlmReact = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.AlmReact = int.Parse(V_AlmReact.Substring(V_AlmReact.IndexOf('=') + 1, V_AlmReact.Length - V_AlmReact.IndexOf('=') - 1));
            string V_InpEnable = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.InpEnable = int.Parse(V_InpEnable.Substring(V_InpEnable.IndexOf('=') + 1, V_InpEnable.Length - V_InpEnable.IndexOf('=') - 1));
            string V_InpLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.InpLogic = int.Parse(V_InpLogic.Substring(V_InpLogic.IndexOf('=') + 1, V_InpLogic.Length - V_InpLogic.IndexOf('=') - 1));
            string V_ErcLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.ErcLogic = int.Parse(V_ErcLogic.Substring(V_ErcLogic.IndexOf('=') + 1, V_ErcLogic.Length - V_ErcLogic.IndexOf('=') - 1));
            string V_ErcOnTime = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.ErcOnTime = int.Parse(V_ErcOnTime.Substring(V_ErcOnTime.IndexOf('=') + 1, V_ErcOnTime.Length - V_ErcOnTime.IndexOf('=') - 1));
            string V_ErcOffTime = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.ErcOffTime = int.Parse(V_ErcOffTime.Substring(V_ErcOffTime.IndexOf('=') + 1, V_ErcOffTime.Length - V_ErcOffTime.IndexOf('=') - 1));
            string V_ErcEnMde = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.ErcEnMde = int.Parse(V_ErcEnMde.Substring(V_ErcEnMde.IndexOf('=') + 1, V_ErcEnMde.Length - V_ErcEnMde.IndexOf('=') - 1));
            string V_SdEnable = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SdEnable = int.Parse(V_SdEnable.Substring(V_SdEnable.IndexOf('=') + 1, V_SdEnable.Length - V_SdEnable.IndexOf('=') - 1));
            string V_SdLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SdLogic = int.Parse(V_SdLogic.Substring(V_SdLogic.IndexOf('=') + 1, V_SdLogic.Length - V_SdLogic.IndexOf('=') - 1));
            string V_SdReact = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SdReact = int.Parse(V_SdReact.Substring(V_SdReact.IndexOf('=') + 1, V_SdReact.Length - V_SdReact.IndexOf('=') - 1));
            string V_SdLatch = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SdLatch = int.Parse(V_SdLatch.Substring(V_SdLatch.IndexOf('=') + 1, V_SdLatch.Length - V_SdLatch.IndexOf('=') - 1));
            string V_ElEnable = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.ElEnable = int.Parse(V_ElEnable.Substring(V_ElEnable.IndexOf('=') + 1, V_ElEnable.Length - V_ElEnable.IndexOf('=') - 1));
            string V_ElLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.ElLogic = int.Parse(V_ElLogic.Substring(V_ElLogic.IndexOf('=') + 1, V_ElLogic.Length - V_ElLogic.IndexOf('=') - 1));
            string V_ElReact = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.ElReact = int.Parse(V_ElReact.Substring(V_ElReact.IndexOf('=') + 1, V_ElReact.Length - V_ElReact.IndexOf('=') - 1));
            string V_SwMelEnable = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SwMelEnable = int.Parse(V_SwMelEnable.Substring(V_SwMelEnable.IndexOf('=') + 1, V_SwMelEnable.Length - V_SwMelEnable.IndexOf('=') - 1));
            string V_SwPelEnable = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SwPelEnable = int.Parse(V_SwPelEnable.Substring(V_SwPelEnable.IndexOf('=') + 1, V_SwPelEnable.Length - V_SwPelEnable.IndexOf('=') - 1));
            string V_SwMelReact = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SwMelReact = int.Parse(V_SwMelReact.Substring(V_SwMelReact.IndexOf('=') + 1, V_SwMelReact.Length - V_SwMelReact.IndexOf('=') - 1));
            string V_SwPelReact = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SwPelReact = int.Parse(V_SwPelReact.Substring(V_SwPelReact.IndexOf('=') + 1, V_SwPelReact.Length - V_SwPelReact.IndexOf('=') - 1));
            string V_SwMelValue = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SwMelValue = int.Parse(V_SwMelValue.Substring(V_SwMelValue.IndexOf('=') + 1, V_SwMelValue.Length - V_SwMelValue.IndexOf('=') - 1));
            string V_SwPelValue = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.SwPelValue = int.Parse(V_SwPelValue.Substring(V_SwPelValue.IndexOf('=') + 1, V_SwPelValue.Length - V_SwPelValue.IndexOf('=') - 1));
            string V_OrgLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.OrgLogic = int.Parse(V_OrgLogic.Substring(V_OrgLogic.IndexOf('=') + 1, V_OrgLogic.Length - V_OrgLogic.IndexOf('=') - 1));
            string V_EzLogic = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.EzLogic = int.Parse(V_EzLogic.Substring(V_EzLogic.IndexOf('=') + 1, V_EzLogic.Length - V_EzLogic.IndexOf('=') - 1));
            string V_PosLagEn = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.PosLagEn = int.Parse(V_PosLagEn.Substring(V_PosLagEn.IndexOf('=') + 1, V_PosLagEn.Length - V_PosLagEn.IndexOf('=') - 1));
            string V_MaxPosLag = sw.ReadLine();
            StaticRes.Global.Spare_Motor_Parameter.MaxPosLag = int.Parse(V_MaxPosLag.Substring(V_MaxPosLag.IndexOf('=') + 1, V_MaxPosLag.Length - V_MaxPosLag.IndexOf('=') - 1));
            sw.Close();
        }
    }
}
