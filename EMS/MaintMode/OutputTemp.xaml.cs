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

namespace EMS
{
	/// <summary>
	/// OutputTemp.xaml 的交互逻辑
	/// </summary>
	public partial class OutputTemp : UserControl
	{
		public OutputTemp()
		{
			this.InitializeComponent();
		}

		private void btn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
            temp();
		}

        # region
        public delegate void OutputClickEventHandler();
        private OutputClickEventHandler temp;
        public event OutputClickEventHandler OutputClick
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
        #endregion

        public string SensorName
        {
            get
            {
                return SensorName;
            }
            set
            {
                btn.Content = value;
            }
        }

        #region ****** ValueProperty ******
        public bool Value
        {
            get
            {
                return (bool)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(bool), typeof(OutputTemp), new UIPropertyMetadata(false, new PropertyChangedCallback(ValueChange)));

        private static void ValueChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OutputTemp x = (OutputTemp)sender;

            if ((bool)e.NewValue)
            {
                x.ep.Fill = StaticRes.ColorBrushes.Linear_Green;
            }
            else
            {
                x.ep.Fill = StaticRes.ColorBrushes.Linear_Silver;
            }
        }
        #endregion

        #region  ****** DescriptionProperty ******
        public string Description
        {
            get
            {
                return (string)GetValue(DescriptionProperty);
            }
            set
            {
                SetValue(DescriptionProperty, value);

            }
        }


        public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register("Description", typeof(string), typeof(OutputTemp), new UIPropertyMetadata("", new PropertyChangedCallback(DescriptionChange)));

        private static void DescriptionChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OutputTemp x = (OutputTemp)sender;
            x.txt.Text = e.NewValue.ToString();
        }

        #endregion
	}
}