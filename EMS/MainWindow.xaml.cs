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
using System.Windows.Threading;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Visifire.Charts;
using System.Data;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;

namespace EMS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static ResourceDictionary langRd = null;
        DispatcherTimer tm = new DispatcherTimer();
        DispatcherTimer tm_expiry = new DispatcherTimer();
        private delegate void ReadCom();
        private HardwareControl.Load load = new HardwareControl.Load();

        public MainWindow()
        {

            //防止多开
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                this.Close();
                return;
            }

            ColorValueInit();
            InitializeComponent();

            
            //this is a test


   

            StaticRes.Global.CurrentLanguage = "Chinese";
            langRd = System.Windows.Application.LoadComponent(new Uri(@"Language\" + StaticRes.Global.CurrentLanguage + ".xaml", UriKind.Relative)) as ResourceDictionary;
            Resources.MergedDictionaries.Add(langRd);
            StaticRes.Global.CurrentLanguageRes = langRd;

            txt_version.Text = StaticRes.Global.Version.GetInfo();
            Common.Reports.LogFile.Log("Start System," + txt_version.Text);


            this.pbMainwindowMixing.Visibility = Visibility.Collapsed;


            try
            {
                contentBar.Children.Clear();
                Report.Slot_Status x = new Report.Slot_Status();
                contentBar.Children.Add(x);
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Start System , Error:" + ee.Message);
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                tm_expiry.Tick += new EventHandler(Timer_Expiry);
                tm_expiry.Interval = TimeSpan.FromSeconds(300);
                tm_expiry.Start();
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Start System , Error:" + ee.Message);
            }
            try
            {         
                HardwareControl.Initial_Hardware.Connect_readParameter();
                if (StaticRes.Global.Hardware_Connection)
                {
                    tm.Tick += new EventHandler(Timer);
                    tm.Interval = TimeSpan.FromSeconds(0.5);
                    tm.Start();
                }
                HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
            }
            catch (Exception ee)
            {
                StaticRes.Global.Need_Homing = false;
                Common.Reports.LogFile.Log("Start System , Error:" + ee.Message);
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ColorValueInit()
        {
            StaticRes.ColorBrushes.Linear_Blue = (LinearGradientBrush)FindResource("Linear_Blue");
            StaticRes.ColorBrushes.Linear_BlackSilver = (LinearGradientBrush)FindResource("Linear_BlackSilver");
            StaticRes.ColorBrushes.Linear_Silver = (LinearGradientBrush)FindResource("Linear_Silver");
            StaticRes.ColorBrushes.Linear_Yellow = (LinearGradientBrush)FindResource("Linear_Yellow");
            StaticRes.ColorBrushes.Linear_Red = (LinearGradientBrush)FindResource("Linear_Red");
            StaticRes.ColorBrushes.Linear_RainySky = (LinearGradientBrush)FindResource("Linear_RainySky");
            StaticRes.ColorBrushes.Linear_Green = (LinearGradientBrush)FindResource("Linear_Green");
            StaticRes.ColorBrushes.ButtonNormalBackground = (LinearGradientBrush)FindResource("ButtonNormalBackground");
        }
        void Timer(object sender, EventArgs e)
        {
            try
            {
                //同步process bar 
                EMS.Transaction.Mix mixPage = Singleton.MixSingleton.GetInstance;

                if (mixPage.onGoing && mixPage.Visibility == Visibility.Hidden)
                    this.pbMainwindowMixing.Visibility = Visibility.Visible;
                else
                    this.pbMainwindowMixing.Visibility = Visibility.Collapsed;


                this.pbMainwindowMixing.Value = mixPage.runningTime / mixPage.mixTime * 100;




                txt_clock.Text = System.DateTime.Now.ToString();
                string Event_Name = string.Empty;
                if (HardwareControl.IO_Control.X001_Emergency_Stop_and_Check(ref Event_Name))
                {
                    ObjectModule.Local.Event ge = new ObjectModule.Local.Event();
                    ge.EVENT_TYPE = StaticRes.Global.Event_Type.Alarm;
                    ge.PROCESS_CODE = "Main Page";
                    ge.EVENT_NAME = Event_Name;
                    ge.USER_ID = "NA";
                    ge.SLOT_NO = "NA";
                    ge.PART_ID = "NA";
                    ge.DEPARTMENT = "NA";
                    new EMS.AlarmWindow(ge).ShowDialog();
                }


               
                

            }
            catch
            {
            }
        }

        void Timer_Expiry(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Logic.Transaction.Expiry_List();
                if (dt.Rows.Count > 0)
                {
                    ObjectModule.Local.Tracking track = new ObjectModule.Local.Tracking(dt.Rows[0]);
                    new EMS.ExpiryWindow(track).ShowDialog();
                }
            }
            catch
            {
            }
        }

        #region UI Logic
        private void btn_load_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {       
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_LoadPage);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
                StaticRes.Global.Homing_Complete = false;

                #region Rotary_Continue_Move
                DataTable dt = DataProvider.Local.Binning.Select.Bin_WithMateria();
                if (dt.Rows.Count > 0)
                {
                    HardwareControl.Motion_Control.Rotary_Continue_Move();
                }
                #endregion
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_LoadPage()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin
                    || StaticRes.Global.Current_User.USER_GROUP==StaticRes.Global.User_Group.MH)
                {
                    Common.Reports.LogFile.Log("Enter homing page , user: " + StaticRes.Global.Current_User.USER_ID);
                    HomingPage xx = new HomingPage(true);
                    xx.BackClick += new HomingPage.BackMaskEventHandler(CloseMask);
                    xx.HDClick += new HomingPage.HDClickEventHandler(ShowReport); 
                    xx.ShowDialog();
                    if (StaticRes.Global.Homing_Complete)
                    {
                        Transaction.Load x = new Transaction.Load();
                        x.BackClick += new Transaction.Load.BackMaskEventHandler(CloseMask);
                        x.HDClick += new Transaction.Load.HDClickEventHandler(ShowReport);
                        Common.Reports.LogFile.Log("Entry load page , user: " + StaticRes.Global.Current_User.USER_ID);
                        x.ShowDialog();
                    }
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n你没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_remove_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {                
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_RemovePage);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
                StaticRes.Global.Homing_Complete = false;
                #region Rotary_Continue_Move
                DataTable dt = DataProvider.Local.Binning.Select.Bin_WithMateria();
                if (dt.Rows.Count > 0)
                {
                    HardwareControl.Motion_Control.Rotary_Continue_Move();
                }
                #endregion
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_RemovePage()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Engineer)
                {
                    Common.Reports.LogFile.Log("Enter homing page , user: " + StaticRes.Global.Current_User.USER_ID);
                    HomingPage xx = new HomingPage(true);
                    xx.BackClick += new HomingPage.BackMaskEventHandler(CloseMask);
                    xx.HDClick += new HomingPage.HDClickEventHandler(ShowReport); 
                    xx.ShowDialog();
                    if (StaticRes.Global.Homing_Complete)
                    {
                        Transaction.Remove x = new Transaction.Remove();
                        x.BackClick += new Transaction.Remove.BackMaskEventHandler(CloseMask);
                        x.HDClick += new Transaction.Remove.HDClickEventHandler(ShowReport);
                        Common.Reports.LogFile.Log("Entry remove page , user: " + StaticRes.Global.Current_User.USER_ID);
                        x.ShowDialog();
                    }
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n你没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_unload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {               
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_UnloadPage);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
                StaticRes.Global.Homing_Complete = false;
                #region Rotary_Continue_Move
                DataTable dt = DataProvider.Local.Binning.Select.Bin_WithMateria();
                if (dt.Rows.Count > 0)
                {
                    HardwareControl.Motion_Control.Rotary_Continue_Move();
                }
                #endregion
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_UnloadPage()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.OP
                    ||StaticRes.Global.Current_User.USER_GROUP==StaticRes.Global.User_Group.Supervisor
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Engineer)
                {
                    Common.Reports.LogFile.Log("Enter homing page , user: " + StaticRes.Global.Current_User.USER_ID);
                    HomingPage xx = new HomingPage(true);
                    xx.BackClick += new HomingPage.BackMaskEventHandler(CloseMask);
                    xx.HDClick += new HomingPage.HDClickEventHandler(ShowReport); 
                    xx.ShowDialog();
                    if (StaticRes.Global.Homing_Complete)
                    {
                        Transaction.Unload x = new Transaction.Unload();
                        x.BackClick += new Transaction.Unload.BackMaskEventHandler(CloseMask);
                        x.HDClick += new Transaction.Unload.HDClickEventHandler(ShowReport);
                        Common.Reports.LogFile.Log("Entry unload page , user: " + StaticRes.Global.Current_User.USER_ID);
                        x.ShowDialog();
                    }
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n你没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_return_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_ReturnPage);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
                StaticRes.Global.Homing_Complete = false;
                #region Rotary_Continue_Move
                DataTable dt = DataProvider.Local.Binning.Select.Bin_WithMateria();
                if (dt.Rows.Count > 0)
                {
                    HardwareControl.Motion_Control.Rotary_Continue_Move();
                }
                #endregion
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_ReturnPage()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.OP
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Supervisor
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Engineer)
                {
                    Common.Reports.LogFile.Log("Enter homing page , user: " + StaticRes.Global.Current_User.USER_ID);
                    HomingPage xx = new HomingPage(true);
                    xx.BackClick += new HomingPage.BackMaskEventHandler(CloseMask);
                    xx.HDClick += new HomingPage.HDClickEventHandler(ShowReport);
                    xx.ShowDialog();
                    if (StaticRes.Global.Homing_Complete)
                    {
                        Transaction.Return x = new Transaction.Return();
                        x.BackClick += new Transaction.Return.BackMaskEventHandler(CloseMask);
                        x.HDClick += new Transaction.Return.HDClickEventHandler(ShowReport);
                        Common.Reports.LogFile.Log("Entry return page , user: " + StaticRes.Global.Current_User.USER_ID);
                        x.ShowDialog();
                    }
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n你没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_adminMode_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_AdminPage);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_AdminPage()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin)
                {
                    Common.Reports.LogFile.Log("Entry admin mode page , user: " + StaticRes.Global.Current_User.USER_ID);
                    AdminMode.AdminPage x = new AdminMode.AdminPage();
                    x.BackClick += new AdminMode.AdminPage.BackMaskEventHandler(CloseMask);
                    x.HDClick+=new AdminMode.AdminPage.HDClickEventHandler(ShowReport);
                    x.ShowDialog();
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n你没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_maintMode_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_MaintPage);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_MaintPage()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Maintenance
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin)
                {
                    Common.Reports.LogFile.Log("Entry maintenance mode page , user: " + StaticRes.Global.Current_User.USER_ID);
                    MaintMode.MaintPage x = new MaintMode.MaintPage();
                    x.BackClick += new MaintMode.MaintPage.BackMaskEventHandler(CloseMask);
                    x.HDClick+=new MaintMode.MaintPage.HDClickEventHandler(ShowReport);
                    x.ShowDialog();
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n您没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_engineerMode_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_EngineerPage);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_EngineerPage()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Engineer
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Supervisor
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin)
                {
                    Common.Reports.LogFile.Log("Entry engineer mode page , user: " + StaticRes.Global.Current_User.USER_ID);
                    EngineerMode.EngineerPage x = new EngineerMode.EngineerPage();
                    x.BackClick += new EngineerMode.EngineerPage.BackMaskEventHandler(CloseMask);
                    x.HDClick+=new EngineerMode.EngineerPage.HDClickEventHandler(ShowReport);
                    x.ShowDialog();
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n您没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_inventory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                contentBar.Children.Clear();
                Report.Slot_Status x = new Report.Slot_Status();
                contentBar.Children.Add(x);
            }
            catch(Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_homing_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Common.Reports.LogFile.Log("Enter homing page , user: " + StaticRes.Global.Current_User.USER_ID);
                HomingPage x = new HomingPage(false);
                x.BackClick += new HomingPage.BackMaskEventHandler(CloseMask);
                x.HDClick += new HomingPage.HDClickEventHandler(ShowReport);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void x_homing()
        {
        }

        private void btn_exit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Views.Login x = new Views.Login();
                x.HDClick += new Views.Login.HDClickEventHandler(x_Close);
                x.BackClick += new Views.Login.BackMaskEventHandler(CloseMask);
                MaskActionPage.Visibility = Visibility.Visible;
                x.ShowDialog();
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }
        void x_Close()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_ID.Length > 0)
                {
                    Common.Reports.LogFile.Log("Exit system , user: " + StaticRes.Global.Current_User.USER_ID);
                    tm.Stop();
                    if (StaticRes.Global.Hardware_Connection)
                        HardwareControl.Initial_Hardware.Close_Connect();
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n您没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void CloseMask()
        {
            MaskActionPage.Visibility = Visibility.Collapsed;
        }

        public void ShowReport()
        {
            contentBar.Children.Clear();
            Report.Slot_Status x = new Report.Slot_Status();
            contentBar.Children.Add(x);
        }

        private void btn_report_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                contentBar.Children.Clear();
                Report.Usage x = new Report.Usage();
                contentBar.Children.Add(x);
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void x_scrapClean()
        {
            try
            {
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin
                    || StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.MH)
                {
                    Common.Reports.LogFile.Log("Entry scrap limit setting page , user: " + StaticRes.Global.Current_User.USER_ID);
                    Transaction.ScrapClean x = new Transaction.ScrapClean();
                    x.BackClick += new Transaction.ScrapClean.BackMaskEventHandler(CloseMask);
                    x.HDClick+=new Transaction.ScrapClean.HDClickEventHandler(ShowReport);
                    x.ShowDialog();
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n您没有访问权限！！");
                    CloseMask();
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void btn_logFile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                contentBar.Children.Clear();
                Report.logFile x = new Report.logFile();
                contentBar.Children.Add(x);
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btn_mix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Transaction.Mix mixPage = EMS.Singleton.MixSingleton.GetInstance;
                

                //正在运行的话, 直接显示窗口, 否则重新登入
                if (mixPage.onGoing)
                {
                    mixPage.ShowWindow();
                }
                else
                {
                    Views.Login x = new Views.Login();
                    x.HDClick += new Views.Login.HDClickEventHandler(x_MixPage);
                    x.ShowDialog();
                }

            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message);
            }
        }



        void x_MixPage()
        {
            try
            {
                //admin, engineer, supervisor,op权限均可登入.
                if (StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Admin ||
                    StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Engineer ||
                    StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.Supervisor ||
                    StaticRes.Global.Current_User.USER_GROUP == StaticRes.Global.User_Group.OP)
                {
                    Transaction.Mix x = EMS.Singleton.MixSingleton.GetInstance;
                    
                    x.userID = StaticRes.Global.Current_User.USER_ID;
                    x.userName = StaticRes.Global.Current_User.USER_NAME;
                    x.userGroup = StaticRes.Global.Current_User.USER_GROUP;
                    x.department = StaticRes.Global.Current_User.DEPARTMENT;
                    
                    x.ShowWindow();
                }
                else if (StaticRes.Global.Current_User.USER_GROUP.Length > 0)
                {
                    System.Windows.MessageBox.Show("You haven't access to entey it !!\n你没有访问权限！！");
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }


        private void cbb_language_DropDownClosed(object sender, EventArgs e)
        {
            string strLanguage = ((System.Windows.Controls.ComboBoxItem)cbb_language.SelectedItem).Content.ToString();


            if (strLanguage == "English")
                StaticRes.Global.CurrentLanguage = "English";
            if (strLanguage == "Chinese(中文)")
                StaticRes.Global.CurrentLanguage = "Chinese";



            langRd = System.Windows.Application.LoadComponent(
                                       new Uri(@"Language\" + StaticRes.Global.CurrentLanguage + ".xaml", UriKind.Relative))
                           as ResourceDictionary;
            Resources.MergedDictionaries.Add(langRd);
            StaticRes.Global.CurrentLanguageRes = langRd;
        }
        #endregion

      

       


    }
}
