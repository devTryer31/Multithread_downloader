using System.IO;
using System.Net;
using System.Threading;

namespace Multithread_downloader.Services.Interfaces
{
	public interface IHttpDownloadClient
	{
		(Stream bytes, HttpStatusCode response_code) GetContentRangedPart(string url, long first_byte, long last_byte);

		/// <summary>
		/// Download a file by http range protocol using multiple threads.
		/// </summary>
		/// <param name="dest_folder">The folder path where file will be downloaded. It can contain a file name.</param>
		/// <param name="file_name">Optional output file name. It will be received from the url if not set.</param>
		/// <returns>The output file path.</returns>
		string DownloadThreading(string url, uint threads_count, string dest_folder, string file_name, CancellationToken cancel = default);

		(long? size, HttpStatusCode response_code) GetContentSize(string url);
	}
}
