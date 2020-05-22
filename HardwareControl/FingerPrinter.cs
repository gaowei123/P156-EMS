using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ObjectModule.Local;
using StaticRes;

namespace HardwareControl
{
    public class FringerPrint
    {
        private ObjectModule.Local.Event et = new Event();
        private string Register_Finger_Template = string.Empty;

        #region delegate
        public delegate void RegisterCompleteEventHandler(string Finger_Template);
        private RegisterCompleteEventHandler registercomplete;
        public event RegisterCompleteEventHandler RegisterComplete
        {
            add
            {
                registercomplete += value;
            }
            remove
            {
                registercomplete -= value;
            }
        }

        public delegate void registererrorEventHandler(string Error_Message);
        private registererrorEventHandler registererror;
        public event registererrorEventHandler RegisterError
        {
            add
            {
                registererror += value;
            }
            remove
            {
                registererror -= value;
            }
        }
        #endregion

        private Thread Thread_Register = null;

        public void Stop()
        {
            this.Thread_Register.Abort();
        }

        void FringerPrint_RegisterComplete(string Finger_Template)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void FringerPrint_RegisterError(string Error_Messagee)
        {
            StaticRes.Global.IsOnProgress = false;
        }

        void Register_Step(string Event)
        {
        }

        class Params
        {
            public string User_ID;
            public string Finger_Template;
            public string Finger_Template_1;
            public bool Login;
        }

        public int Start_Register(string User_ID,string Finger_Template,bool Login,string Finger_Template_1)
        {
            if (StaticRes.Global.IsOnProgress)
            {
                registercomplete(Register_Finger_Template);
                return 0;
            }
            else
                StaticRes.Global.IsOnProgress = true;
            try
            {
                Params x = new Params();
                x.User_ID = User_ID;
                x.Finger_Template = Finger_Template;
                x.Login = Login;
                x.Finger_Template_1=Finger_Template_1;
                RegisterComplete+=new RegisterCompleteEventHandler(FringerPrint_RegisterComplete);
                RegisterError+=new registererrorEventHandler(FringerPrint_RegisterError);
                this.Thread_Register = new Thread(new ParameterizedThreadStart(Registering));
                this.Thread_Register.Start(x);
            }
            catch
            {
                registercomplete(Register_Finger_Template);

                return 0;
            }
            return 11;
        }

        private void Registering(object x)
        {
            try
            {
                Params y = (Params)x;
                LogicRegister(y.User_ID,y.Finger_Template,y.Login,y.Finger_Template_1);
            }
            catch
            {
                //StaticRes.Global.Process_Code.Loading = "L000";
            }
        }

        private void LogicRegister(string User_ID,string Finger_Template,bool Login,string Finger_Template_1)
        {
            try
            {
                #region Login登录
                if (Login)
                {
                    for (int i = 0; i < 10; i++)
                    {

                        if (StaticRes.Global.FingerPrint_Continue)
                        {
                            if (Hardware.Finger.MXOTDLL.MXIsFingerOn() != 1)
                                System.Threading.Thread.Sleep(1000);   //1秒
                            else
                                break;
                            if (i == 9)
                            {
                                registererror("Sorry , you have time out !!");
                                return;
                            }
                        }
                        else
                            return;
                    }

                    Byte[] FingerBuf = new Byte[256 * 304];//字符数组 byte[]是字节数组
                    string strUsername = User_ID;
                    string sTemplateOutDB;
                    Byte[] TemplateOutDB = new Byte[512];
                    Byte[] FeatureOutDB = new Byte[256];
                    Byte[] FeatureDevice = new Byte[256];

                    if (Hardware.Finger.MXOTDLL.MXReadFingerFromSensor(FingerBuf) == 1)
                    {
                        if (Hardware.Finger.MXOTDLL.MXSaveFingerToFile(FingerBuf, System.IO.Directory.GetCurrentDirectory() + "\\FingerBmp\\Finger.bmp") == 1)
                        {
                            if (Hardware.Finger.MXOTDLL.MXExtract(FingerBuf, FeatureDevice) == 0)
                            {
                                sTemplateOutDB = Finger_Template;
                                TemplateOutDB = System.Text.Encoding.Default.GetBytes(sTemplateOutDB);

                                AscToHex(TemplateOutDB, 256, FeatureOutDB);
                                int nRet = Hardware.Finger.MXOTDLL.MXIdentify(FeatureOutDB, FeatureDevice);
                                if (nRet >= 30)
                                {
                                    //StaticRes.Global.Current_User = User;
                                    //CloseComPort();
                                    //LoginSuccessful();
                                    Hardware.Finger.MXOTDLL.MXReleaseDev();
                                    registercomplete(Register_Finger_Template);
                                    return;
                                }
                                else
                                {
                                    sTemplateOutDB = Finger_Template_1;
                                    TemplateOutDB = System.Text.Encoding.Default.GetBytes(sTemplateOutDB);

                                    AscToHex(TemplateOutDB, 256, FeatureOutDB);
                                    nRet = Hardware.Finger.MXOTDLL.MXIdentify(FeatureOutDB, FeatureDevice);
                                    if (nRet >= 30)
                                    {
                                        //StaticRes.Global.Current_User = User;
                                       // CloseComPort();
                                        //LoginSuccessful();
                                        Hardware.Finger.MXOTDLL.MXReleaseDev();
                                        registercomplete(Register_Finger_Template);
                                        return;
                                    }
                                    else
                                    {
                                        registererror("Finger image not match ,Please try agin !!");
                                        return;
                                    }
                                }
                            }
                        }
                        else
                        {
                            registererror("Save image fail !!");
                            return;
                        }
                    }
                    else
                    {
                        registererror("Get image fail !!");
                        return;
                    }
                }
                #endregion
                #region 注册
                else //Register in USER DB
                {
                    for (int i = 0; i < 10; i++)
                    {

                        if (StaticRes.Global.FingerPrint_Continue)
                        {
                            if (Hardware.Finger.MXOTDLL.MXIsFingerOn() != 1)
                                System.Threading.Thread.Sleep(1000);   //1秒
                            else
                                break;
                            if (i == 9)
                            {
                                registererror("Sorry , you have time out !!");
                                return;
                            }
                        }
                        else
                            return;
                    }
                    Byte[] FingerBuf = new Byte[256 * 304];//字符数组 byte[]是字节数组
                    Byte[] Feature = new Byte[256];
                    Byte[] Feature1 = new Byte[256];
                    Byte[] Feature2 = new Byte[256];
                    Byte[] Template = new Byte[256];
                    Byte[] TemplateInDB = new Byte[512];
                    int nSum = 0;

                    while (true && StaticRes.Global.Transaction_Continue)
                    {
                        if (Hardware.Finger.MXOTDLL.MXIsFingerOn() == 1)
                        {
                            if (Hardware.Finger.MXOTDLL.MXReadFingerFromSensor(FingerBuf) == 1)
                            {
                                if (Hardware.Finger.MXOTDLL.MXSaveFingerToFile(FingerBuf, System.IO.Directory.GetCurrentDirectory() + "\\FingerBmp\\Finger.bmp") == 1)
                                {

                                     if (Hardware.Finger.MXOTDLL.MXExtract(FingerBuf, Feature) == 0)
                                    {
                                        if (nSum == 0)
                                        {
                                            //memcpy(Feature1, Feature, 256);
                                            for (int i = 0; i < 256; i++)
                                            {
                                                Feature1[i] = Feature[i];
                                            }
                                        }
                                        if (nSum == 1)
                                        {
                                            //memcpy(Feature2, Feature, 256);
                                            for (int i = 0; i < 256; i++)
                                            {
                                                Feature2[i] = Feature[i];
                                            }
                                        }
                                        nSum++;
                                        if (nSum >= 2)
                                        {
                                            //合并指纹特征，产生指纹模板
                                            if (Hardware.Finger.MXOTDLL.MXMerge2(Feature1, Feature2, Template) == 1)
                                            {
                                                HexToAsc(Template, 256, TemplateInDB);
                                                Register_Finger_Template = System.Text.Encoding.Default.GetString(TemplateInDB);
                                                registercomplete(Register_Finger_Template);
                                                return;
                                            }
                                            nSum = 1;
                                            //memcpy(Feature1, Feature2, 256);
                                            for (int i = 0; i < 256; i++)
                                            {
                                                Feature1[i] = Feature2[i];
                                            }
                                        }
                                        //lb_Message.Text = "获取指纹特征成功!";
                                    }
                                }
                                else
                                {
                                    registererror("Save image fail!");
                                    return;
                                }
                                registererror("Please take up finger...");
                                while (Hardware.Finger.MXOTDLL.MXIsFingerOn() == 1 && StaticRes.Global.Transaction_Continue)
                                {
                                    System.Threading.Thread.Sleep(10);
                                }
                                registererror("Please press finger...");
                            }
                            else
                            {
                                registererror("Get image fail!");
                                return;
                            }
                        }
                        else
                            System.Threading.Thread.Sleep(2000);
                    }
                }
                #endregion
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Fingerprinter Error -- " + ee.Message + " , user:" + StaticRes.Global.Current_User.USER_ID);
                registererror(ee.Message+ " From fingerprint Exception");
                return;
            }
        }

        void AscToHex(byte[] lpAscData, int nHexLength, byte[] lpHexData)//合
        {
            for (int i = 0; i < nHexLength; i++)
            {
                lpHexData[i] = (byte)(((int)GetHex(lpAscData[2 * i]) << 4) + (int)GetHex(lpAscData[2 * i + 1]));
            }
        }

        byte GetAsc(byte b)
        {
            int n;
            if ((int)b < 10)
            {

                n = (int)b + 48;
                return (byte)n;
            }
            n = (int)b - 10 + 65;
            return (byte)n;

            /*if (b < 10)
                return b + '0';
            return b - 10 + 'A';//*/
        }    

        void HexToAsc(byte[] lpHexData, int nHexLength, byte[] lpAscData)//拆
        {
            for (int i = 0; i < nHexLength; i++)
            {
                lpAscData[2 * i] = GetAsc((byte)(lpHexData[i] >> 4));
                lpAscData[2 * i + 1] = GetAsc((byte)(lpHexData[i] & 0xF));
            }

            //lpAscData[2 * nHexLength] = (byte)0;//最后一位结束符\0,要溢出就不要了
        }

        byte GetHex(byte c)
        {
            if ((int)c <= 57)
                return (byte)((int)c - 48);
            if ((int)c < 'a')
                return (byte)((int)c - 65 + 10);
            return (byte)((int)c - 97 + 10);

            /*if (c <= '9')
                return c - '0';
            if (c < 'a')
                return c - 'A' + 10;
            return c - 'a' + 10;//*/
        }

        public void LoadingHandle()
        {
            try
            {
                HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                this.Thread_Register.Abort();
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Load error when stop -- " + ee.Message + " , user:" + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Need_Homing = true;
            }
        }
    }
}
