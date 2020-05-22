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
using ObjectModule.Local;
using System.Data;

namespace EMS
{
	/// <summary>
	/// MotionPage.xaml 的交互逻辑
	/// </summary>
	public partial class LoadSlotTeach : UserControl
	{
        DispatcherTimer tm = new DispatcherTimer();
        public LoadSlotTeach()
		{
			this.InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
            nkb.CurrentTextBox = this.txt_distance;
            this.dg_list.ItemsSource = DataProvider.Local.Slot_Position.Select.All().DefaultView;
            if (StaticRes.Global.Hardware_Connection)
                Timer_Strat();
		}

        public void Timer_Strat()
        {
            tm.Tick += new EventHandler(Timer);
            tm.Interval = TimeSpan.FromSeconds(1);
            tm.Start();
        }

        void Timer(object sender, EventArgs e)
        {
            try
            {
                this.txt_currentPosition.Text = HardwareControl.Motion_Control.Got_Rotary_Position().ToString();
                if (HardwareControl.Motion_Control.Rotary_Homing_Sensor_On())
                    this.ep_rotary_homing.Fill = StaticRes.ColorBrushes.Linear_Green;
                else
                    this.ep_rotary_homing.Fill = StaticRes.ColorBrushes.Linear_Silver;
                GC.Collect();
            }
            catch
            {
            }
        }

        private void btn_save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure to save the params of Slot-\n确定保存修改的数据吗？" + this.txt_slotID.Text + ",Index-" + this.txt_slotIndex.Text + " ?",
                    "Confirm", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.Cancel)
                    return;
                if (HardwareControl.Initial_Hardware.Save_Slot_Parameter(int.Parse(txt_slotID.Text), int.Parse(txt_slotIndex.Text), int.Parse(this.txt_position.Text)))
                {
                    this.dg_list.ItemsSource = DataProvider.Local.Slot_Position.Select.All().DefaultView;
                    MessageBox.Show("Save successful !!\n保存成功！！");
                }
                else
                    MessageBox.Show("Save failed !!\n保存失败！！");

            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.Message + "");
            }
        }

        private void btn_moveSlot_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (!StaticRes.Global.Need_Homing)
                {
                    HardwareControl.Motion_Control.Rotary_MoveTo_Slot(int.Parse(txt_slotID.Text));
                }
                else
                {
                    MessageBox.Show("Please do homing first !!\n请先复位！！");
                }
            }
            else
                MessageBox.Show("No hardware connection !!\n没有硬件链接！！");
        }

        private void btn_moveRight_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                HardwareControl.Motion_Control.Motion_Speed_Checking();
                int distance = 0;
                if (rb_1000.IsChecked == true)
                    distance = 1000;
                else if (rb_10000.IsChecked == true)
                    distance = 10000;
                if (StaticRes.Global.Hardware_Connection)
                    HardwareControl.Motion_Control.Rotary_Move(distance);
                else
                    MessageBox.Show("No hardware connection !!\n没有硬件链接！！");
                this.txt_position.Text = (int.Parse(this.txt_position.Text) + distance).ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_moveLeft_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                HardwareControl.Motion_Control.Motion_Speed_Checking();
                int distance = 0;
                if (rb_1000.IsChecked == true)
                    distance = -1000;
                else if (rb_10000.IsChecked == true)
                    distance = -10000;
                if (StaticRes.Global.Hardware_Connection)
                    HardwareControl.Motion_Control.Rotary_Move(distance);
                else
                    MessageBox.Show("No hardware connection !!\n没有硬件链接！！");
                this.txt_position.Text = (int.Parse(this.txt_position.Text) + distance).ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_moveLeft_Distance_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StaticRes.Global.Hardware_Connection)
                {
                    HardwareControl.Motion_Control.Motion_Speed_Checking();
                    int Distance = int.Parse(this.txt_distance.Text);
                    HardwareControl.Motion_Control.Rotary_Move(Distance); 
                }
                else
                    MessageBox.Show("No hardware connection !!\n没有硬件连接！！");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_moveRight_Distance_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StaticRes.Global.Hardware_Connection)
                {
                    HardwareControl.Motion_Control.Motion_Speed_Checking();
                    int Distance = int.Parse(this.txt_distance.Text);
                    HardwareControl.Motion_Control.Rotary_Move(-Distance); 
                }
                else
                    MessageBox.Show("No hardware connection !!\n没有硬件连接！！");
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (StaticRes.Global.Hardware_Connection)
                {
                    HardwareControl.Motion_Control.Rotary_Motion_Stop();
                }
                else
                    MessageBox.Show("No hardware connection !!\n没有硬件连接！！");
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
                HardwareControl.Motion_Control.Motion_Speed_Checking();
                HardwareControl.Motion_Control.Rotary_Move(100000);
                HardwareControl.Motion_Control.Motion_Speed_Checking();
                HardwareControl.Motion_Control.Rotary_Motor_Homing();
                HardwareControl.Motion_Control.Motion_Speed_Checking();
                HardwareControl.Motion_Control.Rotary_Move(-StaticRes.Global.System_Setting.Rotary_Homing_Pitch);
                HardwareControl.Motion_Control.Motion_Speed_Checking();
                if (HardwareControl.Motion_Control.Rotary_Homing_Sensor_On())
                {
                    MessageBox.Show("Homing Successful !!\n复位成功！！");
                    HardwareControl.Motion_Control.Set_Rotary_Zero_Position();
                }
                else
                    MessageBox.Show("Homing Failed !!\n复位失败！！");
                StaticRes.Global.Need_Homing = false;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dg_list_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                DataRowView selectItem = this.dg_list.SelectedItem as DataRowView;
                if (selectItem == null)
                    return;
                this.txt_slotID.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SLOT_ID"].ToString();
                this.txt_slotIndex.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SLOT_INDEX"].ToString();
                this.txt_position.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["POSITION"].ToString();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
		
	}
}