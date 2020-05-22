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
using System.Data;

namespace EMS.MaintMode
{
    /// <summary>
    /// ComTest.xaml 的交互逻辑
    /// </summary>
    public partial class ComTest : UserControl
    {
        private System.IO.Ports.SerialPort weightscale = new System.IO.Ports.SerialPort();
        private System.IO.Ports.SerialPort Inline_Scanner = new System.IO.Ports.SerialPort();
        private System.IO.Ports.SerialPort Handle_Scanner = new System.IO.Ports.SerialPort();

        delegate void Inline_Scanner_Read();
        delegate void Weight_Scale_Read();
        delegate void Handle_Scanner_Read();

        private bool Scale = false;

        public ComTest()
        {
            InitializeComponent();
            InitComPort();
            Common.Reports.LogFile.Log("Open COM");

            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);//YAKUN2015.2.28
            txt_barcodePrinter_comPort.Text = StaticRes.Global.System_Setting.Printer_COM_Port;
            txt_handleScanner_comPort.Text = StaticRes.Global.System_Setting.Handle_Scanner_COM_Port;
            txt_weighingScale_comPort.Text = StaticRes.Global.System_Setting.Weighing_Scale_COM_Port;
            Handle_Scanner.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(Handle_Scanner_DataReceived);
            this.btn_gotWeightData.Content = "Open Weighting Scale";
            kb.CurrentTextBox = txt_part_id;
        }

        private void InitComPort()
        {
            try
            {
                //Handle Scanner
                //Handle_Scanner.BaudRate = StaticRes.Global.System_Setting.Handle_Scanner_BaudRate;
                //Handle_Scanner.StopBits = System.IO.Ports.StopBits.One;
                //Handle_Scanner.DataBits = StaticRes.Global.System_Setting.Handle_Scanner_DataBits;
                //Handle_Scanner.PortName = StaticRes.Global.System_Setting.Handle_Scanner_COM_Port;
                //if (!Handle_Scanner.IsOpen)
                //    Handle_Scanner.Open();




                Handle_Scanner.BaudRate = 115200;
                Handle_Scanner.StopBits = System.IO.Ports.StopBits.One;
                Handle_Scanner.DataBits = 8;
                Handle_Scanner.PortName = "COM4";
                if (!Handle_Scanner.IsOpen)
                    Handle_Scanner.Open();

                Handle_Scanner.NewLine = "\r";

                string aaaa = Handle_Scanner.ReadLine();


                //Weight Scale
                weightscale.BaudRate = StaticRes.Global.System_Setting.Weighing_Scale_BaudRate;
                weightscale.StopBits = System.IO.Ports.StopBits.One;
                weightscale.DataBits = StaticRes.Global.System_Setting.Weighing_Scale_DataBits;
                weightscale.PortName = StaticRes.Global.System_Setting.Weighing_Scale_COM_Port;
                if (!weightscale.IsOpen)
                    weightscale.Open();

            }
            catch(Exception ee)
            {
                Common.Reports.LogFile.Log("Open COM port error in COM test page :" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_ComPort()
        {
            try
            {
                Handle_Scanner.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(Handle_Scanner_DataReceived);
                if (Handle_Scanner.IsOpen)
                    Handle_Scanner.Close();

                weightscale.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(WeightScaleDataReceived);
                if (weightscale.IsOpen)
                    weightscale.Close();
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Close COM port error in COM test page :" + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Weight Scale
        private string Weight_Data = string.Empty;
        void WeightScaleDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(100);
                this.Dispatcher.Invoke(new Weight_Scale_Read(Do_Weight_Scale_Read), null);
            }
            catch
            { }
        }

        void Do_Weight_Scale_Read()
        {
            try
            {
                lb_weightscaler.Items.Add(weightscale.ReadLine().Replace("g\r", "").Trim());
            }
            catch
            { }
        }
        #endregion

        #region Handle Scanner
        void Handle_Scanner_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(200);//yakun.zhou2015/07/02


              


                byte[] data = new byte[Handle_Scanner.BytesToRead];

                int bbb = Handle_Scanner.Read(data, 0, data.Length);

                string sbbbb = Encoding.Default.GetString(data);








                this.Dispatcher.Invoke(new Handle_Scanner_Read(Do_Handle_Scanner_Read), null);
            }
            catch(Exception ee)
            {
                throw ee;
            }
        }

        void Do_Handle_Scanner_Read()
        {
            try
            {
                this.lb_handleScanner.Items.Add(Handle_Scanner.ReadExisting());
            }
            catch(Exception ex)
            { }
        }
        #endregion

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "txt_mcID":
                    kb.CurrentTextBox = this.txt_mcID;
                    break;
                case "txt_part_id":
                    kb.CurrentTextBox = this.txt_part_id;
                    break;
            }
        }
        private void btn_print_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                string Part_ID = string.Empty;
                string MC_ID = string.Empty;
                if (txt_part_id.Text.Trim().Length > 0)
                Part_ID = this.txt_part_id.Text.Trim().ToUpper();
                Logic.Print.EMS(txt_part_id.Text, txt_mcID.Text, txt_expiryTime.Text, txt_thawingTime.Text, txt_readyTime.Text, txt_sapcode.Text);
                Common.Reports.LogFile.Log("Manual print label , Part ID : " + txt_part_id.Text + " ; MC ID : " + txt_mcID.Text + ", user : " + StaticRes.Global.Current_User.USER_ID);
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Manual print label error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_gotWeightData_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (Scale)
                {
                    weightscale.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(WeightScaleDataReceived);
                    Scale = false;
                    StaticRes.Global.Scale_Open = false;
                    this.btn_gotWeightData.Content = "Open Weighting Scale";
                }
                else
                {
                    weightscale.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(WeightScaleDataReceived);
                    Scale = true;
                    StaticRes.Global.Scale_Open = true;
                    this.btn_gotWeightData.Content = "Close Weighting Scale";
                }
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Manual got weighting data error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void kb_EnterClick()
        {
            this.btn_print.IsEnabled = true;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Close_ComPort();
        }

        private void btn_CapturePartID_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Data.DataTable dt = DataProvider.Local.Tracking.Select.by_PartID(txt_part_id.Text, StaticRes.Global.Status.Unload);
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("These haven't issue record of part ID:" + txt_part_id.Text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ObjectModule.Local.Tracking st = new ObjectModule.Local.Tracking(dt.Rows[0]);
            this.txt_mcID.Text = st.EQUIP_ID;
            this.txt_sapcode.Text = st.SAPCODE;
            //this.txt_expiryTime.Text = st.EXPIRY_TIME.ToString();
        }

        private void btn_humidity_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	// 在此处添加事件处理程序实现。
        }
    }
}
