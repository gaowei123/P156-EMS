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
    public partial class ExpiryWindow : Window
    {
        public MainWindow xx;
        ObjectModule.Local.Tracking tracking = new ObjectModule.Local.Tracking();
        DispatcherTimer tm = new DispatcherTimer();
        private bool buzzer_on = true;

        public ExpiryWindow()
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
            if (!buzzer_on)
            {
                Hardware.IO_LIST.Output.Y103_Tower_Buzzer_On();
                buzzer_on = true;
                GC.Collect();
                return;
            }
        }

        public ExpiryWindow(ObjectModule.Local.Tracking tk)
        {
            InitializeComponent();
            try
            {
                tm.Tick += new EventHandler(Timer);
                tm.Interval = TimeSpan.FromSeconds(0.5);
                tm.Start();
                tracking = tk;
                this.txt_batchNo.Content = tk.BATCH_NO;
                this.txt_expiryTime.Content = tk.EXPIRY_DATETIME.ToString();
                this.txt_partID.Content = tk.PART_ID.ToString();
                this.txt_readyTime.Content = tk.READY_DATETIME.ToString();
                this.txt_sapcode.Content = tk.SAPCODE;
                this.txt_thawingTime.Content = tk.THAWING_DATETIME.ToString();
                this.txt_mcID.Content = tk.EQUIP_ID.ToString();
                HardwareControl.IO_Control.Alarm_Tower_Light_Setting();
                Common.Reports.LogFile.Log("Expiry Alarm : " + this.txt_partID.Content.ToString() + ",Machine ID" + this.txt_mcID.Content.ToString() + "");
            }
            catch (Exception ff)
            {
                MessageBox.Show(ff.Message);
            }
        }

        private void btn_buzzerOff_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            tm.Stop();
            Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off();
        }

        private void btn_remindLate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                tm.Stop();
                Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off();
                HardwareControl.IO_Control.Yellow_Tower_Light_Setting();
                //this.DialogResult = true;
                this.Close();
            }
            catch (Exception ee)
            {
                tm.Stop();
               // this.DialogResult = true;
                this.Close();
                Common.Reports.LogFile.Log("Alarm page Error -- " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_print_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Logic.Print.EMS(tracking.PART_ID, tracking.EQUIP_ID, tracking.EXPIRY_DATETIME.ToString(), tracking.THAWING_DATETIME.ToString(),
                    tracking.READY_DATETIME.ToString(), tracking.SAPCODE);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
