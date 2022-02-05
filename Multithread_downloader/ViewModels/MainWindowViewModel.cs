using System.Diagnostics;
using Multithread_downloader.Services;
using Multithread_downloader.ViewModels.Base;

namespace Multithread_downloader.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
		private HttpDownloadClient _downloadClient = new HttpDownloadClient();

		public HttpDownloadClient DownloadClient {
			get => _downloadClient;
			set => Set(ref _downloadClient, value);
		}

		public MainWindowViewModel()
		{
			_downloadClient.MultiSocketRequestsMode = false;
			_downloadClient.ThreadDownloadProgressUpdater += (t_id, p) => Debug.Write($"t_id:{t_id} -> {p}%");

			_downloadClient.DownloadThreading(@"https://speedtest.selectel.ru/100MB", 5, "D:\\Temp");
		}
	}
}
