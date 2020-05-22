using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Advantech.Motion; // Advantech Common Motion API.
using System.Runtime.InteropServices; //For Marshal

namespace GEMS.Hardware.Motion_DLL
{
    #region Enumerations.
    public enum Axis
    {
        X = 0, 
        Y = 1, 
        Z = 2, 
        W = 3, 
        U = 4 
    }

    public enum HandDirection
    {
        Left,
        Right
    }

    public enum DirMode
    {
        Positive_Direction = 0,
        Negative_Direction = 1
    }

    public enum SwitchMode
    {
        Level_On = 0,
        Level_Off,
        Edge_On,
        Edge_Off
    }

    public enum VelocityProfile
    {
        T_Curve = 0,
        S_Curve = 1
    }
    public enum HomeMode
    {
        Mode1_Abs = 0,
        Mode2_Lmt,
        Mode3_Ref,
        Mode4_Abs_Ref,
        Mode5_Abs_NegRef,
        Mode6_Lmt_Ref,
        Mode7_AbsSearch,
        Mode8_LmtSearch,
        Mode9_AbsSearch_Ref,
        Mode10_AbsSearch_NegRef,
        Mode11_LmtSearch_Ref,
        Mode12_AbsSearchReFind,
        Mode13_LmtSearchReFind,
        Mode14_AbsSearchReFind_Ref,
        Mode15_AbsSearchReFind_NegRef,
        Mode16_LmtSearchReFind_Ref
    }

    public enum SpeedMode
    {
        Home,
        Jog,
        Move 
    }

    public enum MotionDirection
    {
        Positive = 0,
        Negative = 1
    }

    #endregion

    public class Advantech_Axis
    {
        #region Settings.

        #region Configurations.

        public int AxisNumber;

        public int DirMode;
        public int SwitchMode;
        
        public int HomeMode;
        public Double HomeCrossDistance;

        public int EzLogic;
        public int OrgLogic;
        public int ElLogic;

        #endregion

        #region Speeds.

        public Double HomeLowVelocity;
        public Double HomeHighVelocity;
        public Double HomeAcc;
        public Double HomeDec;
        public Double HomeJerk;
        
        public Double JogLowVelocity;
        public Double JogHighVelocity;
        public Double JogAcc;
        public Double JogDec;
        public Double JogJerk;
        
        public Double MoveLowVelocity;
        public Double MoveHighVelocity;
        public Double MoveAcc;
        public Double MoveDec;
        public Double MoveJerk;

        #endregion

        #region Status.

        public bool Org;
        public bool Ez;
        public bool Pel;
        public bool Mel;

        public AxisState State;

        #endregion

        #endregion
    }

    // Helper Class to wrap the Advantech Common Motion APIs.
    public class AdvantechMotion
    {
        #region Properties.

        DEV_LIST[] CurAvailableDevs = new DEV_LIST[Motion.MAX_DEVICES];
        uint deviceCount = 0;
        uint DeviceNum = 0;
        IntPtr m_DeviceHandle = IntPtr.Zero;

        IntPtr[] m_AxisHandle = new IntPtr[32];

        public Advantech_Axis[] axis = new Advantech_Axis[32];
        //int[] m_HomeMode = new int[32];
        //int[] m_DirMode = new int[32];
        //Double[] m_LowVelocity = new Double[32];
        //Double[] m_HighVelocity = new Double[32];
        //int[] m_SwitchMode = new int[32];
        //Double[] m_CrossDistance = new Double[32];

        uint m_ulAxisCount = 0;
        bool m_bInit = false;
        bool m_bServoOn = false;

        #region Motion IO Status.
        
        bool ORG = false; // Origin (Home Position).
        bool PEL = false; // Positive End Limit.
        bool MEL = false; // Negative End Limit.

        #endregion

        string ErrorMessage = "";

        #endregion

        #region Helpers.

        #region Open.

        public bool Open()
        {
            bool bSuccess = false;

            ErrorMessage = "";

            Close();

            FindDevice();
            
            bSuccess = OpenDevice();

            if (bSuccess)
            {
                Init();
            }

            return bSuccess;
        }

        public bool Open(string configFileName)
        {
            bool bSuccess = false;

            ErrorMessage = "";

            try
            {
                FindDevice();
            
                bSuccess = OpenDevice();

                if (bSuccess)
                {
                    bSuccess = LoadCfg(configFileName);

                    if (bSuccess)
                    {
                        Init();
                    }
                }
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - Open - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        public void FindDevice()
        {
            int Result;
            
            try
            {
                Result = Motion.mAcm_GetAvailableDevs(CurAvailableDevs, Motion.MAX_DEVICES, ref deviceCount);

                if (deviceCount > 0)
                {
                    DeviceNum = CurAvailableDevs[0].DeviceNum;
                }
                else
                {
                    DeviceNum = 0;
                }
            }
            catch (Exception e)
            {
                ErrorMessage = "modMotion_Advantech - FindDevice - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }
        }

        public bool OpenDevice()
        {
            bool bSuccess = false;

            uint Result;
            uint i = 0;
            uint[] slaveDevs = new uint[16];
            uint AxesPerDev = new uint();
            uint AxisNumber;
            uint buffLen = 0;
            
            try
            {
                #region Open Device.

                Result = Motion.mAcm_DevOpen(DeviceNum, ref m_DeviceHandle);
            
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    ErrorMessage = "Can Not Open Device";
                    return false;
                }

                #endregion

                #region Open Axes.

                buffLen = 4;
                Result = Motion.mAcm_GetProperty(m_DeviceHandle, (uint)PropertyID.FT_DevAxesCount, ref AxesPerDev, ref buffLen);
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    ErrorMessage = "Get Property Error";
                    return false;
                }
            
                AxisNumber = AxesPerDev;
                buffLen = 64;
                Result = Motion.mAcm_GetProperty(m_DeviceHandle, (uint)PropertyID.CFG_DevSlaveDevs, slaveDevs, ref buffLen);
                if (Result == (uint)ErrorCode.SUCCESS)
                {
                    i = 0;
                    while (slaveDevs[i] != 0)
                    {
                        AxisNumber += AxesPerDev;
                        i++;
                    }
                }

                m_ulAxisCount = AxisNumber;
                //CmbAxes.Items.Clear();
                for (i = 0; i < m_ulAxisCount; i++)
                {
                    // Open every Axis and get the each Axis Handle
                    // And Initial property for each Axis.

                    // Open Axis.
                    Result = Motion.mAcm_AxOpen(m_DeviceHandle, (UInt16)i, ref m_AxisHandle[i]);
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        ErrorMessage = "Open Axis Failed";
                        return false;
                    }

                    // Reset Command Counter.
                    double cmdPosition = new double();
                    cmdPosition = 0;
                    Motion.mAcm_AxSetCmdPosition(m_AxisHandle[i], cmdPosition);
                }

                #endregion

                m_bInit = true;
                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - OpenDevice - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        public void Init()
        {
            int i = 0;

            for (i = 0; i < m_ulAxisCount; i++)
            {
                if (axis[i] == null)
                {
                    axis[i] = new Advantech_Axis();
                }

                axis[i].AxisNumber = i;
                axis[i].HomeMode = (int)HomeMode.Mode12_AbsSearchReFind;
                axis[i].SwitchMode = (int)SwitchMode.Edge_On;

                //modMotion.advantech.axis[i].EzLogic = 0;
                //SetProperty_EzLogic(i);

                //modMotion.advantech.axis[i].ElLogic = 0;
                //SetProperty_ElLogic(i);

                //modMotion.advantech.axis[i].OrgLogic = 0;
                //SetProperty_OrgLogic(i);

                axis[i].HomeJerk = (int)VelocityProfile.S_Curve;
                axis[i].JogJerk = (int)VelocityProfile.S_Curve;
                axis[i].MoveJerk = (int)VelocityProfile.S_Curve;
            }

            // Home Direction.
            axis[(int) Axis.X].DirMode = (int) DirMode.Positive_Direction;
            axis[(int) Axis.Y].DirMode = (int) DirMode.Negative_Direction;
            axis[(int) Axis.Z].DirMode = (int) DirMode.Positive_Direction;
            axis[(int) Axis.W].DirMode = (int) DirMode.Negative_Direction;

            ServoOn(true);
        }
        
        public bool LoadCfg(string configFileName)
        {
            bool bSuccess = false;
            UInt32 Result;

            try
            {
                if (!m_bInit) return false;

                if (!File.Exists(configFileName)) return false;

                Result = Motion.mAcm_DevLoadConfig(m_DeviceHandle, configFileName);
            
                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    ErrorMessage = "Load File Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                    return false;
                }
                
                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - LoadCfg - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        public bool ServoOn(bool bOn)
        {
            bool bSuccess = false;
            UInt32 AxisNum;
            UInt32 Result;
            uint onOff;
            
            //Check the servoOno flag to decide if turn on or turn off the ServoOn output.
            if (!m_bInit) return false;
            
            if (bOn)
            {
                onOff = 1;
            }
            else
            {
                onOff = 0;
            }

            try
            {
                for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
                {
                    Result = Motion.mAcm_AxSetSvOn(m_AxisHandle[AxisNum], onOff);
                
                    if (Result != (uint)ErrorCode.SUCCESS)
                    {
                        ErrorMessage = "modMotion_Advantech - ServoOn - Servo On failed with Error Code[0x" + Convert.ToString(Result, 16) + "]";
                        ////UtilFile.LogDebug(ErrorMessage);
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - ServoOn - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }

            bSuccess = true;

            return bSuccess;
        }

        #endregion

        #region Close.

        public bool Close()
        {
            bool bSuccess = false;

            UInt16[] usAxisState = new UInt16[32];
            uint AxisNum;

            if (!m_bInit) return false;

            try
            {
                // Stop all axes.
                for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
                {
                    Motion.mAcm_AxGetState(m_AxisHandle[AxisNum], ref usAxisState[AxisNum]);

                    if (usAxisState[AxisNum] == (uint)AxisState.STA_AX_ERROR_STOP)
                    {
                        Motion.mAcm_AxResetError(m_AxisHandle[AxisNum]);
                    }

                    Motion.mAcm_AxStopDec(m_AxisHandle[AxisNum]);
                }

                // Close all axes.
                for (AxisNum = 0; AxisNum < m_ulAxisCount; AxisNum++)
                {
                    Motion.mAcm_AxClose(ref m_AxisHandle[AxisNum]);
                }

                m_ulAxisCount = 0;

                // Close Device.
                Motion.mAcm_DevClose(ref m_DeviceHandle);
                m_DeviceHandle = IntPtr.Zero;
                m_bInit = false;

                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - Close - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        #endregion

        #region Stop.

        #region Stop with deceleration.

        public bool StopX_Dec()
        {
            return Stop_Dec((int)Axis.X);
        }

        public bool StopY_Dec()
        {
            return Stop_Dec((int)Axis.Y);
        }

        public bool StopZ_Dec()
        {
            return Stop_Dec((int)Axis.Z);
        }

        public bool StopW_Dec()
        {
            return Stop_Dec((int)Axis.W);
        }

        public bool Stop_Dec(int axisNumber)
        {
            bool bSuccess = false;
            UInt16 AxState = new UInt16();
            int i = 0;

            if (!m_bInit) return false;

            try
            {
                // If the axis is in error state , reset it first, then stop the axis.
                Motion.mAcm_AxGetState(m_AxisHandle[axisNumber], ref AxState);

                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(m_AxisHandle[axisNumber]);
                }

                Motion.mAcm_AxStopDec(m_AxisHandle[axisNumber]);

                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - Stop_Dec - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        // Command all axes to stop with decelerating.
        public bool StopAll_Dec()
        {
            bool bSuccess = false;
            UInt16 AxState = new UInt16();
            int i = 0;

            if (!m_bInit) return false;

            try
            {
                for (i = 0; i < m_ulAxisCount; i++)
                {
                    // If the axis is in error state , reset it first, then stop the axis.
                    Motion.mAcm_AxGetState(m_AxisHandle[i], ref AxState);

                    if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                    {
                        Motion.mAcm_AxResetError(m_AxisHandle[i]);
                    }

                    Motion.mAcm_AxStopDec(m_AxisHandle[i]);
                }

                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - StopAll_Dec - " + e.Message;
                ////UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }
        
        #endregion

        #region Stop immediately without decelerating.

        public bool StopX_Emg()
        {
            return Stop_Emg((int)Axis.X);
        }

        public bool StopY_Emg()
        {
            return Stop_Emg((int)Axis.Y);
        }

        public bool StopZ_Emg()
        {
            return Stop_Emg((int)Axis.Z);
        }

        public bool StopW_Emg()
        {
            return Stop_Emg((int)Axis.W);
        }

        // Command all axes to stop without decelerating as Emergency Stop.
        public bool Stop_Emg(int axisNumber)
        {
            bool bSuccess = false;
            UInt16 AxState = new UInt16();
            int i = 0;

            if (!m_bInit) return false;

            try
            {
                // If the axis is in error state , reset it first, then stop the axis.
                Motion.mAcm_AxGetState(m_AxisHandle[axisNumber], ref AxState);

                if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(m_AxisHandle[axisNumber]);
                }

                Motion.mAcm_AxStopEmg(m_AxisHandle[axisNumber]);

                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - Stop_Emg - " + e.Message;
                //UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        // Command all axes to stop without decelerating as Emergency Stop.
        public bool StopAll_Emg()
        {
            bool bSuccess = false;
            UInt16 AxState = new UInt16();
            int i = 0;

            if (!m_bInit) return false;

            try
            {
                for (i = 0; i < m_ulAxisCount; i++)
                {
                    // If the axis is in error state , reset it first, then stop the axis.
                    Motion.mAcm_AxGetState(m_AxisHandle[i], ref AxState);

                    if (AxState == (uint)AxisState.STA_AX_ERROR_STOP)
                    {
                        Motion.mAcm_AxResetError(m_AxisHandle[i]);
                    }

                    Motion.mAcm_AxStopEmg(m_AxisHandle[i]);
                }

                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - StopAll_Emg - " + e.Message;
                //UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        #endregion

        #endregion

        #region Status.

        public void ResetError(int axisNumber)
        {
            if (!m_bInit) return;

            Motion.mAcm_AxResetError(m_AxisHandle[axisNumber]);
        }

        public void GetState()
        {
            int i;

            for (i = 0; i < m_ulAxisCount; i++)
            {
                GetState(i);
            }
        }

        public void GetState(int axisNumber)
        {
            if (!m_bInit) return;

            UInt16 AxState = new UInt16();

            Motion.mAcm_AxGetState(m_AxisHandle[axisNumber], ref AxState);

            axis[axisNumber].State = (AxisState) AxState;
        }

        public string GetStateStr(int axisStateNumber)
        {
            string strState = "";

            switch (axisStateNumber)
            {
                case 0:
                    strState = "DISABLE";
                    break;

                case 1:
                    strState = "READY";
                    break;

                case 2:
                    strState = "STOPPING";
                    break;

                case 3:
                    strState = "ERROR_STOP";
                    break;

                case 4:
                    strState = "HOMING";
                    break;

                case 5:
                    strState = "PTP_MOT";
                    break;

                case 6:
                    strState = "CONTI_MOT";
                    break;

                case 7:
                    strState = "SYNC_MOT";
                    break;

                case 8:
                    strState = "EXT_JOG";
                    break;

                case 9:
                    strState = "EXT_MPG";
                    break;

                default:
                    strState = "N/A";
                    break;
            }

            return strState;
        }

        public void GetMotionIO()
        {
            int i;

            for (i = 0; i < m_ulAxisCount; i++)
            {
                GetMotionIO(i);
            }
        }

        public void GetMotionIO(int axisNumber)
        {
            UInt32 Result;
            UInt32 IOStatus = new UInt32();

            Result = Motion.mAcm_AxGetMotionIO(m_AxisHandle[axisNumber], ref IOStatus);
            if (Result == (uint)ErrorCode.SUCCESS)
            {
                axis[axisNumber].Mel = ((IOStatus & 0x8) > 0);    // -EL.
                axis[axisNumber].Org = ((IOStatus & 0x10) > 0);   // ORG.
                axis[axisNumber].Pel = ((IOStatus & 0x4) > 0);    // +EL.
                axis[axisNumber].Ez  = ((IOStatus & 0x200) > 0);  // EZ.
            }
        }

        public bool OrgXOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.X].Org;

            return bOn;
        }

        public bool OrgYOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.Y].Org;

            return bOn;
        }

        public bool OrgZOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.Z].Org;

            return bOn;
        }

        public bool OrgWOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.W].Org;

            return bOn;
        }

        public bool MelXOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.X].Mel;

            return bOn;
        }

        public bool MelYOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.Y].Mel;

            return bOn;
        }

        public bool MelZOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.Z].Mel;

            return bOn;
        }

        public bool MelWOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.W].Mel;

            return bOn;
        }


        public bool PelXOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.X].Pel;

            return bOn;
        }

        public bool PelYOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.Y].Pel;

            return bOn;
        }

        public bool PelZOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.Z].Pel;

            return bOn;
        }

        public bool PelWOn()
        {
            bool bOn = false;

            bOn = axis[(int)Axis.W].Pel;

            return bOn;
        }

        public bool ReadyX()
        {
            bool bOn = false;

            bOn = (axis[(int)Axis.X].State == AxisState.STA_AX_READY);

            return bOn;
        }

        public bool ReadyY()
        {
            bool bOn = false;

            bOn = (axis[(int)Axis.Y].State == AxisState.STA_AX_READY);

            return bOn;
        }

        public bool ReadyZ()
        {
            bool bOn = false;

            bOn = (axis[(int)Axis.Z].State == AxisState.STA_AX_READY);

            return bOn;
        }

        public bool ReadyW()
        {
            bool bOn = false;

            bOn = (axis[(int)Axis.W].State == AxisState.STA_AX_READY);

            return bOn;
        }

        #endregion

        #region Positions.

        public void ResetPosition(int axisNumber)
        {
            if (!m_bInit) return;

            double position = new double();
            position = 0;

            Motion.mAcm_AxSetCmdPosition(m_AxisHandle[axisNumber], position);

            Motion.mAcm_AxSetActualPosition(m_AxisHandle[axisNumber], position);
        }

        public void GetCmdPosition(int axisNumber, ref double position)
        {
            if (!m_bInit) return;

            Motion.mAcm_AxGetCmdPosition(m_AxisHandle[axisNumber], ref position);
        }

        public void GetActualPosition(int axisNumber, ref double position)
        {
            if (!m_bInit) return;

            Motion.mAcm_AxGetActualPosition(m_AxisHandle[axisNumber], ref position);
        }

        public void SetPosition(int axisNumber, double position)
        {
            if (!m_bInit) return;

            Motion.mAcm_AxSetCmdPosition(m_AxisHandle[axisNumber], position);

            Motion.mAcm_AxSetActualPosition(m_AxisHandle[axisNumber], position);
        }

        #endregion

        #region Settings.

        #region Settings - General.

        public bool SetProperty_EzLogic(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            // 0 = Low. 1 = High.
            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.CFG_AxElLogic,
                                             ref axis[axisNumber].EzLogic,
                                             (uint)Marshal.SizeOf(typeof(UInt32)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_EzLogic: Set Property-CFG_AxElLogic Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | OrgLogic = " + axis[axisNumber].EzLogic.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_ElLogic(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            // 0 = Low. 1 = High.
            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.CFG_AxElLogic,
                                             ref axis[axisNumber].ElLogic,
                                             (uint)Marshal.SizeOf(typeof(UInt32)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_ElLogic: Set Property-CFG_AxElLogic Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | ElLogic = " + axis[axisNumber].ElLogic.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_OrgLogic(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            // 0 = Low. 1 = High.
            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.CFG_AxOrgLogic,
                                             ref axis[axisNumber].OrgLogic,
                                             (uint)Marshal.SizeOf(typeof(UInt32)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_OrgLogic: Set Property-CFG_AxOrgLogic Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | OrgLogic = " + axis[axisNumber].OrgLogic.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_HomeCrossDistance(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxHomeCrossDistance,
                                             ref axis[axisNumber].HomeCrossDistance,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeCrossDistance: Set Property-PAR_AxHomeCrossDistance Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | HomeCrossDistance = " + axis[axisNumber].HomeCrossDistance.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_HomeExSwitchMode(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxHomeExSwitchMode,
                                             ref axis[axisNumber].DirMode,
                                             (uint)Marshal.SizeOf(typeof(UInt32)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeExSwitchMode: Set Property-PAR_AxHomeExSwitchMode Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | SwitchMode = " + axis[axisNumber].SwitchMode.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_HomeResetEnable(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;
            int enable = 1;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.CFG_AxHomeResetEnable,
                                             ref enable,
                                             (uint)Marshal.SizeOf(typeof(UInt32)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeResetEnable: Set Property-CFG_AxHomeResetEnable Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString();
                return false;
            }

            return bSuccess;
        }

        #endregion

        #region Settings - Speed.

        public bool SetSpeed(int axisNumber, int speedMode)
        {
            bool bSuccess = false;

            switch (speedMode)
            {
                case (int) SpeedMode.Home:
                    bSuccess  = SetProperty_HomeLowVelocity(axisNumber);
                    bSuccess &= SetProperty_HomeHighVelocity(axisNumber);
                    bSuccess &= SetProperty_HomeAcc(axisNumber);
                    bSuccess &= SetProperty_HomeDec(axisNumber);
                    bSuccess &= SetProperty_HomeJerk(axisNumber);
                    break;

                case (int) SpeedMode.Jog:
                    bSuccess  = SetProperty_JogLowVelocity(axisNumber);
                    bSuccess &= SetProperty_JogHighVelocity(axisNumber);
                    bSuccess &= SetProperty_JogAcc(axisNumber);
                    bSuccess &= SetProperty_JogDec(axisNumber);
                    bSuccess &= SetProperty_JogJerk(axisNumber);
                    break;

                case (int) SpeedMode.Move:
                    bSuccess  = SetProperty_MoveLowVelocity(axisNumber);
                    bSuccess &= SetProperty_MoveHighVelocity(axisNumber);
                    bSuccess &= SetProperty_MoveAcc(axisNumber);
                    bSuccess &= SetProperty_MoveDec(axisNumber);
                    bSuccess &= SetProperty_MoveJerk(axisNumber);
                    break;
            }

            return bSuccess;
        }

        #region Settings - Home Speed.

        public bool SetProperty_HomeLowVelocity(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber], 
                                             (uint)PropertyID.PAR_AxVelLow, 
                                             ref axis[axisNumber].HomeLowVelocity, 
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeLowVelocity: Set Property-PAR_AxVelLow Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | HomeLowVelocity = " + axis[axisNumber].HomeLowVelocity.ToString();
                return false;
            }

            return bSuccess; 
        }

        public bool SetProperty_HomeHighVelocity(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxVelHigh,
                                             ref axis[axisNumber].HomeHighVelocity,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeHighVelocity: Set Property-PAR_AxVelHigh Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | HomeHighVelocity = " + axis[axisNumber].HomeHighVelocity.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_HomeAcc(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxAcc,
                                             ref axis[axisNumber].HomeAcc,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeAcc: Set Property-PAR_AxAcc Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | HomeAcc = " + axis[axisNumber].HomeAcc.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_HomeDec(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxDec,
                                             ref axis[axisNumber].HomeDec,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeDec: Set Property-PAR_AxDec Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | HomeDec = " + axis[axisNumber].HomeDec.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_HomeJerk(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxJerk,
                                             ref axis[axisNumber].HomeJerk,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_HomeJerk: Set Property-PAR_AxJerk Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | HomeJerk = " + axis[axisNumber].HomeJerk.ToString();
                return false;
            }

            return bSuccess;
        }

        #endregion

        #region Settings - Jog Speed.

        public bool SetProperty_JogLowVelocity(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxVelLow,
                                             ref axis[axisNumber].JogLowVelocity,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_JogLowVelocity: Set Property-PAR_AxVelLow Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | JogLowVelocity = " + axis[axisNumber].JogLowVelocity.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_JogHighVelocity(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxVelHigh,
                                             ref axis[axisNumber].JogHighVelocity,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_JogHighVelocity: Set Property-PAR_AxVelHigh Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | JogHighVelocity = " + axis[axisNumber].JogHighVelocity.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_JogAcc(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxAcc,
                                             ref axis[axisNumber].JogAcc,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_JogAcc: Set Property-PAR_AxAcc Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | JogAcc = " + axis[axisNumber].JogAcc.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_JogDec(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxDec,
                                             ref axis[axisNumber].JogDec,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_JogDec: Set Property-PAR_AxDec Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | JogDec = " + axis[axisNumber].JogDec.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_JogJerk(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxJerk,
                                             ref axis[axisNumber].JogJerk,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_JogJerk: Set Property-PAR_AxJerk Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | JogJerk = " + axis[axisNumber].JogJerk.ToString();
                return false;
            }

            return bSuccess;
        }

        #endregion

        #region Settings - Move Speed.

        public bool SetProperty_MoveLowVelocity(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxVelLow,
                                             ref axis[axisNumber].MoveLowVelocity,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_MoveLowVelocity: Set Property-PAR_AxVelLow Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | MoveLowVelocity = " + axis[axisNumber].MoveLowVelocity.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_MoveHighVelocity(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxVelHigh,
                                             ref axis[axisNumber].MoveHighVelocity,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_MoveHighVelocity: Set Property-PAR_AxVelHigh Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | MoveHighVelocity = " + axis[axisNumber].MoveHighVelocity.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_MoveAcc(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxAcc,
                                             ref axis[axisNumber].MoveAcc,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_MoveAcc: Set Property-PAR_AxAcc Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | MoveAcc = " + axis[axisNumber].MoveAcc.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_MoveDec(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxDec,
                                             ref axis[axisNumber].MoveDec,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_MoveDec: Set Property-PAR_AxDec Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | MoveDec = " + axis[axisNumber].MoveDec.ToString();
                return false;
            }

            return bSuccess;
        }

        public bool SetProperty_MoveJerk(int axisNumber)
        {
            bool bSuccess = true;
            UInt32 Result;

            Result = Motion.mAcm_SetProperty(m_AxisHandle[axisNumber],
                                             (uint)PropertyID.PAR_AxJerk,
                                             ref axis[axisNumber].MoveJerk,
                                             (uint)Marshal.SizeOf(typeof(double)));

            if (Result != (uint)ErrorCode.SUCCESS)
            {
                ErrorMessage = "SetProperty_MoveJerk: Set Property-PAR_AxJerk Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]. Axis Number = " + axisNumber.ToString() + " | MoveJerk = " + axis[axisNumber].MoveJerk.ToString();
                return false;
            }

            return bSuccess;
        }

        #endregion

        #endregion

        #endregion

        #region Move.

        #region Relative Move.

        public bool MoveRel(int axisNumber, double distance, int speedMode)
        {
            bool bSuccess = false;
            UInt32 Result;

            if (!m_bInit) return false;

            try
            {
                SetSpeed(axisNumber, speedMode);

                if (axis[axisNumber].State == AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(m_AxisHandle[axisNumber]);
                }

                Result = Motion.mAcm_AxMoveRel(m_AxisHandle[axisNumber], distance);

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    ErrorMessage = "PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                    return false;
                }
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - MoveRel - " + e.Message;
                //UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        public bool MoveRelX(double distance)
        {
            return MoveRel((int)Axis.X, distance, (int) SpeedMode.Move);
        }

        public bool MoveRelY(double distance)
        {
            return MoveRel((int)Axis.Y, distance, (int) SpeedMode.Move);
        }

        public bool MoveRelZ(double distance)
        {
            return MoveRel((int)Axis.Z, distance, (int) SpeedMode.Move);
        }

        public bool MoveRelW(double distance)
        {
            return MoveRel((int)Axis.W, distance, (int) SpeedMode.Move);
        }

        public bool MoveRelX_JogSpeed(double distance)
        {
            return MoveRel((int)Axis.X, distance, (int) SpeedMode.Jog);
        }

        public bool MoveRelY_JogSpeed(double distance)
        {
            return MoveRel((int)Axis.Y, distance, (int) SpeedMode.Jog);
        }

        public bool MoveRelZ_JogSpeed(double distance)
        {
            return MoveRel((int)Axis.Z, distance, (int) SpeedMode.Jog);
        }

        public bool MoveRelW_JogSpeed(double distance)
        {
            return MoveRel((int)Axis.W, distance, (int) SpeedMode.Jog);
        }

        #endregion

        #region Continuous Move.

        public bool MoveContinuous(int axisNumber, MotionDirection direction, int speedMode)
        {
            bool bSuccess = false;
            UInt32 Result;

            if (!m_bInit) return false;

            try
            {
                SetSpeed(axisNumber, speedMode);

                if (axis[axisNumber].State == AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(m_AxisHandle[axisNumber]);
                }

                Result = Motion.mAcm_AxMoveVel(m_AxisHandle[axisNumber], (ushort) direction);

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    ErrorMessage = "PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                    return false;
                }
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - MoveContinuous - " + e.Message;
                //UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        public bool MoveContinuousX(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.X, direction, (int)SpeedMode.Move);
        }

        public bool MoveContinuousX_JogSpeed(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.X, direction, (int)SpeedMode.Jog);
        }

        public bool MoveContinuousY(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.Y, direction, (int)SpeedMode.Move);
        }

        public bool MoveContinuousY_JogSpeed(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.Y, direction, (int)SpeedMode.Jog);
        }

        public bool MoveContinuousZ(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.Z, direction, (int)SpeedMode.Move);
        }

        public bool MoveContinuousZ_JogSpeed(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.Z, direction, (int)SpeedMode.Jog);
        }

        public bool MoveContinuousW(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.W, direction, (int)SpeedMode.Move);
        }

        public bool MoveContinuousW_JogSpeed(MotionDirection direction)
        {
            return MoveContinuous((int)Axis.W, direction, (int)SpeedMode.Jog);
        }

        #endregion

        #region Absolute Move.

        public bool MoveAbs(int axisNumber, double position, int speedMode)
        {
            bool bSuccess = false;
            UInt32 Result;

            if (!m_bInit) return false;

            try
            {
                SetSpeed(axisNumber, speedMode);

                if (axis[axisNumber].State == AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(m_AxisHandle[axisNumber]);
                }

                Result = Motion.mAcm_AxMoveAbs(m_AxisHandle[axisNumber], position);

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    ErrorMessage = "modMotion_Advantech - MoveAbs - PTP Move Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                    //UtilFile.LogDebug(ErrorMessage);
                    return false;
                }
                
                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - MoveAbs - " + e.Message;
                //UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        public bool MoveAbsX(double position)
        {
            return MoveAbs((int)Axis.X, position, (int) SpeedMode.Move);
        }

        public bool MoveAbsY(double position)
        {
            return MoveAbs((int)Axis.Y, position, (int) SpeedMode.Move);
        }

        public bool MoveAbsZ(double position)
        {
            return MoveAbs((int)Axis.Z, position, (int) SpeedMode.Move);
        }

        public bool MoveAbsW(double position)
        {
            return MoveAbs((int)Axis.W, position, (int) SpeedMode.Move);
        }

        public bool MoveAbsX_JogSpeed(double position)
        {
            return MoveAbs((int)Axis.X, position, (int) SpeedMode.Jog);
        }

        public bool MoveAbsY_JogSpeed(double position)
        {
            return MoveAbs((int)Axis.Y, position, (int) SpeedMode.Jog);
        }

        public bool MoveAbsZ_JogSpeed(double position)
        {
            return MoveAbs((int)Axis.Z, position, (int) SpeedMode.Jog);
        }

        public bool MoveAbsW_JogSpeed(double position)
        {
            return MoveAbs((int)Axis.W, position, (int) SpeedMode.Jog);
        }

        #endregion

        #endregion

        #region Home.

        public bool Home(int axisNumber)
        {
            bool bSuccess = false;
            UInt32 Result;

            if (!m_bInit) return false;

            try
            {
                if (axis[axisNumber].State == AxisState.STA_AX_ERROR_STOP)
                {
                    Motion.mAcm_AxResetError(m_AxisHandle[axisNumber]);
                }

                bSuccess = SetProperty_HomeLowVelocity(axisNumber);
                bSuccess &= SetProperty_HomeHighVelocity(axisNumber);
                bSuccess &= SetProperty_HomeAcc(axisNumber);
                bSuccess &= SetProperty_HomeDec(axisNumber);
                bSuccess &= SetProperty_HomeJerk(axisNumber);
                bSuccess &= SetProperty_HomeExSwitchMode(axisNumber);
                bSuccess &= SetProperty_HomeCrossDistance(axisNumber);
                bSuccess &= SetProperty_HomeResetEnable(axisNumber);

                if (!bSuccess) return false;

                Result = Motion.mAcm_AxHome(m_AxisHandle[axisNumber], (UInt32)axis[axisNumber].HomeMode, (UInt32)axis[axisNumber].DirMode);

                if (Result != (uint)ErrorCode.SUCCESS)
                {
                    ErrorMessage = "modMotion_Advantech - Home - AxHome Failed With Error Code[0x" + Convert.ToString(Result, 16) + "]";
                    //UtilFile.LogDebug(ErrorMessage);
                    return false;
                }

                bSuccess = true;
            }
            catch (Exception e)
            {
                bSuccess = false;
                ErrorMessage = "modMotion_Advantech - Home " + e.Message;
                //UtilFile.LogDebug(ErrorMessage);
            }

            return bSuccess;
        }

        public bool HomeX()
        {
            bool bSuccess = false;

            bSuccess = Home((int)Axis.X);

            return bSuccess;
        }

        public bool HomeY()
        {
            bool bSuccess = false;

            bSuccess = Home((int)Axis.Y);

            return bSuccess;
        }

        public bool HomeZ()
        {
            bool bSuccess = false;

            bSuccess = Home((int)Axis.Z);

            return bSuccess;
        }

        public bool HomeW()
        {
            bool bSuccess = false;

            bSuccess = Home((int)Axis.W);

            return bSuccess;
        }

        #endregion

        #endregion
    }
}
