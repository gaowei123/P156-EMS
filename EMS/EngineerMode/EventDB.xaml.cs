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
    public partial class EventDB : System.Windows.Controls.UserControl
    {
        private DataTable dt = new DataTable();

        public EventDB()
        {
            InitializeComponent();
            this.dp_to.Text = System.DateTime.Now.ToString();
            this.dp_from.Text = System.DateTime.Now.AddDays(-7).ToString();
        }

        private void btn_search_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (this.dp_from.Text.Length==0 || this.dp_to.Text.Length==0)
                {
                   System.Windows.MessageBox.Show("Please select date first !!", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    return;
                }
                dt = Logic.Common.Event_Search(dp_from.Text, dp_to.Text);
                dg_list.ItemsSource = dt.DefaultView;
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
                System.IO.StreamWriter sw = new System.IO.StreamWriter(MyFileName, false);// new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "\\DataHistory\\Event" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt", false);
                sw.Write(sb1.ToString());
                sw.Close();
               System.Windows.MessageBox.Show("Save file sccessful to " +MyFileName + "",
                   // System.IO.Directory.GetCurrentDirectory() + "\\DataHistory\\History" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt",
                    "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch
            {
              System.Windows.MessageBox.Show("Save failed,Pls try again !", "Message", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}
