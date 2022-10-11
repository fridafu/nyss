using System;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using NSubstitute;
using RX.Nyss.Common.Utils.AzureStorageBlobs;
using Xunit;

namespace RX.Nyss.Common.Tests.Utils.AzureStorageBlobs;

public class BlobClientProviderTests
{

    private BlobServiceClient _blobServiceClientMock;
    private const string _testBlobName = "fooBlob";
    private const string _testBlobContainerName = "fooBlobContainer";

    private BlobClientProvider _getBlobClientProvider(string blobContainerName) {
        _blobServiceClientMock = Substitute.For<BlobServiceClient>();
        return new BlobClientProvider(_blobServiceClientMock, blobContainerName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("")]
    public async void GetBlobClient_WhenBlobContainerNameIsInvalid_ThrowsError(string blobContainerName)
    {
        var exception = await Assert.ThrowsAsync<System.ArgumentException>(async () =>
        {
            await _getBlobClientProvider(blobContainerName).GetBlobClient(_testBlobName);
        });
        Assert.Equal("The configuration of blob is invalid.", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("")]
    public async void GetBlobClient_WhenBlobNameIsInvalid_ThrowsError(string blobName)
    {
        var exception = await Assert.ThrowsAsync<System.ArgumentException>(async () =>
        {
            await _getBlobClientProvider(_testBlobContainerName).GetBlobClient(blobName);
        });
        Assert.Equal("The configuration of blob is invalid.", exception.Message);
    }

    [Fact]
    public async void GetBlobClient_WhenBlobContainerDoesNotExist_ThrowsError()
    {
        var blobContainerClientMock = Substitute.For<BlobContainerClient>();

        blobContainerClientMock
            .ExistsAsync()
            .Returns(Task.FromResult(
                Response.FromValue<bool>(false, Substitute.For<Response>()))
            );

        var blobClientProvider = _getBlobClientProvider(_testBlobContainerName);

        _blobServiceClientMock.GetBlobContainerClient(_testBlobContainerName).Returns(blobContainerClientMock);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
        {
            await blobClientProvider.GetBlobClient(_testBlobName);
        });

        Assert.Equal("Blob container does not exist.", exception.Message);
    }

    [Fact]
    public async void GetBlobClient_ReturnsExpected()
    {
        var blobContainerClientMock = Substitute.For<BlobContainerClient>();

        blobContainerClientMock
            .ExistsAsync()
            .Returns(Task.FromResult(
                Response.FromValue<bool>(true, Substitute.For<Response>()))
            );

        var blobClientMock = Substitute.For<BlobClient>();

        blobContainerClientMock
            .GetBlobClient(_testBlobName)
            .Returns(blobClientMock);

        var blobClientProvider = _getBlobClientProvider(_testBlobContainerName);

        _blobServiceClientMock.GetBlobContainerClient(_testBlobContainerName).Returns(blobContainerClientMock);

        var blobClient = await blobClientProvider.GetBlobClient(_testBlobName);

        Assert.Same(blobClient, blobClientMock);
    }
}
