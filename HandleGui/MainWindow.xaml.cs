using GalaSoft.MvvmLight.Messaging;
using HandleGui.Messages;
using Microsoft.Win32;
using System.Windows;

namespace HandleGui
{
	/// <summary>
	/// Logique d'interaction pour MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow() {
			InitializeComponent();

			Messenger.Default.Register<NMAOpenFileDialog>(this, this.ShowOpenFileDialog);
		}

		#region messages
		private void ShowOpenFileDialog(NMAOpenFileDialog msg) {
			var dialog = new OpenFileDialog();
			dialog.RestoreDirectory = true;

			if (dialog.ShowDialog() == true) {
				msg.Execute(dialog.FileName);
			}
		}
		#endregion
	}
}
