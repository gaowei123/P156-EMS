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

namespace EMS.MaintMode
{
    /// <summary>
    /// EngineerPage.xaml 的交互逻辑
    /// </summary>
    ///         
    public partial class MaintPage : Window
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
        delegate void HomingComplete();
        delegate void HomingError(ObjectModule.Local.Event lh);
        delegate void HomingStep(string Event);
        public HardwareControl.Homing x = new HardwareControl.Homing();
		
        public MaintPage()
        {
            InitializeComponent();
            x.HomeComplete += new HardwareControl.Homing.HomeCompleteEventHandler(x_HomeComplete);
            x.HomeError += new HardwareControl.Homing.HomeErrorEventHandler(x_HomeError);
            x.Step += new HardwareControl.Homing.StepEventHandler(x_Step);
            try
            {
                HardwareControl.Motion_Control.Rotary_Motion_Stop();
            }
            catch
            {
            }
            StaticRes.Global.Need_Homing = true;
        }

        private void btn_close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StaticRes.Global.Scale_Open == true)
                {
                    MessageBox.Show("Please close weighing scale COM port first before return !!\n请先关闭电子称！！", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                Hardware.IO_LIST.Output.Y112_Thawing_Cover_Close();
                Common.Reports.LogFile.Log("Exit maintenance mode  , user : " + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                StaticRes.Global.Current_User.USER_ID = string.Empty;
                StaticRes.Global.Need_Homing = true;
                backClick();
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_home_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StaticRes.Global.Hardware_Connection)
                {
                    StaticRes.Global.IsOnProgress = false;
                    StaticRes.Global.Transaction_Continue = true;
                    tab.IsEnabled = false;
                    pb.Value = 0;
                    pb.Maximum = x.Start_Home(false);
                }
                else
                    MessageBox.Show("No hardware connection !!\n没有硬件链接", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            pb.Value++;
        }

        void x_HomeError(ObjectModule.Local.Event ge)
        {
            this.Dispatcher.Invoke(new HomingError(DoHomingError), new object[1] { ge });
        }

        void DoHomingError(ObjectModule.Local.Event ge)
        {
            try
            {
                MessageBox.Show(StaticRes.Global.CurrentLanguageRes[ge.EVENT_NAME + "_Description"].ToString());
                StaticRes.Global.Process_Code.Homing = "H000";
                tab.IsEnabled = true;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void x_HomeComplete()
        {
            this.Dispatcher.Invoke(new HomingComplete(DoHomingComplete), new object[0]);
        }

        private void DoHomingComplete()
        {
            tab.IsEnabled = true;
            pb.Value = 0;
            StaticRes.Global.Transaction_Continue = false;
            MessageBox.Show("Homing Completed !!\n复位完成！！");
            return;
        }
    }
}
