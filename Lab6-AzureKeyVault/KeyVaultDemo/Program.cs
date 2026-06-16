using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Secrets;
using System.Text;

var vaultUriRaw = Environment.GetEnvironmentVariable("KEY_VAULT_URI");
var secretName = Environment.GetEnvironmentVariable("KV_SECRET_NAME") ?? "MySecret";
var keyName = Environment.GetEnvironmentVariable("KV_KEY_NAME") ?? "MyRsaKey";

if (string.IsNullOrWhiteSpace(vaultUriRaw))
{
    Console.WriteLine("""
        Set KEY_VAULT_URI to your vault, e.g.:
          export KEY_VAULT_URI=https://your-vault-name.vault.azure.net/
        Optional overrides:
          export KV_SECRET_NAME=MySecret
          export KV_KEY_NAME=MyRsaKey
        For local runs, authenticate first: az login
        """);
    return;
}

if (!vaultUriRaw.EndsWith('/'))
{
    vaultUriRaw += "/";
}

var keyVaultUri = new Uri(vaultUriRaw);

TokenCredential credential = new DefaultAzureCredential();

var secretClient = new SecretClient(keyVaultUri, credential);
var secret = await secretClient.GetSecretAsync(secretName);
Console.WriteLine($"Secret '{secretName}': {secret.Value.Value}");

var keyClient = new KeyClient(keyVaultUri, credential);

try
{
    var cryptographyClient = keyClient.GetCryptographyClient(keyName);
    EncryptResult encryptResult =
        cryptographyClient.Encrypt(EncryptionAlgorithm.RsaOaep, Encoding.UTF8.GetBytes("Hello, MFF!"));

    Console.Write("Ciphertext (hex): ");
    foreach (byte b in encryptResult.Ciphertext)
    {
        Console.Write("{0:X2}", b);
    }

    Console.WriteLine();

    DecryptResult decryptResult =
        cryptographyClient.Decrypt(EncryptionAlgorithm.RsaOaep, encryptResult.Ciphertext);
    Console.WriteLine($"Decrypted: {Encoding.UTF8.GetString(decryptResult.Plaintext)}");
}
catch (Azure.RequestFailedException ex) when (ex.Status == 404)
{
    Console.WriteLine($"Key '{keyName}' not found or not an RSA key. Create an RSA key in the vault named '{keyName}' (or set KV_KEY_NAME).");
}
