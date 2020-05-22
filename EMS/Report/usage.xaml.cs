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

namespace EMS.Report
{
    /// <summary>
    /// Slot_Status.xaml 的交互逻辑
    /// </summary>
    public partial class Usage : UserControl
    {
        public Usage()
        {
            InitializeComponent();
            this.dp_to.Text = System.DateTime.Now.ToString();
            this.dp_from.Text = System.DateTime.Now.AddDays(-7).ToString();
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (dp_from.Text.Length == 0)
            {
                MessageBox.Show("Please select date from first !!\n请选择开始日期！！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (dp_to.Text.Length == 0)
            {
                MessageBox.Show("Please select date to first !!\n请选择截止日期！！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DateTime dateFrom=DateTime.Parse(dp_from.Text);
            DateTime dateTo=DateTime.Parse(dp_to.Text);
            if (cb_reportType.Text == "Alarm Pareto")
                Alarm_Pareto(dateFrom, dateTo);
            else if (cb_reportType.Text == "Alarm Trend")
                Alarm_Trend(dateFrom, dateTo);
            else if (cb_reportType.Text == "Usage Trend")
                Usage_Trend(dateFrom, dateTo);
        }

        void Alarm_Pareto(DateTime dateFrom,DateTime dateTo)
        {
            try
            {
                this.MyChart.Series.Clear();
                DataTable dt = DataProvider.Local.Event.Pareto(dateFrom, dateTo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow x in dt.Rows)
                    {
                        DataSeries dataSeries = new DataSeries();
                        dataSeries.LabelEnabled = true;
                        dataSeries.RenderAs = RenderAs.Column;
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.YValue = double.Parse(x["Alarm_Frequency"].ToString());
                        dataPoint.ToolTipText = "Error:" + x["Alarm_Message"].ToString();
                        dataSeries.DataPoints.Add(dataPoint);
                        dataSeries.Name = x["Alarm_Message"].ToString();
                        this.MyChart.Series.Add(dataSeries);
                    }
                }
                else
                    MessageBox.Show("No data during this time !!\n这段时间内没有数据！！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Error Pareto chart click error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Alarm_Trend(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                this.MyChart.Series.Clear();
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Spline;
                dataSeries.LabelEnabled = true;
                dataSeries.Color = new SolidColorBrush(Colors.Blue);
                DataTable dt = DataProvider.Local.Event.Error_Trend(dateFrom, dateTo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow x in dt.Rows)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = x["Alarm_Date"].ToString();
                        dataPoint.YValue = double.Parse(x["Alarm_Frequency"].ToString());
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    this.MyChart.Series.Add(dataSeries);
                }
                else
                    MessageBox.Show("No data during this time !!\n这段时间没有报警！！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Error Trend click error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void Usage_Trend(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                this.MyChart.Series.Clear();
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Column;
                dataSeries.LabelEnabled = true;
                dataSeries.Color = new SolidColorBrush(Colors.Blue);
                DataTable dt = DataProvider.Local.History.Usage_Trend(dateFrom, dateTo);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow x in dt.Rows)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.AxisXLabel = x["UPDATED_TIME"].ToString();
                        dataPoint.YValue = double.Parse(x["WEIGHT"].ToString());
                        dataSeries.DataPoints.Add(dataPoint);
                    }
                    this.MyChart.Series.Add(dataSeries);
                }
                else
                    MessageBox.Show("No data during this time !!\n这段时间内没有数据！！", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Usage Trend click error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
