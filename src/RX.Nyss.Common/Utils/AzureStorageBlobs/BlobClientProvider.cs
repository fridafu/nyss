using System;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace RX.Nyss.Common.Utils.AzureStorageBlobs;

public class BlobClientProvider
{
    private readonly string _blobContainerName;
    private readonly BlobServiceClient _blobServiceClient;

    public BlobClientProvider(BlobServiceClient blobServiceClient, string blobContainerName)
    {
        _blobServiceClient = blobServiceClient;
        _blobContainerName = blobContainerName;
    }

    public async Task<BlobClient> GetBlobClient(string blobName)
    {
        if (string.IsNullOrWhiteSpace(_blobContainerName) ||
            string.IsNullOrWhiteSpace(blobName))
        {
            throw new ArgumentException("The configuration of blob is invalid.");
        }

        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);

        if (!await blobContainerClient.ExistsAsync())
        {
            throw new InvalidOperationException("Blob container does not exist.");
        }

        return blobContainerClient.GetBlobClient(blobName);
    }
}
