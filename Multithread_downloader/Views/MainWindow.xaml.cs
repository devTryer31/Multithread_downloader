using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Multithread_downloader.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void UriTextBoxLostFocus(object sender, RoutedEventArgs e)
		{
			var tb = (sender as TextBox);
			tb!.Text = @"http://example.com/example.txt";
			tb.Foreground = Brushes.DarkGray;
		}

		private void UriTextBoxGotFocus(object sender, RoutedEventArgs e)
		{
			var tb = (sender as TextBox);
			tb!.Text = string.Empty;
			tb.Foreground = Brushes.Black;
		}
	}
}
