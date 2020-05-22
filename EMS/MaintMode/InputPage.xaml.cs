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
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace EMS
{
	/// <summary>
	/// InputPage.xaml 的交互逻辑
	/// </summary>
	public partial class InputPage : UserControl
	{
        DispatcherTimer tm = new DispatcherTimer();

		public InputPage()
		{
			this.InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
            this.btn_Left.IsEnabled = false;
            this.btn_Right.IsEnabled = true;
            if (StaticRes.Global.Hardware_Connection)
                Timer_Strat();
            else
                MessageBox.Show("Can not detect PCI-1756/PCI1733 PCB board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

        public void Timer_Strat()
        {
            tm.Tick += new EventHandler(Timer);
            tm.Interval = TimeSpan.FromSeconds(0.5);
            tm.Start();
        }

        void Timer(object sender, EventArgs e)
        {
            InputTemp[] IO_Input_Status = new InputTemp[] { i_1_1,i_1_2,i_1_3,i_1_4,i_1_5,i_1_6,i_1_7,i_1_8,
                                                             i_1_9,i_1_10,i_1_11,i_1_12,i_1_13,i_1_14,i_1_15,i_1_16,
                                                             i_1_17,i_1_18,i_1_19,i_1_20,i_1_21,i_1_22,i_1_23,i_1_24,
                                                             i_1_25,i_1_26,i_1_27,i_1_28,i_1_29,i_1_30,i_1_31,i_1_32,
                                                             i_2_1,i_2_2,i_2_3,i_2_4,i_2_5,i_2_6,i_2_7,i_2_8,
                                                             i_2_9,i_2_10,i_2_11,i_2_12,i_2_13,i_2_14,i_2_15,i_2_16,
                                                             i_2_17,i_2_18,i_2_19,i_2_20,i_2_21,i_2_22,i_2_23,i_2_24,
                                                             i_2_25,i_2_26,i_2_27,i_2_28,i_2_29,i_2_30,i_2_31,i_2_32};
            int[] PCI_1756_Status = Hardware.IO_LIST.Input.IO_1756_Status();
            int[] PCI_1733_Status = Hardware.IO_LIST.Input.IO_1733_Status();
            for (int i = 0; i < 64; i++)
            {
                if (i < 32)
                {
                    if (PCI_1756_Status[i] == 0)
                        IO_Input_Status[i].Value = false;
                    else
                        IO_Input_Status[i].Value = true;
                }
                else
                {
                    if (PCI_1733_Status[i-32] == 0)
                        IO_Input_Status[i].Value = false;
                    else
                        IO_Input_Status[i].Value = true;
                }
            }
            GC.Collect();
        }

		private void btn_Left_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lb_title.Content.ToString() == "Input-2")
            {
                this.btn_Left.IsEnabled = false;
                this.btn_Right.IsEnabled = true;
                this.sb_left_3.Storyboard.Begin();
                this.lb_title.Content = "Input-1";
            }
		}

		private void btn_Right_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            if (lb_title.Content.ToString() == "Input-1")
            {
                this.btn_Left.IsEnabled = true;
                this.btn_Right.IsEnabled = false;
                this.sb_right_1.Storyboard.Begin();
                this.lb_title.Content = "Input-2";
            }
		}
	}
}