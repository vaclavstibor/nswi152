using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApplication.Pages;

public class IndexModel : PageModel
{
    private readonly BlobServiceClient blobServiceClient;
    private readonly IConfiguration configuration;

    public IndexModel(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        this.blobServiceClient = blobServiceClient;
        this.configuration = configuration;
    }

    public string ContainerName { get; private set; } = string.Empty;

    public List<BlobItemViewModel> Blobs { get; private set; } = [];

    public async Task OnGetAsync()
    {
        ContainerName = configuration["BlobStorage:ContainerName"]
            ?? throw new InvalidOperationException("Missing configuration 'BlobStorage:ContainerName'.");

        var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        var items = new List<BlobItemViewModel>();

        await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
        {
            items.Add(new BlobItemViewModel(
                blobItem.Name,
                Url?.Page("./Index", pageHandler: "Download", values: new { blobName = blobItem.Name }) ?? "#"));
        }

        Blobs = items;
    }

    public async Task<IActionResult> OnGetDownloadAsync(string blobName)
    {
        ContainerName = configuration["BlobStorage:ContainerName"]
            ?? throw new InvalidOperationException("Missing configuration 'BlobStorage:ContainerName'.");

        var containerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
        var blobClient = containerClient.GetBlobClient(blobName);
        var download = await blobClient.DownloadStreamingAsync();

        return File(download.Value.Content, download.Value.Details.ContentType ?? "application/octet-stream", blobName);
    }

    public sealed record BlobItemViewModel(string Name, string DownloadUrl)
    {

    }
}
