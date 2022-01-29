using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Multithread_downloader.ViewModels.Base
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop_name = null)
		{
			if (PropertyChanged != null)
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop_name));
		}

		public bool Set<T>(ref T source, T value, [CallerMemberName] string prop_name = null)
		{
			if (object.Equals(source, value))
				return false;

			source = value;
			OnPropertyChanged(prop_name);
			return true;
		}
	}
}
