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
using ObjectModule.Local;
using System.Data;
using System.Windows.Threading;

namespace EMS.Transaction
{
    /// <summary>
    /// Load.xaml 的交互逻辑
    /// </summary>
    public partial class Remove : Window
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
        private event HDClickEventHandler hdClick;
        public event HDClickEventHandler HDClick
        {
            add
            {
                hdClick += value;
            }
            remove
            {
                hdClick -= value;
            }
        }
        #endregion
        private delegate void XError(Event ee);
        private delegate void Complete(bool complete);
        private delegate void Step(string Event);

        HardwareControl.Unload unload = new HardwareControl.Unload();
        private string DESCRIPTION = string.Empty;
        private string BATCH_NO = string.Empty;
        private string START_WEIGHT = string.Empty;
        private string CURRENT_WEIGHT = string.Empty;
        private string THAWING_TIME = string.Empty;
        private string CAPACITY = string.Empty;
        private string STATUS = string.Empty;
        private string MF_EXPIRY_DATE = string.Empty;

        DispatcherTimer tm = new DispatcherTimer();
        private DateTime time_start = System.DateTime.Now;
        private bool Transaction_ongoing = false;

        public Remove()
        {
            InitializeComponent();
            try
            {
                Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
                unload.UnloadComplete += new HardwareControl.Unload.UnloadCompleteEventHandler(unload_UnloadComplete);
                unload.UnloadError += new HardwareControl.Unload.unloaderrorEventHandler(unload_UnloadError);
                unload.Step += new HardwareControl.Unload.StepEventHandler(unload_Step);

                this.txt_userID.Text = StaticRes.Global.Current_User.USER_ID;
                this.txt_userName.Text = StaticRes.Global.Current_User.USER_NAME;
                this.txt_userGroup.Text = StaticRes.Global.Current_User.USER_GROUP;
                this.txt_userDepartment.Text = StaticRes.Global.Current_User.DEPARTMENT;
                event_list.ItemsSource = new List<string>();
                this.dg_list.ItemsSource = Logic.Transaction.Remove.Bin_withMaterial().DefaultView;
                kb.CurrentTextBox = this.txt_mcID;
                this.txt_mcID.Background = StaticRes.ColorBrushes.Linear_Green;

                tm.Tick += new EventHandler(Timer);
                tm.Interval = TimeSpan.FromSeconds(1);
                tm.Start();

                txt_mcID.Focus();
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Remove page error when start : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Timer(object sender, EventArgs e)
        {
            try
            {
                DateTime time_now = System.DateTime.Now;
                TimeSpan ts = time_now - time_start;
                if (ts.TotalSeconds > StaticRes.Global.System_Setting.Idle_Time_To_Exit && Transaction_ongoing == false)
                {
                    Common.Reports.LogFile.Log("Auto exit remove page becuase idle time achieve setting  , user : " + StaticRes.Global.Current_User.USER_ID);
                    StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                    StaticRes.Global.Current_User.USER_ID = string.Empty;
                    hdClick();
                    backClick();
                    tm.Stop();
                    this.Close();
                }
                GC.Collect();
            }
            catch
            {
            }
        }

        #region UI Logic
        private void btn_close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Common.Reports.LogFile.Log("Exit remove mode  , user : " + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                StaticRes.Global.Current_User.USER_ID = string.Empty;
                StaticRes.Global.Transaction_Continue = false;
                if (StaticRes.Global.Process_Code.Unloading != "U000")
                    StaticRes.Global.Need_Homing = true;
                StaticRes.Global.Process_Code.Unloading = "U000";
                hdClick();
                backClick();
                tm.Stop();
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cb_bypassMCcheck_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cb_bypassMCcheck.IsChecked == true)
            {
                kb.CurrentTextBox = txt_remark;
                TextBox_Logic(txt_mcID, txt_remark);
            }
            else
            {
                kb.CurrentTextBox = txt_mcID;
                TextBox_Logic(txt_remark, txt_mcID);
            }
        } 

        private void dg_list_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView selectItem = this.dg_list.SelectedItem as DataRowView;
                if (selectItem == null)
                    return;
                this.txt_slotIndex.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SLOT_INDEX"].ToString();
                this.txt_slotID.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SLOT_ID"].ToString();
                this.txt_partID.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["PART_ID"].ToString();
                this.txt_sapcode.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SAPCODE"].ToString();
                this.txt_expiryTime.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["EXPIRY_DATETIME"].ToString();
                this.txt_readyTime.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["READY_DATETIME"].ToString();
                DESCRIPTION = ((DataRowView)dg_list.SelectedItems[0]).Row["DESCRIPTION"].ToString();
                BATCH_NO = ((DataRowView)dg_list.SelectedItems[0]).Row["BATCH_NO"].ToString();
                CAPACITY = ((DataRowView)dg_list.SelectedItems[0]).Row["CAPACITY"].ToString();
                START_WEIGHT = ((DataRowView)dg_list.SelectedItems[0]).Row["START_WEIGHT"].ToString();
                CURRENT_WEIGHT = ((DataRowView)dg_list.SelectedItems[0]).Row["CURRENT_WEIGHT"].ToString();
                THAWING_TIME = ((DataRowView)dg_list.SelectedItems[0]).Row["THAWING_DATETIME"].ToString();
                MF_EXPIRY_DATE = ((DataRowView)dg_list.SelectedItems[0]).Row["MF_EXPIRY_DATE"].ToString();
                STATUS = ((DataRowView)dg_list.SelectedItems[0]).Row["STATUS"].ToString();
                time_start = System.DateTime.Now;
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Remove page error when select record : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_mcID_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Machine_Validate();
            }
            else
                time_start = System.DateTime.Now;
        }

        void Machine_Validate()
        {
            try
            {
                if (this.txt_partID.Text.Length == 0)
                {
                    MessageBox.Show("Please select the material which you want !!\n请选择你所需要的材料！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                ObjectModule.Local.Equipment se = Logic.Transaction.Unload.Equip_Infor(this.txt_mcID.Text);
                TextBox_Logic(txt_mcID, txt_remark);
                time_start = System.DateTime.Now;
                kb.CurrentTextBox = txt_remark;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_remark_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Remark_Validate();
            }
            else
                time_start = System.DateTime.Now;
        }
        void Remark_Validate()
        {
            if (txt_remark.Text.Length == 0)
                MessageBox.Show("Please input remark !!\n请输入原因！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            time_start = System.DateTime.Now;
            TextBox_To_Btn(txt_remark, btn_unload);
        }

        private void btn_unload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                event_list.ItemsSource = new List<string>();
                int Slot_No = int.Parse(this.txt_slotID.Text);
                int Slot_Level = int.Parse(this.txt_slotIndex.Text);
                this.btn_close.IsEnabled = false;
                this.btn_stop.IsEnabled = true;
                this.btn_unload.IsEnabled = false;                
                time_start = System.DateTime.Now; //Meaning have action
                Transaction_ongoing = true;
                StaticRes.Global.IsOnProgress = false;
                StaticRes.Global.Transaction_Continue = true;
                Common.Reports.LogFile.Log("Start Remove , Part ID : " + txt_partID + " ; Sapcode : " + txt_sapcode.Text + " ; Batch No : " + BATCH_NO + " ; User ID : " + txt_userID);
                pb.Value = 0;               
                pb.Maximum = unload.Start_UnLoad(Slot_No, Slot_Level);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                StaticRes.Global.Transaction_Continue = false;
                time_start = System.DateTime.Now; //Meaning have action
                Transaction_ongoing = false; 
                this.btn_close.IsEnabled = true;
                this.btn_unload.IsEnabled = true;
                this.btn_stop.IsEnabled = false;
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Remove page error when click stop : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void kb_UndoClick()
        {
            
            this.txt_partID.Text = string.Empty;
            this.txt_sapcode.Text = string.Empty;
            this.txt_slotIndex.Text = string.Empty;
            this.txt_slotID.Text = string.Empty;
            this.txt_remark.Text = string.Empty;
            this.txt_mcID.Text = string.Empty;
            this.txt_readyTime.Text = string.Empty;
            this.txt_expiryTime.Text = string.Empty;
            this.btn_stop.IsEnabled = false;
            this.btn_unload.IsEnabled = false;
            this.btn_close.IsEnabled = true;
            this.txt_mcID.Background = System.Windows.Media.Brushes.White;
            this.txt_mcID.IsEnabled = true;
            this.txt_mcID.Background = StaticRes.ColorBrushes.Linear_Green;
            kb.CurrentTextBox = this.txt_mcID;
            time_start = System.DateTime.Now; //Meaning have action
            Transaction_ongoing = false; 
            this.txt_mcID.Focus();
        }

        private void kb_EnterClick()
        {
            if (this.txt_mcID.IsFocused)
            {
                Machine_Validate();
                return;
            }
            if (this.txt_remark.IsFocused)
            {
                Remark_Validate();
                return;
            }
        }

        private void TextBox_Logic(System.Windows.Controls.TextBox txt_from, System.Windows.Controls.TextBox txt_to)
        {
            txt_from.IsEnabled = false;
            txt_from.Background = System.Windows.Media.Brushes.White;
            txt_to.IsEnabled = true;
            txt_to.Background = StaticRes.ColorBrushes.Linear_Green;
            txt_to.Focus();
        }

        private void TextBox_To_Btn(System.Windows.Controls.TextBox txt_from, System.Windows.Controls.Button btn_to)
        {
            txt_from.IsEnabled = false;
            txt_from.Background = System.Windows.Media.Brushes.White;
            btn_to.IsEnabled = true;
            btn_to.Focus();
        }
        #endregion

        #region Thread Return
        void unload_UnloadComplete(bool complete)
        {
            this.Dispatcher.Invoke(new Complete(DoComplete), new object[1] {complete });
        }
        private void DoComplete(bool complete)
        {
            try
            {
                if (complete)
                {
                    #region Update Server Database
                    while (!Logic.Transaction.Remove.Local_Update(txt_partID.Text,txt_slotID.Text,txt_slotIndex.Text,txt_mcID.Text,STATUS,START_WEIGHT,
                        CURRENT_WEIGHT,txt_sapcode.Text,THAWING_TIME,txt_readyTime.Text,txt_expiryTime.Text,DESCRIPTION,BATCH_NO,CAPACITY,txt_remark.Text))
                    {
                        Event ge = new Event();
                        ge.EVENT_NAME = StaticRes.Global.Error_List.Update_database_failed;
                        ge.PROCESS_CODE = "Update DB";
                        ge.SLOT_NO = "S" + txt_slotID.Text + "L" + txt_slotIndex.Text;
                        AlarmWindow pp = new AlarmWindow(ge);
                        pp.ShowDialog();
                    }
                    #endregion
                    Common.Reports.LogFile.Log("Remove successful - Part ID:" + this.txt_partID.Text + " ; Sapcode:" + txt_mcID.Text + " ; MC ID:" + txt_mcID.Text + " by user " + txt_userID.Text);
                    MessageBox.Show("Remove Successful !!\n移除成功！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    StaticRes.Global.Transaction_Continue = false;
                    this.dg_list.ItemsSource = Logic.Transaction.Remove.Bin_withMaterial().DefaultView;
                    kb_UndoClick();
                    pb.Value = 0;
                }
                else
                {
                    if (MessageBox.Show("Please take out material from slot , then press 'OK' !!\n请把料取出后，点击‘OK’！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                    {
                        StaticRes.Global.IsOnProgress = false;
                        StaticRes.Global.Transaction_Continue = true;
                        unload.Start_UnLoad(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
                    }
                }
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Remove page error when complete , error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void unload_UnloadError(Event ge)
        {
            this.Dispatcher.Invoke(new XError(DoError), new object[1] { ge });
        }
        private void DoError(Event ge)
        {
            try
            {
                ge.USER_ID = StaticRes.Global.Current_User.USER_ID;
                ge.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                ge.PART_ID = this.txt_partID.Text;
                ge.SLOT_NO = "S" + this.txt_slotID.Text + "L" + this.txt_slotIndex.Text;
                btn_unload.IsEnabled = true;
                AlarmWindow aw = new AlarmWindow(ge);
                if ((bool)aw.ShowDialog())
                {
                    btn_unload.IsEnabled = false;
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    unload.Start_UnLoad(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
                }
                else
                {
                    Common.Reports.LogFile.Log("Terminate in remove process , user : " + txt_userID.Text);
                    StaticRes.Global.Transaction_Continue = false;
                    if (StaticRes.Global.Process_Code.Unloading != "U000")
                        StaticRes.Global.Need_Homing = true;
                    StaticRes.Global.Process_Code.Unloading = "U000";
                    backClick();
                    this.Close();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void unload_Step(string Event)
        {
            this.Dispatcher.Invoke(new Step(DoStep), new object[1] { Event });
        }
        private void DoStep(string Event)
        {
            try
            {
                List<string> list = new List<string>();
                list.Add(Event);
                foreach (string x in event_list.Items)
                {
                    list.Add(x);
                }
                event_list.ItemsSource = list;
                Common.Reports.LogFile.Log("Remove -- " + Event);
                pb.Value++;
            }
            catch
            {
            }
        }
  
        #endregion
    }
}
