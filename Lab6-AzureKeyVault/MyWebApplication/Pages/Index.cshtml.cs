using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyWebApplication.Pages;

public class IndexModel(
    BlobContainerClient containerClient,
    IConfiguration configuration,
    IWebHostEnvironment environment) : PageModel
{
    public string ContainerName => containerClient.Name;

    public string EnvironmentLabel => environment.EnvironmentName;

    public string? KeyVaultUri => configuration["KeyVault:VaultUri"];

    public string KeyVaultBannerMessage =>
        configuration["Lab6:KvDemoBanner"]
        ?? "(V Azure vytvořte secret pojmenovaný Lab6--KvDemoBanner → zobrazí se zde z online Key Vault.)";

    public List<BlobItemViewModel> Blobs { get; private set; } = [];

    public async Task OnGetAsync()
    {
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
        var blobClient = containerClient.GetBlobClient(blobName);
        var download = await blobClient.DownloadStreamingAsync();

        return File(download.Value.Content, download.Value.Details.ContentType ?? "application/octet-stream", blobName);
    }

    public sealed record BlobItemViewModel(string Name, string DownloadUrl);

}
