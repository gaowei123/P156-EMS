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

namespace EMS.EngineerMode
{
    /// <summary>
    /// EngineerPage.xaml 的交互逻辑
    /// </summary>
    public partial class EngineerPage : Window
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

        public EngineerPage()
        {
            InitializeComponent();
            Resources.MergedDictionaries.Add(StaticRes.Global.CurrentLanguageRes);
        }

        private void btn_close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Common.Reports.LogFile.Log("Exit engineer page  , user : " + StaticRes.Global.Current_User.USER_ID);
                StaticRes.Global.Current_User.USER_GROUP = string.Empty;
                StaticRes.Global.Current_User.USER_ID = string.Empty;
                backClick();
                this.Close();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
