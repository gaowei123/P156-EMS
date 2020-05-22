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
using System.Data;
using ObjectModule.Local;
using System.Windows.Threading;

namespace EMS.Transaction
{
    /// <summary>
    /// Load.xaml 的交互逻辑
    /// </summary>
    public partial class Return : Window
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

        HardwareControl.Return x = new HardwareControl.Return();
        HardwareControl.Weighting Weighting = new HardwareControl.Weighting();

        DispatcherTimer tm = new DispatcherTimer();
        private DateTime time_start = System.DateTime.Now;
        private bool Transaction_ongoing = false;
        private DateTime time_kb = System.DateTime.Now;

        public Return()
        {
            InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
            x.ReturnComplete += new HardwareControl.Return.ReturnCompleteEventHandler(x_ReturnComplete);
            x.ReturnError +=new HardwareControl.Return.ReturnerrorEventHandler(x_ReturnError);
            Weighting.WeightComplete += new HardwareControl.Weighting.WeightCompleteEventHandler(Weighting_WeightComplete);
            Weighting.WeightContinue += new HardwareControl.Weighting.WeightContinueEventHandler(Weighting_WeightContinue);
            Weighting.WeightError += new HardwareControl.Weighting.weighterrorEventHandler(Weighting_WeightError);
            x.Step += new HardwareControl.Return.StepEventHandler(x_Step);

            this.txt_userID.Text = StaticRes.Global.Current_User.USER_ID;
            this.txt_userName.Text = StaticRes.Global.Current_User.USER_NAME;
            this.txt_userGroup.Text = StaticRes.Global.Current_User.USER_GROUP;
            this.txt_userDepartment.Text = StaticRes.Global.Current_User.DEPARTMENT;
            if (StaticRes.Global.Current_User.USER_GROUP==StaticRes.Global.User_Group.Admin ||
                StaticRes.Global.Current_User.USER_GROUP==StaticRes.Global.User_Group.Engineer||
                StaticRes.Global.Current_User.USER_GROUP==StaticRes.Global.User_Group.Supervisor)
            {
                this.cb_forceScrap.IsEnabled = true;
                this.cbb_remark.IsEnabled = true;
            }
            else
            {
                this.cb_forceScrap.IsEnabled = false;
                this.cbb_remark.IsEnabled = false;
            }
            this.event_list.ItemsSource = new List<string>();
            //kb.CurrentTextBox = this.txt_partID;
            InitComPort();
            Common.Reports.LogFile.Log("Open COM");
            this.txt_partID.Background = StaticRes.ColorBrushes.Linear_Green;

            tm.Tick += new EventHandler(Timer);
            tm.Interval = TimeSpan.FromSeconds(1);
            tm.Start();

            txt_partID.Focus();
        }

        void Timer(object sender, EventArgs e)
        {
            try
            {
                DateTime time_now = System.DateTime.Now;
                TimeSpan ts = time_now - time_start ;
                if (ts.TotalSeconds > StaticRes.Global.System_Setting.Idle_Time_To_Exit && Transaction_ongoing == false)
                {
                    Common.Reports.LogFile.Log("Auto exit return page becuase idle time achieve setting  , user : " + StaticRes.Global.Current_User.USER_ID);
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
                Common.Reports.LogFile.Log("Exit return page  , user : " + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                StaticRes.Global.Current_User.USER_ID = string.Empty;
                StaticRes.Global.Transaction_Continue = false;
                if (StaticRes.Global.Process_Code.Returning != "R000")
                    StaticRes.Global.Need_Homing = true;
                StaticRes.Global.Process_Code.Returning = "R000";
                StaticRes.Global.Re_scan = false;//YAKUN.ZHOU-2015.07.13
                //StaticRes.Global.updata = false;//YAKUN.ZHOU 2015.07.13
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

        private void btn_return_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {               
                Return_Process();   
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Return_Process()
        {
            this.btn_close.IsEnabled = false;
            this.btn_stop.IsEnabled = true;
            this.btn_return.IsEnabled = false;
            StaticRes.Global.IsOnProgress = false;
            Common.Reports.LogFile.Log("Start Return , Part ID : " + txt_partID.Text + " ; Sapcode : " + txt_sapcode.Text + " ; Batch No : " + txt_batchNo.Text + " ; User ID : " + txt_userID.Text);
            time_start = System.DateTime.Now; //Meaning have action
            Transaction_ongoing = true;
            StaticRes.Global.Transaction_Continue = true;
            float Scrap_Weight = float.Parse(txt_scrapWeight.Text);
            if (cb_forceScrap.IsChecked == true)
            {
                if (cbb_remark.Text.Length == 0)
                {
                    MessageBox.Show("Please select remark first if you want force scrap !!\n请选择备注，如果你想要报废！！", "Message", MessageBoxButton.OK);
                    return;
                }
            }
            pb.Value = 0;          
            pb.Maximum = x.Start_Return(int.Parse(txt_slotID.Text),int.Parse(txt_slotIndex.Text), true);
        }

        private void btn_stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                StaticRes.Global.Transaction_Continue = false;
                StaticRes.Global.IsOnProgress = false;
                StaticRes.Global.Re_scan = false;//YAKUN.ZHOU 2015.07.13
                //StaticRes.Global.updata = false;//YAKUN.ZHOU 2015.07.13
                time_start = System.DateTime.Now; //Meaning have action
                this.btn_close.IsEnabled = true;
                this.btn_return.IsEnabled = true;
                this.btn_stop.IsEnabled = false;
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Return page error during click stop , error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void txt_partID_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    event_list.ItemsSource = new List<string>();
                    PartID_Validate();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //2017 06 05 by junyi only use scan not KeyBoard
                if (StaticRes.Global.System_Setting.Allow_Manual_KeyIn != "False")
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.V)
                    {
                        e.Handled = true;
                    }
                    if (e.Key == Key.LeftCtrl && e.Key == Key.V)
                    {
                        e.Handled = true;
                    }
                    DateTime tempDt = DateTime.Now;
                    TimeSpan ts = tempDt.Subtract(time_kb);
                    if (ts.Milliseconds > 150)
                        this.txt_partID.Text = string.Empty;
                    time_kb = tempDt;
                    time_start = System.DateTime.Now;
                }

            }
            //time_start = System.DateTime.Now; //Meaning have action

        }
        void PartID_Validate()
        {
            try
            {
                if (!StaticRes.Global.Re_scan)
                {
                    ObjectModule.Local.Tracking gt = Logic.Transaction.Return.PartID_Validate(txt_partID.Text);
                    StaticRes.Global.Pt = txt_partID.Text;
                    this.txt_batchNo.Text = gt.BATCH_NO;
                    this.txt_description.Text = gt.DESCRIPTION;
                    this.txt_sapcode.Text = gt.SAPCODE;
                    this.txt_equipID.Text = gt.EQUIP_ID;
                    this.txt_batchNo.Text = gt.BATCH_NO;
                    this.txt_description.Text = gt.DESCRIPTION;
                    this.txt_startWeight.Text = gt.START_WEIGHT.ToString();
                    this.txt_thawingTime.Text = gt.THAWING_DATETIME.ToString();
                    this.txt_readyTime.Text = gt.READY_DATETIME.ToString();
                    this.txt_expiryTime.Text = gt.EXPIRY_DATETIME.ToString();
                    this.txt_capacity.Text = gt.CAPACITY.ToString();
                    this.txt_syringeWeight.Text = gt.EMPTY_SYRINGE_WEIGHT.ToString();
                    ObjectModule.Local.Sapcode gs = Logic.Transaction.Sap_Infor(txt_sapcode.Text, StaticRes.Global.Current_User.DEPARTMENT);
                    this.txt_scrapWeight.Text = gs.SCRAP_WEIGHT.ToString();
                    if (cb_forceScrap.IsChecked == true)
                    {
                        if (cbb_remark.Text.Length == 0)
                        {
                            MessageBox.Show("Please select remark first if you want force scrap !!\n请选择备注，如果你想要报废！！", "Message", MessageBoxButton.OK);
                            return;
                        }
                    }
                    txt_partID.IsEnabled = false;
                    txt_partID.Background = System.Windows.Media.Brushes.WhiteSmoke;
                    kb.CurrentTextBox = null;
                    time_start = System.DateTime.Now; //Meaning have action
                    Transaction_ongoing = true;
                    StaticRes.Global.IsOnProgress = false;//2014-12-26yakun
                    Weighting.Start_Weight(gs.CAPACITY, false, gs.SCRAP_WEIGHT.ToString(), gt.EXPIRY_DATETIME.ToString(), (bool)cb_forceScrap.IsChecked, txt_syringeWeight.Text);
                }
                else
                {
                    ObjectModule.Local.Tracking gt = Logic.Transaction.Return.PartID_Validate(txt_partID.Text);
                    if (StaticRes.Global.Pt == txt_partID.Text.ToString())
                    {
                        StaticRes.Global.Re_scan = false;//YAKUN.ZHOU 2015.07.13
                        StaticRes.Global.Pt = string.Empty;
                        TextBox_To_Btn(txt_partID, btn_return);
                    }
                    else
                    {
                        throw new System.Exception("Invalid Part ID !!\n错误的Part ID!!");
                    }

                }
            }

            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

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
                txt_weighing.Text = Weight;
                if (cb_forceScrap.IsChecked != true)
                {
                   // StaticRes.Global.updata = true; //YAKUN.ZHOU 2015.07.13
                    this.cbb_returnType.Text = Logic.Transaction.Return.Weight_Validation(Weight, txt_scrapWeight.Text, txt_expiryTime.Text, txt_syringeWeight.Text);
                }
                else
                {
                   // StaticRes.Global.updata = true; //YAKUN.ZHOU 2015.07.13
                    this.cbb_returnType.Text = StaticRes.Global.Status.Scrap;
                }
                if (this.cbb_returnType.Text == StaticRes.Global.Status.Reuse)
                {
                    ObjectModule.Local.Binning fb = Logic.Transaction.Search_Empty_Slot(int.Parse(txt_capacity.Text));
                    this.txt_slotID.Text = fb.SLOT_ID.ToString();
                    this.txt_slotIndex.Text = fb.SLOT_INDEX.ToString();
                    StaticRes.Global.Re_scan=true;
                    this.txt_partID.Text = string.Empty;
                    this.txt_partID.IsEnabled = true; ;
                    TextBox_Logic(txt_weighing, txt_partID);
                    MessageBox.Show("请再扫描partID确认存储", "Message", MessageBoxButton.OK);
                }
                else
                {
                   // StaticRes.Global.updata = true;//YAKUN.ZHOU 2015.07.13
                    this.txt_slotID.Text = "0";
                    this.txt_slotIndex.Text = "0";
                    TextBox_To_Btn(txt_weighing, btn_return);
                    Return_Process();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void kb_EnterClick()
        {
            if (this.txt_partID.IsFocused)
            {
                PartID_Validate();
                return;
            }
        }

        private void kb_UndoClick()
        {
            this.txt_batchNo.Text = string.Empty;
            this.txt_currentWeight.Text = string.Empty;
            this.txt_startWeight.Text = string.Empty;
            this.txt_description.Text = string.Empty;
            this.txt_partID.Text = string.Empty;
            this.txt_sapcode.Text = string.Empty;
            this.txt_scrapWeight.Text = string.Empty;
            this.txt_slotIndex.Text = string.Empty;
            this.txt_slotID.Text = string.Empty;
            this.txt_scrapWeight.Text = string.Empty;
            this.cbb_returnType.Text = string.Empty;
            this.txt_scrapWeight.Text = string.Empty;
            this.cb_forceScrap.IsChecked = false;
            this.cbb_remark.Text = string.Empty;
            this.cbb_returnType.IsEnabled = false;
            this.btn_close.IsEnabled = true;
            this.btn_return.IsEnabled = false;
            this.btn_stop.IsEnabled = false;
            this.cbb_remark.IsEnabled = false;
            this.txt_expiryTime.Text = string.Empty;
            this.cbb_returnType.Background = System.Windows.Media.Brushes.White;
            this.txt_partID.Background = StaticRes.ColorBrushes.Linear_Green;
            kb.CurrentTextBox = this.txt_partID;
            this.txt_partID.Background = StaticRes.ColorBrushes.Linear_Green;
            time_start = System.DateTime.Now; //Meaning have action
            Transaction_ongoing = false;
            this.txt_partID.IsEnabled = true;
            this.txt_partID.Focus();
        }

        private void TextBox_To_Btn(System.Windows.Controls.TextBox txt_from, System.Windows.Controls.Button btn_to)
        {
            txt_from.IsEnabled = false;
            txt_from.Background = System.Windows.Media.Brushes.White;
            btn_to.IsEnabled = true;
            btn_to.Focus();
        }
        private void TextBox_Logic(System.Windows.Controls.TextBox txt_from, System.Windows.Controls.TextBox txt_to)
        {
            txt_from.IsEnabled = false;
            txt_from.Background = System.Windows.Media.Brushes.White;
            txt_to.IsEnabled = true;
            txt_to.Background = StaticRes.ColorBrushes.Linear_Green;
            txt_to.Focus();
        }
        #endregion

        #region Thread_Return
        void x_ReturnComplete(bool complete)
        {
            this.Dispatcher.Invoke(new Complete(DoComplete), new object[1] {complete});
        }
        private void DoComplete(bool complete)
        {      
            try
            {
                this.btn_return.IsEnabled = false;
                StaticRes.Global.IsOnProgress = false;
                if (complete)
                {
                    string status = cbb_returnType.Text;
                    string remark = string.Empty;
                    if (cbb_remark.Text.Length == 0)
                        remark = "";
                    else
                        remark = cbb_remark.Text;
                    //if (StaticRes.Global.updata)
                    //{
                    #region Update Server Database
                    if (cb_forceScrap.IsChecked == true)
                        status = StaticRes.Global.Status.Scrap;
                    while (!Logic.Transaction.Return.Local_Update(status, txt_batchNo.Text, txt_capacity.Text, txt_weighing.Text, txt_startWeight.Text, txt_description.Text,
                        txt_partID.Text, txt_sapcode.Text, txt_slotID.Text, txt_slotIndex.Text, remark, txt_thawingTime.Text, txt_readyTime.Text, txt_expiryTime.Text, txt_equipID.Text))
                    {
                        Event ge = new Event();
                        ge.EVENT_NAME = StaticRes.Global.Error_List.Update_database_failed;
                        ge.PROCESS_CODE = "Update DB";
                        ge.SLOT_NO = "S" + txt_slotID.Text + "L" + txt_slotIndex.Text;
                        AlarmWindow pp = new AlarmWindow(ge);
                        pp.ShowDialog();
                    }
                    #endregion
                    // }
                    Common.Reports.LogFile.Log("Return successful, Part ID : " + txt_partID.Text + " ; Sapcode : " + txt_sapcode.Text + " ; Weighing : " + txt_weighing.Text + " ; Batch No : " + txt_batchNo.Text + " ; User ID : " + txt_userID.Text);
                    if (status == StaticRes.Global.Status.Scrap)
                        MessageBox.Show("Scrap Successful !!\n报废成功！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    else
                        MessageBox.Show("Return Successful !!\n还料成功！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);

                    StaticRes.Global.Transaction_Continue = false;
                    kb_UndoClick();
                    pb.Value = 0;
                }
                else
                {
                    if (MessageBox.Show("Please put material into slot , then press 'OK' !!\n请把料放入槽中，点击‘OK’！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                    {
                        StaticRes.Global.IsOnProgress = false;
                        StaticRes.Global.Transaction_Continue = true;
                        x.Start_Return(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text), true);
                    }
                }
            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Return page error when complete thread : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void x_ReturnError(Event ge)
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
                btn_return.IsEnabled = true;
                StaticRes.Global.IsOnProgress = false;
                CloseComPort();
                Common.Reports.LogFile.Log("Close COM");
                AlarmWindow aw = new AlarmWindow(ge);
                if ((bool)aw.ShowDialog())
                {
                    btn_return.IsEnabled = false;
                    btn_stop.IsEnabled = true;
                    btn_close.IsEnabled = false;
                    InitComPort();
                    Common.Reports.LogFile.Log("Open COM");
                    Return_Process();
                }
                else
                {
                    if (StaticRes.Global.Process_Code.Returning != "R000")
                        StaticRes.Global.Need_Homing = true;
                    StaticRes.Global.Transaction_Continue = false;
                    StaticRes.Global.Process_Code.Returning = "R000";
                    Common.Reports.LogFile.Log("Terminate in return process , user : " + txt_userID.Text);
                    backClick();
                    this.Close();
                }
            }
            catch(Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void x_Step(string Event)
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
                Common.Reports.LogFile.Log("Return -- " + Event);
                pb.Value++;
            }
            catch
            {
            }
        }

        void Weighting_WeightComplete(float Weight_Return)
        {
            this.Dispatcher.Invoke(new WeightComplete(DoWeightComplete), new object[1] { Weight_Return });
        }
        private void DoWeightComplete(float Weight_Return)
        {
            if (Weight_Return >= 100)
            {
                string status = cbb_returnType.Text;
                string remark = string.Empty;
                if (cbb_remark.Text.Length == 0)
                    remark = "";
                else
                    remark = cbb_remark.Text;
                //if (StaticRes.Global.updata)
                //{
                    #region Update Server Database
                    if (cb_forceScrap.IsChecked == true)
                        status = StaticRes.Global.Status.Scrap;
                    while (!Logic.Transaction.Return.Local_Update(status, txt_batchNo.Text, txt_capacity.Text, (Weight_Return - 100).ToString(), txt_startWeight.Text, txt_description.Text,
                        txt_partID.Text, txt_sapcode.Text, txt_slotID.Text, txt_slotIndex.Text, remark, txt_thawingTime.Text, txt_readyTime.Text, txt_expiryTime.Text, txt_equipID.Text))
                    {
                        Event ge = new Event();
                        ge.EVENT_NAME = StaticRes.Global.Error_List.Update_database_failed;
                        ge.PROCESS_CODE = "Update DB";
                        ge.SLOT_NO = "S" + txt_slotID.Text + "L" + txt_slotIndex.Text;
                        AlarmWindow pp = new AlarmWindow(ge);
                        pp.ShowDialog();
                    }
                    #endregion
                //}
                Common.Reports.LogFile.Log("Return successful, Part ID : " + txt_partID.Text + " ; Sapcode : " + txt_sapcode.Text + " ; Weighing : " + txt_weighing.Text + " ; Batch No : " + txt_batchNo.Text + " ; User ID : " + txt_userID.Text);
                if (status == StaticRes.Global.Status.Scrap)
                    MessageBox.Show("Scrap Successful !!\n报废成功！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                else
                    MessageBox.Show("Return Successful !!\n还料成功！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);                
                StaticRes.Global.Transaction_Continue = false;
                kb_UndoClick();
                pb.Value = 0;
            }
            else
            {
                this.txt_weighing.Text = Weight_Return.ToString();
                Weight_Validate(Weight_Return.ToString());
            }
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
                            Weighting.Start_Weight(int.Parse(txt_capacity.Text), false, txt_scrapWeight.Text, txt_expiryTime.Text, (bool)cb_forceScrap.IsChecked, txt_syringeWeight.Text);
                        }
                    }
                    break;
                case "W1100":
                    {
                        this.txt_weighing.Text = Weight_Return.ToString();
                        if (MessageBox.Show("Please take out material from weight scale then press 'OK' !!\n请将秤台上的物料取出再点击‘OK’！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk) == MessageBoxResult.OK)
                        {
                            StaticRes.Global.IsOnProgress = false;
                            Weighting.Start_Weight(int.Parse(txt_capacity.Text), false, txt_scrapWeight.Text, txt_expiryTime.Text, (bool)cb_forceScrap.IsChecked, txt_syringeWeight.Text);
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
                btn_return.IsEnabled = true;
                AlarmWindow aw = new AlarmWindow(et);
                if ((bool)aw.ShowDialog())
                {
                    btn_close.IsEnabled = false;
                    btn_stop.IsEnabled = true;
                    btn_return.IsEnabled = false;
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    Weighting.Start_Weight(int.Parse(txt_capacity.Text), false, txt_scrapWeight.Text, txt_expiryTime.Text, (bool)cb_forceScrap.IsChecked, txt_syringeWeight.Text);
                }
                else
                {
                    Common.Reports.LogFile.Log("Terminate in return process , user : " + txt_userID);
                    if (StaticRes.Global.Process_Code.Returning != "R000")
                        StaticRes.Global.Need_Homing = true;
                    StaticRes.Global.Transaction_Continue = false;
                    StaticRes.Global.Process_Code.Returning = "R000";
                    backClick();
                    this.Close();
                }
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Terminate in Returning process , user :" + ee.Message);
                MessageBox.Show(ee.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);

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
            if (txt_partID.IsFocused && txt_partID.IsEnabled)
            {
                this.txt_partID.Text = Handle_Scanner.ReadExisting().Replace("\r", "").Replace("\n", "");
                PartID_Validate();
            }
        }
        #endregion
    }
}
