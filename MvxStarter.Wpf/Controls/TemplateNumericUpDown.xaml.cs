using MvvmCross.Platforms.Wpf.Views;
using System.Windows;
using System.Windows.Controls;

namespace MvxStarter.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for TemplateNumericUpDown.xaml
    /// </summary>
    public partial class TemplateNumericUpDown : MvxWpfView
    {
        public TemplateNumericUpDown()
        {
            InitializeComponent();
        }

        private void TxtNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNum == null)
            {
                return;
            }

            if (!int.TryParse(txtNum.Text, out var numValue))
                txtNum.Text = numValue.ToString();
        }
    }
}