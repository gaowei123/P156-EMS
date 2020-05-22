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
using System.Windows.Threading;
using System.Data;

namespace EMS.Transaction
{
    /// <summary>
    /// Load.xaml 的交互逻辑
    /// </summary>
    public partial class Unload : Window
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
        private delegate void ReadCom();

        HardwareControl.Unload unload = new HardwareControl.Unload();

        DispatcherTimer tm = new DispatcherTimer();
        private DateTime time_start = System.DateTime.Now;
        private bool Transaction_ongoing = false;

        public Unload()
        {
            InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
            unload.UnloadComplete += new HardwareControl.Unload.UnloadCompleteEventHandler(unload_UnloadComplete);
            unload.UnloadError += new HardwareControl.Unload.unloaderrorEventHandler(unload_UnloadError);
            unload.Step += new HardwareControl.Unload.StepEventHandler(unload_Step);

            this.txt_userID.Text = StaticRes.Global.Current_User.USER_ID;
            this.txt_userName.Text = StaticRes.Global.Current_User.USER_NAME;
            this.txt_userGroup.Text = StaticRes.Global.Current_User.USER_GROUP;
            this.txt_userDepartment.Text = StaticRes.Global.Current_User.DEPARTMENT;
            event_list.ItemsSource = new List<string>();
            kb.CurrentTextBox = this.txt_mcID;
            this.txt_mcID.Background = StaticRes.ColorBrushes.Linear_Green;
            InitComPort();
            Common.Reports.LogFile.Log("Open COM");
            tm.Tick += new EventHandler(Timer);
            tm.Interval = TimeSpan.FromSeconds(1);
            tm.Start();

            txt_mcID.Focus();
        }

        void Timer(object sender, EventArgs e)
        {
            try
            {
                DateTime time_now = System.DateTime.Now;
                TimeSpan ts = time_now - time_start;
                if (ts.TotalSeconds > StaticRes.Global.System_Setting.Idle_Time_To_Exit && Transaction_ongoing == false)
                {
                    Common.Reports.LogFile.Log("Auto exit unload page becuase idle time achieve setting  , user : " + StaticRes.Global.Current_User.USER_ID);
                    StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                    StaticRes.Global.Current_User.USER_ID = string.Empty;
                    CloseComPort();
                    Common.Reports.LogFile.Log("Close COM");
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
                Common.Reports.LogFile.Log("Exit unload page  , user : " + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                StaticRes.Global.Current_User.USER_ID = string.Empty;
                StaticRes.Global.Transaction_Continue = false;
                if (StaticRes.Global.Process_Code.Unloading != "U000")
                    StaticRes.Global.Need_Homing = true;
                StaticRes.Global.Process_Code.Unloading = "U000";
                hdClick();
                backClick();
                tm.Stop();
                CloseComPort();
                Common.Reports.LogFile.Log("Close COM");
                this.Close();
            }
            catch (Exception ee)
            {
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
                time_start = System.DateTime.Now; //Meaning have action
        }
        void Machine_Validate()
        {
            try
            {
                ObjectModule.Local.Equipment ge = Logic.Transaction.Unload.Equip_Infor(txt_mcID.Text);
                this.txt_locID.Text = ge.LOCID;
                kb.CurrentTextBox = null;// txt_lotID;yakun.zhou.2015.5.13
                time_start = System.DateTime.Now; //Meaning have action
                TextBox_Logic(txt_mcID, txt_lotID);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_lotID_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Lot_Validate();
            }
            else
                time_start = System.DateTime.Now; //Meaning have action
        }
        void Lot_Validate()
        {
            try
            {
                txt_lotID.Text=txt_lotID.Text.Trim();
                Logic.Transaction.Unload.Lot_Validation(txt_lotID.Text);
               // kb.CurrentTextBox = txt_mesDevice;
                time_start = System.DateTime.Now; //Meaning have action
                TextBox_Logic(txt_lotID, txt_mesDevice);    
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void txt_mesDevice_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mesDevice_Validate();
            }
            else
                time_start = System.DateTime.Now; //Meaning have action
        }
        void mesDevice_Validate()
        {
            try
            {
                string a = "TM"; 
                string b = "PU";
                if (a != txt_mesDevice.Text.Substring(0, 2) && b != txt_mesDevice.Text.Substring(0, 2))
                {
                    MessageBox.Show("Enter the PART number error !!\n输入PART NO号码错误", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                txt_mesDevice.Text = txt_mesDevice.Text.Trim();
                Logic.Transaction.Unload.mesDevice_Validation(txt_mesDevice.Text);
               // kb.CurrentTextBox = txt_sapcode;
                time_start = System.DateTime.Now; //Meaning have action
                TextBox_Logic(txt_mesDevice, txt_sapcode);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void txt_sapcode_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Sapcode_Validate();
            }
            else if (e.Key == Key.Escape)
                TextBox_Logic(txt_sapcode, txt_lotID);
            else
                time_start = System.DateTime.Now; //Meaning have action
        }
        void Sapcode_Validate()
        {
            try
            {
                int x=txt_sapcode.Text.IndexOf ('/');
                if (x>0)
                    txt_sapcode .Text = txt_sapcode.Text .Substring (0,x);
                ObjectModule.Local.Binning gb = Logic.Transaction.Unload.search_Inventory(txt_sapcode.Text, StaticRes.Global.Current_User.DEPARTMENT);
                if (gb.STATUS == StaticRes.Global.Binning_Status.New)
                    this.txt_status.Text = StaticRes.Global.Status.Load;
                else if (gb.STATUS == StaticRes.Global.Binning_Status.Reuse)
                this.txt_status.Text = StaticRes.Global.Status.Reuse;
                this.txt_expiryTime.Text = gb.EXPIRY_DATETIME.ToString();
                this.txt_thawingTime.Text = gb.THAWING_DATETIME.ToString();
                this.txt_readyTime.Text = gb.READY_DATETIME.ToString();
               //this.txt_mesDevice.Text = txt_lotID.Text;
                this.txt_partID.Text = gb.PART_ID;
                this.txt_description.Text = gb.DESCRIPTION;
                this.txt_batchNo.Text = gb.BATCH_NO;
                this.txt_currentWeight.Text = gb.CURRENT_WEIGHT.ToString();
                this.txt_startWeight.Text = gb.START_WEIGHT.ToString();
                this.txt_slotIndex.Text = gb.SLOT_INDEX.ToString();
                this.txt_slotID.Text = gb.SLOT_ID.ToString();
                this.txt_capacity.Text = gb.CAPACITY.ToString();
                ObjectModule.Local.Sapcode sap = Logic.Transaction.Sap_Infor(gb.SAPCODE, StaticRes.Global.Current_User.DEPARTMENT);
                this.txt_syringeWeight.Text = sap.EMPTY_SYRINGE_WEIGHT.ToString();
                time_start = System.DateTime.Now; //Meaning have action
                TextBox_To_Btn(txt_sapcode, btn_unload);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                Common.Reports.LogFile.Log("Start Unload , Part ID : " + txt_partID.Text + " ; Sapcode : " + txt_sapcode.Text + " ; Batch No : " + txt_batchNo.Text + " ; User ID : " + txt_userID.Text);
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
                Common.Reports.LogFile.Log("Unload page error when click stop : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void kb_UndoClick()
        {
            this.txt_mcID.Text = string.Empty;
            this.txt_locID.Text = string.Empty;
            this.txt_lotID.Text = string.Empty;
            this.txt_sapcode.Text = string.Empty;
            this.txt_status.Text = string.Empty;
            this.txt_slotID.Text = string.Empty;
            this.txt_slotIndex.Text = string.Empty;
            this.txt_mesDevice.Text = string.Empty;
            this.txt_partID.Text = string.Empty;
            this.txt_description.Text = string.Empty;
            this.txt_batchNo.Text = string.Empty;
            this.txt_startWeight.Text = string.Empty;
            this.txt_currentWeight.Text = string.Empty;
            this.txt_thawingTime.Text = string.Empty;
            this.txt_readyTime.Text = string.Empty;
            this.txt_expiryTime.Text = string.Empty;
            this.txt_syringeWeight.Text = string.Empty;
            this.txt_capacity.Text = string.Empty;
            this.txt_lotID.IsEnabled = false;
            this.btn_stop.IsEnabled = false;
            this.btn_unload.IsEnabled = false;
            this.btn_close.IsEnabled = true;
            this.txt_sapcode.Background = System.Windows.Media.Brushes.White;
            this.txt_lotID.Background = System.Windows.Media.Brushes.White;
            this.txt_mesDevice.Background = System.Windows.Media.Brushes.White;
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
            if (this.txt_lotID.IsFocused)
            {
                Lot_Validate();
                return;
            }
            if (this.txt_mesDevice.IsFocused)
            {
                mesDevice_Validate();
                return;
            }
            if (this.txt_sapcode.IsFocused)
            {
                Sapcode_Validate();
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
            this.Dispatcher.Invoke(new Complete(DoComplete), new object[1] {complete});
        }
        private void DoComplete(bool complete)
        {
            try
            {
                if (complete)
                {
                    #region Update Server Database
                    while (!Logic.Transaction.Unload.Local_Update(txt_partID.Text,txt_slotID.Text,txt_slotIndex.Text,txt_mcID.Text,
                        txt_locID.Text,txt_status.Text,txt_startWeight.Text,txt_currentWeight.Text,txt_sapcode.Text,txt_thawingTime.Text,
                        txt_readyTime.Text,txt_expiryTime.Text,txt_description.Text,txt_batchNo.Text,txt_capacity.Text,txt_lotID.Text,
                        txt_mesDevice.Text))
                        {
                        Event ge = new Event();
                        ge.EVENT_NAME = StaticRes.Global.Error_List.Update_database_failed;
                        ge.PROCESS_CODE = "Update DB";
                        ge.SLOT_NO = "S" + txt_slotID.Text + "L" + txt_slotIndex.Text;
                        AlarmWindow pp = new AlarmWindow(ge);
                        pp.ShowDialog();
                    }
                    #endregion

                    if (StaticRes.Global.Hardware_Connection)
                    {
                        if (StaticRes.Global.System_Setting.Print_Label_After_Unload == "True")
                        {
                            do
                            {
                                Logic.Print.EMS(txt_partID.Text, txt_mcID.Text, txt_expiryTime.Text, txt_thawingTime.Text, txt_readyTime.Text, txt_sapcode.Text);
                            }
                            while (MessageBox.Show(this, "Do you receive the label ?!\n你有保存这个号码吗？！", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No);
                        }
                    }
                    Common.Reports.LogFile.Log("Unload successful - Part ID:" + this.txt_partID.Text + " ; Sapcode:" + txt_sapcode.Text + " ; MC ID:" + txt_mcID.Text + " by user " + txt_userID.Text);
                    MessageBox.Show("Unloading Successful !!\n取料成功！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);                   
                    StaticRes.Global.Transaction_Continue = false;
                    kb_UndoClick();
                    pb.Value = 0;



                    #region  自动跳转mix page. 
                    if (MessageBox.Show("Do you need to mix immediately?","Message",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        Transaction.Mix mixPage = EMS.Singleton.MixSingleton.GetInstance;

                        mixPage.userID = StaticRes.Global.Current_User.USER_ID;
                        mixPage.userGroup = StaticRes.Global.Current_User.USER_GROUP;
                        mixPage.userName = StaticRes.Global.Current_User.USER_NAME;
                        mixPage.department = StaticRes.Global.Current_User.DEPARTMENT;


                        
                        mixPage.ShowWindow();
                    }
                    #endregion

                }
                else
                {
                    //2020 05 13 add slot detect logic. if need to check in GUI side
                    if (StaticRes.Global.Process_Code.Unloading == "U050")
                    {
                        StaticRes.Global.IsOnProgress = false;
                        StaticRes.Global.Transaction_Continue = true;
                        //compare the slot ID and label

                        unload.Start_UnLoad(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
                    }
                    else
                    {
                        if (MessageBox.Show("Please take out material from slot , then press 'OK' !!\n请取出物料再点击‘OK’！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                        {
                            StaticRes.Global.IsOnProgress = false;
                            StaticRes.Global.Transaction_Continue = true;
                            unload.Start_UnLoad(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Unload page error when complete , error : " + ee.Message);
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
                StaticRes.Global.IsOnProgress = false;
                CloseComPort();
                Common.Reports.LogFile.Log("Close COM");
                AlarmWindow aw = new AlarmWindow(ge);
                if ((bool)aw.ShowDialog())
                {
                    InitComPort();
                    Common.Reports.LogFile.Log("Open COM");
                    btn_unload.IsEnabled = false;
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    unload.Start_UnLoad(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
                }
                else
                {
                    Common.Reports.LogFile.Log("Terminate in unload process , user : " + txt_userID.Text);
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
                Common.Reports.LogFile.Log("Unloading-" + Event);
                pb.Value++;
            }
            catch
            {
            }
        }
        #endregion

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
                {
                    Handle_Scanner.Open();
                    Handle_Scanner.DataReceived+=new System.IO.Ports.SerialDataReceivedEventHandler(Scanner_DataReceived);
                }
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
            if (txt_mcID.IsFocused && txt_mcID.IsEnabled)
            {
                this.txt_mcID.Text =  Handle_Scanner.ReadExisting().Replace("\r","").Replace("\n","");
                Machine_Validate();
            }
            if (txt_lotID.IsFocused && txt_lotID.IsEnabled)
            {
                this.txt_lotID.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                Lot_Validate();
            } 
            if (txt_mesDevice.IsFocused && txt_mesDevice.IsEnabled)
            {
                this.txt_mesDevice.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                Sapcode_Validate();
            }
            if (txt_sapcode.IsFocused && txt_sapcode.IsEnabled)
            {
                this.txt_sapcode.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                Sapcode_Validate();
            }
           
        }
        #endregion
    }
}
