using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                app.AddEnvironmentVariables();
            })
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
            })
            .Build();

        await host.RunAsync();
    }
}

public class Worker : IHostedService, IDisposable
{
    private PeriodicTimer? timer;
    private readonly IConfiguration configuration;

    public Worker(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task StartAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Worker started...");

        timer = new PeriodicTimer(TimeSpan.FromSeconds(10));

        do
        {
            await ProcessEmailQueueAsync(stoppingToken);
            Console.WriteLine("Waiting for next tick...");
        }
        while (timer != null && await timer.WaitForNextTickAsync(stoppingToken));
    }

    private async Task ProcessEmailQueueAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Checking for new e-mails to be sent...");

        using var conn = new SqlConnection(configuration.GetConnectionString("MyDatabase"));
        await conn.OpenAsync(stoppingToken);

        using var cmd = new SqlCommand("SELECT * FROM EmailQueue WHERE Sent IS NULL", conn);
        using var reader = await cmd.ExecuteReaderAsync(stoppingToken);

        while (await reader.ReadAsync(stoppingToken))
        {
            Console.WriteLine($"Email ID:{reader["ID"]} found...");

            string recipients = reader["Recipient"].ToString() ?? throw new InvalidOperationException("Recipient not provided");
            string? subject = reader["Subject"].ToString();
            string? body = reader["Body"].ToString();
            int mailId = Convert.ToInt32(reader["ID"]);

            try
            {
                await SendMailAsync(recipients, subject, body, mailId, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email ID:{mailId} failed: {ex.Message}");
            }
        }
    }

    private async Task SendMailAsync(string recipients, string? subject, string? body, int mailId, CancellationToken stoppingToken)
    {
        bool disableSending = configuration.GetValue<bool>("MailSettings:DisableSending");

        if (!disableSending)
        {
            using var smtpClient = new SmtpClient
            {
                Host = configuration.GetValue<string>("MailSettings:SmtpHost"),
                Credentials = new NetworkCredential(
                    configuration.GetValue<string>("MailSettings:SmtpUsername"),
                    configuration.GetValue<string>("MailSettings:SmtpPassword"))
            };

            using var message = new MailMessage
            {
                From = new MailAddress(configuration.GetValue<string>("MailSettings:SmtpFrom") ?? string.Empty),
                Subject = subject ?? string.Empty,
                Body = body ?? string.Empty
            };
            message.To.Add(recipients);

            await smtpClient.SendMailAsync(message);
        }
        else
        {
            Console.WriteLine($"Email ID:{mailId} skipped (MailSettings:DisableSending=true).");
        }

        using var conn = new SqlConnection(configuration.GetConnectionString("MyDatabase"));
        await conn.OpenAsync(stoppingToken);
        using var cmd = new SqlCommand("UPDATE EmailQueue SET Sent = GETDATE() WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", mailId);
        await cmd.ExecuteNonQueryAsync(stoppingToken);

        Console.WriteLine($"Email ID:{mailId} sent...");
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        timer?.Dispose();
    }
}
