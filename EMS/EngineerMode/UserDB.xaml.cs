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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Threading;
using System.Windows.Interop;
using System.IO;
using ObjectModule.Local;

namespace EMS.EngineerMode
{
    /// <summary>
    /// UserDB.xaml 的交互逻辑
    /// </summary>
    public partial class UserDB : UserControl
    {
        ObjectModule.Local.User gu = new User();
        private delegate void XError(string Error_Message);
        private delegate void Complete(string Register_Finger_Template);
        private delegate void XError_1(string Error_Message);
        private delegate void Complete_1(string Register_Finger_Template);
        private string FINGER_TEMPLATE = string.Empty;
        private string FINGER_TEMPLATE_1 = string.Empty;//20150420yakun
        HardwareControl.FringerPrint fp = new HardwareControl.FringerPrint();
        HardwareControl.FringerPrint fp_1 = new HardwareControl.FringerPrint();

        public UserDB()
        {
            InitializeComponent();
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == "Admin")
                    this.cbb_userGroup.Items.Add("Admin");
                DataTable dt_department = Logic.Common.Department_Select();
                if (dt_department.Rows.Count > 0)
                {
                    foreach (DataRow a in dt_department.Rows)
                    {
                        cbb_department.Items.Add(a["DEPARTMENT"].ToString());
                    }
                }
                fp.RegisterComplete += new HardwareControl.FringerPrint.RegisterCompleteEventHandler(fp_RegisterComplete);
                fp.RegisterError += new HardwareControl.FringerPrint.registererrorEventHandler(fp_RegisterError);
                fp_1.RegisterComplete+=new HardwareControl.FringerPrint.RegisterCompleteEventHandler(fp_1_RegisterComplete);
                fp_1.RegisterError+=new HardwareControl.FringerPrint.registererrorEventHandler(fp_1_RegisterError);
                kb.CurrentTextBox = this.txt_userID_search;
            }
            catch (Exception ee)
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
                this.img_finger.Source = bitmap;
                this.sb_imgDisplay.Storyboard.Begin();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            User_Search();
        }

        void User_Search()
        {
            try
            {
                dg_list.ItemsSource = Logic.User_Management.User_Search(this.txt_userID_search.Text).DefaultView;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            img_finger.Resources.Clear();
            this.MaskActionPage.Visibility = Visibility.Visible;
            this.lb_title.Content = "Add User";
            this.txt_userID.Text = null;
            this.txt_userName.Text = null;
            this.cbb_userGroup.Text = null;
            this.cbb_department.Text = null;
            this.pb_password.Password = null;
            this.cbb_shift.Text = null;
            FINGER_TEMPLATE = string.Empty;
            FINGER_TEMPLATE_1 = string.Empty;
            this.txt_userID.IsEnabled = true;
            this.cbb_department.IsEnabled = true;
            this.txt_userID.Focus();
        }

        private void btn_update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                img_finger.Resources.Clear();
                if (dg_list.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select one user first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                this.MaskActionPage.Visibility = Visibility.Visible;
                this.lb_title.Content = "Update User";
                txt_userID.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["USER_ID"].ToString();
                txt_userName.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["USER_NAME"].ToString();
                pb_password.Password = ((DataRowView)dg_list.SelectedItems[0]).Row["PASSWORD"].ToString();
                cbb_department.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString();
                cbb_userGroup.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["USER_GROUP"].ToString();
                cbb_shift.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SHIFT"].ToString();
                FINGER_TEMPLATE = ((DataRowView)dg_list.SelectedItems[0]).Row["FINGER_TEMPLATE"].ToString();
                FINGER_TEMPLATE_1 = ((DataRowView)dg_list.SelectedItems[0]).Row["FINGER_TEMPLATE_1"].ToString();//20150420yakun
                this.txt_userID.IsEnabled = false;
                this.cbb_department.IsEnabled = false;
                this.txt_userName.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btn_Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this user ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (dg_list.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select one user first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
                    Logic.User_Management.User_Delete(((DataRowView)dg_list.SelectedItems[0]).Row["USER_ID"].ToString(), ((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString());
                    Common.Reports.LogFile.Log("Delete user : " + ((DataRowView)dg_list.SelectedItems[0]).Row["USER_ID"].ToString() + " by user:" + StaticRes.Global.Current_User.USER_ID);
                    MessageBox.Show("Delete Succesful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    User_Search();
                    kb.CurrentTextBox = txt_userID_search;
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_confirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                gu.DEPARTMENT = cbb_department.Text;
                gu.PASSWORD = pb_password.Password;
                gu.USER_GROUP = cbb_userGroup.Text;
                gu.USER_ID = txt_userID.Text;
                gu.USER_NAME = txt_userName.Text;
                gu.UPDATED_BY = StaticRes.Global.Current_User.USER_ID;
                gu.UPDATED_TIME = System.DateTime.Now;
                gu.SHIFT = cbb_shift.Text;
                //gu.FINGER_TEMPLATE = FINGER_TEMPLATE;
                //if (StaticRes.Global.Hardware_Connection)
                //{
                    try
                    {
                        if (gu.FINGER_TEMPLATE == null)
                        {
                            MessageBox.Show("Please register fingerprint first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Please register fingerprint first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
                    try
                    {
                        if (gu.FINGER_TEMPLATE_1 == null)
                        {
                            MessageBox.Show("Please register backup fingerprint first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            return;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Please register backup fingerprint first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
                //}
                ////else
                ////    gu.FINGER_TEMPLATE = "No Hardware";
                if (lb_title.Content.ToString() == "Add User")
                {
                    Logic.User_Management.User_Insert(gu);
                    Common.Reports.LogFile.Log("Register user : " + gu.USER_ID + " by user:" + StaticRes.Global.Current_User.USER_ID);
                }
                else
                {
                    Logic.User_Management.User_Update(gu);
                    Common.Reports.LogFile.Log("Update user : " + gu.USER_ID + " by user:" + StaticRes.Global.Current_User.USER_ID);
                }
                MessageBox.Show("Successful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                User_Search();
                this.img_finger.Source = null;
                this.MaskActionPage.Visibility = Visibility.Collapsed;
                kb.CurrentTextBox = txt_userID;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                StaticRes.Global.FingerPrint_Continue = false;
                Hardware.Finger.MXOTDLL.MXReleaseDev();
                this.MaskActionPage.Visibility = Visibility.Collapsed;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "txt_userID":
                    kb.CurrentTextBox = this.txt_userID;
                    break;
                case "txt_userID_search":
                    kb.CurrentTextBox = this.txt_userID_search;
                    break;
                case "txt_userName":
                    kb.CurrentTextBox = this.txt_userName;
                    break;
                case "pb_password":
                    kb.CurrentPasswordBox = this.pb_password;
                    break;
            }
        }

        private void txt_UserPassword_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            kb.CurrentTextBox = null;
            kb.CurrentPasswordBox = this.pb_password;
        }

        private void btn_register_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.img_finger.Source = null;
                if (this.txt_userID.Text.TrimEnd() == string.Empty)
                {
                    MessageBox.Show("Please key in user ID before register finger printer !!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Hardware.Finger.MXOTDLL.MXReleaseDev();

                IntPtr wpfHwnd = new IntPtr();

                if (Hardware.Finger.MXOTDLL.MXInitDev(wpfHwnd) == 0)
                {
                    MessageBox.Show("Open fingerprint connection Fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Please put your finger on fingerprint after click 'OK'", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    StaticRes.Global.FingerPrint_Continue = true;
                    fp.Start_Register(txt_userID.Text, string.Empty, false,string.Empty);
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void btn_register_1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.img_finger.Source = null;
                if (this.txt_userID.Text.TrimEnd() == string.Empty)
                {
                    MessageBox.Show("Please key in user ID before register finger printer !!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Hardware.Finger.MXOTDLL.MXReleaseDev();

                IntPtr wpfHwnd = new IntPtr();

                if (Hardware.Finger.MXOTDLL.MXInitDev(wpfHwnd) == 0)
                {
                    MessageBox.Show("Open fingerprint connection Fail!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    MessageBox.Show("Please put your finger on fingerprint after click 'OK'", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    StaticRes.Global.FingerPrint_Continue = true;
                    fp_1.Start_Register(txt_userID.Text, string.Empty, false,string.Empty);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void kb_EnterClick()
        {
        	// 在此处添加事件处理程序实现。
        }

        #region Thread Return
        void fp_RegisterError(string Error_Message)
        {
            this.Dispatcher.Invoke(new XError(DoError), new object[1] { Error_Message });
        }
        private void DoError(string Error_Message)
        {
            //StaticRes.Global.Transaction_Continue = false;
           // Hardware.Finger.MXOTDLL.MXReleaseDev();
            MessageBox.Show(Error_Message, "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
                //System.GC.Collect();
                MessageBox.Show("Register Fingerprint Successful !!");
                StaticRes.Global.Transaction_Continue = false;
                Hardware.Finger.MXOTDLL.MXReleaseDev();
                gu.FINGER_TEMPLATE = Register_Finger_Template;
                FINGER_TEMPLATE = string.Empty;
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Login page error when complete thread : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void fp_1_RegisterError(string Error_Message)
        {
            this.Dispatcher.Invoke(new XError(DoError_1), new object[1] { Error_Message });
        }
        private void DoError_1(string Error_Message)
        {
            //StaticRes.Global.Transaction_Continue = false;
            // Hardware.Finger.MXOTDLL.MXReleaseDev();
            MessageBox.Show(Error_Message, "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        void fp_1_RegisterComplete(string Register_Finger_Template)
        {
            this.Dispatcher.Invoke(new Complete(DoComplete_1), new object[1] { Register_Finger_Template });
        }
        private void DoComplete_1(string Register_Finger_Template)
        {
            try
            {
                Display_CaptureFingerBmp();
                //System.GC.Collect();
                MessageBox.Show("Register backup Fingerprint Successful !!");
                StaticRes.Global.Transaction_Continue = false;
                Hardware.Finger.MXOTDLL.MXReleaseDev();
                gu.FINGER_TEMPLATE_1 = Register_Finger_Template;
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
