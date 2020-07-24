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
using Visifire.Charts;
using System.Data;

namespace EMS.Report
{
    /// <summary>
    /// Slot_Status.xaml 的交互逻辑
    /// </summary>
    public partial class Slot_Status : UserControl
    {
        DispatcherTimer tm = new DispatcherTimer();

        public Slot_Status()
        {
            InitializeComponent();
            Slot_Refresh();
            Chart_Inventory();
            tm.Tick += new EventHandler(Timer);
            tm.Interval = TimeSpan.FromSeconds(60);
            tm.Start();
            this.btn_Left.IsEnabled = false;
        }

        void Timer(object sender, EventArgs e)
        {
             Slot_Refresh();
             GC.Collect();
        }

        private void Slot_Refresh()
        {
            try
            {
                SlotTemp[] x = new SlotTemp[] { s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15, s16, s17, s18, s19, s20, s21, s22, s23, s24, s25,
                s26,s27,s28,s29,s30,s31,s32,s33,s34,s35,s36,s37,s38,s39,s40,s41,s42,s43,s44,s45,s46,s47,s48,s49,s50,s51,s52,s53,s54,s55,s56,s57,s58,s59,s60,s61,s62,
                s63,s64,s65,s66,s67,s68,s69,s70,s71,s72,s73,s74,s75,s76,s77,s78,s79,s80,s81,s82,s83,s84,s85,s86,s87,s88,s89,s90,s91,s92,s93,s94,s95,s96,s97,s98,s99,s100};
                List<ObjectModule.Local.Binning> fb = Logic.Common.Slot_Status();
                int i = 0;
                foreach (ObjectModule.Local.Binning a in fb)
                {
                    string Slot_NameBegin = string.Empty;
                    if (i >= 0 && i <= 34)
                        Slot_NameBegin = "A-";
                    else if (i > 34 && i < 65)
                        Slot_NameBegin = "B-";
                    else if (i >= 65 && i < 85)
                        Slot_NameBegin = "C-";
                    else
                        Slot_NameBegin = "D-";
                    x[i].SlotName = Slot_NameBegin + (i + 1).ToString();
                    if (a.STATUS != StaticRes.Global.Binning_Status.Empty)
                    {
                        System.Windows.Controls.ToolTip tt = new System.Windows.Controls.ToolTip();
                        x[i].Sapcode = a.SAPCODE;
                        x[i].PartID = a.PART_ID;
                        if (a.EXPIRY_DATETIME < System.DateTime.Now)
                            x[i].Value = "3";
                        else if (a.READY_DATETIME <= System.DateTime.Now && System.DateTime.Now < a.EXPIRY_DATETIME)
                            x[i].Value = "2";
                        else if (a.READY_DATETIME > System.DateTime.Now)
                            x[i].Value = "1";
                        tt.Content = "-----------------------------------------------------------\n" +
                                     "Part_ID:   " + a.PART_ID + "\n" +
                                     "-----------------------------------------------------------\n" +
                                     "Description: " + a.DESCRIPTION + "\n" +
                                     "-----------------------------------------------------------\n" +
                                     "Batch_No:  " + a.BATCH_NO + "\n" +
                                     "-----------------------------------------------------------\n" +
                                     "Department(User_ID):  " + a.DEPARTMENT + "(" + a.USER_ID + ")\n" +
                                     "-----------------------------------------------------------\n" +
                                     "Thawing_DateTime:  " + a.THAWING_DATETIME + "\n" +
                                     "-----------------------------------------------------------\n" +
                                     "Ready_DateTime:  " + a.READY_DATETIME + "\n" +
                                     "-----------------------------------------------------------\n" +
                                     "Expiry_DateTime:  " + a.EXPIRY_DATETIME + "\n" +
                                     "----------------------------------------------------------- ";
                        ToolTipService.SetToolTip(x[i], tt);
                    }
                    else
                    {
                        x[i].Value = "0";
                        x[i].ToolTip = "Empty";
                    }
                    i++;
                }
                    dg_list_expiry.ItemsSource = DataProvider.Local.Tracking.Select.Expiry_List().DefaultView;

            }
            catch
            {
            }
        }
        void Chart_Inventory()
        {
            try
            {
                this.MyChart.Series.Clear();
                DataSeries dataSeries = new DataSeries();
                dataSeries.RenderAs = RenderAs.Pie;





                BLL.Binning binBLL = new BLL.Binning();
                List<Model.Binning> binList = binBLL.GetModelList();
                if (binList == null)
                    return;


                var sapGrouped = from a in binList
                                 where a.STATUS == "NEW" || a.STATUS == "REUSED"
                                 group a by new { a.SAPCODE, a.STATUS } into b
                                 orderby b.Key.SAPCODE ascending, b.Key.STATUS ascending
                                 select new
                                 {
                                     b.Key.SAPCODE,
                                     b.Key.STATUS,
                                     countQty = b.Count()
                                 };
                foreach (var item in sapGrouped)
                {
                    string sapCode = item.SAPCODE;
                    int countQty = item.countQty;
                    string status = item.STATUS;

                    DataPoint dataPoint = new DataPoint();
                    dataPoint.AxisXLabel = string.Format("{0}({1})", sapCode, status);
                    dataPoint.YValue = countQty;
                    dataSeries.DataPoints.Add(dataPoint);
                }


                var statusGrouped = from a in binList
                                    where a.STATUS == "EMPTY" || a.STATUS == "DISABLED"
                                    group a by a.STATUS into b
                                    select new
                                    {
                                        STATUS = b.Key,
                                        countQty = b.Count()
                                    };
                foreach (var item in statusGrouped)
                {
                
                    int countQty = item.countQty;
                    string status = item.STATUS;

                    DataPoint dp = new DataPoint();
                    dp.AxisXLabel = status;
                    dp.YValue = countQty;
                    dataSeries.DataPoints.Add(dp);
                }


              
                this.MyChart.Series.Add(dataSeries);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void btn_Left_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (lb_title.Content.ToString() == "Index-4 Slot Status" )
                {
                    lb_title.Content = "Index-3 Slot Status";
                    this.sb_left_1.Storyboard.Begin();
                    this.btn_Right.IsEnabled = true;
                    return;
                }
                if (lb_title.Content.ToString() == "Index-3 Slot Status")
                {
                    lb_title.Content = "Index-2 Slot Status";
                    this.sb_left_2.Storyboard.Begin();
                    return;
                }
                if (lb_title.Content.ToString() == "Index-2 Slot Status")
                {
                    lb_title.Content = "Index-1 Slot Status";
                    this.sb_left_3.Storyboard.Begin();
                    return;
                }
                if (lb_title.Content.ToString() == "Index-1 Slot Status")
                {
                    lb_title.Content = "Inventory";
                    this.sb_left_4.Storyboard.Begin();
                    this.btn_Left.IsEnabled = false;
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void btn_Right_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (lb_title.Content.ToString() == "Inventory")
                {
                    lb_title.Content = "Index-1 Slot Status";
                    this.sb_right_1.Storyboard.Begin();
                    this.btn_Left.IsEnabled = true;
                    return;
                }
                if (lb_title.Content.ToString() == "Index-1 Slot Status")
                {
                    lb_title.Content = "Index-2 Slot Status";
                    this.sb_right_2.Storyboard.Begin();
                    return;
                }
                if (lb_title.Content.ToString() == "Index-2 Slot Status")
                {
                    lb_title.Content = "Index-3 Slot Status";
                    this.sb_right_3.Storyboard.Begin();
                    return;
                }
                if (lb_title.Content.ToString() == "Index-3 Slot Status")
                {
                    lb_title.Content = "Index-4 Slot Status";
                    this.sb_right_4.Storyboard.Begin();
                    this.btn_Right.IsEnabled = false;
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK);
            }
        }


    }
}
