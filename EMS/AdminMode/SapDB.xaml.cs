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
    /// SapDB.xaml 的交互逻辑
    /// </summary>
    public partial class SapDB : System.Windows.Controls.UserControl
    {
        public SapDB()
        {
            InitializeComponent();
            try
            {
                kb.CurrentTextBox = this.txt_sapcode_search;
                DataTable dt_department = Logic.Common.Department_Select();
                if (dt_department.Rows.Count > 0)
                {
                    foreach (DataRow a in dt_department.Rows)
                    {
                        cbb_department.Items.Add(a["DEPARTMENT"].ToString());
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SAP_Search();
        }

        void SAP_Search()
        {
            try
            {
                dg_list.ItemsSource = Logic.Common.SAP_Search(this.txt_sapcode_search.Text).DefaultView;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MaskActionPage.Visibility = Visibility.Visible;
            this.lb_title.Content = "Add Sapcode";
            this.txt_description.Text = null;
            this.txt_emptySyringeWeight.Text = null;
            this.txt_newMaxWeight.Text = null;
            this.cbb_department.Text = null;
            this.txt_newMinWeight.Text = null;
            this.txt_sapcode.Text = null;
            this.txt_thawingTime.Text = null;
            this.txt_thawingTime.Text = null;
            this.txt_scrapWeight.Text = null;
            this.cbb_capacity.Text = null;
            this.txt_usageLife.Text = null;
            this.cbb_onHold.Text = null;
            this.cbb_department.IsEnabled = true;
            this.txt_sapcode.IsEnabled = true;
            this.txt_sapcode.Focus();
        }
		
		private void btn_update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (dg_list.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select one Sapcode first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                this.MaskActionPage.Visibility = Visibility.Visible;
                this.lb_title.Content = "Update Sapcode";
                this.txt_sapcode.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SAPCODE"].ToString();
                this.txt_description.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["DESCRIPTION"].ToString();
                this.txt_emptySyringeWeight.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["EMPTY_SYRINGE_WEIGHT"].ToString();
                this.cbb_department.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString();
                this.txt_newMaxWeight.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["NEW_MAX_WEIGHT"].ToString();
                this.txt_newMinWeight.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["NEW_MIN_WEIGHT"].ToString();
                this.txt_scrapWeight.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["SCRAP_WEIGHT"].ToString();
                this.cbb_capacity.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["CAPACITY"].ToString();
                this.txt_usageLife.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["USAGE_LIFE"].ToString();
                this.txt_thawingTime.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["THAWING_TIME"].ToString();
                this.cbb_onHold.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["ON_HOLD"].ToString();
                this.txt_sapcode.IsEnabled = false;
                this.cbb_department.IsEnabled = false;
                this.txt_sapcode.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btn_Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this Sapcode ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (dg_list.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select one Sapcode first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }
                    Logic.Common.SAP_Delete(((DataRowView)dg_list.SelectedItems[0]).Row["SAPCODE"].ToString(), ((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString());
                    Common.Reports.LogFile.Log("Delete Sapcode : " + ((DataRowView)dg_list.SelectedItems[0]).Row["SAPCODE"].ToString() + " by user:" + StaticRes.Global.Current_User.USER_ID);
                    MessageBox.Show("Delete Succesful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    SAP_Search();
                    kb.CurrentTextBox = txt_sapcode_search;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void btn_confirm_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                ObjectModule.Local.Sapcode gs = new ObjectModule.Local.Sapcode();
                gs.CAPACITY = int.Parse(this.cbb_capacity.Text);
                gs.DEPARTMENT = this.cbb_department.Text;
                gs.DESCRIPTION = this.txt_description.Text;
                gs.EMPTY_SYRINGE_WEIGHT = float.Parse(txt_emptySyringeWeight.Text);
                gs.NEW_MAX_WEIGHT = float.Parse(txt_newMaxWeight.Text);
                gs.NEW_MIN_WEIGHT = float.Parse(txt_newMinWeight.Text);
                gs.ON_HOLD = cbb_onHold.Text;
                gs.SAPCODE = this.txt_sapcode.Text;
                gs.SCRAP_WEIGHT = float.Parse(txt_scrapWeight.Text);
                gs.THAWING_TIME = int.Parse(txt_thawingTime.Text);
                gs.UPDATED_BY = StaticRes.Global.Current_User.USER_ID;
                gs.UPDATED_TIME = System.DateTime.Now;
                gs.USAGE_LIFE = int.Parse(txt_usageLife.Text);
                if (lb_title.Content.ToString() == "Add Sapcode")
                {
                    Logic.Common.SAP_Insert(gs);
                    Common.Reports.LogFile.Log("Add Sapcode: " + gs.SAPCODE + " ; Description:" + gs.DESCRIPTION + " ; Empty Syringe Weight" + gs.EMPTY_SYRINGE_WEIGHT.ToString() +
                                               " ; New Max Weight:" + gs.NEW_MAX_WEIGHT.ToString() + " ; New Min Weight:" + gs.NEW_MIN_WEIGHT.ToString() +
                                               " ; Scrap Weight:" + gs.SCRAP_WEIGHT.ToString() + " ; Thawing Time:" + gs.THAWING_TIME.ToString() +
                                               " ; Usage Life:" + gs.USAGE_LIFE.ToString() + " ;  On_Hold:" + gs.ON_HOLD.ToString() +
                                               " by user:" + StaticRes.Global.Current_User.USER_ID);
                }
                else
                {
                    Logic.Common.SAP_Update(gs);
                    Common.Reports.LogFile.Log("Update Sapcode: " + gs.SAPCODE + " ; Description:" + gs.DESCRIPTION + " ; Empty Syringe Weight" + gs.EMPTY_SYRINGE_WEIGHT.ToString() +
                                                " ; New Max Weight:" + gs.NEW_MAX_WEIGHT.ToString() + " ; New Min Weight:" + gs.NEW_MIN_WEIGHT.ToString() +
                                                " ; Scrap Weight:" + gs.SCRAP_WEIGHT.ToString() + " ; Thawing Time:" + gs.THAWING_TIME.ToString() +
                                                " ; Usage Life:" + gs.USAGE_LIFE.ToString() + " ;  On_Hold:" + gs.ON_HOLD.ToString() +
                                                " by user:" + StaticRes.Global.Current_User.USER_ID);
                }
                MessageBox.Show("Successful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                SAP_Search();
                this.MaskActionPage.Visibility = Visibility.Collapsed;
                kb.CurrentTextBox = txt_sapcode_search;
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

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (((TextBox)sender).Name)
            {
                case "txt_description":
                    kb.CurrentTextBox = this.txt_description;
                    break;
                case "txt_emptySyringeWeight":
                    kb.CurrentTextBox = this.txt_emptySyringeWeight;
                    break;
                case "txt_newMaxWeight":
                    kb.CurrentTextBox = this.txt_newMaxWeight;
                    break;
                case "txt_newMinWeight":
                    kb.CurrentTextBox = this.txt_newMinWeight;
                    break;
                case "txt_sapcode":
                    kb.CurrentTextBox = this.txt_sapcode;
                    break;
                case "txt_sapcode_search":
                    kb.CurrentTextBox = this.txt_sapcode_search;
                    break;
                case "txt_scrapWeight":
                    kb.CurrentTextBox = this.txt_scrapWeight;
                    break;
                case "txt_usageLife":
                    kb.CurrentTextBox = this.txt_usageLife;
                    break;
                case "txt_thawingTime":
                    kb.CurrentTextBox = this.txt_thawingTime;
                    break;
            }
        }

        private void kb_EnterClick()
        {
        	// 在此处添加事件处理程序实现。
        }
    }
}
