using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    var vaultUri = builder.Configuration["KeyVault:VaultUri"];
    if (!string.IsNullOrWhiteSpace(vaultUri))
    {
        builder.Configuration.AddAzureKeyVault(new Uri(vaultUri), new DefaultAzureCredential());
    }
}

builder.Services.AddRazorPages();

builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var accountName = configuration["BlobStorage:AccountName"]
        ?? throw new InvalidOperationException("Missing configuration BlobStorage:AccountName.");
    var containerName = configuration["BlobStorage:ContainerName"]
        ?? throw new InvalidOperationException("Missing configuration BlobStorage:ContainerName.");
    var containerUri = $"https://{accountName}.blob.core.windows.net/{containerName}";
    return new BlobContainerClient(new Uri(containerUri), new DefaultAzureCredential());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
