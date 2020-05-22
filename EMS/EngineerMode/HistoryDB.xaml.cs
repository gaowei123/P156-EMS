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
using System.Windows.Forms;

namespace EMS.EngineerMode
{
    /// <summary>
    /// HistoryDB.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryDB : System.Windows.Controls.UserControl
    {
        private DataTable dt = new DataTable();

        public HistoryDB()
        {
            InitializeComponent();
            this.dp_to.Text = System.DateTime.Now.ToString();
            this.dp_from.Text = System.DateTime.Now.AddDays(-7).ToString();
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (this.dp_from.Text.Length == 0 || this.dp_to.Text.Length == 0)
                {
                    System.Windows.MessageBox.Show("Please select date first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                if (ck_summaryReport.IsChecked == true)
                {
                    //dt = Logic.Common.History_Summary_Search(DateTime.Parse(dp_from.Text), DateTime.Parse(dp_to.Text));
                    List<ObjectModule.Local.Summary_List> list= Logic.Common.History_Summary_Search(DateTime.Parse(dp_from.Text), DateTime.Parse(dp_to.Text));
                    dt = Logic.Common.ToDataTable<ObjectModule.Local.Summary_List>(list);
                    dg_list.ItemsSource = list;
                }
                else
                {
                    bool Return_with_Expiry = false;
                    if (ck_returnWithExpiry.IsChecked == true)
                        Return_with_Expiry = true;
                    dt = Logic.Common.History_Search(DateTime.Parse(dp_from.Text), DateTime.Parse(dp_to.Text), txt_partID.Text, txt_equipID.Text, Return_with_Expiry);
                    dg_list.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ee)
            {
                System.Windows.MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btn_save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

                SaveFileDialog MyDlg = new SaveFileDialog();
                MyDlg.Filter = "文本文件(.xls)|*.xls|所有文件(*.*)|*.*";
                String MyFileName = "";
                if (MyDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    MyFileName = MyDlg.FileName;
                }

                int rows = this.dt.Rows.Count;
                int columns = this.dt.Columns.Count;
                System.Text.StringBuilder sb1 = new StringBuilder();
                for (int x = 0; x < dt.Columns.Count; x++)
                {
                    string a = dt.Columns[x].ColumnName;
                    sb1.Append(a);
                    sb1.Append("\t");
                }
                sb1.Append("\r\n ");
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        string a = this.dt.Rows[i][j].ToString();
                        sb1.Append(a);
                        sb1.Append("\t");
                    }
                    sb1.Append("\r\n ");
                }
                System.IO.StreamWriter sw = new System.IO.StreamWriter(MyFileName, false);
                sw.Write(sb1.ToString());
                sw.Close();
               System.Windows.MessageBox.Show("Save file sccessful to " +MyFileName+"",
                    "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
                System.Windows.MessageBox.Show("Save failed,Pls try again !", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}
