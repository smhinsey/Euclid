using System;
using System.Collections.Generic;
using System.Linq;
using Euclid.Common.Extensions;
using Euclid.Common.Logging;
using Euclid.Common.Storage.Binary;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Euclid.Common.Storage.Azure
{
	public class AzureBlobStorage : IBlobStorage
	{
		private readonly CloudStorageAccount _storageAccount;

		private bool _init;

		private IBlobStorageSettings _settings;

		public AzureBlobStorage(CloudStorageAccount storageAccount)
		{
			_storageAccount = storageAccount;

			_settings = new BlobStorageSettings();
		}

		public void Configure(IBlobStorageSettings settings)
		{
			_settings = settings;
		}

		public void Delete(Uri uri)
		{
			var container = getContainer();

			var target = container.GetBlobReference(uri.ToString());

			target.Delete(
				new BlobRequestOptions { DeleteSnapshotsOption = DeleteSnapshotsOption.IncludeSnapshots, UseFlatBlobListing = true });
		}

		public bool Exists(Uri uri)
		{
			var exists = true;

			var container = getContainer();

			var target = container.GetBlobReference(uri.ToString());

			try
			{
				target.FetchAttributes();
			}
			catch (StorageClientException s)
			{
				exists = false;
			}

			return exists;
		}

		public IBlob Get(Uri uri)
		{
			IBlob blob = null;

			var options = new BlobRequestOptions { BlobListingDetails = BlobListingDetails.Metadata };

			var container = getContainer();
			var target = container.GetBlobReference(uri.ToString());

			try
			{
				target.FetchAttributes();
				blob = new Blob(target.Properties.ContentMD5, target.Properties.ETag)
					{ Content = target.DownloadByteArray(), ContentType = target.Properties.ContentType, };

				foreach (var key in target.Metadata.AllKeys)
				{
					blob.Metdata.Add(new KeyValuePair<string, string>(key, target.Metadata[key]));
				}
			}
			catch (StorageClientException s)
			{
				this.WriteErrorMessage(string.Format("An error occurred retrieving the blob from {0}", uri), s);
				blob = null;
			}

			return blob;
		}

		public Uri Put(IBlob blob, string name)
		{
			var container = getContainer();
			var uri = container.Uri;
			try
			{
				var blobName = string.Format(
					"{2}.{0}.{1}", name, MimeTypes.GetExtensionFromContentType(blob.ContentType), Guid.NewGuid());

				var azureBlob = container.GetBlobReference(blobName);

				azureBlob.Properties.ContentType = blob.ContentType;

				azureBlob.Properties.ContentMD5 = blob.Md5;

				if (blob.Metdata != null)
				{
					blob.Metdata.ToList().ForEach(item => azureBlob.Metadata.Add(item.Key, item.Value));
				}

				azureBlob.UploadByteArray(blob.Content);

				uri = azureBlob.Uri;
			}
			catch (Exception e)
			{
				this.WriteErrorMessage(string.Format("An error occurred putting the blob into azure storage '{0}'", uri), e);
				uri = null;
			}

			return uri;
		}

		private CloudBlobContainer getContainer()
		{
			var blobStorage = _storageAccount.CreateCloudBlobClient();
			var container = blobStorage.GetContainerReference(_settings.ContainerName.Value);

			if (!_init)
			{
				container.CreateIfNotExist();
				container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
				_init = true;
			}

			return container;
		}
	}
}