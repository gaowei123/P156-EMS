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
using Visifire.Charts;
using System.Windows.Threading;

namespace EMS.Report
{
    /// <summary>
    /// Slot_Status.xaml 的交互逻辑
    /// </summary>
    public partial class N2_Report : UserControl
    {
        DispatcherTimer tm = new DispatcherTimer();

        public N2_Report()
        {
            InitializeComponent();
            if (StaticRes.Global.Current_Humidity != "100")
                txt_currentHumidity.Text = StaticRes.Global.Current_Humidity;
            //this.txt_refreshTime.Text = StaticRes.Global.System_Setting.Humidity_Read_Timer.ToString();
            //this.txt_highLimit.Text = StaticRes.Global.System_Setting.Humidity_SV_High_Limit;
            //this.txt_LowLimit.Text = StaticRes.Global.System_Setting.Humidity_SV_Low_Limit;
            //tm.Tick += new EventHandler(Timer);
            //tm.Interval = TimeSpan.FromSeconds(StaticRes.Global.System_Setting.Humidity_Read_Timer);
            //tm.Start();
        }

        void Timer(object sender, EventArgs e)
        {
            try
            {
                this.txt_currentHumidity.Text = StaticRes.Global.Current_Humidity;
                GC.Collect();
            }
            catch
            {
            }
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                List<string> list = new List<string>();
                System.IO.StreamReader sr = new System.IO.StreamReader(".\\Humidity_Log\\" + DateTime.Parse(this.dp_date.Text).ToString("yyyyMMdd") + ".txt");
                string strline = sr.ReadLine();
                while (strline != null)
                {
                    list.Add(strline);
                    strline = sr.ReadLine();
                }
                sr.Close();
                try
                {
                    this.MyChart.Series.Clear();
                    DataSeries dataSeries_pv = new DataSeries();
                    dataSeries_pv.RenderAs = RenderAs.Spline;
                    dataSeries_pv.LabelEnabled = true;
                    dataSeries_pv.LegendText = "PV(%)";
                    dataSeries_pv.Color = new SolidColorBrush(Colors.Red);

                    DataSeries dataSeries_sv = new DataSeries();
                    dataSeries_sv.RenderAs = RenderAs.Spline;
                    dataSeries_sv.LabelEnabled = true;
                    dataSeries_sv.LegendText = "CV(%)";
                    dataSeries_sv.Color = new SolidColorBrush(Colors.LimeGreen);
                    if (list.Count>0)
                    {
                        foreach (string x in list)
                        {
                            DataPoint dataPoint_sv = new DataPoint();
                            dataPoint_sv.AxisXLabel = x.Substring(9, 5);
                            dataPoint_sv.YValue = double.Parse(x.Substring(24, 4));
                            dataSeries_sv.DataPoints.Add(dataPoint_sv);

                            DataPoint dataPoint_pv = new DataPoint();
                            dataPoint_pv.AxisXLabel = x.Substring(9, 5);
                            dataPoint_pv.YValue = double.Parse(x.Substring(33, 4));
                            dataSeries_pv.DataPoints.Add(dataPoint_pv);
                        }
                        this.MyChart.Series.Add(dataSeries_sv);
                        this.MyChart.Series.Add(dataSeries_pv);
                    }
                    else
                        MessageBox.Show("No data during this time !!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ee)
                {
                    Common.Reports.LogFile.Log("Error Trend click error : " + ee.Message);
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            tm.Stop();
        }
    }
}
