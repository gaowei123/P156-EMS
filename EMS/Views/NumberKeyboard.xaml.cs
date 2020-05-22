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

namespace EMS.Views
{
    /// <summary>
    /// NumverKeyboard.xaml 的交互逻辑
    /// </summary>
    public partial class NumberKeyboard : UserControl
    {
        public TextBox CurrentTextBox = new TextBox();
        public NumberKeyboard()
        {
            InitializeComponent();
        }

        private void btn_KeyBoard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (((Button)sender).Content.ToString())
            {
                
                case "Clear":
                    {
                        if (CurrentTextBox.Text.Length != 0)
                        {
                            CurrentTextBox.Text = string.Empty;
                        }
                        break;
                    }
                
                default:
                    {
                        if (CurrentTextBox != null)
                        {
                            CurrentTextBox.Text += ((Button)sender).Content.ToString();
                        }

                        break;
                    }
            }
        }
    }
}
