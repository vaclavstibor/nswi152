using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

// Portal → cloudappdev26-speech → Keys and Endpoint
const string subscriptionKey = "x";
const string region = "germanywestcentral"; // Location na Overview blade

if (subscriptionKey.Contains("YOUR SUBSCRIPTION"))
{
    Console.WriteLine("Doplň subscriptionKey v Program.cs (Key 1 z Azure Portal).");
    return;
}

Console.WriteLine("Azure Speech SDK – Lab 8d");
Console.WriteLine("1 = speech-to-text (mikrofon, jedna věta)");
Console.WriteLine("2 = text-to-speech (přehrát ukázkovou větu)");
Console.Write("Volba: ");
var choice = Console.ReadLine()?.Trim();

var config = SpeechConfig.FromSubscription(subscriptionKey, region);

if (choice == "2")
{
    await TextToSpeechAsync(config);
}
else
{
    await SpeechToTextAsync(config);
}

static async Task SpeechToTextAsync(SpeechConfig config)
{
    config.SpeechRecognitionLanguage = "cs-CZ";
    Console.WriteLine();
    Console.WriteLine("Mluv do mikrofonu (cca 5 s)…");

    using var recognizer = new SpeechRecognizer(config);
    var result = await recognizer.RecognizeOnceAsync();

    switch (result.Reason)
    {
        case ResultReason.RecognizedSpeech:
            Console.WriteLine($"Rozpoznáno: {result.Text}");
            break;
        case ResultReason.NoMatch:
            Console.WriteLine("Nic nerozpoznáno (NoMatch).");
            break;
        case ResultReason.Canceled:
            var details = CancellationDetails.FromResult(result);
            Console.WriteLine($"Zrušeno: {details.Reason}");
            if (!string.IsNullOrEmpty(details.ErrorDetails))
                Console.WriteLine(details.ErrorDetails);
            break;
    }
}

static async Task TextToSpeechAsync(SpeechConfig config)
{
    const string text = "Dobrý den. Toto je test Azure Speech služby.";
    config.SpeechSynthesisVoiceName = "cs-CZ-VlastaNeural";

    Console.WriteLine();
    Console.WriteLine($"Přehrávám: {text}");

    using var synthesizer = new SpeechSynthesizer(config);
    var result = await synthesizer.SpeakTextAsync(text);

    if (result.Reason == ResultReason.Canceled)
    {
        var details = SpeechSynthesisCancellationDetails.FromResult(result);
        Console.WriteLine($"TTS selhalo: {details.Reason}");
        if (!string.IsNullOrEmpty(details.ErrorDetails))
            Console.WriteLine(details.ErrorDetails);
    }
    else
    {
        Console.WriteLine("TTS OK (měl jsi slyšet audio z reproduktoru).");
    }
}
