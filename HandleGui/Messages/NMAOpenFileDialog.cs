using GalaSoft.MvvmLight.Messaging;
using System;

namespace HandleGui.Messages
{
	public class NMAOpenFileDialog : NotificationMessageAction<string>
	{
		public NMAOpenFileDialog(string notif, Action<string> callback) : base(notif, callback) { }
	}
}
