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
using System.Windows.Threading;
using ObjectModule;


namespace EMS
{
    /// <summary>
    /// AlarmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmWindow : Window
    {
        public MainWindow xx;
        ObjectModule.Local.Event ge_global = new ObjectModule.Local.Event();
        DispatcherTimer tm = new DispatcherTimer();
        private bool buzzer_on = true;

        public AlarmWindow()
        {
            //InitializeComponent();
            //this.image.Source = new BitmapImage(new Uri(@"\Resources\Image\Error2.jpg", UriKind.Relative));
            //txt_error.Text = "Under testing.....";
            //txt_ts.Text = "Under testing too.....";
        }

        void Timer(object sender, EventArgs e)
        {
            if (buzzer_on)
            {
                Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off();
                buzzer_on = false;
                GC.Collect();
                return;
            }
            if(!buzzer_on)
            {
                Hardware.IO_LIST.Output.Y103_Tower_Buzzer_On();
                buzzer_on = true;
                GC.Collect();
                return;
            }
        }

        public AlarmWindow(ObjectModule.Local.Event ge)
        {
            InitializeComponent();
            try
            {
                tm.Tick += new EventHandler(Timer);
                tm.Interval = TimeSpan.FromSeconds(0.5);
                tm.Start();
                //***** Display Error information and picture
                ge_global = ge;
                txt_error.Text = StaticRes.Global.CurrentLanguageRes[ge.EVENT_NAME + "_Description"].ToString();
                txt_ts.Text = StaticRes.Global.CurrentLanguageRes[ge.EVENT_NAME + "_TroubleShooting"].ToString();
                ge.EVENT_NAME = ge.EVENT_NAME;
                ge.EVENT_TYPE = "Alarm";
                ge.EVENT_MESSAGE = txt_error.Text;
                ge.MONTH = System.DateTime.Now.Month;
                ge.WEEK = Logic.Common.weekofyear(System.DateTime.Now);
                ge.YEAR = System.DateTime.Now.Year;
                ge.UPDATED_TIME = System.DateTime.Now;
                ge.USER_ID = StaticRes.Global.Current_User.USER_ID;
                if (ge.USER_ID == null)
                    ge.USER_ID = "NA";
                ge.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                if (ge.DEPARTMENT == null)
                    ge.DEPARTMENT = "NA";
                this.image.Source = new BitmapImage(new Uri(@"\Resources\Image\" + ge.EVENT_NAME + ".jpg", UriKind.Relative));
                HardwareControl.IO_Control.Alarm_Tower_Light_Setting();
                Common.Reports.LogFile.Log("Error : " + txt_error.Text + " ; Process:" + ge.PROCESS_CODE + " ; Part_ID:" + ge.PART_ID + " ; User ID:" + ge.USER_ID);
                if (ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_top_cover_not_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_top_cover_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_5cc_cap_not_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_10cc_cap_not_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_30cc_cap_not_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_5cc_cap_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_10cc_cap_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_30cc_cap_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Emergency_stop &&
                     ge.EVENT_NAME != StaticRes.Global.Error_List.Please_homing_first &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Lower_air_pressure &&
                     ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_not_present &&
                    ge.EVENT_NAME != StaticRes.Global.Error_List.Syringe_present)
                {
                    if (ge.EVENT_NAME == StaticRes.Global.CommonError.Error_Name && StaticRes.Global.CommonError.Job == ge.PROCESS_CODE)
                    {
                        TimeSpan st = System.DateTime.Now - StaticRes.Global.CommonError.Error_Time;
                        if (st.TotalMinutes > 3)
                        {
                            DataProvider.Local.Event.Insert(ge);
                        }
                    }
                    else
                    {
                        DataProvider.Local.Event.Insert(ge);
                    }
                }
                StaticRes.Global.CommonError.Error_Name = ge.EVENT_NAME;
                StaticRes.Global.CommonError.Error_Time = System.DateTime.Now;
                StaticRes.Global.CommonError.Job = ge.PROCESS_CODE;
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }

        }

        private void btn_continuous_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                tm.Stop();
                Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off();
                //if (ge_global.EVENT_NAME == StaticRes.Global.Error_List.Spool_Missing)//if spool missing
                //{
                //    Views.Login_cancel x = new Views.Login_cancel();
                //    x.HDClick += new Views.Login_cancel.HDClickEventHandler(spool_missing);
                //    x.ShowDialog();
                //}
                //else
                //{
                    if (StaticRes.Global.Need_Homing)
                        HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                    else
                        HardwareControl.IO_Control.Green_Tower_Light_Setting();

                    this.DialogResult = true;
                    this.Close();
                //}
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Alarm page Error -- " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void spool_missing()
        {
            try
            {
                if (StaticRes.Global.Current_User_Cancel.USER_GROUP == StaticRes.Global.User_Group.Supervisor ||
                    StaticRes.Global.Current_User_Cancel.USER_GROUP == StaticRes.Global.User_Group.Engineer ||
                    StaticRes.Global.Current_User_Cancel.USER_GROUP == StaticRes.Global.User_Group.Admin)
                {
                    if (StaticRes.Global.Need_Homing)
                        HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                    else
                        HardwareControl.IO_Control.Green_Tower_Light_Setting();
                    this.DialogResult = true;
                    StaticRes.Global.Current_User_Cancel.USER_GROUP = string.Empty;
                    this.Close();
                }
                else
                    System.Windows.MessageBox.Show("You haven't access to continue for this alarm , please call Supervisor/Engineer/Admin to clear  !!");
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_terminal_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                tm.Stop();
                Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off();
                Views.Login_cancel x = new Views.Login_cancel();
                x.HDClick += new Views.Login_cancel.HDClickEventHandler(x_Close);
                x.ShowDialog();
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void x_Close()
        {
            try
            {
                if (StaticRes.Global.Current_User_Cancel.USER_ID.Length > 0)
                {
                    Common.Reports.LogFile.Log("Teminate error:" + txt_error.Text + " by user: " + StaticRes.Global.Current_User_Cancel.USER_ID);
                    HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                    this.DialogResult = false;
                    StaticRes.Global.Need_Homing = true;
                    StaticRes.Global.Process_Code.Homing = "H000";
                    StaticRes.Global.Process_Code.Homing = "H000";
                    StaticRes.Global.Process_Code.Loading = "L000";
                    StaticRes.Global.Process_Code.Unloading = "U000";
                    StaticRes.Global.Process_Code.Returning = "R000";
                    StaticRes.Global.Process_Code.Weighting = "W000";
                    this.Close();
                }
                else
                    System.Windows.MessageBox.Show("You haven't access to teminate alarm !!");
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_buzzerOff_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tm.Stop();
            Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off();
        }
    }
}
