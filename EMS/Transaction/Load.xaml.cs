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
    public partial class Load : Window
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
        private delegate void WeightComplete(float Weight_Return);
        private delegate void WeightContinue(float Weight_Return);
        private delegate void WeightError(Event et);
        private string MFG_Expiry_Date = string.Empty;
        private int Record_Count = 0;

        HardwareControl.Load load = new HardwareControl.Load();
        HardwareControl.Weighting Weighting = new HardwareControl.Weighting();

        DispatcherTimer tm = new DispatcherTimer();
        private DateTime time_start = System.DateTime.Now;
        private bool Transaction_ongoing = false;
        
        public Load()
        {
            InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
            this.txt_userID.Text = StaticRes.Global.Current_User.USER_ID;
            this.txt_userName.Text = StaticRes.Global.Current_User.USER_NAME;
            this.txt_userGroup.Text = StaticRes.Global.Current_User.USER_GROUP;
            this.txt_userDepartment.Text = StaticRes.Global.Current_User.DEPARTMENT;

            load.LoadComplete += new HardwareControl.Load.LoadCompleteEventHandler(load_LoadComplete);
            load.LoadError += new HardwareControl.Load.loaderrorEventHandler(load_LoadError);
            load.Step += new HardwareControl.Load.StepEventHandler(load_Step);

            Weighting.WeightComplete += new HardwareControl.Weighting.WeightCompleteEventHandler(Weighting_WeightComplete);
            Weighting.WeightContinue += new HardwareControl.Weighting.WeightContinueEventHandler(Weighting_WeightContinue);
            Weighting.WeightError += new HardwareControl.Weighting.weighterrorEventHandler(Weighting_WeightError);

            tm.Tick += new EventHandler(Timer);
            tm.Interval = TimeSpan.FromSeconds(1);
            tm.Start();
            event_list.ItemsSource = new List<string>();
            InitComPort();
            Common.Reports.LogFile.Log("Open COM");
            Handle_Scanner.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Scanner_DataReceived);
            kb.CurrentTextBox = txt_sapcode;
            txt_sapcode.Background = StaticRes.ColorBrushes.Linear_Green;
            txt_sapcode.Focus();

            HardwareControl.Motion_Control.Rotary_Motion_Stop();
            HardwareControl.Motion_Control.Motion_Speed_Checking();


        }

        void Timer(object sender, EventArgs e)
        {
            try
            {
                DateTime time_now = System.DateTime.Now;
                TimeSpan ts = time_now - time_start;
                if (ts.TotalSeconds > StaticRes.Global.System_Setting.Idle_Time_To_Exit && Transaction_ongoing == false)
                {
                    Common.Reports.LogFile.Log("Auto exit load page becuase idle time achieve setting  , user : " + StaticRes.Global.Current_User.USER_ID);
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
        private void kb_EnterClick()
        {
            if (this.txt_sapcode.IsFocused && this.txt_sapcode.IsEnabled)
            {
                SAP_Validate();
                return;
            }
            if (this.txt_mfExpiryDate.IsFocused && this.txt_mfExpiryDate.IsEnabled)
            {
                mfExpiryDate_Validate();
                return;
            }
            if (this.txt_batchNo.IsFocused && this.txt_batchNo.IsEnabled)
            {
                Batch_Validate();
                return;
            }
        }

        private void kb_UndoClick()
        {
            this.txt_sapcode.Text = string.Empty;
            this.txt_mfExpiryDate.Text = string.Empty;
            this.txt_batchNo.Text = string.Empty;
            this.txt_weighing.Text = string.Empty;
            this.txt_partID.Text = string.Empty;
            this.txt_description.Text = string.Empty;
            this.txt_thawingTime.Text = string.Empty;
            this.txt_expiryTime.Text = string.Empty;
            this.txt_newMinWeight.Text = string.Empty;
            this.txt_newMaxWeight.Text = string.Empty;
            this.txt_readyTime.Text = string.Empty;
            this.txt_capacity.Text = string.Empty;
            this.txt_emptySyringeWeight.Text = string.Empty;
            MFG_Expiry_Date = string.Empty;
            Record_Count = 0;
            this.txt_slotID.Text = string.Empty;
            this.txt_slotIndex.Text = string.Empty;
            this.btn_close.IsEnabled = true;
            this.btn_load.IsEnabled = true;
            this.btn_stop.IsEnabled = false;
            this.btn_openScrapBin.IsEnabled = true;
            this.btn_openScrapBin2.IsEnabled = true;
            this.txt_batchNo.IsEnabled = false;
            this.txt_mfExpiryDate.IsEnabled = false;
            this.txt_weighing.IsEnabled = false;
            this.txt_mfExpiryDate.Background = System.Windows.Media.Brushes.White;
            this.txt_batchNo.Background = System.Windows.Media.Brushes.White;
            this.txt_weighing.Background = System.Windows.Media.Brushes.White;
            this.txt_sapcode.Background = StaticRes.ColorBrushes.Linear_Green;
            kb.CurrentTextBox = this.txt_sapcode;
            this.txt_sapcode.IsEnabled = true;
            this.txt_sapcode.Focus();
            Transaction_ongoing = false;
        }

        private void txt_sapcode_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SAP_Validate();
            }  
        }
        void SAP_Validate()
        {
            try
            {
                int x = txt_sapcode.Text.IndexOf('/');
                if (x > 0)
                    txt_sapcode.Text = txt_sapcode.Text.Substring(0, x);
                ObjectModule.Local.Sapcode sap = Logic.Transaction.Sap_Infor(txt_sapcode.Text, StaticRes.Global.Current_User.DEPARTMENT);
                this.txt_description.Text = sap.DESCRIPTION;
                this.txt_newMaxWeight.Text = sap.NEW_MAX_WEIGHT.ToString();
                this.txt_newMinWeight.Text = sap.NEW_MIN_WEIGHT.ToString();
                this.txt_thawingTime.Text = System.DateTime.Now.ToString();
                this.txt_readyTime.Text = DateTime.Parse(txt_thawingTime.Text).AddHours(sap.THAWING_TIME).ToString();
                this.txt_expiryTime.Text = DateTime.Parse(txt_thawingTime.Text).AddHours(sap.USAGE_LIFE).ToString();
                this.txt_capacity.Text = sap.CAPACITY.ToString();
                this.txt_emptySyringeWeight.Text = sap.EMPTY_SYRINGE_WEIGHT.ToString();
                this.txt_partID.Text = "P"+ System.DateTime.Now.ToString("yyyyMMddHHmmss");
                ObjectModule.Local.Binning bin = Logic.Transaction.Search_Empty_Slot(int.Parse(txt_capacity.Text));
                this.txt_slotID.Text = bin.SLOT_ID.ToString();
                this.txt_slotIndex.Text = bin.SLOT_INDEX.ToString();
                kb.CurrentTextBox = txt_batchNo; //txt_mfExpiryDate;
                time_start = System.DateTime.Now; //Meaning have action
                Transaction_ongoing = true;
                TextBox_Logic(txt_sapcode, txt_batchNo);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_mfExpiryDate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                mfExpiryDate_Validate();
            }
        }
        void mfExpiryDate_Validate()
        {
            try
            {               
                DateTime mf_expiryTime = DateTime.Parse(this.txt_mfExpiryDate.Text);
                if(mf_expiryTime<DateTime.Parse(txt_expiryTime.Text))
                {
                     MessageBox.Show("MFG Expiry is near than floor life expiry date , don't allow to loading !!\n生产过期时间小于使用过期时间，不允许放入 !!");
                     kb_UndoClick();
                        return;
                }
                Transaction_ongoing = true;
                if (mf_expiryTime <= System.DateTime.Now)
                {
                    MessageBox.Show("Material already expiried , please check !!\n材料已过期，请检查！！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    kb_UndoClick();
                    return;
                }
                kb.CurrentTextBox = txt_batchNo;
                TextBox_Logic(txt_mfExpiryDate, txt_weighing);
                time_start = System.DateTime.Now; //Meaning have action
                Transaction_ongoing = true;
                StaticRes.Global.IsOnProgress = false;
                StaticRes.Global.Transaction_Continue = true;
                Weighting.Start_Weight(int.Parse(txt_capacity.Text), true, string.Empty, string.Empty, false, string.Empty);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Date Format must follow 'YYYY-MM-DD' !!\n日期格式必须是'YYYY-MM-DD'", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_batchNo_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
                Batch_Validate();
        }
        void Batch_Validate()
        {
            try
            {      
                string a = "RW";
                if (a != txt_batchNo.Text.Substring(0,2))
                {
                    MessageBox.Show("Enter the batch number error !!\n输入Batch号码错误", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                ObjectModule.Local.Binning bin = Logic.Transaction.Load.BatchNo_Validate(txt_sapcode.Text, txt_batchNo.Text, ref Record_Count);
                if (Record_Count >= 2)
                {
                    txt_mfExpiryDate.Text = bin.MF_EXPIRY_DATE.ToString();
                    kb.CurrentTextBox = null;
                    this.txt_batchNo.Background = System.Windows.Media.Brushes.White;
                    this.txt_batchNo.IsEnabled = false;
                    time_start = System.DateTime.Now; //Meaning have action
                    Transaction_ongoing = true;
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                     Weighting.Start_Weight(int.Parse(txt_capacity.Text), true, string.Empty, string.Empty, false, string.Empty);
                }
                else
                {
                    if (Record_Count == 1)
                        MFG_Expiry_Date = bin.MF_EXPIRY_DATE.ToString();
                    kb.CurrentTextBox = txt_mfExpiryDate;
                    TextBox_Logic(txt_batchNo, txt_mfExpiryDate);
                }
            }
            catch
            {
            }
        }

        private void txt_weighing_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (StaticRes.Global.Hardware_Connection)
                    this.txt_weighing.IsEnabled = true;
                if (e.Key == Key.Enter)
                    Weight_Validate(txt_weighing.Text);
            }
        }
        void Weight_Validate(string Weight)
        {
            try
            {
                Logic.Transaction.Load.Weight_Validation(Weight, txt_newMaxWeight.Text, txt_newMinWeight.Text);
                time_start = System.DateTime.Now; //Meaning have action
                Transaction_ongoing = false;
                TextBox_To_Btn(txt_weighing, btn_load);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                //Need weighting again
                try
                {
                    Weighting.Start_Weight(int.Parse(txt_capacity.Text), true, string.Empty, string.Empty, false, string.Empty);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_openScrapBin2_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (this.btn_openScrapBin2.Content.ToString() == "Open Scrap Drawer-2")
                {
                    Hardware.IO_LIST.Output.Y113_Scrap_Bin_Drawer_1_Open();
                    this.btn_openScrapBin2.Content = "Close Scrap Drawer-2";
                    return;
                }
                else if (this.btn_openScrapBin2.Content.ToString() == "Close Scrap Drawer_2")
                {
                    Hardware.IO_LIST.Output.Y113_Scrap_Bin_Drawer_1_Close();
                    this.btn_openScrapBin2.Content = "Open Scrap Drawer-2";
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_openScrapBin_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (this.btn_openScrapBin.Content.ToString() == "Open Scrap Drawer-1")
                {
                    Hardware.IO_LIST.Output.Y113_Scrap_Bin_Drawer_1_Open();
                    this.btn_openScrapBin.Content = "Close Scrap Drawer-1";
                    return;
                }
                else if (this.btn_openScrapBin.Content.ToString() == "Close Scrap Drawer_1")
                {
                    Hardware.IO_LIST.Output.Y113_Scrap_Bin_Drawer_1_Close();
                    this.btn_openScrapBin.Content = "Open Scrap Drawer-1";
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Hardware.IO_LIST.Output.Y113_Scrap_Bin_Drawer_1_Close();
                Hardware.IO_LIST.Output.Y114_Scrap_Bin_Drawer_2_Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Common.Reports.LogFile.Log("Exit load page  , user : " + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                StaticRes.Global.Current_User.USER_ID = string.Empty;
                StaticRes.Global.Transaction_Continue = false;
                if (StaticRes.Global.Process_Code.Loading != "L000")
                    StaticRes.Global.Need_Homing = true;
                StaticRes.Global.Process_Code.Loading = "L000";
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

        private void btn_load_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StaticRes.Global.Hardware_Connection)
                {
                    if (StaticRes.Global.System_Setting.Print_Label_Before_Load == "True")
                    {
                        do
                        {
                            Logic.Print.EMS(txt_partID.Text, string.Empty, DateTime.Parse(txt_expiryTime.Text).ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Parse(txt_thawingTime.Text).ToString("yyyyMMddHHmmss"),
                                DateTime.Parse(txt_readyTime.Text).ToString("yyyy-MM-dd HH:mm:ss"), txt_sapcode.Text);
                        }
                        while (MessageBox.Show(this, "Do you receive the label ?!\n你有看到打印的标签吗？！", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No);
                    }
                }

                this.btn_close.IsEnabled = false;
                this.btn_openScrapBin.IsEnabled = false;
                this.btn_openScrapBin2.IsEnabled = false;
                this.btn_stop.IsEnabled = true;
                this.btn_load.IsEnabled = false;
                StaticRes.Global.IsOnProgress = false;
                time_start = System.DateTime.Now; //Meaning have action
                Transaction_ongoing = true;
                Common.Reports.LogFile.Log("Start Loading to S" + this.txt_slotID.Text.ToString() + "L" + this.txt_slotIndex.Text.ToString() + " by User ID : " + txt_userID.Text);
                StaticRes.Global.Transaction_Continue = true;
                pb.Value = 0;              
                pb.Maximum = load.Start_Load(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Loading page error : " + ee.Message);
                MessageBox.Show(ee.Message,"Message",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void btn_stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                time_start = System.DateTime.Now; //Meaning have action
                Transaction_ongoing = false;
                StaticRes.Global.Transaction_Continue = false;
                HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                this.btn_close.IsEnabled = true;
                this.btn_load.IsEnabled = true;
                this.btn_stop.IsEnabled = false;
                this.btn_openScrapBin.IsEnabled = true;
                this.btn_openScrapBin2.IsEnabled = true;
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Loading page error when click stop : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Thread Return
        void load_Step(string Event)
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
                Common.Reports.LogFile.Log("Loading/Weighting -- " + Event);
                pb.Value++;
            }
            catch
            {
            }
        }

        void load_LoadError(Event ee)
        {
            this.Dispatcher.Invoke(new XError(DoError), new object[1] { ee });
        }
        private void DoError(Event et)
        {
            try
            {
                et.USER_ID = StaticRes.Global.Current_User.USER_ID;
                et.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                et.PART_ID = this.txt_partID.Text;
                et.SLOT_NO = "Slot-" + this.txt_slotID.Text + ",Index" + this.txt_slotIndex.Text;
                btn_load.IsEnabled = true;
                CloseComPort();
                Common.Reports.LogFile.Log("Close COM");
                AlarmWindow aw = new AlarmWindow(et);
                if ((bool)aw.ShowDialog())
                {
                    btn_close.IsEnabled = false;
                    btn_stop.IsEnabled = true;
                    btn_load.IsEnabled = false;
                    btn_openScrapBin.IsEnabled = false;
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    InitComPort();
                    Common.Reports.LogFile.Log("Open COM");
                    load.Start_Load(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
                }
                else
                {
                    Common.Reports.LogFile.Log("Terminate in loading process , user : " + txt_userID);
                    if (StaticRes.Global.Process_Code.Loading != "L000")
                        StaticRes.Global.Need_Homing = true;
                    StaticRes.Global.Transaction_Continue = false;
                    StaticRes.Global.Process_Code.Loading = "L000";
                    backClick();
                    this.Close();
                }
            }
            catch
            {
            }
        }

        void load_LoadComplete(bool complete)
        {
            this.Dispatcher.Invoke(new Complete(DoComplete), new object[1] {complete});
        }
        private void DoComplete(bool complete)
        {
            try
            {
                if (complete)
                {
                    #region Update Database
                    while (!Logic.Transaction.Load.Local_Update(txt_slotID.Text, txt_slotIndex.Text, txt_batchNo.Text,
                        txt_sapcode.Text, txt_description.Text, txt_partID.Text, txt_thawingTime.Text, txt_readyTime.Text,
                        txt_expiryTime.Text, this.txt_mfExpiryDate.Text, txt_weighing.Text, txt_capacity.Text, txt_emptySyringeWeight.Text))
                    {
                        Event et = new Event();
                        et.EVENT_NAME = StaticRes.Global.Error_List.Update_database_failed;
                        AlarmWindow pp = new AlarmWindow(et);
                        pp.ShowDialog();
                    }
                    Common.Reports.LogFile.Log("Load successful - Part ID:" + this.txt_partID.Text + " ; Sapcode:" + txt_sapcode.Text + " by user " + txt_userID.Text);
                    MessageBox.Show("Loading Successful !!\n存料成功！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);                   
                    StaticRes.Global.Transaction_Continue = false;
                    kb_UndoClick();
                    pb.Value = 0;
                    #endregion
                }
                else
                {
                    if (MessageBox.Show("Please put material into slot !!\n请把料放入槽中！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                    {
                        StaticRes.Global.IsOnProgress = false;
                        StaticRes.Global.Transaction_Continue = true;
                        load.Start_Load(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text));
                    }
                }
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Loading page error when complete thread : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Weighting_WeightComplete(float Weight_Return)
        {
            this.Dispatcher.Invoke(new WeightComplete(DoWeightComplete), new object[1] { Weight_Return });
        }
        private void DoWeightComplete(float Weight_Return)
        {
            Weight_Validate(Weight_Return.ToString());
        }

        void Weighting_WeightContinue(float Weight_Return)
        {
            this.Dispatcher.Invoke(new WeightContinue(DoWeightContinue), new object[1] { Weight_Return });
        }
        private void DoWeightContinue(float Weight_Return)
        {
            switch (StaticRes.Global.Process_Code.Weighting)
            {
                case "W200":
                    {
                        if (MessageBox.Show("Please put material on weight scale then press 'OK' !!\n请把物料放入秤台再点击‘OK’！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                        {
                            StaticRes.Global.IsOnProgress = false;
                            Weighting.Start_Weight(int.Parse(txt_capacity.Text), true, string.Empty, string.Empty, false,txt_emptySyringeWeight.Text);
                        }
                    }
                    break;
                case "W1100":
                    {
                        this.txt_weighing.Text = Weight_Return.ToString();
                        if (MessageBox.Show("Please take out material from weight scale then press 'OK' !!\n请将秤台上的物料取出再点击‘OK’！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                        {
                            StaticRes.Global.IsOnProgress = false;
                            Weighting.Start_Weight(int.Parse(txt_capacity.Text), true, string.Empty, string.Empty, false, txt_emptySyringeWeight.Text);
                        }
                    }
                    break;
            }
        }

        void Weighting_WeightError(Event et)
        {
            this.Dispatcher.Invoke(new WeightError(DoWeightError), new object[1] { et });
        }
        private void DoWeightError(Event et)
        {
            try
            {
                et.USER_ID = StaticRes.Global.Current_User.USER_ID;
                et.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                et.PART_ID = this.txt_partID.Text;
                et.SLOT_NO = "Slot-" + this.txt_slotID.Text + ",Index" + this.txt_slotIndex.Text;
                btn_load.IsEnabled = true;
                CloseComPort();
                Common.Reports.LogFile.Log("Close COM");
                AlarmWindow aw = new AlarmWindow(et);
                if ((bool)aw.ShowDialog())
                {
                    btn_close.IsEnabled = false;
                    btn_stop.IsEnabled = true;
                    btn_load.IsEnabled = false;
                    btn_openScrapBin.IsEnabled = false;
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    InitComPort();
                    Common.Reports.LogFile.Log("Open COM");
                    Weighting.Start_Weight(int.Parse(txt_capacity.Text), true, string.Empty, string.Empty, false, txt_emptySyringeWeight.Text);
                }
                else
                {
                    Common.Reports.LogFile.Log("Terminate in loading process , user : " + txt_userID);
                    if (StaticRes.Global.Process_Code.Loading != "L000")
                        StaticRes.Global.Need_Homing = true;
                    StaticRes.Global.Transaction_Continue = false;
                    StaticRes.Global.Process_Code.Loading = "L000";
                    backClick();
                    this.Close();
                }
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Terminate in loading process , user :" + ee.Message);
                MessageBox.Show(ee.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
               
            }
        }
        #endregion

        #region Inventory Check
        private void btn_inventoryCheck_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //time_start = System.DateTime.Now; //Meaning have action
            //this.pb_check.Value = 0;
            //this.dg_checkResult.Items.Clear();
            //this.checkEvent_list.ItemsSource = new List<string>();
            //this.MaskActionPage.Visibility = Visibility.Visible;
        }

        private void btn_startCheck_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //try
            //{
            //    if (StaticRes.Global.Hardware_Connection)
            //    {
            //        this.btn_startCheck.IsEnabled = false;
            //        this.btn_stopCheck.IsEnabled = true;
            //        this.btn_closeCheck.IsEnabled = false;
            //        StaticRes.Global.IsOnProgress = false;
            //        time_start = System.DateTime.Now; //Meaning have action
            //        Transaction_ongoing = true;
            //        StaticRes.Global.Transaction_Continue = true;
            //        Common.Reports.LogFile.Log("Start inventory check by User ID : " + txt_userID.Text);
            //        pb_check.Value = 0;
            //        pb_check.Maximum = inventoryCheck.Start_InventoryCheck();
            //    }
            //    else
            //        MessageBox.Show("Non hardward connection !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (Exception ee)
            //{
            //    Common.Reports.LogFile.Log("Inventory check page error : " + ee.Message);
            //    MessageBox.Show(ee.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void btn_stopCheck_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //try
            //{
            //    time_start = System.DateTime.Now; //Meaning have action
            //    Transaction_ongoing = false;
            //    StaticRes.Global.Transaction_Continue = false;
            //    this.btn_startCheck.IsEnabled = true;
            //    this.btn_stopCheck.IsEnabled = false;
            //    this.btn_closeCheck.IsEnabled = true;
            //}
            //catch (Exception ee)
            //{
            //    Common.Reports.LogFile.Log("Inventory check page error when click stop : " + ee.Message);
            //    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
        }

        private void btn_closeCheck_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //time_start = System.DateTime.Now; //Meaning have action
            //StaticRes.Global.Transaction_Continue = false;
            //StaticRes.Global.Process_Code.Inventory_Check = "I000";
            //List<string> list = new List<string>();
            //checkEvent_list.ItemsSource = list;
            //this.MaskActionPage.Visibility = Visibility.Collapsed;
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
                    Handle_Scanner.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Scanner_DataReceived);
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
            if (this.txt_sapcode.IsFocused && txt_sapcode.IsEnabled)
            {
                this.txt_sapcode.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                SAP_Validate();
                return;
            }
            if (this.txt_batchNo.IsFocused && txt_batchNo.IsEnabled)
            {
                this.txt_batchNo.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                Batch_Validate();
                return;
            }
            if (this.txt_mfExpiryDate.IsFocused && txt_mfExpiryDate.IsEnabled)
            {
                this.txt_mfExpiryDate.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                mfExpiryDate_Validate();
                return;
            }

        }
        #endregion

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

        private void btn_Mix_Click(object sender, RoutedEventArgs e)
        {
            Transaction.Mix mixPage = EMS.Singleton.MixSingleton.GetInstance;
            mixPage.ShowWindow();
        }

    }
}