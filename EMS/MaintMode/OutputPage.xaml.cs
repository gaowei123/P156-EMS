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

namespace EMS
{
	/// <summary>
	/// OutputPage.xaml 的交互逻辑
	/// </summary>
	public partial class OutputPage : UserControl
	{

		public OutputPage()
		{
			this.InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);



            #region Default Status
            o_1_1.Value = false; //Y100---Green Light
            o_1_2.Value = false; //Y101---Yellow Light
            o_1_3.Value = false; //Y102---Red Light
            o_1_4.Value = false; //Y103---Buzzer
            o_1_5.Value = false; //Y104---Gate A Open/Close
            o_1_6.Value = false; //Y105---Gate B Open/Close
            o_1_7.Value = false; //Y106---Gate C Open/Close
            o_1_8.Value = false; //Y107---Gate D Open/Close
            o_1_9.Value = false; //Y108---Loading Door Open/Close
            o_1_10.Value = false; //Y109---Weighing Door Open/Close
            o_1_11.Value = false; //Y110---Weighing Tray FWD/BWD
            o_1_12.Value = false; //Y111---Scrap Bin Drawer 1/2 Open
            o_1_13.Value = false; //Y112---Open Thawing Cover
            o_1_14.Value = false; //Y113---Open Scrap Bin Drawer 1
            o_1_15.Value = false; //Y114---Open Scrap Bin Drawer 2
            o_1_16.Value = false; //Y115---Spare
            o_1_17.Value = false; //Y200---Ejecotr A Up/Down
            o_1_18.Value = false; //Y201---Ejecotr B Up/Down
            o_1_19.Value = false; //Y202---Ejecotr C Up/Down
            o_1_20.Value = false; //Y203---Ejecotr D Up/Down
            o_1_21.Value = false; //Y204---Spare
            o_1_22.Value = false; //Y205---Spare
            o_1_23.Value = false; //Y206---Spare
            o_1_24.Value = false; //Y207---Spare
            o_1_25.Value = false; //Y208---Spare
            o_1_26.Value = false; //Y209---Spare
            o_1_27.Value = false; //Y210---Spare
            o_1_28.Value = false; //Y211---Spare
            o_1_29.Value = false; //Y212---Spare
            o_1_30.Value = false; //Y213---Spare
            o_1_31.Value = false; //Y214---Spare
            o_1_32.Value = false; //Y215---Spare
            #endregion
        }

        private void o_1_1_OutputClick() //Y100---Green Light
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_1.Value)
                {
                    Hardware.IO_LIST.Output.Y100_Tower_Light_Green_Off();
                    o_1_1.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y100_Tower_Light_Green_On();
                    o_1_1.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756  Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_2_OutputClick() //Y101---Yellow Light
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_2.Value)
                {
                    Hardware.IO_LIST.Output.Y101_Tower_Light_Yellow_Off();
                    o_1_2.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y101_Tower_Light_Yellow_On();
                    o_1_2.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_3_OutputClick() //Y102---Red Light
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_3.Value)
                {
                    Hardware.IO_LIST.Output.Y102_Tower_Light_Red_Off();
                    o_1_3.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y102_Tower_Light_Red_On();
                    o_1_3.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_4_OutputClick() //Y103---Buzzer
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_4.Value)
                {
                    Hardware.IO_LIST.Output.Y103_Tower_Buzzer_Off();
                    o_1_4.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y103_Tower_Buzzer_On();
                    o_1_4.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_5_OutputClick() //Y104---Gate A Open/Close
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_5.Value)
                {
                    Hardware.IO_LIST.Output.Y104_Gate_A_Close();
                    o_1_5.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y104_Gate_A_Open();
                    o_1_5.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_6_OutputClick() //Y105---Gate B Open/Close
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_6.Value)
                {
                    Hardware.IO_LIST.Output.Y105_Gate_B_Close();
                    o_1_6.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y105_Gate_B_Open();
                    o_1_6.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_7_OutputClick() //Y106---Gate C Open/Close
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_7.Value)
                {
                    Hardware.IO_LIST.Output.Y106_Gate_C_Close();
                    o_1_7.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y106_Gate_C_Open();
                    o_1_7.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_8_OutputClick() //Y107---Gate D Open/Close
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_8.Value)
                {
                    Hardware.IO_LIST.Output.Y107_Gate_D_Close();
                    o_1_8.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y107_Gate_D_Open();
                    o_1_8.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_9_OutputClick() //Y108---Spare On/Off
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_9.Value)
                {
                    Hardware.IO_LIST.Output.Y108_Spare_On();
                    o_1_9.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y108_Spare_Off();
                    o_1_9.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_10_OutputClick() //Y109---Weighing Door Open/Close
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_10.Value)
                {
                    Hardware.IO_LIST.Output.Y109_Weighing_Door_Close();
                    o_1_10.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y109_Weighing_Door_Open();
                    o_1_10.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_11_OutputClick() //Y110---Weighing Tray FWD/BWD
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_11.Value)
                {
                    Hardware.IO_LIST.Output.Y110_Weighing_Tray_Forward();
                    o_1_11.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y110_Weighing_Tray_Backward();
                    o_1_11.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_12_OutputClick() //Y111---Scrap Bin Drawer 1/2 Open
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_12.Value)
                {
                    Hardware.IO_LIST.Output.Y111_Scrap_Bin_Drawer_Chute_Close();
                    o_1_12.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y111_Scrap_Bin_Drawer_Chute_Open();
                    o_1_12.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_13_OutputClick() //Y112---Open Thawing Cover
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_13.Value)
                {
                    Hardware.IO_LIST.Output.Y112_Thawing_Cover_Close();
                    o_1_13.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y112_Thawing_Cover_Open();
                    o_1_13.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_14_OutputClick() //Y113---Open Scrap Bin Drawer 1
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_14.Value)
                {
                    Hardware.IO_LIST.Output.Y113_Scrap_Bin_Drawer_1_Close();
                    o_1_14.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y113_Scrap_Bin_Drawer_1_Open();
                    o_1_14.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_15_OutputClick() //Y114---Open Scrap Bin Drawer 2
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_15.Value)
                {
                    Hardware.IO_LIST.Output.Y114_Scrap_Bin_Drawer_2_Close();
                    o_1_15.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y114_Scrap_Bin_Drawer_2_Open();
                    o_1_15.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_16_OutputClick() //Y115---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_16.Value)
                {
                    Hardware.IO_LIST.Output.Y115_Spare_Off();
                    o_1_16.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y115_Spare_On();
                    o_1_16.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_17_OutputClick() //Y200---Ejector A Up/Down
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_17.Value)
                {
                    Hardware.IO_LIST.Output.Y200_Ejector_A_Down();
                    o_1_17.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y200_Ejector_A_Up();
                    o_1_17.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_18_OutputClick() //Y201---Ejector B Up/Down
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_18.Value)
                {
                    Hardware.IO_LIST.Output.Y201_Ejector_B_Down();
                    o_1_18.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y201_Ejector_B_Up();
                    o_1_18.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_19_OutputClick() //Y202---Ejector C Up/Down
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_19.Value)
                {
                    Hardware.IO_LIST.Output.Y202_Ejector_C_Down();
                    o_1_19.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y202_Ejector_C_Up();
                    o_1_19.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_20_OutputClick() //Y203---Ejector D Up/Down
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_20.Value)
                {
                    Hardware.IO_LIST.Output.Y203_Ejector_D_Down();
                    o_1_20.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y203_Ejector_D_Up();
                    o_1_20.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_21_OutputClick() //Y204---Mix Power On/Off
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_21.Value)
                {
                    Hardware.IO_LIST.Output.Y204_MixPower_Off();
                    o_1_21.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y204_MixPower_On();
                    o_1_21.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_22_OutputClick() //Y205---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_22.Value)
                {
                    Hardware.IO_LIST.Output.Y205_Spare_Off();
                    o_1_22.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y205_Spare_On();
                    o_1_22.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_23_OutputClick() //Y206---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_23.Value)
                {
                    Hardware.IO_LIST.Output.Y206_Spare_Off();
                    o_1_23.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y206_Spare_On();
                    o_1_23.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_24_OutputClick() //Y207---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_24.Value)
                {
                    Hardware.IO_LIST.Output.Y207_Spare_Off();
                    o_1_24.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y207_Spare_Off();
                    o_1_24.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_25_OutputClick() //Y208---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_25.Value)
                {
                    Hardware.IO_LIST.Output.Y208_Spare_Off();
                    o_1_25.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y208_Spare_On();
                    o_1_25.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_26_OutputClick() //Y209---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_26.Value)
                {
                    Hardware.IO_LIST.Output.Y209_Spare_Off();
                    o_1_26.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y209_Spare_On();
                    o_1_26.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_27_OutputClick() //Y210---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_27.Value)
                {
                    Hardware.IO_LIST.Output.Y210_Spare_Off();
                    o_1_27.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y210_Spare_On();
                    o_1_27.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_28_OutputClick() //Y211---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_28.Value)
                {
                    Hardware.IO_LIST.Output.Y211_Spare_Off();
                    o_1_28.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y212_Spare_On();
                    o_1_28.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_29_OutputClick() //Y212--Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_29.Value)
                {
                    Hardware.IO_LIST.Output.Y212_Spare_Off();
                    o_1_29.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y212_Spare_On();
                    o_1_29.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_30_OutputClick() //Y213---Spare
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_30.Value)
                {
                    Hardware.IO_LIST.Output.Y213_Spare_Off();
                    o_1_30.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y213_Spare_On();
                    o_1_30.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void o_1_31_OutputClick() //Y214---Spare
        {
            // 在此处添加事件处理程序实现。
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_31.Value)
                {
                    Hardware.IO_LIST.Output.Y214_Spare_Off();
                    o_1_31.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y214_Spare_On();
                    o_1_31.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }

        private void o_1_32_OutputClick()
        {
            if (StaticRes.Global.Hardware_Connection)
            {
                if (o_1_32.Value)
                {
                    Hardware.IO_LIST.Output.Y215_Spare_Off();
                    o_1_32.Value = false;
                }
                else
                {
                    Hardware.IO_LIST.Output.Y215_Spare_On();
                    o_1_32.Value = true;
                }
            }
            else
                MessageBox.Show("Can not detect PCI-1756 PCB Board !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btn_Right_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lb_title.Content.ToString() == "Output-1")
            {
                this.btn_Left.IsEnabled = true;
                this.btn_Right.IsEnabled = false;
                this.sb_right_1.Storyboard.Begin();
                this.lb_title.Content = "Output-2";
            }
        }

        private void btn_Left_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (lb_title.Content.ToString() == "Output-2")
            {
                this.btn_Left.IsEnabled = false;
                this.btn_Right.IsEnabled = true;
                this.sb_left_3.Storyboard.Begin();
                this.lb_title.Content = "Output-1";
            }
        }
    }
}