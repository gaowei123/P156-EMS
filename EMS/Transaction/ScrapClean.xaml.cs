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

namespace EMS.Transaction
{
    /// <summary>
    /// Load.xaml 的交互逻辑
    /// </summary>
    public partial class ScrapClean : Window
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

        public ScrapClean()
        {
            InitializeComponent();

            try
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(".\\CurScrpQty.txt");
                this.txt_currentScrapQty.Text = sr.ReadLine();
                //this.txt_scrapQtyLimit.Text = StaticRes.Global.System_Setting.Scrap_Limit_Qty.ToString();
                sr.Close();
            }
            catch (Exception ee)
            {
                Common.Reports.LogFile.Log("Scrap cleaning page error when read file : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_reset_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                this.txt_currentScrapQty.Text = "0";
                System.IO.StreamWriter sr = new System.IO.StreamWriter(".\\CurScrpQty.txt");
                sr.WriteLine("0");
                sr.Close();
                MessageBox.Show("Reset successful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                Common.Reports.LogFile.Log("Reset scrap qty successful , user : " + StaticRes.Global.Current_User.USER_ID);
                backClick();
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Reset scrap qty error : " + ee.Message);
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
		
		private void btn_close_Click(object sender,System.Windows.RoutedEventArgs e)
		{
            backClick();
            this.Close();
		}
    }
}