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

namespace EMS.Report
{
    /// <summary>
    /// SlotTemp.xaml 的交互逻辑
    /// </summary>
    public partial class SlotTemp : UserControl
    {
        public SlotTemp()
        {
            InitializeComponent();
        }
		
		public string SlotName
        {
            get
            {
                return SlotName;
            }
            set
            {
                lb_slotPosition.Content = value;
            }
        }

        public string PartID
        {
            get
            {
                return PartID;
            }
            set
            {
                lb_partID.Content = value;
            }
        }

        public string Sapcode
        {
            get
            {
                return Sapcode;
            }
            set
            {
                lb_sap.Content = value;
            }
        }

        #region ****** ValueProperty ******
        public string Value
        {
            get
            {
                return (string)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(string), typeof(SlotTemp), new UIPropertyMetadata("", new PropertyChangedCallback(ValueChange)));

        private static void ValueChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            SlotTemp x = (SlotTemp)sender;
            switch ((string)e.NewValue)
            {
                case "0":
                    x.label.Background = StaticRes.ColorBrushes.Linear_RainySky;
                    break;
                case "1":
                    x.label.Background = StaticRes.ColorBrushes.Linear_Blue;
                    break;
                case "2":
                    x.label.Background = StaticRes.ColorBrushes.Linear_Green;
                    break;
                case "3":
                    x.label.Background = StaticRes.ColorBrushes.Linear_Red;
                    break;
            }
        }
        #endregion
    }
}
