using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Multithread_downloader.Services
{
	public class HttpDownloadClient
	{
		private readonly HttpClient _httpClient = new();

		private const int __range_request_got_progress = 12;

		public delegate void UpdateThreadDownloadProgress(int thread_id, int new_progress);

		public event UpdateThreadDownloadProgress ThreadDownloadProgressUpdater;

		public bool MultiSocketRequestsMode { get; set; } = false;

		public (Stream bytes, HttpStatusCode response_code) GetContentRangedPart(string url, long first_byte, long last_byte)
		{
			var client = establishClient();

			var method = new HttpMethod("GET");
			var request = new HttpRequestMessage(method, url) {
				Headers = { Range = new RangeHeaderValue(first_byte, last_byte) }
			};
			var response = client.Send(request, HttpCompletionOption.ResponseHeadersRead);
			return (response.Content.ReadAsStream(), response.StatusCode);
		}

		public string DownloadThreading(string url, uint threads_count, string dest_folder, string file_name = null, CancellationToken cancel = default)
		{
			file_name ??= new string(Uri.UnescapeDataString(url).Reverse().TakeWhile(c => c != '/').Reverse().ToArray());

			var response = GetContentSize(url);
			if (isBadRequestCode(response.response_code))
				throw new HttpRequestException("Bad attempt at getting the content size.", null, response.response_code);

			long bytes_len = response.size.Value;
			long part_size = bytes_len / threads_count;
			int current_task_id = 0;

			var tasks = new Task[threads_count];
			byte[] file_bytes = new byte[bytes_len];


			while (current_task_id != threads_count) {
				int task_id = current_task_id;
				tasks[current_task_id++] = Task.Run(
					() =>
					{
						long first_byte = task_id * part_size;
						long last_byte = first_byte + part_size - 1;
						if (bytes_len - last_byte <= part_size)
							last_byte = bytes_len - 1;

						var res = GetContentRangedPart(url, first_byte, last_byte);
						if (isBadRequestCode(res.response_code))
							throw new HttpRequestException("Bad attempt at getting the content ranged part.", null, response.response_code);

						ThreadDownloadProgressUpdater?.Invoke(task_id, __range_request_got_progress);

						long idx = first_byte;
						int b;

						int progress = __range_request_got_progress,
							inc = (int)((last_byte - first_byte) / (100 - progress) + 1);

						while ((b = res.bytes.ReadByte()) != -1) {
							file_bytes[idx++] = (byte)b;
							if (idx % inc == 0)
								ThreadDownloadProgressUpdater?.Invoke(task_id, ++progress);
						}

						if (progress > 100) //Progress coverage.
							ThreadDownloadProgressUpdater?.Invoke(task_id, 100);
					}, cancel);
			}

			foreach (Task task in tasks)
				task.Wait(cancel);

			string result_file_path = $"{dest_folder}\\{file_name}";

			File.WriteAllBytes(result_file_path, file_bytes);

			return result_file_path;
		}

		public (long? size, HttpStatusCode response_code) GetContentSize(string url)
		{
			var client = establishClient();

			var method = new HttpMethod("HEAD");
			var request = new HttpRequestMessage(method, url);
			var response = client.Send(request, HttpCompletionOption.ResponseHeadersRead);
			var size = response.Content.Headers.ContentLength;
			return (size, response.StatusCode);
		}

		private HttpClient establishClient() =>
			MultiSocketRequestsMode ? new HttpClient() : _httpClient;

		private static bool isBadRequestCode(HttpStatusCode code) =>
			(int)code < 200 || (int)code >= 300;
	}
}
