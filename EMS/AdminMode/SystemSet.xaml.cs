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

namespace EMS.AdminMode
{
    /// <summary>
    /// SapDB.xaml 的交互逻辑
    /// </summary>
    public partial class SystemSet : System.Windows.Controls.UserControl
    {
        public SystemSet()
        {
            InitializeComponent();
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Configure_Search();
        }

        void Configure_Search()
        {
            try
            {
                dg_list.ItemsSource = Logic.Common.Configure_Search().DefaultView;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
		private void btn_update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (dg_list.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select one record first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                this.MaskActionPage.Visibility = Visibility.Visible;
                this.txt_category.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["CATEGORY"].ToString();
                this.txt_defaultValue.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["DEFAULT_VALUE"].ToString();
                this.txt_name.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["NAME"].ToString();
                this.txt_unit.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["UNIT"].ToString();
                this.txt_value.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["VALUE"].ToString();
                this.txt_category.IsEnabled = false;
                this.txt_defaultValue.IsEnabled = false;
                this.txt_unit.IsEnabled = false;
                this.txt_name.IsEnabled = false;
                this.txt_value.Focus();
                kb.CurrentTextBox = txt_value;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void SystemSet_Match(string Parameter_Name, string Value)
        {
            switch (Parameter_Name)
            {
                case "Print_Label_Before_Load":
                    StaticRes.Global.System_Setting.Print_Label_Before_Load = Value;
                    break;
                case "Print_Label_After_Unload":
                    StaticRes.Global.System_Setting.Print_Label_After_Unload = Value;
                    break;
                case "Weighing_Scale_COM_Port":
                    StaticRes.Global.System_Setting.Weighing_Scale_COM_Port = Value;
                    break;
                case "Weighing_Scale_BaudRate":
                    StaticRes.Global.System_Setting.Weighing_Scale_BaudRate = int.Parse(Value);
                    break;
                case "Weighing_Scale_DataBits":
                    StaticRes.Global.System_Setting.Weighing_Scale_DataBits = int.Parse(Value);
                    break;
                case "Weighing_Scale_ReceivedBytesThreshold":
                    StaticRes.Global.System_Setting.Weighing_Scale_ReceivedBytesThreshold = int.Parse(Value);
                    break;
                case "Printer_COM_Port":
                    StaticRes.Global.System_Setting.Printer_COM_Port = Value;
                    break;
                case "Printer_BaudRate":
                    StaticRes.Global.System_Setting.Printer_BaudRate = int.Parse(Value);
                    break;
                case "Printer_DataBits":
                    StaticRes.Global.System_Setting.Printer_DataBits = int.Parse(Value);
                    break;
                case "Printer_ReceivedBytesThreshold":
                    StaticRes.Global.System_Setting.Printer_ReceivedBytesThreshold = int.Parse(Value);
                    break;
                case "Handle_Scanner_COM_Port":
                    StaticRes.Global.System_Setting.Handle_Scanner_COM_Port = Value;
                    break;
                case "Handle_Scanner_BaudRate":
                    StaticRes.Global.System_Setting.Handle_Scanner_BaudRate = int.Parse(Value);
                    break;
                case "Handle_Scanner_DataBits":
                    StaticRes.Global.System_Setting.Handle_Scanner_DataBits = int.Parse(Value);
                    break;
                case "Handle_Scanner_ReceivedBytesThreshold":
                    StaticRes.Global.System_Setting.Handle_Scanner_ReceivedBytesThreshold = int.Parse(Value);
                    break;
                case "Idle_Time_To_Exit":
                    StaticRes.Global.System_Setting.Idle_Time_To_Exit = int.Parse(Value);
                    break;
                case "Online_Mode":
                    StaticRes.Global.System_Setting.Online_Mode = Value;
                    break;
                case "Allow_Manual_KeyIn":
                    StaticRes.Global.System_Setting.Allow_Manual_KeyIn = Value;
                    break;
                case "Storage_Pre_Set_Qty":
                    StaticRes.Global.System_Setting.Storage_Pre_Set_Qty = int.Parse(Value);
                    break;
                case "One_Cycle_Pulse":
                    StaticRes.Global.System_Setting.One_Cycle_Pulse = int.Parse(Value);
                    break;
                case "Rotary_Home_Low_Velocity":
                    StaticRes.Global.System_Setting.Rotary_Home_Low_Velocity = int.Parse(Value);
                    break;
                case "Rotary_Home_High_Velocity":
                    StaticRes.Global.System_Setting.Rotary_Home_High_Velocity = int.Parse(Value);
                    break;
                case "Rotary_Home_Acc":
                    StaticRes.Global.System_Setting.Rotary_Home_Acc = int.Parse(Value);
                    break;
                case "Rotary_Home_Dcc":
                    StaticRes.Global.System_Setting.Rotary_Home_Dcc = int.Parse(Value);
                    break;
                case "Rotary_Home_Jerk":
                     StaticRes.Global.System_Setting.Rotary_Home_Jerk = int.Parse(Value);
                    break;
                case "Rotary_Move_Low_Velocity":
                    {
                        StaticRes.Global.System_Setting.Rotary_Move_Low_Velocity = int.Parse(Value);
                        HardwareControl.Motion_Control.Save_Motion_Config();
                    }
                    break;
                case "Rotary_Move_High_Velocity":
                    {
                        StaticRes.Global.System_Setting.Rotary_Move_High_Velocity = int.Parse(Value);
                        HardwareControl.Motion_Control.Save_Motion_Config();
                    }
                    break;
                case "Rotary_Move_Acc":
                    {
                        StaticRes.Global.System_Setting.Rotary_Move_Acc = int.Parse(Value);
                        HardwareControl.Motion_Control.Save_Motion_Config();
                    }
                    break;
                case "Rotary_Move_Dcc":
                    {
                        StaticRes.Global.System_Setting.Rotary_Move_Dcc = int.Parse(Value);
                        HardwareControl.Motion_Control.Save_Motion_Config();
                    }
                    break;
                case "Rotary_Move_Jerk":
                    {
                        StaticRes.Global.System_Setting.Rotary_Move_Jerk = int.Parse(Value);
                        HardwareControl.Motion_Control.Save_Motion_Config();
                    }
                    break;
                case "Rotary_Homing_Pitch":
                    StaticRes.Global.System_Setting.Rotary_Homing_Pitch = int.Parse(Value);
                    break;
                case "Index_1_Capacity":
                    {
                        if (int.Parse(Value) != 10 || int.Parse(Value) != 30)
                        {
                            MessageBox.Show("Capacity setting only 10 and 30 !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        DataTable dt_material = DataProvider.Local.Binning.Select.Mateiral_in_Index(1);
                        if (dt_material.Rows.Count > 0)
                        {
                            MessageBox.Show("This index still have material now , please clear it before update !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        StaticRes.Global.System_Setting.Index_1_Capacity = int.Parse(Value);
                        DataProvider.Local.Binning.Update_Capacity(1, int.Parse(Value));
                    }
                    break;
                case "Index_2_Capacity":
                    {
                        if (int.Parse(Value) != 10 || int.Parse(Value) != 30)
                        {
                            MessageBox.Show("Capacity setting only 10 and 30 !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        DataTable dt_material = DataProvider.Local.Binning.Select.Mateiral_in_Index(2);
                        if (dt_material.Rows.Count > 0)
                        {
                            MessageBox.Show("This index still have material now , please clear it before update !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        StaticRes.Global.System_Setting.Index_2_Capacity = int.Parse(Value);
                        DataProvider.Local.Binning.Update_Capacity(2, int.Parse(Value));
                    }
                    break;
                case "Index_3_Capacity":
                    {
                        if (int.Parse(Value) != 10 || int.Parse(Value) != 30)
                        {
                            MessageBox.Show("Capacity setting only 10 and 30 !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        DataTable dt_material = DataProvider.Local.Binning.Select.Mateiral_in_Index(3);
                        if (dt_material.Rows.Count > 0)
                        {
                            MessageBox.Show("This index still have material now , please clear it before update !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        StaticRes.Global.System_Setting.Index_3_Capacity = int.Parse(Value);
                        DataProvider.Local.Binning.Update_Capacity(3, int.Parse(Value));
                    }
                    break;
                case "Index_4_Capacity":
                    {
                        if (int.Parse(Value) != 10 || int.Parse(Value) != 30)
                        {
                            MessageBox.Show("Capacity setting only 10 and 30 !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        DataTable dt_material = DataProvider.Local.Binning.Select.Mateiral_in_Index(4);
                        if (dt_material.Rows.Count > 0)
                        {
                            MessageBox.Show("This index still have material now , please clear it before update !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        StaticRes.Global.System_Setting.Index_4_Capacity = int.Parse(Value);
                        DataProvider.Local.Binning.Update_Capacity(4, int.Parse(Value));
                    }
                    break;
                case "Bypass_Syringe_Top_Cover_Sensor":
                    StaticRes.Global.System_Setting.Bypass_Syringe_Top_Cover_Sensor = Value;
                    break;
                case "Bypass_Syringe_30cc_Cap_Present_Sensor":
                    StaticRes.Global.System_Setting.Bypass_Syringe_30cc_Cap_Present_Sensor = Value;
                    break;
                case "Bypass_Syringe_10cc_Cap_Present_Sensor":
                    StaticRes.Global.System_Setting.Bypass_Syringe_10cc_Cap_Present_Sensor = Value;
                    break;
                case "Bypass_Syringe_5cc_Cap_Present_Sensor":
                    StaticRes.Global.System_Setting.Bypass_Syringe_5cc_Cap_Present_Sensor = Value;
                    break;
                case "Time_1":
                    StaticRes.Global.System_Setting.Time_1 = int.Parse(Value);//yakun.zhou 2015.07.01
                    break;
                case "Reminder_time":
                    StaticRes.Global.System_Setting.Reminder_time = Value;//yakun.zhou 2015.07.01
                    break;
            }
        }

        private void btn_confirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ObjectModule.Local.Configure sc = new ObjectModule.Local.Configure();
                sc.CATEGORY = this.txt_category.Text;
                sc.DEFAULT_VALUE = this.txt_defaultValue.Text;
                sc.NAME = this.txt_name.Text;
                sc.UNIT = this.txt_unit.Text;
                sc.UPDATED_TIME = System.DateTime.Now;
                sc.USER_GROUP = StaticRes.Global.Current_User.USER_GROUP;
                sc.USER_ID = StaticRes.Global.Current_User.USER_ID;
                sc.VALUE = this.txt_value.Text;
                Logic.Common.Configure_Update(sc);
                SystemSet_Match(sc.NAME, sc.VALUE);
                Common.Reports.LogFile.Log("Update Configure Name: " + sc.NAME + " ; Value:" + sc.VALUE + " by user:" + StaticRes.Global.Current_User.USER_ID);
                MessageBox.Show("Update Successful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Configure_Search();
                this.MaskActionPage.Visibility = Visibility.Collapsed;
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Update system setting failed , error:" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MaskActionPage.Visibility = Visibility.Collapsed;
        }

        private void kb_EnterClick()
        {
            //// 在此处添加事件处理程序实现。
        }
    }
}
