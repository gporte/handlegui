using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandleGui.Messages;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace HandleGui.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
	{
		#region ExecutablePath
		private string _executablePath;

		public string ExecutablePath {
			get { return this._executablePath; }
			set {
				if (this._executablePath != value) {
					this._executablePath = value;
					this.RaisePropertyChanged(() => this.ExecutablePath);
				}
			}
		}
		#endregion

		#region FilePath
		private string _filePath;

		public string FilePath {
			get { return this._filePath; }
			set {
				if (this._filePath != value) {
					this._filePath = value;
					this.RaisePropertyChanged(() => this.FilePath);
				}
			}
		}
		#endregion

		#region OutPutText
		private string _outputText;

		public string OutPutText {
			get { return this._outputText; }
			set {
				if (this._outputText != value) {
					this._outputText = value;
					this.RaisePropertyChanged(() => this.OutPutText);
				}
			}
		}
		#endregion

		/// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
			this.ExecutablePath = Properties.Settings.Default.ExecPath;
			
			// création des commandes
			this.CreateBrowseCommand();
			this.CreateChangeExecPathCommand();
			this.CreateHandleCommand();
		}

		#region BrowseCommand
		public ICommand BrowseCommand { get; set; }

		private void CreateBrowseCommand() {
			this.BrowseCommand = new RelayCommand<string>(this.ExecuteBrowseCommand, this.CanExecuteBrowseCommand);
		}

		private bool CanExecuteBrowseCommand(string param) {
			return true;
		}

		private void ExecuteBrowseCommand(string param) {
			MessengerInstance.Send<NMAOpenFileDialog>(
				new NMAOpenFileDialog(param, this.ExecuteBrowseCommandCallBack)
			);
		}

		private void ExecuteBrowseCommandCallBack(string fileName) {
			this.FilePath = fileName;
		}
		#endregion

		#region ChangeExecPathCommand
		public ICommand ChangeExecPathCommand { get; set; }

		private void CreateChangeExecPathCommand() {
			this.ChangeExecPathCommand = new RelayCommand<string>(this.ExecuteChangeExecPathCommand, this.CanExecuteChangeExecPathCommand);
		}

		private bool CanExecuteChangeExecPathCommand(string param) {
			return true;
		}

		private void ExecuteChangeExecPathCommand(string param) {
			MessengerInstance.Send<NMAOpenFileDialog>(
				new NMAOpenFileDialog(param, this.ExecuteChangeExecPathCommandCallBack)
			);
		}

		private void ExecuteChangeExecPathCommandCallBack(string fileName) {
			this.ExecutablePath = fileName;
			Properties.Settings.Default.ExecPath = fileName;
			Properties.Settings.Default.Save();
		}
		#endregion

		#region HandleCommand
		public ICommand HandleCommand { get; set; }

		private void CreateHandleCommand() {
			this.HandleCommand = new RelayCommand(this.ExecuteHandleCommand, this.CanExecuteHandleCommand);
		}

		private bool CanExecuteHandleCommand() {
			return File.Exists(this.FilePath) && File.Exists(this.ExecutablePath);
		}

		private void ExecuteHandleCommand() {
			this.CallHandle();
		}
		#endregion

		

		private void CallHandle() {
			var param = "\"" + this.FilePath + "\"";
			var process = new Process();
			var startInfos = new ProcessStartInfo(this.ExecutablePath, param);
			startInfos.UseShellExecute = false;
			startInfos.RedirectStandardOutput = true;

			process.StartInfo = startInfos;
			process.Start();

			this.OutPutText = process.StandardOutput.ReadToEnd();
			process.WaitForExit();
		}
	}
}