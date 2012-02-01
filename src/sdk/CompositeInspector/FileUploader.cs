using System;
using System.IO;
using Euclid.Common.Storage;
using Euclid.Common.Storage.Binary;
using Nancy;

namespace CompositeInspector
{
	public class FileUploader
	{
		private readonly IBlobStorage _blobStorage;

		public FileUploader(IBlobStorage blobStorage)
		{
			_blobStorage = blobStorage;
		}

		public void UploadFiles(NancyContext context)
		{
			foreach (var file in context.Request.Files)
			{
				var key = file.Key + "Url";
				
				Uri blobUrl;
				using (var ms = new MemoryStream())
				{
					file.Value.CopyTo(ms);
					var blob = new Blob
					           	{
					           		Content = ms.ToArray(), 
					           		ContentType = file.ContentType
					           	};
					blobUrl = _blobStorage.Put(blob, file.Name);
				}

				context.Request.Form[key] = blobUrl.AbsoluteUri;
			}
		}
	}
}