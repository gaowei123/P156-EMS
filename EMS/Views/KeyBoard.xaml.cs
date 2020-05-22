using System;
using System.Collections.Generic;
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
    /// ub_KeyBoard.xaml 的交互逻辑
    /// </summary>
    public partial class KeyBoard
    {
        #region ****** 外部消息扩展 ******

        public delegate void EnterClickEventHandler();
        private EnterClickEventHandler temp;
        public event EnterClickEventHandler EnterClick
        {
            add
            {
                temp += value;
            }
            remove
            {
                temp -= value;
            }
        }

        public delegate void OtherClickEventHandler();
        private OtherClickEventHandler Get_KeyBoard;
        public event OtherClickEventHandler UndoClick
        {
            add
            {
                Get_KeyBoard += value;
            }
            remove
            {
                Get_KeyBoard -= value;
            }
        }
        #endregion

        public KeyBoard()
        {
            this.InitializeComponent();
        }
        public TextBox CurrentTextBox = new TextBox();
        public PasswordBox CurrentPasswordBox = new PasswordBox();
        public ComboBox CurrentComboBox = new ComboBox();
        private void btn_KeyBoard_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (CurrentTextBox != null)
            {
                switch (((Button)sender).Content.ToString())
                {
                    case "<-BackSpace":
                        {
                            if (CurrentTextBox.Text.Length != 0)
                            {
                                CurrentTextBox.Text = CurrentTextBox.Text.Remove(CurrentTextBox.Text.Length - 1);
                                CurrentTextBox.Focus();
                                CurrentTextBox.Select(CurrentTextBox.Text.Length, 0);
                            }
                            break;
                        }
                    case "Space":
                        {
                            if (CurrentTextBox.Text.Length != 0)
                            {
                                CurrentTextBox.Text = CurrentTextBox.Text + " ";
                                CurrentTextBox.Focus();
                                CurrentTextBox.Select(CurrentTextBox.Text.Length, 0);
                            }
                            break;
                        }
                    case "Undo":
                        {
                            try
                            {
                                Get_KeyBoard();
                            }
                            catch
                            {
                            }
                            break;
                        }
                    case "Enter":
                        {
                            try
                            {
                                temp();
                            }
                            catch
                            {
                            }
                            break;
                        }
                    default:
                        {
                            if (CurrentTextBox != null)
                            {
                                CurrentTextBox.Text += ((Button)sender).Content.ToString();
                                CurrentTextBox.Focus();
                                CurrentTextBox.Select(CurrentTextBox.Text.Length, 0);
                            }
                            break;
                        }
                }
            }
            else if (CurrentComboBox != null && CurrentPasswordBox==null) 
            {

                switch (((Button)sender).Content.ToString())
                {
                    case "<-BackSpace":
                        {
                            if (CurrentComboBox.Text.Length != 0)
                            {
                                CurrentComboBox.Text = CurrentComboBox.Text.Remove(CurrentComboBox.Text.Length - 1);
                            }
                            break;
                        }
                    case "Space":
                        {
                            if (CurrentComboBox.Text.Length != 0)
                            {
                                CurrentComboBox.Text = CurrentComboBox.Text + " ";
                            }
                            break;
                        }
                    case "Undo":
                        {
                            try
                            {
                                Get_KeyBoard();
                            }
                            catch
                            {
                            }
                            break;
                        }
                    case "Enter":
                        {
                            try
                            {
                                temp();
                            }
                            catch
                            {
                            }
                            break;
                        }
                    default:
                        {
                            if (CurrentComboBox != null)
                            {
                                CurrentComboBox.Text += ((Button)sender).Content.ToString();
                            }
                            break;
                        }
                }
            }

            else
            {
                switch (((Button)sender).Content.ToString())
                {
                    case "<-BackSpace":
                        {
                            if (CurrentPasswordBox.Password.Length != 0)
                            {
                                CurrentPasswordBox.Password = CurrentPasswordBox.Password.Remove(CurrentPasswordBox.Password.Length - 1);
                            }
                            break;
                        }
                    case "Space":
                        {
                            if (CurrentPasswordBox.Password.Length != 0)
                            {
                                CurrentPasswordBox.Password = CurrentPasswordBox.Password + " ";
                            }
                            break;
                        }
                    case "Undo":
                        {
                            try
                            {
                                Get_KeyBoard();
                            }
                            catch
                            {
                            }
                            break;
                        }
                    case "Enter":
                        {
                            try
                            {
                                temp();
                            }
                            catch
                            {
                            }
                            break;
                        }
                    default:
                        {

                            if (CurrentPasswordBox != null)
                            {
                                CurrentPasswordBox.Password += ((Button)sender).Content.ToString();
                            }
                            break;
                        }
                }

            }
        }
    }
}
