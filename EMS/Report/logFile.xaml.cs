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

namespace EMS.Report
{
    /// <summary>
    /// Slot_Status.xaml 的交互逻辑
    /// </summary>
    public partial class logFile : UserControl
    {
        public logFile()
        {
            InitializeComponent();
            this.dp_date.Text = System.DateTime.Now.ToString();           
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (dp_date.Text.Length == 0)
                {
                    MessageBox.Show("Please select date first !!\n请选择日期！！");
                    return;
                }
                System.IO.StreamReader sr = new System.IO.StreamReader(".\\Log\\" + DateTime.Parse(dp_date.Text).ToString("yyyyMMdd") + ".txt");
                string strline = sr.ReadLine();
                while (strline != null)
                {
                    listBox_log.Items.Add(strline);
                    strline = sr.ReadLine();
                }
                sr.Close();
            }
            catch(Exception ee)
            {
                MessageBox.Show("Read Log File failed !\n读取日志文件失败！" + ee.Message + "","Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
    }
}
