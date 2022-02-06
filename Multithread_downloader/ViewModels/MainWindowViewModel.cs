using System.Diagnostics;
using Multithread_downloader.Services;
using Multithread_downloader.ViewModels.Base;

namespace Multithread_downloader.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		private const ushort _Max_threads_count = 8;


		#region ViewModelProperties

		//private HttpDownloadClient _downloadClient = new();

		//public HttpDownloadClient DownloadClient {
		//	get => _downloadClient;
		//	set => Set(ref _downloadClient, value);
		//}

		public int[] ThreadsProgress { get; } = new int[_Max_threads_count]
		{
			45,78,-9,1,4,5,100,95
		};

		#endregion

		public MainWindowViewModel()
		{
			//_downloadClient.MultiSocketRequestsMode = false;
			//_downloadClient.ThreadDownloadProgressUpdater += (t_id, p) => Debug.Write($"t_id:{t_id} -> {p}%");

			//_downloadClient.DownloadThreading(@"https://speedtest.selectel.ru/100MB", 5, "D:\\Temp");
		}
	}
}
