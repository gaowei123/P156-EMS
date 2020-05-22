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

namespace EMS
{
	/// <summary>
	/// HomingPage.xaml 的交互逻辑
	/// </summary>
	public partial class HomingPage : Window
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
        private event HDClickEventHandler homingSuccessful;
        public event HDClickEventHandler HDClick
        {
            add
            {
                homingSuccessful += value;
            }
            remove
            {
                homingSuccessful -= value;
            }
        }
        #endregion

        delegate void HomingComplete();
        delegate void HomingError(ObjectModule.Local.Event ge);
        delegate void HomingStep(string Event);
        private bool _bAutoHoming = false; //2019 08 02 by Wei LiJia for Auto Homing before transaction
        public HardwareControl.Homing x = new HardwareControl.Homing();

		public HomingPage(bool AutoHomingFlag )
		{
			this.InitializeComponent();
            this.eventList.ItemsSource = new List<string>() ;
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
            x.HomeComplete += new HardwareControl.Homing.HomeCompleteEventHandler(x_HomeComplete);
            x.HomeError += new HardwareControl.Homing.HomeErrorEventHandler(x_HomeError);
            x.Step += new HardwareControl.Homing.StepEventHandler(x_Step);
            pb.Value = 0;
            _bAutoHoming = AutoHomingFlag; //2019 08 02 by Wei LiJia for Auto Homing before transaction
            
        }

        //2019 08 02 by Wei LiJia for Auto Homing before transaction
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(_bAutoHoming)
            {
                btn_startHoming_Click(null, new System.Windows.RoutedEventArgs());
            }
        }
        private void btn_startHoming_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StaticRes.Global.Hardware_Connection)
                {
                    Common.Reports.LogFile.Log("Start Homing");
                    eventList.ItemsSource = new List<string>();
                    StaticRes.Global.IsOnProgress = false;
                    this.btn_startHoming.IsEnabled = false;
                    this.btn_stopHoming.IsEnabled = true;
                    this.btn_Close.IsEnabled = false;
                    StaticRes.Global.Transaction_Continue = true;
                    pb.Maximum = x.Start_Home(false);
                }
                else
                    MessageBox.Show("No hardware connection !! \n没有硬件链接！！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Homing page error when click start homing :" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

		private void btn_stopHoming_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            try
            {
                Common.Reports.LogFile.Log("Stop Homing");
                StaticRes.Global.Transaction_Continue = false;
                this.btn_Close.IsEnabled = true;
                this.btn_stopHoming.IsEnabled = false;
                this.btn_startHoming.IsEnabled = true;
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Homing page error when click stop homing :" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
		}

        private void btn_Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StaticRes.Global.Transaction_Continue = false;
            if (StaticRes.Global.Process_Code.Homing != "H000")
                StaticRes.Global.Need_Homing = true;
            StaticRes.Global.Process_Code.Homing = "H000";
            backClick();
            this.Close();
        }

        void x_HomeComplete()
        {
            this.Dispatcher.Invoke(new HomingComplete(DoHomingComplete), new object[0]);
        }
        private void DoHomingComplete()
        {
            try
            {
                MessageBox.Show("Homing Complete !!\n复位完成！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                StaticRes.Global.Transaction_Continue = false;
                StaticRes.Global.Homing_Complete = true;
                homingSuccessful();
                backClick();
                this.Close();
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Homing page error when homing complete :" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void x_HomeError(ObjectModule.Local.Event ge)
        {
            this.Dispatcher.Invoke(new HomingError(DoHomingError), new object[1] { ge });
        }
        private void DoHomingError(ObjectModule.Local.Event ge)
        {
            try
            {
                ge.PART_ID = "NA";
                ge.SLOT_NO = "NA";
                AlarmWindow aw = new AlarmWindow(ge);
                if ((bool)aw.ShowDialog())
                {
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    this.btn_startHoming.IsEnabled = false;
                    this.btn_stopHoming.IsEnabled = true;
                    this.btn_Close.IsEnabled = false;
                    x.Start_Home(true);
                }
                else
                {
                    StaticRes.Global.Transaction_Continue = false;
                    if (StaticRes.Global.Process_Code.Homing != "H000")
                        StaticRes.Global.Need_Homing = true;
                    StaticRes.Global.Process_Code.Homing = "H000";
                    backClick();
                    this.Close();
                    StaticRes.Global.Need_Homing = true;
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void x_Step(string Event)
        {
            this.Dispatcher.Invoke(new HomingStep(DoHomingStep), new object[1] { Event });
        }
        private void DoHomingStep(string Event)
        {
            try
            {
                List<string> list = new List<string>();
                list.Add(Event);
                foreach (string x in eventList.Items)
                {
                    list.Add(x);
                }
                eventList.ItemsSource = list;
                Common.Reports.LogFile.Log("Homing step -" + Event);
                pb.Value++;
            }
            catch
            {
            }
        }

    
    }
}