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

namespace EMS.AdminMode
{
    /// <summary>
    /// UserDB.xaml 的交互逻辑
    /// </summary>
    public partial class DepartmentDB : UserControl
    {
        public DepartmentDB()
        {
            InitializeComponent();
            kb.CurrentTextBox = this.txt_department;
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Department_Search();
        }

        void Department_Search()
        {
            try
            {
                dg_list.ItemsSource = Logic.Common.Department_Select().DefaultView;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MaskActionPage.Visibility = Visibility.Visible;
            this.lb_title.Content = "Add Department";
            this.txt_department.Text = null;
            this.txt_department.Focus();
        }

        private void btn_Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this Department ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (dg_list.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select one Department first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }

                    Logic.Common.Department_Delete(((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString());
                    Common.Reports.LogFile.Log("Delete department : " + ((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString() + " by user:" + StaticRes.Global.Current_User.USER_ID);
                    MessageBox.Show("Delete Succesful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Department_Search();
                    kb.CurrentTextBox = txt_department;
                }
                catch(Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btn_confirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Logic.Common.Department_Insert(this.txt_department.Text);
                Common.Reports.LogFile.Log("Add department : " + txt_department.Text + " by user:" + StaticRes.Global.Current_User.USER_ID);
                MessageBox.Show("Successful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Department_Search();
                this.MaskActionPage.Visibility = Visibility.Collapsed;
                kb.CurrentTextBox = txt_department;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_cancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MaskActionPage.Visibility = Visibility.Collapsed;
        }
    }
}
