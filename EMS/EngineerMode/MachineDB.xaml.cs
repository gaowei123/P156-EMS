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

namespace EMS.EngineerMode
{
    /// <summary>
    /// UserDB.xaml 的交互逻辑
    /// </summary>
    public partial class MachineDB : UserControl
    {
        public MachineDB()
        {
            InitializeComponent();
            DataTable dt_department = Logic.Common.Department_Select();
            if (dt_department.Rows.Count > 0)
            {
                foreach (DataRow a in dt_department.Rows)
                {
                    cbb_department.Items.Add(a["DEPARTMENT"].ToString());
                }
            }
            kb.CurrentTextBox = this.txt_equipID_Search;
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Machine_Search();
        }

        void Machine_Search()
        {
            try
            {
                dg_list.ItemsSource = Logic.Common.Equipment_Search(this.txt_equipID_Search.Text).DefaultView;
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_add_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MaskActionPage.Visibility = Visibility.Visible;
            this.lb_title.Content = "Add Equipment";
            this.txt_equipID.Text = null;
            this.txt_equipMaker.Text = null;
            this.txt_equipModel.Text = null;
            this.cbb_department.Text = null;
            this.txt_locID.Text = null;
            this.cbb_department.IsEnabled = true;
            this.txt_equipID.IsEnabled = true;
            this.txt_equipID.Focus();
        }

        private void btn_update_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (dg_list.SelectedItems.Count == 0)
                {
                    MessageBox.Show("Please select one equipment first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                this.MaskActionPage.Visibility = Visibility.Visible;
                this.lb_title.Content = "Update Equipment";
                this.txt_equipID.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["EQUIP_ID"].ToString();
                this.txt_equipMaker.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["EQUIP_MAKER"].ToString();
                this.txt_equipModel.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["EQUIP_MODEL"].ToString();
                this.cbb_department.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString();
                this.txt_locID.Text = ((DataRowView)dg_list.SelectedItems[0]).Row["LOCID"].ToString();
                this.txt_equipID.IsEnabled = false;
                this.cbb_department.IsEnabled = false;
                this.txt_equipID.Focus();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btn_Delete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to delete this equipment ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (dg_list.SelectedItems.Count == 0)
                    {
                        MessageBox.Show("Please select one equipment first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                        return;
                    }

                    Logic.Common.Equipment_Delete(((DataRowView)dg_list.SelectedItems[0]).Row["EQUIP_ID"].ToString(), ((DataRowView)dg_list.SelectedItems[0]).Row["DEPARTMENT"].ToString());
                    Common.Reports.LogFile.Log("Delete machine : " + ((DataRowView)dg_list.SelectedItems[0]).Row["EQUIP_ID"].ToString() + " by user:" + StaticRes.Global.Current_User.USER_ID);
                    MessageBox.Show("Delete Succesful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Machine_Search();
                    kb.CurrentTextBox = txt_equipID_Search;
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
                ObjectModule.Local.Equipment ge = new ObjectModule.Local.Equipment();
                ge.DEPARTMENT = StaticRes.Global.Current_User.DEPARTMENT;
                ge.EQUIP_ID = this.txt_equipID.Text;
                ge.EQUIP_MAKER = this.txt_equipMaker.Text;
                ge.EQUIP_MODEL = this.txt_equipModel.Text;
                ge.LOCID = this.txt_locID.Text;
                ge.UPDATED_BY = StaticRes.Global.Current_User.USER_ID;
                ge.UPDATED_TIME = System.DateTime.Now;
                if (lb_title.Content.ToString() == "Add Equipment")
                {
                    Logic.Common.Equipment_Insert(ge);
                    Common.Reports.LogFile.Log("Register machine : " + ge.EQUIP_ID + " by user:" + StaticRes.Global.Current_User.USER_ID);
                }
                else
                {
                    Logic.Common.Equipment_Update(ge);
                    Common.Reports.LogFile.Log("Update machine : " + ge.EQUIP_ID + " by user:" + StaticRes.Global.Current_User.USER_ID);
                }
                MessageBox.Show("Successful !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                Machine_Search();
                this.MaskActionPage.Visibility = Visibility.Collapsed;
                kb.CurrentTextBox = txt_equipID_Search ;
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
                case "txt_equipID":
                    kb.CurrentTextBox = this.txt_equipID;
                    break;
                case "txt_equipID_Search":
                    kb.CurrentTextBox = this.txt_equipID_Search;
                    break;
                case "txt_equipMaker":
                    kb.CurrentTextBox = this.txt_equipMaker;
                    break;
                case "txt_equipModel":
                    kb.CurrentTextBox = this.txt_equipModel;
                    break;
                case "txt_locID":
                    kb.CurrentTextBox = this.txt_locID;
                    break;
            }
        }
    }
}
