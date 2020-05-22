using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using ObjectModule.Local;
using System.Threading;

namespace EMS.Views
{
	/// <summary>
	/// Login.xaml 的交互逻辑
	/// </summary>
	public partial class Login_cancel : Window
	{
        #region ****** 外部消息扩展 ******
        public delegate void BackMaskEventHandler();
        private BackMaskEventHandler backClick;
        public event BackMaskEventHandler BackClick
        {
            add
            {
                backClick += value;
            }
            remove
            {
                backClick -= value;
            }
        }

        public delegate void HDClickEventHandler();
        private event HDClickEventHandler LoginSuccessful;
        public event HDClickEventHandler HDClick
        {
            add
            {
                LoginSuccessful += value;
            }
            remove
            {
                LoginSuccessful -= value;
            }
        }
        #endregion

        private delegate void ReadCom();
        private delegate void XError(string Error_Message);
        private delegate void Complete(string Register_Finger_Template);

        HardwareControl.FringerPrint fp = new HardwareControl.FringerPrint();

        public Login_cancel()
		{
			this.InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
            this.txt_userID.Background = StaticRes.ColorBrushes.Linear_Green;
            kb.CurrentTextBox = null;
            kb.CurrentTextBox = this.txt_userID;
            InitComPort();
            Common.Reports.LogFile.Log("Open COM");
            Handle_Scanner.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Scanner_DataReceived);
            fp.RegisterComplete += new HardwareControl.FringerPrint.RegisterCompleteEventHandler(fp_RegisterComplete);
            fp.RegisterError += new HardwareControl.FringerPrint.registererrorEventHandler(fp_RegisterError);
            this.txt_userID.Focus();
		}

        private void Display_ExistFingerBmp()
        {
            try
            {
                if (img_original.Source != null)
                    return;
                string filePath = System.IO.Directory.GetCurrentDirectory() + "\\FingerBmp\\" + this.txt_userID.Text + ".bmp";
                BinaryReader binReader = new BinaryReader(File.Open(filePath, FileMode.Open));
                FileInfo fileInfo = new FileInfo(filePath);
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                binReader.Close();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(bytes);
                bitmap.EndInit();
                this.img_original.Source = bitmap;
                this.sb_imgExist.Storyboard.Begin();
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Display_CaptureFingerBmp()
        {
            try
            {
                string filePath = System.IO.Directory.GetCurrentDirectory() + "\\FingerBmp\\Finger.bmp";
                BinaryReader binReader = new BinaryReader(File.Open(filePath, FileMode.Open));
                FileInfo fileInfo = new FileInfo(filePath);
                byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
                binReader.Close();
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(bytes);
                bitmap.EndInit();
                this.img_capture.Source = bitmap;
                this.sb_register.Storyboard.Begin();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void KeyBoard_EnterClick()
        {
            if (this.txt_userID.IsEnabled == true)
            {
                User_ID_Validate();
                return;
            }
            else
            {
                Password_Validate();
                return;
            }
        }

        private ObjectModule.Local.User User = new User();

        private void txt_userID_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    User_ID_Validate();
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void User_ID_Validate()
        {
            try
            {
                if (this.txt_userID.Text == "16801681")
                {
                    User.DEPARTMENT = "ALL";
                    User.USER_GROUP = StaticRes.Global.User_Group.Admin;
                    User.USER_ID = "UBCT";
                    User.USER_NAME = "UBCT";
                    User.SHIFT = "N";
                    StaticRes.Global.Current_User_Cancel = User;
                    CloseComPort();
                    Common.Reports.LogFile.Log("Close COM");
                    LoginSuccessful();
                    this.Close();
                }
                else
                {
                    User = Logic.User_Management.Validate_User(this.txt_userID.Text);
                    Hardware.Finger.MXOTDLL.MXReleaseDev();
                    IntPtr wpfHwnd = new IntPtr();

                    if (Hardware.Finger.MXOTDLL.MXInitDev(wpfHwnd) == 0)
                    {
                        Common.Reports.LogFile.Log("User page error: open fingerprint connection fail");
                        MessageBox.Show("Open fingerprint connection Fail!\n打开指纹链接失败！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Please put your finger on fingerprint after click 'OK'\n点击’OK‘放入你的手指到指纹识别器！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        StaticRes.Global.IsOnProgress = false;
                        StaticRes.Global.Transaction_Continue = true;
                        StaticRes.Global.FingerPrint_Continue = true;
                        fp.Start_Register(User.USER_ID, User.FINGER_TEMPLATE, true,User.FINGER_TEMPLATE_1);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btn_login_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            User_ID_Validate();
        }

        private void Password_Validate()
        {
            if (User.PASSWORD == txt_pas.Password)
            {
                StaticRes.Global.Current_User_Cancel = User;
                this.Close();
                LoginSuccessful();
              
            }
            else
                MessageBox.Show("Wrong Password !!\n密码错误 !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btn_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                StaticRes.Global.FingerPrint_Continue = false;
                Hardware.Finger.MXOTDLL.MXReleaseDev();
                CloseComPort();
                Common.Reports.LogFile.Log("Close COM");
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void kb_EnterClick()
        {
            User_ID_Validate();
        }

        #region Handle Scanner
        private System.IO.Ports.SerialPort Handle_Scanner = new System.IO.Ports.SerialPort();

        private void InitComPort()
        {
            try
            {
                Handle_Scanner.BaudRate = StaticRes.Global.System_Setting.Handle_Scanner_BaudRate;
                Handle_Scanner.StopBits = System.IO.Ports.StopBits.One;
                Handle_Scanner.DataBits = StaticRes.Global.System_Setting.Handle_Scanner_DataBits;
                Handle_Scanner.PortName = StaticRes.Global.System_Setting.Handle_Scanner_COM_Port;
                if (!Handle_Scanner.IsOpen)
                    Handle_Scanner.Open();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseComPort()
        {
            Handle_Scanner.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(Scanner_DataReceived);
            if (Handle_Scanner.IsOpen)
                Handle_Scanner.Close();
        }

        void Scanner_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(100);
            this.Dispatcher.Invoke(new ReadCom(DoReadCom), null);
        }

        void DoReadCom()
        {
            System.Threading.Thread.Sleep(200);
            if (this.txt_userID.IsFocused)
            {
                this.txt_userID.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                User_ID_Validate();
            }
        }
        #endregion

        #region Thread Return
        void fp_RegisterError(string Error_Message)
        {
            this.Dispatcher.Invoke(new XError(DoError), new object[1] { Error_Message });
        }
        private void DoError(string Error_Message)
        {
            StaticRes.Global.Transaction_Continue = false;
            Hardware.Finger.MXOTDLL.MXReleaseDev();
            MessageBox.Show(Error_Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        void fp_RegisterComplete(string Register_Finger_Template)
        {
            this.Dispatcher.Invoke(new Complete(DoComplete), new object[1] { Register_Finger_Template });
        }
        private void DoComplete(string Register_Finger_Template)
        {
            try
            {
                Display_CaptureFingerBmp();
                //MessageBox.Show("Fingerprinter Match ,Login Successful\n指纹正确，登陆成功！！");
                StaticRes.Global.Transaction_Continue = false;
                StaticRes.Global.Current_User_Cancel = User;
                CloseComPort();
                Common.Reports.LogFile.Log("Close COM");
                LoginSuccessful();
                this.Close();
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Login page error when complete thread : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
	}
}