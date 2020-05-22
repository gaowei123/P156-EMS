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


        public bool onGoing { get; set; }
        public readonly double mixTime = 0;
        public double runningTime = 0;
        private BackgroundWorker bgwMixing;



        public Mix()
        {
            InitializeComponent();


            runningTime = 0;
            onGoing = false;
            this.btn_startMix.IsEnabled = false;
            InitMaterialInfo();






            try
            {
                DataTable dt = DataProvider.Local.Configure.Select();
                if (dt == null || dt.Rows.Count == 0)
                    mixTime = 0;
                DataRow[] drArr = dt.Select("NAME = 'Mix_Time'");
                if (drArr.Length == 0)
                    mixTime = 0;
                else
                    mixTime = double.Parse(drArr[0]["VALUE"].ToString());
            }
            catch (Exception)
            {
                mixTime = 0;
            }
            



            //后台计时, 显示进度条
            bgwMixing = new BackgroundWorker();
            bgwMixing.WorkerReportsProgress = true;
            bgwMixing.WorkerSupportsCancellation = true;
            bgwMixing.DoWork += BackgroundWorker_DoWork;
            bgwMixing.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            bgwMixing.ProgressChanged += BgwMixing_ProgressChanged;

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


            

            //检测 是否把锡膏拿了出来.  
            bool isMaterialIn = Hardware.IO_LIST.Input.X207_Mix_Material_In();
            while (isMaterialIn)
            {
                MessageBox.Show("Mixing complete, please open door and take out solder paste!");
                
                //重新检下测感应器状态.
                isMaterialIn = Hardware.IO_LIST.Input.X207_Mix_Material_In();
            }





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
            this.txt_partID_input.Focus();

        }

        
        #endregion

        

        #region UI Event
        private void txt_partID_input_KeyDown(object sender, KeyEventArgs e)
        {
            //只有回车后触发.
            if (e.Key != Key.Enter)
                return;

          
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
                MessageBoxResult result = MessageBox.Show("This solder paste is not got from EMS, are you still need to mix?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
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

            
            this.btn_startMix.IsEnabled = true;
            this.btn_startMix.Focus();
        }
        
        private void btn_startMix_Click(object sender, RoutedEventArgs e)
        {

            //检测锡膏是否放入
            bool isMaterialIn = Hardware.IO_LIST.Input.X207_Mix_Material_In();
            if (!isMaterialIn)
            {
                MessageBox.Show("Please put in the solder paste!");
                return;
            }


            //检测盖子是否盖上
            bool isCoverd = Hardware.IO_LIST.Input.X206_Mix_Cover_On();
            if (!isCoverd)
            {
                MessageBox.Show("Please close the door!");
                return;
            }



            //触发IO,接通继电器使搅拌机通电.
            bool result = Hardware.IO_LIST.Output.Y204_MixPower_On();
            while (!result)
            {
                MessageBox.Show("Mixer power error, please check!", "Error");
                result = Hardware.IO_LIST.Output.Y204_MixPower_On();
            }




            this.btn_startMix.IsEnabled = false;
            this.btn_stop.IsEnabled = false;
            this.txt_partID_input.IsEnabled = false;


            //后台计时, 显示进度条.
            bgwMixing.RunWorkerAsync();

        }
        
        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            //没有正在运行的情况下关闭, 初始化界面
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
            {
                cmdList.Add(trackBLL.UpdateCommand(trackModel));
            }

            if (hisModel != null)
            {
                cmdList.Add(hisBLL.AddCommand(hisModel));
            }
       
            
            cmdList.Add(mixBLL.AddCommand(mixModel));



            return Common.DB.SqlDB.SetData_Rollback(cmdList, Common.DB.Connection.SqlServer.EMS);
        }


    }
}
