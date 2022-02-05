using Multithread_downloader.Services;
using System;

namespace TestConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			HttpDownloadClient _downloadClient = new HttpDownloadClient();
			_downloadClient.MultiSocketRequestsMode = true;
			_downloadClient.ThreadDownloadProgressUpdater += (t_id, p) => Console.WriteLine($"t_id:{t_id} -> {p}%");

			Console.WriteLine(
					_downloadClient.DownloadThreading(@"https://speedtest.selectel.ru/100MB", 5, "D:\\Temp")
				);
		}
	}
}
