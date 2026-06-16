namespace EventHub.Config;

/// <summary>
/// Centrální konfigurace pro všechny Event Hub projekty.
/// Vyplňte hodnoty zde – změna se projeví ve všech projektech.
/// </summary>
public static class Config
{
    // ===== EVENT HUB =====
    // Azure Portal → Event Hub Namespace → Shared access policies → connection string
    public const string EventHubConnectionString = @"<<INSERT YOUR EVENT HUB CONNECTION STRING>>";
    public const string EventHubName = "lab7-events";

    /// <summary>
    /// Samostatná consumer group pro lokální konzolové čtení. Logic App a jiné služby typicky používají
    /// <c>$Default</c> s epoch receiverem – druhý „non-epoch“ reader na stejné CG + partition spadne.
    /// V portálu: Event Hub → Consumer groups → přidejte např. <c>console-dev</c>.
    /// </summary>
    public const string ConsoleConsumerGroupName = "console-dev";

    // ===== AZURE OPENAI (Foundry) =====
    // ai.azure.com → váš projekt → Deployments → vyberte model → zkopírujte Endpoint a Key
    public const string AzureOpenAiEndpoint = "<<INSERT YOUR AZURE OPENAI ENDPOINT>>"; // https://xxx.openai.azure.com/
    public const string AzureOpenAiApiKey = "<<INSERT YOUR AZURE OPENAI API KEY>>";
    public const string AzureOpenAiDeployment = "gpt-4o-mini"; // název deploymentu ve Foundry
}
