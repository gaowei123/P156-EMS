using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Data;
using System.Data.SqlClient;

namespace EMS.Transaction
{
    /// <summary>
    /// Mix.xaml 的交互逻辑
    /// </summary>
    public partial class Mix : Window
    {

      
        #region user info
        public string userID
        {
            get { return userID; }
            set { this.txt_userID.Text = value; }
        }
        public string userName
        {
            get { return userName; }
            set { this.txt_userName.Text = value; }
        }
        public string userGroup
        {
            get { return userGroup; }
            set { this.txt_userGroup.Text = value; }
        }
        public string department
        {
            get { return department; }
            set { this.txt_userDepartment.Text = value; }
        }
        #endregion


        //判断搅拌机是否在运行
        public bool onGoing { get; set; }

        //搅拌总时长, 在config表中设定, 单位秒
        public readonly double mixTime = 0;

        //搅拌开始后计时 时长, 单位秒
        public double runningTime = 0;

        private BackgroundWorker bgwMixing;









        public Mix()
        {
            InitializeComponent();

            try
            {
                //initalize UI & Parameters
                runningTime = 0;
                onGoing = false;
                this.btn_startMix.IsEnabled = false;
                InitMaterialInfo();

                
                //initalize mix time
                BLL.Configure confBLL = new BLL.Configure();
                Model.Configure confModel = confBLL.GetModel("Mix_Time");
                mixTime = confModel == null ? 0 : double.Parse(confModel.VALUE);
            
                
                //后台计时, 显示进度条
                bgwMixing = new BackgroundWorker();
                bgwMixing.WorkerReportsProgress = true;
                bgwMixing.WorkerSupportsCancellation = true;
                bgwMixing.DoWork += BackgroundWorker_DoWork;
                bgwMixing.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
                bgwMixing.ProgressChanged += BgwMixing_ProgressChanged;


                kb.CurrentTextBox = this.txt_partID_input;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Initalizing error, Exception:" + ex.ToString());
                Common.Reports.LogFile.Log("[EMS.Transaction.Mix], Initalize error, exception:" + ex.ToString());
            }
        }


        #region  backgroundworker

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            onGoing = true;

            while (runningTime < mixTime)
            {
                runningTime = runningTime + 1;
                System.Threading.Thread.Sleep(1000);
                bgwMixing.ReportProgress(1);
            }
        }


        private void BgwMixing_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pb.Value = runningTime / mixTime * 100;
        }
        

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            ShowWindow();




            //检测 是否把epoxy拿了出来.  
            //bool isMaterialIn = Hardware.IO_LIST.Input.X207_Mix_Material_In();
            //while (isMaterialIn)
            //{
            //    MessageBox.Show("Mixing complete, please open door and take out epoxy!");

            //    //重新检下测感应器状态.
            //    isMaterialIn = Hardware.IO_LIST.Input.X207_Mix_Material_In();
            //}


            //trigger again to stop mixer
            bool result = Hardware.IO_LIST.Output.Y204_MixPower_On();
            System.Threading.Thread.Sleep(100);
            Hardware.IO_LIST.Output.Y204_MixPower_Off();


            MessageBox.Show("Mixing complete, please take out epoxy\n搅拌完成,请取出银浆");





            //更新数据库
            Model.MixHistory mixModel = new Model.MixHistory();
            mixModel.PART_ID = this.txt_partID_input.Text;
            mixModel.MIX_TIME = decimal.Parse(mixTime.ToString());
            mixModel.MIX_DATETIME = DateTime.Now;
            mixModel.MIX_BY = this.txt_userID.Text;
            mixModel.REMARK = "";

            BLL.Tracking trackBLL = new BLL.Tracking();
            Model.Tracking trackModel = trackBLL.GetModel(this.txt_partID.Text);
            if (trackModel != null)
            {
                trackModel.STATUS = StaticRes.Global.Status.Unload;
                trackModel.UPDATED_TIME = DateTime.Now;
                trackModel.ACTION = "Mix";
            }
            
            BLL.History hisBLL = new BLL.History();
            Model.History hisModel = hisBLL.CopyTrackModel(trackModel);
            
            if (!Update(trackModel, hisModel, mixModel))
            {
                MessageBox.Show("Update database fail!");
                Common.Reports.LogFile.Log("[Mix] [BackgroundWorker_RunWorkerCompleted] - Update database fail partID:" + this.txt_partID.Text);
            }





            //init 变量
            onGoing = false;
            runningTime = 0;
            this.pb.Value = 0;



            //init ui
            InitMaterialInfo();
            this.txt_partID_input.Text = "";
            this.txt_partID_input.IsEnabled = true;
            this.txt_partID_input.Focus();
            this.txt_partID_input.Background = StaticRes.ColorBrushes.Linear_Green;
            
            

        }

        
        #endregion

        

        #region UI Event
        private void txt_partID_input_KeyDown(object sender, KeyEventArgs e)
        {
            //只有回车后触发.
            if (e.Key != Key.Enter)
                return;

            validation();


        }



        public  void validation()
        {
            string partID = this.txt_partID_input.Text;
            if (string.IsNullOrEmpty(partID))
            {
                MessageBox.Show("Part ID can not be empty!");
                this.txt_partID.Focus();
                return;
            }



            //查询tracking
            BLL.Tracking trackBLL = new BLL.Tracking();
            Model.Tracking trackModel = trackBLL.GetModel(partID);
            if (trackModel == null || trackModel.STATUS != StaticRes.Global.Status.PendingMix)
            {
                MessageBoxResult result = MessageBox.Show("This epoxy is not got from EMS, are you still need to mix?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel || result == MessageBoxResult.None)
                    return;
            }
            else
            {
                this.txt_partID.Text = trackModel.PART_ID;
                this.txt_description.Text = trackModel.DEPARTMENT;
                this.txt_thawingTime.Text = trackModel.THAWING_DATETIME.Value.ToString("yyyy-MM-dd HH:mm:ss");
                this.txt_readyTime.Text = trackModel.READY_DATETIME.Value.ToString("yyyy-MM-dd HH:mm:ss");
                this.txt_expiryTime.Text = trackModel.EXPIRY_DATETIME.Value.ToString("yyyy-MM-dd HH:mm:ss");
                this.txt_currentWeight.Text = trackModel.CURRENT_WEIGHT.ToString();
                this.txt_capacity.Text = trackModel.CAPACITY.ToString();
                this.txt_emptySyringeWeight.Text = trackModel.EMPTY_SYRINGE_WEIGHT.ToString();
            }


            this.txt_partID_input.Background = System.Windows.Media.Brushes.White;
            this.btn_startMix.IsEnabled = true;
            this.btn_startMix.Focus();
        }
        
        private void btn_startMix_Click(object sender, RoutedEventArgs e)
        {

            //检测epoxy是否放入  --无法安装感应器, 屏蔽
            //bool isMaterialIn = Hardware.IO_LIST.Input.X207_Mix_Material_In();
            //if (!isMaterialIn)
            //{
            //    MessageBox.Show("Please put in the epoxy!");
            //    return;
            //}


            //检测盖子是否盖上  --无法安装感应器, 屏蔽
            //bool isCoverd = Hardware.IO_LIST.Input.X206_Mix_Cover_On();
            //if (!isCoverd)
            //{
            //    MessageBox.Show("Please close the door!");
            //    return;
            //}


            MessageBox.Show("Please open the cover and put in the epoxy\n请打开门并放入银浆");


            //触发IO,接通继电器使搅拌机通电.
            bool result = Hardware.IO_LIST.Output.Y204_MixPower_On();
            System.Threading.Thread.Sleep(100);
            Hardware.IO_LIST.Output.Y204_MixPower_Off();

            while (!result)
            {
                MessageBox.Show("Mixer power error, please check!", "Error");
                result = Hardware.IO_LIST.Output.Y204_MixPower_On();
            }

            

            this.btn_startMix.IsEnabled = false;
            //this.btn_stop.IsEnabled = false;
            this.txt_partID_input.IsEnabled = false;


            //开启后台线程.
            bgwMixing.RunWorkerAsync();

        }
        
        //private void btn_stop_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            //没有正在运行的情况下 初始化界面
            if (!onGoing)
            {
                InitMaterialInfo();
                InitUserInfo();

                this.txt_partID_input.Text = "";
                this.btn_startMix.IsEnabled = false;
            }

            this.Visibility = Visibility.Hidden;
            this.ShowInTaskbar = false;
        }

        #endregion

        

        private void InitMaterialInfo()
        {           
            this.txt_partID.Text = "";
            this.txt_description.Text = "";
            this.txt_thawingTime.Text = "";
            this.txt_readyTime.Text = "";
            this.txt_expiryTime.Text = "";
            this.txt_currentWeight.Text = "";
            this.txt_capacity.Text = "";
            this.txt_emptySyringeWeight.Text = "";
        }

        private void InitUserInfo()
        {
            this.txt_userID.Text = "";
            this.txt_userName.Text = "";
            this.txt_userGroup.Text = "";
            this.txt_userDepartment.Text = "";
        }


   
        
        public void ShowWindow()
        {
            this.Show();
            this.Focus();
            this.Visibility = Visibility.Visible;
            this.ShowInTaskbar = true;

            if (!onGoing)
            {
                this.txt_partID_input.Background = StaticRes.ColorBrushes.Linear_Green;
                this.txt_partID_input.Focus();

                kb.CurrentTextBox = this.txt_partID_input;
            }
        }
                
        //重写onclosing事件, 避免通过window系统关闭窗口后,再打开界面导致报错
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close    
            this.Hide();      // Programmatically hides the window
        }

       

        private bool Update(Model.Tracking trackModel, Model.History hisModel, Model.MixHistory mixModel )
        {
            BLL.MixHistory mixBLL = new BLL.MixHistory();
            BLL.Tracking trackBLL = new BLL.Tracking();
            BLL.History hisBLL = new BLL.History();

            List<SqlCommand> cmdList = new List<SqlCommand>();

            if (trackModel != null)
                cmdList.Add(trackBLL.UpdateCommand(trackModel));

            if (hisModel != null)
                cmdList.Add(hisBLL.AddCommand(hisModel));
            
            cmdList.Add(mixBLL.AddCommand(mixModel));



            return Common.DB.SqlDB.SetData_Rollback(cmdList, Common.DB.Connection.SqlServer.EMS);
        }


     

        private void kb_EnterClick()
        {
            if (this.txt_partID_input.IsFocused && this.txt_partID_input.IsEnabled)
            {
                validation();
                return;
            }
        }


    }
}
