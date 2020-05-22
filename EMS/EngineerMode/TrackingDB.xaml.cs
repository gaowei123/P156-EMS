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

namespace EMS.EngineerMode
{
    /// <summary>
    /// TrackingDB.xaml 的交互逻辑
    /// </summary>
    public partial class TrackingDB : UserControl
    {
        public TrackingDB()
        {
            InitializeComponent();
            kb.CurrentTextBox = this.txt_partID;
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "txt_partID":
                    kb.CurrentTextBox = this.txt_partID;
                    break;
                case "txt_mcID":
                    kb.CurrentTextBox = this.txt_mcID;
                    break;
            }
		}

        private void btn_search_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
               dg_list.ItemsSource = Logic.Common.Tracking_Search(txt_partID.Text, txt_mcID.Text).DefaultView;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
